using Silk.NET.Vulkan;

namespace mynt.Vulkan;

internal struct Queues
{
    public uint GraphicsIndex;
    public uint PresentIndex;
    public uint ComputeIndex;
    
    public Queue Graphics;
    public Queue Present;
    public Queue Compute;

    public HashSet<uint> UniqueQueueIndices => [GraphicsIndex, PresentIndex, ComputeIndex];
}