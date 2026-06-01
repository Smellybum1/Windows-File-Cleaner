# Feature: Local Release Acceptance Notes

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make portable package acceptance recordable in ignored local notes, with a read-only summary command that can surface incomplete package acceptance evidence.

## Non-goals

- Do not create app persistence, cleanup history, package metadata, an installer, shortcut, service, scheduled task, or background automation.
- Do not launch WPF from notes summary.
- Do not scan, move, restore, delete, approve cleanup, or modify real-profile files.
- Do not change Quarantine, selected restore, permanent deletion, broad restore, or history availability.

## Context

`tools\Start-LocalRelease.cmd -ChecklistOnly` prints the package acceptance checklist, but the result of a manual package acceptance pass was still only conversational. The fixture review flow already has ignored local notes plus a summary helper; the portable release flow benefits from the same pattern.

## Implementation

- `tools\Start-LocalRelease.ps1` now supports `-ChecklistOnly -WriteAcceptanceNotes`.
- The generated notes live under ignored `.local\release-acceptance`.
- Notes stamp repo branch/commit, worktree status at notes creation, release metadata commit, publish worktree/preflight evidence, executable/readme/launch paths, exact normal/fixture launch commands, evidence checkboxes, safety boundary text, and exact post-pass summary commands.
- Later packet `Local Release Acceptance Notes Evidence Prefill` made the notes pre-record verifier evidence after `Test-LocalRelease` passes and pre-record current-commit evidence when `-RequireCurrentCommit` was used; normal launch and fixture launch/scan evidence remain manual.
- Later packet `Local Release Acceptance Notes Launch Commands` made the notes and read-only summary print the exact normal and fixture launch commands for the verified package.
- Added `tools\Summarize-LocalReleaseAcceptanceNotes.ps1` and `.cmd`.
- The summary helper defaults to the latest ignored release acceptance notes file or accepts `-Path`.
- `-RequireComplete` fails until verifier evidence, commit evidence, normal launch evidence, fixture launch/read-only-scan evidence, an acceptable overall result, and all checklist items are recorded.

## Verification

- `cmd.exe /c tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` returned non-zero as expected for an unfilled notes template.
- `cmd.exe /c tools\Start-LocalRelease.cmd -Fixture -ChecklistOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-local-release-launcher.md`
- `docs/features/2026-06-02-local-release-acceptance-checklist.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is local acceptance evidence tooling for ignored Portable Release Package notes, with no installer, persistence, cleanup execution, restore scope, data-model, or security-boundary change.

## Follow-up Work

- After committing this packet, cut a fresh ignored portable package so the latest package commit again matches current `HEAD`.
- Optionally run the notes-enabled checklist from a clean worktree and use the summary helper to confirm the clean stamp.

## Risks And Assumptions

- Ignored markdown notes are enough for local v1 acceptance evidence.
- Acceptance notes are useful as local evidence, but they are not a substitute for `tools\Test-LocalRelease.cmd`.
