# Feature: Fixture Acceptance Notes Summary Helper

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make ignored fixture acceptance notes easier to review after a manual fixture pass by adding a read-only terminal summary helper.

## Non-goals

- Do not launch WPF.
- Do not create synthetic fixture files.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, create, or rewrite real-profile files.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not turn ignored `.local` acceptance notes into app persistence.

## User Story / Job Story

As the local app owner, I want a quick summary of the latest fixture acceptance notes, so that I can see open manual-review items without rereading the whole notes template.

## Current Behavior

`Start-MvpFixtureReview.cmd -WriteAcceptanceNotes` can write ignored acceptance notes under `.local\fixture-review-acceptance`.

The notes template is intentionally detailed. After a pass, the reviewer still needs a quick way to see the overall result, checklist counts, and any issue/not-checked/not-recorded items.

## Desired Behavior

`Summarize-FixtureAcceptanceNotes.cmd` should:

- read the latest `.local\fixture-review-acceptance\fixture-acceptance-*.md` file by default,
- accept `-Path` for a specific notes file inside the repo,
- print the notes file path, creation time, Git branch/commit, worktree status at notes creation when present, WPF build context, acceptance-evidence checkbox states, overall result, checklist totals, and items needing review with compact recorded notes or prompt previews,
- support `-RequireComplete`, which exits non-zero until the notes have recorded preflight/worktree evidence, an overall pass result, and no not-checked or not-recorded checklist items,
- stay read-only and avoid WPF launch, scan, movement, restore, delete, and cleanup history.

## Domain Language Changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing fixture acceptance notes and Cleanup Scope terms used. | n/a |

## Evidence And Validation Gate

Evidence gathered:

- Existing code/docs inspected:
  - `tools\Start-MvpFixtureReview.ps1`
  - `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
  - `docs/features/2026-06-01-live-product-readiness-roadmap.md`
  - `README.md`
  - `.codex/progress.md`
  - `docs/codex/thread-handoff.md`
- At implementation time, the ignored preview notes file existed at `.local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md`; later packets generated newer ignored notes for the current handoff.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make the helper edit notes automatically.
- Do not copy ignored `.local` notes into tracked docs automatically.
- Do not use the helper as a substitute for the visible fixture pass.

## Decisions Made

Small feature-level decisions:

- Add a PowerShell helper plus `.cmd` wrapper for execution-policy-friendly local use.
- Default to the newest ignored fixture acceptance notes file.
- Allow an explicit `-Path` only for files inside the repository.
- Summarize issue, not-checked, and not-recorded checklist items as the review queue.

ADR-worthy decisions:

- [x] None.

## Implementation Plan

1. Add `tools\Summarize-FixtureAcceptanceNotes.ps1`.
2. Add `tools\Summarize-FixtureAcceptanceNotes.cmd`.
3. Document usage in README, roadmap, progress, and handoff notes.
4. Run the helper against latest and explicit notes paths, then run whitespace checks.

## Completion Notes

Completed on: 2026-06-01

What changed:

- Added `.\tools\Summarize-FixtureAcceptanceNotes.cmd`.
- Added `.\tools\Summarize-FixtureAcceptanceNotes.ps1`.
- The helper summarizes the latest ignored fixture acceptance notes by default and supports a specific repo-local `-Path`.
- The helper prints local acceptance metadata, overall result, checklist totals, and items needing review.
- Later packet `Fixture Acceptance Summary Prompt Preview` made open checklist items show compact recorded notes or, when no notes exist, a trimmed prompt preview.
- Later packet `Fixture Acceptance Evidence Checkbox Summary` made the summary output show whether the preflight-passed and worktree-clean/intentional evidence checkboxes were recorded.
- Later packet `Fixture Acceptance Completion Check` added `-RequireComplete` so the command can fail fast when acceptance notes are still incomplete after a manual pass.
- Later packet `Fixture Acceptance Post-Pass Guidance` made the fixture launcher print the exact `-Path` summary and `-RequireComplete` commands for the notes file it writes.
- Later packet `Fixture Acceptance Notes Embedded Commands` made the generated notes file include the exact `-Path` summary and `-RequireComplete` commands too.
- Later packet `Fixture Acceptance Notes Worktree Stamp` made generated notes record worktree status at notes creation and made this summary helper print that status. Older notes that lack the line show `unknown`.
- Later packet `Fixture Acceptance Clean Worktree Notes Preview` summarized `.local\fixture-review-acceptance\fixture-acceptance-20260601-122940.md` and printed `Worktree at notes creation: clean`; `-RequireComplete` returned non-zero as expected for the unfilled template.
- Later packet `Fixture Acceptance Current Baseline Notes Preview` summarized `.local\fixture-review-acceptance\fixture-acceptance-20260601-131233.md` from commit `dd86566` and printed `Worktree at notes creation: clean`; `-RequireComplete` returned non-zero as expected for the unfilled template.

Files changed:

- `tools\Summarize-FixtureAcceptanceNotes.ps1`
- `tools\Summarize-FixtureAcceptanceNotes.cmd`
- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Tests run:

- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path .local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md`
- `git diff --check`
- Later prompt-preview packet reran the same two summary commands plus `git diff --check`.
- Later evidence-checkbox packet reran the same two summary commands plus `git diff --check`.
- Later completion-check packet reran the normal and explicit summary commands, verified `-RequireComplete` fails on the current checklist-only preview, and ran `git diff --check`.
- Later post-pass-guidance packet ran checklist-notes output, summarized the newly written notes file by explicit `-Path`, verified explicit `-Path ... -RequireComplete` fails while the checklist-only notes are incomplete, inspected the focused preflight success-output wording, and ran `git diff --check`.
- Later embedded-commands packet ran checklist-notes output, inspected the generated notes command block, summarized the newly written notes file by explicit `-Path`, verified explicit `-Path ... -RequireComplete` fails while the checklist-only notes are incomplete, and ran `git diff --check`.
- Later worktree-stamp packet ran checklist-notes output, inspected the generated notes header, summarized the newly written notes file by explicit `-Path`, and ran whitespace checks.
- Later clean-worktree notes-preview packet summarized `.local\fixture-review-acceptance\fixture-acceptance-20260601-122940.md` by explicit `-Path`, verified explicit `-Path ... -RequireComplete` fails while the checklist-only notes are incomplete, and ran whitespace checks.
- Later current-baseline notes-preview packet summarized `.local\fixture-review-acceptance\fixture-acceptance-20260601-131233.md` by explicit `-Path`, verified explicit `-Path ... -RequireComplete` fails while the checklist-only notes are incomplete, and ran whitespace checks.

Docs updated:

- README, fixture acceptance notes template brief, live-product roadmap, progress log, and thread handoff.

ADRs added or skipped:

- No ADR added. This is a local read-only review helper with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Follow-up work:

- Run the visible fixture pass with `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` when the user is ready.
- After the pass, run the printed `.\tools\Summarize-FixtureAcceptanceNotes.cmd -Path ...` command, use the printed `-RequireComplete` command after filling notes, and copy relevant manual results into `.codex\progress.md`.

Open questions:

- Whether real completed notes make the detailed fixture checklist feel too long, even with a summary helper.

Risky assumptions:

- Summarizing ignored notes is enough to make manual fixture acceptance easier without adding app persistence or tracked note files.
