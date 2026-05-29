# Feature: Review Grid Mode Status Styling

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make Review Grid Mode Status easier to notice when the main grid is showing stale Storage Scan rows or Current-Session Quarantined Review rows, without changing cleanup behavior.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not add permanent deletion.
- Do not add persisted cleanup history.
- Do not merge discovered Restore Manifests into the current-session quarantined grid.
- Do not add another visible row or modal.

## User story / job story

As the project owner, I want the main grid mode line to visually distinguish ordinary scan rows from stale scan warnings and current-session quarantined review, so I can avoid confusing refreshed scan evidence with current-session quarantine evidence.

## Current behavior

Review Grid Mode Status text names the active grid mode and warns when scan rows may be stale, but every state uses the same muted text styling.

## Desired behavior

- Ordinary Storage Scan rows use neutral styling.
- Current-Session Quarantined Review uses informational styling.
- Stale Storage Scan rows after fixture Quarantine execution use warning styling.
- Empty current-session quarantined view uses warning styling.
- Wording continues to say the view is read-only and that no files were modified where applicable.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Review Grid Mode Status | Clarify that the status may use lightweight semantic styling while remaining read-only. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is visual emphasis only.

Questions that can be deferred:

- Should the status eventually use a badge instead of styled text?

## Grill notes

### Scenarios discussed

- During manual fixture review, after fixture Quarantine execution and rescan, the moved synthetic file disappeared from Storage Scan rows as expected.
- The app then needed clearer cues that current-session quarantined entries and stale scan rows are separate review modes.

### Edge cases

- Styling must not imply cleanup approval or refreshed scan evidence.
- The current-session quarantined view remains read-only and current-session-only.

### Dependencies between decisions

- This depends on the existing `ReviewGridModeText`; no new workflow surface is needed.

## Evidence and validation gate

Evidence gathered:

- User answers: the post-execution rescan made moved files disappear from Storage Scan rows, raising discoverability questions around how to undo/review quarantined files.
- Existing code/docs inspected: `UpdateReviewGridModeText`, Review Grid Mode Status feature brief, Current-Session Quarantined Review domain docs, glossary, progress log, handoff.
- Tests/checks planned: WPF app smoke tests, focused app test build, solution build, `git diff --check`.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add a modal, tab, or persistent cleanup history just to distinguish grid modes.

## Decisions made

Small feature-level decisions:

- Use text foreground and semibold weight only, avoiding layout expansion.
- Keep the existing Review Grid Mode Status location above the grid.
- Use styling from current grid mode/stale state, not additional persisted state.

ADR-worthy decisions:

- [x] None expected. This is reversible WPF styling with no persistence, file movement rule, restore rule, data-model, or security change.

## Implementation plan

1. Add a small status-style helper for `ReviewGridModeText`.
2. Apply neutral, information, and warning styles in existing grid-mode update paths.
3. Add focused WPF smoke assertions for ordinary scan rows, stale scan rows, current-session quarantined rows, and empty moved-entry state.
4. Update README, domain docs, progress, and handoff.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-grid-mode-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- During the next fixture visual pass, confirm the styled grid-mode line is noticeable without crowding the grid.

Automated tests:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- Styling might still be too subtle during manual review.
- Too much emphasis could make stale scan rows look like an action prompt rather than review context.

Assumptions:

- Lightweight semantic styling is safer than adding another control or modal because the main grid remains read-only.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added semantic styling for `ReviewGridModeText`.
- Ordinary Storage Scan rows use neutral styling.
- Current-Session Quarantined Review rows use informational styling.
- Stale Storage Scan rows after fixture Quarantine execution use warning styling.
- Empty current-session quarantined rows after moved entries are gone use warning styling.
- Added WPF smoke assertions for neutral, informational, warning, stale-return, and empty moved-entry styling.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `docs/features/2026-05-29-review-grid-mode-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, Review Grid Mode Status brief, this feature brief, progress log, and handoff.

ADRs added or skipped:

- Skipped. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.

Follow-up work:

- Manual fixture visual pass to confirm the styled grid-mode line is noticeable without crowding the grid.

Open questions:

- Should the status eventually use a badge instead of styled text?

Risky assumptions:

- Lightweight styling is enough to distinguish stale scan rows from current-session quarantined review until the next manual fixture pass.
