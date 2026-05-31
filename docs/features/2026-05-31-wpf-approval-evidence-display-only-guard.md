# Feature: WPF Approval Evidence Display-Only Guard

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add regression coverage that WPF Real-Profile Quarantine Approval Evidence remains display-only and cannot accidentally become the `Quarantine included shortlist` execution gate.

## Non-goals

- Do not change WPF behavior.
- Do not enable real-profile Quarantine execution.
- Do not change fixture-only execution or selected restore.
- Do not scan, move, restore, delete, create, write, or clean up real-profile files.

## Implementation

- Added `WpfRealProfileApprovalEvidenceStaysDisplayOnly` to the core test harness.
- The guard checks that WPF builds approval evidence for Quarantine Execution Gate display, leaves current-build movement availability at the default unavailable value, skips fixture scopes, and keeps `CanApproveForRealProfileMovement` inside display formatting only.
- The guard verifies `ExecuteQuarantineButton.IsEnabled` remains based on `_currentQuarantineExecutionGate.CanExecute`, not approval evidence.
- Broadened the local source-method parser in the test harness to recognize `private static void` methods so source guards can name display-formatting methods accurately.

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added source-level regression coverage around the WPF approval-evidence boundary.
- Updated README, progress, and handoff docs.

ADRs:

- No ADR added. This is regression coverage for accepted ADR 0018 and existing WPF output.

Follow-up work:

- Keep this guard updated if a later explicit real-profile execution packet intentionally changes the WPF execution gate.

Open questions:

- None for this guard packet.

Risky assumptions:

- Source-level guard coverage is appropriate because this boundary is about preventing accidental wiring drift rather than user-facing behavior.
