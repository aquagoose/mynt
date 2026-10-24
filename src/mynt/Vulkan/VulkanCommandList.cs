using Silk.NET.Vulkan;

namespace mynt.Vulkan;

internal sealed unsafe class VulkanCommandList : CommandList
{
    public override bool IsDisposed { get; protected set; }

    private readonly Vk _vk;
    private readonly VkDevice _device;
    private readonly CommandPool _commandPool;
    
    public VulkanCommandList(Vk vk, VkDevice device, uint queueFamily)
    {
        _vk = vk;
        _device = device;

        CommandPoolCreateInfo poolInfo = new()
        {
            SType = StructureType.CommandPoolCreateInfo,
            QueueFamilyIndex = queueFamily,
            Flags = CommandPoolCreateFlags.ResetCommandBufferBit
        };
        
        Mynt.Log("Creating command pool.");
        _vk.CreateCommandPool(_device, &poolInfo, null, out _commandPool).Check("Create command pool");
    }
    
    public override void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;
        
        Mynt.Log("Destroying command pool.");
        _vk.DestroyCommandPool(_device, _commandPool, null);
    }
}