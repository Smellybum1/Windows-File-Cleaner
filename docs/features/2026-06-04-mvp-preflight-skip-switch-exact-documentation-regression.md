# Feature: MVP Preflight Skip Switch Exact Documentation Regression

Date started: 2026-06-04
Status: completed
Owner: project-owner

## Goal

Make the preflight skip-switch documentation regression catch both missing and stale skip-switch entries.

## Non-goals

- Do not change MVP preflight behavior.
- Do not add or remove skip switches.
- Do not launch WPF.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, approve cleanup, write Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

## Desired behavior

- `tools\Test-DocumentationConsistency.cmd` parses skip switches from `tools\Invoke-MvpPreflight.ps1`.
- The same regression parses the `docs/operations/ci.md` `Focused Local Skip Switches` section.
- The check fails when a current script skip switch is missing from the runbook.
- The check fails when the runbook lists a stale skip switch that no longer exists in the script.

## Decisions made

Small feature-level decisions:

- Keep the authoritative skip-switch documentation in the CI runbook.
- Parse only the focused skip-switch section to avoid historical or example mentions.
- Keep this as terminal-readonly documentation regression coverage.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-06-04

What changed:

- `tools\Test-DocumentationConsistency.cmd` now compares the exact script and CI-runbook skip-switch sets.
- Missing script switches and stale documented switches produce distinct failure messages.
- `docs/operations/ci.md` now says the runbook section must exactly match current preflight skip switches.
- Compact current-state, progress, and thread-handoff docs name this packet and the stricter documentation consistency coverage.

Tests run:

- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

Docs updated:

- This feature brief.
- `docs/features/index.md`
- `docs/features/2026-06-04-documentation-consistency-regression.md`
- `docs/features/2026-06-04-mvp-preflight-skip-switch-documentation-regression.md`
- `docs/operations/ci.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This is terminal-only documentation regression coverage and does not change product behavior, cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

Open questions:

- None.

Follow-up work:

- Keep the parser focused on the active CI runbook section unless another active runbook becomes the authoritative skip-switch source.

Risky assumptions:

- The CI runbook skip-switch entries will stay as backtick-wrapped `-Skip...` bullets in the `Focused Local Skip Switches` section.
