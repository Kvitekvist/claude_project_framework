@echo off
REM Build script - publishes a self-contained, single-file Windows executable.
setlocal
cd /d "%~dp0\.."

set /p VERSION=<version.txt
echo Building SGrab %VERSION% (self-contained, win-x64, single file)...
echo.

dotnet publish src\SGrab\SGrab.csproj -c Release -r win-x64 --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true ^
  -p:Version=%VERSION% ^
  -o build

if errorlevel 1 (
  echo.
  echo Build FAILED.
  exit /b 1
)

echo.
echo Done. Run:  build\SGrab.exe
endlocal
