# Feature: Review Grid Mode Status

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the active main-grid mode visible when the WPF app switches between Storage Scan rows and current-session quarantined rows.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not add permanent deletion.
- Do not add persisted cleanup history.
- Do not merge discovered Restore Manifests into the current-session quarantined grid.

## User story / job story

As the project owner, I want the main grid to clearly say what it is showing, so I can tell whether I am reviewing scan rows or current-session quarantined entries after using `Quarantined` / `Back to scan rows`.

## Desired behavior

- Show a compact line above the main grid naming the active grid mode.
- For Storage Scan rows, show the current display range and whether current-session quarantined items are available.
- After fixture Quarantine execution, warn that Storage Scan rows may be stale until rescan.
- For Current-Session Quarantined Review, show that the grid is read-only, current-session-only, and returns with `Back to scan rows`.
- After undo removes current moved entries, stop advertising current quarantined rows.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Review Grid Mode Status | Added as visible WPF text that names whether the main grid is showing Storage Scan rows or Current-Session Quarantined Review rows. | yes |

## Open questions

- Later packet `2026-05-29-review-grid-mode-status-styling.md` chose lightweight styled text for now. The remaining open question is whether a small badge would help after manual fixture review.

## Decisions made

Small feature-level decisions:

- Put the mode status above the main grid, after the Quarantine shortlist panel, so it remains tied to the grid without adding another button.
- Keep the wording read-only and current-session-only.

ADR-worthy decisions:

- [x] None expected. This is reversible WPF status text with no persistence, file movement rule, restore rule, data-model, or security change.

## Implementation plan

1. Add `ReviewGridModeText` above the main grid.
2. Update it when scan rows refresh, quarantined rows show, scan rows return, execution changes current quarantined rows, or undo clears them.
3. Extend WPF smoke tests for the mode text through fixture execution, grid switching, and undo.
4. Update README, domain docs, progress, and handoff.

## Test plan

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- Another text line above the grid adds a small amount of vertical pressure.

Assumptions:

- A compact line is worth the space because the grid can now show two different row types.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added `ReviewGridModeText` above the main grid.
- Storage Scan mode now names the scan row display window and says whether current-session quarantined items are available.
- Current-session quarantined mode now identifies the read-only Restore Manifest view and points to `Back to scan rows`.
- Fixture execution state now warns that scan rows may be stale and points to the quarantined view.
- Fixture undo state stops advertising moved current-session entries once no moved entries remain.
- Later packet `2026-05-29-review-grid-mode-status-styling.md` added neutral/informational/warning styling to the same status line without adding layout.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
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

- README, domain context, glossary, this feature brief, progress log, and handoff.
- Later checklist alignment also updated `tools/Start-MvpFixtureReview.ps1` and `docs/features/2026-05-29-fixture-review-checklist-output.md` so the terminal fixture checklist prompts Review Grid Mode Status.

ADRs added or skipped:

- Skipped. This is reversible UI status text.

Follow-up work:

- Manual fixture visual pass to confirm the extra status line helps more than it crowds the grid.

Open questions:

- Lightweight styled text is implemented. After manual fixture review, decide whether a small badge would help more than styled text alone.

Risky assumptions:

- The extra line is acceptable now that the Safety Summary and Quarantine shortlist panels are collapsible.
