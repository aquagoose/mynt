using mynt.Vulkan;

namespace mynt;

/// <summary>
/// Instances are used to enumerate <see cref="Adapter"/>s and create <see cref="Device"/>s.
/// </summary>
public abstract class Instance : IDisposable
{
    /// <summary>
    /// Gets if this <see cref="Instance"/> has been disposed.
    /// </summary>
    public abstract bool IsDisposed { get; protected set; }
    
    /// <summary>
    /// Dispose of this <see cref="Instance"/>.
    /// </summary>
    public abstract void Dispose();

    /// <summary>
    /// Create an <see cref="Instance"/>.
    /// </summary>
    /// <param name="info">The <see cref="InstanceInfo"/> describing instance creation.</param>
    /// <returns></returns>
    public static Instance CreateInstance(in InstanceInfo info)
    {
        RenderAPI api = info.API;
        if (api == RenderAPI.Unknown)
            api = RenderAPI.Vulkan;

        return api switch
        {
            RenderAPI.Vulkan => new VulkanInstance(in info),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}