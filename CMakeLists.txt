cmake_minimum_required(VERSION 4.3)
project(mynt)

set(CMAKE_CXX_STANDARD 17)

option(MYNT_VULKAN "Enable the Vulkan 1.3 backend" ON)

set(MYNT_LIBS "")
file(GLOB MYNT_SRC CONFIGURE_DEPENDS
        include/mynt/*.h
        src/*.cpp
)

if (MYNT_VULKAN)
    file(GLOB MYNT_VULKAN_SRC CONFIGURE_DEPENDS src/VK/*.h src/VK/*.cpp)
    list(APPEND MYNT_SRC ${MYNT_VULKAN_SRC})
    find_package(Vulkan REQUIRED)
    list(APPEND MYNT_LIBS Vulkan::Vulkan)
endif ()

add_library(mynt STATIC ${MYNT_SRC})
target_include_directories(mynt PUBLIC include)
target_link_libraries(mynt PRIVATE ${MYNT_LIBS})