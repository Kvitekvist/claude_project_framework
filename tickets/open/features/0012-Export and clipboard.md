# TICKET-0012

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

TICKET-0010

---

## Description

Export the annotated image: flatten the capture plus all annotation objects
into a single bitmap, then Save As (PNG/JPG) to disk and Copy to clipboard.

## Reason

A screenshot tool is only useful if the finished, annotated result can be
saved and pasted elsewhere.

## Implementation Plan

* [ ] Render capture + annotation objects to a flattened bitmap at native res
* [ ] Save As dialog (PNG default, JPG option), remember last folder
* [ ] Copy flattened image to clipboard
* [ ] Toolbar buttons + shortcuts (Ctrl+S save, Ctrl+C copy)
* [ ] Update the stored library copy on save

---

## Files Modified

---

## Testing

* Saved file opens externally and shows annotations correctly.
* Clipboard paste into another app yields the annotated image.

---

## Result

---

## Notes

Reuses the object-rendering path from TICKET-0010.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

