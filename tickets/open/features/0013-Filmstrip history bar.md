# TICKET-0013

**Status**

Open

**Type**

Feature

**Priority**

Medium

**Created**

2026-08-21

**Parent Ticket**

TICKET-0006

**Child Tickets**

None

**Dependencies**

TICKET-0009, TICKET-0010

---

## Description

A horizontally scrollable filmstrip along the bottom of the main window (and/or
editor) showing thumbnails of past screenshots newest-first. Clicking a
thumbnail opens that capture in the editor. Right-click to delete.

## Reason

The user wants to scroll through past screenshots easily — the third core
requirement.

## Implementation Plan

* [ ] Bottom `FilmstripView` bound to `IScreenshotStore` collection
* [ ] Horizontal, mouse-wheel + drag scrollable thumbnail list (virtualized)
* [ ] Click → open in editor; right-click → delete (with confirm)
* [ ] Live update when a new capture is added
* [ ] Empty-state placeholder (replaces the TICKET-0007 placeholder text)

---

## Files Modified

---

## Testing

* New captures appear at the front of the strip immediately.
* Clicking a thumbnail reopens it in the editor.
* Deleting removes it from strip and library.

---

## Result

---

## Notes

Consumes the store events from TICKET-0009 and the editor from TICKET-0010.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

