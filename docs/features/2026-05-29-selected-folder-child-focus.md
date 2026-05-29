# Feature: Selected Folder Child Focus

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make Storage Scan review easier after a large real-profile scan by letting the user select a folder and focus the main grid on that folder's immediate children.

This advances the user's request for the app to show what is inside large paths before rating importance or recommending cleanup.

## Non-goals

- Do not rescan the filesystem.
- Do not add real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or cleanup history.
- Do not auto-shortlist, auto-quarantine, or change Cleanup Candidate classification.
- Do not build a full recursive tree view in this packet.

## User story / job story

As the project owner, I want a selected-folder action that shows the folder's immediate children in the main grid, so that rows like `AppData`, `NVIDIA`, `pip`, and cache buckets can be drilled into without scrolling through a flat 2,000-row window.

## Current behavior

The app already shows a bounded Child Breakdown in the selected-row detail pane and exposes `Relative path`, `Parent`, and `Contents` columns in the main grid.

The user can manually search by path, but there is no direct selected-folder action that turns a large folder row into a focused child review window.

## Desired behavior

- Add a read-only `Show children` action for selected folder rows.
- Add `parent:` as a Storage Review Search field prefix.
- `Show children` applies `parent:<selected folder full path>` and resets review/category/type/size filters to All so the immediate children are visible.
- Files keep the action disabled.
- The result remains a normal review grid, so the user can sort, shortlist, export, inspect evidence, preview text files, or drill deeper by selecting another folder.
- The status and docs state that no files are modified and that this is not cleanup approval.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Search | Expanded with `parent:` field-prefix search. | yes |
| Selected Folder Child Focus | Added as read-only selected-folder focus action. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should a later tree/grid view replace this selected-folder focus action if manual retesting shows the flat grid is still too awkward?
- Should the app eventually show breadcrumbs for repeated child focus steps?

## Grill notes

### Scenarios discussed

- The user's real scan screenshot showed large container rows and many short/hash-like child names.
- The next safest step is better inspection, not real-profile file movement.

### Edge cases

- A selected file should not enable child focus.
- A selected folder with no immediate children should show zero matched rows without rescanning.
- Existing filters can hide the children the user asked to inspect, so the action resets other review lenses to All before applying the parent search.

### Dependencies between decisions

- This depends on the existing in-memory Storage Review Search and Storage Review Display Window.
- This complements Child Breakdown rather than replacing it.
- This does not change classification, Review Shortlist, Quarantine Preview, or cleanup execution gates.

## Evidence and validation gate

Evidence gathered:

- User screenshot of `C:\Users\moxhe` scan scale and flat-list review friction.
- Existing `StorageReviewSearch`, `StorageScanReview`, `StorageEntryRow.ParentLocation`, WPF selected-row detail actions, Child Breakdown, and WPF smoke tests.
- Project safety rules requiring inspection before recommending removal.

Tests/checks planned:

- Core coverage for `parent:` parsing and immediate-parent matching.
- WPF smoke coverage for selecting a folder, using `Show children`, and proving the review lens is read-only.
- Build and both test harnesses.
- MVP preflight before commit.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not implement a full tree view before proving the smaller focus action against fixture and real-scan review needs.
- Do not preserve active filters during `Show children`; hidden children would make the action feel broken.
- Do not make child focus imply cleanup safety.

## Decisions made

Small feature-level decisions:

- Add `parent:` search in core so WPF and export behavior share the same review lens.
- Use the selected folder's full path for `Show children` so matches are immediate-child rows under that exact parent path.
- Reset review, category, type, and size filters when focusing children.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add parent-prefixed search support in core.
2. Add WPF `Show children` selected-row action.
3. Add core and WPF smoke coverage.
4. Update README, MVP audit/manual checklist if needed, and progress log after verification.

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

- `docs/features/2026-05-28-storage-review-field-prefix-search.md`

## Test plan

Manual checks:

- Run the fixture app, scan, select `Downloads`, click `Show children`, and confirm only direct children appear.
- On the next real scan, select large folders such as `AppData`, `NVIDIA`, `pip`, or `Local`, click `Show children`, and confirm the result is easier to triage without modifying files.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

## Risks and assumptions

Risks:

- Showing immediate children may still be less ergonomic than a true tree view for deep cache paths.
- A full path in the search box is long, but it keeps the lens explicit and exportable.

Assumptions:

- Immediate-child focus is the smallest useful step after the user's real scan screenshot.
- In-memory parent matching is fast enough at the current scan scale.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added `parent:` Storage Review Search support for immediate-parent matching.
- Added the WPF `Show children` selected-folder action.
- The action resets review lenses to All, applies a parent-prefixed search for the selected folder, focuses the grid on immediate child rows, and reports that no files were modified.
- Files keep the action disabled.
- Added core and WPF smoke coverage for parent-prefixed search and selected-folder child focus.

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
- `docs/features/2026-05-29-selected-folder-child-focus.md`
- `.codex/progress.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed before rebuild.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed before rebuild.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuild.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuild.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `.codex/progress.md`
- This feature brief

ADRs added or skipped:

- No ADR added. This is a reversible in-memory review and WPF inspection improvement that does not change persistence, cleanup execution, or recovery rules.

Follow-up work:

- Try `Show children` during the next visible fixture pass and real-profile scan review, especially on `AppData`, `Local`, `NVIDIA`, `pip`, and browser/cache folders.
- Consider breadcrumbs or a tree/grid only if repeated child focus remains awkward after manual use.

Open questions:

- Should a later tree/grid view replace this selected-folder focus action if manual retesting shows the flat grid is still too awkward?
- Should the app eventually show breadcrumbs for repeated child focus steps?

Risky assumptions:

- Immediate-child focus is the smallest useful improvement after the real scan screenshot.
- Long full-path `parent:` searches are acceptable because they are explicit, testable, and exportable.
