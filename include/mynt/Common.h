#pragma once

namespace mynt
{
    /**
     * Defines the built-in graphics backends.
     */
    enum class Backend
    {
        /**
         * Unknown/private backend. Passiing this into Instance::Create will let mynt automatically choose the best backend.
         */
        Unknown = 0,

        /**
         * Vulkan 1.3
         */
        Vulkan = 1,

        /*D3D12 = 2,

        Metal = 3,

        D3D11 = 4,

        OpenGL = 5,

        OpenGLES = 6*/
    };
}