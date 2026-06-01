# Feature: Fixture Acceptance Notes Template

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make the next visible fixture acceptance pass easier to record by letting the fixture launcher write a local markdown notes template from the same checklist it prints.

## Non-goals

- Do not launch WPF in automated checks.
- Do not create synthetic fixture files in checklist-only checks unless the user runs the normal fixture launcher.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, create, or rewrite real-profile files.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not replace README, the terminal checklist, or the visible manual fixture pass.

## User Story / Job Story

As the local app owner, I want the fixture launcher to create a notes template for the manual acceptance pass, so that the pass produces durable evidence without relying on memory or a long chat transcript.

## Current Behavior

`Start-MvpFixtureReview.cmd -ChecklistOnly` prints the current manual fixture checklist without running preflight, creating fixture files, launching WPF, scanning, moving, restoring, deleting, or creating cleanup history.

The next live-product roadmap gate is visible fixture acceptance, but the launcher did not provide a local place to mark pass/issue/not-checked results.

## Desired Behavior

`Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes` should:

- resolve and validate the fixture Cleanup Scope inside the repo,
- print the same manual fixture review checklist,
- write a timestamped markdown notes template under `.local\fixture-review-acceptance`,
- include the fixture Cleanup Scope, safety boundary, overall result checkboxes, and one pass/issue/not-checked notes block per checklist item,
- exit before preflight, fixture creation, WPF launch, scan, movement, restore, deletion, or cleanup history.

`Start-MvpFixtureReview.cmd -WriteAcceptanceNotes` should also be able to write the notes template before launching the manual fixture app. After a successful preflight, the post-preflight shortcut is `Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`.

## Domain Language Changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing fixture review and Cleanup Scope terms used. | n/a |

## Open Questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- After the next visible fixture pass, should the notes template be shortened or grouped to match the tabs more tightly?

## Grill Notes

### Scenarios Discussed

- The live-product roadmap identifies visible fixture acceptance as the next gate.
- The fixture checklist is intentionally detailed, so a notes template makes the pass easier to record.
- The user has not asked to launch WPF in this continuation turn.

### Edge Cases

- Notes template creation must stay inside `.local` so it does not create tracked review artifacts by default.
- `-WhatIf -WriteAcceptanceNotes` should not write the template.
- `-ChecklistOnly -WriteAcceptanceNotes` must still avoid preflight, fixture creation, WPF launch, scans, movement, restore, delete, and cleanup history.

### Dependencies Between Decisions

- This depends on the existing fixture launcher and checklist-only mode.
- It supports the live-product readiness roadmap's manual fixture acceptance gate.
- It does not change ADR 0017, ADR 0018, or ADR 0019.

## Evidence And Validation Gate

Evidence gathered:

- User answers:
  - The tabbed/header UI looks much better.
  - Do not move, delete, quarantine, or restore real-profile files without explicit approval after Grill with Docs.
- Existing code/docs inspected:
  - `tools/Start-MvpFixtureReview.ps1`
  - `tools/Start-MvpFixtureReview.cmd`
  - `README.md`
  - `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
  - `docs/features/2026-05-29-fixture-review-checklist-output.md`
  - `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
  - `docs/features/2026-06-01-live-product-readiness-roadmap.md`
  - ADR 0017, ADR 0018, and ADR 0019
- Tests/checks planned:
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make notes writing the default for every launcher run until the next manual pass proves it is useful.
- Do not store acceptance notes in tracked docs by default.
- Do not automate the visual fixture pass from this packet.

## Decisions Made

Small feature-level decisions:

- Add `-WriteAcceptanceNotes` to the existing fixture launcher.
- Generate notes from the same checklist item source used for terminal output to avoid wording drift.
- Write notes under ignored `.local\fixture-review-acceptance`.
- Keep normal launcher and checklist-only behavior unchanged unless the switch is passed.

ADR-worthy decisions:

- [x] None.

## Implementation Plan

1. Refactor the launcher checklist into a shared item list.
2. Add a markdown notes-template writer.
3. Wire `-WriteAcceptanceNotes` into checklist-only and normal launcher paths.
4. Update README, existing feature notes, progress, and handoff docs.
5. Run focused launcher and diff checks.

