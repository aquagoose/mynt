namespace mynt;

/// <summary>
/// Defines various types of surfaces for different window managers.
/// </summary>
public enum SurfaceType
{
    Win32,

    Wayland,

    Xcb,

    Xlib,

    Cocoa
}