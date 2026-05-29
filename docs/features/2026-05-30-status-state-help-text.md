# Feature: Status State Help Text

Date started: 2026-05-30
Status: completed

## Goal

Make styled Review Grid Mode Status and inline Quarantine Preview readiness/status state available through tooltip and automation help text, not only through color and font weight.

## Non-goals

- Do not add a new row, badge, modal, popup, or help icon.
- Do not change Storage Scan rows, Review Shortlist membership, Quarantine Preview eligibility, fixture execution, undo, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

Styled status lines should keep their visible text and visual state styling, while their tooltip and automation help text also name the current status state:

- Review Grid Mode Status: neutral, information, or warning.
- Quarantine Preview readiness/status: neutral, success, warning, or error.

The state wording remains read-only review context and not cleanup approval.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added textual `Status state:` wording to Review Grid Mode Status tooltip/help text.
- Added textual `Status state:` wording to inline Quarantine Preview readiness/status tooltip/help text.
- Strengthened shared WPF smoke assertions so every exercised grid-mode and preview-status state must expose matching state text.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

ADRs added or skipped:

- Skipped. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.
