# TICKET-0030

**Status**

Open

**Type**

Feature

**Category**

features

**Priority**

High

**Created**

2026-08-13

**Parent Ticket**

TICKET-0027 (VR Headset Emulator Implementation)

**Dependencies**

* TICKET-0029 (Custom Driver Project Setup) - Must complete first

**Blocks**

* TICKET-0031 (Virtual Controllers)
* TICKET-0032 (Input System)

---

## Description

Implement a virtual HMD (Head-Mounted Display) device that appears in SteamVR as a real headset. This involves:
- Implementing ITrackedDeviceServerDriver for HMD
- Providing pose tracking data
- Configuring display properties
- Registering device with SteamVR runtime

---

## Reason

Core functionality - without a virtual HMD, there's no VR emulation. The HMD is the primary tracked device that SteamVR requires.

---

## Implementation Plan

### 1. Create HMD Device Class
* [ ] Implement ITrackedDeviceServerDriver interface
* [ ] Add device activation/deactivation
* [ ] Set up device serial number and model
* [ ] Implement GetPose method

### 2. Configure Display Properties
* [ ] Set IPD (interpupillary distance)
* [ ] Configure field of view
* [ ] Set render target size
* [ ] Set display frequency
* [ ] Configure lens distortion (can be identity for emulator)

### 3. Implement Pose Tracking
* [ ] Create initial pose (origin position)
* [ ] Implement pose update system
* [ ] Add timestamp tracking
* [ ] Ensure pose validity flags correct

**Initial pose:** Fixed at origin (0, 1.6, 0) - standing height

### 4. Register Device with Runtime
* [ ] Use IVRServerDriverHost::TrackedDeviceAdded
* [ ] Pass device class as HMD
* [ ] Provide device driver interface
* [ ] Handle activation callback

### 5. Implement Property System
* [ ] Set device properties (name, manufacturer, etc.)
* [ ] Configure tracking system name
* [ ] Set render model name
* [ ] Add icons for device

### 6. Add Logging and Debugging
* [ ] Log device lifecycle events
* [ ] Log pose updates
* [ ] Add debug output for troubleshooting

### 7. Test HMD Recognition
* [ ] Build and copy driver
* [ ] Start SteamVR
* [ ] Verify HMD appears in SteamVR
* [ ] Check SteamVR dashboard shows device
* [ ] Verify pose updates in SteamVR

---

## Files Modified

### New Files
* `src/driver/devices/hmd/virtual_hmd_device.h`
* `src/driver/devices/hmd/virtual_hmd_device.cpp`
* `src/driver/utils/math_utils.h` - Pose calculation helpers
* `src/driver/utils/math_utils.cpp`

### Modified Files
* `src/driver/provider/server_provider.cpp` - Add HMD device creation
* `docs/BUILD_INSTRUCTIONS.md` - Update with testing steps
* `CHANGELOG.md` - Document HMD implementation

---

## Testing

### Unit Testing
1. Device instantiation succeeds
2. Properties set correctly
3. Pose calculations valid
4. Activation/deactivation works

### Integration Testing
1. **SteamVR Recognition:**
   - Driver loads without errors
   - HMD appears in device list
   - Device shown in SteamVR status window
   - Headset icon visible

2. **Pose Testing:**
   - Initial pose at correct position
   - Pose updates without crashes
   - Timestamps incrementing correctly
   - No jitter or invalid states

3. **Display Testing:**
   - Compositor creates render targets
   - SteamVR window shows dual eye view
   - No rendering errors in logs

---

## Result

*To be filled upon completion*

---

## Notes

### Key Interfaces

**ITrackedDeviceServerDriver methods to implement:**
- `Activate(uint32_t unObjectId)` - Device initialization
- `Deactivate()` - Cleanup
- `EnterStandby()` / `ExitStandby()` - Power management (can be no-op)
- `GetComponent(const char *pchComponentNameAndVersion)` - Component queries
- `DebugRequest(...)` - Debug commands (optional)

### Pose Structure

```cpp
vr::DriverPose_t pose = {0};
pose.qWorldFromDriverRotation = {1, 0, 0, 0}; // Identity quaternion
pose.qDriverFromHeadRotation = {1, 0, 0, 0};
pose.vecPosition[0] = 0;     // X
pose.vecPosition[1] = 1.6;   // Y (standing height)
pose.vecPosition[2] = 0;     // Z
pose.poseIsValid = true;
pose.deviceIsConnected = true;
pose.result = vr::TrackingResult_Running_OK;
```

### Display Configuration

Recommended initial values for SkyrimVR:
- Render width: 2016 per eye (Vive resolution)
- Render height: 2240
- Refresh rate: 90 Hz
- FOV: ~110 degrees
- IPD: 0.063m (63mm)

### References
- [ITrackedDeviceServerDriver Overview](https://github.com/ValveSoftware/openvr/wiki/vr::ITrackedDeviceServerDriver-Overview)
- [SimpleHMD Sample](https://github.com/ValveSoftware/openvr/tree/master/samples/drivers/drivers/simplehmd)

### Expected Time
**Estimated:** 8-16 hours
- 3-4 hours: Interface implementation
- 2-3 hours: Property configuration
- 2-3 hours: Pose system
- 1-2 hours: Testing
- 2-4 hours: Debugging and refinement

---

## Closed

*Open - blocked by TICKET-0029*
