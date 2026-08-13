#include <openvr_driver.h>
#include "provider/server_provider.h"
#include "utils/logger.h"

// Global provider instance
static vremulator::ServerProvider g_serverProvider;

// Driver entry point
extern "C" __declspec(dllexport) void* HmdDriverFactory(const char* pInterfaceName, int* pReturnCode) {
    if (0 == strcmp(vr::IServerTrackedDeviceProvider_Version, pInterfaceName)) {
        LOG_INFO("HmdDriverFactory: Returning ServerTrackedDeviceProvider");
        return &g_serverProvider;
    }

    LOG_WARNING("HmdDriverFactory: Interface not found: " + std::string(pInterfaceName));

    if (pReturnCode) {
        *pReturnCode = vr::VRInitError_Init_InterfaceNotFound;
    }

    return nullptr;
}
