# TICKET-0031

**Status**

Open

**Type**

Feature

**Category**

features

**Priority**

Medium

**Created**

2026-08-13

**Parent Ticket**

TICKET-0027 (VR Headset Emulator Implementation)

**Dependencies**

* TICKET-0030 (Virtual HMD) - Must complete first (controllers need HMD)

**Blocks**

* TICKET-0032 (Input System)

---

## Description

Implement virtual motion controllers (left and right) that appear in SteamVR alongside the HMD. Controllers need:
- Pose tracking (position + rotation)
- Button and trigger states
- Touchpad/thumbstick emulation
- Haptic feedback simulation (can be no-op)

---

## Reason

Many VR applications (including SkyrimVR) require controllers for interaction. Without controller emulation, testing is severely limited.

---

## Implementation Plan

### 1. Create Controller Device Class
* [ ] Implement ITrackedDeviceServerDriver for controllers
* [ ] Support left and right hand variants
* [ ] Add device role assignment
* [ ] Implement activation/deactivation

### 2. Implement Controller Pose
* [ ] Position relative to HMD
* [ ] Rotation tracking
* [ ] Velocity/acceleration (can be zero initially)
* [ ] Pose update system

### 3. Implement Input Components
* [ ] IVRControllerComponent interface
* [ ] Button states (trigger, grip, menu, system)
* [ ] Analog inputs (trigger value, touchpad/thumbstick)
* [ ] Touchpad touch/click detection

### 4. Configure Controller Properties
* [ ] Set controller type (Vive wand, Index, Touch, etc.)
* [ ] Set render model
* [ ] Configure input profile
* [ ] Set axis mappings

### 5. Add Input State Management
* [ ] Create input state structure
* [ ] Implement state update method
* [ ] Send input events to SteamVR
* [ ] Handle button press/release/touch

### 6. Register Controllers with Runtime
* [ ] Add both controllers in provider
* [ ] Set proper device class (Controller)
* [ ] Set hand role (left/right)

### 7. Test Controller Recognition
* [ ] Verify controllers appear in SteamVR
* [ ] Check controller models visible
* [ ] Test button inputs register
* [ ] Verify analog inputs work

---

## Files Modified

### New Files
* `src/driver/devices/controller/virtual_controller_device.h`
* `src/driver/devices/controller/virtual_controller_device.cpp`
* `src/driver/devices/controller/controller_component.h`
* `src/driver/devices/controller/controller_component.cpp`

### Modified Files
* `src/driver/provider/server_provider.cpp` - Add controller creation
* `docs/BUILD_INSTRUCTIONS.md` - Update testing steps
* `CHANGELOG.md` - Document controller implementation

---

## Testing

### Integration Testing
1. **Controller Recognition:**
   - Both controllers appear in SteamVR
   - Correct hand assignment (left/right)
   - Controller models visible in VR view

2. **Pose Testing:**
   - Controllers positioned correctly relative to HMD
   - Rotation updates work
   - No tracking loss

3. **Input Testing:**
   - Trigger pull registers
   - Grip button works
   - Menu/system buttons functional
   - Touchpad/thumbstick input detected
   - Analog values accurate

4. **SkyrimVR Testing:**
   - Weapon grab works
   - Menu navigation functional
   - Spell casting triggers
   - Movement input recognized

---

## Result

*To be filled upon completion*

---

## Notes

### Controller Types to Support

**Initial target:** Vive-style controllers (simplest)
**Future:** Index controllers, Oculus Touch (better SkyrimVR compatibility)

### Input Components

```cpp
vr::VRControllerState_t {
    uint64_t ulButtonPressed;    // Bitmask of pressed buttons
    uint64_t ulButtonTouched;    // Bitmask of touched buttons
    vr::VRControllerAxis_t rAxis[5];  // Analog axes
}
```

### Button Mappings

- Button 0: System button
- Button 1: Application menu
- Button 2: Grip button
- Axis 1: Trigger (0.0 - 1.0)
- Axis 0: Touchpad/Thumbstick (X, Y: -1.0 to 1.0)

### Default Controller Positions

Relative to HMD:
- Left: (-0.2, -0.2, -0.3) - left, slightly down, forward
- Right: (0.2, -0.2, -0.3) - right, slightly down, forward

### References
- [OpenVR Input System](https://github.com/ValveSoftware/openvr/wiki/Input-Profiles)
- [Controller Component](https://github.com/terminal29/Simple-OpenVR-Driver-Tutorial)

### Expected Time
**Estimated:** 8-12 hours
- 3-4 hours: Device implementation
- 2-3 hours: Input component
- 2-3 hours: Testing
- 1-2 hours: SkyrimVR integration testing

---

## Closed

*Open - blocked by TICKET-0030*
