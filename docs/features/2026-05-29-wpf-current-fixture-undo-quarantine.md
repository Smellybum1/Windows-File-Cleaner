# Feature: WPF Current Fixture Undo Quarantine

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add visible WPF Undo Quarantine for the current fixture-only Quarantine execution.

## Non-goals

- Do not enable real-profile WPF Undo Quarantine.
- Do not discover or restore old manifests.
- Do not add cleanup history.
- Do not permanently delete files.
- Do not clean up quarantine folders.
- Do not overwrite existing original paths.

## User story / job story

As the project owner, I want the visible fixture workflow to move a synthetic file and then undo that move, so that the reversible cleanup promise is proven before real-profile cleanup is enabled.

## Current behavior

The WPF app can execute Quarantine for recognized fixture Cleanup Scopes after preview readiness and exact confirmation. It shows execution evidence and stale-state wording, but there is no visible undo action.

## Desired behavior

- After fixture-only WPF Quarantine execution, WPF enables `Undo fixture quarantine`.
- Clicking undo calls `UndoQuarantineExecutor.Undo` for the current Restore Manifest.
- The source file/folder is restored from quarantine when safe.
- The Restore Manifest status is updated to restored or recovery-review state.
- The app shows undo results and keeps real-profile undo unavailable.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| WPF Current Fixture Undo Quarantine | Added as the visible undo path for the current fixture execution only. | yes |

## Open questions

Questions that must be answered before implementation:

- None. ADR 0010 selects current-fixture-execution WPF undo.

Questions that can be deferred:

- What UI should discover and select old Restore Manifests?
- Should successful undo offer to clean up empty action folders?
- What additional confirmation should real-profile undo require?

## Grill notes

### Scenarios discussed

- The user asked for quarantine on `D:` with easy undo.
- Core undo is fixture-tested.
- WPF fixture execution now produces a current Restore Manifest that can be undone without manifest discovery.

### Edge cases

- No current execution.
- Undo attempted twice.
- Original path collision.
- Manifest write failure after restore.
- Current scan remains stale after execute and undo.

### Dependencies between decisions

- Depends on ADR 0008 fixture-first Undo Quarantine.
- Depends on ADR 0009 fixture-only WPF Quarantine execution.
- Adds ADR 0010 current-fixture-execution WPF undo.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: WPF fixture execution, `UndoQuarantineExecutor`, Restore Manifest status, WPF app tests, README, domain docs, progress log.
- Tests/checks planned: WPF fixture execute-then-undo smoke test, custom non-fixture undo remains unavailable, build, both test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add manifest discovery in this packet.
- Do not enable real-profile undo.
- Do not delete quarantine folders after undo.
- Do not implement restore movement in WPF.

## Decisions made

Small feature-level decisions:

- Add a WPF undo action for the current execution result only.
- Disable repeat undo after an undo attempt.
- Keep stale-state wording after undo.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`

## Implementation plan

1. Add ADR 0010 and this feature brief.
2. Add WPF state/result fields and an `Undo fixture quarantine` button.
3. Wire WPF undo to `UndoQuarantineExecutor.Undo`.
4. Extend WPF smoke tests to execute and undo a fixture file.
5. Update README, domain docs, glossary, audit, and progress.
6. Run preflight, commit, push, and verify CI.

## Files expected to change

Expected:

- `docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`
- `docs/features/2026-05-29-wpf-current-fixture-undo-quarantine.md`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

Possible:

- No core code expected unless WPF exposes missing status helpers.

## Test plan

Manual checks:

- In fixture review, execute fixture Quarantine and then undo it.
- Confirm real-profile execution and undo stay unavailable.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- WPF undo can still hit recovery-review cases if fixture state changes between execution and undo.
- Stale scan state remains potentially confusing.

Assumptions:

- Current-execution undo is the safest visible step before manifest discovery or real-profile undo.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added ADR 0010 for WPF current fixture undo.
- Added `Undo fixture quarantine` to the WPF execution gate area.
- Wired WPF undo to `UndoQuarantineExecutor.Undo` for the current fixture Restore Manifest.
- Added WPF result/stale-state wording for undo.
- Disabled repeat undo after an undo attempt.
- Extended WPF smoke coverage to execute and undo a fixture Quarantine action.

Files changed:

- `docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`
- `docs/features/2026-05-29-wpf-current-fixture-undo-quarantine.md`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

Docs updated:

- ADR 0010, README, domain context, glossary, MVP readiness audit, this feature brief, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`.

Follow-up work:

- Verify CI after push.
- Manually test fixture execute then `Undo fixture quarantine` in the launched WPF app.
- Add WPF manifest discovery/history for old fixture and eventual real-profile manifests.
- Decide whether successful undo should offer empty action-folder cleanup.

Open questions:

- What UI should discover and select old Restore Manifests?
- Should successful undo offer to clean up empty action folders?
- What additional confirmation should real-profile undo require?

Risky assumptions:

- Current-execution undo is the safest visible step before manifest discovery or real-profile undo.
