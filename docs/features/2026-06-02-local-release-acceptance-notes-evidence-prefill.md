# Feature: Local Release Acceptance Notes Evidence Prefill

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make generated portable release acceptance notes pre-record objective package evidence that `Start-LocalRelease` has already proven, while leaving human launch and fixture scan evidence manual.

## Non-goals

- Do not mark a package fully accepted.
- Do not launch WPF, click `Scan`, scan, move, restore, delete, approve cleanup, or create cleanup history.
- Do not convert ignored `.local` notes into app persistence or package metadata.
- Do not change Quarantine, selected restore, permanent deletion, broad restore, or history availability.

## Context

`Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes` verifies the package before writing notes, but the generated notes previously left verifier and current-commit evidence unchecked. This made summary output noisier and required the user to manually record evidence that the command had already proven.

## Implementation

- Generated release acceptance notes now mark verifier evidence as recorded when verification was not skipped and completed successfully.
- Generated release acceptance notes now mark current-commit evidence as recorded when `-RequireCurrentCommit` was supplied and verification completed successfully.
- Checklist item 1 is marked `Pass` with an automatic note when verification completed successfully.
- Normal launch and fixture launch/read-only-scan evidence remain unrecorded for manual user acceptance.

## Verification

- `cmd.exe /c tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` returned non-zero as expected because manual launch and fixture evidence remain open.
- `cmd.exe /c tools\Start-LocalRelease.cmd -Fixture -ChecklistOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Docs

- `README.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-local-release-acceptance-notes.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is local acceptance notes evidence ergonomics, with no installer, persistence, cleanup execution, restore scope, data-model, or security-boundary change.

## Follow-up Work

- After committing this packet, cut a fresh ignored portable package so the latest package commit again matches current `HEAD`.
- Generate a clean-worktree notes template against that package for the next human package acceptance pass.
- Later packet `Local Release Acceptance Notes Launch Commands` made generated notes and summary output include exact normal and fixture launch commands while keeping launch evidence manual.

## Risks And Assumptions

- Automatically recording only verifier/current-commit evidence is safe because the launcher has already failed before notes creation if those checks do not pass.
- Human launch and fixture scan evidence should stay manual.
