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
    /// Queries a list of supported <see cref="Adapter"/>s.
    /// </summary>
    /// <returns>A list of supported <see cref="Adapter"/>s.</returns>
    public abstract Adapter[] QueryAdapters();

    /// <summary>
    /// Create a <see cref="Surface"/>.
    /// </summary>
    /// <param name="info">The <see cref="SurfaceInfo"/> describing surface creation.</param>
    public abstract Surface CreateSurface(in SurfaceInfo info);

    /// <summary>
    /// Create a <see cref="Device"/>.
    /// </summary>
    /// <param name="surface">The <see cref="Surface"/> to use while creating the device.</param>
    /// <param name="adapter">An optional <see cref="Adapter"/> to create the device for.</param>
    public abstract Device CreateDevice(Surface surface, Adapter? adapter = null);
    
    /// <summary>
    /// Dispose of this <see cref="Instance"/>.
    /// </summary>
    public abstract void Dispose();

    /// <summary>
    /// Create an <see cref="Instance"/>.
    /// </summary>
    /// <param name="info">The <see cref="InstanceInfo"/> describing instance creation.</param>
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