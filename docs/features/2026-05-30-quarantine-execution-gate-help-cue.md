# Feature: Quarantine Execution Gate Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Add a visible hoverable `?` help cue beside the WPF Quarantine Execution Gate so its tooltip/help text is easier to discover than hidden control hover alone.

## Non-goals

- Do not change Quarantine Preview eligibility.
- Do not change Quarantine Execution Gate behavior.
- Do not change fixture execution, undo, selected restore, manifests, or scan behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

- The Quarantine Execution Gate readout has a small circular `?` cue.
- The cue uses the same hover affordance as the existing help cues.
- The cue tooltip and automation help text explain the current gate state, exact `QUARANTINE` boundary, fixture-only execution, real-profile/custom blockers, and not-cleanup-approval boundary.
- Smoke coverage tracks the cue in the shared hoverable-help-cue affordance list.

## Domain language changes

No new domain terms. The existing Quarantine Execution Gate term now notes its visible `?` help cue.

## Evidence and validation gate

Evidence gathered:

- User feedback favored a small circular `?` help cue as a clearer hover target for important gate tooltip text.
- Execution-control tooltip and automation help text already existed, but disabled controls can still be visually easy to miss.
- Existing WPF patterns use non-clickable circular `?` cues with Help cursor and prompt tooltip delay.

Validation gate before implementation:

- [x] This is WPF affordance/help text only.
- [x] Cleanup execution availability and filesystem behavior remain unchanged.
- [x] The narrowest relevant verification path is WPF app smoke tests plus diff check.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a visible circular `?` cue beside the Quarantine Execution Gate readout.
- Added dynamic gate help text for startup, closed, open, executed, and undone states.
- Mirrored the gate help text onto the readout tooltip, automation help text, and help cue.
- Expanded hoverable-help-cue smoke coverage from eleven to twelve tracked cues.
- Updated the fixture checklist to call out the new Quarantine Execution Gate help cue.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`

ADRs added or skipped:

- Skipped. This is reversible WPF affordance/help-text polish and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- A visible fixture review should confirm the new cue is clear without crowding the Quarantine Shortlist panel.

Risky assumptions:

- A visible cue near the gate text is more discoverable than relying on disabled-control tooltips alone.
