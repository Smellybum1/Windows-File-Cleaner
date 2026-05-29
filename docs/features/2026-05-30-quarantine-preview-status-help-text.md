# Feature: Quarantine Preview Status Help Text

Date started: 2026-05-30
Status: completed

## Goal

Mirror the inline Quarantine Preview readiness/status text into tooltip and WPF automation help text so the same safety boundary remains available when users hover or use assistive tech.

## Non-goals

- Do not add real-profile Quarantine execution.
- Do not add real-profile Undo Quarantine.
- Do not add permanent deletion or persisted cleanup history.
- Do not change Quarantine Preview eligibility, fixture execution, or restore behavior.

## Desired behavior

The inline Quarantine Preview status should keep its dynamic message synchronized across visible text, tooltip, and automation help text. The help text should repeat that the status is read-only review context and does not create folders, move files, restore files, delete files, or approve cleanup.

Covered states include empty shortlist, shortlisted-but-not-previewed, invalid Quarantine Root, ready preview, stale preview after root change, fixture execution evidence, fixture undo evidence, and blocked preview rows.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added tooltip and automation help text to `QuarantinePreviewStatusText`.
- Updated the dynamic status setter so help text mirrors every inline status message.
- Added WPF smoke assertions for tooltip/help text across preview, stale, blocked, execution, and undo states.
- Later packet `2026-05-30-status-state-help-text.md` added textual status-state wording to the tooltip/help text so neutral/success/warning/error state is not color-only.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`

ADRs added or skipped:

- Skipped. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.
