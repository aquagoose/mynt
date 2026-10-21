namespace mynt;

/// <summary>
/// An adapter is a graphics device, usually hardware, that can be used to create a logical <see cref="Device"/>.
/// </summary>
public readonly record struct Adapter
{
    public readonly nint Handle;

    public readonly string Name;

    public readonly ulong DedicatedMemory;

    public readonly AdapterType Type;

    public readonly AdapterSupports Supports;

    public Adapter(IntPtr handle, string name, ulong dedicatedMemory, AdapterType type, AdapterSupports supports)
    {
        Handle = handle;
        Name = name;
        DedicatedMemory = dedicatedMemory;
        Type = type;
        Supports = supports;
    }

    public bool Equals(Adapter other)
    {
        return Handle == other.Handle;
    }
}