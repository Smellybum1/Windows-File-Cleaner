# Feature: Quarantine Root Execution Safety

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add the non-moving core Quarantine Root Execution Safety check from ADR 0018.

This packet validates whether an action-scoped Quarantine Root looks safe for future execution, but it does not create folders, write manifests, move files, restore files, delete files, or enable real-profile execution.

## Non-goals

- Do not wire root execution safety into WPF controls yet.
- Do not enable real-profile Quarantine execution.
- Do not enable real-profile Undo Quarantine.
- Do not implement Pre-Execution Revalidation yet.
- Do not create quarantine folders or manifests.
- Do not modify real-profile files.

## Current behavior

Before this packet, `QuarantineExecutionReadiness` could name that Quarantine Root Execution Safety had not been checked. Quarantine Root Safety Note existed only for preview and did not check execution-specific containment, capacity, or collision evidence.

## Desired behavior

Core code has a read-only `QuarantineRootExecutionSafety` model that checks:

- Quarantine Root is fully qualified.
- `D:` remains preferred.
- Safe non-`D:` roots require an extra acknowledgement.
- Unsafe roots remain blocked with no override.
- Quarantine Root is not inside the Cleanup Scope.
- Quarantine Root is not a parent of the Cleanup Scope.
- Action root, items root, Restore Manifest path, and item destinations stay in their expected action-scoped layout.
- Action root, Restore Manifest path, and item destinations do not already exist.
- Available free-space evidence exists and is enough for planned bytes plus manifest overhead.

`QuarantineExecutionReadiness` can optionally consume this result. If WPF or a future caller does not provide root safety yet, real-profile readiness still reports that root safety has not been checked.

## Decisions made

- Root execution safety is built from `QuarantineActionDraft` for normal use because the action draft already contains the exact action root, items root, manifest path, and action item destinations.
- A lower-level overload accepts explicit paths so tests and future validation can cover malformed or relative root input.
- Capacity can use `DriveInfo` by default, with test overrides for deterministic checks.
- Non-`D:` acknowledgement clears only the non-preferred-root acknowledgement blocker. It does not override containment, collision, capacity, or layout blockers.

## Files changed

- `src/WindowsFileCleaner.Core/QuarantineRootExecutionSafety.cs`
- `src/WindowsFileCleaner.Core/QuarantineRootExecutionSafetyBuilder.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionReadinessBuilder.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/features/2026-05-31-quarantine-root-execution-safety.md`
- `docs/features/2026-05-31-quarantine-execution-readiness-model.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added a read-only Quarantine Root Execution Safety model and builder.
- Added tests for safe non-`D:` root acknowledgement, fully qualified roots, root/scope containment blockers, capacity blockers, action-root collisions, item destination collisions, and readiness consumption.
- Kept WPF execution behavior unchanged.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Follow-up work:

- Add Pre-Execution Revalidation as the next non-moving core packet.
- Add WPF readiness output only after the core model, root safety, and revalidation are stable, keeping execution disabled.

Open questions:

- None for this packet.

Risky assumptions:

- Free-space checks are advisory readiness evidence until a later pre-execution packet reruns them immediately before movement.
