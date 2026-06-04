# Feature: CI Evidence Refresh

Date started: 2026-06-04
Status: completed
Owner: project-owner

## Goal

Keep committed CI and handoff documentation aligned with representative normal push evidence after the documentation consistency hardening packets, without needing to chase every later green docs-only run.

## Non-goals

- Do not change CI workflow behavior.
- Do not launch WPF.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, approve cleanup, write Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

## Desired behavior

- `docs/operations/ci.md` records representative normal push MVP Preflight evidence.
- Compact handoff docs mention that #365 passed on `98d3412`.
- The latest docs/workflow packet breadcrumb stays aligned across current state, progress, and thread handoff.

## Decisions made

Small feature-level decisions:

- Record #365 as representative current-path evidence, while preserving #360 as the first proof of the no-input `windows-2022` fallback.
- Keep this as a docs-only evidence refresh.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-06-04

What changed:

- Updated the CI runbook with GitHub Actions MVP Preflight #365 evidence for commit `98d3412`.
- Updated compact current-state, progress, and thread-handoff docs to name this packet and current CI evidence.
- Updated the feature-index regression brief to record #365 as external CI proof.
- Later packet `CI Evidence Wording Stabilization` changed "latest" wording to representative current-path evidence, so the runbook does not become stale after every green docs-only push.

Tests run:

- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `git diff --check`

Docs updated:

- This feature brief.
- `docs/features/index.md`
- `docs/features/2026-06-04-feature-index-entry-regression.md`
- `docs/operations/ci.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This records CI evidence in committed documentation and does not change product behavior, cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

Open questions:

- None.

Follow-up work:

- Continue recording only meaningful CI evidence changes instead of every successful docs-only push.

Risky assumptions:

- The GitHub web run page remains an acceptable source for public run-number, commit, duration, and success evidence when unauthenticated API quota is unavailable.
