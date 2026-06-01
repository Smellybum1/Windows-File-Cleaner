# Feature: Fixture Acceptance Summary Recorder Guidance

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make incomplete Fixture Acceptance Notes summaries print the exact recorder follow-up command without recording notes automatically.

## Non-goals

- Do not record fixture notes automatically from chat evidence.
- Do not launch WPF.
- Do not create fixture files.
- Do not click `Scan`, scan, move, restore, delete, approve cleanup, write Restore Manifests, or create cleanup history.
- Do not change real-profile movement, selected restore, permanent deletion, broad restore, shortcut, installer, or history availability.

## Context

`Summarize-FixtureAcceptanceNotes.cmd` already reports incomplete formal notes and `Record-FixtureAcceptanceNotes.cmd` already records all-pass ignored notes after a human visible fixture pass. The remaining friction was that the incomplete summary output did not print the exact recorder command at the point where the blocker is visible.

## Implementation

- Added a read-only follow-up block to `Summarize-FixtureAcceptanceNotes.ps1` when notes are incomplete.
- The block prints the exact `Record-FixtureAcceptanceNotes.cmd -Path ... -RecordManualAcceptance` command for the summarized notes file.
- The same guidance appears before incomplete `-RequireComplete` exits nonzero.
- The guidance says to use the command only after an actual all-pass visible fixture review and to fill notes manually when there were issues or not-checked items.

## Verification

- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md"`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md" -RequireComplete` failed as expected because the notes are incomplete, and printed the recorder guidance before exit.
- `git diff --check`

## Docs

- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-02-fixture-acceptance-summary-recorder-guidance.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is a terminal guidance polish for existing ignored local notes tooling.

## Follow-up Work

- Use the printed recorder command only after an actual all-pass visible fixture review.
- Keep issue or not-checked fixture passes in manually filled notes rather than using the all-pass recorder.

## Risks And Assumptions

- Printing the command near the blocker reduces friction without weakening the manual-review boundary.
- The explicit `-RecordManualAcceptance` flag and wording are enough to avoid implying automatic fixture acceptance.
