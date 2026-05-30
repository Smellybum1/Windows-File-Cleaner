# Feature: Manifest Review Control Grouping

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Keep manifest-review `?` help cues visually paired with their related controls when the WPF row wraps.

## Non-goals

- Do not change Quarantine Manifest Discovery behavior.
- Do not change Restore Readiness Preview behavior.
- Do not change Restore Manifest selection, selected restore, fixture Quarantine execution, undo, or all-manifest readiness output.
- Do not enable real-profile WPF Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not move, restore, delete, create, write, or clean up files or folders.

## Desired behavior

- `Discover manifests` wraps together with its `?` help cue.
- `Preview all-manifest readiness` wraps together with its `?` help cue.
- `Selected manifest`, its `?` help cue, and the Restore Manifest picker wrap as a group.
- The existing tooltips, automation help text, and WPF smoke affordance coverage remain unchanged.

## Decisions made

- Use small horizontal `StackPanel` groups inside the existing wrapping rows, so the layout still wraps but does not orphan a help cue.
- Keep this as layout/readability polish only; no new domain term or ADR is needed.

## Completion notes

Completed on: 2026-05-30

What changed:

- Grouped the manifest discovery button with its `?` cue.
- Grouped the all-manifest readiness button with its `?` cue.
- Grouped the selected-manifest label, `?` cue, and selector.
- Updated manual review docs to check that cue/control pairs stay together when rows wrap.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

ADRs added or skipped:

- Skipped. This is reversible WPF layout polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm the grouped manifest controls fit comfortably at normal window sizes.

Risky assumptions:

- Grouping each control with its cue improves readability more than preserving maximum per-child wrap flexibility.
