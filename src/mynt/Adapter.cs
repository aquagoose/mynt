namespace mynt;

/// <summary>
/// An adapter is a device, typically a hardware device, that can be used to create a logical <see cref="Device"/>.
/// </summary>
public readonly struct Adapter : IEquatable<Adapter>
{
    /// <summary>
    /// The handle to the underlying adapter for the current <see cref="Backend"/>.
    /// </summary>
    public readonly nint Handle;

    /// <summary>
    /// The index. Typically, an adapter with a lower index has a higher priority.
    /// </summary>
    public readonly uint Index;

    /// <summary>
    /// The reported name.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// The adapter's type.
    /// </summary>
    public readonly AdapterType Type;

    /// <summary>
    /// The amount of dedicated video memory in bytes.
    /// </summary>
    public readonly ulong DedicatedMemory;

    /// <summary>
    /// Contains a list of things this adapter supports.
    /// </summary>
    public readonly AdapterSupports Supports;

    public Adapter(nint handle, uint index, string name, AdapterType type, ulong dedicatedMemory, AdapterSupports supports)
    {
        Handle = handle;
        Index = index;
        Name = name;
        Type = type;
        DedicatedMemory = dedicatedMemory;
        Supports = supports;
    }

    public bool Equals(Adapter other)
    {
        return Handle == other.Handle;
    }

    public override bool Equals(object? obj)
    {
        return obj is Adapter other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Handle.GetHashCode();
    }

    public static bool operator ==(Adapter left, Adapter right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Adapter left, Adapter right)
    {
        return !left.Equals(right);
    }

    public override string ToString()
    {
        return $"{Name}: {{ Index: {Index}, Type: {Type}, Memory: {DedicatedMemory / 1024}KiB }}";
    }
}