# Feature: Quarantine Execution Gate

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add a read-only gate for future Quarantine execution that requires Quarantine Confirmation Draft readiness, exact confirmation text, and implemented execution support before files can ever move.

## Non-goals

- Do not create the quarantine folder.
- Do not move, delete, rename, or modify scanned files.
- Do not write a Restore Manifest.
- Do not implement Undo Quarantine.
- Do not make typed confirmation sufficient by itself.

## User story / job story

As the project owner, I want the app to prove the exact confirmation gate before actual quarantine exists, so that future file-moving code cannot be enabled by accident or by stale preview state.

## Current behavior

Quarantine Preview creates Restore Manifest Draft and Quarantine Confirmation Draft readiness output. The WPF app shows the future required text but has no typed confirmation gate.

## Desired behavior

- The core library can build a Quarantine Execution Gate from the current Quarantine Confirmation Draft and typed confirmation text.
- The WPF app shows a confirmation field and disabled `Execute quarantine` button after Quarantine Preview.
- The gate requires exact `QUARANTINE` text.
- The gate carries forward confirmation-readiness blockers.
- The gate keeps execution closed because Quarantine execution is not implemented.
- No files are modified and no folders or manifests are created.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Execution Gate | Added as the read-only decision before future execution. | yes |
| Quarantine Confirmation Draft | Clarified that the execution gate consumes its blockers and required phrase. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This packet remains read-only.

Questions that can be deferred:

- What exact manifest write order should actual Quarantine execution use?
- Should future execution require a selected manifest path, a generated action id, or both?

## Grill notes

### Scenarios discussed

- The user wants reversible quarantine on `D:`.
- The app already proves preview destinations and manifest draft metadata.
- Actual execution must not be possible until the user sees exact paths and types a confirmation phrase.

### Edge cases

- Missing Quarantine Preview keeps the gate closed.
- Wrong confirmation text keeps the gate closed.
- Matching confirmation text still keeps the gate closed while execution is not implemented.
- Confirmation Draft blockers stay visible in the execution gate.

### Dependencies between decisions

- The gate depends on Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft.
- Actual Quarantine execution depends on a later packet that decides write order, action layout, failure handling, and Undo Quarantine behavior.

## Evidence and validation gate

Evidence gathered:

- User answers: quarantine should preferably be on `D:` and undoable.
- Existing code/docs inspected: Quarantine Preview, Restore Manifest Draft, Quarantine Confirmation Draft, ADR 0003, README manual checklist, progress log.
- Tests/checks planned: core gate coverage, WPF confirmation gate smoke coverage, build, test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add real file-moving code in the same packet as the first visible execution gate.
- Do not let matching confirmation text override data blockers or unimplemented execution support.

## Decisions made

Small feature-level decisions:

- Add `QuarantineExecutionGate` and `QuarantineExecutionGateBuilder`.
- Keep `CanExecute` dependent on no blockers, exact confirmation text, and implemented execution support.
- Show confirmation text in WPF after preview readiness exists.
- Keep `Execute quarantine` disabled in this build.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add core Quarantine Execution Gate model and builder.
2. Add WPF confirmation field, disabled execute button, and gate text.
3. Reset stale confirmation text when preview state changes.
4. Add core and WPF smoke coverage.
5. Update docs and progress.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/QuarantineExecutionGate.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionGateBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Use fixture review, create Quarantine Preview, type `QUARANTINE`, and confirm execution remains disabled.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- Showing a disabled execute button may make the UI feel closer to cleanup than it is.

Assumptions:

- It is safer to prove confirmation semantics before adding file-moving code.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added core Quarantine Execution Gate model and builder.
- Added WPF confirmation field, disabled `Execute quarantine` button, and execution gate readout.
- Reset confirmation text when preview state changes.
- Kept execution unavailable and read-only.

Files changed:

- `src/WindowsFileCleaner.Core/QuarantineExecutionGate.cs`
- `src/WindowsFileCleaner.Core/QuarantineExecutionGateBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-execution-gate.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

Docs updated:

- Domain context, glossary, README manual checklist, feature brief, and progress log.

ADRs added or skipped:

- No ADR. This is a reversible read-only gate and does not decide file-moving layout, manifest write order, persistence beyond existing ADR 0003, or Undo Quarantine behavior.

Follow-up work:

- Decide actual Quarantine execution layout, manifest write order, and failure handling before adding file-moving code.

Open questions:

- What exact manifest write order should actual Quarantine execution use?
- Should future execution require a selected manifest path, a generated action id, or both?

Risky assumptions:

- The disabled execute button is acceptable because it makes the future gate visible without enabling cleanup.
