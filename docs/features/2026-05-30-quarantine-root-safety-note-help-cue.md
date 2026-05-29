# Feature: Quarantine Root Safety Note Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the Quarantine Root Safety Note easier to discover by adding the same visible hoverable `?` help cue used by other safety-context text.

## Non-goals

- Do not change Quarantine Root validation rules.
- Do not create folders, move files, write manifests, restore files, delete files, or approve cleanup.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

- The Quarantine Root Safety Note remains visible below the Quarantine Root Selection.
- A compact circular `?` cue beside the note uses the help cursor and prompt tooltip delay.
- The cue mirrors the dynamic safety-note tooltip/help text for preferred `D:` roots, non-`D:` roots, relative roots, and invalid roots.
- The wording keeps the note as read-only preview-root context, not cleanup approval.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a visible hoverable `?` cue beside the Quarantine Root Safety Note.
- Mirrored dynamic note text into the note tooltip, note automation help text, cue tooltip, and cue automation help text.
- Expanded WPF smoke coverage to ten tracked circular help cues.
- Added default-root and invalid-root assertions for the cue wording.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with line-ending normalization warnings only.

ADRs added or skipped:

- Skipped. This is reversible WPF UI affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm the extra `?` cue improves discovery without crowding the Quarantine Shortlist area.

Risky assumptions:

- Mirroring the safety note in a tooltip is enough feedback; no modal or popup is needed for Quarantine Root Safety Note context.
