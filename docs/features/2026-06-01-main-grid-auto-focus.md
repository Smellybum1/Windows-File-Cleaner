# Feature: Main Grid Auto-Focus for Grid-Switching Actions

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make the tabbed workbench feel less surprising when a control changes which rows the Main Grid shows.

## Non-goals

- Do not change Storage Scan behavior.
- Do not change Review Shortlist membership behavior.
- Do not change Quarantine Preview, fixture execution, selected restore, manifest discovery, or readiness behavior.
- Do not auto-switch tabs while the user types search text or adjusts ordinary Review-tab filters.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not scan, create, move, restore, delete, write, or clean up real-profile files.

## Implementation

- Named the WPF workbench tab items so code can select `Main Grid` directly.
- Added a small `SelectMainGridTab` helper.
- Safety Summary shortcuts now select `Main Grid` after applying their read-only review lens.
- `Current quarantined` now selects `Main Grid` after switching the main grid to current-session moved entries.
- `Back to scan rows` now selects `Main Grid` after switching the main grid back to Storage Scan rows.
- Kept Review-tab search/filter controls from auto-switching while the user is still setting up a review lens.

## Test plan

Automated checks:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Manual checks:

- Run `.\tools\Start-MvpFixtureReview.cmd` and confirm Safety Summary shortcuts make the filtered rows visible on Main Grid.
- After fixture Quarantine execution, confirm `Current quarantined` and `Back to scan rows` switch Main Grid into view with the expected Review Grid Mode Status.

## Completion notes

Completed on: 2026-06-01

What changed:

- Grid-switching shortcut actions now bring Main Grid forward after changing visible rows.
- WPF smoke coverage proves Safety Summary shortcut, `Current quarantined`, and `Back to scan rows` auto-focus Main Grid.

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
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF navigation polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Should ordinary Review-tab filters remain on the Review tab after changing the row lens, or should some of them also auto-focus Main Grid after visible fixture review?

Risky assumptions:

- Auto-focusing only shortcut/grid-switch actions is less jarring than auto-switching on every filter/search change.
