# Feature: Shown Row Review Shortlist Actions

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make large Storage Scan review sessions less tedious by letting the user add or remove the currently shown rows from Review Shortlist in read-only actions.

## Non-goals

- Do not shortlist or remove hidden matched rows beyond the Storage Review Display Limit.
- Do not persist the Review Shortlist.
- Do not approve cleanup, execute Quarantine, Undo Quarantine, delete files, move files, or write manifests.

## User story / job story

As the project owner, I want to shortlist or remove the currently shown review rows after narrowing with filters/search, so that I can adjust a focused review set without clicking each row one by one.

## Current behavior

The WPF app lets the user add or remove only the selected row from Review Shortlist, and already has a `Shortlist shown` action. On a large real scan, one-way bulk shortlisting is still clumsy if the user applies a broad filter and wants to reverse only the visible review window.

## Desired behavior

After a Storage Scan:

- `Shortlist shown` adds only rows currently visible in the grid.
- `Remove shown` removes only rows currently visible in the grid.
- Neither action touches hidden matched rows beyond the 2,000-row display window.
- It clears stale Quarantine Preview data only when the shortlist actually changes.
- Status text states that Review Shortlist is not cleanup approval and no files were modified.

## Domain language changes

No new terms.

| Term | Change | Docs updated? |
|---|---|---|
| Review Shortlist | Clarified that bulk additions/removals use only currently displayed rows. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should a later review UI support selecting a specific subset of displayed rows before bulk shortlisting/removal?

## Grill notes

### Scenarios discussed

- The user tested a real `C:\Users\moxhe` scan with a large result set.
- The app now reports when the grid is capped at the first 2,000 matched rows.
- Bulk shortlist actions should respect that display boundary rather than quietly changing rows the user cannot see.

### Edge cases

- Pressing `Shortlist shown` when every displayed row is already shortlisted should not clear the current preview.
- Pressing `Remove shown` when no displayed row is shortlisted should not clear the current preview.
- Running a new Storage Scan still clears the Review Shortlist.
- Quarantine Preview remains a read-only dry run and may still block high-risk or ineligible shortlisted rows.

### Dependencies between decisions

- This depends on Review Shortlist, Storage Review Display Limit, Storage Review Search, filters, and Quarantine Preview.
- This does not change cleanup approval semantics.

## Evidence and validation gate

Evidence gathered:

- Existing WPF review interaction smoke test.
- Existing `StorageReviewShortlist` uniqueness behavior.
- User-provided real scan scale and screenshot.

Tests/checks planned:

- Core coverage for bulk shortlist uniqueness.
- WPF smoke coverage for `Shortlist shown` / `Remove shown` status, counts, enabled states, and read-only preview flow.
- MVP preflight.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add or remove all matched rows when the grid is capped.
- Do not name the action as cleanup approval.
- Do not add cleanup execution from the shortlist toolbar.

## Decisions made

Small feature-level decisions:

- Add `Shortlist shown` and `Remove shown` WPF actions in the review toolbar.
- Keep both actions tied to `DisplayedRows`, not the full filtered/matched set.
- Reuse `StorageReviewShortlist` and add a small `AddMany` helper.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add bulk add/remove support to `StorageReviewShortlist`.
2. Add WPF `Shortlist shown` / `Remove shown` controls and command methods.
3. Update WPF smoke tests.
4. Update docs and progress.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/StorageReviewShortlist.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Possible:

- `docs/features/2026-05-28-shortlist-shown-review-rows.md`

## Test plan

Manual checks:

- Run a fixture scan, filter to Quarantine candidates, click `Shortlist shown`, then `Remove shown`, and verify only visible rows are changed.
- Preview quarantine and verify no files are modified.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

## Risks and assumptions

Risks:

- The user could shortlist too many displayed rows if the current filter/search is broad.
- The toolbar is becoming dense and needs continued visible fixture review.

Assumptions:

- Limiting bulk shortlist changes to visible rows is safer and clearer than changing all matched rows.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `StorageReviewShortlist.AddMany` and `RemoveMany`.
- Added WPF `Shortlist shown` and `Remove shown` actions for currently displayed rows.
- Added WPF status wording that says Review Shortlist is not cleanup approval.
- Updated core and WPF smoke coverage.

Files changed:

- `src/WindowsFileCleaner.Core/StorageReviewShortlist.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-shortlist-shown-review-rows.md`
- `.codex/progress.md`

Tests run:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config`
- Narrow build/tests before final preflight:
  - `dotnet build WindowsFileCleaner.sln --no-restore`
  - `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
  - `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-shortlist-shown-review-rows.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is reversible read-only review UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Include `Shortlist shown` and `Remove shown` in the next visible fixture and real-profile review pass.

Open questions:

- Should a later review UI support selecting a specific subset of displayed rows before bulk shortlisting/removal?

Risky assumptions:

- The user will narrow filters/search before using `Shortlist shown` on a real scan.
