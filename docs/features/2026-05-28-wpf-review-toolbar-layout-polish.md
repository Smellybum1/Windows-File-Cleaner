# Feature: WPF Review Toolbar Layout Polish

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make the Storage Scan review controls more robust for the visible fixture UI pass by replacing the wide fixed review toolbar with wrapping toolbars.

## Non-goals

- Do not change scanner behavior.
- Do not scan `C:\Users\moxhe`.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or persistent scan history.
- Do not redesign the whole app shell.

## User story / job story

As the project owner, I want the review controls to stay readable during manual fixture testing, so that filters, shortlist actions, category filtering, and preview controls can be checked without layout crowding.

## Current behavior

The review toolbar uses a fixed grid row for filter buttons and a horizontal stack for shortlist, preview, export, and category controls. This works at the current default size, but it is fragile when the app is narrowed or when button labels include counts.

## Desired behavior

The WPF layout should:

- Keep the same controls and read-only behavior.
- Let Storage Review Filter buttons wrap.
- Let Review Shortlist, Quarantine Preview, export, and Bloat Category Filter controls wrap.
- Keep Filter Summary visible as its own line.
- Preserve automated WPF smoke coverage.

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

- Should the entire app later get a broader visual design pass after the real-profile retest?

## Evidence and validation gate

Evidence gathered:

- The README now points to a manual fixture UI pass for layout and visible wording.
- The current XAML uses fixed toolbar rows that can crowd when controls or counts grow.
- App tests can verify the intended wrapping toolbar structure without launching a visible desktop window.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not introduce a new UI framework or dependency for this polish.
- Do not automate real-profile scans for layout testing.
- Do not treat wrapping layout structure as proof of visual quality.

## Decisions made

Small feature-level decisions:

- Replace the fixed review toolbar grid with two named `WrapPanel` toolbars.
- Keep Filter Summary as a full-width line between filter controls and action controls.
- Add a WPF smoke assertion that the review controls use wrapping toolbars.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update the review toolbar XAML to use wrapping panels.
2. Expose a small read-only layout property for WPF smoke assertions.
3. Add app test coverage for the wrapping toolbar layout.
4. Update README, MVP audit, and progress docs.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-review-toolbar-layout-polish.md`
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

- This improves layout resilience but still does not replace a human visible check.

Assumptions:

- Wrapping the existing controls is enough polish for the next manual fixture pass.

## Completion notes

Completed on: 2026-05-28

What changed:

- Replaced the fixed review toolbar grid with two wrapping toolbars.
- Kept Filter Summary on its own wrapping line between review filters and review actions.
- Added a small WPF smoke assertion that the review controls use wrapping toolbars.
- Kept all scan and review behavior read-only.
- Later refinement on 2026-05-29 split shortlist and Quarantine Preview controls into a separate `ReviewShortlistToolbar` after Quarantine Root Selection and browse controls made the review toolbar wider.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-review-toolbar-layout-polish.md`
- `.codex/progress.md`

Tests run:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`
- Later refinement verified with `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`.

Docs updated:

- This feature brief.
- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is reversible UI layout polish and does not change architecture, persistence, security, deployment, or cleanup behavior.

Follow-up work:

- Manual visible fixture UI pass for actual layout quality, especially the now-separated shortlist/quarantine controls, export dialogs, and wording.
- Manual real-profile retest against `C:\Users\moxhe`.

Open questions:

- Should the entire app later get a broader visual design pass after the real-profile retest?

Risky assumptions:

- Wrapping the existing toolbar controls is enough layout polish for the next manual fixture pass.
