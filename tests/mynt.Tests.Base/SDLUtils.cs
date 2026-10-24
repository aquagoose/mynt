using piko.SDL3;

namespace mynt.Tests.Base;

public static class SDLUtils
{
    extension(SDL)
    {
        public static Surface CreateMyntSurface(Instance instance, SDL.Window window)
        {
            SurfaceInfo surfaceInfo;
            uint props = SDL.GetWindowProperties(window);
            if (OperatingSystem.IsWindows())
            {
                nint hinstance = SDL.GetPointerProperty(props, SDL.Prop.WindowWin32InstancePointer, 0);
                nint hwnd = SDL.GetPointerProperty(props, SDL.Prop.WindowWin32HwndPointer, 0);
                surfaceInfo = new SurfaceInfo(SurfaceType.Win32, hinstance, hwnd);
            }
            else if (OperatingSystem.IsLinux())
            {
                string driver = SDL.GetCurrentVideoDriver();
                switch (driver)
                {
                    case "wayland":
                    {
                        nint display = SDL.GetPointerProperty(props, SDL.Prop.WindowWaylandDisplayPointer, 0);
                        nint surfacePtr = SDL.GetPointerProperty(props, SDL.Prop.WindowWaylandSurfacePointer, 0);
                        surfaceInfo = new SurfaceInfo(SurfaceType.Wayland, display, surfacePtr);
                        break;
                    }
                    case "x11":
                    {
                        nint display = SDL.GetPointerProperty(props, SDL.Prop.WindowX11DisplayPointer, 0);
                        long windowNum = SDL.GetNumberProperty(props, SDL.Prop.WindowX11WindowNumber, 0);
                        surfaceInfo = new SurfaceInfo(SurfaceType.Xcb, display, (nint) windowNum);
                        break;
                    }
                    default:
                        throw new PlatformNotSupportedException($"Unsupported driver \"{driver}\"!");
                }
            }
            else if (OperatingSystem.IsMacOS())
            {
                nint view = SDL.MetalCreateView(window);
                nint layer = SDL.MetalGetLayer(view);
                surfaceInfo = new SurfaceInfo(SurfaceType.Cocoa, 0, layer);
            }
            else
                throw new PlatformNotSupportedException("Unsupported OS!");

            return instance.CreateSurface(in surfaceInfo);
        }
    }
}