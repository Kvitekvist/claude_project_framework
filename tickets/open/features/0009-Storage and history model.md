# TICKET-0009

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

* [ ] `Screenshot` model (Id, CreatedUtc, Width, Height, ImagePath, ThumbPath)
* [ ] Library folder under `%LocalAppData%/SGrab/Library` (configurable)
* [ ] `IScreenshotStore` — Save(bitmap), GetAll(), Load(id), Delete(id)
* [ ] PNG encode + thumbnail generation (e.g. max 200px)
* [ ] Metadata index (JSON manifest) kept in sync on save/delete
* [ ] CollectionChanged / event for observers

---

## Files Modified

---

## Testing

* Saving a capture writes PNG + thumb + manifest entry.
* GetAll() returns items newest-first across restarts.
* Delete removes files and manifest entry.

---

## Result

---

## Notes

Keep store UI-agnostic; filmstrip and editor consume it.

---

## Token Usage

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

