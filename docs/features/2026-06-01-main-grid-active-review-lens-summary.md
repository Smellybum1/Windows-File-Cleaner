# Feature: Main Grid Active Review Lens Summary

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make Main Grid explain which read-only review lens is active after Safety Summary shortcuts or Review-tab filters/search change visible Storage Scan rows.

## Non-goals

- Do not change Storage Scan behavior.
- Do not change Storage Review Filter, Bloat Category Filter, Type filter, Size filter, Search, or display-window behavior.
- Do not change Review Shortlist membership behavior.
- Do not change Quarantine Preview, fixture execution, selected restore, manifest discovery, or readiness behavior.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not scan, create, move, restore, delete, write, or clean up real-profile files.

## Implementation

- Added compact `MainGridReviewLensText` above the Main Grid rows, below Review Grid Mode Status.
- Reused the existing Filter Summary text so Review tab and Main Grid stay synchronized.
- Added tooltip and automation help text that frame the readout as completed Storage Scan filters/search/focus only.
- Hid the readout while Current-Session Quarantined Review rows are showing because those rows do not use Storage Scan filters/search/focus.
- Kept Review Grid Mode Status responsible for the row source: Storage Scan rows versus Current-Session Quarantined Review rows.

## Test plan

Automated checks:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Manual checks:

- Run `.\tools\Start-MvpFixtureReview.cmd` and confirm Main Grid shows `Active review lens` for default scan rows, Safety Summary shortcuts, and stacked filters/search without crowding the grid header.
- Confirm the readout does not look like cleanup approval or Quarantine readiness.

## Completion notes

Completed on: 2026-06-01

What changed:

- Main Grid now mirrors the active Filter Summary above Storage Scan rows.
- WPF smoke coverage proves startup, default scan, Safety Summary shortcut, no-category shortcut, and stacked shortcut/search states update the Main Grid lens readout.
- WPF smoke coverage proves the readout hides for current-session quarantined rows and returns with Storage Scan rows.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-31-wpf-tabbed-workbench.md`
- `docs/features/2026-06-01-main-grid-auto-focus.md`
- `docs/features/2026-06-01-main-grid-active-review-lens-summary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF orientation text with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- During visible fixture review, is the extra Main Grid line helpful enough to keep, or should it be visually tighter?

Risky assumptions:

- Mirroring the existing Filter Summary avoids adding a second source of truth for active review lens wording.
