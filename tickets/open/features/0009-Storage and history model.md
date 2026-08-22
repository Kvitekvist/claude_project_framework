# TICKET-0009

**Status**

Done (unit-tested)

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

TICKET-0007

---

## Description

Persist captures to a local library. Each capture is saved as a PNG plus a
thumbnail, with lightweight metadata (id, timestamp, size, file paths). A
library service exposes save/list/load/delete and raises change events so the
filmstrip (TICKET-0013) can react.

## Reason

Screenshots must survive between sessions and be browsable in history.

## Implementation Plan

* [x] `Screenshot` model (Id, CreatedUtc, Width, Height, ImagePath, ThumbPath)
* [x] Library folder under `%LocalAppData%/SGrab/Library` (constructor-configurable)
* [x] `IScreenshotStore` — Save(CapturedImage), Items (newest-first), Delete(id)
* [x] PNG encode + thumbnail generation (max 200px long edge, bicubic)
* [x] JSON manifest (`index.json`) kept in sync on save/delete; missing images
      pruned on load; corrupt manifest tolerated
* [x] `Changed` event for observers (filmstrip, TICKET-0013)
* [x] App saves each capture to the store on capture
* [x] xUnit test project (`tests/SGrab.Tests`) with 4 passing store tests

---

## Files Modified

* src/SGrab/Models/Screenshot.cs (new)
* src/SGrab/Services/IScreenshotStore.cs (new)
* src/SGrab/Services/FileScreenshotStore.cs (new)
* src/SGrab/App.xaml.cs (register store; save capture on completion)
* tests/SGrab.Tests/* (new xUnit project; added to SGrab.sln)

---

## Testing

* [x] `dotnet test` — 4/4 passing:
  * Save writes PNG + thumbnail + manifest entry and adds the item.
  * Reload restores saved items newest-first across store instances.
  * Delete removes the item and its files.
  * Save raises `Changed`.

---

## Result

File-backed screenshot library implemented and unit-tested. Captures are saved
under `%LocalAppData%/SGrab/Library` (images/ + thumbs/ + index.json). App now
saves every capture to the store in addition to the clipboard. Store is
UI-agnostic and ready for the filmstrip (TICKET-0013) and editor (TICKET-0010).

---

## Notes

Store is UI-agnostic; filmstrip and editor consume it. Save is synchronous disk
IO on the UI thread — fine for typical screenshot sizes; revisit if large
captures cause hitches.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

