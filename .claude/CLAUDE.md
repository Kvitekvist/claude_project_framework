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

Before writing code always load context via the `context-load` skill
(`.claude/skills/context-load/`):

1. Read .claude/memory/project_memory.md in full (trimmed to stay small — see its own archive note)
2. Read .claude/memory/architecture.md in full
3. Grep .claude/memory/ticket_memory.md (and .claude/memory/archive/ if needed) for what's relevant — do not read either in full by default
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
   - Otherwise: Create a new Feature ticket in appropriate category (see docs/TICKET_CATEGORIES.md)
4. **Create feature branch** from main using `feature/TICKET-####` naming
5. Update ticket during implementation.
6. Mark completed.
7. Run `/log-cost` to record token usage on the ticket.
8. Update ticket memory.
9. Commit to feature branch.
10. Push feature branch to GitHub.

Every feature MUST have a ticket.

Large features SHOULD be decomposed for better maintainability.

---

# Bug Fixes

Exactly the same workflow as features:

1. Create or find bug ticket in appropriate category (see docs/TICKET_CATEGORIES.md)
2. **Create bugfix branch** from main using `bugfix/TICKET-####` naming
3. Implement fix
4. Update ticket
5. Run `/log-cost`
6. Update ticket memory
7. Commit to bugfix branch
8. Push bugfix branch to GitHub

Never fix bugs without creating or updating a bug ticket.

NEVER commit bug fixes directly to main.

---

# Before Every Commit

Verify:

✓ On correct branch (feature/TICKET-#### or bugfix/TICKET-####, not main)
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

**CRITICAL**: Every ticket MUST have its own branch.

Before starting work on any ticket:

1. Ensure you're on main: `git checkout main`
2. Pull latest: `git pull origin main`
3. Create ticket branch: `git checkout -b feature/TICKET-#### ` or `bugfix/TICKET-####`

Branch naming:

- Features: `feature/TICKET-####`
- Bug fixes: `bugfix/TICKET-####`
- Main branch: `main`

NEVER commit ticket work directly to main.

Each ticket branch is pushed to GitHub and can become a PR.

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

---

# Research Workflow

When user requests "extensive research" or similar:

1. **Multiple web searches** (10+ minimum) across different angles
2. **Analyze specific experts/sources** when provided (GitHub accounts, etc.)
3. **Create comprehensive documentation**:
   - Separate files for different aspects (don't consolidate into one massive file)
   - RECOMMENDATIONS.md - What to build
   - ANALYSIS.md - How experts do it
   - STATUS.md - Implementation tracking
   - SUMMARY.md - Consolidated overview
4. **Cite extensively** (50+ sources minimum for major research)
5. **Synthesize findings** into actionable recommendations with priorities

Separation allows focused reading. Documentation quality over speed.

---

# Documentation Structure

For large research or implementation tasks, create separate documents:

- **RECOMMENDATIONS.md**: What to build (features, skills, components)
- **ANALYSIS.md**: How experts/industry do it (patterns, methodologies)
- **STATUS.md**: Implementation tracking (phases, progress, metrics)
- **SUMMARY.md**: Consolidated overview (for quick reference)

**Rationale**: 4 focused documents (400 lines each) > 1 massive file (1,600 lines). Readers can choose what's relevant without scrolling through everything.
