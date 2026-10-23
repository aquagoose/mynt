global using VkDevice = Silk.NET.Vulkan.Device;
using Silk.NET.Core;
using Silk.NET.Vulkan;

namespace mynt.Vulkan;

internal sealed unsafe class VulkanDevice : Device
{
    public override bool IsDisposed { get; protected set; }

    private readonly Vk _vk;

    public readonly PhysicalDevice PhysicalDevice;
    public readonly Queues Queues;

    public readonly VkDevice Device;

    public VulkanDevice(Vk vk, PhysicalDevice physicalDevice, VulkanSurface surface)
    {
        _vk = vk;
        PhysicalDevice = physicalDevice;

        uint numQueueFamilies;
        _vk.GetPhysicalDeviceQueueFamilyProperties(PhysicalDevice, &numQueueFamilies, null);
        QueueFamilyProperties* queueFamilies = stackalloc QueueFamilyProperties[(int) numQueueFamilies];
        _vk.GetPhysicalDeviceQueueFamilyProperties(PhysicalDevice, &numQueueFamilies, queueFamilies);

        uint? graphicsFamily = null;
        uint? presentFamily = null;
        uint? computeFamily = null;

        for (uint i = 0; i < numQueueFamilies; i++)
        {
            if ((queueFamilies[i].QueueFlags & QueueFlags.GraphicsBit) != 0)
                graphicsFamily = i;
            if ((queueFamilies[i].QueueFlags & QueueFlags.ComputeBit) != 0)
                computeFamily = i;
            
            surface.KhrSurface.GetPhysicalDeviceSurfaceSupport(PhysicalDevice, i, surface.Surface, out Bool32 supported);
            if (supported)
                presentFamily = i;

            if (graphicsFamily.HasValue && presentFamily.HasValue && computeFamily.HasValue)
                break;
        }

        if (graphicsFamily == null || presentFamily == null || computeFamily == null)
        {
            throw new Exception(
                $"One or more required queues were missing! Found Graphics: {graphicsFamily.HasValue}, Present: {presentFamily.HasValue}, Compute: {computeFamily.HasValue}");
        }

        Queues.GraphicsIndex = graphicsFamily.Value;
        Queues.PresentIndex = presentFamily.Value;
        Queues.ComputeIndex = computeFamily.Value;

        HashSet<uint> uniqueQueues = Queues.UniqueQueueIndices;
        DeviceQueueCreateInfo* queueInfos = stackalloc DeviceQueueCreateInfo[uniqueQueues.Count];

        uint queueInfoIndex = 0;
        float queuePriority = 1.0f;
        foreach (uint index in uniqueQueues)
        {
            queueInfos[queueInfoIndex++] = new DeviceQueueCreateInfo
            {
                SType = StructureType.DeviceQueueCreateInfo,
                QueueCount = 1,
                QueueFamilyIndex = index,
                PQueuePriorities = &queuePriority
            };
        }

        PhysicalDeviceFeatures features = new();

        DeviceCreateInfo deviceInfo = new()
        {
            SType = StructureType.DeviceCreateInfo,

            QueueCreateInfoCount = queueInfoIndex, // the index also doubles as a length
            PQueueCreateInfos = queueInfos,

            PEnabledFeatures = &features
        };
        
        Mynt.Log("Creating device.");
        _vk.CreateDevice(PhysicalDevice, &deviceInfo, null, out Device).Check("Create device");
    }
    
    public override void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;
        
        Mynt.Log("Destroying device.");
        _vk.DestroyDevice(Device, null);
    }
}