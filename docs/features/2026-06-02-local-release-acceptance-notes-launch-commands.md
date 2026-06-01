# Feature: Local Release Acceptance Notes Launch Commands

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make package acceptance notes and their read-only summary show the exact normal and fixture launch commands for the verified Portable Release Package.

## Non-goals

- Do not launch WPF from checklist, notes, or summary paths.
- Do not create an installer, shortcut, service, scheduled task, or background automation.
- Do not scan, move, restore, delete, approve cleanup, or create cleanup history.
- Do not change Quarantine, selected restore, permanent deletion, broad restore, or history availability.

## Context

Generated portable release acceptance notes already stamped package paths and scripts, but the exact commands still had to be recovered from launcher output or package scripts during a manual acceptance pass. Stamping both commands in the notes keeps the next human package pass local, repeatable, and easier to summarize.

## Implementation

- `tools\Start-LocalRelease.ps1` now builds both the normal launch command and fixture launch command for checklist and notes output.
- Checklist-only output now prints both exact launch commands.
- Generated ignored release acceptance notes now include `Normal launch command` and `Fixture launch command` metadata.
- `tools\Summarize-LocalReleaseAcceptanceNotes.ps1` now prints those command lines when summarizing notes.

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
- `docs/features/2026-06-02-local-release-acceptance-notes-launch-commands.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is local package acceptance-note ergonomics, with no installer, persistence, cleanup execution, restore scope, data-model, or security-boundary change.

## Follow-up Work

- After committing this packet, cut a fresh ignored portable package so the latest package commit again matches current `HEAD`.
- Generate a clean-worktree package acceptance notes file for the next manual acceptance pass.

## Risks And Assumptions

- Exact launch commands in ignored notes reduce manual friction without implying the commands were run.
- Summary output should print commands as evidence context, not as proof of launch acceptance.
