# Feature: CI Evidence Refresh After Exact Skip Docs Regression

Date started: 2026-06-04
Status: completed
Owner: project-owner

## Goal

Refresh representative normal push CI evidence after documentation consistency started checking the exact `Invoke-MvpPreflight.cmd` skip-switch set against the CI runbook.

## Non-goals

- Do not change CI workflow behavior.
- Do not change MVP preflight behavior.
- Do not launch WPF.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, approve cleanup, write Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

## Desired behavior

- `docs/operations/ci.md` records representative normal push MVP Preflight #389 evidence.
- Compact current-state and handoff docs mention #389 on `00bdfb3` as the current representative preflight proof.
- Earlier #387, #385, #365, and #360 evidence remains available as historical skip-switch, trust-helper, feature-index, and runner-fallback proof.
- The latest docs/workflow packet breadcrumb stays aligned across current state, progress, and thread handoff.

## Decisions made

Small feature-level decisions:

- Refresh the representative evidence because default MVP preflight coverage meaningfully changed through exact skip-switch documentation consistency.
- Keep #387 as historical initial skip-switch documentation coverage proof.
- Keep this as a docs-only evidence refresh.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-06-04

What changed:

- Updated the CI runbook with GitHub Actions MVP Preflight #389 evidence for commit `00bdfb3`.
- Updated compact current-state, progress, and thread-handoff docs to name this packet and current CI evidence.
- Updated the feature index and prior CI evidence brief so #387 is framed as historical initial skip-switch documentation coverage evidence after exact skip-switch documentation coverage changed.

Tests run:

- GitHub Actions MVP Preflight #389 passed on `00bdfb3` in `1m 34s`.
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `git diff --check`

Docs updated:

- This feature brief.
- `docs/features/index.md`
- `docs/features/2026-06-04-ci-evidence-refresh-after-skip-switch-docs-regression.md`
- `docs/operations/ci.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This records CI evidence in committed documentation and does not change product behavior, cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

Open questions:

- None.

Follow-up work:

- Continue refreshing representative CI evidence only when the CI path, runner baseline, or default preflight coverage changes.

Risky assumptions:

- The GitHub Actions API run metadata for #389 is sufficient evidence for run number, commit, duration, and success status.
