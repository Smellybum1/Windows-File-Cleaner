# Feature: WPF Fixture-Only Quarantine Execution

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Wire the visible WPF `Execute quarantine` path to the core Quarantine Executor for synthetic fixture Cleanup Scopes only.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not add WPF Undo Quarantine.
- Do not permanently delete files.
- Do not clean up quarantine folders.
- Do not add persistent cleanup history.
- Do not bypass Quarantine Preview, confirmation readiness, or exact `QUARANTINE` text.

## User story / job story

As the project owner, I want the visible app to prove it can execute quarantine against a fixture before touching real profile files, so that the final cleanup workflow is safer and easier to trust.

## Current behavior

At packet start, the WPF app could scan, review, shortlist, create Quarantine Preview, show a Quarantine Action Draft, and show a planned write-ahead Restore Manifest. Even with the exact confirmation text, `Execute quarantine` remained disabled because execution was not wired.

## Desired behavior

- For fixture Cleanup Scopes, a clean preview and exact `QUARANTINE` text enables `Execute quarantine`.
- Clicking `Execute quarantine` calls the core `QuarantineExecutor`.
- The app writes the action-scoped Restore Manifest and moves fixture files/folders into quarantine.
- The app shows execution result evidence and marks the current scan/review state as stale.
- For real-profile and non-fixture scopes, WPF execution remains unavailable.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Fixture-only WPF Quarantine Execution | Added as the visible-app execution boundary for synthetic Cleanup Scopes. | yes |
| Quarantine Execution Gate | Extended to allow implemented execution for fixture scopes while blocking real-profile scopes. | yes |

## Open questions

Questions that must be answered before implementation:

- None. ADR 0009 selects fixture-only WPF execution.

Questions that can be deferred:

- What visible WPF Undo Quarantine flow should discover and restore manifests?
- What additional confirmation or backup step should exist before real-profile execution?
- Should a fixture execution clear the current grid or only warn that the scan is stale?

## Grill notes

### Scenarios discussed

- The user has tested real-profile read-only scan.
- Core quarantine and undo are fixture-tested.
- The next visible proof should execute only against synthetic fixture files.

### Edge cases

- Preview has blockers.
- Confirmation text is wrong.
- Cleanup Scope is real profile or custom non-fixture.
- Execution succeeds and the current scan becomes stale.
- Execution fails partially and requires recovery review.

### Dependencies between decisions

- Depends on ADR 0005 write-ahead Restore Manifest.
- Depends on ADR 0006 temp-replace Restore Manifest writes.
- Depends on ADR 0007 fixture-first Quarantine Executor.
- Depends on ADR 0008 fixture-first Undo Quarantine.
- Adds ADR 0009 fixture-only WPF execution.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: WPF execution gate, confirmation draft, action draft, Restore Manifest builder, Quarantine Executor, WPF smoke tests, README/manual checklist, progress log.
- Tests/checks planned: core gate test for implemented execution, WPF fixture execution smoke test, real/custom scope execution remains blocked through gate wording, build, both test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not enable real-profile WPF execution in this packet.
- Do not implement movement directly in WPF.
- Do not leave the gate enabled after execution.
- Do not imply WPF Undo Quarantine exists after fixture execution.

## Decisions made

Small feature-level decisions:

- Pass fixture-only execution availability into Quarantine Confirmation Draft.
- Reuse `QuarantineExecutor.Execute` from WPF.
- Disable the execution gate after an execution attempt and warn that scan state is stale.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0009-use-fixture-only-wpf-quarantine-execution.md`

## Implementation plan

1. Add ADR 0009 and this feature brief.
2. Extend confirmation/gate model to support fixture-only execution availability.
3. Wire WPF execution to `QuarantineExecutor` only when the gate opens.
4. Update WPF smoke tests for fixture execution and blocked non-fixture execution.
5. Update docs/progress.
6. Run full preflight, commit, push, and verify CI.

## Files expected to change

Expected:

- `docs/decisions/0009-use-fixture-only-wpf-quarantine-execution.md`
- `docs/features/2026-05-29-wpf-fixture-only-quarantine-execution.md`
- `src/WindowsFileCleaner.Core/QuarantineConfirmationDraft.cs`
- `src/WindowsFileCleaner.Core/QuarantineConfirmationDraftBuilder.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionGateBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Possible:

- MVP readiness audit and prior feature follow-up wording.

## Test plan

Manual checks:

- Run fixture review and confirm `Execute quarantine` enables only after fixture preview plus exact confirmation.
- Confirm real-profile app still keeps execution unavailable.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- Fixture execution changes the previous "visible app is read-only" invariant.
- Current scan rows become stale after execution.
- WPF Undo Quarantine remains unavailable after this packet.

Assumptions:

- Fixture-only visible execution is the right intermediate step before any real-profile movement.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added ADR 0009 for fixture-only WPF Quarantine execution.
- Extended `QuarantineConfirmationDraft` so execution availability can be true for fixture scopes and false elsewhere.
- Updated the WPF execution gate so fixture preview plus exact `QUARANTINE` can enable `Execute quarantine`.
- Wired WPF fixture execution to `QuarantineExecutor.Execute`.
- Added post-execution result/stale-state wording and disabled re-execution for the current preview.
- Added WPF smoke coverage proving fixture execution moves a synthetic file and writes a Restore Manifest.
- Added WPF smoke coverage proving custom non-fixture execution remains blocked.

Files changed:

- `docs/decisions/0009-use-fixture-only-wpf-quarantine-execution.md`
- `docs/features/2026-05-29-wpf-fixture-only-quarantine-execution.md`
- `src/WindowsFileCleaner.Core/QuarantineConfirmationDraft.cs`
- `src/WindowsFileCleaner.Core/QuarantineConfirmationDraftBuilder.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionGateBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

Docs updated:

- ADR 0009, domain context, glossary, README, MVP readiness audit, this feature brief, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0009-use-fixture-only-wpf-quarantine-execution.md`.

Follow-up work:

- Run the manual fixture review and inspect fixture execution wording/layout.
- Add WPF Undo Quarantine with manifest discovery and stale-state checks.
- Design real-profile WPF Quarantine execution only after fixture execution remains green and the user reviews the visible behavior.

Open questions:

- What visible WPF Undo Quarantine flow should discover and restore manifests?
- What additional confirmation or backup step should exist before real-profile execution?
- Should a fixture execution clear the grid entirely or is stale-state wording enough?

Risky assumptions:

- Fixture-only visible execution is the right intermediate step before any real-profile movement.
