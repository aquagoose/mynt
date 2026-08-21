namespace mynt;

/// <summary>
/// The base mynt instance, used to create logical <see cref="Device"/>s and enumerate <see cref="Adapter"/>s.
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
}