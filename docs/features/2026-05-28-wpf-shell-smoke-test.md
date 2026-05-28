# Feature: WPF Shell Smoke Test

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Add an automated Windows-only smoke test for the WPF shell so launch-scope wiring is verified before manual scans.

## Non-goals

- Do not automate the full WPF interaction flow.
- Do not launch the visible desktop app.
- Do not scan `C:\Users\moxhe`.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing.

## User story / job story

As the project owner, I want the WPF shell to have a small automated smoke check, so that basic desktop-app startup wiring is verified before I run manual Storage Scans.

## Current behavior

Core fixture tests verify scanner, classification, review, preview, draft, export, and safety behavior. The WPF app can launch with `--scope`, but no automated test constructs the WPF shell to prove the launch Cleanup Scope reaches the UI.

## Desired behavior

A Windows-only test project constructs `MainWindow` on an STA thread and verifies:

- Default construction uses `C:\Users\moxhe`.
- Launch-scope construction uses the provided synthetic Cleanup Scope.
- Construction does not start a scan.
- CSV export remains disabled before a scan result exists.
- A user-triggered Storage Scan remains available.

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

- Should a future Playwright/WinAppDriver-style UI test automate filter clicks, shortlist changes, and Quarantine Preview interactions?

## Evidence and validation gate

Evidence gathered:

- MVP readiness audit still records latest WPF interaction retest as pending.
- WPF fixture launch support exists but only had parser-level coverage.
- `MainWindow` already separates construction from user-triggered scan behavior.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not use this smoke test as proof that the full WPF interaction flow is manually acceptable.
- Do not add a visible GUI automation dependency before the shell-level gap is closed.
- Do not expose production cleanup actions for test convenience.

## Decisions made

Small feature-level decisions:

- Add a separate `WindowsFileCleaner.App.Tests` project targeting `net8.0-windows` with `UseWPF`.
- Keep WPF smoke assertions limited to startup state and launch-scope propagation.
- Expose small read-only `MainWindow` properties for smoke-test assertions.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add public read-only `MainWindow` properties for current startup state.
2. Add `tests/WindowsFileCleaner.App.Tests`.
3. Construct `MainWindow` on an STA thread in the smoke test.
4. Add the project to `WindowsFileCleaner.sln`.
5. Update README, AGENTS, MVP readiness audit, and progress log.

## Files expected to change

Expected:

- `WindowsFileCleaner.sln`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/WindowsFileCleaner.App.Tests.csproj`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

Possible:

- None

## Test plan

Manual checks:

- Still run the WPF fixture smoke flow and real-profile retest by hand.

Automated tests:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- This smoke test proves construction and initial state only; it does not prove the full UI interaction flow or layout quality.

Assumptions:

- Constructing `MainWindow` without showing it is enough to catch broken startup/scope wiring.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added read-only `MainWindow` startup-state properties.
- Added a Windows-only WPF app smoke test project.
- Added the smoke test project to the solution.
- Updated docs and verification commands.

Files changed:

- `WindowsFileCleaner.sln`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/WindowsFileCleaner.App.Tests.csproj`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-shell-smoke-test.md`
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
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is a testability improvement and does not change architecture, persistence, security posture, or cleanup behavior.

Follow-up work:

- Use the fixture launch flow to manually smoke test the latest WPF controls.
- Then rerun against `C:\Users\moxhe`.

Open questions:

- Should future UI automation cover actual button/filter interactions after the manual retest?

Risky assumptions:

- Startup-state smoke coverage is useful enough without introducing a visible GUI automation dependency.
