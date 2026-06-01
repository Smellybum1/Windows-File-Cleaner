# Feature: Fixture Checklist Section Grouping

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make the manual fixture acceptance checklist easier to scan by grouping the existing numbered checks around the visible fixture review flow.

## Non-goals

- Do not change checklist safety wording or remove checks.
- Do not launch WPF in automated checks.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, create, or rewrite real-profile files.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.

## Current Behavior

`Start-MvpFixtureReview.cmd -ChecklistOnly` prints ten long checklist items. `-WriteAcceptanceNotes` writes the same items into a local ignored notes template.

The items are current, but the output reads like one long wall before a manual pass through the tabbed WPF workflow.

## Desired Behavior

The launcher should keep the same numbered checks and safety wording while grouping them into fixture-review sections:

- Scan header and gate
- Safety Summary, Review, and Main Grid
- Quarantine Preview and fixture Quarantine
- Restore Manifest review and selected restore
- Real-profile and custom blockers

The generated acceptance notes should use the same section grouping so the next manual fixture pass can be recorded by workflow area.

## Domain Language Changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing fixture review, Cleanup Scope, Quarantine, Restore Manifest, and real-profile blocker terms used. | n/a |

## Evidence And Validation Gate

Evidence gathered:

- User visually approved the tabbed/header UI direction.
- The live-product roadmap keeps visible fixture acceptance as the next gate.
- The previous acceptance notes template worked, but its checklist blocks were not grouped by workflow area.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or deferred.

## Decisions Made

Small feature-level decisions:

- Keep the original checklist item text and numbering.
- Add a small index-to-section mapping in the launcher instead of reshaping the checklist into a heavier data model.
- Use the same sections for terminal output and acceptance notes.

ADR-worthy decisions:

- [x] None.

## Implementation Plan

1. Add section mapping to `Start-MvpFixtureReview.ps1`.
2. Print section headers in checklist output.
3. Add section headings to generated acceptance notes.
4. Update README, feature notes, progress, and handoff docs.
5. Run focused launcher and diff checks.

## Test Plan

Automated checks:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `git diff --check`

## Completion Notes

Completed on: 2026-06-01

What changed:

- Added grouped section headers to the fixture checklist output.
- Added matching section headings to generated acceptance notes.
- Kept the ten numbered checklist prompts unchanged.

Files changed:

- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-checklist-section-grouping.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `git diff --check`

Docs updated:

- README, fixture checklist/checklist-only/acceptance-notes feature briefs, live-product roadmap, progress log, and thread handoff.

ADRs added or skipped:

- No ADR added. This is fixture-review workflow output polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Follow-up work:

- Run the visible fixture pass with `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` after a successful preflight when the user is ready.
- Use the sectioned local notes to record pass/issue/not-checked results.

Open questions:

- After a real manual pass, should any section be split or shortened further?

Risky assumptions:

- Section headers reduce manual-review friction without hiding any safety-critical prompt.
