# Feature: Review Mix Help Cues

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make Review Mix and Matched Review Mix help easier to discover by adding small visible `?` cues next to the summary lines.

## Non-goals

- Do not change Storage Scan results, filters, search, focus actions, Review Shortlist membership, Quarantine Preview, fixture execution, undo, restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not add a popup, modal, new summary row, or clickable action.

## Desired behavior

- Review Mix and Matched Review Mix keep their compact summary text.
- Each line has a small circular `?` cue that exposes the same tooltip and automation help text as the summary.
- The cue reads as help context only, not as cleanup approval or an action.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Review Mix | Clarified visible help cue behavior. | yes |
| Matched Review Mix | Clarified visible help cue behavior. | yes |

## Evidence and validation gate

Evidence gathered:

- User said a little question mark in a circle that can be hovered over for the tooltip would be better than relying on hidden text hover alone.
- Existing Review Mix help text already mirrors the dynamic summaries and carries the safety boundary.
- Existing WPF smoke tests already exercise startup, completed scan, descendant focus, and prefixed search states.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added visible circular `?` help cues beside Review Mix and Matched Review Mix.
- Mirrored dynamic Review Mix and Matched Review Mix help text onto both cues.
- Added WPF smoke assertions for cue automation names, cue tooltips, and cue automation help text.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `docs/features/2026-05-29-matched-review-mix.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-review-mix-help-text.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the `?` cues are visually noticeable without crowding the dense review surface.

Risky assumptions:

- A compact non-clickable `?` cue is more discoverable than hidden hover text and less disruptive than adding another visible help row.
