namespace mynt;

/// <summary>
/// Defines parameters for creating <see cref="Instance"/>s.
/// </summary>
public struct InstanceInfo(string appName, RenderAPI api = RenderAPI.Unknown, bool debug = false)
{
    /// <summary>
    /// The name of the application.
    /// </summary>
    public string AppName = appName;

    /// <summary>
    /// The <see cref="RenderAPI"/> to use.
    /// </summary>
    public RenderAPI API = api;

    /// <summary>
    /// Enable debugging and validation layers.
    /// </summary>
    public bool Debug = debug;
}