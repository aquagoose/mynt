#pragma once

#include "mynt/Instance.h"

namespace mynt::VK
{
    class VulkanInstance final : public Instance
    {
    public:
        explicit VulkanInstance(const InstanceInfo& info);
        ~VulkanInstance() override;
    };
}
