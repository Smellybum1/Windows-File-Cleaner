# Feature: Storage Review Display Limit

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make large Storage Scan results clearer by distinguishing rows shown in the WPF grid from rows matched by the active review filter/search.

## Non-goals

- Do not change scanner traversal.
- Do not reduce the completed Storage Scan result.
- Do not add pagination, virtualization, persisted scan history, cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.

## User story / job story

As the project owner, I want the app to tell me when the grid is showing only the first review window, so that I do not mistake 2,000 displayed paths for the whole scan.

## Current behavior

The real `C:\Users\moxhe` scan completed with 188,580 files, 37,740 folders, and 3 access issues. The WPF grid intentionally showed the largest 2,000 review rows, but the status/filter summary did not clearly say whether those 2,000 rows were all matches or a capped display window.

## Desired behavior

When the active review result has more than 2,000 matched rows:

- The completed-scan status should say `Showing 2,000 of ... paths`.
- The filter summary should say `2,000 shown of ... matched`.
- The filter summary should explain the display limit and suggest narrowing with filters/search.
- The wording should remain read-only and not imply files were modified.
- A later Storage Review Display Window packet replaced first-window-only wording with row ranges such as `rows 1-2,000 of ... matched` and added Previous rows / Next rows navigation.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Display Limit | Added as the WPF grid display cap for matched review rows. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should a future UI add paging or a virtualized tree/grid for all matched rows?

## Grill notes

### Scenarios discussed

- The user tested the app against `C:\Users\moxhe`; the scan completed and the screenshot showed a large real-profile result set.
- The grid needs to stay usable while making it obvious that more matched rows may exist beyond the first 2,000.

### Edge cases

- The display limit must be described as a review UI boundary, not a scan boundary.
- Filter/search combinations below the limit should keep the simpler `shown` wording.
- Largest-row wording should avoid summed flattened sizes because parent/child rows overlap.

### Dependencies between decisions

- This depends on the existing in-memory `StorageScanReview`.
- This does not change classifier output, scan scope, exports, shortlist semantics, or preview eligibility.

## Evidence and validation gate

Evidence gathered:

- User screenshot from a real `C:\Users\moxhe` scan.
- Existing WPF display cap `MaxDisplayedRows = 2000`.
- Existing WPF fixture smoke tests.

Tests/checks planned:

- WPF app smoke coverage with a synthetic fixture containing more than 2,000 review rows.
- Build, core tests, WPF tests, and MVP preflight.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not lower the scan depth or skip files to fit the grid.
- Do not add cleanup execution while improving review wording.
- Do not call displayed rows the complete scan result.

## Decisions made

Small feature-level decisions:

- Keep the limit in the WPF review layer.
- Compute matched count from the active filter/search before applying the display cap.
- Keep scan/export/review data backed by full in-memory review results.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Split matched review rows from displayed WPF rows.
2. Update completed-scan status and filter summary wording.
3. Add WPF smoke coverage for a capped result set.
4. Update domain docs, README, readiness audit, and progress log.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-display-limit.md`
- `.codex/progress.md`

Possible:

- `docs/features/2026-05-28-mvp-readiness-audit.md`

## Test plan

Manual checks:

- On a large real-profile scan, confirm the status/filter summary says whether the grid is capped.
- Use search or category filters and confirm smaller matched sets show naturally.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

## Risks and assumptions

Risks:

- The grid still lacks paging, so very large scans require search/filter narrowing.
- WPF smoke tests verify state and wording, not visible layout quality.

Assumptions:

- A 2,000-row grid cap is acceptable for the MVP when the app clearly reports matched count.

## Completion notes

Completed on: 2026-05-28

What changed:

- Split matched review rows from displayed WPF rows.
- Updated completed-scan status and filter summary to say when only the first 2,000 matched rows are shown.
- Added WPF smoke coverage for a large synthetic fixture that exceeds the display limit.
- Later Storage Review Display Window packet added Previous rows / Next rows controls over matched in-memory rows and updated wording to show row ranges.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-display-limit.md`
- `.codex/progress.md`

Tests run:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-display-limit.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is reversible WPF review wording and smoke coverage; it does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Consider paging or virtualization only after the visible real-profile review flow shows the 2,000-row cap is still too limiting.
- Row-window navigation now exists; consider virtualization or a tree/grid only if row windows are still too limiting.

Open questions:

- Should a future UI add a virtualized tree/grid for all matched rows?

Risky assumptions:

- The current grid cap remains useful when paired with search/filter narrowing.
