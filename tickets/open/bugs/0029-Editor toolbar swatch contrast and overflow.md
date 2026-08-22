# TICKET-0029

**Status**

Open

**Type**

Bug

**Priority**

Medium

**Created**

2026-08-22

**Parent Ticket**

None

**Child Tickets**

None

**Dependencies**

TICKET-0027 (editor must open before its toolbar can be judged)

---

## Description

Two usability defects in the annotation editor's toolbar, both of which make
the colour palette look like it is missing options it actually has:

1. **Black swatch is near-invisible.** The swatch buttons are 20px circles
   filled with their colour and ringed by a 1px `#707782` border, sitting on
   the `#272A30` toolbar. The black (`#000000`) swatch is a dark fill inside a
   dim border on a dark bar, so it reads as empty space rather than as a
   selectable colour. White is legible; black is not.

2. **Toolbar overflows at the default window size.** All toolbar content —
   5 tool buttons, 8 swatches, Custom…, the LINE WEIGHT label + 138px combo,
   Undo/Redo/Delete, Copy, Save As… — lives in a single `ToolBar` totalling
   roughly 1200px against a 1040px-wide window. WPF pushes the tail of the
   band into the `»` overflow menu, and narrowing the window walks further
   back through the row until the swatches themselves disappear.

Reported by the user as "for the colour palette I also need black and white
as options" — both swatches were already present in the markup, but not
reliably visible.

---

## Reason

1. Swatch contrast was tuned against mid-tone fills (red/orange/green/blue)
   and never checked against the two extremes. A single border colour cannot
   separate both a black fill and a white fill from a dark toolbar unless it
   sits between them in luminance and is thick enough to see at 20px.

2. `ToolBarTray` was given one `ToolBar` and no band assignment, so every item
   competes for a single row. Nothing constrained the window's minimum width
   either, so the palette could be shrunk out of view entirely.

---

## Implementation Plan

* [x] Lighten the `SwatchButton` border and thicken it to 2px so both the
      black and the white swatch read as distinct circles against `#272A30`.
      Keep the hover state a white ring at the same thickness so hovering no
      longer shifts layout by 1px.
* [x] Split the single `ToolBar` into two `ToolBarTray` bands: band 0 for
      tools + colour + line weight, band 1 for Undo/Redo/Delete + Copy /
      Save As…, so nothing overflows at the default size.
* [x] Give `EditorWindow` a `MinWidth`/`MinHeight` that keeps band 0 (792px measured)
      fully visible, so the palette cannot be resized into the overflow menu.
* [x] Hide each band's overflow chevron unless it actually has overflow items
      (found during verification — see Notes).
* [x] Verify the editor opens with all 8 swatches visible and no `»` chevron
      at default size, and that the palette survives resizing to the minimum.

---

## Files Modified

* `src/SGrab/Views/EditorWindow.xaml` — swatch border, toolbar bands, window
  minimum size, named the tray.
* `src/SGrab/Views/EditorWindow.xaml.cs` — `HideIdleOverflowButtons` binds each
  band's overflow chevron to `ToolBar.HasOverflowItems`.

---

## Testing

Manual: build Debug, open the editor from a capture and from the filmstrip.
Confirm all 8 swatches render (black distinguishable from the bar), no
overflow chevron at 1040x720, actions on the second band, and that dragging
the window to its minimum size leaves the full palette visible.

---

## Result

Fixed, verified by rendering `EditorWindow` offscreen in a throwaway harness at
both 1040px (default) and 900px (the new minimum) and asserting
`ToolBar.HasOverflowItems` directly rather than eyeballing the row:

```
width=1040 band0 HasOverflowItems=False actualWidth=792
width=1040 band1 HasOverflowItems=False actualWidth=321
width=900  band0 HasOverflowItems=False actualWidth=792
width=900  band1 HasOverflowItems=False actualWidth=321
```

Band 0 measures 792px, so the 900px `MinWidth` leaves ~108px of headroom and
the palette cannot be resized out of view. The rendered image confirms all 8
swatches visible with black and white both reading as distinct circles.

---

## Notes

The colour swatches were never actually missing black or white — see
`EditorWindow.xaml` swatch row. This ticket is about them being visible and
reachable, not about adding them.

Splitting the tray into two bands surfaced a third defect: a `ToolBar` renders
its light-themed overflow chevron even when `HasOverflowItems` is false, so the
second band doubled an existing white sliver on the dark bar. The chevron has no
public style key (`ToolBar.OverflowButtonStyleKey` does not exist — that was
tried first and threw at XAML load); it is a private template part named
`OverflowButton`, so its visibility is bound in code-behind after `Loaded`.

`System.Windows.Data.Binding` collides with `System.Windows.Forms.Binding` in
this project — aliased at the top of `EditorWindow.xaml.cs` like the other
WPF/WinForms collisions already handled there.

---

## Token Usage

<!-- Run /log-cost and paste /cost output to populate this section -->

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

YYYY-MM-DD
