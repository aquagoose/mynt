using piko.SDL3;

namespace mynt.Tests.Core;

public static class SDLExtensions
{
    extension(SDL)
    {
        public static Surface CreateMyntSurface(SDL.Window window, Instance instance)
        {
            SurfaceInfo surfaceInfo;
            uint props = SDL.GetWindowProperties(window);
            if (OperatingSystem.IsWindows())
            {
                nint instancePtr = SDL.GetPointerProperty(props, SDL.Prop.WindowWin32InstancePointer, 0);
                nint windowPtr = SDL.GetPointerProperty(props, SDL.Prop.WindowWin32HwndPointer, 0);
                surfaceInfo = SurfaceInfo.Win32(instancePtr, windowPtr);
            }
            else if (OperatingSystem.IsLinux())
            {
                string driver = SDL.GetCurrentVideoDriver();
                switch (driver)
                {
                    case "wayland":
                    {
                        nint displayPtr = SDL.GetPointerProperty(props, SDL.Prop.WindowWaylandDisplayPointer, 0);
                        nint surfacePtr = SDL.GetPointerProperty(props, SDL.Prop.WindowWaylandSurfacePointer, 0);
                        surfaceInfo = SurfaceInfo.Wayland(displayPtr, surfacePtr);
                        break;
                    }
                    case "x11":
                    {
                        nint displayPtr = SDL.GetPointerProperty(props, SDL.Prop.WindowX11DisplayPointer, 0);
                        long windowNum = SDL.GetNumberProperty(props, SDL.Prop.WindowX11WindowNumber, 0);
                        surfaceInfo = SurfaceInfo.Xlib(displayPtr, (nint) windowNum);
                        break;
                    }
                    default:
                        throw new PlatformNotSupportedException($"Unsupported video driver \"{driver}\"!");
                }
            }
            else if (OperatingSystem.IsMacOS())
            {
                nint view = SDL.MetalCreateView(window);
                nint layer = SDL.MetalGetLayer(view);
                surfaceInfo = SurfaceInfo.Cocoa(layer);
            }
            else
                throw new PlatformNotSupportedException("Unsupported platform!");

            return instance.CreateSurface(in surfaceInfo);
        }
    }
}