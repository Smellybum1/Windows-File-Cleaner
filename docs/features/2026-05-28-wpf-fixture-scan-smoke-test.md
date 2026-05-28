# Feature: WPF Fixture Scan Smoke Test

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Prove that the WPF shell can run a read-only Storage Scan against a synthetic fixture and display the expected review evidence.

## Non-goals

- Do not scan `C:\Users\moxhe`.
- Do not automate every visible button interaction.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or persistent scan history.
- Do not create fixture files from production app code.

## User story / job story

As the project owner, I want an automated WPF fixture scan smoke test, so that basic desktop scan behavior is verified before I scan my real user profile again.

## Current behavior

Core tests prove scanner and review behavior. WPF shell tests prove the window initializes with the default or launch Cleanup Scope without starting a scan. The remaining automated gap is proving a fixture scan can pass through the WPF shell and update the visible review state.

## Desired behavior

The WPF app test project should:

- Create a synthetic fixture under the ignored `test-fixtures` area.
- Construct `MainWindow` with that fixture Cleanup Scope.
- Run the same scan method used by the Scan button.
- Verify the WPF status says Storage Scan completed and no files were modified.
- Verify summary cards, Review Mix, Safety Summary, Filter Summary, category filter, and CSV export state update after the scan.
- Verify visible rows include expected fixture categories, Importance Ratings, and Deletion Recommendations.
- Verify fixture files still exist after the scan.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Storage Scan | No change. | not needed |
| Cleanup Scope | No change. | not needed |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should future WPF tests automate selecting rows, applying filters, adding to Review Shortlist, and creating Quarantine Preview?

## Evidence and validation gate

Evidence gathered:

- The MVP readiness audit still leaves latest WPF interaction retest pending.
- WPF shell smoke tests verify construction and launch-scope wiring but not a completed scan.
- Core tests already prove classifier and preview behavior, so this packet can focus on WPF scan state.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not treat automated fixture scan state as proof of layout quality.
- Do not add visible GUI automation dependencies until manual fixture review identifies a real need.
- Do not use real profile data in automated WPF tests.

## Decisions made

Small feature-level decisions:

- Refactor the Scan button handler to call a public `RunStorageScanForCurrentScopeAsync` method.
- Keep UI assertions read-only through small `MainWindow` state properties.
- Create/delete the WPF fixture only inside the test harness.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Expose read-only WPF state properties needed by the smoke test.
2. Refactor Storage Scan execution into a method shared by the Scan button and the test.
3. Add a WPF app test that creates a synthetic fixture and runs the scan through `MainWindow`.
4. Assert visible rows include expected categories, ratings, and recommendations.
5. Update docs and progress.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-fixture-scan-smoke-test.md`
- `.codex/progress.md`

Possible:

- `AGENTS.md`

## Test plan

Manual checks:

- Still run the fixture WPF app manually to inspect layout and interaction wording.
- Still run a real-profile scan against `C:\Users\moxhe` before considering the MVP fully verified.

Automated tests:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- This proves WPF scan state but not visual layout or every interactive workflow.
- Synthetic fixture sizes are small and do not prove real-profile performance.

Assumptions:

- A synthetic fixture with Downloads, AppData cache, protected Documents, Codex-related data, and uncategorized rows is enough to exercise the core review state in WPF.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added shared WPF scan execution through `RunStorageScanForCurrentScopeAsync`.
- Added WPF fixture scan assertions for status, summary cards, Review Mix, Safety Summary, Filter Summary, category filter, CSV availability, row categories, ratings, recommendations, and fixture file preservation.
- Kept all scan behavior read-only.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-fixture-scan-smoke-test.md`
- `.codex/progress.md`

Tests run:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- This feature brief.
- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is a testability improvement and does not change architecture, persistence, security posture, or cleanup behavior.

Follow-up work:

- Manual fixture UI pass for layout and control wording.
- Manual real-profile retest against `C:\Users\moxhe`.

Open questions:

- Should future WPF automation cover Review Shortlist and Quarantine Preview interactions?

Risky assumptions:

- The fixture scan is representative enough to catch WPF scan-state regressions.
