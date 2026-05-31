# Feature: WPF Selected Restore Revalidation Evidence

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Show Selected Restore Pre-Execution Revalidation evidence in the WPF Selected Restore Execution Gate for exact real-profile Restore Manifests without enabling real-profile restore.

## Non-goals

- Do not enable real-profile selected restore execution.
- Do not call `UndoQuarantineExecutor` for real-profile Restore Manifests.
- Do not enable real-profile Quarantine execution.
- Do not show this evidence as restore approval.
- Do not add all-manifest restore, permanent deletion, or cleanup history.
- Do not move, restore, delete, create, or rewrite real-profile files.

## Current behavior

When the selected Restore Manifest has exact `C:\Users\moxhe` Cleanup Scope, the Selected Restore Execution Gate output now includes:

- whether Selected Restore Pre-Execution Revalidation was checked,
- whether it could proceed,
- exact real-profile scope evidence,
- selected real-profile Undo implementation evidence,
- exact `RESTORE` match evidence,
- restorable, blocked, recovery-review, already-restored, not-moved, and size counts,
- the selected manifest path,
- path-specific stale blockers such as missing quarantine paths,
- a boundary line that says the evidence is read-only and must run again immediately before any future real-profile selected restore movement.

The visible app still keeps real-profile selected restore unavailable.

## Decisions made

- Show revalidation evidence only for exact real-profile selected Restore Manifests, so fixture selected restore stays focused on its existing executable path and custom non-fixture restore stays preview-only.
- Build the evidence from the current selected review, confirmation draft, and selected restore gate every time the selected restore confirmation text changes.
- Keep selected real-profile Undo implementation evidence false in WPF for this packet.

## Files changed

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-31-selected-restore-pre-execution-revalidation.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added read-only Selected Restore Pre-Execution Revalidation output to the WPF selected restore gate for exact real-profile selected Restore Manifests.
- Added WPF smoke coverage for clean real-profile revalidation evidence and stale missing-quarantine-path blockers.
- Kept `CanExecuteSelectedRestore` false for real-profile Restore Manifests.

Follow-up work:

- Future explicit execution work must rerun this revalidation immediately before calling `UndoQuarantineExecutor`.

Open questions:

- None for this display packet.

Risky assumptions:

- Showing the evidence only for exact real-profile selected manifests is clearer than showing it for fixture/custom selected restore gates.
