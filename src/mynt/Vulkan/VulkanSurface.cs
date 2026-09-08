global using VkSurfaceKHR = Silk.NET.Vulkan.SurfaceKHR;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;

namespace mynt.Vulkan;

internal sealed unsafe class VulkanSurface : Surface
{
    private readonly Vk _vk;
    private readonly VkInstance _instance;

    public readonly KhrSurface KhrSurface;
    public readonly VkSurfaceKHR Surface;

    public VulkanSurface(Vk vk, VkInstance instance, ref readonly SurfaceInfo info)
    {
        _vk = vk;
        _instance = instance;

        if (!_vk.TryGetInstanceExtension(_instance, out KhrSurface))
            throw new Exception("Failed to get KHRSurface extension.");

        switch (info.Type)
        {
            case SurfaceType.Win32:
            {
                if (!_vk.TryGetInstanceExtension(_instance, out KhrWin32Surface win32Surface))
                    throw new Exception("Failed to get Win32Surface extension.");

                Win32SurfaceCreateInfoKHR surfaceInfo = new()
                {
                    SType = StructureType.Win32SurfaceCreateInfoKhr,
                    Hinstance = info.Display,
                    Hwnd = info.Window
                };

                Mynt.Log("Creating Win32 surface.");
                win32Surface.CreateWin32Surface(_instance, &surfaceInfo, null, out Surface)
                    .Check("Create Win32 Surface");

                win32Surface.Dispose();
                break;
            }

            case SurfaceType.Wayland:
            {
                if (!_vk.TryGetInstanceExtension(_instance, out KhrWaylandSurface waylandSurface))
                    throw new Exception("Failed to get WaylandSurface extension.");

                WaylandSurfaceCreateInfoKHR surfaceInfo = new()
                {
                    SType = StructureType.WaylandSurfaceCreateInfoKhr,
                    Display = (nint*) info.Display,
                    Surface = (nint*) info.Window
                };

                Mynt.Log("Creating Wayland surface.");
                waylandSurface.CreateWaylandSurface(_instance, &surfaceInfo, null, out Surface)
                    .Check("Create Wayland Surface");

                waylandSurface.Dispose();
                break;
            }

            case SurfaceType.Xcb:
            {
                if (!_vk.TryGetInstanceExtension(_instance, out KhrXcbSurface xcbSurface))
                    throw new Exception("Failed to get XcbSurface extension.");

                XcbSurfaceCreateInfoKHR surfaceInfo = new()
                {
                    SType = StructureType.XcbSurfaceCreateInfoKhr,
                    Connection = (nint*) info.Display,
                    Window = info.Window
                };

                Mynt.Log("Creating Xcb surface.");
                xcbSurface.CreateXcbSurface(_instance, &surfaceInfo, null, out Surface)
                    .Check("Create Xcb Surface");

                xcbSurface.Dispose();
                break;
            }

            case SurfaceType.Xlib:
            {
                if (!_vk.TryGetInstanceExtension(_instance, out KhrXlibSurface xlibSurface))
                    throw new Exception("Failed to get XlibSurface extension.");

                XlibSurfaceCreateInfoKHR surfaceInfo = new()
                {
                    SType = StructureType.XlibSurfaceCreateInfoKhr,
                    Dpy = (nint*) info.Display,
                    Window = info.Window
                };

                Mynt.Log("Creating Xlib surface.");
                xlibSurface.CreateXlibSurface(_instance, &surfaceInfo, null, out Surface)
                    .Check("Create Xlib Surface");

                xlibSurface.Dispose();
                break;
            }

            case SurfaceType.Cocoa:
            {
                if (!_vk.TryGetInstanceExtension(_instance, out ExtMetalSurface metalSurface))
                    throw new Exception("Failed to get MetalSurface extension.");

                MetalSurfaceCreateInfoEXT surfaceInfo = new()
                {
                    SType = StructureType.MetalSurfaceCreateInfoExt,
                    PLayer = (nint*) info.Window
                };

                Mynt.Log("Creating Metal surface.");
                metalSurface.CreateMetalSurface(_instance, &surfaceInfo, null, out Surface)
                    .Check("Create Metal Surface");

                metalSurface.Dispose();
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

        KhrSurface.DestroySurface(_instance, Surface, null);
        KhrSurface.Dispose();
    }
}