using System.Diagnostics;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;

namespace mynt.Vulkan;

internal sealed unsafe class VulkanCommandList : CommandList
{
    public override bool IsDisposed { get; protected set; }

    private readonly Vk _vk;
    private readonly VkDevice _device;
    private readonly CommandPool _commandPool;

    private Queue<CommandBuffer> _availableCommandBuffers;
    private Queue<Fence> _availableFences;
    private List<(CommandBuffer cb, Fence f)> _submittedCommandBuffers;

    public CommandBuffer CurrentCommandBuffer;
    
    public VulkanCommandList(Vk vk, VkDevice device, uint queueFamily)
    {
        _vk = vk;
        _device = device;

        _availableCommandBuffers = [];
        _availableFences = [];
        _submittedCommandBuffers = [];

        CommandPoolCreateInfo poolInfo = new()
        {
            SType = StructureType.CommandPoolCreateInfo,
            QueueFamilyIndex = queueFamily,
            Flags = CommandPoolCreateFlags.ResetCommandBufferBit
        };
        
        Mynt.Log("Creating command pool.");
        _vk.CreateCommandPool(_device, &poolInfo, null, out _commandPool).Check("Create command pool");
    }

    public override void Begin(bool reusable = false)
    {
        Debug.Assert(CurrentCommandBuffer.Handle == 0, "CommandList is already active!");

        if (!_availableCommandBuffers.TryDequeue(out CurrentCommandBuffer))
        {
            CommandBufferAllocateInfo allocInfo = new()
            {
                SType = StructureType.CommandBufferAllocateInfo,
                CommandPool = _commandPool,
                CommandBufferCount = 1,
                Level = CommandBufferLevel.Primary
            };
            
            Mynt.Log("Allocating command buffer.");
            _vk.AllocateCommandBuffers(_device, &allocInfo, out CurrentCommandBuffer).Check("Allocate command buffer");
        }

        CommandBufferBeginInfo beginInfo = new()
        {
            SType = StructureType.CommandBufferBeginInfo,
            Flags = reusable ? 0 : CommandBufferUsageFlags.OneTimeSubmitBit
        };
        
        _vk.BeginCommandBuffer(CurrentCommandBuffer, &beginInfo).Check("Begin command buffer");
    }
    
    public override void End()
    {
        Debug.Assert(CurrentCommandBuffer.Handle != 0, "CommandList is not active!");
        _vk.EndCommandBuffer(CurrentCommandBuffer).Check("End command buffer");
    }

    public override void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;
        
        Mynt.Log("Destroying command pool.");
        _vk.DestroyCommandPool(_device, _commandPool, null);
    }

    public Fence RegisterSubmitted()
    {
        // check the status of each fence in the submitted queue.
        // the fence is signaled when the command buffer has finished executing.
        // once the fence is signaled, return the command buffer and fence to the available queue.
        ReadOnlySpan<(CommandBuffer cb, Fence f)> submitted = CollectionsMarshal.AsSpan(_submittedCommandBuffers);
        int length = submitted.Length;
        for (int i = 0; i < length; i++)
        {
            (CommandBuffer cb, Fence f) = submitted[i];
            Result res = _vk.GetFenceStatus(_device, f);
            if (res != Result.Success)
                continue;

            _vk.ResetFences(_device, 1, &f);
            _availableCommandBuffers.Enqueue(cb);
            _availableFences.Enqueue(f);
            
            // !! this is an unsafe operation !!
            // removing stuff from lists when iterating over a span is invalid behaviour
            // however since a list is not resized when removing items, its pointer is not changed,
            // meaning the internal span ref is still valid.
            _submittedCommandBuffers.RemoveAt(i--);
            length--;
        }

        if (!_availableFences.TryDequeue(out Fence fence))
        {
            FenceCreateInfo fenceInfo = new()
            {
                SType = StructureType.FenceCreateInfo
            };

            Mynt.Log("Creating fence.");
            _vk.CreateFence(_device, &fenceInfo, null, out fence);
        }
        
        _submittedCommandBuffers.Add((CurrentCommandBuffer, fence));
        
        // reset the command buffer back to VK_NULL_HANDLE
        CurrentCommandBuffer = new CommandBuffer();

        return fence;
    }
}