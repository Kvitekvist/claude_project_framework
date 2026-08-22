# TICKET-0013

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

* [x] Bottom filmstrip in `MainWindow` bound to the store via `MainViewModel`
      (`ObservableCollection<Screenshot>`, refreshed on `IScreenshotStore.Changed`)
* [x] Horizontal scrollable thumbnail list (ItemsControl + horizontal StackPanel)
* [x] Click → open in editor; per-thumbnail ✕ delete button (with confirm)
* [x] Live update when a new capture is added (Changed → refresh, UI-thread safe)
* [x] Empty-state placeholder (replaces the TICKET-0007 placeholder text)
* [x] `PathToImageConverter` / `InverseBooleanToVisibilityConverter`

---

## Files Modified

* src/SGrab/ViewModels/MainViewModel.cs (store, Screenshots collection, refresh)
* src/SGrab/Common/Converters.cs (new)
* src/SGrab/Views/MainWindow.xaml(.cs) (filmstrip, click-to-open, delete)

---

## Testing

* [x] `dotnet build` clean (0/0); app launches (main window + filmstrip + DI).
* [ ] New captures appear at the front of the strip immediately (interactive).
* [ ] Clicking a thumbnail reopens it in the editor (interactive).
* [ ] Deleting removes it from strip and library (interactive).

---

## Result

Filmstrip implemented: a horizontally scrollable row of thumbnails along the
bottom of the main window, bound to the store and refreshed live on change.
Clicking a thumbnail opens it in the editor; a ✕ button deletes (with confirm).
An empty-state message shows when the library is empty.

---

## Notes

Consumes the store `Changed` event from TICKET-0009 and opens the editor from
TICKET-0010. Right-click was replaced by an always-visible ✕ button to avoid the
WPF ContextMenu DataContext-inheritance pitfall.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

