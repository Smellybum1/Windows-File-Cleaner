# Feature: WPF Panel Cleanup

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Make the WPF review surface feel less messy by grouping the dense controls into quieter panels and letting compact safety/readiness headers carry the first-pass state.

## Non-goals

- Do not change Storage Scan behavior.
- Do not change Cleanup Scope Scan Gate behavior.
- Do not remove safety wording, help cues, tooltip text, or automation help text.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not scan, create, move, restore, delete, write, or clean up real-profile files.

## Implementation

- Added a shared WPF panel border style and section-heading style.
- Put the scan header and summary cards into consistent quiet panels.
- Grouped review filters, search, display-window controls, visible-row shortlist controls, and Review Shortlist Safety Mix in a named `Review filters and shortlist` panel.
- Kept Quarantine Shortlist as its own panel and made the panel visually consistent with the rest of the shell.
- Set Safety Summary and Quarantine Shortlist to start collapsed so the main review rows have more room while their panel-name headers still expose state, help cues, and safety boundaries.
- Kept selected-path detail in the same panel style.

## Test plan

Automated checks:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Manual checks:

- Run `.\tools\Start-MvpFixtureReview.cmd` and confirm the panel layout feels calmer, the collapsed headers are still understandable, and expanding Safety Summary / Quarantine Shortlist does not crowd the grid.

## Completion notes

Completed on: 2026-05-31

What changed:

- The visible app now starts with the largest safety/readiness sections collapsed and uses quieter panel boundaries around the scan, metric, review-control, Quarantine, and selected-path areas.

ADRs:

- No ADR added. This is reversible WPF layout polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Follow-up work:

- Run the visible fixture pass and decide whether the calmer panel layout is enough or whether the review surface needs a larger navigation redesign.

Open questions:

- Should Safety Summary or Quarantine Shortlist auto-expand after scan/shortlist changes, or is manual expansion clearer?

Risky assumptions:

- Starting with collapsed safety/readiness panels reduces visual clutter without hiding critical state because the headers, status styling, tooltips, and help cues remain visible.
