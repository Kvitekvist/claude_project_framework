# AI Project Bootstrap Template

## Overview

This is a copy-pasteable starting point for new AI-assisted software projects.
It provides a standardized structure, workflow, and long-term memory system so
that any project built from it stays maintainable over months of AI-assisted
development, rather than accumulating undocumented, untracked changes.

---

## How to Use This Template

1. Copy this entire folder to a new location and rename it for the new project.
2. Open the project in Claude Code.
3. Answer `.claude/prompts/project_questionnaire.md` (project name, stack,
   git details, quality requirements, etc.).
4. Let Claude follow `.claude/prompts/project_init.md` to fill in
   `.claude/project_config.md`, the memory files, scripts, and root
   documentation for the new project.
5. Start requesting features and bug fixes — every change is tracked through
   the ticket system described below.

---

## How It Works

* **`.claude/CLAUDE.md`** — the operating instructions Claude follows in this
  repository: when to create tickets, what to update before every commit, and
  the git workflow to use.
* **`.claude/PROJECT_RULES.md`** — the definition of done for any ticket.
* **`.claude/PROJECT_SKELETON.md`** — the canonical folder/file layout for any
  project built from this template.
* **`.claude/memory/`** — persistent, continuously-updated project memory
  (architecture, tech stack, coding conventions, project status,
  ticket history). Read at the start of every session.
* **`.claude/prompts/`** — step-by-step workflows for features, bug fixes,
  refactors, releases, and initializing a brand-new project.
* **`.claude/templates/`** — reusable templates for README, CHANGELOG, and
  ticket files.
* **`tickets/`** — every feature and bug fix is tracked as a ticket
  (`open/`, `closed/`, `archived/`), based on `tickets/TEMPLATE.md`.
* **`scripts/`** — helper batch scripts (`setup`, `build`, `run`,
  `git_commit`, `clear_cache`, `release`), customized per project's stack.

---

## Project Structure

See `.claude/PROJECT_SKELETON.md` for the full, authoritative layout.

---

## Development Workflow

* Every feature or bug fix requires a ticket before code is written.
* Documentation (README, architecture, changelog) is updated
  alongside the code change, not after the fact.
* Commits follow the format `[TICKET-####] Short description`.
* Memory files are the source of truth for project context across sessions.

---

## Version

Framework Version: 1.0.0

See `.claude/framework_version.md` for framework-level version history and
migration rules.

---

## License

No license has been chosen yet — see `LICENSE`.
