# Feature: Fixture Checklist Approval Boundary

Date started: 2026-05-29  
Status: completed  
Owner: project-owner

## Goal

Keep the fixture review launcher checklist aligned with the Quarantine approval-boundary wording now shown in the WPF preview and gate panes.

## Non-goals

- Do not launch the app in automated checks.
- Do not auto-scan when WPF opens.
- Do not scan or modify `C:\Users\moxhe`.
- Do not change Quarantine Preview eligibility, execution gates, restore behavior, or real-profile cleanup availability.

## Desired behavior

The compact checklist printed by `Start-MvpFixtureReview.ps1` should remind the user to check both `Approval boundary` and `Execution scope status` in Quarantine Preview/Gate output during the next fixture review pass.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing Quarantine Preview and approval-boundary wording used. | n/a |

## Evidence and validation gate

Evidence gathered:

- `2026-05-29-quarantine-approval-boundary-wording.md` added WPF preview/gate approval-boundary text.
- The fixture launcher checklist still named Execution scope status but not the new approval-boundary line.
- The next recommended work remains manual fixture review.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: script output only, no filesystem mutation in automated checks.
- [x] The narrowest relevant verification path is launcher `-ChecklistOnly` plus `git diff --check`.

## Decisions made

Small feature-level decisions:

- Update the existing checklist line instead of adding another checklist item.
- Keep the wording compact so the terminal checklist stays useful.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update `Start-MvpFixtureReview.ps1` checklist wording.
2. Update README and checklist feature notes.
3. Record progress and handoff updates.
4. Verify checklist output without preflight, fixture creation, or WPF launch.

## Completion notes

Completed on: 2026-05-29

What changed:

- Updated the fixture launcher checklist to check `Approval boundary` plus `Execution scope status`.
- Updated README and feature notes so manual review prompts match the latest WPF panes.

Tests run:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-checklist-approval-boundary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This is local checklist wording with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- After visible fixture review, should the checklist be shortened or split into grouped prompts?

Risky assumptions:

- Mentioning both Approval boundary and Execution scope status in one checklist item is enough to draw attention without making the checklist too long.
