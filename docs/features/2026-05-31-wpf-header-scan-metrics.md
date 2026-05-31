# Feature: WPF Header Scan Metrics

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Move the sparse Scan-tab summary into the open header space so scan totals stay visible without dedicating a whole tab page to four numbers.

## Non-goals

- Do not change Storage Scan behavior.
- Do not change Cleanup Scope Scan Gate behavior.
- Do not change Review Shortlist, Quarantine Preview, fixture execution, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not scan, create, move, restore, delete, write, or clean up real-profile files.

## Implementation

- Added a compact wrapping scan-metric strip to the WPF header for total size, folders, files, and access issues.
- Removed the sparse Scan tab from the horizontal workbench.
- Kept Safety Summary, Review, Quarantine, and Main Grid as dedicated tab pages with Main Grid selected by default.
- Preserved the existing named metric text blocks so completed scans still populate the same values.

## Test plan

Automated checks:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Manual checks:

- Run `.\tools\Start-MvpFixtureReview.cmd` and confirm the header totals look balanced beside the Cleanup Scope controls.
- Confirm the tab row now starts with Safety Summary and the Main Grid tab remains selected by default.

## Completion notes

Completed on: 2026-05-31

What changed:

- The visible app now keeps scan totals in the header and reserves tab pages for Safety Summary, Review, Quarantine, and Main Grid.

ADRs:

- No ADR added. This is reversible WPF layout polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Follow-up work:

- Run a visible fixture pass and decide whether the header totals need tighter spacing on narrower windows.

Open questions:

- Should selecting a Safety Summary shortcut or Quarantine action automatically switch to the Main Grid tab afterward?

Risky assumptions:

- Keeping scan totals global improves orientation without hiding scan-gate or movement blockers.
