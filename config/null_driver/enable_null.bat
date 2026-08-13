@echo off
setlocal enabledelayedexpansion

echo ============================================
echo VREmulator - Enable Null Driver
echo ============================================
echo.

REM Check if SteamVR is installed
set "STEAMVR_PATH="
set "STEAM_PATH=C:\Program Files (x86)\Steam"

if exist "%STEAM_PATH%\steamapps\common\SteamVR" (
    set "STEAMVR_PATH=%STEAM_PATH%\steamapps\common\SteamVR"
) else (
    echo ERROR: SteamVR not found at default location
    echo Please ensure SteamVR is installed
    echo.
    echo Expected path: %STEAM_PATH%\steamapps\common\SteamVR
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
    echo Please close SteamVR before enabling null driver
    echo.
    echo Close SteamVR and press any key to continue...
    pause >nul
)

REM Locate null driver settings
set "NULL_DRIVER_PATH=%STEAMVR_PATH%\drivers\null\resources\settings"
set "NULL_SETTINGS=%NULL_DRIVER_PATH%\default.vrsettings"

if not exist "%NULL_DRIVER_PATH%" (
    echo ERROR: Null driver path not found
    echo Path: %NULL_DRIVER_PATH%
    echo.
    pause
    exit /b 1
)

echo Null driver found at: %NULL_DRIVER_PATH%
echo.

REM Backup existing settings if not already backed up
if not exist "%NULL_SETTINGS%.backup" (
    echo Creating backup of original settings...
    copy "%NULL_SETTINGS%" "%NULL_SETTINGS%.backup" >nul
    echo Backup created: %NULL_SETTINGS%.backup
) else (
    echo Backup already exists, skipping...
)

echo.
echo Enabling null driver...
copy /Y "%~dp0templates\null_enabled.vrsettings" "%NULL_SETTINGS%" >nul

if %ERRORLEVEL% EQU 0 (
    echo SUCCESS: Null driver enabled
    echo.
    echo Configuration applied:
    echo - Driver: Enabled
    echo - Serial: VREMULATOR001
    echo - Resolution: 2016x2240 per eye
    echo - Refresh: 90Hz
) else (
    echo ERROR: Failed to copy configuration
    echo.
    pause
    exit /b 1
)

REM Configure global SteamVR settings
set "STEAM_CONFIG=%STEAM_PATH%\config"
set "STEAMVR_CONFIG=%STEAM_CONFIG%\steamvr.vrsettings"

echo.
echo Configuring global SteamVR settings...

if not exist "%STEAM_CONFIG%" (
    echo Creating Steam config directory...
    mkdir "%STEAM_CONFIG%"
)

REM Backup global settings if not already backed up
if exist "%STEAMVR_CONFIG%" (
    if not exist "%STEAMVR_CONFIG%.backup" (
        echo Creating backup of global settings...
        copy "%STEAMVR_CONFIG%" "%STEAMVR_CONFIG%.backup" >nul
        echo Backup created: %STEAMVR_CONFIG%.backup
    )
)

echo Applying global settings (requireHmd=false, activateMultipleDrivers=true)...
copy /Y "%~dp0templates\steamvr_global.vrsettings" "%STEAMVR_CONFIG%" >nul

if %ERRORLEVEL% EQU 0 (
    echo SUCCESS: Global settings applied
) else (
    echo WARNING: Could not update global settings
    echo You may need to manually edit: %STEAMVR_CONFIG%
)

echo.
echo ============================================
echo Null driver configuration complete!
echo ============================================
echo.
echo You can now:
echo 1. Launch SteamVR
echo 2. The null HMD should appear
echo 3. Launch SkyrimVR for testing
echo.
echo To disable: Run disable_null.bat
echo.
pause
