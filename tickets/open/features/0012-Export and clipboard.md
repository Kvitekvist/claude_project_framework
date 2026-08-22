# TICKET-0012

**Status**

In Progress (implemented; pending interactive verification)

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

* [x] `AnnotationCanvas.RenderFlattened()` → `RenderTargetBitmap` at native
      pixel size via the shared `DrawScene` (no selection handles)
* [x] Save As dialog (PNG default, JPG option); remembers last folder
* [x] Copy flattened image to clipboard
* [x] Toolbar buttons + shortcuts (Ctrl+S save, Ctrl+C copy)
* [ ] Update the stored library copy on save (deferred — see Notes)

---

## Files Modified

* src/SGrab/Controls/AnnotationCanvas.cs (RenderFlattened)
* src/SGrab/Views/EditorWindow.xaml(.cs) (Copy / Save As buttons, encoders,
  Ctrl+S / Ctrl+C)

---

## Testing

* [x] `dotnet build` clean (0/0).
* [ ] Saved PNG/JPG opens externally and shows annotations (interactive).
* [ ] Clipboard paste into another app yields the annotated image (interactive).

---

## Result

Export implemented: the editor flattens the capture + annotations to a
`RenderTargetBitmap` (reusing `DrawScene`) and either copies it to the clipboard
or saves it via a Save As dialog (PNG or JPEG, remembers the last folder), with
Ctrl+C / Ctrl+S shortcuts.

---

## Notes

Reuses `DrawScene` from TICKET-0010. Annotations are not written back into the
library original (the store keeps the clean capture); persisting editable
annotations or an annotated library copy is a deliberate future enhancement.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

