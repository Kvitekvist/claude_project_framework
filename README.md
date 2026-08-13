# VREmulator - Virtual Reality Headset Emulator

## Overview

VREmulator is a development tool that allows you to develop and test VR applications (specifically SkyrimVR mods) without requiring physical VR hardware. It provides a virtual HMD (Head-Mounted Display) that works with SteamVR and OpenComposite, enabling you to:

- Develop and test SkyrimVR mods without a physical headset
- Automate VR application testing
- Debug VR interactions using keyboard/mouse input
- Simulate VR tracking and input for development purposes

**This project is built on the AI Project Bootstrap framework**, providing a standardized structure, workflow, and long-term memory system for maintainable AI-assisted development.

---

## Features

### Current
- Comprehensive research documentation
- Phased implementation roadmap

### Planned
- **Null Driver Configuration** - Quick setup using SteamVR's built-in null driver
- **Custom Virtual HMD** - Full-featured OpenVR driver for advanced control
- **Pose Control** - Keyboard/mouse control for head tracking simulation
- **Controller Emulation** - Virtual motion controllers
- **Input Simulation** - Map VR inputs to keyboard/mouse
- **SkyrimVR Integration** - Optimized for SkyrimVR development workflow

---

## Quick Start

### For Users

**Prerequisites**: 
- SteamVR installed
- Windows 10/11
- SkyrimVR (for testing)

**Coming Soon** - Full setup instructions will be provided once Phase 1 is implemented.

### For Developers

See [`docs/VR_EMULATOR_RESEARCH.md`](docs/VR_EMULATOR_RESEARCH.md) for comprehensive research and implementation details.

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
* **`tickets/`** — every feature and bug fix is tracked as a ticket,
  organized by category (`features/`, `bugs/`, `documentation/`,
  `infrastructure/`, `research/`) in `open/`, `closed/`, and `archived/`
  directories. See `docs/TICKET_CATEGORIES.md` for the category system.
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

## Documentation

- **[VR Emulator Research](docs/VR_EMULATOR_RESEARCH.md)** - Comprehensive research on implementation approaches
- **[CLAUDE.md](.claude/CLAUDE.md)** - AI development workflow and rules
- **[CHANGELOG.md](CHANGELOG.md)** - Project version history

---

## Technology Stack

- **Language:** C++ (for custom driver development)
- **VR APIs:** OpenVR, SteamVR
- **Build System:** Visual Studio / CMake
- **Target Platform:** Windows 10/11
- **Primary Use Case:** SkyrimVR mod development

---

## Version

VREmulator Version: 0.1.0 (Research Phase)  
Framework Version: 1.1.0

See `.claude/framework_version.md` for framework-level version history and
migration rules.

---

## License

No license has been chosen yet — see `LICENSE`.
