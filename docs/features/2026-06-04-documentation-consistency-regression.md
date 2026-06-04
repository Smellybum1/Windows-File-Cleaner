# Feature: Documentation Consistency Regression

Date started: 2026-06-04
Status: completed
Owner: project-owner

## Goal

Catch active documentation drift before handoff and CI evidence goes stale, especially operation runbook links and latest packet breadcrumbs.

## Non-goals

- Do not launch WPF.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, approve cleanup, write Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.
- Do not validate archived historical docs.

## Desired behavior

- A targeted command verifies committed read-first and handoff docs.
- The check verifies active markdown references in README, feature index, current state, progress, and thread handoff point at existing files.
- The check verifies bare active feature-index entries point at existing files under `docs/features`.
- The check verifies required operational runbooks are listed in both the feature index and thread handoff.
- The check verifies latest docs/workflow and tooling/evidence packet breadcrumbs align between current state, progress, and thread handoff.
- The check verifies the CI runbook lists every current `Invoke-MvpPreflight.cmd` skip switch.
- MVP preflight runs the check by default, with a skip switch for focused local loops.

## Decisions made

Small feature-level decisions:

- Add `tools\Test-DocumentationConsistency.cmd` and `.ps1`.
- Add the check after the Restore Manifest safety regressions and before whitespace diff in MVP preflight.
- Add `-SkipDocumentationConsistencyCheck` for focused local loops.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-06-04

What changed:

- Added a documentation consistency regression command.
- MVP preflight now runs the documentation consistency regression by default.
- The regression checks active operational runbook references and latest packet breadcrumb alignment.
- Later packet `Feature Index Entry Regression` added coverage for bare `Active Or Current` feature-index filenames.
- Later packet `MVP Preflight Skip Switch Documentation Regression` added coverage that the CI runbook lists every current `Invoke-MvpPreflight.cmd` skip switch.

Tests run:

- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

Docs updated:

- This feature brief.
- `docs/features/index.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/operations/ci.md`
- `docs/operations/daily-use.md`
- `README.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. This is terminal-only documentation verification and does not change product behavior, cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

Open questions:

- None.

Follow-up work:

- Keep the parser focused on active docs. Add archived-doc coverage only if archived docs become an active handoff source again.

Risky assumptions:

- Comparing packet slugs to compact current-state names after removing the date prefix is stable enough for current packet naming.
