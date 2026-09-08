#!/usr/bin/env dotnet
#:project ../src/mynt/mynt.csproj
#:package piko.SDL3@0.0.6

using mynt;
using piko.SDL3;

if (!SDL.Init(SDL.InitFlags.Video | SDL.InitFlags.Events))
    throw new Exception($"Failed to initialize SDL: {SDL.GetError()}");

SDL.Window window = SDL.CreateWindow("Create Device Test", 800, 600, 0);
if (window.IsNull)
    throw new Exception($"Failed to create window: {SDL.GetError()}");

InstanceInfo instanceInfo = new InstanceInfo("Create Device Test", true);
Instance instance = Instance.Create(in instanceInfo);
Console.WriteLine(instance.Backend);

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
else
    throw new PlatformNotSupportedException("Unsupported platform!");

Surface surface = instance.CreateSurface(in surfaceInfo);

surface.Dispose();
instance.Dispose();
SDL.DestroyWindow(window);
SDL.Quit();