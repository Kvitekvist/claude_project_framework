---
name: new-ticket
description: Use this skill whenever creating a new ticket file in tickets/open/ — before picking a ticket number. Two separate concurrent-session collisions have already happened in this project's history (TICKET-0129/0131, and a 5-ticket collision spanning TICKET-0365-0369) from assuming "highest local number + 1" is safe. Triggers on "create a ticket", "new ticket", "what's the next ticket number".
version: 1.0.0
---

# Picking a new ticket number safely

Flowgrid tickets live at `tickets/{open,closed}/NNNN-Short Title.md`
(convention since 2026-07-23 — see `project_memory.md`'s "Conventions"
section for the full rationale). The ticket number itself is a shared
resource: two sessions working concurrently and each computing "highest
number I can see, +1" have collided twice already:

- **TICKET-0129/0131**: two entirely different, unrelated tickets ended up
  with the same number, one in `open/`, one in `closed/`.
- **TICKET-0365-0369**: a concurrent session pushed 9 commits to
  `origin/main` while another session had 9 local commits of its own, both
  branching from the same base — 5 ticket numbers collided at once.

Neither collision was caused by carelessness — both sessions correctly
scanned their own local `tickets/` directory. The actual cause was that
**local-only scanning can't see a number another session already claimed on
origin but hasn't been pulled locally yet.**

## What to do

Run the helper script instead of eyeballing a directory listing or `git
log`:

```
scripts\next_ticket.bat
```

This checks `tickets/open/` and `tickets/closed/` **both locally and on
`origin/main`** (via `git fetch` + `git ls-tree`) and returns
`max(local, origin) + 1`. If origin is ahead of local, it warns you to pull
first rather than silently numbering past a ticket you don't have yet.

If the script can't reach origin (offline, no remote configured), it falls
back to local-only and says so explicitly — treat that number as
provisional and re-check once you're back online, rather than treating the
warning as noise.

## If a collision happens anyway

It can still happen if two sessions run the script within the same window
before either pushes. If you discover one after the fact (two ticket files
with the same `NNNN` prefix, one you didn't create):

1. Renumber **your own** ticket, not the other one, unless you know the
   other session already referenced its number elsewhere (commit messages,
   cross-ticket Parent/Child fields, `ticket_memory.md`).
2. Update every cross-reference to the old number: the ticket's own
   `# TICKET-XXXX` heading, any Parent/Child/Dependency fields on other
   tickets, and any commit messages already made (commit messages
   themselves can't be rewritten after push — note the discrepancy in
   `ticket_memory.md` instead, the way the 0129/0131 and 0365-0369
   collisions are documented there rather than pretending they didn't
   happen).
3. Leave `ticket_memory.md`'s existing entries for the old number alone if
   they're already committed and pushed — add a note pointing to the
   renumbering rather than trying to rewrite history.
