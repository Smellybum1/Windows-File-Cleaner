# Feature: Selected Restore Pre-Execution Revalidation

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add the non-moving core revalidation model required before a future real-profile selected Restore Manifest can be restored.

This packet advances ADR 0019 without enabling real-profile restore, real-profile Quarantine, all-manifest restore, permanent deletion, or cleanup history.

## Non-goals

- Do not enable real-profile selected restore execution.
- Do not call `UndoQuarantineExecutor`.
- Do not enable real-profile Quarantine execution.
- Do not enable custom non-fixture selected restore.
- Do not enable all-manifest restore.
- Do not move, restore, delete, create, or rewrite real-profile files.

## Current behavior

Core code now has `SelectedRestorePreExecutionRevalidation`, a read-only check that:

- rediscovers the selected Restore Manifest from the selected Quarantine Root,
- rebuilds selected-manifest readiness immediately before future selected restore movement,
- checks exact `C:\Users\moxhe` Cleanup Scope,
- checks selected restore confirmation and gate evidence,
- blocks missing quarantine paths, original-path collisions, recovery-review states, not-moved states, all-restored states, stale selected manifest evidence, non-real-profile scopes, and unavailable implementation,
- reports path-specific entry blockers for future WPF output,
- does not create folders, move files, restore files, delete files, write manifests, or approve cleanup.

## Decisions made

- Revalidation is selected-manifest-only and real-profile-only for the future first phase.
- Revalidation rediscovers the selected manifest instead of trusting the earlier selected review snapshot.
- Clean revalidation can only report `CanProceed` when the caller explicitly records that selected-manifest real-profile Undo is implemented.
- This model is core evidence only; WPF remains blocked until a later explicit implementation packet wires execution.

## Files changed

- `src/WindowsFileCleaner.Core/SelectedRestorePreExecutionRevalidation.cs`
- `src/WindowsFileCleaner.Core/SelectedRestorePreExecutionRevalidationBuilder.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0019-use-real-profile-selected-restore-execution-contract.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added the read-only Selected Restore Pre-Execution Revalidation model and builder.
- Added tests for clean synthetic real-profile selected-manifest evidence, stale missing quarantine paths, and unavailable/non-real-profile blockers.
- Kept all restore and cleanup movement unavailable for real-profile scopes.

Follow-up work:

- A later explicit WPF packet can show this evidence in the selected restore gate.
- A later explicit execution packet can rerun this model immediately before calling `UndoQuarantineExecutor`.

Open questions:

- None for this model packet.

Risky assumptions:

- The first real-profile selected restore implementation can keep custom non-fixture and all-manifest restore unavailable.
