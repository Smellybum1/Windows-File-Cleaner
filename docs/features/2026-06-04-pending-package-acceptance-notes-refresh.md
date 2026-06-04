# Pending Package Acceptance Notes Refresh

Date: 2026-06-04

Status: completed

## Goal

Refresh the pending human acceptance notes for `.local\releases\windows-file-cleaner-v20260604-121922` after docs-only `HEAD` advanced beyond the packaged app commit.

## Safety Profile

`terminal-readonly`. This packet used the existing local release checklist-only path and wrote ignored `.local\release-acceptance` notes only. It did not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, create shortcuts, install anything, or create cleanup history.

## Problem

The first candidate acceptance notes were generated when package commit `e6ac3eb` matched current `HEAD`. After later docs/tooling commits, the candidate package intentionally became behind current docs-only `HEAD`, so the human acceptance pass needed notes that preserve the expected mismatch instead of suggesting current-commit commands.

## Changes

- Generated refreshed pending notes at `.local\release-acceptance\release-acceptance-20260604-134337.md`.
- The refreshed notes stamp exact `-ReleasePath` verifier, checklist, and fixture checklist commands for `.local\releases\windows-file-cleaner-v20260604-121922`.
- The refreshed notes record verifier evidence and intentionally leave commit evidence unrecorded until the human accepts the expected `e6ac3eb` package versus `bb82b29` docs-only `HEAD` mismatch with `-RecordCommitMismatch`.
- The accepted package baseline remains `.local\releases\windows-file-cleaner-v20260602-011556` at `bc9b869`.

## Verification

- `cmd.exe /c tools\Start-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md"`
- Expected incomplete candidate check: `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md" -RequireComplete` exited `1` with the expected missing commit, human launch, overall result, and checklist evidence.
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` still selected the completed accepted `bc9b869` notes.
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly` still printed the accepted `bc9b869` launch command without launching WPF.
- `git diff --check` passed with expected CRLF warnings only.

## ADRs

No ADR added. This refreshes ignored acceptance evidence and committed handoff docs under ADR 0020 portable-package boundaries. It does not change deployment behavior, add shortcuts, install anything, or change cleanup or restore behavior.

## Follow-Up

- Complete the human package acceptance pass for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it.
- Keep using the accepted `bc9b869` package baseline until `.local\release-acceptance\release-acceptance-20260604-134337.md` is completed and passes `Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`.
