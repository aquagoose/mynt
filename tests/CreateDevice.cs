#!/usr/bin/env dotnet
#:project ../src/mynt/mynt.csproj
#:package piko.SDL3@0.0.6

using mynt;
using piko.SDL3;

Mynt.MessageLogged += (severity, message, _, _) => Console.WriteLine($"[{severity}] {message}");

if (!SDL.Init(SDL.InitFlags.Video | SDL.InitFlags.Events))
    throw new Exception($"Failed to initialize SDL: {SDL.GetError()}");

SDL.Window window = SDL.CreateWindow("Create Device Test", 800, 600, 0);
if (window.IsNull)
    throw new Exception($"Failed to create window: {SDL.GetError()}");

InstanceInfo info = new InstanceInfo("Create Instance Test");
Instance instance = Instance.CreateInstance(in info);

Adapter[] adapters = instance.QueryAdapters();
foreach (Adapter adapter in adapters)
    Console.WriteLine(adapter);

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

Surface surface = instance.CreateSurface(surfaceInfo);
Device device = instance.CreateDevice(surface);

device.Dispose();
surface.Dispose();
instance.Dispose();
SDL.DestroyWindow(window);
SDL.Quit();