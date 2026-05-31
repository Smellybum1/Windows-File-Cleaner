# Feature: Fixture Checklist Selected Restore Revalidation Alignment

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Align the manual fixture review checklist with the new WPF Selected Restore Pre-Execution Revalidation evidence.

## Non-goals

- Do not change WPF behavior.
- Do not enable real-profile selected restore execution.
- Do not enable real-profile Quarantine execution.
- Do not add permanent deletion or cleanup history.
- Do not scan, move, restore, delete, create, or rewrite real-profile files.

## Current behavior

The fixture launcher checklist now asks the reviewer to check read-only selected restore revalidation evidence for exact real-profile Restore Manifests when available, and to confirm that clean selected restore revalidation evidence still does not unlock real-profile/custom Quarantine or selected restore execution.

README manual review wording now mirrors that boundary.

## Decisions made

- Keep this as checklist/docs alignment only because the WPF behavior and tests were added in the prior packet.
- Keep the checklist concise enough for terminal use instead of adding a new long step.

## Files changed

- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/features/2026-05-31-fixture-checklist-selected-restore-revalidation.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## Test plan

Automated checks:

- `.\tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Updated the checklist and README manual review wording for selected restore revalidation evidence.
- Refreshed handoff/startup guidance so new threads start from the current pushed boundary.

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

Follow-up work:

- Run a manual fixture review when the user wants another visible pass.

Open questions:

- None for this checklist packet.

Risky assumptions:

- A concise checklist mention is enough because the detailed WPF behavior is already covered by app smoke tests and feature docs.
