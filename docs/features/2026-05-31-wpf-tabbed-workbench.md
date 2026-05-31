# Feature: WPF Tabbed Workbench

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Give the large WPF review sections their own horizontal tab pages so Safety Summary, Review, Quarantine, and Main Grid each have enough room.

## Non-goals

- Do not change Storage Scan behavior.
- Do not change Cleanup Scope Scan Gate behavior.
- Do not change Review Shortlist, Quarantine Preview, fixture execution, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not scan, create, move, restore, delete, write, or clean up real-profile files.

## Implementation

- Added a `WorkbenchTabs` WPF tab control below the global Cleanup Scope and scan-gate header.
- Initially moved scan summary cards to a `Scan` tab; a follow-up header-metrics packet moved those totals back into the header and removed the sparse Scan tab.
- Moved Safety Summary to its own tab and kept its header text, state styling, tooltip/help text, and hoverable `?` help cue.
- Moved review filters, search, display-window controls, visible-row shortlist controls, and Review Shortlist Safety Mix to a `Review` tab.
- Moved Quarantine Root Selection, Quarantine Preview, compact Quarantine Readiness Summary, confirmation, fixture execution, current-fixture undo, and Quarantine Execution Gate to a `Quarantine` tab.
- Moved the Storage Scan / Current-Session Quarantined Review grids and selected-path detail panel to a `Main Grid` tab, selected by default.
- Kept the existing Expander controls available inside Safety Summary and Quarantine tabs, but expanded by default because the tab pages now provide the recovered space.
- Later packet `2026-06-01-main-grid-auto-focus.md` made Safety Summary shortcuts and current-session quarantined grid switches select `Main Grid` after they change which rows it shows.

## Test plan

Automated checks:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Manual checks:

- Run `.\tools\Start-MvpFixtureReview.cmd` and confirm the horizontal tabs feel calmer than the previous stacked layout.
- Confirm the Main Grid tab is selected by default and the Safety Summary / Quarantine pages are expanded and readable.

## Completion notes

Completed on: 2026-05-31

What changed:

- The visible app now has tab pages for Safety Summary, Review, Quarantine, and Main Grid, with large safety/readiness sections moved out of the main grid's vertical path.

ADRs:

- No ADR added. This is reversible WPF navigation/layout polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Follow-up work:

- Run a visible fixture pass and decide whether the Safety Summary / Review / Quarantine / Main Grid tab order should stay as-is after hands-on use.

Open questions:

- Should Review-tab filters remain on the Review tab after changing the row lens, or should only shortcut/grid-switch actions auto-focus Main Grid?

Risky assumptions:

- Keeping the scan controls and scan-gate safety text global while tabbing the large sections preserves discoverability without hiding movement blockers.
