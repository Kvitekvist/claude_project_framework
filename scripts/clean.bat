@echo off
echo ============================================
echo VREmulator - Clean Build
echo ============================================
echo.

set BUILD_DIR=%~dp0..\build

echo Cleaning build directories...

if exist "%BUILD_DIR%\cmake" (
    rmdir /s /q "%BUILD_DIR%\cmake"
    echo Removed cmake build directory
)

if exist "%BUILD_DIR%\driver\vremulator\bin\win64\driver_vremulator.dll" (
    del /q "%BUILD_DIR%\driver\vremulator\bin\win64\driver_vremulator.dll"
    echo Removed driver DLL
)

if exist "%BUILD_DIR%\driver\vremulator\bin\win64\driver_vremulator.pdb" (
    del /q "%BUILD_DIR%\driver\vremulator\bin\win64\driver_vremulator.pdb"
    echo Removed driver PDB
)

if exist "%BUILD_DIR%\driver\vremulator\bin\win64\driver_vremulator.exp" (
    del /q "%BUILD_DIR%\driver\vremulator\bin\win64\driver_vremulator.exp"
    echo Removed driver EXP
)

if exist "%BUILD_DIR%\driver\vremulator\bin\win64\driver_vremulator.lib" (
    del /q "%BUILD_DIR%\driver\vremulator\bin\win64\driver_vremulator.lib"
    echo Removed driver LIB
)

echo.
echo ============================================
echo Clean complete!
echo ============================================
echo.
pause
