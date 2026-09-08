namespace mynt;

/// <summary>
/// A surface can be used to create a <see cref="Swapchain"/>, and can be drawn to.
/// </summary>
public abstract class Surface : IDisposable
{
    /// <summary>
    /// Gets if this <see cref="Surface"/> has been disposed.
    /// </summary>
    public bool IsDisposed { get; protected set; }

    /// <summary>
    /// Dispose of this <see cref="Surface"/>.
    /// </summary>
    public abstract void Dispose();
}