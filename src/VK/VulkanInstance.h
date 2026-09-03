#pragma once

#include "mynt/Instance.h"

#include <vulkan/vulkan.h>

namespace mynt::VK
{
    class VulkanInstance final : public Instance
    {
        VkInstance _instance;

    public:
        explicit VulkanInstance(const InstanceInfo& info);
        ~VulkanInstance() override;
    };
}
