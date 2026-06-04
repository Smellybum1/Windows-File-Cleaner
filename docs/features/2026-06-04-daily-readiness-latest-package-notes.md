# Daily Readiness Latest Package Notes

Date: 2026-06-04

Status: completed

## Goal

Make the pending package acceptance state visible from the daily terminal readiness command without promoting the pending candidate or launching WPF.

## Safety Profile

`terminal-readonly`. This packet changes terminal output and docs only. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, create shortcuts, install anything, or create cleanup history.

## Problem

`Invoke-DailyLocalReadiness.cmd` verified the completed accepted package baseline and printed accepted launch commands, but the current pending candidate notes were only visible if the human already knew the explicit notes path. That made the next package acceptance step easier to miss during the daily check.

## Changes

- Daily readiness now prints an informational `Latest package acceptance notes` block after the completed accepted package evidence block.
- The accepted package evidence block still uses `-RequireComplete`, so incomplete candidate notes do not replace the accepted `bc9b869` baseline.
- The latest-notes block is non-fatal; if a future ignored notes file is malformed, accepted package readiness still relies on completed accepted notes only.
- The command header now says the latest notes block is informational only.

## Verification

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd` passed. It verified the completed accepted `bc9b869` notes, showed the latest incomplete candidate notes as informational context, printed accepted normal and fixture launch commands in print-only mode, and printed the read-only Restore Manifest summary.
- `git diff --check` passed with expected CRLF warnings only.

## ADRs

No ADR added. This keeps ADR 0020 intact: accepted package commands remain the daily path, no shortcuts or installers are created, and incomplete candidate notes remain human-acceptance evidence only.

## Follow-Up

- Complete human acceptance for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it.
