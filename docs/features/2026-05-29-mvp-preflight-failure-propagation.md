# Feature: MVP Preflight Failure Propagation

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make `tools/Invoke-MvpPreflight.ps1` fail when any native child command fails.

## Non-goals

- Do not change the set of preflight checks.
- Do not scan or modify real user files.
- Do not launch the WPF app from preflight.
- Do not add cleanup execution behavior.

## User story / job story

As the project owner, I want MVP preflight to stop on failed build/test commands, so that a false green check cannot bless unsafe or broken work.

## Current behavior

During Quarantine Manifest Discovery work, a WPF app test failed while `Invoke-MvpPreflight.ps1` still printed `MVP preflight passed`. PowerShell did not throw for the non-zero native `dotnet` exit code.

## Desired behavior

- Each preflight step resets `$LASTEXITCODE` before running.
- Each preflight step checks `$LASTEXITCODE` after running.
- A non-zero native command exit code throws and prevents the success message.
- A regression check verifies that the script keeps native exit-code handling.

## Domain language changes

No new domain terms.

## Open questions

Questions that must be answered before implementation:

- None. This is a verification reliability fix.

Questions that can be deferred:

- Should the preflight script also emit structured machine-readable output later?

## Evidence and validation gate

Evidence gathered:

- Observed `Invoke-MvpPreflight.ps1` print success after a failing WPF app test.
- Inspected `tools/Invoke-MvpPreflight.ps1` and found it invoked native commands without checking `$LASTEXITCODE`.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not duplicate the local preflight command list elsewhere.
- Do not add a fake failure mode to the production preflight script.

## Decisions made

Small feature-level decisions:

- Keep one shared `Invoke-PreflightStep` wrapper and make it responsible for native exit-code handling.
- Add a source-level regression check in the existing console test harness.

ADR-worthy decisions:

- [x] None. This is a local verification script reliability fix.

## Implementation plan

1. Patch `Invoke-PreflightStep` to reset and check `$global:LASTEXITCODE`.
2. Add a source-level regression check for native exit-code handling.
3. Update README and progress evidence.
4. Run the full MVP preflight.
5. Commit, push, and verify CI.

## Files expected to change

Expected:

- `tools/Invoke-MvpPreflight.ps1`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `README.md`
- `.codex/progress.md`
- `docs/features/2026-05-29-mvp-preflight-failure-propagation.md`

## Test plan

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- A future PowerShell-only step might need separate exception handling, but existing steps are native commands or nested PowerShell launched as a native command.

Assumptions:

- Checking `$global:LASTEXITCODE` in the shared wrapper covers the current restore, build, test, fixture dry-run, and diff-check steps.

## Completion notes

Completed on: 2026-05-29

What changed:

- `Invoke-PreflightStep` now resets and checks `$global:LASTEXITCODE`.
- A non-zero native command exit code now throws before the success message.
- Added a source-level regression check for the preflight failure-propagation behavior.

Files changed:

- `tools/Invoke-MvpPreflight.ps1`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `README.md`
- `.codex/progress.md`
- `docs/features/2026-05-29-mvp-preflight-failure-propagation.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

Docs updated:

- README, this feature brief, and progress log.

ADRs added or skipped:

- Skipped ADR. This is a reversible verification script fix.

Follow-up work:

- Consider a runtime negative test harness for preflight if script complexity grows.

Open questions:

- Should the preflight script also emit structured machine-readable output later?

Risky assumptions:

- Existing and near-term preflight steps remain native commands or nested PowerShell launched as a native command.
