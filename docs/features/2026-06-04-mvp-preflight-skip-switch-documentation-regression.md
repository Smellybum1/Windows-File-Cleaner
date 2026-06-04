# Feature: MVP Preflight Skip Switch Documentation Regression

Date started: 2026-06-04
Status: completed
Owner: project-owner

## Goal

Keep focused `Invoke-MvpPreflight.cmd` skip-switch documentation aligned with the actual preflight script.

## Non-goals

- Do not change MVP preflight behavior.
- Do not add or remove skip switches.
- Do not launch WPF.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, approve cleanup, write Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

## Desired behavior

- `docs/operations/ci.md` is the authoritative runbook for current `Invoke-MvpPreflight.cmd` skip switches.
- `tools\Test-DocumentationConsistency.cmd` parses `tools\Invoke-MvpPreflight.ps1` and fails if the CI runbook omits any current skip switch. Later exact-set coverage also fails stale runbook switch entries.
- Daily-use guidance points to the CI runbook instead of carrying a partial skip-switch list.
- MVP preflight keeps running documentation consistency by default.

## Decisions made

Small feature-level decisions:

- Check all `Skip*` switches exposed by `tools\Invoke-MvpPreflight.ps1`.
- Keep skip-switch documentation in the CI runbook, because that is the runbook for normal and local preflight behavior.
- Keep this as terminal-readonly documentation regression coverage.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-06-04

What changed:

- `docs/operations/ci.md` now lists every current `Invoke-MvpPreflight.cmd` skip switch.
- `tools\Test-DocumentationConsistency.cmd` initially verified the CI runbook contains every current preflight skip switch; the later exact documentation regression tightened this to an exact set.
- `docs/operations/daily-use.md` now points to the CI runbook for the full skip-switch list instead of keeping a partial list.
- Compact current-state, progress, and thread-handoff docs name this packet and the new documentation consistency coverage.
- Later packet `MVP Preflight Skip Switch Exact Documentation Regression` made the check exact, so stale documented skip switches fail too.

Tests run:

- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

Docs updated:

- This feature brief.
- `docs/features/index.md`
- `docs/features/2026-06-04-documentation-consistency-regression.md`
- `docs/operations/ci.md`
- `docs/operations/daily-use.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This is terminal-only documentation regression coverage and does not change product behavior, cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

Open questions:

- None.

Follow-up work:

- Keep the CI runbook's focused skip-switch section current whenever `Invoke-MvpPreflight.cmd` adds or removes a skip switch.

Risky assumptions:

- A simple regex over `[switch]$Skip...` parameters is sufficient for the current preflight script style.
