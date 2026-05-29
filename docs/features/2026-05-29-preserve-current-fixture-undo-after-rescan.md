# Feature: Preserve Current Fixture Undo After Rescan

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Keep `Undo fixture quarantine` available after the user rescans a fixture Cleanup Scope following fixture-only Quarantine execution.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not add permanent deletion.
- Do not add persisted cleanup history.
- Do not redesign the full quarantined-files UI in this packet.

## User story / job story

As the project owner, I want fixture undo to remain reachable after a rescan removes the moved file from the review grid, so that the visible reversible workflow does not depend on the quarantined row still being selected.

## Current behavior

After fixture execution, `Undo fixture quarantine` was available while the stale scan result still displayed the moved row. A post-execution rescan refreshed the grid, the moved source row disappeared, and the app cleared the in-memory current Restore Manifest, making current-fixture undo unavailable.

## Desired behavior

- After fixture execution, status text should point to `Undo fixture quarantine` and say that rescan refreshes review rows.
- A post-execution fixture rescan should preserve the current in-memory Restore Manifest and execution result.
- `Undo fixture quarantine` should remain available after that rescan until undo is attempted.
- Post-execution rescan status should point to the Quarantine execution area where the preserved undo action lives.
- Real-profile/custom undo should remain unavailable.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| WPF Current Fixture Undo Quarantine | Clarified that current-fixture undo remains available after a post-execution rescan until undo is attempted. | yes |

## Open questions

Questions that must be answered before implementation:

- None. The user found the bug during fixture review and suggested a separate quarantined-files area as a follow-up UX direction.

Questions that can be deferred:

- Should the app add a dedicated quarantined-files area that lists current and discovered quarantined entries independently of Storage Scan rows?
- Should Quarantine Preview success use a more visible inline/pane highlight instead of only the status bar?

## Grill notes

### Scenarios discussed

- User executed fixture Quarantine, rescanned, and correctly observed that the moved file was gone from the main review grid.
- User asked how to undo if the button had been associated with the file detail area that was no longer reachable.
- User suggested a separate area for quarantined files.

### Edge cases

- The current scan snapshot can be stale immediately after execution.
- A rescan can remove the original source row because the synthetic file has moved.
- Undo should remain fixture-only and current-execution-only in this packet.

### Dependencies between decisions

- Depends on ADR 0009 fixture-only WPF Quarantine execution.
- Depends on ADR 0010 current-fixture-execution WPF undo.

## Evidence and validation gate

Evidence gathered:

- User answers: post-execution rescan removed the moved file from the grid, making the visible undo path confusing/unavailable.
- Existing code/docs inspected: `ClearQuarantinePreview`, WPF fixture execution/undo smoke test, README manual checklist, domain context, glossary, ADR 0010, current-fixture undo feature brief.
- Tests/checks planned: WPF app smoke test, solution build, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make the Storage Scan row persist after the file has moved; the rescan should reflect filesystem state.
- Do not add broad manifest history or real-profile restore while fixing current-fixture undo reachability.

## Decisions made

Small feature-level decisions:

- Preserve the current fixture execution Restore Manifest/result when clearing stale Quarantine Preview state if current-fixture undo is still available.
- Update fixture execution status wording to point to `Undo fixture quarantine` and clarify that rescan refreshes review rows.
- Record a dedicated quarantined-files area as follow-up rather than expanding this bugfix packet.

ADR-worthy decisions:

- [x] None. This refines ADR 0010 behavior without changing persistence, real-profile availability, or cleanup execution boundaries.

## Implementation plan

1. Preserve current-fixture undo state across `ClearQuarantinePreview` when undo is still available.
2. Update execution status wording.
3. Extend WPF smoke coverage to execute fixture Quarantine, rescan, confirm undo remains available, then undo.
4. Update README, domain docs, glossary, progress, handoff, and this feature brief.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-preserve-current-fixture-undo-after-rescan.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- No ADR expected.

## Test plan

Manual checks:

- In fixture review, execute fixture Quarantine, rescan, confirm the moved source row disappears, then confirm `Undo fixture quarantine` remains available in the Quarantine execution area.
- Use manifest discovery/selected restore if testing an older app build that already lost current in-memory undo state.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git diff --check`

## Risks and assumptions

Risks:

- The current-fixture undo control is still visually lower in the Quarantine execution area and may be missed.
- Current in-memory undo still disappears if the app closes; discovered manifest selected restore remains the recovery path for persisted fixture manifests.

Assumptions:

- Preserving current-fixture undo across rescan is safer than tying undo reachability to the moved row remaining visible.

## Completion notes

Completed on: 2026-05-29

What changed:

- Preserved current-fixture Restore Manifest and execution result when a post-execution rescan clears Quarantine Preview state.
- Updated fixture execution status text to point to `Undo fixture quarantine` and explain that rescan refreshes review rows.
- Updated post-execution rescan status text to point to the preserved undo action in the Quarantine execution area.
- Extended WPF smoke coverage to prove fixture undo remains available after a post-execution rescan.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-preserve-current-fixture-undo-after-rescan.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, this feature brief, progress log, and thread handoff.

ADRs added or skipped:

- Skipped. This is a bounded lifecycle bugfix for ADR 0010, not a new durable architecture or persistence decision.

Follow-up work:

- Consider a dedicated quarantined-files area that is independent of currently visible Storage Scan rows.
- Consider making Quarantine Preview success more visible than a status-bar line.

Open questions:

- Should the dedicated quarantined-files area show only current-session fixture executions at first, or should it start from Quarantine Manifest Discovery?

Risky assumptions:

- Current-fixture undo state can remain in memory through a rescan without confusing it with broad/persisted cleanup history.
