namespace mynt;

/// <summary>
/// A surface represents something (often an OS Window) that can be rendered to, through a <see cref="Swapchain"/>.
/// </summary>
public abstract class Surface : IDisposable
{
    /// <summary>
    /// Gets if this <see cref="Surface"/> has been disposed.
    /// </summary>
    public abstract bool IsDisposed { get; protected set; }
    
    /// <summary>
    /// Dispose of this <see cref="Surface"/>.
    /// </summary>
    public abstract void Dispose();
}