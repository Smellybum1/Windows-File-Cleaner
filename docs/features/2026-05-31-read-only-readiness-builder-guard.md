# Feature: Read-Only Readiness Builder Guard

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add regression coverage that the read-only readiness and revalidation builders stay separate from cleanup execution components.

This advances full-app readiness by making the current safety boundary harder to cross accidentally while real-profile Quarantine execution and real-profile selected restore remain unavailable.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile selected restore execution.
- Do not change WPF button availability.
- Do not move, restore, delete, create, or rewrite real-profile files.

## Current behavior

The core test suite now includes a source-level guard for read-only readiness builders. It scans the builders for calls to execution components, manifest writes, and direct filesystem movement/write tokens.

Covered builders:

- `QuarantineExecutionReadinessBuilder`
- `QuarantineRootExecutionSafetyBuilder`
- `PreExecutionRevalidationBuilder`
- `RealProfileRestoreReadinessBuilder`
- `SelectedRestorePreExecutionRevalidationBuilder`
- `RestoreReadinessPreviewBuilder`
- `SelectedRestoreManifestReviewBuilder`

The existing broader production guard still limits raw filesystem write calls to report exports, Restore Manifest File Store, Quarantine Executor, and Undo Quarantine Executor.

## Decisions made

- Keep this as source-level regression coverage rather than a new runtime abstraction.
- Guard the readiness builders against calls to `QuarantineExecutor.Execute`, `UndoQuarantineExecutor.Undo`, and `RestoreManifestFileStore.Write`.
- Guard the same builders against direct folder creation, folder/file movement, deletion, and `File.WriteAllText`.

## Files changed

- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/features/2026-05-31-read-only-readiness-builder-guard.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added `ReadOnlyReadinessBuildersDoNotCallExecutionComponents`.
- The guard prevents read-only readiness/revalidation builders from directly invoking movement executors, writing Restore Manifests, or performing direct filesystem movement/write operations.

ADRs:

- No ADR added. This is regression coverage for ADR 0017, ADR 0018, and ADR 0019 safety boundaries, not a new durable decision.

Follow-up work:

- Keep future readiness builders in this guard until an explicit execution packet separates read-only evidence from movement.

Open questions:

- None for this guard packet.

Risky assumptions:

- Source-level guard coverage is appropriate here because these builders are deliberately read-only boundary code.
