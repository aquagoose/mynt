namespace mynt;

/// <summary>
/// Defines parameters for creating <see cref="Instance"/>s.
/// </summary>
public struct InstanceInfo
{
    /// <summary>
    /// The name of the application.
    /// </summary>
    public string AppName;

    /// <summary>
    /// The <see cref="RenderAPI"/> to use.
    /// </summary>
    public RenderAPI API;

    /// <summary>
    /// Enable debugging and validation layers.
    /// </summary>
    public bool Debug;
}