# Fixture Acceptance Notes Regression

Date: 2026-06-04

Status: completed

## Goal

Cover fixture acceptance notes summary and recorder behavior in default MVP preflight so formal fixture notes stay human-owned and completion-checked.

## Safety Profile

`terminal-readonly`. The regression writes temporary ignored fixture acceptance notes under `.local\fixture-acceptance-notes-test`, runs summary and recorder tooling against those notes, and removes the test folder. It does not launch WPF, create fixtures, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, create shortcuts, install anything, or create cleanup history.

## Problem

Fixture acceptance notes already had summary and recorder helpers, but their completion blockers and manual-recording boundary were not covered by default MVP preflight. That left a human-owned acceptance path less protected than the package acceptance notes path.

## Changes

- Added `tools\Test-FixtureAcceptanceNotes.cmd` and `.ps1`.
- The regression verifies incomplete fixture notes summaries print the exact recorder guidance and that `-RequireComplete` reports preflight, worktree, overall result, and checklist blockers.
- The regression verifies `Record-FixtureAcceptanceNotes.cmd` requires `-RecordManualAcceptance`.
- The regression verifies `-WhatIf` leaves notes unchanged.
- The regression verifies explicit manual acceptance recording completes synthetic ignored notes and passes `Summarize-FixtureAcceptanceNotes.cmd -RequireComplete`.
- `Invoke-MvpPreflight.cmd` now runs the fixture acceptance notes regression by default after the fixture checklist.
- Added `Invoke-MvpPreflight.cmd -SkipFixtureAcceptanceNotesCheck` for focused local loops.

## Verification

- `cmd.exe /c tools\Test-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

## ADRs

No ADR added. This adds terminal-only regression coverage for existing fixture acceptance notes tooling and does not change cleanup execution, restore execution, persistence, deployment, package acceptance policy, or app behavior.

## Follow-Up

- Keep using `Record-FixtureAcceptanceNotes.cmd -RecordManualAcceptance` only after an actual all-pass visible fixture review.
