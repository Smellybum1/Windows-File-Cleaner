# Feature: Fixture Acceptance Current Commit Notes Preview

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Prepare the next visible fixture acceptance pass by generating the checklist-only acceptance notes template from the current pushed commit after the latest full MVP preflight.

## Non-goals

- Do not launch WPF.
- Do not create synthetic fixture files.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, create, or rewrite real-profile files.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not treat ignored `.local` notes as tracked cleanup history.

## User Story / Job Story

As the local app owner, I want the next manual fixture pass to start from notes that stamp the current commit and build context, so that visual acceptance can be tied back to the exact code state already covered by preflight.

## Current Behavior

The latest full `.cmd` MVP preflight passed after the Fixture Acceptance Build Context Header packet and printed the notes-enabled visible fixture command.

The fixture launcher can write ignored `.local` acceptance notes with repository path, branch, commit, .NET SDK, WPF project, target framework, WPF enabled flag, preflight command, visible fixture command, and local evidence checkboxes.

## Desired Behavior

`Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes` should print the current grouped checklist and write an ignored notes template stamped with the current commit and build context, without running preflight, creating fixtures, launching WPF, scanning, moving, restoring, deleting, or creating cleanup history.

## Domain Language Changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing fixture acceptance, Cleanup Scope, preflight, and cleanup history terms used. | n/a |

## Open Questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether the visible fixture pass finds any checklist section that should be shortened or split further.

## Grill Notes

### Scenarios Discussed

- The next acceptance gate remains a visible fixture pass.
- Checklist-only notes generation is local evidence preparation and does not replace visual review.
- Real-profile movement remains unavailable and out of scope.

### Edge Cases

- Notes generation must stay under ignored `.local`.
- Checklist-only mode must still avoid WPF launch and fixture creation.
- The notes header must keep saying that the notes are not cleanup history.

### Dependencies Between Decisions

- Builds on the full preflight after the Fixture Acceptance Build Context Header packet.
- Supports the live-product readiness roadmap without changing ADR 0017, ADR 0018, or ADR 0019.

## Evidence And Validation Gate

Evidence gathered:

- Existing docs inspected:
  - `AGENTS.md`
  - `README.md`
  - `.codex/progress.md`
  - `docs/codex/thread-handoff.md`
  - `docs/domain/context.md`
  - `docs/domain/glossary.md`
  - `docs/features/2026-06-01-live-product-readiness-roadmap.md`
  - `docs/features/2026-06-01-fixture-acceptance-build-context-header.md`
  - `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
  - ADR 0017, ADR 0018, and ADR 0019
- Current Git state before the command was clean and synced on `main`.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or deferred.

Rejected ideas buffer:

- Do not launch WPF from an automatic continuation; keep visible fixture review user-started.
- Do not turn `.local` acceptance notes into tracked cleanup history.

## Decisions Made

Small feature-level decisions:

- Record the checklist-only notes preview as readiness evidence in docs.
- Keep the next visible fixture command unchanged: `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`.

ADR-worthy decisions:

- [x] None.

## Implementation Plan

1. Run the notes-enabled checklist-only launcher from the current clean commit.
2. Inspect the generated notes header.
3. Update progress, handoff, roadmap, and fixture-notes docs.
4. Run whitespace checks and commit.

## Files Expected To Change

Expected:

- `docs/features/2026-06-01-fixture-acceptance-current-commit-notes-preview.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## Test Plan

Manual checks:

- Inspect the generated `.local\fixture-review-acceptance\fixture-acceptance-*.md` header.

Automated checks:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `git diff --check`

## Risks And Assumptions

Risks:

- A generated notes template could be mistaken for completed visual acceptance. The docs keep it labeled as checklist-only preparation.

Assumptions:

- The latest full MVP preflight plus current-commit notes preview is enough preparation before the user starts the visible fixture pass.

## Completion Notes

Completed on: 2026-06-01

What changed:

- Generated checklist-only fixture acceptance notes from current commit `fd8e1d4`.
- Inspected the ignored notes header and confirmed repository, branch, commit, .NET SDK, WPF app project, target framework, WPF enabled flag, preflight command, visible fixture command, preflight/worktree checkboxes, and local-not-cleanup-history wording.
- Updated docs to make the next step the visible fixture pass, not another checklist-only preparation step.

Files changed:

- `docs/features/2026-06-01-fixture-acceptance-current-commit-notes-preview.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- Inspected `.local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md`; the evidence header stamped `main`, commit `fd8e1d4`, `.NET SDK: 8.0.421`, the WPF app project, `net8.0-windows`, `WPF enabled: true`, required preflight, visible fixture command, local evidence checkboxes, and not-cleanup-history wording.
- `git diff --check`

Docs updated:

- This feature brief, fixture acceptance notes brief, live-product roadmap, progress log, and thread handoff.

ADRs added or skipped:

- No ADR added. This is local fixture-review evidence preparation with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Follow-up work:

- Run the visible fixture pass with `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` when the user is ready.
- Record manual results or issues from the visible pass in `.codex/progress.md`.

Open questions:

- Whether the visible pass finds any fixture checklist section that should be shortened or split.

Risky assumptions:

- Current-commit notes preview helps manual acceptance without implying the visual pass has already happened.
