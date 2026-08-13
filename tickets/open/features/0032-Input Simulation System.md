# TICKET-0032

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

* TICKET-0030 (Virtual HMD) - Need HMD for pose control
* TICKET-0031 (Virtual Controllers) - Need controllers for button mapping

**Blocks**

* TICKET-0033 (SkyrimVR Integration)

---

## Description

Create a system that maps keyboard and mouse inputs to VR poses and controller actions. This is what makes the emulator actually usable - turning standard PC input into VR headset and controller movements.

**Key features:**
- Keyboard/mouse → HMD pose (WASD + mouse look)
- Keyboard → controller buttons (number keys, letters)
- Mouse buttons → trigger/grip
- Configuration system for custom mappings
- Optional GUI for visual control

---

## Reason

Without input simulation, the virtual HMD and controllers are static and unusable. This system bridges the gap between PC input and VR interaction, making development actually possible.

---

## Implementation Plan

### 1. Create Input Handler
* [ ] Set up Windows input capture (keyboard/mouse)
* [ ] Create input event processing loop
* [ ] Implement key state tracking
* [ ] Handle mouse movement/buttons

### 2. Implement HMD Pose Control
* [ ] Mouse look → HMD rotation
* [ ] WASD → HMD position translation
* [ ] Mouse sensitivity configuration
* [ ] Movement speed configuration
* [ ] Optional: Q/E for roll (rarely needed)

**Default mapping:**
- Mouse X/Y → Head rotation (yaw/pitch)
- W/A/S/D → Forward/Left/Back/Right
- Space/Ctrl → Up/Down
- Shift → Movement speed modifier

### 3. Implement Controller Input Mapping
* [ ] Number keys → controller buttons
* [ ] Mouse buttons → trigger/grip
* [ ] Arrow keys → thumbstick/touchpad
* [ ] Tab → switch active controller
* [ ] Handle simultaneous inputs

**Default mapping:**
- Left Click → Right trigger
- Right Click → Right grip
- Middle Click → Left trigger
- Mouse 4 → Left grip
- 1-6 → Controller face buttons
- Arrow keys → Touchpad/thumbstick

### 4. Create Configuration System
* [ ] Load/save input mappings from JSON
* [ ] Validate configurations
* [ ] Provide default configuration
* [ ] Allow runtime reconfiguration

### 5. Implement IPC Communication
* [ ] Create shared memory or named pipe
* [ ] Send input data from handler to driver
* [ ] Ensure thread safety
* [ ] Handle connection/disconnection

**Why IPC needed:** Input handler runs in separate process from driver (driver loaded by SteamVR)

### 6. Create Control Interface (CLI)
* [ ] Command-line tool to start/stop input handler
* [ ] Show current input state
* [ ] Allow mapping changes at runtime
* [ ] Display help/key bindings

### 7. Optional: Create GUI
* [ ] Visual pose control panel
* [ ] Button mapping editor
* [ ] Real-time state display
* [ ] Preset manager

**Note:** Can be deferred to later if CLI is sufficient

### 8. Test Input System
* [ ] Verify keyboard/mouse input captured
* [ ] Check HMD pose updates in SteamVR
* [ ] Verify controller buttons trigger
* [ ] Test in SteamVR dashboard
* [ ] Test with simple VR application

---

## Files Modified

### New Files
* `src/control/input_handler.h`
* `src/control/input_handler.cpp`
* `src/control/input_config.h`
* `src/control/input_config.cpp`
* `src/control/ipc/shared_memory.h`
* `src/control/ipc/shared_memory.cpp`
* `src/control/main.cpp` - Input handler executable
* `config/custom_driver/input_mappings.json` - Default mappings
* `docs/INPUT_MAPPING.md` - Mapping documentation

### Modified Files
* `src/driver/devices/hmd/virtual_hmd_device.cpp` - Read pose from IPC
* `src/driver/devices/controller/virtual_controller_device.cpp` - Read inputs from IPC
* `scripts/run_emulator.bat` - Launch input handler + SteamVR
* `README.md` - Add usage instructions
* `CHANGELOG.md` - Document input system

---

## Testing

### Unit Testing
1. Input capture works
2. Configuration loading/saving
3. IPC communication reliable
4. Mapping validation

### Integration Testing
1. **HMD Control:**
   - Mouse moves head rotation
   - WASD moves position
   - Movement smooth (no jitter)
   - Sensitivity appropriate

2. **Controller Input:**
   - Button presses registered
   - Trigger analog values correct
   - Touchpad/thumbstick input accurate
   - No input loss or delays

3. **Configuration:**
   - Custom mappings load correctly
   - Runtime changes apply immediately
   - Invalid configs rejected gracefully

4. **Multi-input:**
   - Simultaneous key presses work
   - No conflicts between mappings
   - State tracking accurate

---

## Result

*To be filled upon completion*

---

## Notes

### Input Capture Options

**Windows API:** GetAsyncKeyState, Raw Input API
**Advantage:** Direct, no dependencies
**Disadvantage:** Requires window message loop

**SDL2 (alternative):**
**Advantage:** Cross-platform, easier input handling
**Disadvantage:** Extra dependency

**Recommendation:** Start with Windows API, consider SDL2 if porting to Linux

### IPC Options

1. **Shared Memory** - Fastest, good for high-frequency pose updates
2. **Named Pipes** - More structured, easier debugging
3. **UDP Localhost** - Simplest, might have latency

**Recommendation:** Shared memory for pose, named pipe for configuration

### Configuration Format

```json
{
  "hmd": {
    "mouse_sensitivity": 0.002,
    "movement_speed": 1.0,
    "rotation": {
      "mouse_x": "yaw",
      "mouse_y": "pitch"
    },
    "position": {
      "w": "forward",
      "a": "left",
      "s": "back",
      "d": "right",
      "space": "up",
      "ctrl": "down"
    }
  },
  "controllers": {
    "left": {
      "trigger": "mouse4",
      "grip": "shift",
      "menu": "tab"
    },
    "right": {
      "trigger": "mouse1",
      "grip": "mouse2",
      "menu": "m"
    }
  }
}
```

### References
- [VirtualHMD_OpenVR](https://github.com/xiaofeiyu0723/VirtualHMD_OpenVR) - Has input control examples
- [OpenVR Input Emulator](https://github.com/matzman666/OpenVR-InputEmulator)

### Expected Time
**Estimated:** 12-20 hours
- 4-6 hours: Input handling implementation
- 3-4 hours: IPC system
- 2-3 hours: Configuration system
- 2-3 hours: Driver integration
- 1-2 hours: CLI tool
- 2-4 hours: Testing and refinement

---

## Closed

*Open - blocked by TICKET-0030, TICKET-0031*
