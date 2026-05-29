# Feature: Review Grid Mode Status Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make Review Grid Mode Status help easier to discover by adding a small visible `?` cue beside the grid-mode status line.

## Non-goals

- Do not change Storage Scan rows, Current-Session Quarantined Review rows, Quarantine Preview, fixture execution, undo, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not add a popup, modal, new approval step, or cleanup action.

## Desired behavior

- Review Grid Mode Status keeps its compact text and existing neutral/information/warning styling.
- A small circular `?` cue beside it exposes the same dynamic tooltip and automation help text as the status line.
- The cue mirrors the current `Status state:` wording and keeps read-only, no-rescan, no-restore, and not-cleanup-approval boundaries visible.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Review Grid Mode Status | Clarified visible help cue behavior. | yes |

## Evidence and validation gate

Evidence gathered:

- User said a little question mark in a circle that can be hovered over for the tooltip would be better than relying on hidden text hover alone.
- Review Grid Mode Status already mirrors dynamic status wording and status-state text into tooltip/help text.
- Existing WPF smoke tests already exercise startup, scan-row, stale scan-row, current-session quarantined, returned scan-row, and empty moved-entry states.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: the cue is read-only UI help and does not touch files.
- [x] The narrowest relevant verification path is known.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a visible circular `?` help cue beside Review Grid Mode Status.
- Mirrored the dynamic Review Grid Mode Status tooltip and automation help text onto the cue.
- Added WPF smoke assertions for cue automation name, cue tooltip, and cue automation help text across the existing grid-mode states.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

ADRs added or skipped:

- Skipped. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the `?` cue is noticeable without crowding the main-grid area.

Risky assumptions:

- A compact non-clickable `?` cue is enough to make the grid-mode safety boundary discoverable without adding a larger badge or another explanatory row.
