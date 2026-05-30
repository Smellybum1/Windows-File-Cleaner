# Feature: Pre-Execution Revalidation

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add the non-moving core Pre-Execution Revalidation check from ADR 0018.

This packet re-checks reviewed synthetic fixture evidence against the live filesystem immediately before a future execution attempt would begin. It does not create folders, write manifests, move files, restore files, delete files, or enable real-profile execution.

## Non-goals

- Do not wire Pre-Execution Revalidation into WPF controls yet.
- Do not enable real-profile Quarantine execution.
- Do not enable real-profile Undo Quarantine.
- Do not create quarantine folders or manifests.
- Do not modify real-profile files.

## Current behavior

Before this packet, `QuarantineExecutionReadiness` could consume Quarantine Root Execution Safety but still reported that Pre-Execution Revalidation had not been checked.

## Desired behavior

Core code has a read-only `PreExecutionRevalidation` model that checks:

- Quarantine Preview, Quarantine Confirmation Draft, Quarantine Action Draft, and Quarantine Root Execution Safety still agree.
- Included row counts and byte counts have not changed.
- The Quarantine Action Draft still maps every included preview row and has no extra rows.
- Source paths still exist.
- Source file size and modified timestamp still match the reviewed preview evidence.
- Source paths have not become reparse points.
- Action root and Restore Manifest path do not already exist.
- Planned item destinations do not already exist.
- Quarantine Root Execution Safety remains clean.

`QuarantineExecutionReadiness` can optionally consume this result. If WPF or a future caller does not provide revalidation yet, real-profile readiness still reports that Pre-Execution Revalidation has not been checked.

## Decisions made

- Revalidation is built from Quarantine Preview, Quarantine Confirmation Draft, Quarantine Action Draft, and Quarantine Root Execution Safety.
- Revalidation checks files more deeply than directories for now: files compare size and modified timestamp; directories check existence, type, and reparse status.
- Revalidation is read-only and should run again immediately before any future real-profile movement.

## Files changed

- `src/WindowsFileCleaner.Core/PreExecutionRevalidation.cs`
- `src/WindowsFileCleaner.Core/PreExecutionRevalidationBuilder.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionReadinessBuilder.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/features/2026-05-31-pre-execution-revalidation.md`
- `docs/features/2026-05-31-quarantine-root-execution-safety.md`
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

- Added a read-only Pre-Execution Revalidation model and builder.
- Added tests for unchanged fixture action, missing source, changed source, destination collision, stale action draft mismatch, and readiness consumption.
- Kept WPF execution behavior unchanged.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Follow-up work:

- Design selected-manifest real-profile Undo Quarantine execution boundary before forward real-profile movement. The read-only Real-Profile Restore Readiness model was added in the next safety packet.
- Add WPF readiness output only after the core model, root safety, revalidation, and real-profile restore readiness are stable, keeping execution disabled.

Open questions:

- None for this packet.

Risky assumptions:

- Directory revalidation can remain conservative without recursively recomputing full directory contents until a later packet decides whether the extra scan cost is worth it.
