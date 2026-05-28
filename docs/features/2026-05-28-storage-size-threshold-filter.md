# Feature: Storage Size Threshold Filter

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Make real-profile Storage Scan review easier by letting the user narrow completed results to rows at or above useful size thresholds.

## Non-goals

- Do not change scanner traversal.
- Do not change Bloat Category assignment, Importance Rating, or Deletion Recommendation based only on size.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not treat large rows as cleanup approval or confirmed Storage Savings.

## User story / job story

As the project owner, I want to filter the review grid to large rows, so that I can inspect the biggest folders and files in a real scan without scrolling through thousands of smaller paths.

## Current behavior

The WPF review grid can be narrowed by Storage Review Filter, Bloat Category Filter, Storage Entry Type Filter, and Storage Review Search. A real scan can still show many large and small rows together.

## Desired behavior

- Add a Size filter with `All sizes`, `1 MB+`, `100 MB+`, `1 GB+`, `5 GB+`, and `10 GB+`.
- Combine Size with Storage Review Filter, Bloat Category Filter, Storage Entry Type Filter, and Storage Review Search.
- Include the active Size filter in the filter summary and Scan Report Export filename.
- Keep the filter read-only and in-memory.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Size Threshold Filter | Added as a read-only row-size review lens. | yes |

## Decisions made

- Implement size-threshold filtering in `StorageScanReview` so WPF display, export row selection, and review-window paging use the same active review lens.
- Use binary byte thresholds to match `ByteSizeFormatter`.
- Keep thresholds broad and review-oriented rather than trying to infer safety from size.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `StorageSizeThresholdFilter`.
- Added core filtering for minimum row size.
- Added a WPF Size filter combo box.
- Updated Scan Report Export filenames to include the active size threshold segment.
- Added core and WPF smoke coverage.

ADRs added or skipped:

- No ADR. This is reversible read-only review filtering and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Try `1 GB+` and `5 GB+` on the next real scan to focus the large AppData/cache rows.
