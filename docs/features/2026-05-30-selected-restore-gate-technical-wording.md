# Feature: Selected Restore Gate Technical Wording

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Remove technical `Execution implemented` wording from the visible Selected Restore Execution Gate pane now that fixture-only versus preview-only behavior is explained by plain scope-status and approval-boundary lines.

## Non-goals

- Do not change Selected Restore Execution Gate semantics.
- Do not change fixture-only selected restore execution.
- Do not enable real-profile or custom selected restore.
- Do not change Quarantine Preview, fixture Quarantine execution, current-fixture undo, manifest discovery, restore readiness, permanent deletion, or cleanup history.

## Desired behavior

- Selected restore gate output shows selected manifest, restorable counts, required `RESTORE`, confirmation-match state, `Execution scope status`, `Approval boundary`, and `Can execute`.
- Selected restore gate output does not show the technical `Execution implemented` field.
- Fixture selected restore can still execute only after selected readiness, exact `RESTORE`, fixture scope, and no blockers.
- Real-profile/custom selected restore stays preview-only and unavailable.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- `docs/features/2026-05-29-selected-restore-scope-status.md` left open whether the selected restore gate should eventually hide the technical line.
- Current WPF output already includes plain `Execution scope status`, `Approval boundary`, and `Can execute` lines.
- README manual checks already ask the user to verify fixture-only scope status, approval boundary, and `Can execute`, not implementation internals.

Validation gate before implementation:

- [x] Selected Restore Execution Gate terminology is already defined.
- [x] The change is user-facing wording only.
- [x] The narrowest relevant verification path is WPF app smoke tests plus diff check.

## Completion notes

Completed on: 2026-05-30

What changed:

- Removed `Execution implemented` from WPF Selected Restore Confirmation Draft/Gate display text.
- Updated WPF smoke assertions to prove selected restore panes use plain scope-status wording while fixture selected restore and custom blockers still behave the same.
- Updated domain and feature notes to close the old follow-up.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

ADRs added or skipped:

- Skipped. This is reversible WPF wording polish and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Risky assumptions:

- `Execution scope status`, `Approval boundary`, and `Can execute` are clearer than exposing the internal implementation flag in the selected restore pane.
