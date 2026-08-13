# Null Driver Setup Guide

## Overview

The null driver is SteamVR's built-in capability to run without a physical VR headset. This guide will help you set it up for SkyrimVR development.

**Time required:** 5-10 minutes  
**Difficulty:** Easy

---

## Prerequisites

- **SteamVR installed** via Steam
- **Windows 10/11**
- **Administrator rights** (for first-time setup)
- **SkyrimVR** (for testing)

**Default SteamVR location:** `C:\Program Files (x86)\Steam\steamapps\common\SteamVR`

---

## Quick Start

### Step 1: Enable Null Driver

1. Navigate to `config/null_driver/`
2. Right-click `enable_null.bat`
3. Select "Run as Administrator"
4. Follow on-screen prompts

The script will:
- Locate your SteamVR installation
- Backup original settings
- Apply null driver configuration
- Configure global SteamVR settings

### Step 2: Launch SteamVR

1. Open Steam
2. Launch SteamVR
3. You should see: "HMD: VREmulator Null HMD v1.0"
4. A window should appear showing dual eye view

**No "Headset not detected" error should appear!**

### Step 3: Test with SkyrimVR

1. Launch SkyrimVR from Steam
2. Game should start in VR mode
3. You'll see the dual-eye rendered view

---

## Detailed Instructions

### Enabling the Null Driver

**What happens:**
- Backs up `SteamVR\drivers\null\resources\settings\default.vrsettings`
- Applies custom configuration with:
  - `enable: true`
  - Serial number: VREMULATOR001
  - Resolution: 2016x2240 per eye (Vive resolution)
  - Refresh rate: 90Hz
- Updates Steam global config to allow running without headset

**Manual enable (if script fails):**

1. Edit `SteamVR\drivers\null\resources\settings\default.vrsettings`:
```json
{
   "driver_null" : {
      "enable" : true,
      "serialNumber" : "VREMULATOR001",
      "modelNumber" : "VREmulator Null HMD v1.0",
      "renderWidth" : 2016,
      "renderHeight" : 2240,
      "displayFrequency" : 90
   }
}
```

2. Edit Steam config `C:\Program Files (x86)\Steam\config\steamvr.vrsettings`:
```json
{
   "steamvr" : {
      "requireHmd" : false,
      "activateMultipleDrivers" : true,
      "forcedDriver" : "null"
   }
}
```

3. Restart SteamVR

### Disabling the Null Driver

**To return to normal (physical headset) operation:**

1. Close SteamVR
2. Run `config/null_driver/disable_null.bat`
3. Restores original backed-up settings
4. SteamVR will require physical headset again

---

## Configuration Details

### Null Driver Settings

**Location:** `SteamVR\drivers\null\resources\settings\default.vrsettings`

| Setting | Value | Purpose |
|---------|-------|---------|
| `enable` | `true` | Activates null driver |
| `serialNumber` | `VREMULATOR001` | Device identifier |
| `modelNumber` | `VREmulator Null HMD v1.0` | Display name |
| `windowWidth` | `1920` | Preview window width |
| `windowHeight` | `1080` | Preview window height |
| `renderWidth` | `2016` | Per-eye render width |
| `renderHeight` | `2240` | Per-eye render height |
| `displayFrequency` | `90` | Refresh rate (Hz) |

**Why these values?**
- Resolution matches HTC Vive (common SkyrimVR target)
- 90Hz is standard VR refresh rate
- Window size fits typical 1080p monitor

### Global SteamVR Settings

**Location:** `C:\Program Files (x86)\Steam\config\steamvr.vrsettings`

| Setting | Value | Purpose |
|---------|-------|---------|
| `requireHmd` | `false` | Allow SteamVR without headset |
| `activateMultipleDrivers` | `true` | Enable multiple driver support |
| `forcedDriver` | `"null"` | Force null driver to load |

---

## Testing

### Verify Null Driver is Active

1. **In SteamVR:**
   - Open SteamVR status window
   - Should show: "HMD: VREmulator Null HMD v1.0"
   - Status should be green/ready

2. **Visual Confirmation:**
   - A borderless window appears (centered on main monitor)
   - Shows split-screen dual-eye view
   - Rendered scene visible (SteamVR home or game)

3. **No Errors:**
   - ❌ "Headset not detected" = null driver not active
   - ❌ "Display not found" = configuration issue
   - ✅ Green ready indicator = success!

### Test with SkyrimVR

**Basic Launch Test:**
1. Launch SkyrimVR
2. Verify: Game starts without errors
3. Verify: VR rendering active (dual-eye view)
4. Verify: Main menu accessible

**Known Limitations at this stage:**
- ⚠️ Head tracking is **fixed** (cannot look around)
- ⚠️ No motion controllers (yet)
- ⚠️ No position tracking
- ⚠️ Limited to menu navigation

