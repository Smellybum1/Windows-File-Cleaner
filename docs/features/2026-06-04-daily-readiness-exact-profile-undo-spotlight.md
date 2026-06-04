# Daily Readiness Exact-Profile Undo Spotlight

Date: 2026-06-04

Status: completed

## Goal

Make the current exact-profile undo-work stop state visible in the default daily readiness command without requiring a separate Restore Manifest command.

## Safety Profile

`terminal-readonly`. The daily readiness spotlight reads Restore Manifest summaries only. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, write Restore Manifests, create shortcuts, install anything, or create cleanup history.

## Problem

The default daily readiness command already printed a broad Restore Manifest summary, but exact-profile undo work was mixed with fixture manifests. After the second exact-profile Quarantine batch, the important daily stop-state evidence is the exact `C:\Users\moxhe` undo-work count and manifest.

## Changes

- `Invoke-DailyLocalReadiness.cmd` now prints an `Exact-profile undo-work stop state` section after the broad Restore Manifest summary.
- The section runs the read-only Restore Manifest summary with `-CleanupScope "C:\Users\moxhe" -UndoWorkOnly`.
- `-ShowRestoreEntries` is forwarded to the spotlight when requested, but the default daily command stays concise.
- `tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd` now verifies the spotlight with temporary ignored synthetic Restore Manifests, and MVP preflight runs that regression by default.
- The regression uses `Invoke-DailyLocalReadiness.cmd -SyntheticRestoreManifestOnly`, which requires an explicit ignored `.local` Quarantine Root and skips package evidence only for this focused synthetic check.
- The regression also asserts the synthetic mode rejects a missing Quarantine Root, a Quarantine Root outside ignored `.local`, and package/fixture acceptance notes parameters.

## Verification

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `cmd.exe /c tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check` passed with expected CRLF warnings only.

The output showed the exact-profile undo-work manifest `quarantine-action-draft-20260604014901-b7b402a2` with displayed undo work `1`, without launching WPF, scanning, moving, restoring, deleting, approving cleanup, writing manifests, or creating cleanup history.

The synthetic regression proved broad daily readiness can see both exact-profile and fixture-scope undo-work manifests, while the final exact-profile spotlight displays only the exact `C:\Users\moxhe` undo-work manifest.

The regression does not depend on ignored accepted-package notes or local package folders, so the default MVP preflight remains suitable for CI and clean clones. Normal daily readiness still verifies accepted package evidence before Restore Manifest evidence.

The regression also covers the synthetic-mode guardrails for missing roots, outside-`.local` roots, and acceptance-note parameters.

## ADRs

No ADR added. This adds terminal-only visibility for existing Restore Manifest evidence and does not change cleanup execution, restore execution, persistence, deployment, or package acceptance policy.

## Follow-Up

- Keep using selected-manifest restore only if recovery is needed and after exact selected-manifest readiness, exact `RESTORE`, and immediate revalidation.
- Use `Invoke-MvpPreflight.cmd -SkipDailyReadinessUndoSpotlightCheck` only for focused local loops where this spotlight is not in scope.
