#pragma once

#include <vulkan/vulkan.h>
#include <stdexcept>
#include <string>

#define VK_API_VERSION VK_API_VERSION_1_3

#define VK_CHECK(res, operation) {\
    VkResult result = res;\
    if (result != VK_SUCCESS) throw std::runtime_error("Vulkan operation \"" + std::string(operation) + "\" failed: " + mynt::VK::VulkanCommon::VkResultToString(result));\
}

namespace mynt::VK::VulkanCommon
{
    std::string VkResultToString(VkResult result)
    {
#define RES(res) case VK_##res: return #res;
        switch (result)
        {
            RES(SUCCESS)
            RES(NOT_READY)
            RES(TIMEOUT)
            RES(EVENT_SET)
            RES(EVENT_RESET)
            RES(INCOMPLETE)
            RES(ERROR_OUT_OF_HOST_MEMORY)
            RES(ERROR_OUT_OF_DEVICE_MEMORY)
            RES(ERROR_INITIALIZATION_FAILED)
            RES(ERROR_DEVICE_LOST)
            RES(ERROR_MEMORY_MAP_FAILED)
            RES(ERROR_LAYER_NOT_PRESENT)
            RES(ERROR_EXTENSION_NOT_PRESENT)
            RES(ERROR_FEATURE_NOT_PRESENT)
            RES(ERROR_INCOMPATIBLE_DRIVER)
            RES(ERROR_TOO_MANY_OBJECTS)
            RES(ERROR_FORMAT_NOT_SUPPORTED)
            RES(ERROR_FRAGMENTED_POOL)
            RES(ERROR_UNKNOWN)
            RES(ERROR_VALIDATION_FAILED)
            RES(ERROR_OUT_OF_POOL_MEMORY)
            RES(ERROR_INVALID_EXTERNAL_HANDLE)
            RES(ERROR_INVALID_OPAQUE_CAPTURE_ADDRESS)
            RES(ERROR_FRAGMENTATION)
            RES(PIPELINE_COMPILE_REQUIRED)
            RES(ERROR_NOT_PERMITTED)
            RES(ERROR_SURFACE_LOST_KHR)
            RES(ERROR_NATIVE_WINDOW_IN_USE_KHR)
            RES(SUBOPTIMAL_KHR)
            RES(ERROR_OUT_OF_DATE_KHR)
            RES(ERROR_INCOMPATIBLE_DISPLAY_KHR)
            RES(ERROR_INVALID_SHADER_NV)
            RES(ERROR_IMAGE_USAGE_NOT_SUPPORTED_KHR)
            RES(ERROR_VIDEO_PICTURE_LAYOUT_NOT_SUPPORTED_KHR)
            RES(ERROR_VIDEO_PROFILE_OPERATION_NOT_SUPPORTED_KHR)
            RES(ERROR_VIDEO_PROFILE_FORMAT_NOT_SUPPORTED_KHR)
            RES(ERROR_VIDEO_PROFILE_CODEC_NOT_SUPPORTED_KHR)
            RES(ERROR_VIDEO_STD_VERSION_NOT_SUPPORTED_KHR)
            RES(ERROR_INVALID_DRM_FORMAT_MODIFIER_PLANE_LAYOUT_EXT)
            RES(ERROR_PRESENT_TIMING_QUEUE_FULL_EXT)
            RES(ERROR_FULL_SCREEN_EXCLUSIVE_MODE_LOST_EXT)
            RES(THREAD_IDLE_KHR)
            RES(THREAD_DONE_KHR)
            RES(OPERATION_DEFERRED_KHR)
            RES(OPERATION_NOT_DEFERRED_KHR)
            RES(ERROR_INVALID_VIDEO_STD_PARAMETERS_KHR)
            RES(ERROR_COMPRESSION_EXHAUSTED_EXT)
            RES(INCOMPATIBLE_SHADER_BINARY_EXT)
            RES(PIPELINE_BINARY_MISSING_KHR)
            RES(ERROR_NOT_ENOUGH_SPACE_KHR)
            RES(RESULT_MAX_ENUM)
        }
#undef RES
    }
}