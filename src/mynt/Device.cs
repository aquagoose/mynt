namespace mynt;

/// <summary>
/// A logical device connected to an <see cref="Adapter"/>.
/// </summary>
public abstract class Device : IDisposable
{
    /// <summary>
    /// Gets if this <see cref="Device"/> has been disposed.
    /// </summary>
    public abstract bool IsDisposed { get; protected set; }

    /// <summary>
    /// Create a <see cref="CommandList"/>.
    /// </summary>
    public abstract CommandList CreateCommandList();
    
    /// <summary>
    /// Dispose of this <see cref="Device"/>.
    /// </summary>
    public abstract void Dispose();
}