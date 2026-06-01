# Feature: Fixture Notes Launcher Wording Alignment

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make the manual fixture acceptance docs consistently point to the post-preflight notes-enabled launcher command.

## Non-goals

- Do not launch WPF.
- Do not create fixture files.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, create, or rewrite real-profile files.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.

## Current Behavior

Some current-facing docs still said to run `.\tools\Start-MvpFixtureReview.cmd -WriteAcceptanceNotes` for the visible fixture pass, while preflight now prints `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`.

Both commands are valid in different contexts, but the current next step after successful preflight should avoid rerunning preflight.

## Desired Behavior

Current-facing next-step wording should recommend:

```powershell
.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes
```

The older no-`SkipPreflight` form can remain described as a normal launcher capability, but not as the preferred post-preflight next step.

## Domain Language Changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing fixture review, MVP preflight, and Cleanup Scope language used. | n/a |

## Evidence And Validation Gate

Evidence gathered:

- `Invoke-MvpPreflight.cmd` now prints the notes-enabled `-SkipPreflight` launcher command.
- README, the live-product roadmap, and fixture-notes feature briefs still had a few current-facing no-`SkipPreflight` references.
- The next visible fixture pass is the current manual acceptance gate.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or deferred.

## Decisions Made

Small feature-level decisions:

- Keep the normal `-WriteAcceptanceNotes` capability documented where it describes launcher behavior.
- Use `-SkipPreflight -WriteAcceptanceNotes` for current post-preflight next-step wording.
- Keep this as docs/workflow alignment only.

ADR-worthy decisions:

- [x] None.

## Implementation Plan

1. Update README current-facing fixture notes wording.
2. Update live-product roadmap and existing fixture notes feature briefs.
3. Record the packet in progress and handoff docs.
4. Run focused checklist and whitespace checks.

## Test Plan

Automated checks:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `git diff --check`

## Completion Notes

Completed on: 2026-06-01

What changed:

- Updated current-facing fixture acceptance wording to recommend `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` after successful preflight.
- Kept launcher behavior, preflight behavior, Storage Scan behavior, fixture execution behavior, real-profile blockers, permanent deletion, and cleanup history unchanged.

Files changed:

- `README.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-checklist-section-grouping.md`
- `docs/features/2026-06-01-fixture-notes-launcher-wording-alignment.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `git diff --check`

Docs updated:

- README, live-product roadmap, fixture acceptance notes brief, fixture checklist section grouping brief, this feature brief, progress log, and thread handoff.

ADRs added or skipped:

- No ADR added. This is docs/workflow wording alignment with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Follow-up work:

- Run the visible fixture pass with `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` after successful preflight when the user is ready.

Open questions:

- None for this packet.

Risky assumptions:

- Users who have already run preflight should prefer the `-SkipPreflight` form for the immediate fixture visual pass.
