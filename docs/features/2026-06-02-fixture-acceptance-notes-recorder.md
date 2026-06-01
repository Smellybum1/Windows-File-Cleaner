# Feature: Fixture Acceptance Notes Recorder

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make a completed manual fixture acceptance pass recordable from the terminal without hand-editing ignored markdown notes.

## Non-goals

- Do not launch WPF.
- Do not create fixture files.
- Do not click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, approve cleanup, write Restore Manifests, or create cleanup history.
- Do not make fixture acceptance automatic; the recorder is for evidence after a human has already completed the visible fixture pass.
- Do not change Quarantine, selected restore, real-profile movement, permanent deletion, broad restore, or history availability.

## User story / job story

As the local app owner, I want to record a completed visible fixture acceptance pass from the terminal, so that formal ignored notes can be completed without manual markdown editing.

## Current behavior

`Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` can write ignored fixture acceptance notes, and `Summarize-FixtureAcceptanceNotes.cmd -RequireComplete` can prove whether they are complete. The remaining friction is hand-editing all checklist statuses after the human has completed the visible pass.

## Desired behavior

`tools\Record-FixtureAcceptanceNotes.cmd` should:

- default to the latest ignored `.local\fixture-review-acceptance\fixture-acceptance-*.md` file or accept `-Path`,
- require `-RecordManualAcceptance`,
- mark preflight and worktree evidence checkboxes as recorded,
- mark overall result `Pass`,
- mark all fixture checklist items `Pass`,
- write a summary line,
- refuse paths outside ignored `.local`,
- print exact summary and completion-check commands,
- stay local-note-only and avoid WPF launch, fixture creation, scan, movement, restore, delete, approval, and cleanup history.

## Domain language changes

No new durable product terms. The existing fixture acceptance notes language is clarified in domain docs.

| Term | Change | Docs updated? |
|---|---|---|
| Fixture Acceptance Notes | Clarified that ignored fixture notes may be recorded after a human pass by an explicit terminal recorder. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether the user wants to record the existing user-reported fixture pass in ignored notes, or only use the recorder for the next visible pass.

## Grill notes

### Scenarios discussed

- The user already reported the visible fixture pass looked good, but the local ignored notes remain formally unfilled.
- Portable release acceptance already has a terminal recorder with an explicit manual-confirmation switch.
- Fixture acceptance should get the same friction reduction without weakening the human-review boundary.

### Edge cases

- Running without `-RecordManualAcceptance` should fail before writing notes.
- Explicit paths must stay under ignored `.local`.
- The recorder may be used against a test copy of a notes file for verification.
- Recording notes is not proof that the visible pass happened; it is a local way to record the human's confirmation.

### Dependencies between decisions

- The fixture notes template and summary helper already define the note shape.
- This remains separate from Portable Release Package acceptance notes.
- ADR 0017, ADR 0018, and ADR 0019 still govern real-profile movement and selected restore.

## Evidence and validation gate

Evidence gathered:

- User answers:
  - Continue toward a safe live product while preserving safety gates.
  - Do not move, delete, quarantine, or restore real-profile files unless explicitly asked for the specific action.
- Existing code/docs inspected:
  - `AGENTS.md`
  - `.codex/progress.md`
  - `README.md`
  - `docs/codex/thread-handoff.md`
  - `docs/codex/grill-with-docs.md`
  - `docs/codex/skillopt-inspired-workflow.md`
  - `docs/domain/context.md`
  - `docs/domain/glossary.md`
  - `docs/features/2026-06-01-live-product-readiness-roadmap.md`
  - `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
  - `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
  - `docs/features/2026-06-02-local-release-acceptance-notes-recorder.md`
  - ADR 0017, ADR 0018, and ADR 0019
  - `tools/Start-MvpFixtureReview.ps1`
  - `tools/Summarize-FixtureAcceptanceNotes.ps1`
  - `tools/Record-LocalReleaseAcceptanceNotes.ps1`
