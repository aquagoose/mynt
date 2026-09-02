global using VkInstance = Silk.NET.Vulkan.Instance;
using System.Reflection;
using Silk.NET.Core;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;

namespace mynt.Vulkan;

internal sealed unsafe class VulkanInstance : Instance
{
    private static readonly Version32 ApiVersion = Vk.Version13;

    public override bool IsDisposed { get; protected set; }

    private readonly Vk _vk;
    private readonly VkInstance _instance;

    public VulkanInstance(ref readonly InstanceInfo info)
    {
        _vk = Vk.GetApi();

        Version32 instanceVersion;
        _vk.EnumerateInstanceVersion((uint*) &instanceVersion).Check("Enumerate instance version");
        Mynt.Log(Mynt.LogSeverity.Info, $"Vulkan API version: {instanceVersion.Major}.{instanceVersion.Minor}.{instanceVersion.Patch}");

        if (instanceVersion.Value < ApiVersion)
        {
            throw new PlatformNotSupportedException(
                $"Vulkan version 1.3 is required, but only {instanceVersion.Major}.{instanceVersion.Minor}.{instanceVersion.Patch} is supported. Please ensure your drivers are up-to-date.");
        }

        nint pAppName = SilkMarshal.StringToPtr(info.AppName);
        // the entry assembly is usually the application, so get the version from that
        Version appVersion = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(0, 0, 0);

        // the "engine" in this case is mynt.
        nint pEngineName = SilkMarshal.StringToPtr("mynt");
        Version engineVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0);

        uint numInstanceExtensions;
        _vk.EnumerateInstanceExtensionProperties((byte*) null, &numInstanceExtensions, null);
        ExtensionProperties* instanceExtensionProperties = stackalloc ExtensionProperties[(int) numInstanceExtensions];
        _vk.EnumerateInstanceExtensionProperties((byte*) null, &numInstanceExtensions, instanceExtensionProperties);

        uint numExtensions = 0;
        sbyte** instanceExtensions = stackalloc sbyte*[4];
        for (uint i = 0; i < numInstanceExtensions; i++)
        {
            sbyte* extensionName = (sbyte*) instanceExtensionProperties[i].ExtensionName;

            if (Mynt.ManagedAndUnmanagedStringsAreEqual(KhrWin32Surface.ExtensionName, extensionName) ||
                Mynt.ManagedAndUnmanagedStringsAreEqual(KhrWaylandSurface.ExtensionName, extensionName) ||
                Mynt.ManagedAndUnmanagedStringsAreEqual(KhrXcbSurface.ExtensionName, extensionName) ||
                Mynt.ManagedAndUnmanagedStringsAreEqual(KhrXlibSurface.ExtensionName, extensionName))
            {
                instanceExtensions[numExtensions++] = extensionName;
            }
        }

        ApplicationInfo appInfo = new()
        {
            SType = StructureType.ApplicationInfo,
            ApiVersion = ApiVersion,

            PApplicationName = (byte*) pAppName,
            ApplicationVersion = Vk.MakeVersion((uint) appVersion.Major, (uint) appVersion.Minor, (uint) appVersion.Build),

            PEngineName = (byte*) pEngineName,
            EngineVersion = Vk.MakeVersion((uint) engineVersion.Major, (uint) engineVersion.Minor, (uint) engineVersion.Build)
        };

        InstanceCreateInfo instanceInfo = new()
        {
            SType = StructureType.InstanceCreateInfo,
            PApplicationInfo = &appInfo,

            EnabledExtensionCount = numExtensions,
            PpEnabledExtensionNames = (byte**) instanceExtensions
        };

        Mynt.Log("Creating instance.");
        _vk.CreateInstance(&instanceInfo, null, out _instance).Check("Create instance");
    }

    public override Backend Backend => Backend.Vulkan;

    public override Adapter[] EnumerateAdapters()
    {
        List<Adapter> adapters = [];

        uint numPhysicalDevices;
        _vk.EnumeratePhysicalDevices(_instance, &numPhysicalDevices, null);
        PhysicalDevice* physicalDevices = stackalloc PhysicalDevice[(int) numPhysicalDevices];
        _vk.EnumeratePhysicalDevices(_instance, &numPhysicalDevices, physicalDevices);

        for (uint i = 0; i < numPhysicalDevices; i++)
        {
            PhysicalDevice device = physicalDevices[i];

            PhysicalDeviceProperties properties;
            PhysicalDeviceMemoryProperties memProperties;
            PhysicalDeviceFeatures features;

            _vk.GetPhysicalDeviceProperties(device, &properties);
            _vk.GetPhysicalDeviceMemoryProperties(device, &memProperties);
            _vk.GetPhysicalDeviceFeatures(device, &features);

            if (properties.ApiVersion < ApiVersion)
                continue;

            string name = new string((sbyte*) properties.DeviceName);
            AdapterType type = properties.DeviceType switch
            {
                PhysicalDeviceType.Other => AdapterType.Unknown,
                PhysicalDeviceType.IntegratedGpu => AdapterType.Integrated,
                PhysicalDeviceType.DiscreteGpu => AdapterType.Dedicated,
                PhysicalDeviceType.VirtualGpu => AdapterType.Unknown,
                PhysicalDeviceType.Cpu => AdapterType.Software,
                _ => throw new ArgumentOutOfRangeException()
            };

            // todo use the other memory heaps?
            ulong dedicatedMemory = memProperties.MemoryHeapCount > 0 ? memProperties.MemoryHeaps[0].Size : 0;
            AdapterSupports supports = new AdapterSupports();

            adapters.Add(new Adapter(device.Handle, i, name, type, dedicatedMemory, supports));
        }

        return adapters.ToArray();
    }

    public override void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;

        _vk.DestroyInstance(_instance, null);
        _vk.Dispose();
    }
}