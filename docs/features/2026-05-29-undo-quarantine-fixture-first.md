# Feature: Undo Quarantine Fixture First

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add fixture-tested core Undo Quarantine so moved entries can be restored from action-scoped quarantine paths before WPF cleanup execution is enabled.

## Non-goals

- Do not wire Undo Quarantine into WPF.
- Do not enable WPF Quarantine execution.
- Do not permanently delete files.
- Do not clean up quarantine folders.
- Do not overwrite existing original paths.

## User story / job story

As the project owner, I want the app to prove it can undo a quarantine action before I can run cleanup from the visible app, so that reversible cleanup is more trustworthy.

## Current behavior

At packet start, the core Quarantine Executor could move fixture files into quarantine and write Restore Manifest move status, but there was no core Undo Quarantine component.

## Desired behavior

- Core Undo Quarantine restores only entries with `Moved` status.
- It writes `Restoring` before restore attempts.
- It revalidates quarantine path, original path, and reparse-point state.
- It refuses to overwrite existing original paths.
- It writes `Restored` or `RestoreFailed` after each attempt.
- It handles partial restore and manifest write failures with recovery-review results.
- WPF remains unwired for both execution and undo.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Undo Quarantine Executor | Added as the fixture-first core restore component. | yes |
| Undo Quarantine Result | Added as the summary of restore attempts. | yes |
| Undo Quarantine Entry Result | Added as per-entry restore outcome evidence. | yes |
| Restore Manifest Action Status | Extended with restore statuses. | yes |
| Restore Manifest Entry Status | Extended with restore statuses. | yes |

## Open questions

Questions that must be answered before implementation:

- None. ADR 0008 selects fixture-first core Undo Quarantine.

Questions that can be deferred:

- What UI should discover and select existing restore manifests?
- Should successful Undo Quarantine offer to clean up empty action folders?
- How should the app surface leftover temp files after a hard crash?

## Grill notes

### Scenarios discussed

- The user prefers quarantine with easy undo.
- Forward fixture execution exists but WPF execution remains disabled.
- Undo should be proven before the visible cleanup button can move real-profile files.

### Edge cases

- Original path exists.
- Quarantine path missing.
- One of multiple restores fails.
- Manifest write fails before a restore.
- Manifest write fails after a restore.
- Directory restore.
- Entries in Moving, Failed, Planned, or RestoreFailed status are not restored automatically.

### Dependencies between decisions

- Depends on ADR 0005 write-ahead Restore Manifest.
- Depends on ADR 0006 temp-replace Restore Manifest writes.
- Depends on ADR 0007 fixture-first Quarantine Executor.
- Adds ADR 0008 fixture-first Undo Quarantine.

## Evidence and validation gate

Evidence gathered:

- User answers: quarantine should be undoable and preferably on `D:`.
- Existing code/docs inspected: Restore Manifest statuses, Quarantine Executor, Restore Manifest File Store, source-level filesystem-call allowlist.
- Tests/checks planned: core restore success, original-path collision, missing quarantine path, partial restore, write failures, directory restore, WPF still disabled, build, test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not overwrite original paths during undo.
- Do not automatically delete quarantine action folders after restore.
- Do not implement same-execution rollback inside Quarantine Executor.
- Do not wire Undo Quarantine into WPF in this packet.

## Decisions made

Small feature-level decisions:

- Add `UndoQuarantineExecutor`, `UndoQuarantineResult`, and `UndoQuarantineEntryResult`.
- Extend Restore Manifest statuses for restore lifecycle.
- Allow restore filesystem APIs only in `UndoQuarantineExecutor`.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0008-use-fixture-first-undo-quarantine.md`

## Implementation plan

1. Add ADR 0008.
2. Extend Restore Manifest statuses for restore lifecycle.
3. Add core Undo Quarantine executor and result models.
4. Add fixture tests for restore success, collisions, missing quarantine paths, partial restore, write failures, and directory restore.
5. Update docs and progress.
6. Run full preflight, commit, push, and verify CI.

## Files expected to change

Expected:

- `docs/decisions/0008-use-fixture-first-undo-quarantine.md`
- `src/WindowsFileCleaner.Core/UndoQuarantineExecutor.cs`
- `src/WindowsFileCleaner.Core/UndoQuarantineResult.cs`
- `src/WindowsFileCleaner.Core/UndoQuarantineEntryResult.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestActionStatus.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestEntryStatus.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestEntry.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestBuilder.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-undo-quarantine-fixture-first.md`
- `.codex/progress.md`

Possible:

- ADR 0005/0007 follow-up wording.
- MVP readiness audit.

## Test plan

Manual checks:

- None required for this packet; Undo Quarantine is not wired into WPF.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- Restore status values add schema surface before real manifests exist.
- WPF will later need careful manifest discovery and stale-state wording.

Assumptions:

- Fixture-first core undo is the safest next step before enabling visible cleanup execution.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added ADR 0008 for the fixture-first Undo Quarantine boundary.
- Added `UndoQuarantineExecutor`, `UndoQuarantineResult`, and `UndoQuarantineEntryResult`.
- Extended Restore Manifest action and entry statuses for restore lifecycle.
- Added restore timestamps to Restore Manifest entries.
- Added fixture tests for restore success, original-path collision, partial restore, moved-only restore after move failure, missing quarantine path, initial restore manifest write failure, post-restore manifest write failure, and directory restore.
- Extended the source-level filesystem-call guard to allow restore move APIs only in `UndoQuarantineExecutor`.
- Kept WPF Quarantine execution and WPF Undo Quarantine unavailable.

Files changed:

- `docs/decisions/0008-use-fixture-first-undo-quarantine.md`
- `src/WindowsFileCleaner.Core/UndoQuarantineExecutor.cs`
- `src/WindowsFileCleaner.Core/UndoQuarantineResult.cs`
- `src/WindowsFileCleaner.Core/UndoQuarantineEntryResult.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestActionStatus.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestEntryStatus.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestEntry.cs`
- `src/WindowsFileCleaner.Core/RestoreManifest.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `docs/decisions/0007-use-fixture-first-quarantine-executor.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-quarantine-executor-fixture-first.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `docs/features/2026-05-29-undo-quarantine-fixture-first.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

Docs updated:

- ADR 0008, domain context, glossary, README, MVP readiness audit, related prior ADR/feature follow-up wording, this feature brief, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0008-use-fixture-first-undo-quarantine.md`.

Follow-up work:

- Wire WPF Quarantine execution only after stale-state and confirmation checks are explicit.
- Add WPF Undo Quarantine with manifest discovery, stale-state checks, confirmation wording, and recovery UI.
- Add optional quarantine action folder cleanup only after restore behavior is trusted.

Open questions:

- What UI should discover and select existing Restore Manifests?
- Should successful WPF Undo Quarantine offer to clean up empty action folders?
- How should the app surface leftover temp files after a hard crash?

Risky assumptions:

- Fixture-proven core undo is the right intermediate step before enabling visible cleanup execution.
