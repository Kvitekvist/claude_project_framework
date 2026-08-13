#pragma once

#include <openvr_driver.h>
#include <vector>
#include <memory>

namespace vremulator {

class ServerProvider : public vr::IServerTrackedDeviceProvider {
public:
    // IServerTrackedDeviceProvider interface
    virtual vr::EVRInitError Init(vr::IVRDriverContext* pDriverContext) override;
    virtual void Cleanup() override;
    virtual const char* const* GetInterfaceVersions() override;
    virtual void RunFrame() override;
    virtual bool ShouldBlockStandbyMode() override;
    virtual void EnterStandby() override;
    virtual void LeaveStandby() override;

private:
    bool m_initialized;

    // Device instances
    std::unique_ptr<class VirtualHMDDevice> m_hmd;
    // TODO: Controllers in TICKET-0031
    // std::unique_ptr<VirtualControllerDevice> m_leftController;
    // std::unique_ptr<VirtualControllerDevice> m_rightController;
};

} // namespace vremulator
