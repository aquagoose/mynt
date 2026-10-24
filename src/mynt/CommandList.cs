namespace mynt;

/// <summary>
/// Records and stores commands to be executed by a <see cref="Device"/> later.
/// </summary>
public abstract class CommandList : IDisposable
{
    /// <summary>
    /// Gets if this <see cref="CommandList"/> is disposed.
    /// </summary>
    public abstract bool IsDisposed { get; protected set; }
    
    /// <summary>
    /// Dispose of this <see cref="CommandList"/>.
    /// </summary>
    public abstract void Dispose();
}