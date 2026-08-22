# TICKET-0011

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

TICKET-0010

---

## Description

Concrete annotation tools on the editor canvas:

* Numbered step bubbles that auto-increment (1, 2, 3…) as they are placed
* Circle / ellipse
* Rectangle / square
* Text

Plus a toolbar with a color picker and stroke-width control that apply to the
selected object(s) and set the default for newly drawn ones.

## Reason

These are the specific annotation types and the easy recoloring the user asked
for.

## Implementation Plan

* [x] Toolbar tool buttons (select, step, rectangle, ellipse, text)
* [x] `StepAnnotation` with shared auto-increment counter (reset per image)
* [x] `RectangleAnnotation` / `EllipseAnnotation` (from 0010) as square/circle tools
* [x] `TextAnnotation` with editing (double-click / on-create overlay TextBox)
* [x] Colour picker: 8 swatches + Custom… (WinForms ColorDialog), applied to
      selection and set as the new-object default
* [x] Stroke-width selector (1/2/3/5/8), applied to selection + default
* [x] Live preview while dragging to create (shapes)
* [ ] Shift = perfect circle/square (deferred — minor)
* [ ] True in-place text editing (currently an overlay TextBox; good enough)

---

## Files Modified

* src/SGrab/Models/Annotations/{StepAnnotation,TextAnnotation}.cs (new)
* src/SGrab/Controls/AnnotationCanvas.cs (step/text create, text-edit event,
  ApplyColor/ApplyStroke, NotifyObjectModified)
* src/SGrab/Views/EditorWindow.xaml(.cs) (toolbar tools, colour swatches +
  custom, stroke combo, text-edit overlay)

---

## Testing

* [x] `dotnet build` clean (0/0); store tests still pass.
* [ ] Placing multiple step bubbles numbers them 1,2,3… (interactive).
* [ ] Circle, square, text drawn/edited; colour + stroke apply to selection and
      future objects (interactive).

---

## Result

All annotation tools implemented: numbered step bubbles (auto-increment),
rectangle, ellipse, and text, plus an 8-swatch colour picker with a custom
colour dialog and a stroke-width selector. Colour/stroke apply to the current
selection and become the default for new objects; text is edited via an overlay
box on create or double-click. Every change is undoable.

---

## Notes

Built on the `AnnotationTool`/canvas from TICKET-0010. In-place text editing uses
an overlay TextBox positioned via PointToScreen; Shift-constrain and true inline
caret editing are minor future polish.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