- Tests/checks planned:
  - `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd` should fail without `-RecordManualAcceptance`.
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
  - Copy the generated ignored notes to `.local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md`.
  - `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md" -RecordManualAcceptance -Summary "Test-only recorded fixture acceptance for recorder verification."`
  - `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md" -RequireComplete`
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not mark real ignored fixture notes complete automatically from user-reported chat evidence.
- Do not remove the manual notes editing path; issue or not-checked outcomes still need human notes.
- Do not treat the recorder as the visible fixture pass itself.

## Decisions made

Small feature-level decisions:

- Mirror the portable release acceptance recorder shape for fixture acceptance.
- Require `-RecordManualAcceptance` before any write.
- Mark all fixture checklist items pass only for an all-pass manual acceptance.
- Keep writing constrained to ignored `.local`.

ADR-worthy decisions:

- [x] None. This is local ignored evidence-note recording, with no architecture, persistence, cleanup execution, restore rule, data-model, deployment, or security-boundary change.

## Implementation plan

1. Add `tools/Record-FixtureAcceptanceNotes.ps1`.
2. Add `tools/Record-FixtureAcceptanceNotes.cmd`.
3. Update the fixture launcher notes guidance to print the recorder command.
4. Update README, domain docs, glossary, roadmap, handoff, and progress log.
5. Run read-only/local-notes verification checks.

## Files expected to change

Expected:

- `tools/Record-FixtureAcceptanceNotes.ps1`
- `tools/Record-FixtureAcceptanceNotes.cmd`
- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-02-fixture-acceptance-notes-recorder.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Inspect recorder output to confirm it repeats the no-WPF/no-scan/no-movement/no-history boundary.
- Inspect generated notes command block to confirm the recorder command is printed before summary commands.

Automated checks:

- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md" -RecordManualAcceptance -Summary "Test-only recorded fixture acceptance for recorder verification."`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md" -RequireComplete`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- A recorder can be misused if someone runs it without actually completing the visible pass; the explicit switch and wording reduce but cannot eliminate that risk.
- All-pass recording is intentionally simple; issue or partial-pass cases still need manual notes.

Assumptions:

- The latest fixture notes format remains checklist-heading compatible with `### N. Fixture check`.
- Recording ignored fixture notes after a human pass is useful before more live-product movement work.

## Completion notes

Completed on: 2026-06-02

What changed:

- Added `tools\Record-FixtureAcceptanceNotes.ps1`.
- Added `tools\Record-FixtureAcceptanceNotes.cmd`.
- Updated `Start-MvpFixtureReview.ps1` so generated notes and launcher output print the recorder command before summary commands.
- Kept the recorder constrained to ignored `.local` notes and behind explicit `-RecordManualAcceptance`.

Files changed:

- `tools/Record-FixtureAcceptanceNotes.ps1`
- `tools/Record-FixtureAcceptanceNotes.cmd`
- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-02-fixture-acceptance-notes-recorder.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes` passed twice after wording polish, without preflight, fixture creation, WPF launch, scan, movement, restore, deletion, approval, or cleanup history.
- Copied the latest generated ignored notes to `.local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md` for verification.
- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd` failed as expected without `-RecordManualAcceptance`.
- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md"` failed as expected without `-RecordManualAcceptance`.
- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md" -RecordManualAcceptance -Summary "Test-only recorded fixture acceptance for recorder verification."` passed and updated only copied ignored notes.
- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md" -RecordManualAcceptance -WhatIf` passed and reported no notes changes.
- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd -Path README.md -RecordManualAcceptance` failed as expected because the path is outside ignored `.local`.
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md" -RequireComplete` passed with `10 pass, 0 issue, 0 not checked, 0 not recorded`.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected CRLF conversion warnings.

Docs updated:

- README, domain context, glossary, fixture notes template and summary-helper feature briefs, live-product roadmap, thread handoff, and progress log.

ADRs added or skipped:

- No ADR added. This is local ignored evidence-note recording.

Follow-up work:

- Use the recorder only after a human completes the visible fixture pass.

Open questions:

- Whether the user wants to record the existing user-reported fixture pass in ignored notes, or only use the recorder for the next visible pass.

Risky assumptions:

- The explicit manual-confirmation flag is enough to keep the recorder from being mistaken for automated fixture acceptance.
