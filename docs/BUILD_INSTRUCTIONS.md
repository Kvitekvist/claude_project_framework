# Build Instructions

## Prerequisites

### Required Tools
- **Visual Studio 2019 or later** with C++ development workload
- **CMake 3.15 or later**
- **Git** (for cloning OpenVR SDK)

### Verify Installation

```powershell
# Check Visual Studio
where cl

# Check CMake
cmake --version

# Check Git
git --version
```

---

## Quick Build

### Option 1: Using Build Script (Recommended)

1. Open terminal in project root
2. Run build script:
```batch
.\scripts\build.bat
```

The script will:
- Find Visual Studio installation
- Configure CMake project
- Build driver DLL
- Output to `build/driver/vremulator/bin/win64/`

### Option 2: Manual CMake Build

```batch
# Create build directory
mkdir build\cmake
cd build\cmake

# Configure
cmake ..\..\src\driver -G "Visual Studio 17 2022" -A x64

# Build
cmake --build . --config Release

# Output: build\driver\vremulator\bin\win64\driver_vremulator.dll
```

---

## Project Structure

```
VREmulator/
├── src/
│   ├── openvr_sdk/          # OpenVR SDK (cloned from GitHub)
│   └── driver/
│       ├── exports.cpp       # DLL entry point
│       ├── provider/         # IServerTrackedDeviceProvider
│       ├── devices/          # HMD and controller implementations
│       │   ├── hmd/         # (TODO: TICKET-0030)
│       │   └── controller/  # (TODO: TICKET-0031)
│       └── utils/           # Logger and helpers
│
├── build/
│   ├── cmake/               # CMake build files
│   └── driver/vremulator/   # Driver output
│       ├── bin/win64/
│       │   ├── driver_vremulator.dll
│       │   └── openvr_api.dll
│       └── resources/
│           └── driver.vrdrivermanifest
│
└── scripts/
    ├── build.bat            # Build script
    └── clean.bat            # Clean build
```

---

## Build Configurations

### Release Build (Default)
```batch
cmake --build . --config Release
```

Optimized for performance, smaller size.

### Debug Build
```batch
cmake --build . --config Debug
```

Includes debug symbols, useful for development.

---

## Testing the Driver

### Step 1: Install Driver to SteamVR

**Option A: Manual Copy**

1. Locate SteamVR drivers directory:
```
C:\Program Files (x86)\Steam\steamapps\common\SteamVR\drivers\
```

2. Copy entire `vremulator` folder:
```batch
copy /Y build\driver\vremulator "C:\Program Files (x86)\Steam\steamapps\common\SteamVR\drivers\"
```

**Option B: Symlink (for development)**

```batch
# Run as Administrator
mklink /D "C:\Program Files (x86)\Steam\steamapps\common\SteamVR\drivers\vremulator" "%CD%\build\driver\vremulator"
```

Benefits:
- No copying needed after rebuild
- Changes reflect immediately
- Easier development workflow

### Step 2: Verify Driver Loading

1. Start SteamVR
2. Check SteamVR log for driver loading:

**Log location:**
```
C:\Program Files (x86)\Steam\steamapps\common\SteamVR\logs\vrserver.txt
```

**Look for:**
```
Loaded driver vremulator
Driver vremulator initialized successfully
```

**Also check driver log:**
```
build\driver\vremulator\bin\win64\driver_vremulator.log
```

### Step 3: Expected Behavior (Current Phase)

**What works now:**
- ✅ Driver loads without errors
- ✅ Logger writes to file
- ✅ Provider initializes

**What doesn't work yet:**
- ❌ No HMD device (TICKET-0030)
- ❌ No controllers (TICKET-0031)
- ❌ No input simulation (TICKET-0032)

**This is expected!** The driver skeleton is complete, but devices will be added in later tickets.

---

## Troubleshooting

### CMake Configuration Fails

**Error:** `CMake could not find OpenVR SDK`

**Solution:**
```batch
# Ensure OpenVR SDK is cloned
cd src
git clone https://github.com/ValveSoftware/openvr.git openvr_sdk
```

### Build Fails - Missing openvr_api.lib

**Error:** `LINK : fatal error LNK1104: cannot open file 'openvr_api.lib'`

**Solution:**

The OpenVR SDK includes pre-built libraries. Verify they exist:
```
src\openvr_sdk\lib\win64\openvr_api.lib
src\openvr_sdk\bin\win64\openvr_api.dll
```

If missing, re-clone OpenVR SDK or download from releases.

### Driver Not Loading in SteamVR

**Check 1: Manifest file exists**
```
build\driver\vremulator\resources\driver.vrdrivermanifest
```

**Check 2: DLL exists**
```
build\driver\vremulator\bin\win64\driver_vremulator.dll
```

**Check 3: Dependencies**

Use Dependency Walker or `dumpbin` to check DLL dependencies:
```batch
dumpbin /dependents build\driver\vremulator\bin\win64\driver_vremulator.dll
```

Should depend on:
- `openvr_api.dll` (included)
- Standard Windows DLLs

### Access Denied when Copying to SteamVR

**Solution:** Run command prompt as Administrator

### Driver Crashes SteamVR

**Check driver log:**
```
build\driver\vremulator\bin\win64\driver_vremulator.log
```

**Check SteamVR log:**
```
C:\Program Files (x86)\Steam\steamapps\common\SteamVR\logs\vrserver.txt
```

Look for error messages or crash dumps.

---

## Clean Build

To start fresh:

```batch
.\scripts\clean.bat
```

Or manually:
```batch
rmdir /s /q build\cmake
del /q build\driver\vremulator\bin\win64\driver_vremulator.*
```

---

## Development Workflow

### Recommended Iteration Cycle

1. **Make code changes**
2. **Rebuild:**
   ```batch
   .\scripts\build.bat
   ```

3. **Restart SteamVR** (if using symlink, no copy needed)

4. **Check logs:**
   ```batch
   type build\driver\vremulator\bin\win64\driver_vremulator.log
   ```

### Using Visual Studio IDE

1. Open CMake project:
   - File → Open → CMake
   - Select `src/driver/CMakeLists.txt`

2. Configure build settings:
   - Set configuration to Release or Debug
   - Ensure x64 architecture

3. Build: Ctrl+Shift+B

4. Output appears in `build/driver/vremulator/bin/win64/`

### Debugging with Visual Studio

1. Install driver to SteamVR (symlink recommended)

2. Set up debugging:
   - Project → Properties
   - Debugging → Command: `C:\Program Files (x86)\Steam\steamapps\common\SteamVR\bin\win64\vrserver.exe`

3. Set breakpoints in driver code

4. Start debugging (F5)

**Note:** SteamVR must not already be running

---

## Next Steps

### Phase 3: Virtual HMD (TICKET-0030)

After confirming driver builds and loads:
1. Implement `VirtualHMDDevice` class
2. Register HMD with SteamVR runtime
3. Provide pose updates
4. Configure display properties

### Phase 4: Virtual Controllers (TICKET-0031)

1. Implement `VirtualControllerDevice` class
2. Add button/trigger states
3. Register left/right controllers

### Phase 5: Input Simulation (TICKET-0032)

1. Create input handler application
2. Implement IPC communication
3. Map keyboard/mouse to VR inputs

---

## Additional Resources

- [OpenVR SDK Documentation](https://github.com/ValveSoftware/openvr/wiki)
- [CMake Documentation](https://cmake.org/documentation/)
- [Visual Studio C++ Documentation](https://docs.microsoft.com/en-us/cpp/)

---

**Last Updated:** 2026-08-13  
**Version:** 0.1.0  
**Ticket:** TICKET-0029
