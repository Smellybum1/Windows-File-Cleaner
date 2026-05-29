# Feature: Fixture Checklist Broad Restore Wording

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Keep the fixture review launcher checklist aligned with the broad-restore boundary wording now shown in discovery and readiness panes.

## Non-goals

- Do not launch the app in automated checks.
- Do not auto-scan when WPF opens.
- Do not scan or modify `C:\Users\moxhe`.
- Do not change Quarantine execution, selected restore execution, Undo Quarantine, permanent deletion, or cleanup history availability.

## Desired behavior

The compact checklist printed by `Start-MvpFixtureReview.ps1` should remind the user to check that discovery/readiness panes say no broad restore action is available, while selected fixture restore still goes through selected readiness and the selected restore gate.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing broad-restore and selected-restore wording used. | n/a |

## Evidence and validation gate

Evidence gathered:

- `2026-05-29-broad-restore-action-wording.md` added WPF discovery/readiness/selected-review wording that distinguishes broad restore from fixture selected restore.
- The fixture launcher checklist still called out selected restore scope status but not the new broad-restore boundary.
- The next recommended work remains manual fixture review.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: script output only, no filesystem mutation in automated checks.
- [x] The narrowest relevant verification path is launcher `-ChecklistOnly` plus `git diff --check`.

## Decisions made

Small feature-level decisions:

- Update the existing manifest-review checklist line instead of adding a new checklist item.
- Keep the terminal prompt compact by grouping no-broad-restore, selected restore approval boundary, and execution scope status in one line.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update `Start-MvpFixtureReview.ps1` checklist wording.
2. Update README and related feature notes.
3. Record progress and handoff updates.
4. Verify checklist output without preflight, fixture creation, or WPF launch.

## Test plan

Manual checks:

- During visible fixture review, confirm the checklist prompt leads to checking discovery/readiness no-broad-restore wording.

Automated tests:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- The manifest-review checklist line is becoming dense.

Assumptions:

- Grouping related restore-readiness wording in one checklist line is still more useful than omitting the broad-restore boundary.

## Completion notes

Completed on: 2026-05-29

What changed:

- Updated the fixture launcher checklist to check no broad restore action wording alongside selected restore approval boundary and execution scope status.
- Updated README and feature notes to match the latest launcher checklist.

Tests run:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-29-broad-restore-action-wording.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-checklist-broad-restore-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. This is local checklist wording with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Use the checklist during visible fixture review and decide whether line 8 should be split for readability.

Open questions:

- Is `no broad restore action` clear enough in the compact checklist?

Risky assumptions:

- The compact checklist can carry one more restore-readiness cue without becoming too noisy.
