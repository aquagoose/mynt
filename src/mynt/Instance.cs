using mynt.Vulkan;

namespace mynt;

/// <summary>
/// The base instance for a mynt context.
/// </summary>
public abstract class Instance : IDisposable
{
    /// <summary>
    /// Gets if this <see cref="Instance"/> is disposed.
    /// </summary>
    public abstract bool IsDisposed { get; protected set; }

    /// <summary>
    /// Gets this instance's <see cref="mynt.Backend"/>.
    /// </summary>
    public abstract Backend Backend { get; }

    /// <summary>
    /// Dispose of this <see cref="Instance"/>.
    /// </summary>
    public abstract void Dispose();

    /// <summary>
    /// Create an <see cref="Instance"/>.
    /// </summary>
    /// <param name="info">The <see cref="InstanceInfo"/> used on instance creation.</param>
    public static Instance Create(in InstanceInfo info)
    {
        return new VulkanInstance(in info);
    }
}