# Feature: Quarantine Executor Fixture First

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add the first core Quarantine executor that moves fixture files/folders through write-ahead Restore Manifest updates, without wiring WPF execution yet.

## Non-goals

- Do not enable the WPF `Execute quarantine` button.
- Do not scan or modify the real profile.
- Do not implement Undo Quarantine.
- Do not implement permanent deletion.
- Do not overwrite destination paths.

## User story / job story

As the project owner, I want Quarantine execution to be proven on synthetic fixtures before it can touch my real profile, so that the app can move toward cleanup safely.

## Current behavior

The core library can build a planned Restore Manifest and write it to `restore-manifest.json`, but no component moves selected source files or folders into the action-scoped quarantine items folder.

## Desired behavior

- The core library can execute a planned Restore Manifest against fixture files.
- The executor writes the planned manifest before the first move.
- The executor updates each entry to Moving before a move attempt.
- The executor updates each entry to Moved or Failed after each move attempt.
- The executor creates only needed destination parent folders.
- The executor refuses missing sources, existing destinations, out-of-scope paths, and reparse points detected at execution time.
- The WPF UI remains unavailable for execution.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Executor | Added as the narrow core component that performs fixture-proven file moves. | yes |
| Quarantine Execution Result | Added as the summary of moved and failed entries. | yes |
| Quarantine Execution Entry Result | Added as per-entry execution outcome evidence. | yes |

## Open questions

Questions that must be answered before implementation:

- None. ADR 0007 selects fixture-first core execution and keeps WPF wiring deferred.

Questions that can be deferred:

- What exact WPF confirmation/stale-state checks are required before calling the executor?
- What recovery UI should handle Moving entries after interruption?
- How should the app surface leftover temp manifest files after a hard crash?

## Grill notes

### Scenarios discussed

- Quarantine should be reversible and preferably use `D:`.
- The app must not move files before write-ahead recovery metadata exists.
- The first executor should be tested against synthetic fixture files, not the real profile.
- UI execution should stay disabled until a separate wiring packet.

### Edge cases

- Source missing after preview.
- Destination already exists.
- One of multiple entries fails while another moves.
- Source becomes a reparse point after preview.
- Manifest write fails before any move.
- Manifest write fails after some moves.
- Entry path is outside the Cleanup Scope or destination escapes the action items root.

### Dependencies between decisions

- Depends on ADR 0004 action-scoped quarantine layout.
- Depends on ADR 0005 write-ahead Restore Manifest.
- Depends on ADR 0006 temp-replace Restore Manifest writes.
- Adds ADR 0007 fixture-first Quarantine executor.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: Restore Manifest, Restore Manifest File Store, Quarantine Action Draft, Quarantine Execution Gate, source-level filesystem-call regression test.
- Tests/checks planned: fixture execution success, partial failure, destination collision, missing source, allowlist regression, WPF still disabled, build, test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not wire WPF execution in the same packet as the first executor.
- Do not overwrite quarantine destinations.
- Do not stop the whole action after the first per-entry move failure if later entries can still be represented safely.
- Do not implement permanent deletion before Undo Quarantine.

## Decisions made

Small feature-level decisions:

- Add `QuarantineExecutor`, `QuarantineExecutionResult`, and `QuarantineExecutionEntryResult`.
- Move files with `File.Move` and folders with `Directory.Move` only inside `QuarantineExecutor`.
- Keep `RestoreManifestFileStore` responsible for manifest writes.
- Extend the source-level filesystem-call allowlist only for the executor component.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0007-use-fixture-first-quarantine-executor.md`

## Implementation plan

1. Add ADR 0007.
2. Add core executor and result models.
3. Add fixture tests for successful execution and partial failures.
4. Keep WPF execution disabled and covered.
5. Update docs and progress.
6. Run full preflight, commit, push, and verify CI.

## Files expected to change

Expected:

- `docs/decisions/0007-use-fixture-first-quarantine-executor.md`
- `src/WindowsFileCleaner.Core/QuarantineExecutor.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionResult.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionEntryResult.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-executor-fixture-first.md`
- `.codex/progress.md`

Possible:

- `docs/features/2026-05-29-restore-manifest-file-store.md`

## Test plan

Manual checks:

- None required for this packet; the executor is not wired into WPF.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- This introduces production file-moving APIs, so the allowlist must stay narrow.
- A manifest write failure after a move can leave recovery state weaker than intended.

Assumptions:

- Fixture-proven core execution is the right intermediate step before enabling WPF execution.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added ADR 0007 for the fixture-first Quarantine Executor boundary.
- Added `QuarantineExecutor`, `QuarantineExecutionResult`, and `QuarantineExecutionEntryResult`.
- Added write-ahead execution behavior: planned manifest write, Moving write, move attempt, then Moved or Failed write.
- Added fixture tests for successful moves, partial failure, destination collision, missing source, initial manifest write failure, and post-move manifest write failure.
- Kept WPF execution disabled and status-only.

Files changed:

- `docs/decisions/0007-use-fixture-first-quarantine-executor.md`
- `src/WindowsFileCleaner.Core/QuarantineExecutor.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionResult.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionEntryResult.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `docs/features/2026-05-29-quarantine-executor-fixture-first.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

Docs updated:

- ADR 0007, ADR 0005/0006 follow-up wording, domain context, glossary, README, feature briefs, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0007-use-fixture-first-quarantine-executor.md`.

Follow-up work:

- Wire WPF execution only after stale-state and confirmation checks are explicit.
- Core Undo Quarantine for Moved entries is covered by `docs/features/2026-05-29-undo-quarantine-fixture-first.md`; WPF execution and WPF undo remain separate follow-ups.
- Add recovery UI for Moving, Failed, and leftover temp manifest states.

Open questions:

- What exact WPF stale-state checks are required before calling the executor?
- What recovery UI should handle Moving entries after interruption?
- How should the app surface leftover temp manifest files after a hard crash?

Risky assumptions:

- Fixture-proven core execution is the right intermediate step before enabling WPF execution.