## Files Expected To Change

Expected:

- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Possible:

- None.

## Test Plan

Manual checks:

- Inspect the generated `.local\fixture-review-acceptance\fixture-acceptance-*.md` template after the focused check.

Automated checks:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `git diff --check`

## Risks And Assumptions

Risks:

- The notes template may still be long because the fixture checklist is long.
- `.local` notes are intentionally not tracked, so the user must copy relevant results into progress docs after a pass.

Assumptions:

- A local ignored notes file is useful for the next visible fixture pass without adding persistence to the app.

## Completion Notes

Completed on: 2026-06-01

What changed:

- Added `-WriteAcceptanceNotes` to `Start-MvpFixtureReview.ps1`.
- Refactored terminal checklist output to reuse a single checklist item source.
- Added timestamped markdown notes templates under `.local\fixture-review-acceptance`.
- Later packet `2026-06-01-fixture-checklist-section-grouping.md` added matching section headings to the notes template while preserving the same numbered prompts.
- Later packet `2026-06-01-fixture-acceptance-evidence-header.md` added repository, Git branch/commit, preflight command, visible fixture command, and local evidence checkboxes to the notes header.
- Later packet `2026-06-01-fixture-acceptance-build-context-header.md` added .NET SDK, WPF app project, WPF app target framework, and WPF enabled evidence to the notes header.
- Later packet `2026-06-01-fixture-acceptance-current-commit-notes-preview.md` generated a current-commit checklist-only notes preview at `.local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md`, stamped with commit `fd8e1d4` and the build-context fields.
- Later packet `2026-06-01-fixture-acceptance-notes-summary-helper.md` added `.\tools\Summarize-FixtureAcceptanceNotes.cmd` to read ignored notes and print overall result, checklist totals, and issue/not-checked/not-recorded items without launching WPF, scanning, moving, restoring, deleting, or creating cleanup history.
- Later packet `Fixture Acceptance Summary Prompt Preview` made the summary output include compact notes or prompt previews for open checklist items.
- Later packet `Fixture Acceptance Evidence Checkbox Summary` made the summary output include preflight-passed and worktree-clean/intentional evidence checkbox states.
- Later packet `Fixture Acceptance Completion Check` added `.\tools\Summarize-FixtureAcceptanceNotes.cmd -RequireComplete` so incomplete acceptance notes can fail fast after a manual pass.
- Later packet `Fixture Acceptance Post-Pass Guidance` made the fixture launcher print the exact summary and completion-check commands for the notes file it just wrote.
- Updated docs and handoff/progress notes.

Files changed:

- `tools/Start-MvpFixtureReview.ps1`
- `tools/Summarize-FixtureAcceptanceNotes.ps1`
- `tools/Summarize-FixtureAcceptanceNotes.cmd`
- `README.md`
- `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `git diff --check`
- Later current-commit notes-preview packet inspected `.local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md` after `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`.
- Later summary-helper packet ran `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd` and `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path .local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md`.
- Later post-pass-guidance packet ran checklist-notes output, explicit `-Path` summary, expected explicit `-Path ... -RequireComplete` failure on a fresh incomplete notes file, and `git diff --check`.

Docs updated:

- README, fixture launcher/checklist/checklist-only feature briefs, live-product roadmap, progress log, and thread handoff.

ADRs added or skipped:

- No ADR added. This is local fixture-review workflow evidence capture with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Follow-up work:

- Run the visible fixture pass with `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` after a successful preflight when the user is ready.
- Run the printed `.\tools\Summarize-FixtureAcceptanceNotes.cmd -Path ...` command after the pass to review open checklist items for that notes file.
- Use the printed `.\tools\Summarize-FixtureAcceptanceNotes.cmd -Path ... -RequireComplete` command when you want a non-zero exit for incomplete local notes before copying results into progress docs.
- Copy relevant manual results from `.local` notes into `.codex/progress.md` after the pass.

Open questions:

- After a real manual pass, should any section be split or shortened further?

Risky assumptions:

- Notes template creation under `.local` is enough to make manual acceptance evidence easier without adding tracked noise or app persistence.
