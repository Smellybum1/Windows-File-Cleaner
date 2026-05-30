# Feature: Redundant Shortlist Overlap Cleanup

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Let the user fix Quarantine Preview overlap blockers with one click by removing broader Review Shortlist parent rows that make narrower rows redundant.

## Non-goals

- Do not allow redundant rows to pass the Quarantine Execution Gate.
- Do not silently quarantine overlapping parent/child rows.
- Do not change Quarantine Preview overlap detection.
- Do not enable real-profile WPF Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not move, restore, delete, create, write, or clean up files or folders.

## Desired behavior

- After `Preview shortlist quarantine` finds redundant rows, the WPF app enables a `Remove overlapping parents` action.
- Clicking it removes only included rows from the Review Shortlist when those rows are parent paths covering redundant child rows in the current Quarantine Preview.
- The app clears the now-stale Quarantine Preview, confirmation draft, action draft, and execution gate so the user must click `Preview shortlist quarantine` again.
- The status explains how many overlapping parent rows were removed and that no files were modified.
- The action stays disabled when no redundant preview rows exist or after fixture execution has already used the current preview.

## Decisions made

- Keep redundancy as a blocker rather than asking the user to approve overlapping execution. Moving parent and child paths together would create confusing Restore Manifest semantics.
- Prefer narrower rows by removing the included parent that covers redundant children. This matches the project rule that broad parent folders should stay inspection-first.
- Add a one-click cleanup action instead of a modal choice between parent-first and child-first behavior. A parent-first choice can be revisited after more manual review evidence.

## Implementation plan

1. Add a `Remove overlapping parents` WPF control near the preview/export controls.
2. Track enabled state from `_currentQuarantinePreview.RedundantCount`.
3. Remove included parent entries that cover redundant preview entries from `StorageReviewShortlist`, clear stale preview state, update rows/status, and require re-preview.
4. Add WPF smoke coverage for the redundant-overlap fix.
5. Update README, docs, progress, and handoff.

## Test plan

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- If a user wanted to keep the parent and drop children, this first version requires manual row removal instead.

Assumptions:

- A one-click stale-preview-clearing fix is a better first UX improvement than adding a confirmation dialog or changing overlap semantics.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added `Remove overlapping parents` beside Quarantine Preview controls.
- The action enables only when the current Quarantine Preview has redundant parent/child overlap.
- Clicking it removes broader included parent rows from Review Shortlist, keeps narrower child rows, clears stale preview/gate state, clears stale `QUARANTINE` text, and requires `Preview shortlist quarantine` again.
- Added WPF smoke coverage for the exact parent/child overlap path.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

ADRs added or skipped:

- Skipped. This is reversible WPF review-workflow polish that preserves existing Quarantine Preview and fixture-only execution boundaries.

Open questions:

- Should a later packet offer an explicit parent-first choice, or is the safer narrower-row default enough?

Risky assumptions:

- Removing broader parent rows is the safer default for overlapping shortlist fixes.