**These will be addressed in Phase 2-6 (custom driver + input simulation)**

---

## Troubleshooting

### SteamVR won't start

**Symptom:** SteamVR crashes or shows errors

**Solutions:**
1. Verify SteamVR is closed completely (`vrserver.exe` not running in Task Manager)
2. Run `disable_null.bat`, then `enable_null.bat` again
3. Verify SteamVR files through Steam:
   - Library → Right-click SteamVR
   - Properties → Local Files → Verify Integrity

### "Headset not detected" error

**Symptom:** SteamVR shows headset not found

**Solutions:**
1. Check `default.vrsettings` in null driver folder:
   - Verify `"enable": true` is set
2. Check global config has `"requireHmd": false`
3. Restart SteamVR completely
4. Try running `enable_null.bat` again

### SkyrimVR won't start

**Symptom:** Game fails to launch or crashes

**Solutions:**
1. Verify SteamVR is running successfully first
2. Launch SkyrimVR from Steam (not SKSE launcher yet)
3. Check SkyrimVR launch options (should be empty for now)
4. Try launching regular Skyrim SE first to rule out game issues

### Black screen in SteamVR window

**Symptom:** Window appears but shows black

**Solutions:**
1. Check GPU drivers are up to date
2. Try changing render resolution in null driver config
3. Check SteamVR compositor is running
4. Verify monitor resolution settings

### Permission denied when running scripts

**Symptom:** Access denied errors

**Solutions:**
1. Right-click script → "Run as Administrator"
2. Check SteamVR is not running
3. Verify you have write access to Steam installation folder

### Script can't find SteamVR

**Symptom:** "SteamVR not found" error

**Solutions:**
1. Edit `enable_null.bat` and update `STEAM_PATH` variable to your Steam location
2. Common alternative locations:
   - `D:\Steam`
   - `C:\Steam`
   - `E:\SteamLibrary`

---

## Limitations of Null Driver

### Current Limitations (Phase 1)

**What works:**
- ✅ SteamVR starts without headset
- ✅ SkyrimVR launches
- ✅ Basic VR rendering
- ✅ Menu viewing

**What doesn't work:**
- ❌ Head tracking (fixed position/rotation)
- ❌ Motion controllers
- ❌ Gameplay interaction
- ❌ Movement controls

### Why Custom Driver is Needed (Phase 2-6)

The null driver is a **quick win** for basic testing, but real development requires:
- **Head tracking control** (keyboard/mouse)
- **Virtual controllers** (for interactions)
- **Input simulation** (button presses, trigger pulls)
- **Position tracking** (for movement testing)

**These capabilities will be implemented in tickets TICKET-0029 through TICKET-0033.**

---

## Next Steps

After confirming null driver works:

1. **Phase 2:** Set up custom driver project (TICKET-0029)
2. **Phase 3:** Implement virtual HMD with tracking (TICKET-0030)
3. **Phase 4:** Add virtual controllers (TICKET-0031)
4. **Phase 5:** Create input simulation (TICKET-0032)
5. **Phase 6:** Full SkyrimVR integration (TICKET-0033)

---

## Advanced Configuration

### Custom Resolution

Edit `config/null_driver/templates/null_enabled.vrsettings`:

```json
"renderWidth" : 2560,   // Higher resolution (e.g., Index)
"renderHeight" : 2880,
"displayFrequency" : 120  // Higher refresh rate
```

Then run `enable_null.bat` again.

**Note:** Higher resolutions require more GPU power!

### Custom Window Position

```json
"windowX" : 1920,   // Move to second monitor
"windowY" : 0,
"windowWidth" : 1920,
"windowHeight" : 1080
```

---

## FAQ

**Q: Is this safe? Will it break my SteamVR?**  
A: Yes, it's safe. The scripts backup original settings and can be reverted anytime with `disable_null.bat`.

**Q: Can I use my real VR headset after this?**  
A: Yes! Run `disable_null.bat` to restore normal operation.

**Q: Will this work with other VR games?**  
A: Yes, any SteamVR game will work, but with same limitations (no tracking/controllers).

**Q: Do I need to keep scripts running?**  
A: No, scripts only modify config files. SteamVR reads them on startup.

**Q: Can I run this on Linux?**  
A: The concept works, but these scripts are Windows-specific. See Linux VR documentation for equivalent.

---

## Support

**Issues with null driver setup?**

1. Check troubleshooting section above
2. Review SteamVR logs: `SteamVR\logs\`
3. Create GitHub issue with:
   - Error message
   - SteamVR version
   - Steps to reproduce

---

**Last Updated:** 2026-08-13  
**Version:** 1.0  
**Ticket:** TICKET-0028
