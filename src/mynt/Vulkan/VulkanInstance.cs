global using VkInstance = Silk.NET.Vulkan.Instance;
using System.Reflection;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

namespace mynt.Vulkan;

internal sealed unsafe class VulkanInstance : Instance
{
    public override bool IsDisposed { get; protected set; }

    private readonly Vk _vk;
    private readonly VkInstance _instance;

    public VulkanInstance(ref readonly InstanceInfo info)
    {
        _vk = Vk.GetApi();

        nint pAppName = SilkMarshal.StringToPtr(info.AppName);
        // the entry assembly is usually the application, so get the version from that
        Version appVersion = Assembly.GetEntryAssembly()?.GetName().Version ?? new Version(0, 0, 0);

        // the "engine" in this case is mynt.
        nint pEngineName = SilkMarshal.StringToPtr("mynt");
        Version engineVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version(0, 0, 0);

        ApplicationInfo appInfo = new()
        {
            SType = StructureType.ApplicationInfo,
            ApiVersion = Vk.Version13,

            PApplicationName = (byte*) pAppName,
            ApplicationVersion = Vk.MakeVersion((uint) appVersion.Major, (uint) appVersion.Minor, (uint) appVersion.Build),

            PEngineName = (byte*) pEngineName,
            EngineVersion = Vk.MakeVersion((uint) engineVersion.Major, (uint) engineVersion.Minor, (uint) engineVersion.Build)
        };

        InstanceCreateInfo instanceInfo = new()
        {
            SType = StructureType.InstanceCreateInfo,
            PApplicationInfo = &appInfo
        };

        _vk.CreateInstance(&instanceInfo, null, out _instance).Check("Create instance");
    }

    public override Backend Backend => Backend.Vulkan;

    public override void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;

        _vk.DestroyInstance(_instance, null);
        _vk.Dispose();
    }
}