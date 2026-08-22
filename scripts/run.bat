@echo off
REM Run script - starts SGrab from source (Debug).
setlocal
cd /d "%~dp0\.."
dotnet run --project src\SGrab\SGrab.csproj -c Debug
endlocal
