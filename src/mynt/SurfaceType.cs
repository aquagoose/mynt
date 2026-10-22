namespace mynt;

/// <summary>
/// Defines various supported types of <see cref="Surface"/>.
/// </summary>
public enum SurfaceType
{
    Win32,
    Wayland,
    Xcb,
    Xlib,
    Cocoa
}