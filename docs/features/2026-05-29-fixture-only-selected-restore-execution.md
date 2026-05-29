# Feature: Fixture-Only Selected Restore Execution

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add fixture-only WPF selected restore execution for discovered Restore Manifests, while keeping real-profile and custom non-fixture selected restore unavailable.

## Non-goals

- Do not enable real-profile WPF Undo Quarantine.
- Do not enable custom non-fixture selected restore execution.
- Do not enable real-profile WPF Quarantine execution.
- Do not add permanent deletion.
- Do not add persisted cleanup history.
- Do not clean up empty quarantine action folders.
- Do not implement restore movement in WPF.

## User story / job story

As the project owner, I want the selected restore flow to restore a synthetic fixture manifest end to end before touching real profile files, so that the broad undo path has visible fixture proof.

## Current behavior

The WPF app can select a discovered Restore Manifest, preview selected readiness, and preview a Selected Restore Execution Gate. The selected restore gate can show `RESTORE` matches, but execution is always unavailable.

## Desired behavior

- WPF selected restore execution is available only when the selected Restore Manifest's Cleanup Scope is a recognized fixture Cleanup Scope.
- The selected restore gate opens only after clean readiness and exact `RESTORE`.
- Clicking `Restore selected fixture manifest` calls `UndoQuarantineExecutor.Undo` for the selected discovered Restore Manifest.
- The result pane shows restored/failed rows and stale-state guidance.
- Real-profile and custom non-fixture selected restore execution stay unavailable.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Fixture-only Selected Restore Execution | Add as visible WPF restore execution for selected discovered fixture Restore Manifests. | yes |

## Open questions

Questions that must be answered before implementation:

- None. ADR 0015 selects fixture-only selected restore execution before real-profile restore.

Questions that can be deferred:

- Should successful selected restore offer empty action-folder cleanup?
- What extra backup/manual review should real-profile selected restore require?
- Should selected restore refresh discovery automatically after execution or require the user to rediscover?

## Grill notes

### Scenarios discussed

- Current fixture undo restores only the current in-memory fixture execution.
- Discovered older manifests now have selection, readiness, and confirmation gate semantics.
- The next safe step is selected old-manifest restore execution for fixtures only.

### Edge cases

- Selected manifest cleanup scope is real-profile or custom non-fixture.
- Selected readiness has blockers.
- `RESTORE` is missing or wrong.
- The original path appears after readiness but before execution.
- The selected restore attempt already ran for the current manifest review.

### Dependencies between decisions

- Depends on ADR 0010 WPF Current Fixture Undo Quarantine.
- Depends on ADR 0011 read-only Quarantine Manifest Discovery.
- Depends on ADR 0012 read-only Restore Readiness Preview.
- Depends on ADR 0013 read-only Selected Restore Manifest Review.
- Depends on ADR 0014 read-only Selected Restore Confirmation Gate.
- Adds ADR 0015 fixture-only selected restore execution.

## Evidence and validation gate

Evidence gathered:

- User answers: the project owner wants quarantine on `D:` and easy undo, but does not want current apps or user data broken.
- Existing code/docs inspected: `UndoQuarantineExecutor`, WPF current-fixture undo, Selected Restore Confirmation Gate, Quarantine Manifest Discovery, WPF smoke tests, source-level filesystem write guard, README, domain docs, progress log.
- Tests/checks planned: WPF smoke coverage restoring an older discovered fixture manifest; custom-scope coverage proving selected restore stays blocked; core/source guard coverage; build, both test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not enable real-profile selected restore in this packet.
- Do not implement restore movement in WPF.
- Do not clean up action folders after restore.
- Do not let selected restore run more than once for the same current selected review.

## Decisions made

Small feature-level decisions:

- Use the button label `Restore selected fixture manifest`.
- Use `UndoQuarantineExecutor.Undo` for movement and manifest updates.
- Keep selected restore execution unavailable unless the selected manifest Cleanup Scope is recognized as a fixture scope.
- Disable repeat selected restore execution after an attempt.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0015-use-fixture-only-selected-restore-execution.md`

## Implementation plan

1. Add ADR 0015 and this feature brief.
2. Add WPF selected restore execution state, availability, button, and result formatting.
3. Wire selected restore gate availability from selected manifest fixture status.
4. Extend WPF smoke tests for selected fixture restore and custom-scope blocker.
5. Update README, domain docs, glossary, audit, and progress.
6. Run verification, commit, push, and verify CI.

## Files expected to change

Expected:

- `docs/decisions/0015-use-fixture-only-selected-restore-execution.md`
- `docs/features/2026-05-29-fixture-only-selected-restore-execution.md`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

## Test plan

Manual checks:

- Execute fixture quarantine, open a new WPF instance with the same fixture and Quarantine Root, discover/select the manifest, preview selected readiness, preview selected restore gate, type `RESTORE`, click `Restore selected fixture manifest`, and confirm the synthetic file is restored.
- Repeat against a custom non-fixture scope and confirm selected restore execution stays unavailable.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- Fixture-only selected restore could be mistaken for real-profile restore readiness if wording is vague.
- Filesystem state can change between readiness preview and execution.

Assumptions:

- `CleanupScopeSafetyNoteBuilder.IsFixtureScope` remains the authority for fixture-only WPF execution.
- `UndoQuarantineExecutor` should remain the only component that moves quarantined files back.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added ADR 0015 for fixture-only selected restore execution.
- Added WPF `Restore selected fixture manifest` action.
- Wired selected restore execution to `UndoQuarantineExecutor.Undo` for selected discovered fixture Restore Manifests only.
- Kept real-profile and custom non-fixture selected restore execution blocked even when `RESTORE` is typed.
- Added WPF smoke coverage for selected fixture restore and custom non-fixture selected restore blocking.

Files changed:

- `docs/decisions/0015-use-fixture-only-selected-restore-execution.md`
- `docs/features/2026-05-29-fixture-only-selected-restore-execution.md`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- Final build, no-build test harnesses, MVP preflight, and `git diff --check` are recorded in `.codex/progress.md`.

Docs updated:

- ADR 0015, README, domain context, glossary, MVP readiness audit, this feature brief, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0015-use-fixture-only-selected-restore-execution.md`.

Follow-up work:

- Add real-profile selected restore design only after manual fixture review.
- Decide final stale-state wording and rescan behavior after selected restore.
- Decide whether successful selected restore should offer empty action-folder cleanup.

Open questions:

- Should successful selected restore offer empty action-folder cleanup?
- What extra backup/manual review should real-profile selected restore require?
- Should selected restore refresh discovery automatically after execution or require the user to rediscover?

Risky assumptions:

- `CleanupScopeSafetyNoteBuilder.IsFixtureScope` remains the authority for fixture-only WPF execution.
- `UndoQuarantineExecutor` should remain the only component that moves quarantined files back.
