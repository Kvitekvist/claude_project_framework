# SGrab

A fast, personal Windows screenshot tool inspired by Snagit.

## What it does

- **One-click capture** — press the button or a global hotkey (**Ctrl+Shift+S**)
  to immediately start a region-select screenshot.
- **Quick annotation editor** — mark up captures with numbered step bubbles
  (1, 2, 3…), circles, squares, and text, and change any item's color easily.
- **History filmstrip** — scroll through past screenshots along the bottom and
  reopen any of them in the editor.

## Status

Early development. The MVP is decomposed into tickets under
`tickets/open/features/` (parent **TICKET-0006**, children 0007–0014):

| Phase | Ticket | Feature | State |
|-------|--------|---------|-------|
| 1 Foundation | 0007 | App scaffold & shell | Implemented (building) |
| 1 Foundation | 0008 | Capture engine (region select) | Planned |
| 1 Foundation | 0009 | Storage & history model | Planned |
| 2 Editor | 0010 | Editor window + annotation canvas | Planned |
| 2 Editor | 0011 | Annotation tools + color | Planned |
| 2 Editor | 0012 | Export & clipboard | Planned |
| 3 Ship | 0013 | Filmstrip history bar | Planned |
| 3 Ship | 0014 | Build & packaging | Planned |

## Tech stack

- **C# / .NET 8 (LTS)**, **WPF**, MVVM with dependency injection
  (`Microsoft.Extensions.Hosting`).
- Windows desktop, x64.

## Build & run

Requires the **.NET 8 SDK**.

```bash
dotnet build SGrab.sln -c Debug
dotnet run --project src/SGrab/SGrab.csproj
```

A one-command self-contained build (`scripts/build.bat`) arrives in TICKET-0014.

## Project layout

```
src/SGrab/          WPF application (Views, ViewModels, Services, Common)
tickets/            Ticket-tracked work (features/bugs/… under open/closed)
docs/               Project documentation
scripts/            Helper batch scripts
.claude/            AI-assisted development framework (memory, skills, rules)
```

## Development workflow

This repo uses a ticket-driven workflow: every feature or bug fix gets a ticket
and its own branch (`feature/TICKET-####`), documentation is updated alongside
code, and `.claude/memory/` holds the project's source-of-truth context. See
`.claude/CLAUDE.md` for the full operating rules.

## License

See `LICENSE`.
