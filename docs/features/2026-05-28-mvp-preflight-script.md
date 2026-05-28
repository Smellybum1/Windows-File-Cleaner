# Feature: MVP Preflight Script

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Add one local command that runs the required verification path before any real-profile Storage Scan.

## Non-goals

- Do not scan `C:\Users\moxhe`.
- Do not launch the visible desktop app.
- Do not create fixture files by default.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or persistent scan history.

## User story / job story

As the project owner, I want a single preflight command before real scans, so that restore, build, tests, and fixture dry-run checks are easy to run consistently.

## Current behavior

README documents the verification commands individually. This works, but it is easy to skip a command before scanning real user files.

## Desired behavior

The repo should include a script that:

- Runs solution restore unless explicitly skipped.
- Builds the solution without restoring again.
- Runs core tests.
- Runs WPF app smoke tests.
- Runs the synthetic fixture generator in `-WhatIf` mode.
- Runs `git diff --check`.
- Prints the next fixture review launcher command.
- States that no real user files were scanned or modified by preflight.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Storage Scan | No change. | not needed |
| Cleanup Scope | No change. | not needed |
| Dry Run | No change. | not needed |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should the app later expose a built-in diagnostics/preflight screen?

## Evidence and validation gate

Evidence gathered:

- README verification path already lists restore, build, core tests, and app tests.
- Existing fixture script supports `-WhatIf`.
- Progress log repeatedly records this same verification sequence.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make preflight launch the WPF app or scan `C:\Users\moxhe`.
- Do not create fixture files by default from preflight.
- Do not hide individual commands from README; keep them visible for troubleshooting.

## Decisions made

Small feature-level decisions:

- Add `tools/Invoke-MvpPreflight.ps1`.
- Keep fixture generation dry-run by default through `New-StorageScanSmokeFixture.ps1 -WhatIf`.
- Add `-SkipRestore`, `-SkipFixtureWhatIf`, and `-SkipDiffCheck` switches for focused local loops.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add the preflight script.
2. Update README and AGENTS project commands.
3. Update MVP audit and progress log.
4. Run the preflight script and standard diff checks.

## Files expected to change

Expected:

- `tools/Invoke-MvpPreflight.ps1`
- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `.codex/progress.md`

Possible:

- None

## Test plan

Manual checks:

- Real-profile scan remains manual and should happen only after preflight passes.

Automated tests:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- The script still depends on local .NET/NuGet access, so restore may need the same user NuGet config permission as direct restore.

Assumptions:

- Keeping the individual README commands plus a wrapper script is clearer than replacing the command list.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `tools/Invoke-MvpPreflight.ps1`.
- Preflight runs restore, build, core tests, WPF app tests, fixture `-WhatIf`, and `git diff --check`.
- Preflight prints the next fixture review launcher command.
- Kept the workflow read-only with respect to real user files.

Files changed:

- `tools/Invoke-MvpPreflight.ps1`
- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `.codex/progress.md`

Tests run:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- This feature brief.
- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is a local verification wrapper around existing commands and does not change architecture, persistence, security, deployment, or cleanup behavior.

Follow-up work:

- Run the manual fixture UI pass after preflight.
- Then rerun the WPF app against `C:\Users\moxhe`.

Open questions:

- Should the app later expose a built-in diagnostics/preflight screen?

Risky assumptions:

- A PowerShell wrapper around the existing commands is the clearest way to prevent skipped checks before real scans.
