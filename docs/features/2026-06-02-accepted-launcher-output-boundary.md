# Feature: Accepted Launcher Output Boundary

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make the accepted package launcher output itself repeat that the accepted release launch command is the daily path, not any debug-build desktop shortcut.

## Non-goals

- Do not create, update, or delete any shortcut.
- Do not create an installer, Start Menu entry, service, scheduled task, or background automation.
- Do not publish or accept a new package.
- Do not launch WPF, click `Scan`, scan, move, restore, delete, approve cleanup, or create cleanup history.
- Do not enable broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, or persisted cleanup history.

## Context

Tracked docs now clarify that debug-build desktop shortcuts are development-only context, but day-to-day use happens through terminal launch commands. `Start-AcceptedLocalRelease.cmd -PrintOnly` should carry the same boundary in its output so the safest launch path is visible without re-reading docs.

## Implementation

- Added one accepted-launcher output line after accepted notes/package evidence.
- The line names the accepted release launch command as the daily path instead of any debug-build desktop shortcut.
- The same line states that the launcher does not create shortcuts or install anything.
- Updated README, handoff, roadmap, launch-path clarity notes, and progress docs.

## Verification

- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly`
- `rg -n "Daily path: use this accepted release launch command|debug-build desktop shortcut|Accepted Launcher Output Boundary" tools README.md docs\codex\thread-handoff.md .codex\progress.md docs\features`
- `git diff --check`

## Docs

- `README.md`
- `docs/features/2026-06-02-accepted-package-launch-path-clarity.md`
- `docs/features/2026-06-02-accepted-launcher-output-boundary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is terminal-output wording for existing accepted-package tooling and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Follow-up Work

- Consider installed shortcut or installer automation only as a later explicit user-approved packaging packet.
- Keep daily accepted package verification read-only and print-only unless the user intentionally launches the package.

## Risks And Assumptions

- Repeating the boundary in terminal output reduces launch-path mistakes more than it adds noise.
- The accepted package remains the right daily-use baseline until a fresh package is intentionally cut and accepted.
