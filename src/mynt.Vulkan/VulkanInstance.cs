global using VkInstance = Silk.NET.Vulkan.Instance;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;

namespace mynt.Vulkan;

/// <summary>
/// A Vulkan 1.3 <see cref="Instance"/>.
/// </summary>
public sealed unsafe class VulkanInstance : Instance
{
    /// <inheritdoc />
    public override bool IsDisposed { get; protected set; }

    private readonly Vk _vk;
    private readonly VkInstance _instance;

    public VulkanInstance(string appName)
    {
        _vk = Vk.GetApi();

        uint availableVersion;
        _vk.EnumerateInstanceVersion(&availableVersion);

        if (availableVersion < Vk.Version13)
            throw new Exception($"Vulkan version {Vk.Version13} (1.3) is required, however only version {availableVersion} is available. Please update your driver, or your system may be too old to support Vulkan 1.3.");

        nint pAppName = SilkMarshal.StringToPtr(appName);
        nint pEngineName = SilkMarshal.StringToPtr("mynt");

        ApplicationInfo appInfo = new()
        {
            SType = StructureType.ApplicationInfo,
            ApiVersion = Vk.Version13,

            PApplicationName = (byte*) pAppName,
            ApplicationVersion = Vk.MakeVersion(1, 0, 0),

            PEngineName = (byte*) pEngineName,
            EngineVersion = Vk.MakeVersion(1, 0, 0)
        };

        InstanceCreateInfo instanceInfo = new()
        {
            SType = StructureType.InstanceCreateInfo,
            PApplicationInfo = &appInfo
        };

        _vk.CreateInstance(&instanceInfo, null, out _instance).Check("Create instance");
    }

    /// <inheritdoc />
    public override void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;

        _vk.DestroyInstance(_instance, null);
    }
}