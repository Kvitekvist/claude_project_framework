@echo off
echo ============================================
echo VREmulator - SkyrimVR Launcher
echo ============================================
echo.
echo Starting input handler...
start "" "C:\Users\jensr\Documents\VS Projects\VREmulator\build\vr_input.exe"
timeout /t 2 /nobreak >nul

echo Starting SteamVR...
start "" "D:\games\Steam\steamapps\common\SteamVR\bin\win64\vrstartup.exe"
timeout /t 10 /nobreak >nul

echo Launching SkyrimVR...
start "" "D:\games\Steam\steamapps\common\SkyrimVR\SkyrimVR.exe"

echo.
echo ============================================
echo All launched!
echo ============================================
echo.
echo Controls:
echo   RIGHT MOUSE + Move = Look around
echo   W/A/S/D            = Move
echo   LEFT CLICK         = Right trigger
echo.
echo Press any key to close this window...
pause >nul
