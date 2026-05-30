# Feature: Real-Profile Restore Readiness

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add the non-moving core Real-Profile Restore Readiness check from ADR 0018.

This packet turns the selected-manifest real-profile Undo prerequisite into named core evidence that future forward real-profile Quarantine readiness can consume. It does not restore files, write manifests, create folders, delete files, enable selected real-profile restore, or enable real-profile Quarantine execution.

## Non-goals

- Do not wire Real-Profile Restore Readiness into WPF controls yet.
- Do not enable real-profile selected restore execution.
- Do not enable real-profile Quarantine execution.
- Do not enable all-manifest restore.
- Do not add permanent deletion or persisted cleanup history.
- Do not modify real-profile files.

## Current behavior

Before this packet, `QuarantineExecutionReadiness` carried a simple selected-manifest Undo prerequisite but did not consume a named restore-readiness evidence model.

## Desired behavior

Core code has a read-only `RealProfileRestoreReadiness` model that checks selected-manifest restore evidence:

- Selected Restore Manifest Review exists and targets the exact real-profile Cleanup Scope `C:\Users\moxhe`.
- Selected manifest readiness has restorable entries and no blocked, recovery-review, not-moved, or manifest-level blockers.
- Selected Restore Confirmation Draft matches the selected manifest review.
- Selected Restore Execution Gate has exact `RESTORE` confirmation evidence.
- The caller explicitly records whether selected-manifest real-profile Undo is implemented.
- Restore Manifest remains the only durable cleanup record.

`QuarantineExecutionReadiness` can consume this result. If WPF or a future caller does not provide Real-Profile Restore Readiness yet, real-profile readiness still reports that selected-manifest Undo readiness has not been checked.

## Decisions made

- Real-Profile Restore Readiness is selected-manifest-only.
- The model consumes existing selected restore review, confirmation draft, and gate evidence instead of rediscovering manifests or reading files.
- Clean readiness does not enable movement in this build; WPF selected real-profile restore remains unavailable.
- All-manifest restore, action-folder cleanup, and persisted cleanup history remain out of scope.

## Files changed

- `src/WindowsFileCleaner.Core/RealProfileRestoreReadiness.cs`
- `src/WindowsFileCleaner.Core/RealProfileRestoreReadinessBuilder.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionReadinessBuilder.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/features/2026-05-31-real-profile-restore-readiness.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added a read-only Real-Profile Restore Readiness model and builder.
- Added tests for unavailable implementation, clean implemented evidence, fixture-scope blocking, selected readiness blockers, and `QuarantineExecutionReadiness` consumption.
- Kept WPF execution behavior unchanged.

Follow-up work:

- WPF readiness output was added in the next display-only packet; it groups root safety, pre-execution revalidation, and real-profile restore readiness blockers while keeping execution disabled.
- Real-profile selected restore execution design was recorded in ADR 0019; implementation remains unavailable until a later explicit safety packet.

Open questions:

- None for this packet.

Risky assumptions:

- A selected-manifest-only restore path is enough recovery readiness for the first real-profile Quarantine phase; all-manifest restore remains deferred.
