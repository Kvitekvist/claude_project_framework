# Project Memory

This file represents the long-term memory of the project.

Update continuously.

---

## Project Vision

SGrab is a personal, maintainable Windows screenshot tool inspired by Snagit.
Long-term goal: fast one-click region capture, a lightweight annotation editor
(numbered step bubbles, circles, squares, text, easy color changes), and a
scrollable filmstrip of past screenshots. Optimize for long-term
maintainability over rapid feature delivery.

---

## Current Milestone

M1 — Capture & Annotate MVP (parent TICKET-0006). Phased:
Phase 1 Foundation (0007 scaffold, 0008 capture, 0009 storage) →
Phase 2 Editor (0010 canvas, 0011 tools, 0012 export) →
Phase 3 History & ship (0013 filmstrip, 0014 packaging).

---

## Active Priorities

* TICKET-0007 — App scaffold & shell (DONE — WPF/.NET 8, MVVM+DI, tray, hotkey infra; builds clean)

* TICKET-0008 — Capture engine (DONE — region-select overlay + CopyFromScreen; clipboard+tray placeholder sink)

* TICKET-0009 — Storage & history model (DONE — FileScreenshotStore, %LocalAppData%/SGrab/Library, 4/4 unit tests)

* TICKET-0010 — Editor window + annotation canvas (DONE — owner-drawn AnnotationCanvas, select/move/resize/delete, undo/redo)

* TICKET-0011 — Annotation tools (step/text/rect/ellipse) + color/stroke (DONE)

* TICKET-0012 — Export & clipboard (DONE — flatten to PNG/JPG + clipboard)

* TICKET-0013 — Filmstrip history bar (DONE — thumbnails, click-to-open, delete)

* TICKET-0014 — Build & packaging ← NEXT (last ticket)

---

## Technical Debt

List known technical debt.

---

## Known Issues

List unresolved problems.

---

## Future Ideas

Ideas worth considering later.

---

## Development Patterns

### User Work Style

**Autonomous Implementation Preference**: User prefers minimal clarifying questions for infrastructure/system tasks. When given clear directive ("make a ticket subfolder structure system"), proceed with full implementation using best practices without seeking approval for architectural decisions. Applies to: infrastructure improvements, tooling, system organization, documentation structure.

### Phased Implementation

For large feature sets (10+ items), use phased implementation:

**Phase 1: Foundation** (Implement immediately)
- Highest priority items
- Items other phases depend on
- Core infrastructure
- Quality gates

**Phases 2-5: Roadmap** (Document, implement iteratively)
- Build on Phase 1 foundations
- Each phase has clear theme/purpose
- Dependencies flow downward (Phase N needs Phase N-1)

**Rationale**: 
- Prevents overwhelming scope
- Allows testing foundations before scaling
- Enables learning from early phases
- Follows expert pattern: Context → Connections → Capabilities → Cadence

**Example**: 28 skills → Phase 1 (6 foundation skills) implemented, 22 documented for future phases. Each phase builds quality infrastructure before productivity tools.

---

## Notes

General development notes.
