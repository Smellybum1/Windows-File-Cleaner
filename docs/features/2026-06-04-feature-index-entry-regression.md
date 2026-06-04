# Feature: Feature Index Entry Regression

Date started: 2026-06-04
Status: completed
Owner: project-owner

## Goal

Make the documentation consistency regression catch stale bare filenames in the active feature brief index.

## Non-goals

- Do not launch WPF.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, approve cleanup, write Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.
- Do not validate archived historical feature briefs.

## Desired behavior

- `tools\Test-DocumentationConsistency.cmd` verifies every `Active Or Current` entry in `docs/features/index.md` points at an existing file under `docs/features`.
- The check handles the index's current bare filename style, such as `2026-06-04-documentation-consistency-regression.md`.
- Missing or outside-`docs/features` active entries fail before MVP preflight can pass.

## Decisions made

Small feature-level decisions:

- Extend the existing documentation consistency regression instead of adding another command.
- Keep the new check focused on the active feature index section only.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-06-04

What changed:

- `tools\Test-DocumentationConsistency.ps1` now parses `docs/features/index.md` `Active Or Current` entries.
- Bare active feature filenames are resolved under `docs/features`.
- The regression rejects missing active feature briefs and entries that resolve outside `docs/features`.

Tests run:

- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- GitHub Actions [MVP Preflight #365](https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26940183876) passed on push commit `98d3412` in `1m 33s`.

Docs updated:

- This feature brief.
- `docs/features/index.md`
- `docs/features/2026-06-04-documentation-consistency-regression.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/operations/ci.md`
- `README.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This is terminal-only documentation verification and does not change product behavior, cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

Open questions:

- None.

Follow-up work:

- Keep archived feature briefs out of this focused regression unless archived docs become an active handoff source again.

Risky assumptions:

- The feature index's active entry format will remain a markdown bullet with a backtick-wrapped `.md` reference and a colon.
