global using VkDevice = Silk.NET.Vulkan.Device;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Silk.NET.Core;
using Silk.NET.Vulkan;

namespace mynt.Vulkan;

internal sealed unsafe class VulkanDevice : Device
{
    private readonly Vk _vk;
    private readonly VkInstance _instance;
    private readonly PhysicalDevice _physicalDevice;

    public readonly uint GraphicsQueueIndex;
    public readonly Queue GraphicsQueue;
    public readonly uint PresentQueueIndex;
    public readonly Queue PresentQueue;

    public readonly VkDevice Device;

    public VulkanDevice(Vk vk, VkInstance instance, VulkanSurface surface, PhysicalDevice physicalDevice)
    {
        _vk = vk;
        _instance = instance;
        _physicalDevice = physicalDevice;

        uint numQueueFamilies;
        _vk.GetPhysicalDeviceQueueFamilyProperties(_physicalDevice, &numQueueFamilies, null);
        QueueFamilyProperties* queueFamilies = stackalloc QueueFamilyProperties[(int) numQueueFamilies];
        _vk.GetPhysicalDeviceQueueFamilyProperties(_physicalDevice, &numQueueFamilies, queueFamilies);

        uint? graphicsQueueIndex = null;
        uint? presentQueueIndex = null;

        for (uint i = 0; i < numQueueFamilies; i++)
        {
            if ((queueFamilies[i].QueueFlags & QueueFlags.GraphicsBit) != 0)
                graphicsQueueIndex = i;

            surface.KhrSurface.GetPhysicalDeviceSurfaceSupport(_physicalDevice, i, surface.Surface, out Bool32 supported);
            if (supported)
                presentQueueIndex = i;

            if (graphicsQueueIndex.HasValue && presentQueueIndex.HasValue)
                break;
        }

        if (!graphicsQueueIndex.HasValue || !presentQueueIndex.HasValue)
        {
            throw new Exception(
                $"Failed to get device queues: Has queue: Graphics: {graphicsQueueIndex != null}, Present: {presentQueueIndex != null}");
        }

        GraphicsQueueIndex = graphicsQueueIndex.Value;
        PresentQueueIndex = presentQueueIndex.Value;

        HashSet<uint> uniqueQueueFamilies = [GraphicsQueueIndex, PresentQueueIndex];

        DeviceQueueCreateInfo* queueInfos = stackalloc DeviceQueueCreateInfo[uniqueQueueFamilies.Count];
        int queueIndex = 0;
        float priority = 1.0f;
        foreach (uint queue in uniqueQueueFamilies)
        {
            queueInfos[queueIndex++] = new DeviceQueueCreateInfo
            {
                SType = StructureType.DeviceQueueCreateInfo,
                QueueFamilyIndex = queue,
                QueueCount = 1,
                PQueuePriorities = &priority
            };
        }

        PhysicalDeviceFeatures features = new();

        DeviceCreateInfo deviceInfo = new()
        {
            SType = StructureType.DeviceCreateInfo,

            QueueCreateInfoCount = (uint) uniqueQueueFamilies.Count,
            PQueueCreateInfos = queueInfos,

            PEnabledFeatures = &features
        };

        Mynt.Log("Creating device.");
        _vk.CreateDevice(_physicalDevice, &deviceInfo, null, out Device).Check("Create device");

        Mynt.Log("Getting device queues.");
        _vk.GetDeviceQueue(Device, GraphicsQueueIndex, 0, out GraphicsQueue);
        _vk.GetDeviceQueue(Device, PresentQueueIndex, 0, out PresentQueue);
    }

    public override CommandList CreateCommandList()
        => new VulkanCommandList(_vk, Device, GraphicsQueueIndex);

    public override void ExecuteCommandList(CommandList cl)
    {
        VulkanCommandList vulkanCl = (VulkanCommandList) cl;
        Debug.Assert(vulkanCl.CurrentCommandBuffer.Handle != 0,
            "Cannot execute: No commands have been issued to the command list");

        SubmitInfo submitInfo = new()
        {
            SType = StructureType.SubmitInfo,
            CommandBufferCount = 1,
            PCommandBuffers = (CommandBuffer*) Unsafe.AsPointer(ref vulkanCl.CurrentCommandBuffer)
        };

        _vk.QueueSubmit(GraphicsQueue, 1, &submitInfo);
    }

    public override void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;

        _vk.DeviceWaitIdle(Device).Check("Wait for device idle");

        _vk.DestroyDevice(Device, null);
    }
}