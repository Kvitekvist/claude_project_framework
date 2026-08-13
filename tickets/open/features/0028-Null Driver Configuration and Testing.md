# TICKET-0028

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

None - can start immediately

**Blocks**

None (other tickets can proceed in parallel)

---

## Description

Configure SteamVR's built-in null driver to enable VR application testing without physical hardware. This is the quickest path to getting SkyrimVR running for development.

The null driver is already included with SteamVR but disabled by default. This ticket involves:
1. Creating configuration files to enable it
2. Building automation scripts
3. Testing with SteamVR
4. Verifying SkyrimVR compatibility

---

## Reason

Provides immediate value - developers can start testing SkyrimVR within hours rather than weeks. While the custom driver (later tickets) will provide more features, the null driver gives:

* **Fast time-to-value** - Working solution in 1-2 hours
* **Low risk** - Using built-in SteamVR feature
* **Learning opportunity** - Understand SteamVR configuration before building custom driver
* **Fallback option** - Always available if custom driver has issues

---

## Implementation Plan

### 1. Locate and Understand Null Driver
* [ ] Find null driver files in SteamVR installation
* [ ] Read existing configuration
* [ ] Document default settings

**Path:** `Steam/steamapps/common/SteamVR/drivers/null/resources/settings/default.vrsettings`

### 2. Create Configuration Templates
* [ ] Create enabled configuration template
* [ ] Create disabled configuration template
* [ ] Add configuration for display parameters
* [ ] Document each setting

**Settings to configure:**
- `enable: true`
- `serialNumber`
- `modelNumber`
- `windowWidth` / `windowHeight`
- `renderWidth` / `renderHeight`
- `displayFrequency`

### 3. Create Automation Scripts
* [ ] Create `enable_null_driver.bat`
* [ ] Create `disable_null_driver.bat`
* [ ] Add backup/restore functionality
* [ ] Add validation/error checking

**Features:**
- Backup existing settings before modification
- Restore original settings on disable
- Verify SteamVR installation exists
- Check if SteamVR is running (warn if so)

### 4. Modify Global SteamVR Settings
* [ ] Locate global steamvr.vrsettings
* [ ] Add `requireHmd: false`
* [ ] Add `activateMultipleDrivers: true`
* [ ] Create script to automate this

**Path:** `C:\Program Files (x86)\Steam\config\steamvr.vrsettings` (or similar)

### 5. Test Configuration
* [ ] Enable null driver
* [ ] Start SteamVR
* [ ] Verify virtual headset appears
* [ ] Check SteamVR dashboard
* [ ] Verify window rendering

### 6. Test with SkyrimVR
* [ ] Launch SkyrimVR with null driver active
* [ ] Verify game starts
* [ ] Test basic navigation
* [ ] Test menu interaction
* [ ] Document any issues

### 7. Create Documentation
* [ ] Write setup instructions
* [ ] Document troubleshooting steps
* [ ] Add screenshots/examples
* [ ] Document limitations

---

## Files Modified

### New Files
* `config/null_driver/enable_null.bat` - Enable null driver
* `config/null_driver/disable_null.bat` - Disable null driver
* `config/null_driver/templates/null_enabled.vrsettings` - Template configuration
* `config/null_driver/templates/steamvr_global.vrsettings` - Global settings template
* `docs/NULL_DRIVER_SETUP.md` - Setup and usage guide

### Modified Files
* `README.md` - Add null driver setup instructions
* `CHANGELOG.md` - Document Phase 1 completion

---

## Testing

### Unit Testing
1. **Script Validation:**
   - Run enable script → verify settings changed
   - Run disable script → verify settings restored
   - Test with SteamVR not installed (should fail gracefully)
   - Test with SteamVR running (should warn user)

### Integration Testing
2. **SteamVR Testing:**
   - Enable null driver
   - Launch SteamVR
   - Verify: "Headset not detected" does NOT appear
   - Verify: Virtual HMD shown in SteamVR status
   - Verify: Compositor window appears

3. **SkyrimVR Testing:**
   - Launch SkyrimVR
   - Verify: Game starts without errors
   - Verify: Main menu accessible
   - Verify: Can load save game
   - Verify: Basic movement works
   - Verify: Can access in-game menus

### Edge Cases
4. **Configuration Edge Cases:**
   - Multiple enables in a row (should be idempotent)
   - Multiple disables in a row (should not break)
   - Corrupted config file recovery
   - Missing SteamVR installation

---

## Result

*To be filled upon completion*

---

## Notes

### Research References

From `docs/VR_EMULATOR_RESEARCH.md`:
- [Enable SteamVR Null Driver](https://gist.github.com/Adamcbrz/aadc8f613e596d6d503b007afd28fb73)
- [SteamVR No Headset](https://github.com/username223/SteamVRNoHeadset)
- [Null Driver Tutorial](https://www.vrwiki.cs.brown.edu/hardware/vr-hardware/hardware-emulators/null-driver-tutorial)

### Null Driver Behavior

**What it does:**
- Creates borderless window in center of main monitor
- Allows SteamVR to initialize without physical HMD
- Provides basic tracking (fixed or minimal movement)

**Limitations:**
- No realistic head tracking
- No motion controller support (requires separate setup)
- Fixed position/orientation by default
- Not suitable for testing motion-based gameplay

**This is why custom driver (TICKET-0030+) is needed for full functionality**

### Expected Time

**Estimated:** 2-4 hours
- 1 hour: Configuration understanding and template creation
- 1 hour: Script development
- 1-2 hours: Testing and documentation

---

## Closed

*Open - not started*
