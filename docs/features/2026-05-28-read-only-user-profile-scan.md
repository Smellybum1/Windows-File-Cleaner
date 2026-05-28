# Feature: Read-only User Profile Scan

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Create the first workflow for the WPF desktop app: a read-only Storage Scan and review experience for `C:\Users\moxhe` that helps the user understand where storage is going before any cleanup action exists.

The workflow should identify large folders and possible Cleanup Candidates, show what is inside them, estimate Storage Savings, assign conservative Importance Ratings, and provide Deletion Recommendations that prioritize not breaking current apps.

## Non-goals

- Do not delete files.
- Do not move files.
- Do not create or use Quarantine yet.
- Do not scan the entire `C:` drive by default.
- Do not make automatic cleanup decisions.
- Do not require administrator privileges unless a future decision explicitly chooses that.

## User story / job story

As the project owner, I want to recursively scan `C:\Users\moxhe` and see which folders look like unwanted bloat, so that I can decide what is safe to clean without breaking Windows, Codex, development tools, games, or current apps.

## Current behavior

There is no app yet. The repo currently contains the Grill with Docs scaffold and project docs.

## Desired behavior

The first production workflow should:

- Run as a desktop app.
- Use C# and WPF.
- Run the Storage Scan workflow.
- Recursively scan the Cleanup Scope `C:\Users\moxhe`.
- Show the largest folders and files contributing to storage usage.
- Show folder contents enough to understand what a candidate is.
- Categorize likely unwanted bloat.
- Rate candidate importance conservatively.
- Recommend keep, inspect further, quarantine candidate, or delete later.
- Clearly mark Protected Locations and high-risk candidates.
- Avoid modifying the filesystem.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Bloat Category | Added as the reason a candidate may be unwanted or removable. | yes |
| Importance Rating | Added as a conservative estimate of how important a path may be. | yes |
| Deletion Recommendation | Added as the suggested action after inspection. | yes |
| Quarantine | Refined as preferred future cleanup destination on `D:`. | yes |
| Undo Quarantine | Added as the future restore workflow. | yes |
| Restore Manifest | Added as required metadata for future undo behavior. | yes |
| Storage Scan | Added as the first workflow name. | yes |

## Open questions

Questions that must be answered before implementation:

- None. The first implementation can start.

Questions that can be deferred:

- What exact `D:` quarantine path should be used?
- Should permanent deletion ever be included?
- Should Recycle Bin be offered as an alternative to Quarantine?
- Should the app persist scan history?
- Should the app export reports?

## Grill notes

### Scenarios discussed

- The user wants a read-only storage scan first.
- The user wants protected and sensitive areas to be visible for inspection, with an app-provided estimate of importance and cleanup suitability.
- The user considers old downloads, temp folders, installer caches, app caches, duplicate files, large old files, old game files, Node/Python package caches, and Windows app leftovers to be likely bloat if removing them does not break current apps.
- The user prefers a Quarantine folder on `D:` with easy undo for eventual cleanup.
- The user prefers a desktop app.
- The user chose `C:\Users\moxhe` as the initial Cleanup Scope.
- The user chose recursive scanning for the first scan.
- The user chose `Likely safe`, `Caution`, and `High risk` as Importance Rating labels.
- The user chose Storage Scan as the workflow name.
- The user asked Codex to choose the desktop stack; Codex chose C# WPF.

### Edge cases

- Large files may be important.
- `AppData` may contain both bloat and critical app state.
- Package caches may be reclaimable but can affect active development tools.
- Codex-related files must be treated conservatively.
- Browser profiles, credentials, source code, and game saves need high-risk handling.

### Dependencies between decisions

- The desktop stack affects filesystem access, Windows integration, packaging, and quarantine implementation.
- The scan depth affects performance and how useful the first report feels.
- The Importance Rating scale affects UI language, tests, and recommendation logic.
- Quarantine design depends on a Restore Manifest, but Quarantine is deferred until after read-only scan/review.
- Targeting .NET 8 allows immediate local builds; targeting .NET 10 requires SDK installation but gives a longer support horizon.

## Evidence and validation gate

Evidence gathered:

- User answers:
  - First workflow should be read-only.
  - App should show folder contents and rate importance/deletion suitability.
  - Listed bloat categories are valid if cleanup does not break current apps.
  - Quarantine on `D:` with easy undo is preferred.
  - Desktop app is preferred.
  - Initial Cleanup Scope is `C:\Users\moxhe`.
  - First scan should be recursive.
  - Importance Rating labels are `Likely safe`, `Caution`, and `High risk`.
  - Workflow name is Storage Scan.
  - Codex should choose the desktop stack.
- Existing code/docs inspected:
  - `AGENTS.md`
  - `docs/domain/context.md`
  - `docs/domain/glossary.md`
  - `.codex/progress.md`
- Local environment inspected:
  - .NET SDK 8.0.421 is installed.
  - .NET SDK 9.0.314 is installed.
  - Microsoft.WindowsDesktop.App runtimes for .NET 8 and .NET 9 are installed.
- Tests/checks planned:
  - Unit tests for scan summarization, path protection rules, category assignment, and recommendation logic after stack is selected.
  - Manual scan against a small fixture directory before scanning real `C:\Users`.

