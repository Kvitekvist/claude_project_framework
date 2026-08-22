# TICKET-0008

**Status**

Open

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

* [ ] Real `ICaptureService` implementation replacing `StubCaptureService`
* [ ] Enumerate monitors (virtual screen bounds, per-monitor DPI aware)
* [ ] Full-screen borderless topmost overlay window(s) with dimmed backdrop
* [ ] Rubber-band selection rectangle with live dimensions readout
* [ ] Capture region via `Graphics.CopyFromScreen` (or BitBlt) into a Bitmap
* [ ] Esc cancels; release confirms; return capture as a shared image type
* [ ] Handle multi-monitor + fractional DPI scaling correctly

---

## Files Modified

---

## Testing

* Hotkey and button both launch the overlay.
* Selection on primary and secondary monitors captures the correct pixels.
* DPI-scaled displays capture without offset/stretch.
* Esc cancels cleanly with no leftover overlay.

---

## Result

---

## Notes

Capture result type shared with TICKET-0009 (storage) and TICKET-0010 (editor).

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

