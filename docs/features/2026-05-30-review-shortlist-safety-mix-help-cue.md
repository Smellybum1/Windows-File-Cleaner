# Feature: Review Shortlist Safety Mix Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make Review Shortlist Safety Mix help easier to discover by adding a small visible `?` cue beside the summary line.

## Non-goals

- Do not change Review Shortlist membership, Storage Scan rows, Quarantine Preview eligibility, fixture execution, undo, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not add a popup, modal, new approval step, or clickable cleanup action.

## Desired behavior

- Review Shortlist Safety Mix keeps its compact summary text.
- A small circular `?` cue beside it exposes the same dynamic tooltip and automation help text as the summary.
- The cue reads as help context only, not cleanup approval or Quarantine readiness.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Review Shortlist Safety Mix | Clarified visible help cue behavior. | yes |

## Evidence and validation gate

Evidence gathered:

- User said a little question mark in a circle that can be hovered over for the tooltip would be better than relying on hidden text hover alone.
- Review Shortlist Safety Mix already mirrors dynamic safety wording into tooltip/help text.
- Existing WPF smoke tests already exercise empty, populated, and empty-after-removal shortlist states.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: the cue is read-only UI help and does not touch files.
- [x] The narrowest relevant verification path is known.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a visible circular `?` help cue beside Review Shortlist Safety Mix.
- Mirrored the dynamic Review Shortlist Safety Mix tooltip and automation help text onto the cue.
- Added WPF smoke assertions for cue automation name, cue tooltip, and cue automation help text.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

ADRs added or skipped:

- Skipped. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the `?` cue is noticeable without crowding the Review Shortlist toolbar area.

Risky assumptions:

- A compact non-clickable `?` cue is enough to make the safety boundary discoverable without turning the line into a larger table or chips layout.
