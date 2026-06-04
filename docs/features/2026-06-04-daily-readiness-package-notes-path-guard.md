# Daily Readiness Package Notes Path Guard

Date: 2026-06-04

Status: completed

## Goal

Cover the daily readiness accepted package notes path boundary so explicit package acceptance notes paths outside ignored `.local` fail before latest-notes or accepted launch-command printing.

## Safety Profile

`terminal-readonly`. The regression writes temporary ignored package acceptance notes, fixture acceptance notes, and an empty Restore Manifest root under `.local\daily-readiness-latest-package-notes-test`, uses committed `README.md` only as a non-`.local` rejection target, and removes the temporary files. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, write real acceptance notes, write Restore Manifests, or create cleanup history.

## Problem

Package acceptance summaries already reject explicit notes paths outside ignored `.local`, but daily readiness forwards `-AcceptanceNotesPath` before printing latest package notes or accepted launch commands. Without a composed regression, daily readiness could accidentally loosen the accepted-package notes boundary or print launch guidance after a rejected package notes path.

## Changes

- `tools\Test-DailyReadinessLatestPackageNotes.ps1` now verifies a non-`.local` explicit accepted package notes path fails during the daily readiness `Accepted package evidence` step.
- The regression asserts the package summary guard message is visible in daily readiness output.
- The regression asserts daily readiness stops before the informational latest-notes block and before accepted normal launch-command printing.
- Daily-use, portable release, compact handoff docs, and the feature index now record the composed path-boundary coverage.

## Verification

- `cmd.exe /c tools\Test-DailyReadinessLatestPackageNotes.cmd`
- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

## ADRs

No ADR added. This tightens terminal-only regression coverage for existing package acceptance notes and daily readiness tooling and does not change cleanup execution, restore execution, persistence, deployment, package acceptance policy, or app behavior.

## Follow-Up

- Keep explicit package acceptance notes paths under ignored `.local` for both standalone summaries and daily readiness.
- Keep incomplete and malformed-looking candidate notes informational until the human completes package acceptance and intentionally records any package/current-HEAD mismatch.
