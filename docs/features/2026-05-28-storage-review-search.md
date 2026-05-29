# Feature: Storage Review Search

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make large Storage Scan results easier to review by adding an in-memory text search over completed review rows.

## Non-goals

- Do not rescan the filesystem.
- Do not search outside the completed Storage Scan result.
- Do not persist search history.
- Do not change Bloat Categories, Importance Ratings, Deletion Recommendations, or cleanup eligibility based on search text.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.

## User story / job story

As the project owner, I want to search the completed Storage Scan review for names like `pip`, `NVIDIA`, `Codex`, app names, and game folders, so that I can inspect specific paths without scrolling through thousands of rows.

## Current behavior

The WPF app supports rating filters, category filters, safety shortcuts, selected-row guidance, and child breakdowns. On a real profile scan with many rows, finding a specific app, tool, or folder name still requires scrolling or exporting a CSV.

## Desired behavior

The WPF app should provide Storage Review Search that:

- Is enabled only after a Storage Scan completes.
- Combines with the active Storage Review Filter and Bloat Category Filter.
- Matches path, parent path, name, category, Importance Rating, Deletion Recommendation, evidence, Access Status, and access issue text.
- Supports field prefixes such as `path:`, `parent:`, `under:`, `category:`, `rating:`, and `recommendation:` for narrower searches.
- Resets after a new Storage Scan completes.
- Keeps all behavior read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Search | Added as read-only text search over completed Storage Scan review rows. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- None.

## Grill notes

### Scenarios discussed

- The user's real scan showed 188,580 files and 37,740 folders.
- The largest 2,000-row view needs targeted review tools for specific app/tool paths.

### Edge cases

- Search should match spaced terms such as `high risk` and `python package cache` even though enum names do not contain spaces.
- Search should combine with review and category filters rather than replacing them.
- Recognized prefixes should restrict matching to the named field; unrecognized prefixes should remain literal broad search text.
- Search should not trigger file IO.

### Dependencies between decisions

- This depends on the existing in-memory `StorageScanReview`.
- This does not change scanner traversal, classification, preview, or cleanup readiness.

## Evidence and validation gate

Evidence gathered:

- Existing `StorageScanReview` filter implementation.
- Existing WPF filter toolbar and smoke tests.
- User-provided real scan scale.

Tests/checks planned:

- Core search coverage for spaced category/rating terms and combined filters.
- WPF smoke coverage for search text, filter summary, displayed rows, and clearing search.
- Build and both test harnesses.
- MVP preflight.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make search rescan or watch the filesystem.
- Do not persist search history.
- Do not use search results as cleanup approval.

## Decisions made

Small feature-level decisions:

- Implement search in core so it is testable outside WPF.
- Match normalized text so `high risk` can match `HighRisk` and `python package cache` can match `PythonPackageCache`.
- Reset search after each new scan.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add `StorageReviewSearch`.
2. Extend `StorageScanReview` filtering to accept search.
3. Add WPF search box and clear action.
4. Add core and WPF smoke coverage.
5. Update docs and progress.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/StorageReviewSearch.cs`
- `src/WindowsFileCleaner.Core/StorageScanReview.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Search a fixture scan for `pip`, `NVIDIA`, and `high risk`.
- Confirm search combines with rating/category filters.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

## Risks and assumptions

Risks:

- Text search is useful for triage but does not solve visual layout verification.
- Search over displayed rows can still be bounded by the 2,000-row display cap after filtering.

Assumptions:

- In-memory search over completed scan results is fast enough for the MVP.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `StorageReviewSearch`.
- Extended Storage Scan review filtering to combine review filter, category filter, and search query.
- Added WPF search controls and clear search action.
- Added tests for spaced category/rating search, combined filters, WPF search summary, and clearing search.
- Later field-prefix packet added `StorageReviewSearchField`, restricted searches such as `path:` and `category:`, WPF tooltip examples, and fixture coverage for prefixed searches.
- Later Access Status Search packet added broad and prefixed search for Access Status values such as `Readable` and `Access issue`.
- Later Selected Folder Child Focus packet added `parent:` for immediate-parent matching.
- Later Selected Folder Descendant Focus packet added `under:` for recursive descendant matching that excludes the selected ancestor row.

Files changed:

- `src/WindowsFileCleaner.Core/StorageReviewSearch.cs`
- `src/WindowsFileCleaner.Core/StorageReviewSearchField.cs`
- `src/WindowsFileCleaner.Core/StorageScanReview.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `.codex/progress.md`

Tests run:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- Later field-prefix packet:
  - `dotnet build WindowsFileCleaner.sln --no-restore`
  - `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
  - `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
  - `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is a reversible in-memory review feature and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Include Storage Review Search in the next visible fixture UI pass.

Open questions:

- None.

Risky assumptions:

- In-memory search remains responsive enough for the current real-profile scan scale.
