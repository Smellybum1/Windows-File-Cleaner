# Feature: Quarantine Execution Readiness Model

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add the first non-moving core model from ADR 0018 so the app can name fixture-executable, real-profile-candidate, and custom-preview-only readiness states without enabling real-profile movement.

## Non-goals

- Do not wire this model into WPF execution controls yet.
- Do not move, restore, delete, create, or modify real-profile files.
- Do not enable real-profile Quarantine execution.
- Do not enable real-profile Undo Quarantine.
- Quarantine Root Execution Safety and Pre-Execution Revalidation were implemented in later non-moving core packets.

## Current behavior

The current WPF execution gate still uses the existing fixture-only confirmation/execution path. Real-profile and custom non-fixture scopes remain preview-only.

## Desired behavior

Core code has a read-only `QuarantineExecutionReadiness` model that:

- reports fixture scopes as fixture-executable only when current fixture execution is already available and confirmation blockers are clear,
- reports exact `C:\Users\moxhe` as a real-profile candidate while keeping current-build execution unavailable,
- reports real-profile child scopes and custom non-fixture scopes as custom-preview-only for this first phase,
- carries first-phase user decisions: exact `QUARANTINE`, 10-row cap, 1 GB cap, Likely safe + Quarantine candidate only, narrow folders only with strict descendant checks, selected-manifest real-profile Undo prerequisite, non-`D:` acknowledgement, manual rescan guidance, and Restore Manifest-only durable record.

## Decisions made

- ADR 0018 is accepted with the user decisions from the Grill with Docs pass.
- `QuarantineExecutionReadiness` is a core model only in this packet; it does not replace `QuarantineExecutionGate` yet.
- Real-profile candidate readiness always remains non-executable in the current build.

## Files changed

- `src/WindowsFileCleaner.Core/QuarantineExecutionReadiness.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionReadinessBuilder.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionReadinessDisposition.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionReadinessScopeKind.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/features/2026-05-31-real-profile-quarantine-design-pass.md`
- `docs/features/2026-05-31-quarantine-execution-readiness-model.md`
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

- Added a read-only core readiness model and builder.
- Added tests for fixture-executable, real-profile-candidate, custom-preview-only, real-profile child preview-only, first-phase caps, row eligibility, narrow folder checks, non-`D:` acknowledgement, selected-manifest Undo prerequisite, manual rescan guidance, and Restore Manifest-only durable record.
- Kept WPF execution behavior unchanged.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Follow-up work:

- Pre-Execution Revalidation was implemented in a later non-moving core packet.
- Add WPF readiness output only after the core model, root safety, revalidation, and real-profile restore readiness are stable, keeping execution disabled.

Open questions:

- None for this packet.

Risky assumptions:

- Exact `C:\Users\moxhe` is the only first-phase real-profile execution scope.
