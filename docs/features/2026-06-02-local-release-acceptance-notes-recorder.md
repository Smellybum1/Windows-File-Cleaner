# Feature: Local Release Acceptance Notes Recorder

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make a completed manual Portable Release Package acceptance pass recordable from the terminal without hand-editing ignored markdown notes.

## Non-goals

- Do not launch WPF, click `Scan`, scan, move, restore, delete, approve cleanup, or create cleanup history.
- Do not create app persistence, package metadata, an installer, shortcut, service, scheduled task, or background automation.
- Do not make package acceptance automatic; the recorder is for evidence after a human has already completed the checks.
- Do not change Quarantine, selected restore, permanent deletion, broad restore, or history availability.

## Context

`Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes` can generate clean ignored package acceptance notes, and `Summarize-LocalReleaseAcceptanceNotes.cmd` can prove whether they are complete. The remaining friction was hand-editing markdown after the user had completed README review, normal launch, fixture launch/read-only scan, portable-boundary review, and the real-profile stop boundary.

## Implementation

- Added `tools\Record-LocalReleaseAcceptanceNotes.ps1` and `.cmd`.
- The recorder defaults to the latest ignored `.local\release-acceptance\release-acceptance-*.md` file or accepts `-Path`.
- It requires `-RecordManualAcceptance` so it cannot update notes accidentally.
- It marks the manual launch evidence checkboxes, checklist items 2-6, overall result, and summary text as accepted.
- It only updates ignored `.local` notes and prints the exact summary and completion-check commands.
- It refuses paths outside ignored `.local`.

## Verification

- `cmd.exe /c tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes`
- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-recording-test.md" -RecordManualAcceptance -Summary "Test-only recorded acceptance for recorder verification."`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-recording-test.md" -RequireComplete`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-local-release-acceptance-notes.md`
- `docs/features/2026-06-02-local-release-acceptance-notes-recorder.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is local ignored evidence-note recording, with no installer, app persistence, cleanup execution, restore scope, data-model, or security-boundary change.

## Follow-up Work

- Use the recorder only after a human completes the package acceptance pass.
- Keep future package acceptance evidence local and ignored unless the user asks to copy a summary into durable docs.

## Risks And Assumptions

- A terminal recorder reduces friction without weakening the human acceptance requirement because `-RecordManualAcceptance` must be explicit.
- Recording notes is not package verification; `Test-LocalRelease.cmd` remains the authoritative package verifier.
