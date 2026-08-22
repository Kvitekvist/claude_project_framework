# TICKET-0008

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

TICKET-0007

---

## Description

Implement region-select screen capture. Triggering a capture (button or
global hotkey) shows a dimmed full-screen overlay spanning all monitors;
the user drags a rectangle; on release the selected region is captured to a
bitmap and handed off (to storage/editor in later tickets). Esc cancels.

## Reason

Fast, immediate region capture is the product's core "#1" feature.

## Implementation Plan

* [x] Real `CaptureService : ICaptureService` replacing `StubCaptureService`
* [x] Full-screen borderless topmost overlay covering the virtual desktop
* [x] Dimmed backdrop + rubber-band selection rectangle with live px readout
* [x] Capture region via `Graphics.CopyFromScreen` into a 32bpp Bitmap
* [x] Esc / zero-size cancels; drag-release confirms
* [x] `CapturedImage` shared result type (TICKET-0009/0010 consume it)
* [x] DPI scaling via the overlay window's DPI (exact single-/uniform-DPI)
* [x] Placeholder sink until 0009/0010: copy to clipboard + tray balloon
* [ ] Perfect mixed-DPI multi-monitor mapping (deferred — see Notes)

---

## Files Modified

* src/SGrab/Models/CapturedImage.cs (new)
* src/SGrab/Services/ICaptureService.cs (added CaptureCompleted event)
* src/SGrab/Services/CaptureService.cs (new; replaces StubCaptureService.cs, deleted)
* src/SGrab/Views/CaptureOverlayWindow.xaml(.cs) (new)
* src/SGrab/App.xaml.cs (register CaptureService, clipboard+tray sink)

---

## Testing

* [x] `dotnet build` clean (0/0); app launches without crash.
* [ ] Button and Ctrl+Shift+S both open the overlay (interactive).
* [ ] Drag selects; release captures; clipboard receives the image (interactive).
* [ ] Esc cancels with no leftover overlay (interactive).
* [ ] Selection on a secondary monitor captures the correct pixels (interactive).

---

## Result

Region-select capture implemented. Overlay dims the whole virtual desktop, the
user drags a rectangle (live pixel size shown), and the selected region is
grabbed via GDI `CopyFromScreen` after the overlay hides. Result flows through
`ICaptureService.CaptureCompleted`; until storage/editor exist, App copies it to
the clipboard and shows a tray balloon. Builds clean; interactive checks pending.

---

## Notes

Capture result type (`CapturedImage`) shared with TICKET-0009 (storage) and
TICKET-0010 (editor).

DPI: selection is tracked in the overlay's DIP space and converted to physical
pixels using the overlay window's DPI scale. This is exact on single-monitor and
uniform-DPI multi-monitor setups. On mixed-DPI setups the capture stays WYSIWYG
on the overlay's own monitor but may be offset on a monitor with a different
scale; a per-monitor overlay would fix this and is deferred as a refinement.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

