#pragma once

#include <openvr_driver.h>
#include <string>

namespace vremulator {

class VirtualHMDDevice : public vr::ITrackedDeviceServerDriver {
public:
    VirtualHMDDevice();
    virtual ~VirtualHMDDevice();

    // ITrackedDeviceServerDriver interface
    virtual vr::EVRInitError Activate(uint32_t unObjectId) override;
    virtual void Deactivate() override;
    virtual void EnterStandby() override;
    virtual void* GetComponent(const char* pchComponentNameAndVersion) override;
    virtual void DebugRequest(const char* pchRequest, char* pchResponseBuffer, uint32_t unResponseBufferSize) override;
    virtual vr::DriverPose_t GetPose() override;

    // Device management
    uint32_t GetDeviceIndex() const { return m_deviceIndex; }
    bool IsActivated() const { return m_activated; }
    std::string GetSerialNumber() const { return m_serialNumber; }

    // Pose control
    void UpdatePose(const vr::DriverPose_t& newPose);
    void SetPosition(double x, double y, double z);
    void SetRotation(double yaw, double pitch, double roll);
    void UpdateFromInput();  // Read from shared memory

private:
    void InitSharedMemory();
    void CleanupSharedMemory();

    void* m_sharedMemoryHandle;
    void* m_sharedMemoryData;
    void SetupProperties();
    void SetDisplayProperties();

    uint32_t m_deviceIndex;
    bool m_activated;

    vr::DriverPose_t m_pose;

    // Device properties
    std::string m_serialNumber;
    std::string m_modelNumber;

    // Display configuration
    float m_ipd;  // Interpupillary distance
    float m_displayFrequency;
    int32_t m_renderWidth;
    int32_t m_renderHeight;
    float m_secondsFromVsyncToPhotons;
};

} // namespace vremulator
