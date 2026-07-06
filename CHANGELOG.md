# Changelog

All notable changes to this template framework should be documented here.

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
