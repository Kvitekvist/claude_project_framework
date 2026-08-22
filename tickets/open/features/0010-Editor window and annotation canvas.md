# TICKET-0010

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

TICKET-0008, TICKET-0009

---

## Description

Editor window that opens a captured image on a retained-mode annotation
canvas. Annotations are discrete objects that can be selected, moved,
resized, and deleted, with undo/redo. This ticket delivers the canvas
framework and object model; concrete tools come in TICKET-0011.

## Reason

A retained-mode object canvas (vs. flattened pixels) is what makes editing
and recoloring shapes easy — the second core requirement.

## Implementation Plan

* [ ] `EditorWindow` + `EditorViewModel`, hosts the capture as background
* [ ] `AnnotationObject` base (bounds, color, stroke, z-order, hit-test)
* [ ] Canvas surface rendering objects over the image
* [ ] Selection: click to select, drag to move, handles to resize, Del to remove
* [ ] Undo/redo stack (command pattern)
* [ ] Tool abstraction (`IAnnotationTool`) that TICKET-0011 plugs into

---

## Files Modified

---

## Testing

* Opening a capture shows it at correct size in the editor.
* An object can be selected, moved, resized, deleted.
* Undo/redo reverses/replays each edit.

---

## Result

---

## Notes

Rendering approach chosen here is reused by TICKET-0012 export (render objects
onto the bitmap).

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

