# Feature: Confirmation Field Help Cues

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the exact-confirmation field tooltips easier to discover by adding compact circular `?` help cues beside the Quarantine shortlist confirmation and selected restore confirmation fields.

## Non-goals

- Do not change Quarantine Preview, Quarantine execution, selected restore execution, or Restore Manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, real-profile selected restore, permanent deletion, or persisted cleanup history.
- Do not make the cues clickable buttons, popups, modals, or cleanup/restore approval controls.

## Desired behavior

- The shortlist confirmation field has a visible hoverable `?` cue that mirrors the exact `QUARANTINE` tooltip and automation help text.
- The selected restore confirmation field has a visible hoverable `?` cue that mirrors the exact `RESTORE` tooltip and automation help text.
- Both cues use the existing Help cursor and prompt tooltip delay.
- The cues stay non-clickable and preserve fixture-only versus real-profile/custom blocked wording.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Execution Gate | Added `QuarantineConfirmationHelpCue` as visible help for the exact `QUARANTINE` field. | yes |
| Selected Restore Execution Gate | Added `SelectedRestoreConfirmationHelpCue` as visible help for the exact `RESTORE` field. | yes |

## Evidence and validation gate

Evidence gathered:

- User preferred a little question mark in a circle as a tooltip cue.
- Existing app pattern already uses non-clickable circular `?` help cues for safety and gate text.
- Exact-confirmation fields were still primarily tooltip-only.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Lifecycle and safety boundaries remain unchanged.
- [x] Narrow WPF smoke coverage can verify cue tooltip/help text and hover affordance.

## Decisions made

- Place cues directly beside the confirmation labels so the hover affordance is visible before typing exact confirmation text.
- Mirror existing field tooltip/help text instead of adding new wording paths.
- Track both cues in the existing hoverable-help-cue affordance snapshot.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added visible circular `?` help cues beside the shortlist `QUARANTINE` confirmation field and selected restore `RESTORE` confirmation field.
- Mirrored each field's existing tooltip and automation help text onto its cue.
- Expanded WPF smoke coverage from thirteen to fifteen tracked circular help cues.
- Updated README, domain docs, fixture checklist, progress log, and handoff wording.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the updated shortlist confirmation and selected restore confirmation `?` help-cue prompts without preflight, fixture creation, WPF launch, scan, or file modification.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- Later verification packet: `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed; restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output with the new confirmation field `?` help cues, and whitespace diff check all passed without scanning or modifying real user files.

ADRs added or skipped:

- Skipped. This is reversible WPF affordance/help-text polish with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the extra confirmation-field cues improve discovery without crowding the Quarantine Shortlist or selected restore rows.

Risky assumptions:

- Two small exact-confirmation cues are clearer than relying on tooltip-only disabled fields and will not make exact confirmation feel like approval by itself.
