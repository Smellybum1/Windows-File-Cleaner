# Feature: Scope-Specific Scan Gate Ready Wording

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make Cleanup Scope Scan Gate ready-state wording honest about later cleanup execution availability for fixture, real-profile, and custom scopes.

## Non-goals

- Do not change when `Scan` is enabled.
- Do not run MVP preflight from the WPF app.
- Do not create fixtures from the WPF app.
- Do not persist acknowledgement state.
- Do not scan or modify real-profile files.
- Do not enable real-profile or custom cleanup execution.

## User story / job story

As the project owner, I want scan-gate ready wording to match the selected Cleanup Scope, so that fixture readiness does not obscure the fact that real-profile and custom cleanup execution remain unavailable.

## Current behavior

The scan gate correctly enables fixture and custom scans and requires acknowledgement for real-profile scans. Its ready message is generic for non-real scopes, saying no cleanup execution is enabled even though fixture-only cleanup actions exist later behind preview and exact confirmation gates.

## Desired behavior

- Real-profile ready wording says Storage Scan is read-only and real-profile cleanup execution remains unavailable.
- Fixture ready wording says Storage Scan is read-only first and fixture cleanup actions still require preview and exact confirmation.
- Custom ready wording says Storage Scan is read-only and real-profile/custom cleanup execution remains unavailable.
- Scan enablement behavior stays unchanged.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope Scan Gate | Clarified that ready wording should be scope-specific about later cleanup execution availability. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is scope-specific wording on an existing gate.

Questions that can be deferred:

- Does the longer header wording fit comfortably during visible fixture and real-profile review?

## Grill notes

### Scenarios discussed

- Fixture scope: Scan is ready, but later fixture cleanup actions still require Quarantine Preview and exact confirmation.
- Real-profile scope: Scan is ready only after acknowledgement, and cleanup execution remains unavailable.
- Custom scope: Scan can start as read-only review, and cleanup execution remains unavailable.

### Edge cases

- Blank or invalid Cleanup Scope stays disabled.
- Locked real-profile wording stays unchanged.

### Dependencies between decisions

- Depends on existing Cleanup Scope Scan Gate semantics.
- Does not affect Quarantine Preview, Quarantine Execution Gate, restore, deletion, or cleanup history.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: `CleanupScopeScanGateBuilder`, WPF scan-gate summary/tooltip formatting, scan-gate tests, README, domain docs, handoff.
- Handoff guidance prefers scan-gate discoverability before any real cleanup execution.
- Tests/checks planned: core tests, WPF smoke tests, build, checklist-only output, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make fixture scan readiness imply cleanup approval.
- Do not hide fixture-only cleanup execution by saying no cleanup execution exists for fixture scopes.

## Decisions made

Small feature-level decisions:

- Keep locked real-profile wording unchanged.
- Make ready summaries and Scan tooltips scope-specific.
- Keep the scan-start gate behavior unchanged.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update core scan-gate ready messages.
2. Update WPF scan-gate summary and tooltip wording.
3. Add core and WPF smoke assertions for real-profile, fixture, and custom ready states.
4. Update README, domain docs, feature notes, progress, handoff, and fixture checklist.
5. Run core tests, WPF smoke tests, build, checklist-only output, and diff check.

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During visible fixture review, confirm the longer scan-gate summary fits the header.

## Risks and assumptions

Risks:

- The scope-specific wording is longer and may need later visual tightening.

Assumptions:

- More precise ready-state wording is worth the extra text because it reinforces the cleanup execution boundary.

## Completion notes

Completed on: 2026-05-29

What changed:

- Updated core scan-gate ready messages for acknowledged real-profile, fixture, and custom scopes.
- Updated WPF scan-gate summaries and Scan tooltips with matching scope-specific execution-boundary wording.
- Added core and WPF smoke assertions for the new ready states.
- Updated fixture checklist wording to include gated fixture cleanup actions.

Files changed:

- `src/WindowsFileCleaner.Core/CleanupScopeScanGateBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- README, domain docs, progress log, handoff, and related feature notes.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, scan-gate feature note, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible wording clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Check during visible fixture/real-profile review whether the longer header wording fits comfortably.

Open questions:

- Does the longer header wording need to be shortened after visual review?

Risky assumptions:

- Scope-specific wording improves safety clarity more than it increases header density.
