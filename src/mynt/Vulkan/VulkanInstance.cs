using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Silk.NET.Core;
using Silk.NET.Vulkan;

namespace mynt.Vulkan;

internal sealed unsafe class VulkanInstance : Instance
{
    public static Version32 APIVersion = Vk.Version13;
    
    public override bool IsDisposed { get; protected set; }

    private readonly Vk _vk;

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
            ApplicationVersion = Vk.MakeVersion((uint) appVersion.Major, (uint) appVersion.Minor, (uint) appVersion.Build),
            
            PEngineName = (byte*) Unsafe.AsPointer(ref MemoryMarshal.GetReference(engineName)),
            EngineVersion = Vk.MakeVersion((uint) engineVersion.Major, (uint) engineVersion.Minor, (uint) engineVersion.Build)
        };
        
        
    }
    
    public override void Dispose()
    {
        if (IsDisposed)
            return;
        IsDisposed = true;
        
        _vk.Dispose();
    }
}