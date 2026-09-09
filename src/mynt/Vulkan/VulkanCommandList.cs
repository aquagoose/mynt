using System.Diagnostics;
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

    public CommandBuffer CurrentCommandBuffer;

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

    public override void Begin(bool reusable = false)
    {
        Debug.Assert(CurrentCommandBuffer.Handle == 0,
            "Cannot begin: The command list has already had commands issued to it. You must either Execute or Reset the command list.");
        CurrentCommandBuffer = GetOrCreateCommandBuffer();

        CommandBufferBeginInfo beginInfo = new()
        {
            SType = StructureType.CommandBufferBeginInfo,
            Flags = reusable ? 0 : CommandBufferUsageFlags.OneTimeSubmitBit
        };

        _vk.BeginCommandBuffer(CurrentCommandBuffer, &beginInfo).Check("Begin command buffer");
    }

    public override void End()
    {
        Debug.Assert(CurrentCommandBuffer.Handle != 0, "Cannot end: The command list is not currently active.");
        _vk.EndCommandBuffer(CurrentCommandBuffer).Check("End command buffer");
    }

    public CommandBuffer GetOrCreateCommandBuffer()
    {
        if (_availableCommandBuffers.TryDequeue(out CommandBuffer cb))
            return cb;

        CommandBufferAllocateInfo allocInfo = new()
        {
            SType = StructureType.CommandBufferAllocateInfo,
            CommandPool = _pool,
            CommandBufferCount = 1,
            Level = CommandBufferLevel.Primary
        };

        Mynt.Log("Allocating command buffer.");
        _vk.AllocateCommandBuffers(_device, &allocInfo, out cb).Check("Allocate command buffer");

        return cb;
    }

    public Fence GetOrCreateFence()
    {
        if (_availableFences.TryDequeue(out Fence fence))
            return fence;

        FenceCreateInfo fenceInfo = new()
        {
            SType = StructureType.FenceCreateInfo
        };

        Mynt.Log("Creating fence.");
        _vk.CreateFence(_device, &fenceInfo, null, out fence).Check("Create fence");

        return fence;
    }

    public void FinishCommandBuffer(Fence fence)
    {
        _submittedCommandBuffers.Add((CurrentCommandBuffer, fence));
        CurrentCommandBuffer = new CommandBuffer(); // reset the command buffer back to "null"

        for (int i = 0; i < _submittedCommandBuffers.Count; i++)
        {
            if (_vk.GetFenceStatus(_device, _submittedCommandBuffers[i].fence) != Result.Success)
                continue;

            _availableCommandBuffers.Enqueue(_submittedCommandBuffers[i].cb);
            _availableFences.Enqueue(_submittedCommandBuffers[i].fence);

            _submittedCommandBuffers.RemoveAt(i--);
        }
    }

    public override void Dispose()
    {
        _vk.DestroyCommandPool(_device, _pool, null);
    }
}