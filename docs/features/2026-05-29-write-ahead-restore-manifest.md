# Feature: Write-Ahead Restore Manifest

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Resolve Restore Manifest write order and partial-failure state before adding file-moving code.

## Non-goals

- Do not create quarantine folders.
- Do not move, delete, rename, or modify scanned files.
- Do not write a Restore Manifest file.
- Do not implement Undo Quarantine.
- Do not update the source-level filesystem-call guard to permit execution APIs yet.

## User story / job story

As the project owner, I want Quarantine execution to have recovery metadata before it moves anything, so that a failed or interrupted cleanup action can be inspected and undone where feasible.

## Current behavior

Quarantine Action Draft shows action-scoped item and manifest paths, but the exact manifest write order and partial-failure model were unresolved. Existing docs also disagreed about whether the executed manifest should be written before or after moves.

## Desired behavior

- ADR 0005 defines a write-ahead Restore Manifest model.
- The core library can build an in-memory planned Restore Manifest from a Quarantine Action Draft and Restore Manifest Draft.
- The planned manifest uses action-scoped quarantine paths, not preview paths.
- The manifest records action status and per-entry status.
- In-memory status transitions can represent Moving, Completed, Partial failure, and Failed.
- The WPF Quarantine Execution Gate shows the planned write order while actual execution remains unavailable.
- No folders are created and no files are modified.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Restore Manifest | Clarified as a write-ahead recovery record once execution begins. | yes |
| Restore Manifest Action Status | Added for overall action state. | yes |
| Restore Manifest Entry Status | Added for per-entry move state. | yes |

## Open questions

Questions that must be answered before implementation:

- None for this read-only packet.

Questions that can be deferred:

- Should the future manifest file writer use temp-file replacement, append-only journal entries, or another local durability pattern?
- What exact recovery UI should be shown for Moving entries after an app crash or manifest write failure?

## Grill notes

### Scenarios discussed

- Quarantine should preferably be on `D:` and undoable.
- A future execution flow must not move files without recovery metadata.
- Partial failures should preserve enough state to decide what can be restored and what needs manual review.
- Sidecar safety review identified a doc conflict between write-before-move and write-after-move wording.

### Edge cases

- Source missing after preview.
- Destination already exists.
- Manifest write failure before the first move.
- Crash after planned manifest write but before any move.
- Crash after an entry is marked Moving.
- One of multiple moves fails.
- Original path exists again during Undo Quarantine.

### Dependencies between decisions

- Depends on ADR 0003 JSON Restore Manifest.
- Depends on ADR 0004 action-scoped quarantine layout.
- Adds ADR 0005 write-ahead Restore Manifest.
- Future execution must update the filesystem-call regression guard with a narrow allowlist.

## Evidence and validation gate

Evidence gathered:

- User answers: Quarantine should be on `D:` where possible and easy to undo.
- Existing code/docs inspected: Restore Manifest Draft, Quarantine Confirmation Draft, Quarantine Execution Gate, Quarantine Action Draft, ADR 0003, ADR 0004, source-level read-only regression test.
- Sidecar safety review: identified manifest timing conflict and recommended write-ahead state with partial-failure coverage.
- Tests/checks planned: core manifest write-ahead tests, WPF gate readout smoke coverage, build, test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not write the Restore Manifest only after all moves succeed.
- Do not write the Restore Manifest only after moves are attempted.
- Do not introduce a separate database for the MVP execution record.
- Do not call this a Restore Log or Execution Journal in the app language; keep the glossary term Restore Manifest.

## Decisions made

Small feature-level decisions:

- Add `RestoreManifest`, `RestoreManifestEntry`, `RestoreManifestBuilder`, and `RestoreManifestJsonSerializer`.
- Add `RestoreManifestActionStatus` with Planned, Moving, Completed, PartialFailure, and Failed.
- Add `RestoreManifestEntryStatus` with Planned, Moving, Moved, and Failed.
- Show the write-ahead Restore Manifest readout in the Quarantine Execution Gate.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0005-use-write-ahead-restore-manifest.md`

## Implementation plan

1. Add ADR 0005 for write-ahead manifest ordering.
2. Add core planned Restore Manifest model, serializer, and status helpers.
3. Show planned manifest write order in the WPF execution gate.
4. Add core and WPF tests.
5. Update docs and progress.

## Files expected to change

Expected:

- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `src/WindowsFileCleaner.Core/RestoreManifest.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestEntry.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestActionStatus.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestEntryStatus.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestBuilder.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestJsonSerializer.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Possible:

- `docs/decisions/0003-use-json-restore-manifest.md`
- `docs/decisions/0004-use-action-scoped-quarantine-layout.md`
- Existing quarantine feature briefs.

## Test plan

Manual checks:

- Use fixture review, create Quarantine Preview, and confirm the Quarantine Execution Gate shows the write-ahead Restore Manifest and write order.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- Repeated manifest writes can fail, so future execution must treat manifest-write failure as a first-class blocker.
- Moving entries after a crash may require careful recovery UI to avoid making things worse.

Assumptions:

- JSON remains sufficient for the first local Restore Manifest.
- Repeated writes are acceptable for the MVP because the selected action set should be small and reviewed.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added ADR 0005 and updated ADR 0003/0004 follow-up wording.
- Added planned write-ahead Restore Manifest model and status transitions.
- Added JSON serialization for the planned/executed Restore Manifest shape.
- Added WPF Quarantine Execution Gate readout for write-ahead manifest order.
- Kept all changes read-only; no production file-moving or manifest-writing APIs were added.

Files changed:

- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/decisions/0003-use-json-restore-manifest.md`
- `docs/decisions/0004-use-action-scoped-quarantine-layout.md`
- `src/WindowsFileCleaner.Core/RestoreManifest.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestEntry.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestActionStatus.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestEntryStatus.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestBuilder.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestJsonSerializer.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-write-ahead-restore-manifest.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

Docs updated:

- ADR 0005, ADR 0003, ADR 0004, domain context, glossary, README, feature brief, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0005-use-write-ahead-restore-manifest.md`.

Follow-up work:

- Narrow manifest file writing is now covered by `docs/features/2026-05-29-restore-manifest-file-store.md` and ADR 0006.
- Update the filesystem-call regression guard from blanket denial to strict execution-component allowlist when real writes are introduced.
- Implement actual Quarantine moves only after manifest write behavior is proven.

Open questions:

- What exact recovery UI should handle Moving entries after interruption?
- How should the future app surface leftover temp files after a hard crash?

Risky assumptions:

- Write-ahead manifesting is the right recovery model for the first reversible cleanup executor.
