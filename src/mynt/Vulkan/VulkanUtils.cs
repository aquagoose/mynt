using Silk.NET.Vulkan;

namespace mynt.Vulkan;

internal static class VulkanUtils
{
    public static void Check(this Result result, string operation)
    {
        if (result != Result.Success)
            throw new Exception($"Vulkan operation \"{operation}\" failed: {result}");
    }
}