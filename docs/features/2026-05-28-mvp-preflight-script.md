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
- Prints the fixture review checklist in checklist-only mode.
- Runs the local release acceptance summary regression check.
- Runs the real-profile next-batch stop guard regression check.
- Runs `git diff --check`.
- Prints the next notes-enabled fixture review launcher command.
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
- Add `-SkipRestore`, `-SkipFixtureWhatIf`, `-SkipFixtureChecklist`, `-SkipLocalReleaseAcceptanceSummaryCheck`, `-SkipRealProfileNextBatchStopGuardCheck`, and `-SkipDiffCheck` switches for focused local loops.

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
- Preflight runs restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and `git diff --check`.
- Preflight prints the next fixture review launcher command.
- Kept the workflow read-only with respect to real user files.
- Later packet `2026-05-30-preflight-fixture-checklist-step.md` added a checklist-only fixture review step to the full preflight so manual checklist regressions fail preflight before visible review.
- Later packet `2026-05-30-ci-preflight-cmd-wrapper.md` made GitHub Actions call the preferred `.cmd` wrapper so CI verifies the same entry point as local user instructions.
- Later packet `2026-06-01-preflight-fixture-notes-next-step.md` changed the success output to suggest `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`, aligning the next manual fixture step with the acceptance-notes workflow. Later packet `Fixture Acceptance Post-Pass Guidance` added a reminder that the notes-enabled launcher prints the exact post-pass summary and completion-check commands after it writes notes. Later packet `Fixture Acceptance Notes Embedded Commands` made the generated notes file include those commands too.
- Later packet `Full Local MVP Preflight After Clean Notes Preview` reran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` after `8529a91`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output, and whitespace diff passed without launching WPF, scanning real-profile files, moving, restoring, deleting, or creating cleanup history.
- Later packet `Full Local MVP Preflight After Current Evidence Wording Alignment` reran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` after `466ad79`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output, and whitespace diff passed without launching WPF, scanning real-profile files, moving, restoring, deleting, or creating cleanup history.
- Later packet `Checklist-Only Visible Fixture Next Step` made the checklist-only output printed by preflight more self-contained by repeating the exact notes-enabled visible fixture command and the checklist-only no-preflight/no-fixture/no-WPF/no-scan/no-movement boundary.
- Later packet `Full Local MVP Preflight After Checklist-Only Next Step` reran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` after `71cf15a`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output with the exact visible fixture next-step block, and whitespace diff passed without launching WPF, scanning real-profile files, moving, restoring, deleting, or creating cleanup history.
- Later packet `MVP Preflight Release Summary Regression` added the local release acceptance summary regression step before the whitespace diff check, with `-SkipLocalReleaseAcceptanceSummaryCheck` for focused local loops.
- Later packet `MVP Preflight Next-Batch Stop Guard Regression` added the real-profile next-batch stop guard regression step before the whitespace diff check, with `-SkipRealProfileNextBatchStopGuardCheck` for focused local loops.

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
- Later full-preflight-after-clean-notes packet ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd`.
- Later full-preflight-after-current-evidence packet ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd`.
- Later checklist-only-next-step packet ran `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`, `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`, `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`, `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore`, and whitespace checks.
- Later full-preflight-after-checklist-only-next-step packet ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd`.
- Later MVP-preflight-release-summary-regression packet ran `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`, `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore`, and `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, local release acceptance summary regression, and whitespace diff passed without launching WPF, scanning real-profile files, moving, restoring, deleting, approving cleanup, or creating cleanup history.
- Later MVP-preflight-next-batch-stop-guard-regression packet ran `cmd.exe /c tools\Test-RealProfileNextBatchStopGuard.cmd` and then the updated MVP preflight; restore/build/test, fixture dry-run/checklist output, local release acceptance summary regression, real-profile next-batch stop guard regression, and whitespace diff passed without launching WPF, scanning real-profile files, moving, restoring, deleting, approving cleanup, writing real Restore Manifests, or creating cleanup history.

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
