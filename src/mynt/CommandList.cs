namespace mynt;

/// <summary>
/// Records and stores commands to be executed by a <see cref="Device"/>.
/// </summary>
public abstract class CommandList : IDisposable
{
    /// <summary>
    /// Gets if this <see cref="CommandList"/> is disposed.
    /// </summary>
    public abstract bool IsDisposed { get; protected set; }

    /// <summary>
    /// Begin command list recording.
    /// </summary>
    /// <param name="reusable">By default, command lists are not reusable when executed.
    /// If <see langword="true"/>, the command list will be reusable.</param>
    public abstract void Begin(bool reusable = false);

    /// <summary>
    /// End command list recording and ready for execution.
    /// </summary>
    public abstract void End();
    
    /// <summary>
    /// Dispose of this <see cref="CommandList"/>.
    /// </summary>
    public abstract void Dispose();
}