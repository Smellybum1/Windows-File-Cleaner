# Feature: Manual Fixture Show Children and Clipboard Fix

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Address manual fixture review findings around returning from `Show children` and preventing `Copy path` from crashing when the Windows Clipboard is busy.

## Non-goals

- Do not change Storage Scan results or filtering semantics.
- Do not change Review Shortlist behavior.
- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not enable permanent deletion or cleanup history.
- Do not move, restore, delete, quarantine, create, write, or clean up files or folders.

## User story / job story

As the project owner, I want `Show children` to tell me how to return to all rows and `Copy path` to tolerate a busy clipboard, so that manual fixture review can continue without confusion or app crashes.

## Current behavior

Manual fixture review showed that `Show children` worked but did not make the reverse path obvious. The intended reverse action was `Reset view`, but the status text did not say that.

The same review crashed after a selected-row action. Windows Application logs showed a `.NET Runtime` unhandled `COMException` from `Clipboard.SetText` in `CopyPathButton_Click`: `OpenClipboard Failed (CLIPBRD_E_CANT_OPEN)`.

## Desired behavior

- `Show children` status text tells the user to use `Reset view` to show all rows again.
- `Copy path` catches a busy Windows Clipboard failure and reports a warning/status instead of terminating the app.
- Existing selected-row action safety boundaries remain unchanged.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Folder Child Focus | Clarify that status text should name `Reset view` as the way back. | yes |
| Selected Path Inspection | Clarify that clipboard failures during Copy path should not crash the app. | yes |

## Open questions

Questions that must be answered before implementation:

- None. Manual fixture evidence and Windows crash logs identify the issue clearly.

Questions that can be deferred:

- Should the UI add a visible Back/All rows button near selected-folder focus, or is the improved status text enough?

## Grill notes

### Scenarios discussed

- The user clicked `Show children`, saw the narrowed row set, and did not know how to return to all rows.
- The app then crashed during manual fixture review.
- Windows logs showed the crash was from `Copy path` trying to open a busy Clipboard, not from `Show children`.

### Edge cases

- Windows Clipboard can be temporarily locked by another process.
- `Show children` uses a generated `parent:` search, so `Reset view` is the existing way back.
- Status text should remain read-only/no-file-modified.

### Dependencies between decisions

- Depends on existing Selected Folder Child Focus and Review View Reset behavior.
- Depends on existing Selected Path Inspection behavior.
- Does not affect Quarantine Preview, Quarantine execution, Undo Quarantine, selected restore, deletion, or cleanup history.

## Evidence and validation gate

Evidence gathered:

- User answer: manual fixture pass succeeded until `Show children`, where the way back was unclear; the app then crashed.
- Windows Application log: unhandled `System.Runtime.InteropServices.COMException (0x800401D0): OpenClipboard Failed` in `CopyPathButton_Click`.
- Existing code/docs inspected: WPF selected-row actions, `ShowSelectedFolderChildren`, `ResetReviewView`, selected-row action tests, README, domain context, glossary, handoff, and progress log.
- Tests/checks planned: WPF app smoke test, build, diff checks.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add a new navigation stack for this bugfix packet.
- Do not change `Show children` search semantics.
- Do not remove Copy path because the Clipboard can be transiently busy.

## Decisions made

Small feature-level decisions:

- Reuse existing `Reset view` as the way back from `Show children`.
- Catch `COMException` from `Clipboard.SetText` and show a warning/status update.

ADR-worthy decisions:

- [x] None. This is bugfix/manual-polish work with no durable architecture or data-model decision.

## Implementation plan

1. Update `ShowSelectedFolderChildren` status text to name `Reset view`.
2. Catch busy Clipboard failures in `CopyPathButton_Click`.
3. Add WPF smoke coverage for the `Reset view` status guidance.
4. Update README, domain docs, progress, handoff, and this feature brief.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Restart fixture review, scan, select a folder, click `Show children`, and confirm the status says `Reset view` returns to all rows.
- Click `Reset view` and confirm all rows return.
- Click `Copy path`; if the Clipboard is busy, the app should stay open and show a warning/status.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git diff --check`

## Risks and assumptions

Risks:

- Clipboard contention is timing-sensitive and not directly simulated in the current WPF smoke harness.

Assumptions:

- `COMException` is the relevant failure type for the observed Clipboard lock crash.
- Naming `Reset view` in status text is enough for the next manual pass; a dedicated Back button can be considered if confusion persists.

## Completion notes

Completed on: 2026-05-29

What changed:

- `Show children` status text now says to use `Reset view` to show all rows again.
- `Copy path` catches busy Windows Clipboard `COMException` and reports a warning/status instead of crashing.
- Added WPF smoke coverage for the `Reset view` way-back wording.
- Updated README, domain docs, progress, handoff, and this feature brief.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-manual-fixture-show-children-clipboard-fix.md`
- `.codex/progress.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, handoff, this feature brief, and progress log.

ADRs added or skipped:

- No ADR added. This is a bounded manual fixture bugfix with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Rerun the manual fixture review from `Show children`.
- Consider an explicit visible Back/All rows affordance if `Reset view` remains too hidden.

Open questions:

- Is naming `Reset view` in status text enough, or should the UI add a dedicated way-back button near selected-folder focus?

Risky assumptions:

- The observed crash is fully explained by the Windows Clipboard `COMException` in the event log.
