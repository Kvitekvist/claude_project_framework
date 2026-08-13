# TICKET-0029

**Status**

Open

**Type**

Feature

**Category**

infrastructure

**Priority**

High

**Created**

2026-08-13

**Parent Ticket**

TICKET-0027 (VR Headset Emulator Implementation)

**Dependencies**

None - can start after research complete

**Blocks**

* TICKET-0030 (Virtual HMD)
* TICKET-0031 (Virtual Controllers)
* TICKET-0032 (Input System)

---

## Description

Set up the C++ development environment and project structure for building a custom OpenVR driver. This includes:
- Visual Studio project configuration
- OpenVR SDK integration
- Build system setup
- Basic driver skeleton implementation

---

## Reason

Provides the foundation for custom driver development (TICKET-0030+). Without proper project setup, development will be inefficient and error-prone.

---

## Implementation Plan

### 1. Install Development Tools
* [ ] Verify Visual Studio 2019+ installed with C++ workload
* [ ] Install CMake (if using CMake build system)
* [ ] Install Git (already have it)

### 2. Obtain OpenVR SDK
* [ ] Clone OpenVR repository
* [ ] Build OpenVR headers/libs (or use pre-built)
* [ ] Document SDK version used

**Repository:** https://github.com/ValveSoftware/openvr

### 3. Create Project Structure
* [ ] Create Visual Studio solution
* [ ] Set up driver project (DLL)
* [ ] Configure include paths
* [ ] Configure library paths
* [ ] Set output directory structure

**Directory structure:**
```
src/
├── driver/
│   ├── provider/       # IServerTrackedDeviceProvider
│   ├── devices/        # Device implementations
│   ├── utils/          # Helpers, math, logging
│   └── exports.cpp     # DLL entry points
```

### 4. Implement Driver Skeleton
* [ ] Create HMDDriverFactory export function
* [ ] Implement minimal IServerTrackedDeviceProvider
* [ ] Set up logging system
* [ ] Create driver manifest file

### 5. Configure Build Output
* [ ] Set up build output structure for SteamVR
* [ ] Create driver.vrdrivermanifest
* [ ] Configure post-build copy to SteamVR drivers folder (optional)

**Output structure:**
```
build/driver/vremulator/
├── bin/win64/
│   └── driver_vremulator.dll
└── resources/
    └── driver.vrdrivermanifest
```

### 6. Add Dependencies
* [ ] Add GLM for math (quaternions, matrices)
* [ ] Add JSON library (nlohmann/json or similar)
* [ ] Set up logging (spdlog or custom)

### 7. Create Build Scripts
* [ ] Create build.bat for command-line builds
* [ ] Add clean/rebuild scripts
* [ ] Document build process

### 8. Test Basic Build
* [ ] Build solution
* [ ] Verify DLL created
* [ ] Check exports with dumpbin
* [ ] Verify driver manifest structure

---

## Files Modified

### New Files
* `src/driver/provider/server_provider.h`
* `src/driver/provider/server_provider.cpp`
* `src/driver/exports.cpp`
* `src/driver/utils/logger.h`
* `src/driver/utils/logger.cpp`
* `VREmulator.sln` - Visual Studio solution
* `driver_vremulator.vcxproj` - Driver project
* `build/driver/vremulator/resources/driver.vrdrivermanifest`
* `scripts/build.bat`
* `scripts/clean.bat`
* `docs/BUILD_INSTRUCTIONS.md`

### Modified Files
* `README.md` - Add build instructions
* `.gitignore` - Add build artifacts
* `CHANGELOG.md` - Document setup complete

---

## Testing

### Build Testing
1. Clean build succeeds without errors
2. DLL exports correct symbols
3. Driver manifest is valid JSON
4. Output directory structure correct

### Integration Testing
1. Copy driver to SteamVR drivers folder
2. Start SteamVR
3. Check SteamVR logs for driver loading
4. Verify no errors (even though driver does nothing yet)

---

## Result

*To be filled upon completion*

---

## Notes

### Sample Implementations to Reference
- [Simple OpenVR Driver Tutorial](https://github.com/terminal29/Simple-OpenVR-Driver-Tutorial)
- [OpenVR Driver Sample](https://github.com/ValveSoftware/openvr/tree/master/samples/drivers)

### Driver Manifest Format
```json
{
    "alwaysActivate": false,
    "name": "vremulator",
    "directory": "",
    "resourceOnly": false
}
```

### Expected Time
**Estimated:** 4-8 hours
- 2-3 hours: Project setup and configuration
- 2-3 hours: Skeleton implementation
- 1-2 hours: Testing and documentation

---

## Closed

*Open - not started*