Validation gate before implementation:

- [x] Desktop stack selected.
- [x] Initial target framework selected.
- [x] Initial Cleanup Scope selected.
- [x] Scan depth and performance expectations clarified enough for a first version.
- [x] Importance Rating scale selected.
- [x] First workflow name selected.
- [x] Cleanup execution deferred.

Rejected ideas buffer:

- Do not implement deletion in the first feature.
- Do not treat all listed bloat categories as automatically safe.
- Do not hide Protected Locations entirely; show them with conservative ratings instead.
- Do not optimize only for maximum reclaimed space.

## Decisions made

Small feature-level decisions:

- Start with read-only scan/review.
- Prefer desktop app.
- Use C# WPF for the desktop stack.
- Start with `C:\Users\moxhe` as the Cleanup Scope.
- Scan recursively.
- Use `Likely safe`, `Caution`, and `High risk` for Importance Rating.
- Name the first workflow Storage Scan.
- Treat Quarantine on `D:` with easy undo as the preferred future cleanup path.
- Show contents and explain recommendations instead of only listing sizes.

ADR-worthy decisions:

- [ ] None
- [x] ADR added: `docs/decisions/0002-use-dotnet-wpf-desktop-stack.md`
- [ ] ADR may be needed before implementing Quarantine and Undo Quarantine.

## Implementation plan

Implementation may start. The first implementation should target .NET 8 for local buildability.

1. Scaffold a WPF app project and a testable core library.
2. Define scan result model using glossary terms.
3. Build read-only recursive scanner against a fixture directory.
4. Add Protected Location and Bloat Category rules.
5. Add Importance Rating and Deletion Recommendation logic.
6. Build Storage Scan desktop review UI for scan results.
7. Verify with fixture data before scanning real `C:\Users\moxhe`.

## Files expected to change

Expected:

- App source files after stack selection.
- Test fixture files or generated test directories.
- `AGENTS.md` project commands.
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- This feature brief.

Possible:

- New ADR for Quarantine design later.

## Test plan

Manual checks:

- Run scanner against a small artificial fixture tree.
- Confirm hidden/protected-like folders are not recommended as low risk by default.
- Confirm output shows path, size, category evidence, Importance Rating, and Deletion Recommendation.
- Confirm no files are modified during scan.
- Confirm recursive scan remains within `C:\Users\moxhe` or fixture Cleanup Scope.

Automated tests:

- Scanner totals file sizes correctly.
- Scanner remains within Cleanup Scope.
- Protected Location rules raise risk/recommend keep or inspect.
- Bloat Category rules explain why a candidate is shown.
- Recommendation logic does not recommend cleanup solely because a path is large.

## Risks and assumptions

Risks:

- Recursively scanning all of `C:\Users\moxhe` may be slow or hit permission errors.
- Some folders may be inaccessible without administrator privileges.
- App caches and package caches can be safe in one context and risky in another.
- Importance Rating can create false confidence if explanations are weak.

Assumptions:

- Read-only scan of `C:\Users\moxhe` does not need administrator privileges for a useful first version.
- The first version can use fixture directories for verification before scanning real user data.
- Quarantine and deletion are separate future features.

## Completion notes

Completed on: 2026-05-28

What changed:

- Scaffolded a .NET 8 WPF app, a testable core library, and a dependency-free console test harness.
- Implemented read-only recursive Storage Scan logic constrained by Cleanup Scope.
- Added Bloat Category, Importance Rating, and Deletion Recommendation classification.
- Added a WPF Storage Scan screen with path input, scan/cancel buttons, scan totals, a sortable results grid, and an evidence detail panel.
- Added fixture-based tests before scanning real `C:\Users\moxhe`.

Files changed:

- `WindowsFileCleaner.sln`
- `global.json`
- `NuGet.Config`
- `.gitignore`
- `src/WindowsFileCleaner.Core/`
- `src/WindowsFileCleaner.App/`
- `tests/WindowsFileCleaner.Tests/`
- `AGENTS.md`
- `.codex/progress.md`
- `docs/features/2026-05-28-read-only-user-profile-scan.md`

Tests run:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- `AGENTS.md`
- `.codex/progress.md`
- This feature brief

ADRs added or skipped:

- Existing ADR used: `docs/decisions/0002-use-dotnet-wpf-desktop-stack.md`
- No new ADR added for scanner internals; implementation follows the feature brief and existing safety rules.

Follow-up work:

- Add a richer candidate review model that can group results by Bloat Category and Importance Rating.
- Add a safe scan preview mode against a small bundled fixture from the UI.
- Add Quarantine and Undo Quarantine only after Storage Scan is tested by the user.
- Consider moving from the console harness to a formal test framework when adding more rules.

Open questions:

- What exact `D:` quarantine path should be used later?
- Which Protected Locations should be shown as expandable groups in the UI?
- How much result history should the app persist, if any?

Risky assumptions:

- The first UI can show the top 2,000 largest paths rather than every scanned path.
- Recursive scanning of `C:\Users\moxhe` is acceptable when explicitly triggered by the user.
- Fixture-based verification is enough before asking the user to run the desktop app.
