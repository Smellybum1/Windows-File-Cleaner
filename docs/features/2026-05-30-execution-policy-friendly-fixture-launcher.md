# Feature: Execution-Policy Friendly Fixture Launcher

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the manual fixture review launcher easier to run on Windows systems where direct `.ps1` script execution is blocked by local execution policy.

## Non-goals

- Do not change Storage Scan, fixture creation, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.
- Do not change machine, user, or process execution policy permanently.
- Do not bypass any in-app safety gate, real-profile acknowledgement, Quarantine confirmation, or restore confirmation.
- Do not replace the existing PowerShell launcher.

## Desired behavior

- `tools\Start-MvpFixtureReview.cmd` invokes the existing `Start-MvpFixtureReview.ps1` with `powershell.exe -NoProfile -ExecutionPolicy Bypass`.
- All arguments pass through unchanged, including `-ChecklistOnly`, `-SkipPreflight`, `-SkipLaunch`, `-SkipChecklist`, `-WhatIf`, and `-FixtureRoot`.
- The wrapper exits with the PowerShell script's exit code.
- Docs offer the `.cmd` form when direct `.ps1` execution is blocked.

## Domain language changes

No new domain terms. This is local fixture-review tooling only.

## Evidence and validation gate

Evidence gathered:

- The user hit PowerShell's `running scripts is disabled on this system` error more than once while trying to run the fixture review checklist.
- The existing PowerShell launcher already runs nested scripts with process-scoped `-ExecutionPolicy Bypass`, but direct invocation of the launcher itself can be blocked before that code runs.
- Manual fixture review remains the best next product signal after recent UI/help-cue polish.

Validation gate before implementation:

- [x] Permission boundary is clear: the wrapper only starts the existing fixture launcher and does not move or delete real-profile files.
- [x] The narrowest relevant verification path is the `.cmd` wrapper in `-ChecklistOnly` mode plus the existing `.ps1` bypass invocation.
- [x] No ADR is needed because this is reversible local tooling and documentation.

## Decisions made

Small feature-level decisions:

- Add a `.cmd` wrapper instead of asking the user to change execution policy.
- Keep the wrapper in `tools\` next to the existing launcher.
- Update preflight's suggested next command to the `.cmd` wrapper because preflight already runs after a PowerShell bypass command and should point to the least-friction manual next step.

ADR-worthy decisions:

- [x] None.

## Implementation plan

1. Add `tools\Start-MvpFixtureReview.cmd`.
2. Update README, AGENTS command examples, preflight output, progress, handoff, and related fixture-checklist feature notes.
3. Verify `.cmd -ChecklistOnly`, `.ps1 -ChecklistOnly` through explicit bypass, and whitespace diff.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added `tools\Start-MvpFixtureReview.cmd`, a process-scoped execution-policy-friendly wrapper for the existing fixture launcher.
- Updated `Invoke-MvpPreflight.ps1` to suggest the `.cmd -SkipPreflight` manual next step.
- Updated docs to present `.cmd` as the preferred manual fixture-review command when script execution is blocked.
- Later packet `2026-05-30-execution-policy-friendly-tool-wrappers.md` added matching `.cmd` wrappers for standalone preflight and synthetic fixture generation, making the process-scoped bypass pattern consistent across human-facing tools.

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the manual fixture checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and preserved the original explicit-bypass path.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch` passed and forwarded arguments through the wrapper without creating fixture files or launching WPF.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1 -SkipRestore -SkipFixtureWhatIf` passed build, core tests, WPF app tests, whitespace diff check, and printed the updated `.cmd -SkipPreflight` next manual fixture step.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `AGENTS.md`
- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-execution-policy-friendly-fixture-launcher.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. This is reversible local tooling with no app architecture, persistence, cleanup execution, restore rule, data-model, or security model change.

Open questions:

- None.

Risky assumptions:

- A `.cmd` wrapper is clearer and safer for this repo than asking the user to change PowerShell execution policy settings.
