# Local Release Acceptance Command Stamping

Date: 2026-06-04

Status: completed

## Goal

Make generated portable release acceptance notes stamp the actual verifier, checklist, and recorder commands for the package context that created the notes.

## Safety Profile

`terminal-readonly`. This packet changes release tooling and writes only ignored test notes under `.local\release-acceptance`, then removes those generated test notes after validation. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, create shortcuts, install anything, or create cleanup history.

## Problem

Generated package acceptance notes could still show hard-coded `-RequireCurrentCommit` verifier and checklist commands even when the notes were intentionally created for a verified package candidate that is behind later docs/tooling commits.

That wording made the correct behind-HEAD candidate workflow harder to follow, even after the recorder required explicit `-RecordCommitMismatch` evidence.

## Changes

- `Start-LocalRelease.ps1` now formats release-tool commands with quoted arguments when paths contain spaces.
- Generated notes now include `-ReleasePath "<release folder>"` for the required verifier, checklist command, and fixture checklist command.
- Generated notes include `-RequireCurrentCommit` only when the notes were created with that switch.
- Behind-current-HEAD notes now include a package/current-HEAD mismatch note plus the exact `-RecordCommitMismatch` recorder command.
- The printed post-pass next steps now show the exact recorder, mismatch recorder, summary, and completion-check commands for the generated notes path.

## Verification

- `cmd.exe /c tools\Start-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -ChecklistOnly -WriteAcceptanceNotes`
- Parsed the generated ignored notes and verified release-path command stamping, `-RecordCommitMismatch` guidance, the package/current-HEAD mismatch note, and no stale `-RequireCurrentCommit` in the required/checklist command lines.
- Removed the generated ignored test notes after confirming the generated path stayed under `.local\release-acceptance`.
- `cmd.exe /c tools\Start-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -ChecklistOnly -WriteAcceptanceNotes -SkipVerify -RequireCurrentCommit`
- Parsed the generated ignored notes and verified `-RequireCurrentCommit` command stamping, no mismatch note, and no `-RecordCommitMismatch` recorder command.
- Removed the generated current-commit test notes after confirming the generated path stayed under `.local\release-acceptance`.
- `git diff --check`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`

## ADRs

No ADR added. This is a release-acceptance tooling correction under ADR 0020 and does not add installed shortcut, installer, cleanup, restore, deletion, persistence, or history behavior.

## Follow-Up

- Complete the human package acceptance pass for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it over the accepted `bc9b869` baseline.
