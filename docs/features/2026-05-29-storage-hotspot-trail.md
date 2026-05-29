# Feature: Storage Hotspot Trail

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make selected-folder inspection faster by showing the largest nested path inside the selected folder.

This helps the user understand where a large container folder's size appears to concentrate before shortlisting or previewing anything.

## Non-goals

- Do not rescan the filesystem.
- Do not add a recursive tree view in this packet.
- Do not calculate confirmed storage savings.
- Do not change Bloat Categories, Importance Ratings, Deletion Recommendations, Review Shortlist, Quarantine Preview, or cleanup eligibility.
- Do not add real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or cleanup history.

## User story / job story

As the project owner, I want selecting a large folder to show the largest path through its nested contents, so that I can quickly see whether storage is concentrated in something like `DXCache`, a package cache, a browser profile, or a protected data area.

## Current behavior

The selected-row detail pane shows Evidence, Review guidance, Largest immediate children, and File preview. The new `Show children` action can focus the grid on direct children.

The user still has to manually hop level by level to identify the largest nested path inside a large folder.

## Desired behavior

- Add a read-only Storage Hotspot Trail for selected folders.
- The trail follows the largest immediate child at each level, with deterministic name tie-breaking.
- The trail stops at a file, at an empty folder, or at a bounded maximum depth.
- The detail pane explains that sizes overlap down the trail and are not storage savings.
- Files explicitly show that they do not have descendant hotspot trails.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Hotspot Trail | Added as a read-only selected-folder summary. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should a later version show multiple hotspot trails instead of only the single largest-child trail?
- Should the trail eventually support clickable rows that focus the grid on that child?

## Grill notes

### Scenarios discussed

- The user's real scan screenshot showed large nested folders under `AppData`, `NVIDIA`, and `pip`.
- A one-path trail helps identify the next folder to inspect without making cleanup claims.

### Edge cases

- Files have no trail.
- Empty folders have no trail.
- Ties should be deterministic.
- The terminal row may be a file.

### Dependencies between decisions

- This depends on the completed Storage Scan tree.
- This complements Child Breakdown and Selected Folder Child Focus.
- This remains read-only and does not affect cleanup workflows.

## Evidence and validation gate

Evidence gathered:

- User screenshot of a large real-profile scan.
- Existing `StorageEntry.Children`, Child Breakdown, Selected Folder Child Focus, WPF detail pane, and smoke tests.
- Project rule: inspect before recommending removal.

Tests/checks planned:

- Core coverage for largest-child path, deterministic ordering, bounded depth, and file terminal behavior.
- WPF smoke coverage that selected folder details show a hotspot trail and selected files do not enable one.
- Build, both test harnesses, and MVP preflight.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not implement a full tree view before manual review proves the smaller trail/focus tools are insufficient.
- Do not call the trail a savings estimate.
- Do not use the hotspot trail to auto-shortlist or recommend cleanup.

## Decisions made

Small feature-level decisions:

- Start with the single largest-child trail.
- Include a file as the terminal row when the largest branch ends in a file.
- Keep depth bounded for readability.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add `StorageHotspotTrailEntry` and `StorageHotspotTrailBuilder`.
2. Add WPF detail-pane hotspot trail text.
3. Add core and WPF smoke coverage.
4. Update README/manual checklist and progress after verification.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/StorageHotspotTrailEntry.cs`
- `src/WindowsFileCleaner.Core/StorageHotspotTrailBuilder.cs`
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

## Test plan

Manual checks:

- Run the fixture app, scan, select `AppData`, and confirm the detail pane shows a largest nested path.
- In the next real scan, try the trail on `AppData`, `Local`, `NVIDIA`, `pip`, and browser folders.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

## Risks and assumptions

Risks:

- A single largest branch can hide important second-place children.
- Trail rows can still involve overlapping parent/child sizes, so wording must avoid implying savings.

Assumptions:

- The single largest-child path is useful enough to test before designing a larger tree/grid.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added `StorageHotspotTrailEntry` and `StorageHotspotTrailBuilder`.
- Added a Largest hotspot trail section to the WPF selected-row detail pane.
- The trail follows the largest child at each level, stops at the terminal file/folder or bounded depth, and tells the user that sizes overlap and are not storage savings.
- Files explicitly report that they do not have descendant hotspot trails.
- Added core and WPF smoke coverage.

Files changed:

- `src/WindowsFileCleaner.Core/StorageHotspotTrailEntry.cs`
- `src/WindowsFileCleaner.Core/StorageHotspotTrailBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-storage-hotspot-trail.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`
- This feature brief

ADRs added or skipped:

- No ADR added. This is a reversible read-only review aid that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Follow-up work:

- Try the trail during the next fixture and real-profile review pass on `AppData`, `Local`, `NVIDIA`, `pip`, and browser/cache folders.
- Consider multiple hotspot trails or clickable/focusable trail rows if the single-path trail is too narrow.

Open questions:

- Should a later version show multiple hotspot trails instead of only the single largest-child trail?
- Should the trail eventually support clickable rows that focus the grid on that child?

Risky assumptions:

- The single largest-child path is useful enough to test before designing a larger tree/grid.
- The overlap/not-storage-savings wording is clear enough for manual review.
