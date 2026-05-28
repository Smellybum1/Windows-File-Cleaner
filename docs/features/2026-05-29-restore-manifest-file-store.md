# Feature: Restore Manifest File Store

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add a narrow, fixture-tested file store for writing Restore Manifest JSON to the action-scoped manifest path, without moving files or wiring Quarantine execution.

## Non-goals

- Do not move, delete, rename, or modify scanned files.
- Do not implement Quarantine execution.
- Do not implement Undo Quarantine.
- Do not wire the WPF `Execute quarantine` button.
- Do not allow arbitrary production filesystem writes outside the manifest file-store component and existing user-selected CSV exports.

## User story / job story

As the project owner, I want the app's future Quarantine executor to write recovery metadata reliably before moving anything, so that reversible cleanup can be tested one safety layer at a time.

## Current behavior

The core library can build a planned write-ahead Restore Manifest in memory, but no production component writes the manifest file.

## Desired behavior

- The core library can write a Restore Manifest to its action-scoped `restore-manifest.json` path.
- The writer validates that the path stays inside the action root.
- The writer uses a temp-file replacement pattern.
- The writer does not create item folders and does not move scanned files.
- The source-level filesystem-call regression test allowlists only the manifest file-store component for manifest writes.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Restore Manifest File Store | Added as the narrow writer for action-scoped Restore Manifest JSON. | yes |

## Open questions

Questions that must be answered before implementation:

- None. ADR 0006 selects the temp-file replacement write pattern.

Questions that can be deferred:

- How should the future app surface leftover temp files after a hard crash?
- Should future Undo Quarantine expose a manifest integrity check before restore?

## Grill notes

### Scenarios discussed

- Future execution must write the planned Restore Manifest before the first move.
- Manifest writes should be tested before file-moving code exists.
- Existing source-level guard must remain strict and become an allowlist rather than a broad bypass.

### Edge cases

- Manifest path outside the action root.
- Manifest filename is not `restore-manifest.json`.
- First write when the manifest path does not exist.
- Replacement write when the manifest already exists.
- Temporary file cleanup after a failed write.

### Dependencies between decisions

- Depends on ADR 0004 action-scoped quarantine layout.
- Depends on ADR 0005 write-ahead Restore Manifest.
- Adds ADR 0006 temp-replace Restore Manifest writes.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: Restore Manifest model, action draft path rules, source-level filesystem-call regression test.
- Sidecar safety review: recommended a narrow execution component and strict allowlist once write APIs appear.
- Tests/checks planned: fixture-backed manifest write tests, source allowlist regression, build, test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not write manifests directly to the target path.
- Do not loosen the filesystem-call guard globally.
- Do not wire WPF execution in this packet.

## Decisions made

Small feature-level decisions:

- Add `RestoreManifestFileStore` and `RestoreManifestFileWriteResult`.
- Allow manifest file-store write APIs only in the source-level regression test.
- Keep WPF execution unavailable.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`

## Implementation plan

1. Add ADR 0006.
2. Add core Restore Manifest file store and write result.
3. Add fixture-backed tests for first write, replacement write, path validation, and source-level allowlist.
4. Update docs and progress.
5. Run full preflight, commit, push, and verify CI.

## Files expected to change

Expected:

- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `src/WindowsFileCleaner.Core/RestoreManifestFileStore.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestFileWriteResult.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `.codex/progress.md`

Possible:

- `docs/features/2026-05-29-write-ahead-restore-manifest.md`

## Test plan

Manual checks:

- None required for this packet; the writer is not wired into the WPF UI.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- Allowlisting write APIs too broadly would weaken the read-only safety guard.
- Temp replacement still needs future recovery handling for hard-crash leftovers.

Assumptions:

- Writing the manifest directory under the selected Quarantine root is acceptable only after explicit future execution starts; this packet adds the component but does not call it from WPF.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added ADR 0006 for temp-replace Restore Manifest writes.
- Added `RestoreManifestFileStore` and `RestoreManifestFileWriteResult`.
- Added fixture-backed coverage for first manifest write, replacement write, rejected paths, rejected filenames, and source-file preservation.
- Updated the source-level filesystem-call regression from blanket denial to a strict allowlist for user-selected CSV exports and the manifest file store.
- Kept WPF execution unavailable and added no file-moving code.

Files changed:

- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `src/WindowsFileCleaner.Core/RestoreManifestFileStore.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestFileWriteResult.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

Docs updated:

- ADR 0006, domain context, glossary, README, feature brief, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`.

Follow-up work:

- Core fixture-first file/folder moving is covered by `docs/features/2026-05-29-quarantine-executor-fixture-first.md` and ADR 0007.
- Add failure-injection coverage for manifest write failures.
- Core fixture-first Undo Quarantine is covered by `docs/features/2026-05-29-undo-quarantine-fixture-first.md` and ADR 0008; WPF Undo Quarantine remains a follow-up.

Open questions:

- How should the future app surface leftover temp files after a hard crash?
- Should future Undo Quarantine expose a manifest integrity check before restore?

Risky assumptions:

- Temp-file replacement is sufficient for the first local MVP manifest writer.
