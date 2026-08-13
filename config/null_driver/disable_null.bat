@echo off
setlocal enabledelayedexpansion

echo ============================================
echo VREmulator - Disable Null Driver
echo ============================================
echo.

REM Check if SteamVR is installed
set "STEAMVR_PATH="
set "STEAM_PATH=C:\Program Files (x86)\Steam"

if exist "%STEAM_PATH%\steamapps\common\SteamVR" (
    set "STEAMVR_PATH=%STEAM_PATH%\steamapps\common\SteamVR"
) else (
    echo ERROR: SteamVR not found at default location
    echo.
    pause
    exit /b 1
)

echo Found SteamVR at: %STEAMVR_PATH%
echo.

REM Check if SteamVR is running
tasklist /FI "IMAGENAME eq vrserver.exe" 2>NUL | find /I /N "vrserver.exe">NUL
if "%ERRORLEVEL%"=="0" (
    echo WARNING: SteamVR is currently running!
    echo Please close SteamVR before disabling null driver
    echo.
    echo Close SteamVR and press any key to continue...
    pause >nul
)

REM Locate null driver settings
set "NULL_DRIVER_PATH=%STEAMVR_PATH%\drivers\null\resources\settings"
set "NULL_SETTINGS=%NULL_DRIVER_PATH%\default.vrsettings"

if not exist "%NULL_SETTINGS%.backup" (
    echo WARNING: No backup found
    echo Cannot restore original settings
    echo.
    echo The null driver will be disabled but original settings are lost
    echo You may need to verify SteamVR files through Steam
    echo.
    pause
    exit /b 1
)

echo Restoring original null driver settings...
copy /Y "%NULL_SETTINGS%.backup" "%NULL_SETTINGS%" >nul

if %ERRORLEVEL% EQU 0 (
    echo SUCCESS: Original settings restored
    echo Backup preserved at: %NULL_SETTINGS%.backup
) else (
    echo ERROR: Failed to restore settings
    pause
    exit /b 1
)

REM Restore global SteamVR settings
set "STEAM_CONFIG=%STEAM_PATH%\config"
set "STEAMVR_CONFIG=%STEAM_CONFIG%\steamvr.vrsettings"

if exist "%STEAMVR_CONFIG%.backup" (
    echo.
    echo Restoring global SteamVR settings...
    copy /Y "%STEAMVR_CONFIG%.backup" "%STEAMVR_CONFIG%" >nul

    if %ERRORLEVEL% EQU 0 (
        echo SUCCESS: Global settings restored
    ) else (
        echo WARNING: Could not restore global settings
    )
else
    echo.
    echo NOTE: No global settings backup found
    echo Global settings were not modified or backup is missing
)

echo.
echo ============================================
echo Null driver disabled successfully!
echo ============================================
echo.
echo Original settings have been restored
echo SteamVR will now require a physical headset
echo.
echo To re-enable: Run enable_null.bat
echo.
pause
