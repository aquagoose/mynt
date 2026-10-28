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
    /// Queue a <see cref="CommandList"/> for execution.
    /// </summary>
    /// <param name="cl">The <see cref="CommandList"/> to execute.</param>
    /// <remarks>This is an async operation, and the command list may not be executed immediately.</remarks>
    public abstract void ExecuteCommandList(CommandList cl);
    
    /// <summary>
    /// Dispose of this <see cref="Device"/>.
    /// </summary>
    public abstract void Dispose();
}