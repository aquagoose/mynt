#include "mynt/Instance.h"

#ifdef MYNT_VULKAN
#include "VK/VulkanInstance.h"
#endif

namespace mynt
{
    std::unique_ptr<Instance> Instance::Create(const InstanceInfo& info)
    {
#ifdef MYNT_VULKAN
        return std::make_unique<VK::VulkanInstance>();
#endif
    }
}
