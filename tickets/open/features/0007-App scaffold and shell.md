# TICKET-0007

**Status**

In Progress (implemented; pending interactive verification)

**Type**

Feature

**Priority**

High

**Created**

2026-08-21

**Parent Ticket**

TICKET-0006

**Child Tickets**

None

**Dependencies**

None

---

## Description

Create the SGrab application skeleton: a WPF (.NET 8) desktop app using MVVM
with dependency injection. Provide the main window with a prominent "New
Capture" button, a system-tray icon, and reusable global-hotkey infrastructure
that later tickets hook into.

## Reason

Every other ticket depends on a running app shell, DI container, and the
hotkey/tray plumbing. Establishes project structure and conventions.

## Implementation Plan

* [x] Create `src/SGrab/SGrab.csproj` (WPF, net8.0-windows, nullable, x64)
* [x] Create `SGrab.sln` and add the project
* [x] Wire `Microsoft.Extensions.Hosting`/DI in `App.xaml.cs`
* [x] MVVM base (`ViewModelBase`, `RelayCommand`), folder layout
      (Views/ViewModels/Services/Common)
* [x] `MainWindow` with a large "New Capture" button + filmstrip placeholder
* [x] Tray icon (NotifyIcon) with New Capture / Show / Exit menu
* [x] `IHotkeyService` — global hotkeys via RegisterHotKey; Ctrl+Shift+S →
      capture command
* [x] `ICaptureService` + `StubCaptureService` stub (replaced by TICKET-0008)
* [x] Single-instance guard (Mutex) that surfaces the running instance
* [x] app.manifest with PerMonitorV2 DPI awareness (for TICKET-0008 accuracy)

---

## Files Modified

* src/SGrab/SGrab.csproj, app.manifest
* src/SGrab/App.xaml(.cs)
* src/SGrab/Views/MainWindow.xaml(.cs)
* src/SGrab/ViewModels/ViewModelBase.cs, MainViewModel.cs
* src/SGrab/Common/RelayCommand.cs
* src/SGrab/Services/ICaptureService.cs, StubCaptureService.cs,
  IHotkeyService.cs, HotkeyService.cs
* SGrab.sln

---

## Testing

* [x] `dotnet build` succeeds, 0 warnings / 0 errors.
* [x] App launches without startup crash (process smoke-test).
* [ ] Button and Ctrl+Shift+S both show the stub capture message (interactive).
* [ ] Tray menu New Capture / Show / Exit behave correctly (interactive).
* [ ] Second launch surfaces the existing window instead of a new one (interactive).

---

## Result

Scaffold complete and building cleanly on .NET 8 SDK 8.0.401. App starts,
registers the global hotkey, and shows the main window + tray icon. Interactive
UI checks await user confirmation.

---

## Notes

Capture is behind `ICaptureService` so TICKET-0008 swaps the stub for the real
region-select overlay with no other changes.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

