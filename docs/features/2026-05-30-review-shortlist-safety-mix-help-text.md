# Feature: Review Shortlist Safety Mix Help Text

Date started: 2026-05-30
Status: completed

## Goal

Mirror Review Shortlist Safety Mix wording into tooltip and WPF automation help text so its safety boundary is available outside the visible text line.

## Non-goals

- Do not change Review Shortlist membership, Storage Scan rows, Quarantine Preview eligibility, fixture execution, undo, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not add a new row, badge, modal, popup, or help icon.

## Desired behavior

Review Shortlist Safety Mix should keep its visible compact summary. Its tooltip and automation help text should mirror the current dynamic text and say the readout is read-only review context:

- It does not rescan.
- It does not modify files.
- It does not prove Quarantine readiness.
- It does not prove storage savings.
- It does not approve cleanup.

Covered states include startup/empty, populated shortlist, and empty-after-removal states.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added static startup tooltip and automation help text to `ShortlistSafetyMixText`.
- Updated the dynamic setter so empty and populated Review Shortlist Safety Mix text is mirrored into tooltip/help text.
- Added WPF smoke assertions for empty, populated, and empty-after-removal states.
- Later packet `2026-05-30-review-shortlist-safety-mix-help-cue.md` added a visible circular `?` help cue that mirrors the same dynamic tooltip and automation help text.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

ADRs added or skipped:

- Skipped. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.
