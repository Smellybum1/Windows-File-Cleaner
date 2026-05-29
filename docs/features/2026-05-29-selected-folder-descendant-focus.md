# Feature: Selected Folder Descendant Focus

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make recursive folder inspection easier by letting the user focus the review grid on every descendant under the selected folder.

This helps large real-profile folders such as `AppData`, `NVIDIA`, `pip`, and browser data become navigable without rescanning or enabling cleanup execution.

## Non-goals

- Do not rescan the filesystem.
- Do not add real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or cleanup history.
- Do not auto-shortlist, auto-quarantine, or change Cleanup Candidate classification.
- Do not build a tree view in this packet.
- Do not treat descendant focus as cleanup approval or storage savings.

## User story / job story

As the project owner, I want selecting a large folder to show all scanned rows under it, so that I can use the existing grid filters, sorting, paging, shortlist, and export tools on that folder's subtree.

## Current behavior

The WPF app shows a Descendant review summary, Largest immediate children, Storage Hotspot Trail, and `Show children` for immediate children.

The user can manually search for a path prefix, but there is no explicit recursive selected-folder action that excludes the selected folder itself and focuses the grid on descendants.

## Desired behavior

- Add an `under:` Storage Review Search prefix for descendant matching.
- Add a read-only `Show descendants` selected-folder action.
- The action applies `under:<selected folder full path>`, resets review/category/type/size filters to All, and leaves the user in the normal review grid.
- The selected folder itself is excluded from descendant matches.
- Files keep the action disabled.
- Status and docs state that no files are modified and that this is not cleanup approval.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Search | Expanded with `under:` field-prefix search. | yes |
| Selected Folder Descendant Focus | Added as read-only selected-folder recursive focus action. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should descendant focus eventually preserve the active rating/category filters when invoked from a summary bucket?
- Should descendant bucket shortcuts be added after a visible layout pass?

## Grill notes

### Scenarios discussed

- The user's real scan screenshot showed large flat results and nested buckets where immediate-child focus may not be enough.
- Descendant focus should support existing display-window pagination when a subtree has more than 2,000 rows.

### Edge cases

- The selected folder should not match itself.
- Files should not enable descendant focus.
- Invalid `under:` search paths should simply match no rows.
- Active filters can hide descendants, so the selected-folder action resets review lenses to All.

### Dependencies between decisions

- This depends on Storage Review Search and the completed Storage Scan result.
- This complements Selected Folder Subtree Summary and Selected Folder Child Focus.
- This remains read-only and does not affect cleanup workflows.

## Evidence and validation gate

Evidence gathered:

- User screenshot of a large real-profile scan.
- Existing `StorageReviewSearch`, `StorageScanReview`, `StorageEntryRow.ParentLocation`, Selected Folder Subtree Summary, Child Breakdown, WPF selected-row actions, and smoke tests.
- Project safety rule: inspect before recommending removal.

Tests/checks planned:

- Core coverage for `under:` parsing and descendant matching that excludes the selected folder.
- WPF smoke coverage for selecting a folder, using `Show descendants`, and proving the review lens is read-only.
- Build, both test harnesses, and MVP preflight.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not implement a full tree view before proving descendant focus against fixture and real-scan review needs.
- Do not make descendant focus imply cleanup safety.
- Do not add bucket shortcut buttons until manual layout review shows where they should live.

## Decisions made

Small feature-level decisions:

- Use `under:` for recursive descendant focus.
- Exclude the selected folder row itself.
- Reset review, category, type, and size filters when focusing descendants.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add under-prefixed search support in core.
2. Add WPF `Show descendants` selected-row action.
3. Add core and WPF smoke coverage.
4. Update README, MVP audit/manual checklist if needed, and progress after verification.

## Files expected to change

Expected:

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
- `.codex/progress.md`

Possible:

- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `docs/features/2026-05-28-storage-review-search.md`

## Test plan

Manual checks:

- Run the fixture app, scan, select `AppData`, click `Show descendants`, and confirm nested rows under `AppData` appear while the `AppData` row itself is excluded.
- On the next real scan, select large folders such as `AppData`, `NVIDIA`, `pip`, and browser folders, then use `Show descendants` with size/category filters.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

## Risks and assumptions

Risks:

- Very large subtrees may still require display-window paging.
- A new button adds density to the selected-row action strip.

Assumptions:

- Recursive subtree focus is useful enough to add before designing a tree/grid.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added `under:` Storage Review Search parsing and descendant matching in core.
- Added WPF `Show descendants` for selected folders.
- The action resets review/category/type/size lenses to All, applies `under:<selected folder full path>`, excludes the selected folder itself, and reports that no files were modified.
- Added core and WPF smoke coverage for descendant search, lens reset behavior, file-row disablement, and nested descendant matching.

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
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `docs/features/2026-05-29-selected-folder-descendant-focus.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `docs/features/2026-05-29-selected-folder-descendant-focus.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is a reversible in-memory review and WPF inspection improvement that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Follow-up work:

- In the next visible fixture pass, select `AppData` and confirm `Show descendants` feels usable in the real layout.
- In the next real scan, use `Show descendants` on `AppData`, `NVIDIA`, `pip`, and browser folders, then combine the focused grid with size/category filters.

Open questions:

- Should descendant focus eventually preserve the active rating/category filters when invoked from a future summary bucket shortcut?
- Should descendant bucket shortcuts be added after a visible layout pass?

Risky assumptions:

- Recursive subtree focus is useful enough before a full tree/grid design.
- Resetting review lenses to All is less surprising than preserving filters that could hide descendants.
