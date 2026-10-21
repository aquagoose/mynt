global using VkInstance = Silk.NET.Vulkan.Instance;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Silk.NET.Core;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.EXT;
using Silk.NET.Vulkan.Extensions.KHR;

namespace mynt.Vulkan;

internal sealed unsafe class VulkanInstance : Instance
{
    public static Version32 APIVersion = Vk.Version13;
    
    public override bool IsDisposed { get; protected set; }

    private readonly Vk _vk;
    private readonly VkInstance _instance;

    public VulkanInstance(ref readonly InstanceInfo info)
    {
        _vk = Vk.GetApi();

        nint pAppName = Marshal.StringToHGlobalAnsi(info.AppName);
        Version appVersion = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version();
        
        ReadOnlySpan<byte> engineName = "mynt"u8;
        Version engineVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version();

        ApplicationInfo appInfo = new()
        {
            SType = StructureType.ApplicationInfo,
            ApiVersion = APIVersion,

            PApplicationName = (byte*) pAppName,
            ApplicationVersion =
                Vk.MakeVersion((uint) appVersion.Major, (uint) appVersion.Minor, (uint) appVersion.Build),

            PEngineName = (byte*) Unsafe.AsPointer(ref MemoryMarshal.GetReference(engineName)),
            EngineVersion = Vk.MakeVersion((uint) engineVersion.Major, (uint) engineVersion.Minor,
                (uint) engineVersion.Build)
        };
        
        uint numInstanceExtensions = 0;
        byte** instanceExtensions = stackalloc byte*[6];

        uint numExtensionProperties;
        _vk.EnumerateInstanceExtensionProperties((byte*) null, &numExtensionProperties, null);
        ExtensionProperties* extensionProperties = stackalloc ExtensionProperties[(int) numExtensionProperties];
        _vk.EnumerateInstanceExtensionProperties((byte*) null, &numExtensionProperties, extensionProperties);

        for (uint i = 0; i < numExtensionProperties; i++)
        {
            byte* extensionName = extensionProperties[i].ExtensionName;
            if
            (
                Utils.NullTerminatedStringEqualsManagedString(extensionName, KhrSurface.ExtensionName) ||
                Utils.NullTerminatedStringEqualsManagedString(extensionName, KhrWin32Surface.ExtensionName) ||
                Utils.NullTerminatedStringEqualsManagedString(extensionName, KhrWaylandSurface.ExtensionName) ||
                Utils.NullTerminatedStringEqualsManagedString(extensionName, KhrXcbSurface.ExtensionName) ||
                Utils.NullTerminatedStringEqualsManagedString(extensionName, KhrXlibSurface.ExtensionName) ||
                Utils.NullTerminatedStringEqualsManagedString(extensionName, ExtMetalSurface.ExtensionName)
            )
            {
                instanceExtensions[numInstanceExtensions++] = extensionName;
            }
        }

        InstanceCreateInfo instanceInfo = new()
        {
            SType = StructureType.InstanceCreateInfo,
            PApplicationInfo = &appInfo,

            EnabledExtensionCount = numInstanceExtensions,
            PpEnabledExtensionNames = instanceExtensions
        };

        Mynt.Log("Creating instance");
        _vk.CreateInstance(&instanceInfo, null, out _instance).Check("Create instance");
    }

    public override Adapter[] QueryAdapters()
    {
        uint numPhysicalDevices;
        _vk.EnumeratePhysicalDevices(_instance, &numPhysicalDevices, null);
        PhysicalDevice* devices = stackalloc PhysicalDevice[(int) numPhysicalDevices];
        _vk.EnumeratePhysicalDevices(_instance, &numPhysicalDevices, devices);

        List<Adapter> adapters = new List<Adapter>((int) numPhysicalDevices);
        for (uint i = 0; i < numPhysicalDevices; i++)
        {
            PhysicalDevice device = devices[i];

            PhysicalDeviceProperties properties;
            _vk.GetPhysicalDeviceProperties(device, &properties);
            PhysicalDeviceMemoryProperties memoryProps;
            _vk.GetPhysicalDeviceMemoryProperties(device, &memoryProps);
            
            if (properties.ApiVersion < APIVersion)
                continue;

            string name = new string((sbyte*) properties.DeviceName);
            // if there isn't even one memory heap... may be a problem. we'll deal with it later!!!!
            ulong memory = memoryProps.MemoryHeapCount > 0 ? memoryProps.MemoryHeaps[0].Size : 0;

            AdapterType type = properties.DeviceType switch
            {
                PhysicalDeviceType.Other => AdapterType.Unknown,
                PhysicalDeviceType.IntegratedGpu => AdapterType.Integrated,
                PhysicalDeviceType.DiscreteGpu => AdapterType.Dedicated,
                PhysicalDeviceType.VirtualGpu => AdapterType.Unknown,
                PhysicalDeviceType.Cpu => AdapterType.Software,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            adapters.Add(new Adapter(device.Handle, name, memory, type, new AdapterSupports()));
        }

        return adapters.ToArray();
    }

    public override void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;

        Mynt.Log("Destroying instance.");
        _vk.DestroyInstance(_instance, null);
        _vk.Dispose();
    }
}