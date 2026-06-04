# Daily Readiness Latest Package Notes Regression

Date: 2026-06-04

Status: completed

## Goal

Cover daily readiness latest package notes visibility so newer incomplete or malformed-looking notes remain informational and do not replace completed accepted package evidence.

## Safety Profile

`terminal-readonly`. The regression writes temporary ignored package acceptance notes, fixture acceptance notes, and an empty synthetic Restore Manifest root under `.local\daily-readiness-latest-package-notes-test`, runs daily readiness with explicit accepted and latest package notes paths, uses committed `README.md` only as a non-`.local` rejection target, and removes the test files. It now covers complete, incomplete, malformed-looking, and outside-`.local` explicit package acceptance notes paths without writing temporary latest-notes candidates into the default `.local\release-acceptance` search root. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, write real acceptance notes, write Restore Manifests, or create cleanup history.

## Problem

Daily readiness already showed latest package acceptance notes as informational context, but default preflight did not directly cover the composition where accepted evidence uses completed notes while the latest notes file is newer and incomplete. A follow-up tightened the same regression so a malformed-looking newest notes file reports missing evidence as informational context and still cannot block accepted-package readiness.

## Changes

- Added `tools\Test-DailyReadinessLatestPackageNotes.cmd` and `.ps1`.
- The regression creates complete accepted notes, incomplete candidate notes, and malformed-looking notes under its private ignored `.local\daily-readiness-latest-package-notes-test` folder.
- It runs daily readiness with the complete accepted notes as explicit accepted evidence and the incomplete or malformed-looking notes as explicit informational latest package notes.
- It verifies an explicit accepted package notes path outside ignored `.local` fails during accepted-package evidence before latest-notes or launch-command printing.
- It verifies the informational latest-notes block can use explicit incomplete notes and prints guarded pending next steps including `-RecordCommitMismatch`.
- It verifies the informational step is not treated as a failure and the flow reaches fixture-note evidence before intentionally stopping on incomplete fixture notes.
- It also verifies an explicit malformed-looking notes file is selected by the informational block, reports missing evidence/checklist sections, and still continues to the same fixture-note stop before accepted launch-command printing.
- `Invoke-MvpPreflight.cmd` now runs this regression by default after the daily readiness fixture acceptance notes regression.
- Added `Invoke-MvpPreflight.cmd -SkipDailyReadinessLatestPackageNotesCheck` for focused local loops.

## Verification

- `cmd.exe /c tools\Test-DailyReadinessLatestPackageNotes.cmd`
- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Test-DailyReadinessFixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

## ADRs

No ADR added. This adds terminal-only regression coverage for existing daily readiness package acceptance evidence and does not change cleanup execution, restore execution, persistence, deployment, package acceptance policy, or app behavior.

## Follow-Up

- Keep incomplete and malformed-looking candidate notes informational until the human completes package acceptance and intentionally records any package/current-HEAD mismatch.
