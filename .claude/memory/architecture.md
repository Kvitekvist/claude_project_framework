# Project Architecture

## Overview

VREmulator uses a **hybrid architecture** combining SteamVR's built-in null driver for quick testing with a custom OpenVR driver for advanced functionality.

**Design Philosophy:**
- **Phased implementation** - Start simple (null driver), build toward comprehensive (custom driver)
- **Modular components** - Each piece (HMD, controllers, input) can be developed/tested independently
- **Developer-first** - Optimized for SkyrimVR mod development workflow
- **Extensible** - Easy to add new features and device types

---

## Components

### Phase 1: Null Driver Configuration (Immediate)

**Purpose:** Quick-start VR emulation using SteamVR's built-in capabilities

**Components:**
- **Configuration Manager** - Scripts to enable/disable null driver
- **Settings Templates** - Pre-configured steamvr.vrsettings files
- **Launch Scripts** - Easy SkyrimVR startup with null driver
- **Documentation** - Setup guide and troubleshooting

**Technologies:** Batch scripts, JSON configuration files

### Phase 2-3: Custom OpenVR Driver (Advanced)

**Purpose:** Full-featured virtual HMD with complete control

**Components:**

#### 1. Driver Core (C++ DLL)
- **IServerTrackedDeviceProvider Implementation**
  - Driver lifecycle management
  - Device registration with SteamVR
  - Event handling
  
- **VirtualHMDDevice (ITrackedDeviceServerDriver)**
  - Simulated HMD hardware
  - Pose tracking updates
  - Display configuration
  - Device properties

- **VirtualControllerDevice (ITrackedDeviceServerDriver)**
  - Simulated motion controllers (left/right)
  - Button/trigger states
  - Pose tracking
  - Haptic feedback simulation

#### 2. Control Interface
- **Input Handler** - Keyboard/mouse to VR pose/input mapping
- **Configuration System** - Runtime adjustable settings
- **IPC Layer** - Communication between driver and control app

#### 3. Control Application (Optional GUI)
- **Pose Control Panel** - Visual HMD position/rotation control
- **Input Mapper** - Configure keyboard/mouse bindings
- **Debug Visualizer** - Show current tracking state
- **Preset Manager** - Save/load test scenarios

#### 4. Integration Tools
- **SkyrimVR Launcher** - One-click launch with emulator
- **Testing Utilities** - Automated test scenarios
- **Performance Monitor** - VR-specific metrics

---

## Folder Responsibilities

### Ticket System Structure

Tickets are organized in category-based subfolders for scalability:

```
tickets/
├── open/
│   ├── features/       # New functionality, enhancements
│   ├── bugs/           # Bug fixes, defects
│   ├── documentation/  # Docs, comments, guides
│   ├── infrastructure/ # Build, CI/CD, tooling
│   └── research/       # Investigation, analysis
├── closed/             # Same structure
└── archived/           # Same structure
```

See `docs/TICKET_CATEGORIES.md` for detailed category guidance.

### Source Code Structure (Planned)

```
src/
├── driver/                    # Custom OpenVR driver (C++)
│   ├── provider/             # IServerTrackedDeviceProvider
│   ├── devices/              # Device implementations
│   │   ├── hmd/             # Virtual HMD
│   │   └── controllers/     # Virtual controllers
│   ├── utils/               # Math, logging, helpers
│   └── exports.cpp          # Driver entry points
│
├── control/                  # Control application
│   ├── input/               # Input handling
│   ├── ipc/                 # Inter-process communication
│   └── config/              # Configuration management
│
├── gui/ (optional)          # Control interface GUI
│   └── imgui/               # Dear ImGui integration
│
└── integration/             # SkyrimVR integration tools
    ├── launcher/            # Custom launcher
    └── testing/             # Test utilities
```

### Configuration Structure

