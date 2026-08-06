---
name: node-map
description: Generate an interactive, self-contained HTML "node network" dashboard that visualizes a project's brain - its central CLAUDE.md, memory files, skills, tickets, docs, source, and other conventional folders - as a dark, glowing radial graph. Use when the user asks to visualize, map, or chart the project's structure/memory/skills as a node graph, network diagram, or "brain" dashboard, or explicitly invokes /node-map.
---

# node-map

Produces one self-contained HTML file that renders the target project as a radial
node graph: a central "brain" node (CLAUDE.md, or the project itself), a ring of
category nodes (one per conventional folder that actually exists), and a cluster
of file nodes around each category. No build step, no dependencies - the file
opens directly in any browser.

You (Claude) do the scanning and data-building yourself, live, each time this
skill runs. There is no separate generator script to execute.

## Inputs

- **Target project root**: the current working directory, unless the user names
  a different path.
- **Output path**: `<target>/docs/node-map.html`, unless the user asks for a
  different location. Create `docs/` if it doesn't exist.

## Step 1 - Discover categories

A "category" is a top-level grouping worth its own cluster in the graph. Check
for these, in this order, but **only include ones that actually exist** - this
skill must degrade gracefully on projects that don't follow this repo's exact
layout:

| Category label | Typical source                                                        |
| --------------- | ---------------------------------------------------------------------- |
| Memory          | `.claude/memory/*.md`                                                  |
| Skills           | `.claude/skills/*/SKILL.md` (one node per skill folder)                |
| Tickets          | `tickets/open/*.md`, `tickets/closed/*.md` (split into two categories if both are non-trivial in size, otherwise merge) |
| Docs             | `docs/**`, root-level `README.md`, `CHANGELOG.md`                      |
| Source           | `src/**`                                                                |
| Tests            | `tests/**`                                                              |
| Scripts          | `scripts/**`                                                            |
| Assets           | `assets/**`                                                             |
| Prompts/Templates | `.claude/prompts/*.md`, `.claude/templates/*.md`                     |

If the project has none of these (an unfamiliar layout), fall back to its actual
top-level folders (excluding noise, see below) as categories instead of
forcing it into this list.

**Always exclude** (never list individually, never recurse into):
`.git`, `node_modules`, `.venv`, `__pycache__`, `dist`, `build`, `.vs`,
`.vscode`, any folder already covered as a category output path itself
(e.g. don't recurse into `docs/node-map.html`'s own directory listing infinitely
- just skip prior generated node-map HTML files), binary build artifacts,
`.gitkeep` placeholder files, and any file that may hold secrets (`.env` and
similar) - never surface its name or path in the graph.

Skip a category entirely (don't render an empty cluster) if, after exclusions,
it has zero files.

## Step 2 - Collect file metadata per category

Prefer one shell call per category over reading files individually - this is
metadata only (name, size, modified date), not file content.

PowerShell (Windows, primary in this environment):

```powershell
Get-ChildItem -Path ".claude/memory" -File | Select-Object Name, Length, LastWriteTime | ConvertTo-Json
```

Bash equivalent (if running on a POSIX shell / non-Windows host):

```bash
find .claude/memory -maxdepth 1 -type f -printf '%f\t%s\t%TY-%Tm-%TdT%TH:%TM:%TS\n'
```

For each file, capture: `label` (filename), `size` (bytes, format to a short
human string like `"2.1 KB"`), `modified` (ISO date or relative "3d ago" - your
choice, keep it short), and `path` (repo-relative path for the tooltip).

**Cap large categories.** If a category has more than ~60 files, include the 60
most recently modified and fold the rest into one synthetic node labeled
`"+N more"` (no path) so the layout stays legible instead of choking on
hundreds of dots.

## Step 3 - Determine the center node

- `label`: `"CLAUDE.md"` if a CLAUDE.md exists at the project root or in
  `.claude/`, otherwise the project's folder name.
- `sublabel`: a short phrase - e.g. the project name from `project_config.md`,
  or `"project brain"` if nothing better is available.

## Step 4 - Build the data block

Assemble JSON matching this schema:

```json
{
  "project": "string - project name",
  "generated": "ISO 8601 timestamp",
  "center": { "label": "string", "sublabel": "string" },
  "categories": [
    {
      "id": "short-slug",
      "label": "Display Label",
      "nodes": [
        { "label": "filename.ext", "meta": "2.1 KB · 3d ago", "path": "relative/path" }
      ]
    }
  ]
}
```

Category `color` is optional - omit it and the template assigns one from its
built-in palette automatically, in category order. Only set `color` explicitly
if the user asks for specific colors.

## Step 5 - Inject and write the output file

1. Read `.claude/skills/node-map/assets/template.html` (this skill's engine -
   do not modify it in place; it's the reusable source of truth for every
   project this skill runs against).
2. Replace the contents of the
   `<script id="node-map-data" type="application/json">...</script>` block with
   your generated JSON (the placeholder demo data lives there by default -
   overwrite the whole block's inner text).
3. If any string value could contain the literal sequence `</script>` (rare -
   e.g. a filename), escape it as `<\/script>` so it doesn't terminate the tag
   early.
4. Write the result to `<target>/docs/node-map.html` (creating `docs/` if
   needed). Never edit the template file itself as part of a run.

## Step 6 - Report back

Tell the user: the output path, the category/node counts, and that it opens
directly in any browser (double-click, or `start docs/node-map.html` on
Windows / `open` on macOS). Mention the controls: scroll to zoom, drag to pan,
drag a node to reposition it, click to inspect/highlight connections,
double-click to fly to a node, click empty space to reset.

## Notes on the engine itself

`assets/template.html` is a hand-built, dependency-free force-directed graph
(no D3, no CDN) so the output works fully offline. It re-simulates physics
continuously in the browser (gentle ambient motion, never fully freezes) and
supports pan/zoom/drag/hover/click/double-click out of the box - none of that
needs to be regenerated per project. Your only job each run is producing the
JSON data block; treat the rest of the file as a stable library.

If you improve the engine itself (new interaction, visual fix, perf work), edit
`assets/template.html` directly and it improves every future project this
skill is used on - just make sure the demo placeholder data block still parses
and renders on its own, since that's what a fresh copy of this skill ships with.
