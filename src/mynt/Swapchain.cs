namespace mynt;

/// <summary>
/// A swapchain contains a series of <see cref="Texture"/>s that can be drawn to.
/// </summary>
public abstract class Swapchain : IDisposable
{
    /// <summary>
    /// Gets if this <see cref="Swapchain"/> has been disposed.
    /// </summary>
    public bool IsDisposed { get; protected set; }

    /// <summary>
    /// Dispose of this <see cref="Swapchain"/>.
    /// </summary>
    public abstract void Dispose();
}