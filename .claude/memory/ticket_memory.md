# Ticket Memory

This file provides a quick overview of completed work.

Append entries only.

---

## Completed Tickets

Example

TICKET-0001

Created initial project.

---

TICKET-0002

Added settings window.

---

TICKET-0003

Improved rendering performance.

---

TICKET-0001

Fixed version consistency between version.txt (1.0.0) and project_status.md (was 0.0.1).

---

TICKET-0002

Added ticket decomposition system for managing large requests. Enhanced ticket template with parent/child/dependency fields, created comprehensive decomposition workflow guide, updated framework to v1.1.0.

---

TICKET-0003

Added GitHub Template distribution system. Created init_project.bat initialization script and comprehensive distribution guide. Users can now create new projects with single command.

---

TICKET-0004

Installed FlowGrid's "second brain" system into Template. Added smart context loading (70-80% token savings), safe ticket management (prevents concurrent collisions), decomposition workflows, token usage tracking, memory archival, 5 core skills, 7 workflow prompts, helper scripts, and comprehensive documentation. Framework v1.1.0, Template v1.2.0.

---

TICKET-0005

Added ticket subfolder structure system. Created category-based organization (features/bugs/documentation/infrastructure/research) for tickets/open/, tickets/closed/, tickets/archived/. Updated ticket template, new-ticket skill, next_ticket.js for subfolder support. Created TICKET_CATEGORIES.md guide and migrate_tickets.bat helper. Maintains backward compatibility with flat structure.

---

## SGrab product (numbering restarted at 0006 to avoid collision with the
## framework meta-tickets 0001–0005 above)

TICKET-0006 (parent, open)

SGrab — Snagit-style Windows capture & annotation tool. Decomposed into 8
phased child tickets (0007–0014). Tech stack decided: C# / .NET 8 (LTS) + WPF,
MVVM+DI. Phase 1 Foundation → Phase 2 Editor → Phase 3 History & ship.

---

TICKET-0007 (in progress — implemented, pending interactive verification)

App scaffold & shell. Created WPF/.NET 8 project (`src/SGrab`), `SGrab.sln`,
MVVM base (ViewModelBase, RelayCommand), DI via Microsoft.Extensions.Hosting,
MainWindow with "New Capture" button + filmstrip placeholder, tray icon
(NotifyIcon), global-hotkey service (RegisterHotKey; Ctrl+Shift+S), capture
behind ICaptureService with a StubCaptureService (replaced by 0008),
single-instance Mutex guard, PerMonitorV2 app.manifest. Builds clean (0/0);
app launches without crash. Interactive UI checks await user confirmation.

---

TICKET-0008 (in progress — implemented, pending interactive verification)

Capture engine. Added `CaptureOverlayWindow` (full-screen dimmed virtual-desktop
overlay, rubber-band selection with live pixel size, Esc/zero-size cancels) and
`CaptureService` (grabs the selected region via GDI `CopyFromScreen` into a
32bpp Bitmap after the overlay hides). New shared `CapturedImage` result type;
`ICaptureService` gained a `CaptureCompleted` event. Replaced StubCaptureService.
Until storage (0009) + editor (0010) exist, App copies the capture to the
clipboard and shows a tray balloon. Builds clean. DPI: exact on single/uniform-
DPI; mixed-DPI multi-monitor mapping deferred (per-monitor overlay refinement).

---

Continue adding completed tickets in chronological order.
