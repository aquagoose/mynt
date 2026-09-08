using Silk.NET.Vulkan;

namespace mynt.Vulkan;

internal sealed unsafe class VulkanCommandList : CommandList
{
    private readonly Vk _vk;
    private readonly VkDevice _device;
    private readonly CommandPool _pool;

    private Queue<CommandBuffer> _availableCommandBuffers;
    private Queue<Fence> _availableFences;
    private List<(CommandBuffer cb, Fence fence)> _submittedCommandBuffers;

    public VulkanCommandList(Vk vk, VkDevice device, uint graphicsFamily)
    {
        _vk = vk;
        _device = device;

        _availableCommandBuffers = [];
        _availableFences = [];
        _submittedCommandBuffers = [];

        CommandPoolCreateInfo poolInfo = new()
        {
            SType = StructureType.CommandPoolCreateInfo,
            QueueFamilyIndex = graphicsFamily,
            Flags = CommandPoolCreateFlags.ResetCommandBufferBit
        };

        Mynt.Log("Creating command pool.");
        _vk.CreateCommandPool(_device, &poolInfo, null, out _pool).Check("Create command pool");
    }

    public override void Dispose()
    {
        _vk.DestroyCommandPool(_device, _pool, null);
    }
}