# VR Headset Emulator Research

## Project Goal

Create a VR headset emulator that allows development and testing of SkyrimVR content without requiring physical VR hardware. The emulator should work with both SteamVR and OpenComposite runtimes.

---

## Research Summary

### SteamVR Null Driver Approach

SteamVR includes a built-in "null" driver that can run without physical VR hardware. This is the quickest path to basic functionality.

**Key Features:**
- Creates a borderless window in the center of the main monitor
- Allows SteamVR applications to run without physical headset
- Configurable display and render parameters
- Minimal setup required

**Configuration Requirements:**
1. Enable null driver in `Steam/steamapps/common/SteamVR/drivers/null/resources/settings/default.vrsettings`
2. Modify global SteamVR settings to allow running without HMD
3. Enable multiple driver support for flexibility

**Limitations:**
- Basic pose tracking (fixed position or limited movement)
- No real motion control emulation out of the box
- Primarily for testing, not full development workflow

### Custom OpenVR Driver Approach

For full control and advanced features, a custom OpenVR driver can be developed in C++.

**Architecture Components:**

1. **IServerTrackedDeviceProvider**
   - Entry point for the driver
   - Manages lifecycle of tracked devices
   - Returns device instances to SteamVR runtime

2. **ITrackedDeviceServerDriver**
   - Represents individual tracked devices (HMD, controllers, trackers)
   - Provides pose updates to SteamVR
   - Handles device properties and configuration

3. **IVRServerDriverHost**
   - Interface to communicate with SteamVR runtime
   - Used to add/remove devices
   - Poll for events from runtime

**Implementation Pattern:**
```cpp
// Driver exports HMDDriverFactory function
// Returns IServerTrackedDeviceProvider implementation
// Provider creates ITrackedDeviceServerDriver instances
// Each device sends pose updates via IVRServerDriverHost
```

**Advantages:**
- Complete control over device behavior
- Can simulate realistic tracking data
- Support for multiple devices (HMD + controllers)
- Custom input simulation
- Programmatic control for automated testing

**Development Requirements:**
- C++ compiler (Visual Studio 2019+ recommended)
- OpenVR SDK headers
- Understanding of 3D math (quaternions, matrices)
- Knowledge of COM-style interfaces

---

## Implementation Approaches

### Approach 1: Quick Start - Null Driver Configuration (Phase 1)

**Time Estimate:** 1-2 hours  
**Complexity:** Low  
**Use Case:** Basic testing, initial SkyrimVR development

**Steps:**
1. Create configuration utility to enable null driver
2. Provide scripts to toggle null driver on/off
3. Document configuration for SkyrimVR launch
4. Test basic SteamVR functionality

**Deliverables:**
- Configuration script
- Documentation
- Verified SteamVR startup without headset

### Approach 2: Custom Virtual HMD Driver (Phase 2-3)

**Time Estimate:** 2-4 weeks  
**Complexity:** High  
**Use Case:** Full development workflow, automated testing

**Steps:**
1. Set up OpenVR driver project structure
2. Implement basic driver skeleton (provider + HMD device)
3. Add pose tracking with keyboard/mouse control
4. Implement display configuration matching SkyrimVR requirements
5. Add controller emulation
6. Create input simulation layer
7. Build testing and debugging tools

**Deliverables:**
- Custom OpenVR driver DLL
- Control interface (GUI or CLI)
- Input simulation tools
- Complete documentation

### Approach 3: Hybrid Solution (Recommended)

**Time Estimate:** 1 week initial + ongoing enhancement  
**Complexity:** Medium  
**Use Case:** Balanced approach for immediate needs + future flexibility

**Phase 1 - Immediate (1-2 days):**
- Configure null driver for quick testing
- Create launch scripts for SkyrimVR
- Document basic workflow

**Phase 2 - Enhanced (1 week):**
- Develop custom driver with pose control
- Add keyboard/mouse input mapping
- Implement basic controller emulation

**Phase 3 - Advanced (ongoing):**
- Add motion simulation
- Create automated testing framework
- Build visualization tools

---

## Technical Requirements

### For Null Driver Configuration
- SteamVR installed
- Text editor or JSON configuration tool
- Windows batch scripts for automation

### For Custom Driver Development
- **Build Tools:**
  - Visual Studio 2019 or later
  - C++17 support
  - CMake (optional, for build configuration)

- **Libraries:**
  - OpenVR SDK (headers + lib)
  - GLM or similar math library (for quaternions/matrices)
  - Optional: Dear ImGui for control interface

- **Runtime:**
  - SteamVR
  - Windows 10/11

