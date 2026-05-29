# Feature: Execution-Policy Friendly Tool Wrappers

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make every human-facing PowerShell tool in `tools\` runnable through a matching `.cmd` wrapper when direct `.ps1` execution is blocked by local PowerShell policy.

## Non-goals

- Do not change Storage Scan, fixture creation semantics, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.
- Do not change machine, user, or process execution policy permanently.
- Do not bypass any in-app safety gate, real-profile acknowledgement, Quarantine confirmation, or restore confirmation.
- Do not remove the existing PowerShell scripts.

## Desired behavior

- `tools\Invoke-MvpPreflight.cmd` invokes `Invoke-MvpPreflight.ps1` with `powershell.exe -NoProfile -ExecutionPolicy Bypass`.
- `tools\New-StorageScanSmokeFixture.cmd` invokes `New-StorageScanSmokeFixture.ps1` with `powershell.exe -NoProfile -ExecutionPolicy Bypass`.
- All arguments pass through unchanged.
- Each wrapper exits with the wrapped PowerShell script's exit code.
- Docs prefer `.cmd` for user-facing commands while keeping explicit `.ps1` / `powershell.exe -ExecutionPolicy Bypass -File ...` examples for diagnostics and CI.

## Domain language changes

No new domain terms. This is local tooling only.

## Evidence and validation gate

Evidence gathered:

- The user hit PowerShell's `running scripts is disabled on this system` error while running fixture-review commands.
- The fixture launcher already has a `.cmd` wrapper, but standalone preflight and fixture-generation commands still appeared in docs as direct `.ps1` commands.
- Preflight and fixture generation are user-facing safety/review workflows, so the same process-scoped bypass pattern should be available there too.

Validation gate before implementation:

- [x] Permission boundary is clear: wrappers only start existing scripts and do not change cleanup availability.
- [x] The narrowest relevant verification path is each `.cmd` wrapper with non-destructive arguments plus diff check.
- [x] No ADR is needed because this is reversible local tooling and documentation.

## Decisions made

Small feature-level decisions:

- Add one `.cmd` wrapper per human-facing PowerShell tool.
- Keep wrappers minimal and colocated in `tools\`.
- Prefer `.cmd` in README and AGENTS for manual commands.

ADR-worthy decisions:

- [x] None.

## Implementation plan

1. Add `tools\Invoke-MvpPreflight.cmd`.
2. Add `tools\New-StorageScanSmokeFixture.cmd`.
3. Update README, AGENTS, progress, handoff, and related feature notes.
4. Verify wrapper argument forwarding and preflight behavior.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added `.cmd` wrappers for preflight and synthetic fixture generation.
- Updated user-facing docs to prefer `.cmd` commands for preflight, fixture generation, and fixture review.

Tests run:

- `cmd.exe /c tools\New-StorageScanSmokeFixture.cmd -WhatIf` passed and showed intended fixture writes only.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the manual fixture checklist without preflight, fixture creation, or WPF launch.
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore` passed build, core tests, WPF app tests, fixture dry run, and whitespace diff check.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `AGENTS.md`
- `README.md`
- `docs/features/2026-05-30-execution-policy-friendly-fixture-launcher.md`
- `docs/features/2026-05-30-execution-policy-friendly-tool-wrappers.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. This is reversible local tooling with no app architecture, persistence, cleanup execution, restore rule, data-model, or security model change.

Open questions:

- None.

Risky assumptions:

- Keeping the wrappers tiny and explicit is clearer than adding a larger launcher framework or asking users to modify PowerShell policy.
