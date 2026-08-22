# Changelog

All notable changes to this template framework should be documented here.

---

## Unreleased — SGrab product

### Added

* **SGrab defined** — the repository now targets a real product: a Snagit-style
  Windows screenshot capture & annotation tool (parent TICKET-0006, decomposed
  into child tickets 0007–0014).
* Tech stack: C# / .NET 8 (LTS) + WPF, MVVM with dependency injection.
* **App scaffold (TICKET-0007)** — WPF/.NET 8 project `src/SGrab`, `SGrab.sln`,
  MVVM base (`ViewModelBase`, `RelayCommand`), DI host, main window with a
  "New Capture" button and filmstrip placeholder, system-tray icon, global
  hotkey (Ctrl+Shift+S) via `RegisterHotKey`, single-instance guard, and a
  capture stub behind `ICaptureService` (replaced in TICKET-0008). Builds clean.

---

## Version 1.2.0 — 2026-08-06

### Added

* **Second Brain System** - Comprehensive AI project management framework from FlowGrid
* Smart context loading via `context-load` skill (70-80% token cost reduction)
* Safe ticket numbering via `new-ticket` skill (prevents concurrent session collisions)
* Token usage tracking via `log-cost` command
* Memory archival system via `memory-archive` skill
* Enhanced CLAUDE.md with context-load integration
* Framework structure files: PROJECT_RULES.md, PROJECT_SKELETON.md, framework_version.md, project_config.md
* Enhanced memory templates: coding_conventions.md, project_status.md, tech_stack.md
* Memory archive directory structure (`.claude/memory/archive/`)
* Changelog append skill for automated changelog updates
* Definition-of-done skill for commit verification
* Helper scripts: `next_ticket.bat` and `next_ticket.js`
* Comprehensive `docs/SECOND_BRAIN.md` documentation

### Changed

* Ticket template now includes Token Usage tracking section
* Ticket naming convention documented (NNNN-Short Title.md format)
* All closed tickets renamed to new convention
* Enhanced "Every Session" workflow in CLAUDE.md to use context-load skill

---

## Version 1.1.0 — 2026-07-06

### Added

* Ticket decomposition system for managing large requests
* Parent/child ticket relationships in ticket template
* Dependency tracking between tickets
* `.claude/prompts/decomposition.md` - comprehensive decomposition workflow guide
* Decomposition guidance in CLAUDE.md and PROJECT_RULES.md

### Changed

* Enhanced ticket template with Parent Ticket, Child Tickets, and Dependencies fields
* Updated feature workflow in CLAUDE.md to include scope assessment and decomposition

---

## Version 1.0.0 — 2026-07-05

Initial template framework creation.

### Added

* `.claude/` AI operating instructions, rules, and framework version tracking.
* Persistent memory system (`.claude/memory/`): architecture, coding conventions,
  project memory, project status, tech stack, ticket memory.
* Workflow prompts (`.claude/prompts/`): feature, bugfix, refactor, release,
  project initialization, and project questionnaire.
* Reusable templates (`.claude/templates/`): README, changelog, ticket.
* Ticket system skeleton (`tickets/open`, `tickets/closed`, `tickets/archived`, `tickets/TEMPLATE.md`).
* Helper scripts (`scripts/`): setup, build, run, git commit, clear cache, release.
* Standard project skeleton (`src/`, `tests/`, `docs/`, `build/`, `releases/`, `assets/`).
* Root documentation: README, CHANGELOG, LICENSE, `.gitignore`, `version.txt`.

### Changed

*

### Fixed

*

### Removed

*
