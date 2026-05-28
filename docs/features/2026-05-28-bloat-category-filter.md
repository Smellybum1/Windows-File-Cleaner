# Feature: Bloat Category Filter

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Let Storage Scan results be filtered by Bloat Category after a scan.

The real scan showed large areas such as app caches, Python package caches, GPU shader caches, browser data, and protected locations. Category filtering should make those groups easier to inspect without adding cleanup execution.

## Non-goals

- Do not add Quarantine.
- Do not add deletion.
- Do not calculate non-overlapping Storage Savings.
- Do not treat a category match as automatic approval to remove.

## User story / job story

As the project owner, I want to filter Storage Scan rows by Bloat Category, so that I can inspect one kind of cleanup evidence at a time.

## Desired behavior

- Show a Category dropdown after Storage Scan.
- Include All categories plus categories found in the current scan.
- Later packet adds No category when the scan has Uncategorized Results.
- Show each category option with row count and largest row.
- Combine the selected category with the existing Storage Review Filter.
- Export CSV using both the current Storage Review Filter and selected Bloat Category Filter.
- Keep all behavior read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Bloat Category Filter | Added for read-only filtering by category. | yes |
| Scan Report Export | Clarified that export uses both review and category filters. | yes |

## Decisions made

- Keep risk/recommendation filters and category filter separate.
- Use largest-row sizes for category summaries because flattened recursive rows overlap.
- Include rows in every category they match; one row may appear under multiple categories.

## Implementation plan

1. Add category summaries to `StorageScanReview`.
2. Add combined filter support for `StorageReviewFilter` plus category filtering.
3. Add WPF Category dropdown.
4. Include category in CSV export filenames and exported row selection.
5. Add fixture coverage for category summaries and combined filtering.

## Test plan

Automated checks:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Manual checks:

- Run Storage Scan and confirm the Category dropdown appears after scan.
- Select categories such as App cache, Python package cache, GPU shader cache, Protected location, or Access issue and confirm rows narrow as expected.
- Export CSV with a category selected and confirm the file contains only the combined filter rows.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `StorageCategorySummaryEntry`.
- Added category summaries to `StorageScanReview`.
- Added combined review-filter plus category-filter behavior.
- Added a WPF Category dropdown below the filter buttons.
- CSV export now uses the active review filter and selected category filter.
- Added fixture coverage for category summaries and combined filtering.

Files changed:

- `src/WindowsFileCleaner.Core/StorageCategorySummaryEntry.cs`
- `src/WindowsFileCleaner.Core/StorageScanReview.cs`
- `src/WindowsFileCleaner.Core/StorageScanReviewBuilder.cs`
- `src/WindowsFileCleaner.App/CategoryFilterOption.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`
- This feature brief

ADRs added or skipped:

- No ADR added. This is an incremental read-only review feature.

Follow-up work:

- Let the user rerun Storage Scan and identify which categories are most useful.
- Refine classifier rules for categories that still produce too much `None` or too much noise.

Open questions:

- Resolved in later packet: `No category` is its own filter option for uncategorized rows.
- Which category labels should be treated as Protected Locations in future cleanup preview work?

Risky assumptions:

- Category filtering is more useful than adding more top-level buttons for every category.
