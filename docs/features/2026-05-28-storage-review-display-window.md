# Feature: Storage Review Display Window

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Let the WPF review grid move through large matched Storage Scan result sets in bounded 2,000-row windows without rescanning or loading an unbounded grid.

## Non-goals

- Do not change scanner traversal.
- Do not discard matched rows outside the visible window.
- Do not change Bloat Categories, Importance Ratings, Deletion Recommendations, or cleanup eligibility.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, deletion, or manifest writing.

## User story / job story

As the project owner, I want Previous rows and Next rows controls when a scan matches more than 2,000 rows, so that I can inspect more of the completed scan without exporting or guessing what is hidden beyond the first grid window.

## Desired behavior

- A completed Storage Scan starts on `rows 1-2,000 of N matched` when more than 2,000 rows match.
- `Next rows` moves to the next matched row window.
- `Previous rows` returns to the prior matched row window.
- The row-window label and filter summary show the active row range.
- Changing the active review lens resets the display window to the first matched rows.
- `Shortlist visible rows` and `Remove visible rows` apply only to the currently displayed window.
- Scan Report Export uses the full active review lens, not only the visible display window.
- All behavior remains in-memory and read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Display Limit | Clarified as the per-window row cap rather than the only reachable rows. | yes |
| Storage Review Display Window | Added as the current slice of matched rows shown in the WPF grid. | yes |

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added read-only Previous rows and Next rows WPF controls.
- Added an active row-window label to the review toolbar.
- Added `_currentDisplayStartIndex` for the WPF display-window offset.
- Updated status and filter summary wording to show row ranges such as `rows 2,001-4,000 of N matched`.
- Reset the display window when filters, type filters, category filters, search, safety shortcuts, or Review View Reset change the active review lens.
- Corrected Scan Report Export row selection to include the active Storage Entry Type Filter.
- Added WPF fixture coverage for next/previous row windows, display-window reset, read-only status wording, and type-filtered exports.
- Later combo-reset packet made the WPF category and type combo-box event handlers reset the display window and added coverage for the actual combo selection path.

ADRs added or skipped:

- No ADR. This is reversible WPF review behavior and does not change architecture, persistence, security, deployment, cleanup execution, or public APIs.

Follow-up work:

- In the next manual real-profile review, confirm whether row-window controls are useful enough or whether a virtualized tree/grid is still needed later.
