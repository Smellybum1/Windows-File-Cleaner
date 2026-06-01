# Feature: Cross-Volume Directory Quarantine Fallback

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Fix the first live exact real-profile folder Quarantine failure where Windows rejected moving a directory from `C:` to the preferred `D:\WindowsFileCleanerQuarantine` root, while keeping the same ADR 0017/0018 gates and making the WPF Quarantine Execution Gate details easier to read.

## Non-goals

- Do not run or retry real-profile Quarantine from Codex.
- Do not enable custom or non-exact real-profile Quarantine.
- Do not enable broad/all-manifest Undo Quarantine.
- Do not enable permanent deletion or persisted cleanup history.
- Do not add action-folder cleanup.

## Current Behavior

- The user reported a first live exact real-profile Quarantine attempt for `C:\Users\moxhe\AppData\LocalLow\NVIDIA\DXCache` failed with `moved 0, failed 1`.
- The failure was caused by `Directory.Move` requiring identical source and destination roots when moving a folder from `C:` to `D:`.
- The app wrote failure evidence to the Restore Manifest and requested recovery review; no moved entries were reported.

## Desired Behavior

- File Quarantine continues using the existing file move path.
- Same-volume directory Quarantine continues using `Directory.Move`.
- Cross-volume directory Quarantine uses a copy-then-delete fallback inside the same `QuarantineExecutor` path after the write-ahead Restore Manifest is in place.
- The fallback stages the copied directory under the action item parent, refuses reparse-point source paths and descendants, moves the staging folder to the final quarantine path, and deletes the original only after the destination copy is present.
- If deleting the original fails after the copy is placed, the executor records failure/recovery-review evidence rather than pretending the entry cleanly moved.
- WPF Quarantine Execution Gate details use a taller constrained scroll area so real-profile readiness and execution evidence are readable.

## Domain Language Changes

No new durable terms.

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Executor | Clarified that it is wired for fixture execution and the exact first real-profile phase, and that directory movement can use a cross-volume fallback. | yes |

## Grill Notes

### Scenarios Discussed

- The user reported the first live real-profile folder attempt error and asked to expand the information bar.
- The fix keeps the user's movement click as the only real-profile action; Codex only changes code and tests.

### Edge Cases

- Cross-volume directory movement cannot be as atomic as same-volume rename.
- Reparse points stay blocked during copy traversal.
- If the original folder cannot be removed after copy, the entry stays failed and requires recovery review.
- Existing destination collisions remain blockers.

## Decisions Made

Small feature-level decisions:

- Add a narrow `QuarantineDirectoryMove` component for directory movement instead of spreading copy/delete calls through WPF or readiness builders.
- Keep all filesystem write calls covered by the existing production source guard.
- Expand the Quarantine Execution Gate scroll area from a small strip to a taller bounded details area.

ADR-worthy decisions:

- [x] No ADR added. ADR 0017/0018 already choose the preferred `D:` Quarantine Root and exact first real-profile execution contract; this packet fixes an implementation bug inside that accepted path.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Follow-up Work

- Run full `.cmd` MVP preflight before the user retries any exact real-profile Quarantine batch.
- Have the user retry only a tiny reviewed exact batch after confirming the app has been rebuilt and the same readiness gates are open.
- After a successful live Quarantine, use manifest discovery and selected restore readiness for the created Restore Manifest if recovery proof is needed.

## Risks And Assumptions

Risks:

- Copy-then-delete directory movement is not a single filesystem rename. If deletion fails after copy placement, recovery review is required.

Assumptions:

- The first live folder target is a narrow rebuildable cache that already passed ADR 0018 readiness and strict descendant checks in the user's WPF session.
