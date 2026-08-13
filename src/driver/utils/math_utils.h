#pragma once

#include <openvr_driver.h>
#include <cmath>

namespace vremulator {

// Utility functions for VR math operations

class MathUtils {
public:
    // Create identity pose at origin
    static vr::DriverPose_t CreateDefaultPose();

    // Create pose at specific position
    static vr::DriverPose_t CreatePose(double x, double y, double z);

    // Create pose with position and rotation (euler angles in radians)
    static vr::DriverPose_t CreatePose(double x, double y, double z,
                                       double yaw, double pitch, double roll);

    // Euler angles to quaternion
    static void EulerToQuaternion(double yaw, double pitch, double roll,
                                  double& qw, double& qx, double& qy, double& qz);

    // Quaternion to euler angles
    static void QuaternionToEuler(double qw, double qx, double qy, double qz,
                                  double& yaw, double& pitch, double& roll);

    // Create identity quaternion
    static vr::HmdQuaternion_t IdentityQuaternion();

    // Degrees to radians
    static inline double DegToRad(double degrees) { return degrees * M_PI / 180.0; }

    // Radians to degrees
    static inline double RadToDeg(double radians) { return radians * 180.0 / M_PI; }
};

} // namespace vremulator
