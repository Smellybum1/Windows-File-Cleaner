# Feature: Fixture Checklist Selected Restore Scope

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Keep the fixture review launcher checklist aligned with the Selected Restore Execution Gate scope-status and approval-boundary wording.

## Non-goals

- Do not launch the app in automated checks.
- Do not auto-scan when WPF opens.
- Do not scan or modify `C:\Users\moxhe`.
- Do not change selected restore execution, Quarantine execution, Undo Quarantine, permanent deletion, or cleanup history availability.

## Desired behavior

The compact checklist printed by `Start-MvpFixtureReview.ps1` should remind the user to check `Approval boundary` and `Execution scope status` in the Selected Restore Execution Gate during fixture review.

It should also explicitly remind the user that real-profile/custom Quarantine and selected restore execution must remain unavailable before any later real-profile work.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing Selected Restore Execution Gate wording used. | n/a |

## Evidence and validation gate

Evidence gathered:

- `2026-05-29-selected-restore-scope-status.md` added WPF selected restore gate `Execution scope status` and `Approval boundary` wording.
- `Start-MvpFixtureReview.ps1` still mentioned selected readiness/gate generically, without calling out the new selected restore scope-status line.
- The next recommended work remains manual fixture review.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: script output only, no filesystem mutation in automated checks.
- [x] The narrowest relevant verification path is launcher `-ChecklistOnly` plus `git diff --check`.

## Decisions made

Small feature-level decisions:

- Update existing checklist lines instead of adding more checklist items.
- Mention selected restore Approval boundary plus Execution scope status in one line to keep the terminal checklist compact.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update `Start-MvpFixtureReview.ps1` checklist wording for selected restore scope status.
2. Update README and related feature notes.
3. Record progress and handoff updates.
4. Verify checklist output without preflight, fixture creation, or WPF launch.

## Test plan

Manual checks:

- During visible fixture review, confirm the checklist prompt leads to checking selected restore scope status and approval-boundary wording.

Automated tests:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- The selected restore checklist line is longer, so the terminal prompt may feel denser.

Assumptions:

- One compact selected restore checklist line is enough to draw attention without making the checklist noisy.

## Completion notes

Completed on: 2026-05-29

What changed:

- Updated the fixture launcher checklist to check selected restore `Approval boundary` plus `Execution scope status`.
- Updated the real/custom blocker reminder to name both Quarantine and selected restore execution.
- Updated README and feature notes to match the latest launcher checklist.

Tests run:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-selected-restore-scope-status.md`
- `docs/features/2026-05-29-fixture-checklist-selected-restore-scope.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. This is local checklist wording with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Use the checklist during the next visible fixture review and decide whether to shorten or group prompts.

Open questions:

- After visible fixture review, should selected restore scope-status wording stay in the compact checklist or move only to README?

Risky assumptions:

- The extra selected restore wording is worth the terminal checklist density.
