# Feature: Selected Row Contents Context

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Help the project owner understand large recursive rows by showing how many files and folders they contain.

## Non-goals

- Do not change scanner traversal.
- Do not change Bloat Categories, Importance Ratings, or Deletion Recommendations.
- Do not treat contained counts as storage savings or cleanup approval.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.

## User story / job story

As the project owner, I want selected folder rows to show contained file and folder counts, so that large containers like `AppData`, `pip`, and `DXCache` are easier to interpret before I shortlist anything.

## Desired behavior

- Selected folder detail context shows contained file count and descendant folder count.
- WPF grid rows show a compact Contents column for quick comparison before selecting a row.
- Selected file detail context identifies the row as a single file.
- Scan Report CSV includes contained file and folder counts for offline review.
- Counts remain triage context only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Row Contents Context | Added as read-only contained file/folder counts for selected rows and CSV exports. | yes |

## Decisions made

- For folder rows, folder count means descendant folders, excluding the selected folder itself.
- For file rows, the detail pane says `Single file`.
- CSV exports include numeric counts so filtered/offline review can compare recursive rows.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added contained file/folder count properties to `StorageEntryRow`.
- Added a WPF grid `Contents` column.
- Added contents context to the selected-row detail pane.
- Added `Contained files` and `Contained folders` columns to Scan Report CSV export.
- Added WPF fixture coverage for grid/detail contents context and CSV coverage.

ADRs added or skipped:

- No ADR. This is reversible read-only review context and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Check whether contents counts make real scan containers easier to compare with largest-child breakdowns.
- In the next real scan, confirm whether the grid `Contents` column is useful without making the table too crowded.
