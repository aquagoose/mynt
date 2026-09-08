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
    /// Enumerates a list of supported <see cref="Adapter"/>s.
    /// </summary>
    public abstract Adapter[] EnumerateAdapters();

    /// <summary>
    /// Create a <see cref="Surface"/>.
    /// </summary>
    /// <param name="info">The <see cref="SurfaceInfo"/> that describes the surface</param>
    public abstract Surface CreateSurface(in SurfaceInfo info);

    /// <summary>
    /// Create a <see cref="Device"/>.
    /// </summary>
    /// <param name="surface">The <see cref="Surface"/> to use when creating the device.</param>
    /// <param name="adapter">The <see cref="Adapter"/> to use, if any. If <see langword="null"/> is provided,
    /// the default adapter will be used.</param>
    public abstract Device CreateDevice(Surface surface, Adapter? adapter = null);

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