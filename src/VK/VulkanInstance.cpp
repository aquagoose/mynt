#include "VulkanInstance.h"
#include "VulkanCommon.h"

namespace mynt::VK
{
    VulkanInstance::VulkanInstance(const InstanceInfo& info)
    {
        VkApplicationInfo appInfo;
        appInfo.sType = VK_STRUCTURE_TYPE_APPLICATION_INFO;
        appInfo.apiVersion = VK_API_VERSION;
        appInfo.pApplicationName = info.AppName.c_str();
        appInfo.applicationVersion = VK_VERSION_1_0;
        appInfo.pEngineName = "mynt";
        appInfo.engineVersion = VK_VERSION_1_0;

        VkInstanceCreateInfo instanceInfo;
        instanceInfo.sType = VK_STRUCTURE_TYPE_INSTANCE_CREATE_INFO;
        instanceInfo.pApplicationInfo = &appInfo;

        VK_CHECK(vkCreateInstance(&instanceInfo, nullptr, &_instance), "Create instance");
    }

    VulkanInstance::~VulkanInstance()
    {
        vkDestroyInstance(_instance, nullptr);
    }
}
