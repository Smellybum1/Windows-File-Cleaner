# Feature: Visible Row Shortlist Labels

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the Review Shortlist bulk actions clearer by naming their scope as visible rows.

## Non-goals

- Do not change which rows are added or removed.
- Do not persist the Review Shortlist.
- Do not change Quarantine Preview eligibility.
- Do not move, restore, delete, create, or modify scanned files.
- Do not enable real-profile Quarantine execution.

## User story / job story

As the project owner, I want the bulk shortlist buttons to say they affect visible rows, so that I do not confuse them with actions over all matched rows or the whole scan.

## Current behavior

The buttons say `Shortlist shown` and `Remove shown`. They already affect only the current Storage Review Display Window, but the label is a little less explicit than the underlying behavior.

## Desired behavior

- The bulk add button says `Shortlist visible rows`.
- The bulk remove button says `Remove visible rows`.
- Status text also says visible rows.
- The fixture launcher checklist prompts visible-row shortlist label review.
- The behavior remains limited to currently displayed rows.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Review Shortlist | Clarify that bulk actions are labeled as visible-row actions and still use only currently displayed rows. | yes |

## Evidence and validation gate

Evidence gathered:

- Handoff guidance prefers Review Shortlist safety context before any real cleanup execution.
- Domain docs already define Storage Review Display Window as the current slice of matched rows.
- Existing WPF smoke tests cover bulk shortlist add/remove behavior.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: UI wording and tests only, no filesystem mutation.
- [x] The narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.

Rejected ideas buffer:

- Do not rename internal method names in this packet; keep the change user-facing and low-risk.
- Do not make the action apply to all matched rows.

## Decisions made

Small feature-level decisions:

- Use `visible rows` in button labels and status text.
- Keep method names and existing behavior unchanged for a focused packet.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update WPF button labels and status text.
2. Add WPF smoke assertions for the visible-row button labels.
3. Update README, domain docs, feature notes, progress, and handoff.
4. Run WPF smoke tests, build, checklist-only output, and diff check.

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During fixture review, confirm the buttons fit the toolbar and clearly refer to the current visible grid rows.

## Risks and assumptions

Risks:

- Longer button labels may need a later visual pass if the toolbar feels crowded on narrow windows.

Assumptions:

- `Visible rows` is clearer than `shown` for the current displayed review window.

## Completion notes

Completed on: 2026-05-29

What changed:

- Renamed WPF bulk shortlist buttons to `Shortlist visible rows` and `Remove visible rows`.
- Updated bulk shortlist status text to say visible rows.
- Updated fixture launcher checklist wording to prompt visible-row shortlist label review.
- Added WPF smoke assertions for the button labels.
- Updated docs and handoff notes.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, related feature briefs, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible UI wording clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Check during visible fixture/real-profile review whether the longer labels fit comfortably.

Open questions:

- Should a later layout pass add icons or tooltips for Review Shortlist bulk actions?

Risky assumptions:

- The longer labels improve clarity more than they add toolbar density.

Follow-up note:

- The later 2026-05-29 visible-row shortlist tooltip clarity packet added scope/no-file-modified/not-cleanup-approval tooltips for these bulk actions.
