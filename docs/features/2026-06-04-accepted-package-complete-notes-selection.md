# Accepted Package Complete Notes Selection

Date: 2026-06-04

Status: completed

## Goal

Keep accepted-package daily commands stable after a newer package candidate writes incomplete ignored acceptance notes.

## Safety Profile

`terminal-readonly`. This packet changes release/acceptance tooling only and verifies ignored local release metadata, ignored local acceptance notes, and Restore Manifest summaries. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, create shortcuts, install anything, or create cleanup history.

## Problem

The verified package candidate `.local\releases\windows-file-cleaner-v20260604-121922` wrote incomplete ignored acceptance notes at `.local\release-acceptance\release-acceptance-20260604-122009.md`. Before this fix, accepted-package commands that selected the latest notes by timestamp could land on that incomplete candidate file instead of the completed accepted baseline notes.

## Changes

- `tools\Summarize-LocalReleaseAcceptanceNotes.ps1` now selects the latest complete acceptance notes when `-RequireComplete` is used without an explicit `-Path`.
- `tools\Start-AcceptedLocalRelease.ps1` now selects the latest complete acceptance notes by default.
- Explicit notes paths are still honored, so pending candidate notes can be inspected and can still fail `-RequireComplete` with their current blockers.
- Added `tools\Test-AcceptedLocalReleaseSelection.cmd` and `.ps1` for clean-runner regression coverage.
- `Invoke-MvpPreflight.cmd` now runs the accepted local release selection regression by default before the package acceptance summary regression.
- Added `Invoke-MvpPreflight.cmd -SkipAcceptedLocalReleaseSelectionCheck` for focused local loops.

## Verification

- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-122009.md"`
- Expected incomplete check: `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-122009.md" -RequireComplete` exited `1` and reported missing normal launch, fixture launch, overall result, and five checklist items.
- Expected incomplete accepted-launcher check: `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -AcceptanceNotesPath ".local\release-acceptance\release-acceptance-20260604-122009.md" -PrintOnly` exited `1` before launch-command printing.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `cmd.exe /c tools\Test-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922"`
- `cmd.exe /c tools\Test-AcceptedLocalReleaseSelection.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

The reusable regression writes temporary ignored notes under `.local\release-acceptance`, writes synthetic print-only package placeholder files under `.local\accepted-release-selection-test`, verifies default accepted launcher selection ignores newer incomplete notes, verifies explicit incomplete notes still stop before launch-command printing, and cleans up its temporary files.

## ADRs

No ADR added. This is a tooling correction under ADR 0020: accepted package launch commands remain the v1 daily path, pending package candidates are not promoted until human acceptance is complete, and no installed shortcut or installer behavior is added.

## Follow-Up

- Complete the human package acceptance pass for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it over the accepted `bc9b869` baseline.
