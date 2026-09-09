namespace mynt;

/// <summary>
/// Represents a 2-dimensional size with a width and height.
/// </summary>
public struct Size2D : IEquatable<Size2D>
{
    /// <summary>
    /// The width.
    /// </summary>
    public uint Width;

    /// <summary>
    /// The height.
    /// </summary>
    public uint Height;

    /// <summary>
    /// Create a <see cref="Size2D"/> from a width and height.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public Size2D(uint width, uint height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Create a <see cref="Size2D"/> from a scalar value.
    /// </summary>
    /// <param name="wh">The scalar value that will be applied to both the width and height.</param>
    public Size2D(uint wh)
    {
        Width = wh;
        Height = wh;
    }

    public bool Equals(Size2D other)
    {
        return Width == other.Width && Height == other.Height;
    }

    public override bool Equals(object? obj)
    {
        return obj is Size2D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Width, Height);
    }

    public static bool operator ==(Size2D left, Size2D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Size2D left, Size2D right)
    {
        return !left.Equals(right);
    }
}