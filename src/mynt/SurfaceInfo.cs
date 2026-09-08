namespace mynt;

/// <summary>
/// Information used to create a <see cref="Surface"/>.
/// </summary>
public struct SurfaceInfo
{
    public SurfaceType Type;

    public nint Display;

    public nint Window;

    public SurfaceInfo(SurfaceType type, IntPtr display, IntPtr window)
    {
        Type = type;
        Display = display;
        Window = window;
    }

    public static SurfaceInfo Win32(nint hinstance, nint hwnd)
        => new SurfaceInfo(SurfaceType.Win32, hinstance, hwnd);

    public static SurfaceInfo Wayland(nint display, nint surface)
        => new SurfaceInfo(SurfaceType.Wayland, display, surface);

    public static SurfaceInfo Xcb(nint connection, nint window)
        => new SurfaceInfo(SurfaceType.Xcb, connection, window);

    public static SurfaceInfo Xlib(nint dpy, nint window)
        => new SurfaceInfo(SurfaceType.Xlib, dpy, window);

    public static SurfaceInfo Cocoa(nint metalLayer)
        => new SurfaceInfo(SurfaceType.Cocoa, 0, metalLayer);
}