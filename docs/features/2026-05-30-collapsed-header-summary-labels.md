# Feature: Collapsed Header Summary Labels

Date started: 2026-05-30
Status: completed

## Goal

Make collapsed Safety Summary and Quarantine Shortlist header summaries easier to scan by starting each header with the visible panel name.

## Non-goals

- Do not add a new row, badge, modal, or popup. Later packet `2026-05-30-collapsed-header-help-cues.md` added compact visible `?` help cues without changing the panel-name header labels.
- Do not change Storage Scan rows, Review Shortlist membership, Quarantine Preview eligibility, fixture execution, undo, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

The existing compact header summaries remain available while panels are collapsed, but the first words clearly identify which panel the summary belongs to:

- `Safety Summary: ...`
- `Quarantine Shortlist: ...`

The tooltip/help text still mirrors the dynamic header, names the current header state, and says the header summary is read-only review context, not cleanup approval.

## Completion notes

Completed on: 2026-05-30

What changed:

- Renamed the Safety Summary header prefix from sentence-style `Scan safety summary:` to `Safety Summary:`.
- Renamed the Quarantine shortlist header prefix to `Quarantine Shortlist:`.
- Aligned the initial Quarantine Shortlist XAML header with the dynamic runtime summary by including `undo unavailable`.
- Added WPF smoke assertions that both headers start with the visible panel name.
- Later packet `2026-05-30-collapsed-header-help-cues.md` added visible circular `?` help cues that mirror the same panel-name header summaries and state help text.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

ADRs added or skipped:

- Skipped. This is reversible WPF readability polish with no persistence, cleanup execution, restore rule, data-model, or security change.
