# Feature: Collapsed Header State Help Text

Date started: 2026-05-30
Status: completed

## Goal

Make collapsed Safety Summary and Quarantine shortlist header state available through tooltip and automation help text, not only through color.

## Non-goals

- Do not add a new row, badge, modal, or popup. Later packet `2026-05-30-collapsed-header-help-cues.md` added compact visible `?` help cues without changing header state semantics.
- Do not change Storage Scan results, Review Shortlist membership, Quarantine Preview eligibility, fixture execution, undo, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

Collapsed panel headers should keep their compact summaries and visual state styling, while their tooltip and automation help text also name the current header state:

- Safety Summary: neutral or needs review.
- Quarantine shortlist: neutral, needs review, ready/completed, or current-session quarantined review.

The state wording remains read-only review context and not cleanup approval.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added textual header-state wording to Safety Summary header tooltip/help text.
- Added textual header-state wording to Quarantine shortlist header tooltip/help text.
- Added WPF smoke assertions for startup neutral state, Safety Summary needs-review state, Quarantine shortlist needs-review state, preview-ready/completed state, and current-session quarantined review state.
- Later packet `2026-05-30-collapsed-header-summary-labels.md` made the same compact headers start with the visible panel names without changing header state semantics.
- Later packet `2026-05-30-collapsed-header-help-cues.md` mirrored the same state wording onto visible circular `?` help cues for both headers.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

ADRs added or skipped:

- Skipped. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.
