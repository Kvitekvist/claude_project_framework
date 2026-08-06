# Second Brain System - Installation Complete

This document describes the "second brain" system that has been installed from FlowGrid into this Template repository.

---

## What is the Second Brain?

The second brain is a comprehensive AI project management system designed to maintain long-term project memory across multiple Claude Code sessions. It ensures that Claude always has the right context without wasting tokens on irrelevant historical data.

---

## Key Components

### 1. Enhanced CLAUDE.md

The main instruction file now integrates with the `context-load` skill for smart, token-efficient memory loading:

- **Before**: Read all memory files in full every session (~150K+ tokens)
- **After**: Load only relevant context via grep and targeted reads

### 2. Framework Structure

- **PROJECT_RULES.md**: Development rules and ticket decomposition workflow
- **PROJECT_SKELETON.md**: Standard project layout definition
- **framework_version.md**: Framework versioning (currently 1.1.0)
- **project_config.md**: Project-specific configuration

### 3. Enhanced Memory System

`.claude/memory/` contains:

- **project_memory.md**: Project vision, milestones, priorities, technical debt, completed work
- **architecture.md**: Current system architecture and design principles
- **ticket_memory.md**: Chronological log of all completed tickets
- **coding_conventions.md**: Coding standards and best practices
- **project_status.md**: Current project status metrics
- **tech_stack.md**: Technology stack documentation
- **archive/**: Historical memory entries (grep only, never read in full)

### 4. Smart Skills

`.claude/skills/` contains:

- **context-load**: Smart memory loading at session start (grep instead of full reads)
- **new-ticket**: Safe ticket number assignment (prevents concurrent session collisions)
- **node-map**: Interactive HTML visualization of project structure as radial node graph
- **changelog-append**: Automated changelog updates
- **definition-of-done**: Checklist verification before closing tickets
- **memory-archive**: Archive old entries to keep memory files small

### 5. Commands

`.claude/commands/`:

- **log-cost**: Record token usage from `/cost` output into ticket files

### 6. Enhanced Prompts

`.claude/prompts/`:

- **feature.md**: Feature ticket workflow
- **bugfix.md**: Bug fix workflow
- **decomposition.md**: Large feature decomposition guide
- **refactor.md**: Refactoring workflow
- **release.md**: Release preparation workflow
- **project_init.md**: New project initialization
- **project_questionnaire.md**: Project setup questions

### 7. Helper Scripts

`scripts/`:

- **next_ticket.bat/js**: Safely get the next available ticket number (checks both local and origin)

### 8. Enhanced Ticket Template

`tickets/TEMPLATE.md` now includes:

- Filename convention documentation
- Category field for subfolder organization
- Parent/Child/Dependencies fields for ticket decomposition
- Token Usage tracking section (populated by `/log-cost`)

### 9. Ticket Category System

Tickets organized in category-based subfolders for scalability:

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

See `docs/TICKET_CATEGORIES.md` for the complete category guide.

---

## How It Works

### Every Session Workflow

1. **Context Loading** (via `context-load` skill):
   - Read `project_memory.md` in full (~600 lines, trimmed to essentials)
   - Read `architecture.md` in full if needed
   - **Grep** `ticket_memory.md` for relevant tickets (don't read in full)
   - Scan open tickets directory
   - Understand current milestone

2. **Feature Development**:
   - Assess scope (small → single ticket, large → decompose)
   - For large features: Read `decomposition.md`, propose breakdown, get approval
   - Create/update ticket using `new-ticket` skill
   - Implement and update ticket during work
   - Run `/log-cost` to record token usage
   - Update ticket memory
   - Commit and push

3. **Memory Management**:
   - Keep memory files small and focused on current state
   - Archive historical entries when files grow large (via `memory-archive` skill)
   - Use grep for historical lookups instead of full reads

---

## Token Efficiency

### Before Second Brain

- Full memory read every session: ~150K tokens
- No smart filtering
- Historical noise mixed with current state
- Memory files growing indefinitely

### After Second Brain

- Targeted memory loading: ~30-50K tokens
- Grep-based historical lookups
- Clear separation of current state vs. archive
- Automatic archival keeps files small

**Token savings**: ~70-80% reduction in context loading costs

---

## Key Conventions

### Ticket Naming

Tickets are named: `tickets/{open,closed}/[category]/NNNN-Short Title.md`

- Category subfolder (features/bugs/documentation/infrastructure/research)
- 4-digit ticket number (no `TICKET-` prefix in filename)
- Hyphen separator
- Short descriptive title
- Internal heading still uses `# TICKET-XXXX`
- Flat structure still supported for backward compatibility

### Ticket Number Assignment

Always use `scripts/next_ticket.bat` to get the next ticket number:

- Checks both local and `origin/main` (prevents concurrent session collisions)
- Scans both flat structure and category subfolders
- Warns if origin is ahead (prompts you to pull first)
- Falls back to local-only if offline (with explicit warning)

### Commit Message Format

```
[TICKET-####] Short description
```

Example: `[TICKET-0012] Added Login Window`

---

## Skills Reference

### /context-load (Automatic)

Loads session context efficiently:
- Full read of `project_memory.md` and `architecture.md`
- Grep-based lookup of `ticket_memory.md`
- Scans open tickets directory

### /new-ticket

Creates a new ticket safely:
1. Runs `scripts/next_ticket.bat` to get next available number
2. Determines appropriate category (features/bugs/documentation/infrastructure/research)
3. Creates ticket file in category subfolder with proper naming convention
4. Updates ticket memory when closed

### /node-map

Generates interactive project visualization:
1. Scans project structure (memory, skills, tickets, docs, source)
2. Creates self-contained HTML with radial node graph
3. Outputs to `docs/node-map.html`
4. Opens directly in any browser (no dependencies)
5. Interactive: zoom, pan, drag, click to explore connections

### /log-cost

Records token usage on tickets:
1. Paste `/cost` output when prompted
2. Parses input/output/cache tokens and cost
3. Appends to ticket's Token Usage table
4. Updates Total row

### /changelog-append

Appends entries to CHANGELOG.md following project format

### /definition-of-done

Verifies all completion criteria before closing a ticket:
- Code implemented
- Tested
- Documentation updated
- Memory updated
- No regressions
- Successfully committed

### /memory-archive

Archives old entries from memory files to `.claude/memory/archive/`:
- Keeps live files small and fast to read
- Preserves all history (nothing deleted)
- Enables grep-based historical lookups

---

## Best Practices

### 1. Always Use Context-Load

Don't read memory files manually at session start. The `context-load` skill does it efficiently.

### 2. Decompose Large Features

Features with 3+ components should be decomposed into parent + child tickets:
- Read `.claude/prompts/decomposition.md`
- Propose breakdown
- Get user approval
- Create parent listing all children
- Create child tickets with dependencies
- Work in dependency order

### 3. Record Token Usage

After every significant work session:
1. Run `/cost` to get usage
2. Run `/log-cost` to record it on the ticket
3. This builds a historical record of project token costs

### 4. Keep Memory Current

- Update `project_memory.md` when project state changes
- Update `architecture.md` when structure changes
- Append to `ticket_memory.md` when tickets close
- Archive old entries periodically to keep files small

### 5. Never Read Archives in Full

`.claude/memory/archive/` files exist for grep only:
- Use targeted grep searches for specific tickets or keywords
- Full archive reads waste tokens
- If grep doesn't find it, widen search terms before falling back to full read

---

## Migration from FlowGrid

The following components were successfully copied:

✅ Enhanced CLAUDE.md with context-load integration
✅ Framework structure (PROJECT_RULES, PROJECT_SKELETON, framework_version)
✅ Enhanced memory templates (6 memory files + archive structure)
✅ All prompts (7 workflow guides)
✅ Core skills (6 essential skills: context-load, new-ticket, node-map, changelog-append, definition-of-done, memory-archive)
✅ Commands (log-cost)
✅ Helper scripts (next_ticket.bat/js with subfolder support)
✅ Enhanced ticket template with Category and Token Usage tracking
✅ Ticket category system (features/bugs/documentation/infrastructure/research)
✅ Interactive node-map visualization

---

## What's Not Included

The following FlowGrid-specific components were NOT copied (project-specific):

- Project-specific memory content (you'll build your own)
- Closed ticket history (starts fresh)
- Project-specific skills (duckdb-diagnosis, pushdown-parity, etc.)
- Run/verify skills (app-specific)

---

## Next Steps

1. **Initialize Your Project**:
   - Update `project_config.md` with your project details
   - Fill in `tech_stack.md` with your technology choices
   - Update `project_memory.md` with your project vision
   - Update `architecture.md` with your architecture

2. **Start Using Tickets**:
   - Run `scripts/next_ticket.bat` to get your first ticket number
   - Create tickets using the enhanced TEMPLATE.md
   - Follow the feature/bugfix workflows in `.claude/prompts/`

3. **Build Your Memory**:
   - As you work, update memory files
   - Use `/log-cost` to track token usage
   - Let the system grow organically with your project

4. **Leverage Skills**:
   - Use `context-load` at session start
   - Use `new-ticket` when creating tickets (handles categories automatically)
   - Use `node-map` to visualize project structure
   - Use `log-cost` after work sessions
   - Use `memory-archive` when memory files grow large

5. **Explore the Node Map**:
   - Run `/node-map` to generate `docs/node-map.html`
   - Opens in browser to see your project's "brain" as an interactive graph
   - Central node (CLAUDE.md) connected to categories (Memory, Skills, Tickets, etc.)
   - Zoom, pan, drag nodes to explore structure
   - Click nodes to highlight connections

---

## Framework Version

Current Version: **1.2.0**

This version includes:
- Ticket decomposition workflow
- Parent/child ticket relationships
- Dependency tracking
- Token usage tracking
- Smart context loading
- Memory archival system
- **Ticket category system** (features/bugs/documentation/infrastructure/research)
- **Interactive node-map visualization** (radial graph of project structure)
- Enhanced next_ticket.js with subfolder support

---

## Support

For questions or issues with the second brain system:
- Read the skill documentation in `.claude/skills/*/SKILL.md`
- Check the prompt guides in `.claude/prompts/`
- Review the PROJECT_RULES.md for workflow details

---

## Summary

The second brain system transforms Claude Code from a stateless tool into a persistent development partner with:

- **Long-term memory** across sessions
- **Token-efficient** context loading (70-80% savings)
- **Smart workflows** for features, bugs, and decomposition
- **Historical tracking** without token waste
- **Concurrent-safe** ticket management
- **Organized ticket categories** for scalability
- **Interactive visualization** of project structure
- **Automatic documentation** of project evolution

Enjoy building with your new second brain! 🧠

---

## Visualizing Your Brain

Use `/node-map` to generate an interactive visualization of your project:

- **Center**: CLAUDE.md (your project's brain)
- **Categories**: Memory, Skills, Tickets, Docs, Source, Scripts
- **Connections**: Visual graph showing relationships
- **Interactive**: Zoom, pan, drag nodes, click to explore

The node map updates dynamically as your project grows, providing a living diagram of your second brain's structure.
