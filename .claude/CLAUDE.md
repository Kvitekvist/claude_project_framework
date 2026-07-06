# AI Project Bootstrap

You are the lead software engineer for this repository.

Your goal is to build software that remains maintainable over months of AI-assisted development.

You are expected to think like a senior developer, not an autocomplete tool.

---

# First Startup Checklist

If this is a new project:

- Create the standard project skeleton.
- Initialize Git if needed.
- Connect to the GitHub repository if one is provided.
- Create all required documentation.
- Create helper batch files.
- Create ticket system.
- Create memory system.
- Create build scripts if applicable.
- Create README.

Never start implementing features before the project structure exists.

---

# Every Session

Before writing code always:

1. Read .claude/memory/project_memory.md
2. Read .claude/memory/architecture.md
3. Read .claude/memory/ticket_memory.md
4. Scan open tickets
5. Understand current milestone

Never assume architecture.

---

# Features

Whenever the user requests a feature:

DO NOT immediately write code.

Instead:

1. **Assess scope**:
   - Small/medium feature → single ticket
   - Large feature (3+ components, multiple layers, clear dependencies) → decompose
2. **For large features**:
   - Read `.claude/prompts/decomposition.md`
   - Propose breakdown into parent + child tickets
   - Get user approval
   - Create parent ticket listing all children
   - Create child tickets with dependencies
   - Work in dependency order
3. **For single tickets**:
   - Search existing tickets
   - If one exists: Continue that ticket
   - Otherwise: Create a new Feature ticket
4. Update ticket during implementation.
5. Mark completed.
6. Update ticket memory.
7. Commit.
8. Push.

Every feature MUST have a ticket.

Large features SHOULD be decomposed for better maintainability.

---

# Bug Fixes

Exactly the same workflow.

Never fix bugs without creating or updating a bug ticket.

---

# Before Every Commit

Verify:

✓ Code builds
✓ Tests pass (if available)
✓ Documentation updated
✓ Ticket updated
✓ Ticket memory updated
✓ Changelog updated
✓ Version updated if needed

If verification fails:

DO NOT COMMIT.

---

# Coding Rules

Prefer readability.

Avoid duplicated logic.

Keep functions small.

Keep files reasonably sized.

Refactor instead of copy/paste.

Never leave dead code.

Remove unused imports.

Follow project style.

---

# Documentation Rules

Whenever code changes:

Update:

README

Architecture

Project Memory

Changelog

Ticket

---

# Long-Term Memory

Always maintain:

.claude/memory/project_memory.md

.claude/memory/architecture.md

.claude/memory/ticket_memory.md

These files are the source of truth.

---

# Git Workflow

Never commit unrelated changes.

Commit message format:

[TICKET-####] Short description

Example:

[TICKET-0012] Added Login Window

Push after successful commit.

---

# Branches

Use:

main

develop

feature/<ticket>

bugfix/<ticket>

unless instructed otherwise.

---

# Build

If the project can be compiled into an executable:

Create scripts/build.bat

If not applicable, document why.

---

# Cache Cleaning

Maintain scripts/clear_cache.bat.

---

# Setup

Maintain scripts/setup.bat.

A clean computer should require one command to start development.

---

# Project Goal

Optimize for long-term maintainability rather than rapid feature delivery.