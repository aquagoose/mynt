namespace mynt;

/// <summary>
/// A command list accepts commands to be executed later.
/// </summary>
public abstract class CommandList : IDisposable
{
    /// <summary>
    /// Gets if this <see cref="CommandList"/> has been disposed.
    /// </summary>
    public bool IsDisposed { get; protected set; }

    /// <summary>
    /// Begin accepting commands.
    /// </summary>
    /// <param name="reusable">Indicates to the driver that the command list may be reused more than once.
    /// If <see langword="false"/>, the command list <b>cannot</b> be used more than once.</param>
    public abstract void Begin(bool reusable = false);

    /// <summary>
    /// Finish accepting commands and prepare the command list for execution.
    /// </summary>
    public abstract void End();

    /// <summary>
    /// Dispose of this <see cref="CommandList"/>.
    /// </summary>
    public abstract void Dispose();
}