# Feature: Shortlist-Level Quarantine Wording

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make fixture Quarantine execution clearly read as one action for all included Review Shortlist rows.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not remove exact `QUARANTINE` confirmation.
- Do not add permanent deletion or cleanup history.
- Do not change Quarantine Preview blocker behavior.

## User story / job story

As the project owner, I want one button to quarantine the files/folders on the shortlist, so that I do not think I need to visit each selected file and approve it separately.

## Current behavior

The underlying execution already uses the current Quarantine Preview, which is built from the Review Shortlist. But the visible button said `Execute quarantine`, making the action feel generic or selected-row-oriented.

## Desired behavior

- The preview button names the Review Shortlist scope.
- The execution button says `Quarantine included shortlist`.
- The confirmation tooltip says `QUARANTINE` is typed once for all included Review Shortlist rows.
- The preview/gate/status wording says blocked and redundant rows stay out of execution.
- WPF smoke coverage proves more than one included shortlist row moves and restores through the same action.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Preview | Clarified that Review Shortlist is the source and included rows are the execution target. | yes |
| Fixture-only WPF Quarantine Execution | Clarified that the visible action is shortlist-level, not selected-row execution. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is wording and smoke coverage for existing fixture-only behavior.

Questions that can be deferred:

- Should a future Quarantined Files area show the included shortlist rows immediately after execution?
- Should real-profile execution use the same button label after its separate Grill with Docs pass?

## Grill notes

### Scenarios discussed

- User expected a single action for the Review Shortlist instead of visiting each file and typing `QUARANTINE`.
- Existing Quarantine Preview and execution already operate from the Review Shortlist, but the UI did not make that obvious.

### Edge cases

- Blocked or redundant preview rows must not move.
- Real-profile/custom scopes must still remain preview-only even if the button wording is clearer.

### Dependencies between decisions

- Depends on ADR 0009 fixture-only WPF Quarantine execution.
- Keeps ADR 0010 current-fixture undo behavior unchanged.

## Evidence and validation gate

Evidence gathered:

- User feedback from manual fixture review.
- Existing code/docs inspected: WPF Quarantine Preview, execution gate, fixture execution smoke test, README, domain context, glossary, ADRs 0009 and 0010.
- Tests/checks planned: WPF app smoke test, solution build, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not remove exact confirmation while clarifying shortlist-level execution.
- Do not call the action `Quarantine all` without qualifying that only included rows move.

## Decisions made

Small feature-level decisions:

- Rename the visible execution button to `Quarantine included shortlist`.
- Keep exact `QUARANTINE` confirmation, but say it is typed once.
- Extend fixture execution smoke coverage to two included rows.

ADR-worthy decisions:

- [x] None. This is reversible UI wording and test coverage for existing ADR 0009 behavior.

## Implementation plan

1. Rename preview/execution labels and tooltips around Review Shortlist scope.
2. Add preview/gate/status text that names included Review Shortlist rows.
3. Extend WPF smoke tests to quarantine and undo two included rows.
4. Update docs and progress.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-shortlist-level-quarantine-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- No ADR expected.

## Test plan

Manual checks:

- In fixture review, add multiple eligible rows to Review Shortlist, click `Preview shortlist quarantine`, type `QUARANTINE` once, and click `Quarantine included shortlist`.

Automated tests:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- `Quarantine included shortlist` is more explicit but slightly longer.
- Users may still expect no typed confirmation at all; this packet keeps the confirmation for safety.

Assumptions:

- The right safe behavior is one exact confirmation for all included preview rows, not one confirmation per file.

## Completion notes

Completed on: 2026-05-29

What changed:

- Renamed visible preview/execution controls around shortlist-level Quarantine.
- Added preview/gate/status wording that says the Review Shortlist is the source and included rows are the execution target.
- Extended WPF smoke coverage to prove two included Review Shortlist rows move together and undo together.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-shortlist-level-quarantine-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, this feature brief, progress log, and handoff.

ADRs added or skipped:

- Skipped. This is wording/test coverage for existing fixture-only execution behavior.

Follow-up work:

- Consider a dedicated Quarantined Files area.
- Consider making Quarantine Preview success more visible than the status bar.

Open questions:

- Should future real-profile execution keep this exact label?

Risky assumptions:

- Keeping exact `QUARANTINE` confirmation is still desirable even when the button clearly describes the shortlist-level action.
