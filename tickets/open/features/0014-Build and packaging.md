# TICKET-0014

**Status**

Done (verified)

**Type**

Infrastructure

**Priority**

Medium

**Created**

2026-08-21

**Parent Ticket**

TICKET-0006

**Child Tickets**

None

**Dependencies**

TICKET-0007, TICKET-0008, TICKET-0009, TICKET-0010, TICKET-0011, TICKET-0012, TICKET-0013

---

## Description

Make SGrab shippable: a `scripts/build.bat` that publishes a self-contained
Windows executable, versioning wired to `version.txt`, and (optionally) a
simple installer.

## Reason

The app needs a one-command build producing an artifact the user can run
without Visual Studio.

## Implementation Plan

* [x] `scripts/build.bat` → `dotnet publish -c Release -r win-x64` self-contained
      single-file into `build/`
* [x] Version from `version.txt` applied via `-p:Version` (app set to 0.1.0)
* [x] Assembly metadata (Product/Company/Title/Description)
* [x] `scripts/run.bat` → `dotnet run`
* [x] README build/run/packaging instructions
* [ ] App icon (.ico) + installer (MSIX/Inno) — deferred, not needed for MVP

---

## Files Modified

* scripts/build.bat, scripts/run.bat
* src/SGrab/SGrab.csproj (metadata, Version, conditional RID)
* version.txt (→ 0.1.0), README.md

---

## Testing

* [x] `scripts\build.bat` produces `build\SGrab.exe` (156 MB, self-contained).
* [x] The published exe launches with no .NET SDK on the PATH (runs the bundled
      runtime).
* [x] `dotnet test` still passes (4/4) after csproj changes.

---

## Result

`scripts\build.bat` publishes a self-contained, single-file `build\SGrab.exe`
(~156 MB, bundles the .NET 8 runtime) versioned from `version.txt`; verified to
run. `run.bat` starts from source. README documents build/run/packaging. Icon and
an installer are deferred as non-MVP polish.

---

## Notes

Final integration ticket. `build/` is gitignored so the large artifact is not
committed.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

