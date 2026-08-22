# TICKET-0027

**Status**

Open

**Type**

Bug

**Priority**

High

**Created**

2026-08-22

**Parent Ticket**

None

**Child Tickets**

None

**Dependencies**

None

---

## Description

Opening the annotation editor fails: the editor window (and therefore the
tool palette / toolbar) never appears. After a capture the user only sees a
"Capture failed: Object reference not set to an instance of an object"
balloon tip; opening a screenshot from the filmstrip shows the equivalent
"Could not open screenshot" message box. Both entry points into the editor
are affected.

---

## Reason

WPF XAML initialization-order defect in `EditorWindow`.

The stroke-width `ComboBox` (`StrokeCombo`) is declared in the toolbar near
the top of `EditorWindow.xaml` with `<ComboBoxItem Content="3"
IsSelected="True"/>` and `SelectionChanged="OnStrokeChanged"`. During
`InitializeComponent()` the XAML parser builds the ComboBox and its initial
selection raises `SelectionChanged`, invoking `OnStrokeChanged`. That handler
calls `Canvas.ApplyStrokeToSelection(...)`, but the `Canvas` named element is
declared *later* in the XAML (line 53) and has not been assigned to its
generated field yet — so `Canvas` is `null` and the call throws
`NullReferenceException` out of the constructor.

Both `App.OnCaptureCompleted` and `MainWindow.OpenInEditor` construct the
editor inside a try/catch, so the throw is swallowed into a generic error
message and the window is never shown.

---

## Implementation Plan

* [x] Guard `OnStrokeChanged` so it no-ops while the named `Canvas` element
      is not yet assigned (fires during `InitializeComponent`).
* [x] Verify the editor window opens with the full palette from both the
      capture flow and the filmstrip.

---

## Files Modified

* `src/SGrab/Views/EditorWindow.xaml.cs` — null-guard in `OnStrokeChanged`.

---

## Testing

Manual: build Debug, capture a region → editor opens with palette; open a
screenshot from the filmstrip → editor opens with palette. No "Capture
failed" / "Could not open screenshot" errors.

---

## Result

Fixed. `OnStrokeChanged` now returns early while `Canvas` is null (the state
during `InitializeComponent`). Verified in a Debug build: triggering a
capture via the Ctrl+Shift+S hotkey and drag-selecting a region opens the
editor with the full palette rendered (tools, colour swatches, Custom…,
stroke Width, Undo/Redo/Delete, Copy, Save As…) instead of the previous
"Capture failed" error balloon.

---

## Notes

Root cause is a classic WPF pitfall: an initial value set in XAML raises a
change event during `InitializeComponent()`, before x:Name'd elements
declared later in the tree exist. Only `OnStrokeChanged` is affected — the
"Select" tool RadioButton uses `Click` (not `Checked`), which does not fire
on load.

---

## Token Usage

<!-- Run /log-cost and paste /cost output to populate this section -->

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

YYYY-MM-DD
