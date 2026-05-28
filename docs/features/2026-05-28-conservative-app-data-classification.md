# Feature: Conservative App Data Classification

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Make real-scan-inspired app, game, and Windows package folders easier to understand while keeping recommendations conservative.

## Non-goals

- Do not add deletion.
- Do not add Quarantine execution.
- Do not turn active app state into Quarantine candidates.
- Do not classify paths as removable only because they are large or old.

## User story / job story

As the project owner, I want ambiguous app and game folders to show clearer categories and risk, so that I can avoid breaking current apps while reviewing storage usage.

## Current behavior

The scanner already labels AppData areas, browser data, package caches, and GPU shader caches. Real scan rows such as Windows app package folders, per-user installed app folders, and game folders need clearer labels so they do not sit in broad or ambiguous review buckets.

## Desired behavior

- `AppData\Local\Packages` paths are labeled as Windows app data and Protected Location.
- `AppData\Local\Programs` paths are labeled as installed applications and Protected Location.
- Known game and mod-manager folders such as Larian/Baldur's Gate/Stellaris/IronyMod, Minecraft/OptiFine, CurseForge, Modrinth, Vortex, and Nexus Mods paths are labeled as game data and Protected Location.
- These rows stay `High risk` / `Keep`.
- Cache-specific rows such as GPU shader caches remain conservative `Caution` / `Inspect`.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Bloat Category | Expanded category examples to include game data, Windows app data, and installed applications. | yes |
| Protected Location | Clarified Windows app data, per-user app installs, and game data as protected by default. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This packet makes conservative labels more explicit.

Questions that can be deferred:

- Which specific app, game, or mod-manager folders should get user-approved cleanup exceptions later?
- Should some Windows app package subfolders, such as `TempState`, become Caution instead of High risk after manual review?

## Grill notes

### Scenarios discussed

- The first real scan showed large app, game, and mod-related folders mixed with cache rows.
- The app must not break current apps such as Codex or installed tools.
- The user wants to remove bloat, but only when it will not affect current apps.

### Edge cases

- A known game or mod-manager folder may contain saves, mods, profiles, load orders, or configuration, so it should be protected even if large.
- `AppData\Local\Programs` can contain per-user app installations, so it should not be treated as cache.
- Windows app package data can contain active app state even when stale-looking.

### Dependencies between decisions

- Depends on existing Bloat Category and Protected Location behavior.
- Future cleanup exceptions must be explicit and separate from this conservative classification.

## Evidence and validation gate

Evidence gathered:

- User scan screenshot and progress notes showed app/game/vendor rows that needed clearer triage.
- Existing code/docs inspected: classifier, category formatting, CSV exporters, test harness, domain context, glossary.
- Tests/checks planned: classifier fixture coverage, build, test harness, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not mark Windows app package data as likely safe by default.
- Do not classify game or mod-manager folders as removable just because they look old.
- Do not make `AppData\Local\Programs` a cleanup candidate.

## Decisions made

Small feature-level decisions:

- Add `WindowsAppData`, `InstalledApplication`, and `GameData` Bloat Category values.
- Treat those categories as Protected Location / High risk / Keep by default.
- Preserve existing cache categories for cache-specific paths.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add new category values and display labels.
2. Add conservative classifier hints for Windows app package data, installed app folders, and known game/mod-manager folders.
3. Add fixture coverage for the new patterns.
4. Update docs and progress log.
5. Run build, tests, and diff checks.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/BloatCategory.cs`
- `src/WindowsFileCleaner.Core/CleanupCandidateClassifier.cs`
- `src/WindowsFileCleaner.Core/StorageScanCsvExporter.cs`
- `src/WindowsFileCleaner.Core/QuarantinePreviewCsvExporter.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `src/WindowsFileCleaner.App/StorageEntryRow.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-conservative-app-data-classification.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Rerun Storage Scan and inspect Windows app package, per-user installed app, and known game/mod-manager folders.
- Confirm these rows are clearer and remain High risk / Keep.

Automated tests:

- Verify `Packages` is Windows app data and Protected Location.
- Verify `Programs` is installed application data and High risk.
- Verify known game/mod-manager folders are game data and High risk.
- Verify build and full fixture test harness pass.

## Risks and assumptions

Risks:

- More high-risk labels may increase review noise, but that is preferable to accidentally recommending active app data.

Assumptions:

- It is safer for the MVP to over-protect active app/game state until the user defines precise cleanup exceptions.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added Windows app data, installed application, and game data category values.
- Added conservative classifier hints for `AppData\Local\Packages`, `AppData\Local\Programs`, known game folders, and later Minecraft/OptiFine, CurseForge, Modrinth, Vortex, and Nexus Mods mod-manager folders.
- Kept these rows High risk / Keep and blocked from Quarantine Preview through Protected Location.
- Added display/export labels and fixture coverage.

Files changed:

- `src/WindowsFileCleaner.Core/BloatCategory.cs`
- `src/WindowsFileCleaner.Core/CleanupCandidateClassifier.cs`
- `src/WindowsFileCleaner.Core/StorageScanCsvExporter.cs`
- `src/WindowsFileCleaner.Core/QuarantinePreviewCsvExporter.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `src/WindowsFileCleaner.App/StorageEntryRow.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-conservative-app-data-classification.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- Later game/mod-manager packet:
  - `dotnet build WindowsFileCleaner.sln --no-restore`
  - `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
  - `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

Docs updated:

- Added this feature brief.
- Updated domain category examples and Protected Location examples.
- Updated progress log.

ADRs added or skipped:

- Skipped. This is an incremental conservative classifier refinement.

Follow-up work:

- Rerun the real Storage Scan and inspect whether No category rows shrink without hiding useful review candidates.
- Add user-approved exceptions only after specific folders are manually reviewed.

Open questions:

- Which specific app, game, or mod-manager folders should get cleanup exceptions later?
- Should some Windows app package subfolders be downgraded after manual review?

Risky assumptions:

- Over-protecting app/game state is preferable to accidentally suggesting cleanup that breaks current apps.
