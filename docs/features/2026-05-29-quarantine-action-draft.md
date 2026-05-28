# Feature: Quarantine Action Draft

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Define and show the action-scoped quarantine layout that future Quarantine execution should use, without creating folders, moving files, or writing manifests.

## Non-goals

- Do not create the quarantine folder.
- Do not move, delete, rename, or modify scanned files.
- Do not write a Restore Manifest.
- Do not implement Undo Quarantine.
- Do not decide manifest write order or failure recovery.

## User story / job story

As the project owner, I want the app to show where a future quarantine action would store items and the restore manifest, so that the layout can be reviewed before file-moving code exists.

## Current behavior

Quarantine Preview shows preview destination paths under the selected root, and Quarantine Execution Gate proves confirmation text. There is no action-scoped destination layout yet.

## Desired behavior

- The core library can build a Quarantine Action Draft only when Quarantine Confirmation Draft has no data blockers.
- The draft maps included Restore Manifest Draft rows to action-scoped item paths.
- The draft exposes an action root, items root, and future restore manifest path.
- The WPF execution gate shows the action draft summary when available.
- No folders are created and no files are modified.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Action Draft | Added as the read-only action-scoped layout before future execution. | yes |
| Quarantine Execution Gate | Clarified that it can show the action draft layout while execution remains unavailable. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This packet remains read-only.

Questions that can be deferred:

- What exact manifest write order should actual Quarantine execution use?
- How should partial move failures update the executed Restore Manifest?

## Grill notes

### Scenarios discussed

- Future quarantine needs a `D:` holding area and easy undo.
- Preview paths should stay visibly separate from executed quarantine storage.
- Each future cleanup action should be independently restorable.

### Edge cases

- Action ids must not contain path separators, drive separators, or punctuation that could escape the action root.
- Confirmation Draft blockers prevent building an action draft.
- Entries outside the Cleanup Scope cannot be mapped into action-scoped item paths.

### Dependencies between decisions

- Depends on ADR 0003 JSON Restore Manifest.
- Adds ADR 0004 action-scoped quarantine layout.
- Future execution still depends on manifest write order and failure handling.

## Evidence and validation gate

Evidence gathered:

- User answers: quarantine should preferably be on `D:` and undoable.
- Existing code/docs inspected: Quarantine Preview, Restore Manifest Draft, Quarantine Confirmation Draft, Quarantine Execution Gate, ADR 0003.
- Tests/checks planned: core action draft path coverage, WPF readout smoke coverage, build, test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not reuse preview paths as executed quarantine paths.
- Do not use a flat quarantine root for all moved items.

## Decisions made

Small feature-level decisions:

- Add `QuarantineActionDraft`, `QuarantineActionEntryDraft`, and `QuarantineActionDraftBuilder`.
- Use `<quarantine-root>\actions\<action-id>\items\<relative-path>` for future moved items.
- Use `<quarantine-root>\actions\<action-id>\restore-manifest.json` for the future executed restore manifest.
- Keep this draft read-only and in memory.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0004-use-action-scoped-quarantine-layout.md`

## Implementation plan

1. Add ADR 0004 for action-scoped quarantine layout.
2. Add core action draft model and builder.
3. Show action draft summary in the WPF Quarantine Execution Gate.
4. Add core and WPF smoke coverage.
5. Update docs and progress.

## Files expected to change

Expected:

- `docs/decisions/0004-use-action-scoped-quarantine-layout.md`
- `src/WindowsFileCleaner.Core/QuarantineActionDraft.cs`
- `src/WindowsFileCleaner.Core/QuarantineActionEntryDraft.cs`
- `src/WindowsFileCleaner.Core/QuarantineActionDraftBuilder.cs`
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

- Use fixture review, create Quarantine Preview, and confirm the Quarantine Execution Gate shows action items root and restore manifest path.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- More path concepts can confuse the UI unless preview and action draft wording stays explicit.

Assumptions:

- Action-scoped folders are the right first layout for reversible local quarantine.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added ADR 0004 for action-scoped quarantine layout.
- Added core Quarantine Action Draft model and builder.
- Added consistency checks across Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft metadata.
- Added WPF readout of action items root and restore manifest path.
- Kept the layout draft in-memory and read-only.

Files changed:

- `docs/decisions/0004-use-action-scoped-quarantine-layout.md`
- `src/WindowsFileCleaner.Core/QuarantineActionDraft.cs`
- `src/WindowsFileCleaner.Core/QuarantineActionEntryDraft.cs`
- `src/WindowsFileCleaner.Core/QuarantineActionDraftBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-action-draft.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

Docs updated:

- ADR 0004, domain context, glossary, README manual checklist, feature brief, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0004-use-action-scoped-quarantine-layout.md`.

Follow-up work:

- Decide manifest write order and failure handling before adding file-moving code.

Open questions:

- What exact manifest write order should actual Quarantine execution use?
- How should partial move failures update the executed Restore Manifest?

Risky assumptions:

- Action-scoped folders are the right first layout for reversible local quarantine.
