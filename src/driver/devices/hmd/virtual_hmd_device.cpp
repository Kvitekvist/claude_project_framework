#include "virtual_hmd_device.h"
#include "../../utils/logger.h"
#include "../../utils/math_utils.h"

namespace vremulator {

VirtualHMDDevice::VirtualHMDDevice()
    : m_deviceIndex(vr::k_unTrackedDeviceIndexInvalid)
    , m_activated(false)
    , m_serialNumber("VREMULATOR_HMD_001")
    , m_modelNumber("VREmulator Virtual HMD v1.0")
    , m_ipd(0.063f)  // 63mm default IPD
    , m_displayFrequency(90.0f)  // 90Hz
    , m_renderWidth(2016)  // Vive resolution per eye
    , m_renderHeight(2240)
    , m_secondsFromVsyncToPhotons(0.011f)  // ~11ms latency
{
    LOG_INFO("VirtualHMDDevice created: " + m_serialNumber);

    // Initialize pose at default standing height
    m_pose = MathUtils::CreateDefaultPose();
}

VirtualHMDDevice::~VirtualHMDDevice() {
    LOG_INFO("VirtualHMDDevice destroyed");
}

vr::EVRInitError VirtualHMDDevice::Activate(uint32_t unObjectId) {
    m_deviceIndex = unObjectId;

    LOG_INFO("VirtualHMDDevice::Activate - Index: " + std::to_string(m_deviceIndex));

    // Set up device properties
    SetupProperties();

    // Set up display properties
    SetDisplayProperties();

    m_activated = true;

    LOG_INFO("VirtualHMDDevice activated successfully");

    return vr::VRInitError_None;
}

void VirtualHMDDevice::Deactivate() {
    LOG_INFO("VirtualHMDDevice::Deactivate");

    m_deviceIndex = vr::k_unTrackedDeviceIndexInvalid;
    m_activated = false;
}

void VirtualHMDDevice::EnterStandby() {
    LOG_DEBUG("VirtualHMDDevice::EnterStandby");
}

void* VirtualHMDDevice::GetComponent(const char* pchComponentNameAndVersion) {
    // No additional components for now
    // In the future, could add IVRDisplayComponent, IVRCameraComponent, etc.
    return nullptr;
}

void VirtualHMDDevice::DebugRequest(const char* pchRequest, char* pchResponseBuffer, uint32_t unResponseBufferSize) {
    if (unResponseBufferSize >= 1) {
        pchResponseBuffer[0] = 0;
    }
}

vr::DriverPose_t VirtualHMDDevice::GetPose() {
    return m_pose;
}

void VirtualHMDDevice::UpdatePose(const vr::DriverPose_t& newPose) {
    m_pose = newPose;

    // Notify SteamVR of pose update
    if (m_activated && m_deviceIndex != vr::k_unTrackedDeviceIndexInvalid) {
        vr::VRServerDriverHost()->TrackedDevicePoseUpdated(m_deviceIndex, m_pose, sizeof(vr::DriverPose_t));
    }
}

void VirtualHMDDevice::SetPosition(double x, double y, double z) {
    m_pose.vecPosition[0] = x;
    m_pose.vecPosition[1] = y;
    m_pose.vecPosition[2] = z;

    if (m_activated && m_deviceIndex != vr::k_unTrackedDeviceIndexInvalid) {
        vr::VRServerDriverHost()->TrackedDevicePoseUpdated(m_deviceIndex, m_pose, sizeof(vr::DriverPose_t));
    }
}

void VirtualHMDDevice::SetRotation(double yaw, double pitch, double roll) {
    double qw, qx, qy, qz;
    MathUtils::EulerToQuaternion(yaw, pitch, roll, qw, qx, qy, qz);

    m_pose.qRotation.w = qw;
    m_pose.qRotation.x = qx;
    m_pose.qRotation.y = qy;
    m_pose.qRotation.z = qz;

    if (m_activated && m_deviceIndex != vr::k_unTrackedDeviceIndexInvalid) {
        vr::VRServerDriverHost()->TrackedDevicePoseUpdated(m_deviceIndex, m_pose, sizeof(vr::DriverPose_t));
    }
}

void VirtualHMDDevice::SetupProperties() {
    vr::VRProperties()->SetStringProperty(m_deviceIndex, vr::Prop_SerialNumber_String, m_serialNumber.c_str());
    vr::VRProperties()->SetStringProperty(m_deviceIndex, vr::Prop_ModelNumber_String, m_modelNumber.c_str());
    vr::VRProperties()->SetStringProperty(m_deviceIndex, vr::Prop_ManufacturerName_String, "VREmulator");
    vr::VRProperties()->SetStringProperty(m_deviceIndex, vr::Prop_TrackingSystemName_String, "vremulator");

    // Render model (use generic HMD model)
    vr::VRProperties()->SetStringProperty(m_deviceIndex, vr::Prop_RenderModelName_String, "{htc}vr_tracker_vive_1_0");

    // Device class
    vr::VRProperties()->SetInt32Property(m_deviceIndex, vr::Prop_DeviceClass_Int32, vr::TrackedDeviceClass_HMD);

    // Tracking
    vr::VRProperties()->SetBoolProperty(m_deviceIndex, vr::Prop_WillDriftInYaw_Bool, false);
    vr::VRProperties()->SetBoolProperty(m_deviceIndex, vr::Prop_DeviceIsWireless_Bool, true);
    vr::VRProperties()->SetBoolProperty(m_deviceIndex, vr::Prop_DeviceIsCharging_Bool, false);
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_DeviceBatteryPercentage_Float, 1.0f);

