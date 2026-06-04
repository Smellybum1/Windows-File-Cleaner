# Package Acceptance Summary Next Steps

Date: 2026-06-04

Status: completed

## Goal

Make incomplete portable package acceptance summaries print the exact next commands while keeping package acceptance human-owned.

## Safety Profile

`terminal-readonly`. This packet changes terminal output and docs only. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, create shortcuts, install anything, promote a package, or create cleanup history.

## Problem

The pending package notes summary showed missing evidence, but the human still had to find the recorder and recheck commands elsewhere. Daily readiness now surfaces those latest notes, so the summary itself should carry the next-step commands without implying Codex can record acceptance.

## Changes

- `Summarize-LocalReleaseAcceptanceNotes.cmd` now prints `Pending acceptance next steps` when the selected notes are incomplete.
- The next-step block prints the guarded recorder command only after verifier evidence is recorded.
- If commit evidence is not recorded, the block prints the `-RecordCommitMismatch` recorder command and says to use it only after accepting the package/current-HEAD mismatch.
- The block also prints the explicit summary and `-RequireComplete` recheck commands.
- Completed accepted notes do not print pending next-step commands.

## Verification

- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md"`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- Expected incomplete candidate check: `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md" -RequireComplete` exited `1`.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `git diff --check` passed with expected CRLF warnings only.

## ADRs

No ADR added. This preserves ADR 0020 portable-package boundaries by adding terminal guidance only; it does not create shortcuts, installers, acceptance records, or package promotion behavior.

## Follow-Up

- Complete human package acceptance for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it.
- Later packet `2026-06-04-local-release-acceptance-summary-regression-check.md` added targeted temporary-note regression coverage for this summary behavior.
