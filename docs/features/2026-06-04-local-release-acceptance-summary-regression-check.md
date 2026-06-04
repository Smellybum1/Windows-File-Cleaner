# Local Release Acceptance Summary Regression Check

Date: 2026-06-04

Status: completed

## Goal

Add a targeted terminal-only regression check for portable release acceptance summary output, especially the guarded next-step guidance and default completed-notes selection when incomplete or malformed-looking notes exist.

## Safety Profile

`terminal-readonly`. The check writes temporary ignored complete, incomplete, and malformed-looking test notes under `.local\release-acceptance-summary-test` and `.local\release-acceptance`, runs the read-only summary helper, and removes those test notes. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete real-profile files, approve cleanup, create shortcuts, install anything, promote a package, or create cleanup history.

## Problem

The summary helper now prints guarded next-step commands for incomplete package acceptance notes. That behavior is important because daily readiness surfaces the latest candidate notes, but it was only verified manually against the current local notes. A follow-up also covered malformed-looking notes so missing checklist structure is reported as missing instead of all-pass.

## Changes

- Added `tools\Test-LocalReleaseAcceptanceSummary.cmd`.
- Added `tools\Test-LocalReleaseAcceptanceSummary.ps1`.
- The test synthesizes incomplete and complete acceptance notes under ignored `.local`.
- It verifies incomplete notes print `Pending acceptance next steps`, `-RecordCommitMismatch`, and the no-launch/no-scan/no-cleanup-history boundary.
- It verifies malformed-looking notes report missing evidence, zero checklist sections, guarded verifier guidance, no recorder command while verifier evidence is missing, and `-RequireComplete` blockers.
- It verifies default `Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` skips newer incomplete and malformed-looking notes under `.local\release-acceptance` and selects the latest complete notes.
- It verifies complete notes pass `-RequireComplete` and do not print pending next steps.
- The test removes its generated notes and test folder when complete.
- MVP preflight now runs this regression by default before the local release acceptance recorder regression.
- `Invoke-MvpPreflight.cmd -SkipLocalReleaseAcceptanceSummaryCheck` can skip only this regression for focused local loops.

## Verification

- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- Confirmed `.local\release-acceptance-summary-test` was absent after cleanup.
- `git diff --check` passed with expected CRLF warnings only.

## ADRs

No ADR added. This adds regression coverage for existing terminal tooling and does not change product behavior, cleanup execution, restore execution, persistence, deployment, or package acceptance policy.

## Follow-Up

- Keep using this targeted check after changes to `Summarize-LocalReleaseAcceptanceNotes.ps1` or package acceptance note structure.
- The normal MVP preflight path now covers the same regression before real-profile scan review evidence.
