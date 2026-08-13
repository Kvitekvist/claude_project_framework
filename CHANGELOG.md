# Changelog

All notable changes to this project should be documented here.

---

## Version 0.1.0 — 2026-08-13 (Phase 1: Null Driver)

### Added

* **Null Driver Configuration System** [TICKET-0028]
  - Automated scripts to enable/disable SteamVR null driver
  - Configuration templates for null driver and global SteamVR settings
  - Backup/restore functionality for safe configuration changes
  - NULL_DRIVER_SETUP.md comprehensive guide
  - Support for SkyrimVR launch without physical VR headset

* **Project Research and Planning** [TICKET-0027]
  - VR_EMULATOR_RESEARCH.md with 40+ sources analyzed
  - Parent/child ticket structure (TICKET-0027 through TICKET-0033)
  - Updated architecture and project memory documentation
  - Phased implementation roadmap

### Files Added

* `config/null_driver/enable_null.bat` - Enable null driver script
* `config/null_driver/disable_null.bat` - Disable null driver script
* `config/null_driver/templates/null_enabled.vrsettings` - Null driver config
* `config/null_driver/templates/steamvr_global.vrsettings` - Global config
* `docs/NULL_DRIVER_SETUP.md` - Setup and troubleshooting guide
* `docs/VR_EMULATOR_RESEARCH.md` - Research documentation
* `tickets/open/features/0027-*.md` through `0033-*.md` - Implementation tickets

---

## Version 1.2.0 — 2026-08-06 (Framework)

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
