# Feature: Selected Restore Confirmation Gate

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add a read-only confirmation draft and execution gate for a selected Restore Manifest before any selected old-manifest restore execution exists.

## Non-goals

- Do not restore discovered manifests.
- Do not call `UndoQuarantineExecutor.Undo`.
- Do not enable real-profile WPF Quarantine execution.
- Do not enable broad WPF Undo Quarantine.
- Do not write manifests.
- Do not create, move, delete, or clean up files or folders.
- Do not add persisted cleanup history.
- Do not expose a selected restore execution button.

## User story / job story

As the project owner, I want the selected restore flow to show the exact confirmation and blockers before execution exists, so that future undo work has a clear safety gate.

## Current behavior

The WPF app can discover Restore Manifests, select one, and preview readiness for that selected manifest. It does not yet show a selected restore confirmation phrase or execution gate.

## Desired behavior

- After `Preview selected readiness`, WPF can build a Selected Restore Confirmation Draft.
- The draft shows selected manifest path, restorable entries, restorable bytes, readiness blockers, and required confirmation text `RESTORE`.
- The Selected Restore Execution Gate shows whether the typed confirmation matches.
- WPF keeps selected restore execution unavailable even when `RESTORE` is typed.
- No restore button is exposed and no files are modified.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Restore Confirmation Draft | Add as read-only confirmation readiness for one selected Restore Manifest. | yes |
| Selected Restore Execution Gate | Add as read-only gate combining selected restore confirmation with typed text and execution availability. | yes |

## Open questions

Questions that must be answered before implementation:

- None. ADR 0014 selects a read-only selected restore confirmation gate before restore execution.

Questions that can be deferred:

- Should future selected restore execution require zero blocked, recovery-review, and not-moved readiness rows?
- Should successful selected restore offer empty action-folder cleanup?
- What stale-state wording should appear when readiness changes between preview and execution?

## Grill notes

### Scenarios discussed

- The project owner wants quarantine on `D:` and an easy undo path.
- Current WPF can undo only the current fixture execution.
- Discovery, selected review, and readiness preview are read-only for older Restore Manifests.
- The next safest step is an exact confirmation gate that still cannot execute.

### Edge cases

- Selected readiness has not been previewed.
- Selected readiness has zero restorable entries.
- Selected readiness has blocked, recovery-review, or not-moved entries.
- Typed confirmation is blank or wrong.
- Typed confirmation is exactly `RESTORE` while execution remains unavailable.

### Dependencies between decisions

- Depends on ADR 0011 read-only Quarantine Manifest Discovery.
- Depends on ADR 0012 read-only Restore Readiness Preview.
- Depends on ADR 0013 read-only Selected Restore Manifest Review.
- Adds ADR 0014 read-only Selected Restore Confirmation Gate.
- Precedes fixture-first selected restore execution.

## Evidence and validation gate

Evidence gathered:

- User answers: the project owner wants reversible cleanup through quarantine and undo, but does not want current apps or user data broken.
- Existing code/docs inspected: Quarantine Confirmation Draft, Quarantine Execution Gate, Selected Restore Manifest Review, Restore Readiness Preview, Undo Quarantine Executor, WPF gate UI/tests, README, domain docs, progress log.
- Tests/checks planned: core selected restore confirmation/gate tests; WPF smoke coverage proving exact text still does not restore; build, both test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not restore selected old manifests in this packet.
- Do not reuse the Quarantine confirmation phrase for restore.
- Do not expose a restore execution button.
- Do not treat typed `RESTORE` as approval while execution is unavailable.

## Decisions made

Small feature-level decisions:

- Use `RESTORE` as the selected restore confirmation phrase.
- Keep WPF selected restore execution unavailable in this packet.
- Let the core gate model support future implementation availability, but pass unavailable from WPF.
- Block confirmation readiness when selected readiness has blocked, recovery-review, or not-moved rows.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0014-use-read-only-selected-restore-confirmation-gate.md`

## Implementation plan

1. Add ADR 0014 and this feature brief.
2. Add Selected Restore Confirmation Draft and Selected Restore Execution Gate models/builders.
3. Add core tests for clean selected readiness, readiness blockers, and exact confirmation semantics.
4. Add WPF selected restore gate controls and status-only formatting.
5. Extend WPF smoke coverage to prove typing `RESTORE` still does not restore old-manifest files.
6. Update README, domain docs, glossary, audit, and progress.
7. Run verification, commit, push, and verify CI.

## Files expected to change

Expected:

- `docs/decisions/0014-use-read-only-selected-restore-confirmation-gate.md`
- `docs/features/2026-05-29-selected-restore-confirmation-gate.md`
- `src/WindowsFileCleaner.Core/SelectedRestoreConfirmationDraft*.cs`
- `src/WindowsFileCleaner.Core/SelectedRestoreExecutionGate*.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

## Test plan

Manual checks:

- Run fixture execute without undo, restart WPF with the same Quarantine Root, discover manifests, select the discovered manifest, preview selected readiness, preview selected restore gate, type `RESTORE`, and confirm no restore action is exposed and files are not restored.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- Confirmation wording could be mistaken for available execution if the UI is vague.
- The selected readiness result can become stale if files change after preview.

Assumptions:

- `RESTORE` is a clear enough confirmation phrase for selected restore.
- Future restore execution will recompute readiness immediately before moving files.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added ADR 0014 for a read-only Selected Restore Confirmation Gate.
- Added core Selected Restore Confirmation Draft and Selected Restore Execution Gate models/builders.
- Added WPF `Preview selected restore gate`, selected restore confirmation text entry, and status-only gate output.
- Kept WPF selected restore execution unavailable even when `RESTORE` is typed.
- Added core and WPF coverage proving selected restore confirmation/gate behavior does not restore files.

Files changed:

- `docs/decisions/0014-use-read-only-selected-restore-confirmation-gate.md`
- `docs/features/2026-05-29-selected-restore-confirmation-gate.md`
- `src/WindowsFileCleaner.Core/SelectedRestoreConfirmationDraft.cs`
- `src/WindowsFileCleaner.Core/SelectedRestoreConfirmationDraftBuilder.cs`
- `src/WindowsFileCleaner.Core/SelectedRestoreExecutionGate.cs`
- `src/WindowsFileCleaner.Core/SelectedRestoreExecutionGateBuilder.cs`
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

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- Final build, no-build test harnesses, MVP preflight, and `git diff --check` are recorded in `.codex/progress.md`.

Docs updated:

- ADR 0014, README, domain context, glossary, MVP readiness audit, this feature brief, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0014-use-read-only-selected-restore-confirmation-gate.md`.

Follow-up work:

- Add fixture-first selected restore execution after the gate is manually reviewed.
- Recompute selected readiness immediately before any future restore movement.
- Decide whether successful selected restore should offer empty action-folder cleanup.

Open questions:

- Should future selected restore execution require zero blocked, recovery-review, and not-moved readiness rows?
- Should successful selected restore offer empty action-folder cleanup?
- What stale-state wording should appear when readiness changes between preview and execution?

Risky assumptions:

- `RESTORE` is a clear enough confirmation phrase for selected restore.
- Future restore execution will recompute readiness immediately before moving files.
