# Project Architecture

## Overview

SGrab is a single WPF desktop app (.NET 8) using MVVM with dependency
injection (Microsoft.Extensions.Hosting). No server or network component. Data
is stored locally on disk. Flow: hotkey/button → capture overlay → bitmap →
screenshot store → annotation editor → export/clipboard; the filmstrip browses
the store.

---

## Components

### User Interface

WPF/XAML views with MVVM view models. Main window (New Capture button + tray),
full-screen capture overlay, annotation editor window (retained-mode canvas of
selectable objects), and a bottom filmstrip of thumbnails.

### Backend

In-process services: `ICaptureService` (region capture), `IScreenshotStore`
(library persistence), `IHotkeyService` (global hotkeys via Win32
RegisterHotKey). No external backend.

### Database

None. Screenshots saved as PNG + thumbnails under
`%LocalAppData%/SGrab/Library`, indexed by a JSON manifest.

### Networking

None.

### Services

DI-registered singletons wired in `App.xaml.cs`: capture, store, hotkey, plus
export/clipboard helpers.

---

## Folder Responsibilities

### Ticket System Structure

Tickets are organized in category-based subfolders for scalability:

```
tickets/
├── open/
│   ├── features/       # New functionality, enhancements
│   ├── bugs/           # Bug fixes, defects
│   ├── documentation/  # Docs, comments, guides
│   ├── infrastructure/ # Build, CI/CD, tooling
│   └── research/       # Investigation, analysis
├── closed/             # Same structure
└── archived/           # Same structure
```

See `docs/TICKET_CATEGORIES.md` for detailed category guidance.

### Other Folders

Explain what each major folder contains.

---

## Dependencies

Document important libraries and why they are used.

---

## Design Principles

Record architectural principles followed throughout the project.

---

## Future Improvements

Track planned architectural enhancements.