    // User IPD
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_UserIpdMeters_Float, m_ipd);

    // Display firmware version
    vr::VRProperties()->SetUint64Property(m_deviceIndex, vr::Prop_CurrentUniverseId_Uint64, 2);

    LOG_DEBUG("VirtualHMDDevice properties set");
}

void VirtualHMDDevice::SetDisplayProperties() {
    // Display frequency
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_DisplayFrequency_Float, m_displayFrequency);

    // Seconds from vsync to photons
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_SecondsFromVsyncToPhotons_Float, m_secondsFromVsyncToPhotons);

    // Display dimensions (meters)
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_DisplayMCImageWidth_Float, 0.11f);
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_DisplayMCImageHeight_Float, 0.12f);
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_DisplayMCImageNumChannels_Int32, 3);

    // Field of view
    // Vive-like FOV: ~110 degrees
    const float fov = 1.57f;  // ~90 degrees in radians, per direction from center
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_DisplayMCImageLeft_Float, -fov);
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_DisplayMCImageRight_Float, fov);
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_DisplayMCImageTop_Float, fov);
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_DisplayMCImageBottom_Float, -fov);

    // Recommended render target size
    vr::VRProperties()->SetInt32Property(m_deviceIndex, vr::Prop_DisplayMCImageWidth_Int32, m_renderWidth);
    vr::VRProperties()->SetInt32Property(m_deviceIndex, vr::Prop_DisplayMCImageHeight_Int32, m_renderHeight);

    // Distortion settings (use identity - no distortion for emulator)
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_LensCenterLeftU_Float, 0.5f);
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_LensCenterLeftV_Float, 0.5f);
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_LensCenterRightU_Float, 0.5f);
    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_LensCenterRightV_Float, 0.5f);

    vr::VRProperties()->SetFloatProperty(m_deviceIndex, vr::Prop_UserHeadToEyeDepthMeters_Float, 0.0f);

    LOG_DEBUG("VirtualHMDDevice display properties set - Resolution: " +
        std::to_string(m_renderWidth) + "x" + std::to_string(m_renderHeight) +
        " @ " + std::to_string((int)m_displayFrequency) + "Hz");
}

} // namespace vremulator
