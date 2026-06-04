# Daily Readiness Latest Package Notes Regression

Date: 2026-06-04

Status: completed

## Goal

Cover daily readiness latest package notes visibility so a newer incomplete candidate remains informational and does not replace completed accepted package evidence.

## Safety Profile

`terminal-readonly`. The regression writes temporary ignored package acceptance notes under `.local\release-acceptance`, fixture acceptance notes and an empty synthetic Restore Manifest root under `.local\daily-readiness-latest-package-notes-test`, runs daily readiness with explicit accepted notes, and removes the test files. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

## Problem

Daily readiness already showed latest package acceptance notes as informational context, but default preflight did not directly cover the composition where accepted evidence uses completed notes while the latest notes file is newer and incomplete.

## Changes

- Added `tools\Test-DailyReadinessLatestPackageNotes.cmd` and `.ps1`.
- The regression creates older complete accepted notes and newer incomplete candidate notes under ignored `.local\release-acceptance`.
- It runs daily readiness with the complete accepted notes as explicit accepted evidence.
- It verifies the informational latest-notes block selects the newer incomplete notes and prints guarded pending next steps including `-RecordCommitMismatch`.
- It verifies the informational step is not treated as a failure and the flow reaches fixture-note evidence before intentionally stopping on incomplete fixture notes.
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

- Keep incomplete candidate notes informational until the human completes package acceptance and intentionally records any package/current-HEAD mismatch.
