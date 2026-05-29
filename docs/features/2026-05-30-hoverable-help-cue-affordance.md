# Feature: Hoverable Help Cue Affordance

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the existing circular `?` help cues feel more obviously hoverable by using the Windows help cursor and a short tooltip delay.

## Non-goals

- Do not change Storage Scan results, filters, search, Review Shortlist membership, Quarantine Preview, fixture execution, undo, restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not turn help cues into buttons, approval controls, popups, or modal help.

## Desired behavior

- Existing circular `?` help cues keep the same tooltip and automation help text.
- Hovering a cue uses a help cursor and shows the tooltip promptly.
- The cues remain compact, read-only, and visually separate from cleanup approval.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added `Cursor="Help"` and a short tooltip initial delay to the seven existing circular `?` help cues.
- Preserved the existing dynamic tooltip/help-text wiring and all safety wording.
- Later packet `2026-05-30-fixture-checklist-hoverable-help-cues.md` aligned the manual fixture checklist wording with the hoverable cue behavior.
- Later packet `2026-05-30-hoverable-help-cue-affordance-coverage.md` added WPF smoke coverage for the Help cursor and prompt tooltip delay on all seven circular `?` cues.
- Later packet `2026-05-30-real-profile-acknowledgement-help-cue.md` added an eighth tracked circular `?` cue for the real-profile acknowledgement.
- Later packet `2026-05-30-scan-gate-summary-help-cue.md` added a ninth tracked circular `?` cue for the scan-gate summary.
- Later packet `2026-05-30-quarantine-root-safety-note-help-cue.md` added a tenth tracked circular `?` cue for the Quarantine Root Safety Note.
- Later packet `2026-05-30-cleanup-scope-safety-note-help-cue.md` added an eleventh tracked circular `?` cue for the Cleanup Scope Safety Note.
- Later packets added the Quarantine Execution Gate, Selected Restore Execution Gate, shortlist confirmation, Quarantine Manifest Discovery, Restore Manifest selection, all-manifest readiness, and selected restore confirmation cues; the WPF smoke affordance snapshot now tracks eighteen circular `?` cues.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

ADRs added or skipped:

- Skipped. This is reversible WPF UI affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the help cursor and faster tooltip feel clear without making the dense UI feel noisy.

Risky assumptions:

- A help cursor and prompt tooltip are enough to make the existing `?` cues discoverable without adding larger labels.
