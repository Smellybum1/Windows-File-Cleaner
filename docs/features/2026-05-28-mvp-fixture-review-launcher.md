# Feature: MVP Fixture Review Launcher

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Add one local command for the manual fixture UI pass before rerunning Storage Scan against `C:\Users\moxhe`.

## Non-goals

- Do not scan `C:\Users\moxhe`.
- Do not auto-scan when the WPF app opens.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or persistent scan history.
- Do not replace the real-profile manual retest.

## User story / job story

As the project owner, I want one command that runs the safe preflight, creates the synthetic Cleanup Scope, and launches the WPF app against it, so that the manual fixture UI pass is harder to skip or mis-run.

## Current behavior

The repo has a preflight script and a fixture creation script. README then asks the user to copy the printed `dotnet run --project ... -- --scope "<fixture path>"` command manually.

## Desired behavior

The repo should include a script that:

- Runs MVP preflight by default.
- Creates the synthetic fixture Cleanup Scope inside the repo.
- Launches the WPF app with the fixture path in the Cleanup Scope box.
- Clearly states that the app will not auto-scan and the user must click `Scan`.
- Supports `-WhatIf` so the command can be tested without running preflight, writing fixture files, or launching WPF.
- Supports `-SkipPreflight` and `-SkipLaunch` for focused loops.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope | No change. | not needed |
| Storage Scan | No change. | not needed |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should the app later include an in-app fixture/demo mode for visual QA?

## Evidence and validation gate

Evidence gathered:

- Progress log identifies the manual fixture UI pass as the next recommended work.
- Existing preflight covers restore/build/tests/fixture dry-run/diff check.
- Existing fixture launch support only pre-fills the Cleanup Scope and does not auto-scan.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make the launcher scan automatically.
- Do not point the launcher at `C:\Users\moxhe`.
- Do not make fixture creation happen from production app code.

## Decisions made

Small feature-level decisions:

- Add `tools/Start-MvpFixtureReview.ps1`.
- Run preflight by default unless `-SkipPreflight` is passed.
- Keep fixture output constrained to the repo.
- Use `-WhatIf` verification for the launcher to avoid starting a visible app during automated checks.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add the fixture review launcher script.
2. Update README and AGENTS project commands.
3. Update MVP audit and progress log.
4. Verify launcher `-WhatIf` plus MVP preflight.

## Files expected to change

Expected:

- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

Possible:

- None

## Test plan

Manual checks:

- Run `.\tools\Start-MvpFixtureReview.ps1` to launch the visible fixture review app.
- Click `Scan` manually after the app opens.

Automated tests:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- Launching the WPF app remains a human manual step, so visual quality still depends on user review.

Assumptions:

- A launcher script is a better next step than adding more invisible WPF state assertions.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `tools/Start-MvpFixtureReview.ps1`.
- The launcher runs MVP preflight by default, creates the synthetic fixture Cleanup Scope inside the repo, and launches the WPF app with the fixture scope.
- The launcher states that the app will not auto-scan and the user must click `Scan`.
- Kept the workflow away from `C:\Users\moxhe` unless the user later runs the normal app manually.

Files changed:

- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

Tests run:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- This feature brief.
- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is a local workflow launcher around existing preflight, fixture, and WPF launch paths; it does not change architecture, persistence, security, deployment, or cleanup behavior.

Follow-up work:

- Run `.\tools\Start-MvpFixtureReview.ps1` without `-WhatIf`, click `Scan`, and inspect the fixture UI by eye.
- Then rerun the WPF app against `C:\Users\moxhe`.

Open questions:

- Should the app later include an in-app fixture/demo mode for visual QA?

Risky assumptions:

- A launcher script is enough to make the manual fixture UI pass easier to run consistently.
