namespace mynt;

/// <summary>
/// Defines how a <see cref="Swapchain"/> will be created.
/// </summary>
public struct SwapchainInfo
{
    /// <summary>
    /// The <see cref="mynt.Surface"/> that the swapchain will be attached to.
    /// </summary>
    public Surface Surface;

    /// <summary>
    /// The surface format to assign to the swapchain.
    /// </summary>
    public Format Format;

    /// <summary>
    /// The size, in pixels, of the swapchain.
    /// </summary>
    public Size2D Size;

    public
}