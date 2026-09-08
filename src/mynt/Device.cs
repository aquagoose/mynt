namespace mynt;

/// <summary>
/// A logical device that can perform graphics operations.
/// </summary>
public abstract class Device : IDisposable
{
    /// <summary>
    /// Gets if this <see cref="Device"/> has been disposed.
    /// </summary>
    public bool IsDisposed { get; protected set; }

    /// <summary>
    /// Dispose of this <see cref="Device"/>.
    /// </summary>
    public abstract void Dispose();
}