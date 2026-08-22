# TICKET-0014

**Status**

Open

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

* [ ] `scripts/build.bat` → `dotnet publish -c Release -r win-x64` (self-contained,
      single-file) into `build/`
* [ ] Version from `version.txt` applied to assembly info
* [ ] App icon + metadata
* [ ] (Optional) installer via MSIX or a simple Inno Setup script
* [ ] Update README with build/run instructions

---

## Files Modified

---

## Testing

* `scripts/build.bat` on a clean checkout produces a runnable exe in `build/`.
* Exe runs on a machine without the .NET SDK installed.

---

## Result

---

## Notes

Final integration ticket; closes only after end-to-end flow works.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

