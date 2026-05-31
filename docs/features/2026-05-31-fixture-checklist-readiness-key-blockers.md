# Feature: Fixture Checklist Readiness Key Blockers

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Keep the printed fixture review checklist aligned with the compact Quarantine Readiness Summary key-blocker wording.

## Non-goals

- Do not change WPF behavior.
- Do not enable real-profile Quarantine execution.
- Do not enable real-profile Undo Quarantine, selected real-profile restore, permanent deletion, or cleanup history.
- Do not scan, move, restore, delete, create, write, or clean up real-profile files.

## Implementation

- Updated `Start-MvpFixtureReview.ps1` checklist step 6 so reviewers check preview-only `movement unavailable` wording and specific `Key blockers` labels in the compact Quarantine Readiness Summary.
- Updated README manual review wording with the same expectation, including `custom scope preview-only`, `exact profile scope`, `pre-execution revalidation`, `restore readiness`, `current build unavailable`, `10-row cap`, `1 GB cap`, `no-category rows`, and `strict descendant checks`.
- Left `AGENTS.md` unchanged.

## Test plan

Automated checks:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Manual fixture review prompts now call out compact summary key-blocker labels alongside the existing preview-only and tooltip/help-text boundaries.
- Later checklist refinement named the specific preview-only key labels covered by WPF smoke tests so the manual review prompt matches the current compact-summary vocabulary.

ADRs:

- No ADR added. This is checklist wording alignment for existing WPF output under accepted ADR 0018.

Follow-up work:

- Run the next visible fixture review and decide whether the compact summary is clear enough without a dedicated readiness pane.

Open questions:

- None for this checklist packet.

Risky assumptions:

- Calling out specific `Key blockers` labels in the checklist helps manual review without overloading the fixture prompt.
