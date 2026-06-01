# Feature: Fixture Acceptance Evidence Header

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make the ignored fixture acceptance notes template capture the basic evidence needed for the next visible fixture pass: repository, branch, commit, preflight checkbox, and the post-preflight launcher command.

## Non-goals

- Do not launch WPF.
- Do not create synthetic fixture files.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, create, or rewrite real-profile files.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not turn ignored `.local` notes into persisted cleanup history or tracked acceptance records.

## User Story / Job Story

As the local app owner, I want fixture acceptance notes to stamp the repo evidence and preflight reminder, so that the manual pass can be tied back to the exact code state being reviewed.

## Current Behavior

`Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes` writes an ignored `.local\fixture-review-acceptance\fixture-acceptance-*.md` template with the fixture Cleanup Scope, safety boundary, overall result, and sectioned checklist prompts.

The template did not include the current Git branch/commit or a local checkbox confirming that full MVP preflight passed immediately before the visible fixture pass.

## Desired Behavior

Generated fixture acceptance notes should include an evidence header with:

- repository path,
- current Git branch,
- current Git commit,
- required preflight command,
- recommended visible fixture command after preflight,
- a checkbox for preflight passing immediately before the visible fixture pass,
- a checkbox for clean or intentionally recorded worktree state,
- a reminder that the `.local` note is not cleanup history.

If Git evidence cannot be read, the template should still be written with `unknown` values.

## Domain Language Changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing fixture review, Cleanup Scope, preflight, and cleanup history terms used. | n/a |

## Open Questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- After the visible pass, should the notes template capture WPF app version/build output as well, or is Git branch/commit enough?

## Grill Notes

### Scenarios Discussed

- The live-product roadmap names visible fixture acceptance as the next gate.
- The current UI header layout is visually approved, so the next pass should focus on Quarantine/readiness and selected restore behavior.
- Acceptance notes are ignored local evidence, not app persistence or cleanup history.

### Edge Cases

- Checklist-only notes generation must still avoid preflight, fixture creation, WPF launch, scans, movement, restore, delete, and cleanup history.
- Git may be unavailable or fail in some shells; notes should still be generated.
- `-WhatIf -WriteAcceptanceNotes` should still not write notes.

### Dependencies Between Decisions

- This builds on the existing fixture acceptance notes template and sectioned checklist.
- It supports the live-product readiness roadmap without changing ADR 0017, ADR 0018, or ADR 0019.

## Evidence and Validation Gate

Evidence gathered:

- User answers:
  - The compact wide-header layout with Review Shortlist totals before scan totals looks good.
  - Do not move, delete, quarantine, or restore real-profile files without explicit approval after Grill with Docs.
- Existing code/docs inspected:
  - `tools/Start-MvpFixtureReview.ps1`
  - `README.md`
  - `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
  - `docs/features/2026-06-01-fixture-checklist-section-grouping.md`
  - `docs/features/2026-06-01-fixture-notes-launcher-wording-alignment.md`
  - `docs/features/2026-06-01-live-product-readiness-roadmap.md`
  - ADR 0017, ADR 0018, and ADR 0019
- Tests/checks planned:
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
  - Inspect generated `.local` notes header.
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make `.local` acceptance notes the durable cleanup history.
- Do not launch the visible fixture pass from this packet.
- Do not make preflight checkbox automation pretend to prove the user visually inspected the fixture.

## Decisions Made

Small feature-level decisions:

- Add the evidence header to generated notes only.
- Capture Git branch/commit opportunistically with `unknown` fallback.
- Keep the normal launcher and checklist-only behavior unchanged unless `-WriteAcceptanceNotes` is passed.

ADR-worthy decisions:

- [x] None.

## Implementation Plan

1. Add a narrow Git evidence helper in `Start-MvpFixtureReview.ps1`.
2. Add the evidence header to generated fixture acceptance notes.
3. Update README, acceptance-notes docs, progress, and handoff docs.
4. Run focused checklist, notes inspection, what-if, and whitespace checks.

## Files Expected To Change

Expected:

- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-acceptance-evidence-header.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Possible:

- `docs/features/2026-06-01-fixture-checklist-section-grouping.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`

## Test Plan

Manual checks:

- Inspect the generated `.local\fixture-review-acceptance\fixture-acceptance-*.md` header.

Automated tests:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `git diff --check`

## Risks And Assumptions

Risks:

- Git evidence could be unavailable in a future environment; the template uses `unknown` rather than failing.
- More header lines could make the notes template feel heavier.

Assumptions:

- Branch/commit plus an explicit preflight checkbox is useful acceptance evidence for the next visible fixture pass.

## Completion Notes

Completed on: 2026-06-01

What changed:

- Added an acceptance evidence header to fixture acceptance notes.
- The header records repository path, Git branch, Git commit, required preflight command, recommended visible fixture command, preflight/worktree checkboxes, and the local-not-cleanup-history boundary.
- Kept Storage Scan, fixture execution, restore behavior, real-profile/custom blockers, permanent deletion, and cleanup history unchanged.

Files changed:

- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-acceptance-evidence-header.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- Inspected `.local\fixture-review-acceptance\fixture-acceptance-20260601-105046.md` and confirmed the evidence header includes repository, branch, commit, preflight command, visible fixture command, evidence checkboxes, and local-not-cleanup-history wording.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `git diff --check`

Docs updated:

- README, fixture acceptance notes feature brief, this feature brief, progress log, and thread handoff.

ADRs added or skipped:

- No ADR added. This is local fixture-review evidence capture with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Follow-up work:

- Run the visible fixture pass with `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` after a successful preflight when the user is ready.
- Record relevant manual results from ignored notes in `.codex/progress.md`.

Open questions:

- Whether a future packet should capture app build/version evidence in the notes header.

Risky assumptions:

- Git branch/commit evidence is enough for the next manual fixture acceptance packet.
