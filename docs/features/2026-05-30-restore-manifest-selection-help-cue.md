# Feature: Restore Manifest Selection Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the Restore Manifest selection boundary easier to discover by adding a visible hoverable `?` cue beside the selected-manifest control.

## Non-goals

- Do not change Quarantine Manifest Discovery behavior.
- Do not change Restore Manifest selection, readiness, selected restore, or all-manifest readiness behavior.
- Do not enable real-profile WPF Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not move, restore, delete, create, write, or clean up files or folders.

## Desired behavior

- The selected-manifest row has a circular hoverable `?` cue.
- The cue mirrors the Restore Manifest selection tooltip and automation help text.
- The cue says selection is read-only review, not restore approval, and does not move, restore, delete, write manifests, or clean up folders.
- The WPF smoke affordance snapshot tracks the cue's Help cursor and prompt tooltip delay.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Restore Manifest Review | Add `RestoreManifestSelectionHelpCue` as a visible WPF affordance for the selection boundary. | yes |

## Decisions made

- Mirror the existing selection tooltip/help text instead of inventing another explanation.
- Keep this cue near `Selected manifest` because selection is the first step toward selected readiness and fixture-only selected restore.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a visible circular `?` help cue beside the WPF Restore Manifest selection control.
- Added test-facing accessors and WPF smoke assertions that the cue mirrors the selection tooltip/help text.
- Expanded the tracked hoverable help-cue affordance snapshot from fifteen to sixteen cues.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

ADRs added or skipped:

- Skipped. This is reversible WPF affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm the selected-manifest row still fits comfortably with the new cue.
- Decide later whether `Discover manifests` also needs a visible help cue.

Risky assumptions:

- Mirroring the existing selection wording is clearer than adding separate longer help text.
