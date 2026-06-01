# Feature: Exact-Profile Recovery Review Entry Evidence

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Record focused entry-level Restore Manifest evidence for exact `C:\Users\moxhe` recovery-review debt before any future real-profile batch review.

## Non-goals

- Do not launch WPF.
- Do not click `Scan` or scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, write Restore Manifests, approve cleanup, or create cleanup history.
- Do not add broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, persisted cleanup history, shortcut creation, or installer behavior.

## Evidence

`cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries` passed as a read-only terminal check.

It reported the default Quarantine Root `D:\WindowsFileCleanerQuarantine` with 10 valid manifests, 13 entries, 101.1 MB total size, 2 full-root undo-work manifests, and 2 full-root recovery-review manifests. The exact-profile recovery-review display matched 2 of 10 manifests, with displayed undo work `0` and displayed recovery review `2`.

The two displayed recovery-review manifests are older failed-only NVIDIA `DXCache` attempts:

- `quarantine-action-draft-20260601110130-9010b9d9`: `Failed`, 1 directory entry, 51.06 MB, moved 0, restored 0, failed 1, undo work `no`, recovery review `yes`; original path `C:\Users\moxhe\AppData\LocalLow\NVIDIA\DXCache`; error: descendant `.nvph` file was in use by another process.
- `quarantine-action-draft-20260601103558-b3e00fc2`: `Failed`, 1 directory entry, 36.06 MB, moved 0, restored 0, failed 1, undo work `no`, recovery review `yes`; original path `C:\Users\moxhe\AppData\LocalLow\NVIDIA\DXCache`; error: source and destination roots differed before the guarded cross-volume fallback existed.

`cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly` also passed and showed zero exact-profile undo-work matches.

`cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayed -RequireNoDisplayedUndoWork` passed and showed the exact-profile display still has 4 of 10 manifests, displayed undo work `0`, and displayed recovery review `2`.

## Decisions

- Treat the two failed NVIDIA `DXCache` manifests as visible recovery-review evidence, not as undo work.
- Keep them non-blocking for the current next-batch preset because they moved zero files and the exact-profile displayed undo-work count is zero.
- Keep any future recovery action selected-manifest-only through existing WPF gates unless a later ADR expands restore/history behavior.

## Verification

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayed -RequireNoDisplayedUndoWork`

## Docs

- `docs/features/2026-06-02-exact-profile-recovery-review-entry-evidence.md`
- `docs/features/2026-06-02-restore-manifest-recovery-review-filter.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is read-only terminal evidence over existing Restore Manifest Summary behavior and ADR 0017/0018/0019 movement gates.

## Follow-up Work

- Use `tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries` when exact-profile recovery-review debt needs refreshed entry-level evidence.
- Use `tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly` or `-RequireNoDisplayedUndoWork` when exact-profile undo-work clearance needs refreshed evidence.

## Risks And Assumptions

- The two old failed NVIDIA `DXCache` attempts should remain visible until deliberately reviewed; hiding or auto-cleaning them would weaken recovery evidence.
- The current next-batch preset can remain focused on zero displayed undo work while still displaying recovery-review debt.
