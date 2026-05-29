# Feature: Quarantine Shortlist Header Styling

Date started: 2026-05-30
Status: completed

## Goal

Make the collapsed Quarantine shortlist header easier to scan by giving its compact summary lightweight semantic styling.

## Non-goals

- Do not add a new row, badge, modal, or help icon.
- Do not change Storage Scan rows, Review Shortlist membership, Quarantine Preview eligibility, fixture execution, undo, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

The Quarantine shortlist header should remain compact and useful while closed, but its color should reflect the current review state:

- Neutral when no shortlist, preview, current quarantine, or undo state exists.
- Warning when shortlisted rows still need preview, preview destinations are stale, preview is blocked, or recovery review is needed.
- Success when preview readiness is clean or fixture undo completed.
- Information when current-session quarantined rows exist.

The header text, tooltip, and automation help text remain read-only review context and not cleanup approval.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added semantic state to `QuarantineShortlistHeaderText`.
- Styled the header using the existing neutral/success/information/warning color vocabulary.
- Added WPF smoke assertions for startup, needs-preview, invalid-root, clean preview, stale preview, blocked preview, current quarantined, and undo-completed header states.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

ADRs added or skipped:

- Skipped. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.
