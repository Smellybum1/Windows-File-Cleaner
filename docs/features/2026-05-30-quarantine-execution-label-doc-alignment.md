# Feature: Quarantine Execution Label Docs Alignment

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Align current-facing manual-review docs with the visible `Quarantine included shortlist` action label.

## Non-goals

- Do not rename code symbols.
- Do not change Quarantine Preview, fixture execution, undo, selected restore, manifests, or scan behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

- Current manual-check wording points to `Quarantine included shortlist`, matching the WPF button and smoke-test assertions.
- Older feature-history notes may still mention the previous generic label when they describe the label before shortlist-level wording shipped.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- WPF XAML and smoke tests assert `Quarantine included shortlist`.
- README and progress next-review prompts still had a few current-facing references to the older generic label.

Validation gate before implementation:

- [x] This is wording alignment only.
- [x] The visible label is already implemented and tested.
- [x] The narrowest relevant check is fixed-string search plus `git diff --check`.

## Completion notes

Completed on: 2026-05-30

What changed:

- Updated current-facing README, progress, handoff, domain, and feature-note wording to use `Quarantine included shortlist`.
- Left historical feature notes alone where they explain the earlier label that was replaced.

Tests run:

- `rg -n --fixed-strings "Execute quarantine" README.md docs\codex\thread-handoff.md docs\domain\context.md docs\features\2026-05-29-execution-control-tooltip-clarity.md docs\features\2026-05-29-execution-readiness-automation-help-text.md`
- `git diff --check`

ADRs added or skipped:

- Skipped. This is docs wording alignment only.

Open questions:

- None.

Risky assumptions:

- Leaving internal code identifiers unchanged is clearer than creating rename churn for a user-facing wording-only packet.
