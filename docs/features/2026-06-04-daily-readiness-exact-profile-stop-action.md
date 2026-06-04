# Feature: Daily Readiness Exact-Profile Stop Action

Date started: 2026-06-04
Status: completed
Owner: project-owner

## Goal

Make daily readiness state the next action when exact-profile undo work is present, not only show the Restore Manifest spotlight.

## Non-goals

- Do not change real-profile Quarantine eligibility.
- Do not change selected restore eligibility.
- Do not launch WPF.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, approve cleanup, write real Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

## Desired behavior

- `Invoke-DailyLocalReadiness.cmd` keeps printing the broad Restore Manifest summary and exact-profile undo-work spotlight.
- After the spotlight, daily readiness prints that nonzero displayed exact-profile undo work means another next-batch review must not be run or treated as movement evidence.
- The reminder points to selected-manifest restore if recovery is needed, or a new Grill with Docs pass before another tiny real-profile batch.
- The existing synthetic daily readiness spotlight regression verifies the reminder without depending on local accepted-package evidence or real Restore Manifests.

## Decisions made

Small feature-level decisions:

- Keep this as terminal-readonly wording, not a new gate or state parser.
- Print the reminder unconditionally after the spotlight so the next action is visible in both current and synthetic daily readiness output.
- Preserve the existing next-batch evidence preset as the hard blocking command for movement evidence.

ADR-worthy decisions:

- [x] None.

## Completion notes

Completed on: 2026-06-04

What changed:

- `Invoke-DailyLocalReadiness.cmd` now prints an exact-profile stop-state action reminder after the exact-profile undo-work spotlight.
- `tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd` now asserts that reminder in focused synthetic Restore Manifest-only mode.
- `docs/operations/daily-use.md` describes the reminder and updates skip-switch documentation wording to the exact-set gate.
- Compact current-state, progress, and thread-handoff docs name this packet and the stricter daily readiness stop-state guidance.

Tests run:

- `cmd.exe /c tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd`
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

Docs updated:

- This feature brief.
- `docs/features/index.md`
- `docs/features/2026-06-04-daily-readiness-exact-profile-undo-spotlight.md`
- `docs/operations/daily-use.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This is terminal-only wording and regression coverage for existing ADR 0017/0018/0019 stop boundaries and does not change cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

Open questions:

- None.

Follow-up work:

- Keep the hard next-batch stop guard in `Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence`; this reminder is guidance, not substitute movement evidence.

Risky assumptions:

- Printing the reminder unconditionally after the spotlight is clearer than adding a second parser around Restore Manifest summary output.
