# Feature: Quarantined Review Mode

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make current fixture Quarantine execution and undo controls visible outside the selected-row detail pane, and add a read-only way to switch the main grid to current-session quarantined items.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not add permanent deletion.
- Do not add persisted cleanup history.
- Do not restore discovered manifests from the new grid view.

## User story / job story

As the project owner, I want Quarantine controls and quarantined entries to live in their own area, so I can execute or undo the current fixture action without needing the moved source row to remain visible in Storage Scan results.

## Desired behavior

- Move the Quarantine shortlist execution gate out of the right scrollable selected-row detail panel.
- Group Quarantine Root Selection, preview/export, exact `QUARANTINE` confirmation, fixture execution, current-fixture undo, and gate text in one visible Quarantine shortlist area.
- Add a `Quarantined` button that switches the main grid to current-session `Moved` Restore Manifest entries.
- Add a `Back to scan rows` button that returns to the normal Storage Scan grid.
- Keep the quarantined view read-only and current-session fixture-only for this packet.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Current-Session Quarantined Review | Added as the read-only grid view for current in-memory Restore Manifest entries still in Moved state. | yes |
| WPF Current Fixture Undo Quarantine | Clarified that current fixture Restore Manifest state can keep moved entries visible after a post-execution rescan. | yes |

## Open questions

Questions that can be deferred:

- Should a future version merge current-session entries with read-only Quarantine Manifest Discovery results?
- Should discovered quarantined items get their own tab or share the same `Quarantined` view after broader restore design?

## Decisions made

Small feature-level decisions:

- Start the `Quarantined` view from the current in-memory Restore Manifest only.
- Show only entries still in `Moved` state as current quarantined items.
- Keep older/discovered manifest review in the existing read-only discovery panes.

ADR-worthy decisions:

- [x] None expected. This is UI placement and read-only current-session review for existing ADR 0009/0010 behavior.

## Implementation plan

1. Move Quarantine root, preview, confirmation, execution, undo, and gate text into a dedicated Quarantine shortlist area above the grid.
2. Add a read-only quarantined-items grid in the main grid position.
3. Add `Quarantined` and `Back to scan rows` buttons and public test hooks.
4. Extend WPF smoke tests for the current-session quarantined view.
5. Update README, domain docs, glossary, progress, handoff, and completion notes.

## Test plan

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- A current-session-only `Quarantined` view may not satisfy future older-manifest review needs.
- The area above the grid can become dense if too many restore controls are added there later.

Assumptions:

- The safest small step is current-session review only, with discovered manifest review left in the existing read-only pane.

## Completion notes

Completed on: 2026-05-29

What changed:

- Moved Quarantine Root Selection, preview/export, confirmation text, fixture execution, current-fixture undo, and Quarantine Execution Gate text into a dedicated Quarantine shortlist area above the grid.
- Made the Quarantine shortlist area collapsible and kept verbose gate details in a constrained scroll area so the main grid remains usable.
- Made the Safety Summary section collapsible to recover more review-grid height during focused review.
- Added a `Quarantined` button that switches the main grid to current-session `Moved` Restore Manifest entries.
- Added `Back to scan rows` to return to normal Storage Scan rows.
- Added WPF smoke coverage for the current-session quarantined view after execution, after post-execution rescan, and after undo.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `src/WindowsFileCleaner.App/QuarantinedItemRow.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantined-review-mode.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Post-completion UI polish:

- User review showed the main grid became too short after the Quarantine shortlist area moved above it.
- The Quarantine shortlist and Safety Summary sections are now collapsible.
- The Quarantine Execution Gate detail text remains available but constrained to a small scroll viewport.
- User verified the collapsible panel pass worked and asked for useful closed-panel summaries.
- Collapsed Safety Summary now shows compact risk counts.
- Collapsed Quarantine shortlist now shows shortlist, preview, current quarantined, and undo state.

Docs updated:

- README, domain context, glossary, this feature brief, progress log, and handoff.

ADRs added or skipped:

- Skipped. This is a reversible UI placement and read-only current-session review packet under ADRs 0009 and 0010.

Follow-up work:

- Decide whether a future `Quarantined` view should include discovered Restore Manifest entries or remain current-session only.
- Consider making Quarantine Preview success/readiness more prominent than status-bar text.

Open questions:

- Should discovered manifests eventually appear in the same grid view, a tab, or stay in the existing manifest discovery panes?

Risky assumptions:

- Current-session-only quarantined review is enough for the next manual fixture pass and avoids implying persisted cleanup history.
