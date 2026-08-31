namespace mynt;

public struct InstanceInfo(string appName, bool debug = false, Backend backend = Backend.Unknown)
{
    public string AppName = appName;

    public bool Debug = debug;

    public Backend Backend = backend;
}