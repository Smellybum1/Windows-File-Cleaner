# Feature: WPF Review Interaction Smoke Test

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Prove that the WPF shell can exercise key read-only review interactions against a synthetic fixture before another real-profile scan.

## Non-goals

- Do not scan `C:\Users\moxhe`.
- Do not launch the visible desktop app.
- Do not automate export dialogs.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or persistent scan history.

## User story / job story

As the project owner, I want automated WPF smoke coverage for the review controls, so that filters, shortlist state, and Quarantine Preview readiness are less likely to regress before I review real files.

## Current behavior

The WPF app test project already constructs `MainWindow`, verifies launch Cleanup Scope wiring, and runs a synthetic fixture scan through the WPF shell. It does not yet exercise the review controls that the README asks the user to manually test.

## Desired behavior

The WPF app test project should verify that a synthetic fixture scan can:

- Apply Storage Review Filters.
- Apply Bloat Category Filters, including No category.
- Apply Safety Summary review shortcuts.
- Select a likely-safe cleanup candidate.
- Add and remove the selected row from the Review Shortlist.
- Create a Quarantine Preview from the Review Shortlist.
- Show Restore Manifest Draft and Quarantine Confirmation Draft readiness text without modifying files.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Filter | No change. | not needed |
| Bloat Category Filter | No change. | not needed |
| Review Shortlist | No change. | not needed |
| Quarantine Preview | No change. | not needed |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should future visible GUI automation or screenshot verification be added after the manual fixture pass?

## Evidence and validation gate

Evidence gathered:

- README manual MVP checklist asks the user to test filters, Review Shortlist, and Quarantine Preview.
- Core tests already prove the underlying review and preview logic.
- The current WPF fixture scan smoke test proves scan state, but not control interactions.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not use real profile data in automated WPF interaction tests.
- Do not automate file export dialogs in this packet.
- Do not treat automated interaction state as proof of visible layout quality.

## Decisions made

Small feature-level decisions:

- Refactor button handlers into small command methods that can be called by tests and UI events.
- Keep assertions focused on read-only state, visible text, and fixture file preservation.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add read-only WPF state properties needed by interaction assertions.
2. Add command methods for filters, safety shortcuts, selection, shortlist, and Quarantine Preview.
3. Add a WPF app test that runs a fixture scan and exercises the review controls.
4. Verify fixture files still exist after the interactions.
5. Update docs and progress.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-review-interaction-smoke-test.md`
- `.codex/progress.md`

Possible:

- None

## Test plan

Manual checks:

- Visible fixture UI pass remains recommended for layout and wording.
- Real-profile retest against `C:\Users\moxhe` remains recommended after fixture review.

Automated tests:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- This proves WPF interaction state but not visual layout or human readability.
- Synthetic fixture rows are smaller and simpler than the real profile.

Assumptions:

- The existing synthetic fixture has enough category variety to cover the important review interactions.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added WPF command methods for Storage Review Filters, Bloat Category Filters, Safety Summary review shortcuts, displayed-row selection, Review Shortlist changes, and Quarantine Preview generation.
- Added WPF fixture interaction coverage for filters, No category, protected shortcut, shortlist add/remove, Quarantine Preview, Restore Manifest Draft text, Quarantine Confirmation Draft text, disabled execution wording, and fixture file preservation.
- Kept the workflow read-only; no cleanup execution or manifest writing was added.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-review-interaction-smoke-test.md`
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

- No ADR added. This is testability and smoke coverage for existing read-only UI behavior, not a durable architecture, persistence, security, deployment, or cleanup-execution decision.

Follow-up work:

- Manual visible fixture UI pass for layout, export dialogs, and wording.
- Manual real-profile retest against `C:\Users\moxhe`.

Open questions:

- Should visible GUI automation or screenshot verification be added after the manual fixture pass?

Risky assumptions:

- State-level WPF assertions are enough to catch the main review interaction regressions before the next manual pass.
