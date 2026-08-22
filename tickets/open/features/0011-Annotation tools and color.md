# TICKET-0011

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

* [ ] Toolbar with tool buttons (select, step, circle, rect, text)
* [ ] `StepBubbleTool` with shared auto-increment counter (reset per image)
* [ ] `EllipseTool`, `RectangleTool` (Shift = perfect circle/square)
* [ ] `TextTool` with in-place editing + font size
* [ ] Color picker (swatches + custom) applied to selection & new-object default
* [ ] Stroke-width selector
* [ ] Live preview while dragging to create

---

## Files Modified

---

## Testing

* Placing multiple step bubbles numbers them 1,2,3… in order.
* Circle, square, and text can be drawn and edited.
* Changing color/stroke updates the selected object and future objects.

---

## Result

---

## Notes

Built on the `IAnnotationTool` abstraction from TICKET-0010.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