```
config/
├── null_driver/             # Null driver configurations
│   ├── enable_null.bat
│   ├── disable_null.bat
│   └── templates/          # steamvr.vrsettings templates
│
└── custom_driver/          # Custom driver settings
    ├── driver_config.json
    └── input_mappings.json
```

### Build Output

```
build/
├── driver/                  # Compiled driver DLL
│   └── vremulator/         # Driver folder for SteamVR
│       ├── bin/
│       │   └── win64/
│       │       └── driver_vremulator.dll
│       └── resources/
│           └── driver.vrdrivermanifest
│
└── control/                # Control application
    └── vr_control.exe
```

### Other Folders

- **`docs/`** - Research, API documentation, guides
- **`scripts/`** - Build scripts, installation helpers
- **`tests/`** - Unit tests, integration tests
- **`assets/`** - Icons, models for GUI
- **`releases/`** - Packaged releases for distribution

---

## Dependencies

### Phase 1: Null Driver (Minimal)
- **SteamVR** - VR runtime (user must install)
- **Windows batch scripts** - Configuration automation

### Phase 2-3: Custom Driver

#### Build Dependencies
- **Visual Studio 2019+** - C++ compiler and toolchain
- **CMake 3.15+** - Build system (optional)
- **OpenVR SDK** - Headers and interface definitions
  - Repo: https://github.com/ValveSoftware/openvr
  - Why: Required for driver development

#### Runtime Dependencies
- **SteamVR** - VR runtime and driver host
- **Windows 10/11** - Target platform

#### Code Libraries
- **GLM (OpenGL Mathematics)** - Header-only math library
  - Why: Quaternion/matrix operations for pose calculations
  - Alternative: DirectXMath

- **nlohmann/json** (optional) - JSON parsing
  - Why: Configuration file handling
  - Alternative: Built-in Windows JSON (jsoncpp)

- **Dear ImGui** (optional) - GUI library
  - Why: Control interface if GUI is implemented
  - Note: Only needed for GUI version

#### Development Dependencies
- **Google Test** (optional) - Unit testing framework
  - Why: Test pose calculations and driver logic

---

## Design Principles

### 1. Phased Complexity
Start with simplest working solution (null driver), incrementally add features. Each phase must be independently functional.

### 2. Separation of Concerns
- **Driver layer** - Pure OpenVR driver, no direct user interaction
- **Control layer** - Input handling, configuration, user interface
- **Integration layer** - SkyrimVR-specific tools

### 3. Minimal SteamVR Invasiveness
- Use driver system properly (no hacking SteamVR internals)
- Easy to enable/disable
- No permanent modifications to SteamVR installation

### 4. Developer-Centric Design
- Optimize for SkyrimVR mod development workflow
- Fast iteration cycle (start emulator → test → stop)
- Good defaults, easy customization

### 5. Extensibility
- Easy to add new device types (trackers, etc.)
- Pluggable input systems
- Configuration-driven where possible

### 6. Platform Awareness
- Windows-first (SkyrimVR requirement)
- 64-bit architecture (SteamVR requirement)
- Consider future Linux/Proton support (low priority)

---

## Future Improvements

### Near-term
- **Hot-reload configuration** - Change settings without restarting SteamVR
- **Multiple device profiles** - Quick switch between HMD types (Vive, Index, Quest)
- **Input recording/playback** - Record VR sessions, replay for testing

### Mid-term
- **Network control** - Control emulator from phone/tablet
- **Room-scale simulation** - Virtual playspace boundaries
- **Advanced motion** - Head bobbing, natural movement patterns

### Long-term
- **Multi-HMD simulation** - Test asymmetric/spectator VR
- **Performance testing framework** - Automated VR performance benchmarks
- **Cross-runtime support** - OpenXR native implementation
- **Hardware-in-the-loop** - Mix real and virtual devices

### Integration Enhancements
- **SKSE plugin** - Control from Skyrim console commands
- **Mod Organizer 2 integration** - Launch from MO2
- **Visual test editor** - Record test scenarios visually
- **CI/CD integration** - Automated mod testing in emulator
