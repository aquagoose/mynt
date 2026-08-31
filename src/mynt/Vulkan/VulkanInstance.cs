using Silk.NET.Vulkan;

namespace mynt.Vulkan;

internal sealed class VulkanInstance : Instance
{
    public override bool IsDisposed { get; protected set; }

    private readonly Vk _vk;

    public VulkanInstance(ref readonly InstanceInfo info)
    {

    }

    public override Backend Backend => Backend.Vulkan;

    public override void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;
    }
}