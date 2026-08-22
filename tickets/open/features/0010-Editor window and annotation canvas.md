# TICKET-0010

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

* [x] `EditorWindow` hosting the capture on an owner-drawn `AnnotationCanvas`
* [x] `AnnotationObject` base (bounds, color, stroke, hit-test, clone/copy)
* [x] Reference objects: `RectangleAnnotation`, `EllipseAnnotation`
* [x] Owner-drawn canvas (`OnRender` + reusable `DrawScene`) over the image
* [x] Selection: click to select, drag to move, 8 handles to resize, Del to remove
* [x] Undo/redo stack (`UndoStack` + `DelegateAction`); Ctrl+Z / Ctrl+Y
* [x] `AnnotationTool` enum abstraction that TICKET-0011 extends (Step/Text)
* [x] Capture now opens in the editor (App.OnCaptureCompleted)

---

## Files Modified

* src/SGrab/Common/ImageInterop.cs (new; Bitmap/file → BitmapSource)
* src/SGrab/Common/Undo/UndoStack.cs (new)
* src/SGrab/Models/Annotations/{AnnotationObject,RectangleAnnotation,EllipseAnnotation}.cs (new)
* src/SGrab/Controls/AnnotationCanvas.cs (new)
* src/SGrab/Views/EditorWindow.xaml(.cs) (new)
* src/SGrab/App.xaml.cs (open editor on capture)

---

## Testing

* [x] `dotnet build` clean (0/0); existing store tests still pass.
* [ ] Draw rectangle/ellipse; select, move, resize via handles; delete (interactive).
* [ ] Undo/redo across create/move/resize/delete (interactive).

---

## Result

Editor framework complete: an owner-drawn `AnnotationCanvas` renders the capture
plus retained-mode annotation objects, with click-select, drag-move, 8-handle
resize, delete, and full undo/redo. Rectangle and ellipse ship as the reference
tools; step bubbles, text, and colour come in TICKET-0011. The single `DrawScene`
path is reused by export (TICKET-0012). Captures now open straight in the editor.

---

## Notes

Interaction is handled in the control (idiomatic for a canvas); a thin VM was
unnecessary. `DrawScene(dc, includeSelection:false)` is the export hook.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

