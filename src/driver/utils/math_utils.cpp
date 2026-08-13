#include "math_utils.h"
#include <cstring>

namespace vremulator {

vr::DriverPose_t MathUtils::CreateDefaultPose() {
    vr::DriverPose_t pose = {};

    // Position at standing height (1.6m up)
    pose.vecPosition[0] = 0.0;  // X
    pose.vecPosition[1] = 1.6;  // Y (up)
    pose.vecPosition[2] = 0.0;  // Z

    // Identity rotation (no rotation)
    pose.qWorldFromDriverRotation = IdentityQuaternion();
    pose.qDriverFromHeadRotation = IdentityQuaternion();
    pose.qRotation = IdentityQuaternion();

    // Velocity and acceleration (all zero - static for now)
    pose.vecVelocity[0] = 0.0;
    pose.vecVelocity[1] = 0.0;
    pose.vecVelocity[2] = 0.0;

    pose.vecAngularVelocity[0] = 0.0;
    pose.vecAngularVelocity[1] = 0.0;
    pose.vecAngularVelocity[2] = 0.0;

    pose.vecAcceleration[0] = 0.0;
    pose.vecAcceleration[1] = 0.0;
    pose.vecAcceleration[2] = 0.0;

    pose.vecAngularAcceleration[0] = 0.0;
    pose.vecAngularAcceleration[1] = 0.0;
    pose.vecAngularAcceleration[2] = 0.0;

    // Pose state
    pose.poseIsValid = true;
    pose.deviceIsConnected = true;
    pose.result = vr::TrackingResult_Running_OK;

    // Time offset (0 for immediate)
    pose.poseTimeOffset = 0.0;

    // Will be filled by driver when pose is updated
    pose.shouldApplyHeadModel = false;
    pose.willDriftInYaw = false;

    return pose;
}

vr::DriverPose_t MathUtils::CreatePose(double x, double y, double z) {
    vr::DriverPose_t pose = CreateDefaultPose();
    pose.vecPosition[0] = x;
    pose.vecPosition[1] = y;
    pose.vecPosition[2] = z;
    return pose;
}

vr::DriverPose_t MathUtils::CreatePose(double x, double y, double z,
                                       double yaw, double pitch, double roll) {
    vr::DriverPose_t pose = CreatePose(x, y, z);

    double qw, qx, qy, qz;
    EulerToQuaternion(yaw, pitch, roll, qw, qx, qy, qz);

    pose.qRotation.w = qw;
    pose.qRotation.x = qx;
    pose.qRotation.y = qy;
    pose.qRotation.z = qz;

    return pose;
}

void MathUtils::EulerToQuaternion(double yaw, double pitch, double roll,
                                  double& qw, double& qx, double& qy, double& qz) {
    // Conversion from Euler angles (in radians) to quaternion
    double cy = std::cos(yaw * 0.5);
    double sy = std::sin(yaw * 0.5);
    double cp = std::cos(pitch * 0.5);
    double sp = std::sin(pitch * 0.5);
    double cr = std::cos(roll * 0.5);
    double sr = std::sin(roll * 0.5);

    qw = cr * cp * cy + sr * sp * sy;
    qx = sr * cp * cy - cr * sp * sy;
    qy = cr * sp * cy + sr * cp * sy;
    qz = cr * cp * sy - sr * sp * cy;
}

void MathUtils::QuaternionToEuler(double qw, double qx, double qy, double qz,
                                  double& yaw, double& pitch, double& roll) {
    // Roll (x-axis rotation)
    double sinr_cosp = 2.0 * (qw * qx + qy * qz);
    double cosr_cosp = 1.0 - 2.0 * (qx * qx + qy * qy);
    roll = std::atan2(sinr_cosp, cosr_cosp);

    // Pitch (y-axis rotation)
    double sinp = 2.0 * (qw * qy - qz * qx);
    if (std::abs(sinp) >= 1)
        pitch = std::copysign(M_PI / 2, sinp); // Use 90 degrees if out of range
    else
        pitch = std::asin(sinp);

    // Yaw (z-axis rotation)
    double siny_cosp = 2.0 * (qw * qz + qx * qy);
    double cosy_cosp = 1.0 - 2.0 * (qy * qy + qz * qz);
    yaw = std::atan2(siny_cosp, cosy_cosp);
}

vr::HmdQuaternion_t MathUtils::IdentityQuaternion() {
    vr::HmdQuaternion_t q;
    q.w = 1.0;
    q.x = 0.0;
    q.y = 0.0;
    q.z = 0.0;
    return q;
}

} // namespace vremulator
