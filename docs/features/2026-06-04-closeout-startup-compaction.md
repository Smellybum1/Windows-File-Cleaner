# Feature: Closeout Startup Compaction

Date started: 2026-06-04
Status: completed
Owner: project-owner

## Goal

Prepare the next thread with a compact read-first handoff after the daily readiness stop-action packet.

## Non-goals

- Do not change app behavior.
- Do not change cleanup or restore eligibility.
- Do not launch WPF.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, approve cleanup, write real Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

## Desired behavior

- `.codex/progress.md` is compact enough for new threads to read first.
- Detailed packet evidence remains available in `.codex/archive/`.
- `docs/codex/thread-handoff.md` gives a concise startup prompt instead of duplicating the full state.
- Current docs record GitHub Actions MVP Preflight #391 on `9204247` as the representative proof after the daily readiness stop-action reminder joined default preflight coverage.

## Decisions made

Small feature-level decisions:

- Archive the long progress log instead of deleting evidence.
- Keep the startup prompt short and point to read-first docs for details.
- Do not refresh ignored package acceptance notes; package acceptance remains human-owned.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-06-04

What changed:

- Moved the long progress log to `.codex/archive/progress-2026-06-04-pre-closeout.md`.
- Replaced `.codex/progress.md` with a compact current-status log.
- Shortened the thread-handoff startup prompt.
- Updated current-state, CI runbook, progress, thread-handoff, and feature index docs for #391 and this closeout packet.

Tests run:

- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `git diff --check`

Docs updated:

- This feature brief.
- `.codex/progress.md`
- `.codex/archive/progress-2026-06-04-pre-closeout.md`
- `docs/codex/current-state.md`
- `docs/codex/thread-handoff.md`
- `docs/features/index.md`
- `docs/operations/ci.md`

ADRs added or skipped:

- Skipped. This is docs-only startup/handoff compaction and does not change product behavior, cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

Open questions:

- None.

Follow-up work:

- Keep read-first docs compact; archive detailed packet evidence when it stops being useful for fresh-thread startup.

Risky assumptions:

- The public GitHub Actions run page for #391 is sufficient evidence for run number, commit, duration, and success status.
