# Feature: CI Evidence Wording Stabilization

Date started: 2026-06-04
Status: completed
Owner: project-owner

## Goal

Prevent CI evidence docs from becoming stale after every successful docs-only push.

## Non-goals

- Do not change CI workflow behavior.
- Do not update the recorded evidence just because a newer docs-only CI run exists.
- Do not launch WPF.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, approve cleanup, write Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

## Desired behavior

- CI docs describe a representative current-path run, not the evergreen latest run.
- The CI runbook says not to update the evidence line for every green docs-only push.
- Latest docs/workflow breadcrumbs stay aligned across current state, progress, and thread handoff.

## Decisions made

Small feature-level decisions:

- Keep representative CI evidence stable until the CI path, runner baseline, or preflight coverage changes.
- Update future CI evidence only when the CI path, runner baseline, or preflight coverage meaningfully changes.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-06-04

What changed:

- Reworded the CI runbook to use representative current-path evidence.
- Reworded the CI evidence refresh brief to avoid self-staling latest-run language.
- Updated compact handoff docs to name this docs/workflow packet.
- Later packet `CI Evidence Refresh After Trust Helper Preflight` refreshed the representative evidence after preflight coverage changed.

Tests run:

- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `git diff --check`

Docs updated:

- This feature brief.
- `docs/features/index.md`
- `docs/features/2026-06-04-ci-evidence-refresh.md`
- `docs/operations/ci.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This is docs-only CI wording stabilization and does not change product behavior, cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

Open questions:

- None.

Follow-up work:

- Refresh the representative CI evidence only after meaningful CI path, runner baseline, or preflight coverage changes. This happened next when the trust-helper path guard joined default MVP preflight.

Risky assumptions:

- #365 remains historical evidence for active feature-index documentation consistency, but is no longer the current representative normal push evidence after trust-helper preflight coverage changed.
