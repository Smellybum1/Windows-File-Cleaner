# Feature: Safety Summary Header Styling

Date started: 2026-05-30
Status: completed

## Goal

Make the collapsed Safety Summary header easier to scan by giving its compact summary lightweight state styling.

## Non-goals

- Do not add a new row, badge, modal, or help icon.
- Do not change Storage Scan results, review filters, Safety Summary shortcut behavior, Quarantine Preview, fixture execution, undo, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

The Safety Summary header should remain compact and useful while closed:

- Neutral before a scan, while the header is waiting for scan safety signals.
- Warning when the current `StorageScanSafetySummary` has review warnings.

The header should not use a success state because the Safety Summary is a read-only review readout, not a safety guarantee or cleanup approval.

The header text, tooltip, and automation help text remain read-only review context and not cleanup approval.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added lightweight neutral/warning state to `SafetySummaryHeaderText`.
- Styled the header with the existing neutral/warning color vocabulary.
- Added WPF smoke assertions for startup neutral styling and post-scan warning styling when fixture safety signals need review.
- Later packet `2026-05-30-collapsed-header-state-help-text.md` added textual header-state wording to the tooltip/help text so neutral/warning state is not color-only.
- Later packet `2026-05-30-collapsed-header-summary-labels.md` changed the visible prefix to `Safety Summary:` so the closed summary starts with the panel name.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

ADRs added or skipped:

- Skipped. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.
