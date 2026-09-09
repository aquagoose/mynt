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

    public abstract Swapchain CreateSwapchain();

    /// <summary>
    /// Create a <see cref="CommandList"/>.
    /// </summary>
    public abstract CommandList CreateCommandList();

    /// <summary>
    /// Submit a <see cref="CommandList"/> to the queue to be executed.
    /// </summary>
    /// <param name="cl">The command list to execute.</param>
    /// <remarks>This is an asynchronous operation and the command list MAY NOT be executed immediately.
    /// Previously submitted command lists will be executed first.</remarks>
    public abstract void ExecuteCommandList(CommandList cl);

    /// <summary>
    /// Dispose of this <see cref="Device"/>.
    /// </summary>
    public abstract void Dispose();
}