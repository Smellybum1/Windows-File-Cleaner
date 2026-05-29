# Feature: All-Manifest Readiness Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the `Preview all-manifest readiness` safety boundary easier to discover by adding a visible hoverable `?` cue beside the readiness action.

## Non-goals

- Do not change Restore Readiness Preview behavior.
- Do not change Quarantine Manifest Discovery, Restore Manifest selection, selected restore, or all-manifest readiness output.
- Do not enable real-profile WPF Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not move, restore, delete, create, write, or clean up files or folders.

## Desired behavior

- The `Preview all-manifest readiness` row has a circular hoverable `?` cue.
- The cue mirrors the readiness button tooltip and automation help text.
- The cue says readiness is read-only all-manifest blocker evidence and no files are restored.
- The WPF smoke affordance snapshot tracks the cue's Help cursor and prompt tooltip delay.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Restore Readiness Preview | Add `AllManifestReadinessHelpCue` as a visible WPF affordance for the all-manifest readiness boundary. | yes |

## Decisions made

- Mirror the existing all-manifest readiness tooltip/help text instead of adding separate wording.
- Place the cue beside `Preview all-manifest readiness` because the action reads restore-adjacent but remains read-only.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a visible circular `?` help cue beside the WPF `Preview all-manifest readiness` action.
- Added test-facing accessors and WPF smoke assertions that the cue mirrors the readiness tooltip/help text.
- Expanded the tracked hoverable help-cue affordance snapshot from seventeen to eighteen cues.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

ADRs added or skipped:

- Skipped. This is reversible WPF affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm the manifest discovery/readiness row still fits comfortably with the added cue.

Risky assumptions:

- Mirroring the existing all-manifest readiness wording is clearer than adding separate longer help text.
