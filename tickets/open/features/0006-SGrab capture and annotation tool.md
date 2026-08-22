# TICKET-0006

**Status**

Open

**Type**

Feature

**Priority**

High

**Created**

2026-08-21

**Parent Ticket**

None

**Child Tickets**

TICKET-0007, TICKET-0008, TICKET-0009, TICKET-0010, TICKET-0011, TICKET-0012, TICKET-0013, TICKET-0014

**Dependencies**

None

---

## Description

Build SGrab: a Windows screenshot tool inspired by Snagit. Core value:

1. One click (button or global hotkey) immediately starts a region-select
   screenshot capture.
2. Captures open in an annotation editor supporting numbered step bubbles
   (1, 2, 3…), circles, squares, and text, with easy per-object color
   changes.
3. A scrollable filmstrip along the bottom lets the user browse and reopen
   past screenshots.

## Reason

The user wants a personal, maintainable alternative to Snagit focused on
fast capture + quick annotation + easy history browsing.

## Implementation Plan

Decomposed into phased child tickets (dependencies in parentheses):

* [x] TICKET-0007 App scaffold — WPF/.NET 8 shell, MVVM+DI, main window, tray
      icon, global-hotkey infrastructure (no deps)
* [x] TICKET-0008 Capture engine — region-select overlay + bitmap capture,
      triggered by hotkey & button (0007) — implemented, pending verification
* [ ] TICKET-0009 Storage & history model — save PNG + thumbnails, library
      service (0007)
* [ ] TICKET-0010 Editor window + annotation canvas — objects, select/move/
      resize/delete, undo/redo (0008, 0009)
* [ ] TICKET-0011 Annotation tools — step bubbles, circle, square, text;
      per-object color picker + stroke width (0010)
* [ ] TICKET-0012 Export — save annotated image to file + copy to clipboard (0010)
* [ ] TICKET-0013 Filmstrip — scrollable bottom row of past captures, click
      to reopen in editor (0009, 0010)
* [ ] TICKET-0014 Build & packaging — build.bat producing publishable exe/
      installer (all)

---

## Files Modified

(Tracked in child tickets.)

---

## Testing

Parent verified when all child tickets are closed and the end-to-end flow
(capture → annotate → export, plus filmstrip history) works from a clean build.

---

## Result

---

## Notes

Tech stack decided 2026-08-21: C# / .NET 8 (LTS) + WPF, MVVM. (.NET 10 was the
first choice but only the .NET 8 SDK is installed; .NET 8 LTS is fully capable.)
Chosen over WinForms (weak annotation canvas), Electron (heavier, awkward
overlay/hotkeys), and C++/Qt (slow to build UI). See architecture.md.

---

## Token Usage

<!-- Run /log-cost and paste /cost output to populate this section -->

| Session | Input | Output | Cache Read | Cache Write | Cost |
|---------|-------|--------|------------|-------------|------|
| | | | | | |

---

## Closed

