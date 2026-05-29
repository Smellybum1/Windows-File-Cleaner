# Feature: Fixture Review Checklist-Only Mode

Date started: 2026-05-29  
Status: completed  
Owner: project-owner

## Goal

Make the manual fixture review checklist printable without running preflight, creating fixture files, or launching the WPF app.

## Non-goals

- Do not scan or modify `C:\Users\moxhe`.
- Do not create synthetic fixture files in checklist-only mode.
- Do not launch WPF in checklist-only mode.
- Do not change fixture-only Quarantine execution, fixture-only restore, or real-profile cleanup availability.
- Do not replace the full README manual checklist.

## Desired behavior

`Start-MvpFixtureReview.ps1 -ChecklistOnly` should resolve and validate the fixture Cleanup Scope path, print the same compact manual fixture review checklist, and exit before any preflight, fixture creation, or WPF launch steps.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing fixture review and Cleanup Scope language used. | n/a |

## Evidence and validation gate

Evidence gathered:

- The next recommended work remains a visible fixture review pass.
- The launcher checklist is useful as a reminder, but sometimes the user may want to see it without running the whole fixture startup sequence.
- The existing fixture launcher already validates the fixture root stays inside the repository.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: checklist-only mode must not create, scan, move, restore, delete, or launch.
- [x] The narrowest relevant verification path is the launcher with `-ChecklistOnly`.

## Decisions made

Small feature-level decisions:

- Add `-ChecklistOnly` to the existing launcher instead of adding a second script.
- Keep fixture root path resolution and repository containment validation before printing the checklist.
- Let `-ChecklistOnly` run independently of `-SkipPreflight`, `-SkipLaunch`, and `-SkipChecklist`.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add `-ChecklistOnly` to `tools/Start-MvpFixtureReview.ps1`.
2. Exit before preflight, fixture creation, and WPF launch when the switch is set.
3. Update README, progress, and handoff docs.
4. Verify checklist-only mode and existing launcher dry-run behavior.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added `-ChecklistOnly` to `Start-MvpFixtureReview.ps1`.
- Checklist-only mode prints the Fixture Cleanup Scope and manual review checklist, then exits before preflight, fixture creation, and WPF launch.
- Kept repository containment validation for custom fixture roots.

Tests run:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly -FixtureRoot ".local\storage-scan-smoke-fixture"`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch -SkipChecklist`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This is a local workflow-output option with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- After the next visible fixture review, should the checklist be shortened or split into grouped prompts?

Risky assumptions:

- A checklist-only mode will help manual review without encouraging the user to skip the actual fixture run.
