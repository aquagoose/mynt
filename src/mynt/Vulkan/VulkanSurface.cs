using System.Runtime.InteropServices;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;

namespace mynt.Vulkan;

internal sealed unsafe class VulkanSurface : Surface
{
    public override bool IsDisposed { get; protected set; }

    private readonly Vk _vk;
    private readonly VkInstance _instance;
    private readonly SurfaceExtensionUnion _surfaceExtension;

    public readonly KhrSurface KhrSurface;
    public readonly SurfaceKHR Surface;
    
    public VulkanSurface(Vk vk, VkInstance instance, ref readonly SurfaceInfo info)
    {
        _vk = vk;
        _instance = instance;

        if (!_vk.TryGetInstanceExtension(_instance, out KhrSurface))
            throw new Exception("Failed to get KhrSurface extension.");

        switch (info.Type)
        {
            case SurfaceType.Win32:
            {
                if (!_vk.TryGetInstanceExtension(_instance, out _surfaceExtension.Win32!))
                    throw new Exception("Failed to get Win32 surface extension.");

                Win32SurfaceCreateInfoKHR surfaceInfo = new()
                {
                    SType = StructureType.Win32SurfaceCreateInfoKhr,
                    Hinstance = info.Display,
                    Hwnd = info.Window
                };

                Mynt.Log("Creating Win32 surface.");
                _surfaceExtension.Win32!.CreateWin32Surface(_instance, &surfaceInfo, null, out Surface)
                    .Check("Create Win32 surface");
                
                break;
            }
            case SurfaceType.Wayland:
            {
                if (!_vk.TryGetInstanceExtension(_instance, out _surfaceExtension.Wayland!))
                    throw new Exception("Failed to get Wayland surface extension.");

                WaylandSurfaceCreateInfoKHR surfaceInfo = new()
                {
                    SType = StructureType.WaylandSurfaceCreateInfoKhr,
                    Display = (nint*) info.Display,
                    Surface = (nint*) info.Window
                };

                Mynt.Log("Creating Wayland surface.");
                _surfaceExtension.Wayland!.CreateWaylandSurface(_instance, &surfaceInfo, null, out Surface)
                    .Check("Create Wayland surface");
                
                break;
            }
            case SurfaceType.Xcb:
            {
                if (!_vk.TryGetInstanceExtension(_instance, out _surfaceExtension.Xcb!))
                    throw new Exception("Failed to get xcb surface extension.");

                XcbSurfaceCreateInfoKHR surfaceInfo = new()
                {
                    SType = StructureType.XcbSurfaceCreateInfoKhr,
                    Connection = (nint*) info.Display,
                    Window = info.Window
                };

                Mynt.Log("Creating xcb surface.");
                _surfaceExtension.Xcb!.CreateXcbSurface(_instance, &surfaceInfo, null, out Surface)
                    .Check("Create xcb surface");
                
                break;
            }
            case SurfaceType.Xlib:
            {
                if (!_vk.TryGetInstanceExtension(_instance, out _surfaceExtension.Xlib!))
                    throw new Exception("Failed to get xlib surface extension.");

                XlibSurfaceCreateInfoKHR surfaceInfo = new()
                {
                    SType = StructureType.XlibSurfaceCreateInfoKhr,
                    Dpy = (nint*) info.Display,
                    Window = info.Window
                };

                Mynt.Log("Creating xlib surface.");
                _surfaceExtension.Xlib!.CreateXlibSurface(_instance, &surfaceInfo, null, out Surface)
                    .Check("Create xlib surface");
                
                break;
            }
            case SurfaceType.Cocoa:
            {
                if (!_vk.TryGetInstanceExtension(_instance, out _surfaceExtension.Metal!))
                    throw new Exception("Failed to get Metal surface extension.");

                MetalSurfaceCreateInfoEXT surfaceInfo = new()
                {
                    SType = StructureType.MetalSurfaceCreateInfoExt,
                    PLayer = (nint*) info.Window
                };

                Mynt.Log("Creating Metal surface.");
                _surfaceExtension.Metal!.CreateMetalSurface(_instance, &surfaceInfo, null, out Surface)
                    .Check("Create Metal surface");
                
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public override void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;
        
        Mynt.Log("Destroying surface.");
        KhrSurface.DestroySurface(_instance, Surface, null);
        
        _surfaceExtension.Win32?.Dispose();
        _surfaceExtension.Wayland?.Dispose();
        _surfaceExtension.Xcb?.Dispose();
        _surfaceExtension.Xlib?.Dispose();
        _surfaceExtension.Metal?.Dispose();
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct SurfaceExtensionUnion
    {
        [FieldOffset(0)] public KhrWin32Surface? Win32;
        [FieldOffset(0)] public KhrWaylandSurface? Wayland;
        [FieldOffset(0)] public KhrXcbSurface? Xcb;
        [FieldOffset(0)] public KhrXlibSurface? Xlib;
        [FieldOffset(0)] public ExtMetalSurface? Metal;
    }
}