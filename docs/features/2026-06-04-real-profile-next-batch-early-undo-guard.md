# Real-Profile Next-Batch Early Undo Guard

Date: 2026-06-04

Status: completed

## Goal

Stop the next-batch evidence preset before MVP preflight when displayed Restore Manifest undo work is present, so a blocked run cannot produce fresh preflight evidence that looks like movement readiness.

## Safety Profile

`terminal-readonly`. The implementation reads Restore Manifest evidence and the regression writes temporary ignored synthetic manifests under `.local\real-profile-next-batch-stop-guard-test`, then removes them. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, write real Restore Manifests, create shortcuts, install anything, or create cleanup history.

## Problem

After the 2026-06-04 second exact-profile Quarantine batch, exact-profile displayed undo work is expected to be `1`. The `-RequireNextBatchEvidence` preset already failed when displayed undo work was present, but only after the full MVP preflight step. That could leave fresh preflight output near a failed next-batch run, which is noisier than the current stop-state rule needs.

## Changes

- `Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` now runs an early displayed undo-work stop check before MVP preflight.
- The early check uses the same read-only Restore Manifest summary strictness: `-UndoWorkOnly` plus `-RequireNoDisplayedUndoWork`.
- Added `tools\Test-RealProfileNextBatchStopGuard.cmd` and `.ps1`.
- The regression synthesizes a blocked root with a `Moved` exact-profile entry and proves the preset exits before full preflight or daily readiness.
- The regression synthesizes a clear root with a `Restored` exact-profile entry and proves the preset can continue when displayed undo work is absent.

## Verification

- `cmd.exe /c tools\Test-RealProfileNextBatchStopGuard.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check` passed with expected CRLF warnings only.

The real current next-batch review was not rerun as movement evidence because exact-profile displayed undo work is present.

## ADRs

No ADR added. This strengthens terminal-only enforcement around existing ADR 0017, ADR 0018, and ADR 0019 gates without changing cleanup execution, restore execution, persistence, deployment, or package acceptance policy.

## Follow-Up

- Keep the current stop state until the selected manifest is restored or a new Grill with Docs pass decides outstanding selected-manifest undo work is acceptable.
