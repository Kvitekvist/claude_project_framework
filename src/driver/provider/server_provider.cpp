#include "server_provider.h"
#include "../utils/logger.h"
#include "../devices/hmd/virtual_hmd_device.h"

namespace vremulator {

vr::EVRInitError ServerProvider::Init(vr::IVRDriverContext* pDriverContext) {
    VR_INIT_SERVER_DRIVER_CONTEXT(pDriverContext);

    LOG_INFO("===========================================");
    LOG_INFO("VREmulator Driver Initializing...");
    LOG_INFO("===========================================");

    m_initialized = true;

    LOG_INFO("Driver provider initialized successfully");

    // Create and register HMD device
    LOG_INFO("Creating virtual HMD device...");
    m_hmd = std::make_unique<VirtualHMDDevice>();
    bool added = vr::VRServerDriverHost()->TrackedDeviceAdded(
        m_hmd->GetSerialNumber().c_str(),
        vr::TrackedDeviceClass_HMD,
        m_hmd.get()
    );

    if (added) {
        LOG_INFO("Virtual HMD device registered successfully");
    } else {
        LOG_ERROR("Failed to register virtual HMD device");
        return vr::VRInitError_Driver_Failed;
    }

    // TODO: Create and register controller devices (TICKET-0031)

    return vr::VRInitError_None;
}

void ServerProvider::Cleanup() {
    LOG_INFO("VREmulator Driver Cleanup");

    // Cleanup devices
    if (m_hmd) {
        m_hmd.reset();
    }

    VR_CLEANUP_SERVER_DRIVER_CONTEXT();
    m_initialized = false;
}

const char* const* ServerProvider::GetInterfaceVersions() {
    return vr::k_InterfaceVersions;
}

void ServerProvider::RunFrame() {
    // Called every frame by SteamVR
    // TODO: Update device poses here (TICKET-0030, TICKET-0031)
}

bool ServerProvider::ShouldBlockStandbyMode() {
    // Don't block standby mode
    return false;
}

void ServerProvider::EnterStandby() {
    LOG_INFO("Entering standby mode");
}

void ServerProvider::LeaveStandby() {
    LOG_INFO("Leaving standby mode");
}

} // namespace vremulator
