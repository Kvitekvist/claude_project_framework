@echo off
setlocal

echo ============================================
echo VREmulator - Build Driver
echo ============================================
echo.

set BUILD_DIR=%~dp0..\build\cmake
set SOURCE_DIR=%~dp0..\src\driver

REM Check if CMake is installed
where cmake >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: CMake not found in PATH
    echo Please install CMake from https://cmake.org/download/
    echo.
    pause
    exit /b 1
)

REM Check for Visual Studio
where cl >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo Searching for Visual Studio...

    REM Try to find VS2022
    if exist "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvars64.bat" (
        call "C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build\vcvars64.bat"
    ) else if exist "C:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Auxiliary\Build\vcvars64.bat" (
        call "C:\Program Files\Microsoft Visual Studio\2022\Professional\VC\Auxiliary\Build\vcvars64.bat"
    ) else if exist "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\VC\Auxiliary\Build\vcvars64.bat" (
        call "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\VC\Auxiliary\Build\vcvars64.bat"
    ) else if exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Auxiliary\Build\vcvars64.bat" (
        call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Auxiliary\Build\vcvars64.bat"
    ) else (
        echo ERROR: Visual Studio not found
        echo Please install Visual Studio 2019 or later with C++ development tools
        echo.
        pause
        exit /b 1
    )
)

echo.
echo Creating build directory...
if not exist "%BUILD_DIR%" mkdir "%BUILD_DIR%"

cd /d "%BUILD_DIR%"

echo.
echo Running CMake configuration...
cmake "%SOURCE_DIR%" -G "Visual Studio 17 2022" -A x64

if %ERRORLEVEL% NEQ 0 (
    REM Try VS2019 if VS2022 not found
    echo VS2022 not found, trying VS2019...
    cmake "%SOURCE_DIR%" -G "Visual Studio 16 2019" -A x64

    if %ERRORLEVEL% NEQ 0 (
        echo ERROR: CMake configuration failed
        echo.
        cd /d "%~dp0"
        pause
        exit /b 1
    )
)

echo.
echo Building driver...
cmake --build . --config Release

if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Build failed
    echo.
    cd /d "%~dp0"
    pause
    exit /b 1
)

cd /d "%~dp0"

echo.
echo ============================================
echo Build complete!
echo ============================================
echo.
echo Driver DLL: build\driver\vremulator\bin\win64\driver_vremulator.dll
echo.
echo Next steps:
echo 1. Copy driver folder to SteamVR drivers directory
echo 2. Restart SteamVR
echo 3. Check logs for driver loading
echo.
pause
