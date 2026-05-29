# Feature: Real Profile Scan Gate

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make fixture-first verification harder to skip before scanning the real profile.

## Non-goals

- Do not run MVP preflight from the WPF app.
- Do not create fixture files from the WPF app.
- Do not persist acknowledgement state.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not treat acknowledgement as cleanup approval.

## User story / job story

As the project owner, I want the real-profile `Scan` button to require an explicit preflight and fixture-review acknowledgement, so that scanning `C:\Users\moxhe` stays visibly behind the fixture-first workflow.

## Desired behavior

- Default real-profile launch shows the Real Profile Cleanup Scope note.
- Default real-profile launch shows an acknowledgement checkbox.
- Default real-profile launch keeps `Scan` disabled until acknowledgement is checked.
- Fixture Cleanup Scopes do not show the real-profile acknowledgement and remain scan-ready.
- Blank or invalid Cleanup Scopes stay blocked.
- The scan-start method enforces the gate even if invoked directly.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope Scan Gate | Added as the read-only gate for starting Storage Scan. | yes |

## Decisions made

- Keep acknowledgement local and in-memory.
- Reset acknowledgement when the Cleanup Scope changes.
- Keep preflight and fixture creation as external scripts rather than running them from WPF.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `CleanupScopeScanGate` and `CleanupScopeScanGateBuilder`.
- Added a WPF real-profile preflight/fixture-review acknowledgement checkbox.
- Disabled `Scan` for real-profile Cleanup Scopes until acknowledgement.
- Added core and WPF smoke coverage for the gate.

ADRs added or skipped:

- No ADR. This is a reversible local scan-start UI gate and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Manually open the app normally and confirm `Scan` is disabled until acknowledgement.
- Manually launch the fixture review and confirm fixture scopes do not require real-profile acknowledgement.
- Later packet `2026-05-29-scan-gate-discoverability-polish.md` added a visible gate summary line and `Scan` button tooltip while preserving this gate.
