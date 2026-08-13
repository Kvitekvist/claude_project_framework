cmake_minimum_required(VERSION 3.15)
project(driver_vremulator VERSION 0.1.0)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# OpenVR SDK path
set(OPENVR_SDK_PATH "${CMAKE_CURRENT_SOURCE_DIR}/../openvr_sdk")

# Driver sources
set(DRIVER_SOURCES
    exports.cpp
    provider/server_provider.cpp
    devices/hmd/virtual_hmd_device.cpp
    utils/logger.cpp
    utils/math_utils.cpp
)

set(DRIVER_HEADERS
    provider/server_provider.h
    devices/hmd/virtual_hmd_device.h
    utils/logger.h
    utils/math_utils.h
)

# Create DLL
add_library(driver_vremulator SHARED ${DRIVER_SOURCES} ${DRIVER_HEADERS})

# Include directories
target_include_directories(driver_vremulator PRIVATE
    ${OPENVR_SDK_PATH}/headers
    ${CMAKE_CURRENT_SOURCE_DIR}
)

# Link OpenVR lib
if(CMAKE_SIZEOF_VOID_P EQUAL 8)
    target_link_libraries(driver_vremulator
        ${OPENVR_SDK_PATH}/lib/win64/openvr_api.lib
    )
else()
    target_link_libraries(driver_vremulator
        ${OPENVR_SDK_PATH}/lib/win32/openvr_api.lib
    )
endif()

# Output directory
set_target_properties(driver_vremulator PROPERTIES
    RUNTIME_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/../../build/driver/vremulator/bin/win64"
    LIBRARY_OUTPUT_DIRECTORY "${CMAKE_SOURCE_DIR}/../../build/driver/vremulator/bin/win64"
)

# Copy OpenVR DLL to output directory
if(CMAKE_SIZEOF_VOID_P EQUAL 8)
    add_custom_command(TARGET driver_vremulator POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy_if_different
        "${OPENVR_SDK_PATH}/bin/win64/openvr_api.dll"
        "$<TARGET_FILE_DIR:driver_vremulator>/openvr_api.dll"
    )
else()
    add_custom_command(TARGET driver_vremulator POST_BUILD
        COMMAND ${CMAKE_COMMAND} -E copy_if_different
        "${OPENVR_SDK_PATH}/bin/win32/openvr_api.dll"
        "$<TARGET_FILE_DIR:driver_vremulator>/openvr_api.dll"
    )
endif()

# Installation
install(TARGETS driver_vremulator
    RUNTIME DESTINATION "bin/win64"
    LIBRARY DESTINATION "bin/win64"
)
