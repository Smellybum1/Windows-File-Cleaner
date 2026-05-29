# Feature: Quarantine Manifest Discovery Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the `Discover manifests` safety boundary easier to discover by adding a visible hoverable `?` cue beside the discovery action.

## Non-goals

- Do not change Quarantine Manifest Discovery behavior.
- Do not change Restore Manifest selection, readiness, selected restore, or all-manifest readiness behavior.
- Do not enable real-profile WPF Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not move, restore, delete, create, write, or clean up files or folders.

## Desired behavior

- The `Discover manifests` row has a circular hoverable `?` cue.
- The cue mirrors the discovery button tooltip and automation help text.
- The cue says discovery is read-only and does not restore, move, delete, clean up folders, or create cleanup history.
- The WPF smoke affordance snapshot tracks the cue's Help cursor and prompt tooltip delay.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Manifest Discovery | Add `QuarantineManifestDiscoveryHelpCue` as a visible WPF affordance for the discovery boundary. | yes |

## Decisions made

- Mirror the existing discovery tooltip/help text instead of adding a second explanation.
- Place the cue beside `Discover manifests`, before all-manifest readiness, because discovery is the manifest-review entry point.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a visible circular `?` help cue beside the WPF `Discover manifests` action.
- Added test-facing accessors and WPF smoke assertions that the cue mirrors the discovery tooltip/help text.
- Expanded the tracked hoverable help-cue affordance snapshot from sixteen to seventeen cues.
- Later packet `2026-05-30-all-manifest-readiness-help-cue.md` added the companion all-manifest readiness cue and expanded the snapshot to eighteen cues.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

ADRs added or skipped:

- Skipped. This is reversible WPF affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm the manifest discovery/readiness row still fits comfortably with the discovery and all-manifest readiness cues.

Risky assumptions:

- Mirroring the existing discovery wording is clearer than adding separate longer help text.
