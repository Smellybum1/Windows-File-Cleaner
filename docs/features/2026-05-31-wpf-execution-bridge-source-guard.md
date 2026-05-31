# Feature: WPF Execution Bridge Source Guard

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add regression coverage that WPF movement executor calls stay isolated to the known gated execution bridge methods.

This advances safe full-app readiness by making accidental UI wiring around the fixture-only boundary harder to introduce while real-profile Quarantine and real-profile selected restore remain unavailable.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile selected restore execution.
- Do not change fixture execution behavior.
- Do not move, restore, delete, create, or rewrite real-profile files.

## Current behavior

The core test harness now scans `MainWindow.xaml.cs` and verifies:

- WPF calls `QuarantineExecutor.Execute` and `UndoQuarantineExecutor.Undo` only from the three known execution bridge methods:
  - `ExecuteQuarantineForCurrentPreview`
  - `ExecuteSelectedRestoreForCurrentSelection`
  - `UndoQuarantineForCurrentExecution`
- forward Quarantine remains behind the current execution gate and Restore Manifest guard,
- selected restore remains behind the selected restore gate and current discovery lookup,
- current-fixture undo remains behind `CanUndoCurrentQuarantineExecution`,
- WPF forward and selected restore execution availability remain fixture-scope based.

## Decisions made

- Keep this as a source-level regression guard beside the existing production filesystem-write guards.
- Guard call placement and the local gate/fixture checks, while leaving the existing WPF smoke tests responsible for user-facing button behavior and wording.

## Files changed

- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/features/2026-05-31-wpf-execution-bridge-source-guard.md`
- `README.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added `WpfExecutionBridgeKeepsExecutorCallsInGatedMethods`.
- The guard checks executor call count, enclosing methods, and fixture/gate guard text in the WPF execution bridge.

ADRs:

- No ADR added. This is regression coverage for existing fixture-only and real-profile-unavailable decisions in ADR 0017, ADR 0018, and ADR 0019.

Follow-up work:

- Keep this guard updated if a later user-approved real-profile execution packet intentionally adds new WPF executor calls.

Open questions:

- None for this guard packet.

Risky assumptions:

- Source-level checks are acceptable here because the guarded WPF methods are narrow execution boundary code.
