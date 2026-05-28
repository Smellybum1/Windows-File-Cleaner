# Feature: WPF Fixture Smoke Launch

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make the WPF app easier to smoke test against a synthetic Cleanup Scope before scanning `C:\Users\moxhe`.

## Non-goals

- Do not add cleanup execution.
- Do not add automatic scanning on startup.
- Do not modify real user files.
- Do not persist scan history or Review Shortlist state.

## User story / job story

As the project owner, I want to launch the WPF app against a small fixture Cleanup Scope, so that I can verify the latest review controls before scanning my real user profile.

## Current behavior

The WPF app defaults to `C:\Users\moxhe`. The core test harness uses synthetic fixtures, but there is no documented way to open the WPF app with a fixture path already in the Cleanup Scope box.

## Desired behavior

The WPF app accepts a launch argument:

```powershell
dotnet run --project src\WindowsFileCleaner.App -- --scope "<fixture path>"
```

The argument only sets the initial Cleanup Scope text. The user must still click `Scan`.

The repo also includes a helper script that creates a small synthetic fixture inside `.local\storage-scan-smoke-fixture`.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope | No change. The launch argument sets the initial Cleanup Scope text. | not needed |
| Storage Scan | No change. Startup does not trigger a scan. | not needed |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should a future debug-only mode auto-load a fixture result without opening the real scanner?

## Grill notes

### Scenarios discussed

- The latest WPF controls need manual retesting, but scanning the real profile is slower and higher-friction than a fixture pass.
- A fixture launch path gives a safer first pass while preserving the real manual retest as the final check.

### Edge cases

- Missing `--scope` value should be rejected.
- Unknown launch arguments should be rejected so accidental flags do not silently change behavior.
- Startup should not begin scanning automatically.

### Dependencies between decisions

- Fixture-driven WPF smoke testing depends on launch-scope support.
- Actual Quarantine execution remains deferred until after review/preview semantics are trusted.

## Evidence and validation gate

Evidence gathered:

- User wants fixture-based verification before scanning real user files.
- Existing app startup used `StartupUri`, which always created a default `MainWindow`.
- Existing core tests already validate scanner behavior with synthetic fixtures.
- MVP readiness audit recorded latest WPF manual retest as pending.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make the app auto-scan on launch because that would surprise the user.
- Do not create fixture files from production app code.
- Do not treat fixture WPF smoke testing as a substitute for one final real-profile retest.

## Decisions made

Small feature-level decisions:

- Add launch argument parsing in the core library so it can be tested without WPF automation.
- Keep fixture creation in `tools\`, outside production code.
- Ignore `.local\` fixture output in Git.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add `StorageScanLaunchOptions` to parse optional `--scope`.
2. Replace `StartupUri` with explicit WPF startup wiring.
3. Let `MainWindow` accept an initial Cleanup Scope path.
4. Add parser coverage to the console test harness.
5. Add a synthetic fixture creation script.
6. Update README, AGENTS, the MVP readiness audit, and progress log.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/StorageScanLaunchOptions.cs`
- `src/WindowsFileCleaner.App/App.xaml`
- `src/WindowsFileCleaner.App/App.xaml.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tools/New-StorageScanSmokeFixture.ps1`
- `.gitignore`
- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

Possible:

- None

## Test plan

Manual checks:

- Run `tools\New-StorageScanSmokeFixture.ps1`.
- Launch the app with the printed `--scope` command.
- Click `Scan` and check filters, shortlist, Quarantine Preview, and readiness draft wording against the fixture.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- Fixture smoke testing cannot prove real-profile performance or exact real-profile categories.
- WPF layout and interaction still need human eyes.

Assumptions:

- Launching with `--scope` should only set the initial Cleanup Scope box.
- Fixture output belongs under `.local\` and should stay untracked.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added tested launch-scope parsing.
- Updated WPF startup to create `MainWindow` with the parsed Cleanup Scope.
- Added a synthetic fixture helper script.
- Documented a fixture-first WPF smoke path.

Files changed:

- `src/WindowsFileCleaner.Core/StorageScanLaunchOptions.cs`
- `src/WindowsFileCleaner.App/App.xaml`
- `src/WindowsFileCleaner.App/App.xaml.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tools/New-StorageScanSmokeFixture.ps1`
- `.gitignore`
- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- This feature brief.
- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is a reversible launch/testability improvement and does not change architecture, persistence, cleanup behavior, or security posture.

Follow-up work:

- Run the WPF app against the synthetic fixture and then repeat the manual MVP checklist against `C:\Users\moxhe`.

Open questions:

- Should fixture smoke output be expanded with more app/game examples after real scan feedback?

Risky assumptions:

- The fixture categories are representative enough to exercise the latest review controls.
