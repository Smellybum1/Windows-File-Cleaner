# Feature: Cleanup Scope Safety Note Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the Cleanup Scope Safety Note easier to discover by adding the same visible hoverable `?` help cue used by other safety-context text.

## Non-goals

- Do not change Cleanup Scope classification rules.
- Do not change the Cleanup Scope Scan Gate or real-profile acknowledgement requirement.
- Do not start scans, run preflight, create fixtures, persist approvals, move files, restore files, delete files, or approve cleanup.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

- The Cleanup Scope Safety Note remains visible below the Cleanup Scope Selection controls.
- A compact circular `?` cue beside the note uses the help cursor and prompt tooltip delay.
- The cue mirrors the dynamic safety-note tooltip/help text for real-profile, fixture, and custom scopes.
- The wording keeps the note as read-only scope context, not scan approval or cleanup approval.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a visible hoverable `?` cue beside the Cleanup Scope Safety Note.
- Mirrored dynamic note text into the note tooltip, note automation help text, cue tooltip, and cue automation help text.
- Expanded WPF smoke coverage to eleven tracked circular help cues.
- Added real-profile, fixture, and custom-scope assertions for the cue wording.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with line-ending normalization warnings only.
- Later packet `2026-05-30-full-local-mvp-preflight-after-cleanup-scope-cue` ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, and whitespace diff check all passed without scanning or modifying real user files.

ADRs added or skipped:

- Skipped. This is reversible WPF UI affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm the header remains readable with both the Cleanup Scope Safety Note cue and the scan-gate cue.

Risky assumptions:

- Mirroring the safety note in a tooltip is enough; no modal, popup, or extra scan gate is needed for this note.