### For SkyrimVR Integration
- SkyrimVR installed
- Understanding of SkyrimVR's VR requirements
- Ability to launch with custom launch options

---

## Key Resources & References

### Official Documentation
- [OpenVR GitHub Repository](https://github.com/ValveSoftware/openvr)
- [OpenVR Driver API Documentation](https://github.com/ValveSoftware/openvr/blob/master/docs/Driver_API_Documentation.md)
- [OpenVR Driver Documentation Wiki](https://github.com/ValveSoftware/openvr/wiki/Driver-Documentation)
- [ITrackedDeviceServerDriver Overview](https://github.com/ValveSoftware/openvr/wiki/vr::ITrackedDeviceServerDriver-Overview)

### Sample Code & Tutorials
- [Simple OpenVR Driver Tutorial](https://github.com/terminal29/Simple-OpenVR-Driver-Tutorial) - Excellent starting point
- [VirtualHMD_OpenVR](https://github.com/xiaofeiyu0723/VirtualHMD_OpenVR) - Complete virtual HMD implementation
- [OpenVR SimpleHMD Sample](https://github.com/ValveSoftware/openvr/tree/master/samples/drivers/drivers/simplehmd)
- [OpenVR Driver for DIY](https://github.com/r57zone/OpenVR-driver-for-DIY)
- [Custom HMD Implementation](https://github.com/sencercoltu/openvr-customhmd)

### Configuration Resources
- [Enable SteamVR Null Driver](https://gist.github.com/Adamcbrz/aadc8f613e596d6d503b007afd28fb73)
- [SteamVR No Headset Setup](https://github.com/username223/SteamVRNoHeadset)
- [VR Software Wiki - Null Driver Tutorial](https://www.vrwiki.cs.brown.edu/hardware/vr-hardware/hardware-emulators/null-driver-tutorial)
- [steamvr.vrsettings Documentation](https://developer.valvesoftware.com/wiki/SteamVR/steamvr.vrsettings)

### Community & Tools
- [OpenVR Input Emulator](https://github.com/matzman666/OpenVR-InputEmulator) - Controller emulation reference
- [OpenVR Driver Topics](https://github.com/topics/openvr-driver) - Collection of driver projects

### OpenComposite Information
- [OpenComposite](https://gitlab.com/znixian/OpenOVR) - OpenXR to OpenVR compatibility layer
- [OpenComposite for SkyrimVR](https://www.nexusmods.com/skyrimspecialedition/mods/171182)
- [OpenComposite Linux Wiki](https://wiki.vronlinux.org/docs/fossvr/opencomposite/)

**Note:** OpenComposite is primarily for allowing OpenXR headsets to run SteamVR games. For emulation purposes, a custom OpenVR driver is more appropriate.

---

## SkyrimVR Specific Considerations

### VR Requirements
- 6DOF tracking (position + rotation)
- Dual eye rendering
- Motion controller support
- Minimum recommended IPD handling

### Testing Approach
1. **Basic Launch Test:** Verify SkyrimVR starts with null driver
2. **Menu Navigation:** Test in-game VR menus
3. **Movement:** Verify movement controls work
4. **Interaction:** Test object interaction
5. **Combat:** Verify weapon mechanics

### Known Issues
- SkyrimVR requires actual HMD connection by default
- Some VR features may not work with basic emulation
- Input remapping may be necessary for keyboard/mouse control

### Recommended Development Workflow
1. Use null driver for initial testing and debugging
2. Develop mod features in flat Skyrim SE when possible
3. Test VR-specific features with emulator
4. Final testing with physical headset (if available)

---

## Next Steps

### Immediate (Phase 1)
1. ✅ Complete research documentation
2. Set up null driver configuration
3. Create SteamVR launch scripts
4. Test with SkyrimVR
5. Document basic workflow

### Short-term (Phase 2)
1. Set up C++ development environment
2. Create custom driver project structure
3. Implement basic HMD driver
4. Add pose tracking control
5. Test with SkyrimVR

### Long-term (Phase 3+)
1. Enhanced controller emulation
2. Motion simulation
3. Automated testing framework
4. Integration with mod development tools
5. Performance profiling tools

---

## Conclusion

A hybrid approach is recommended:
- **Start with null driver** for immediate SkyrimVR testing capability
- **Build custom driver** incrementally for enhanced control and features
- **Iterate based on development needs** - add features as required

This provides quick wins while building toward a comprehensive development environment.

---

**Last Updated:** 2026-08-13  
**Author:** VREmulator Development Team  
**Status:** Research Complete, Ready for Implementation
