# Local Release Recorder Commit Evidence Guard

Date: 2026-06-04

Status: completed

## Goal

Make portable release acceptance recording safer when a package candidate is intentionally behind current `HEAD` because later commits are docs/tooling-only.

## Safety Profile

`terminal-readonly` for verification plus ignored-note test copies. The recorder updates ignored `.local` notes only when explicitly invoked. This packet does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, create shortcuts, install anything, or create cleanup history.

## Problem

`Record-LocalReleaseAcceptanceNotes.cmd -RecordManualAcceptance` could mark manual launch and checklist evidence while leaving verifier or commit evidence missing. That made it possible to produce notes that looked recorded in terminal output but still failed `Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`.

Package candidates can also intentionally differ from current `HEAD` after docs-only commits. The notes label already allowed "commit matched current HEAD or mismatch was intentionally recorded", but the recorder did not expose a safe explicit way to mark that mismatch evidence.

## Changes

- `Record-LocalReleaseAcceptanceNotes.ps1` now refuses to record manual acceptance unless verifier evidence is already recorded.
- If commit evidence is not already recorded, the recorder now requires explicit `-RecordCommitMismatch` before it marks the commit evidence checkbox.
- `Start-LocalRelease.ps1` now prints and embeds the recorder command in generated notes, plus the explicit `-RecordCommitMismatch` variant when notes are created without `-RequireCurrentCommit`.
- README and the portable release runbook now document when to use `-RecordCommitMismatch`.

## Verification

- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-missing-verifier.md" -RecordManualAcceptance`
- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-missing-commit.md" -RecordManualAcceptance`
- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-missing-commit.md" -RecordManualAcceptance -RecordCommitMismatch -Summary "Test-only recorded portable release acceptance with explicit commit mismatch evidence."`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-missing-commit.md" -RequireComplete`
- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-missing-commit.md" -RecordManualAcceptance -RecordCommitMismatch -WhatIf`
- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd` exited `1` before writing because `-RecordManualAcceptance` was missing.
- `cmd.exe /c tools\Start-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -ChecklistOnly -WriteAcceptanceNotes`; the generated ignored test notes contained `-RecordCommitMismatch` guidance and were removed after verifying the generated path stayed under `.local\release-acceptance`.
- `git diff --check`

## ADRs

No ADR added. This is a release-acceptance tooling guard under ADR 0020 and does not add installed shortcut or installer behavior.

## Follow-Up

- Complete the human package acceptance pass for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it over the accepted `bc9b869` baseline.
