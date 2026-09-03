#pragma once

#include "Common.h"

#include <memory>
#include <string>

namespace mynt
{
    struct InstanceInfo
    {
        std::string AppName;
        bool Debug;
        mynt::Backend Backend;
    };

    class Instance
    {
    public:
        virtual ~Instance() = default;

        static std::unique_ptr<Instance> Create(const InstanceInfo& info);
    };
}