# Feature: No category filter

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make uncategorized Storage Scan rows directly reviewable.

The first real scan showed many rows with `None` in the Categories column. Those rows are important because they show where classifier rules are still conservative or incomplete.

## Non-goals

- Do not infer that uncategorized rows are safe or unsafe.
- Do not add cleanup execution.
- Do not broaden scanner scope.
- Do not create new Bloat Categories in this packet.

## User story / job story

As the project owner, I want to filter to rows with no category, so that I can inspect unknown large folders and decide which classifier rules need refinement.

## Desired behavior

- Add `No category` to the Category dropdown when the current scan has uncategorized rows.
- Show row count and largest row for `No category`.
- Combine `No category` with the active Storage Review Filter.
- Include `no-category` in CSV filenames when exporting that filtered view.
- Keep all behavior read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Uncategorized Result | Added for rows with no Bloat Category. | yes |
| Bloat Category Filter | Expanded to include `No category`. | yes |

## Decisions made

- Treat `No category` as a filter option, not as a Bloat Category.
- Preserve empty category lists in scan data; do not replace them with `Unknown`.
- Use `StorageCategoryFilter` to distinguish All categories, named categories, and No category.

## Implementation plan

1. Add `StorageCategoryFilter` and `StorageCategoryFilterKind`.
2. Add core filtering for `No category`.
3. Add a `No category` dropdown option with count and largest row.
4. Add fixture coverage for combined review filter plus no-category filtering.
5. Update docs and progress.

## Test plan

Automated checks:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Manual checks:

- Run Storage Scan and choose `No category` in the Category dropdown.
- Confirm the table shows only rows whose Categories column is `None`.
- Export CSV and confirm the filename contains `no-category`.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `StorageCategoryFilter` and `StorageCategoryFilterKind`.
- Added core `No category` filtering.
- Added WPF `No category` dropdown option when uncategorized rows exist.
- Added fixture coverage for `No category` and combined review/category filters.

Files changed:

- `src/WindowsFileCleaner.Core/StorageCategoryFilter.cs`
- `src/WindowsFileCleaner.Core/StorageCategoryFilterKind.cs`
- `src/WindowsFileCleaner.Core/StorageScanReview.cs`
- `src/WindowsFileCleaner.App/CategoryFilterOption.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-bloat-category-filter.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-bloat-category-filter.md`
- `.codex/progress.md`
- This feature brief

ADRs added or skipped:

- No ADR added. This is an incremental read-only review feature.

Follow-up work:

- Use `No category` rows from the next real scan to refine classifier rules.

Open questions:

- Which uncategorized real-scan folders should become explicit Bloat Categories or Protected Locations?

Risky assumptions:

- Showing uncategorized rows as `No category` is clearer than assigning a broad `Unknown` category.
