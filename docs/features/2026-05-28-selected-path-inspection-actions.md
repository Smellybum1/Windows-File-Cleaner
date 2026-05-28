# Feature: Selected path inspection actions

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Make Storage Scan results easier to inspect manually without adding cleanup execution.

The user can now select a scan result, copy its path, or open it in File Explorer.

## Non-goals

- Do not delete files.
- Do not move files.
- Do not add Quarantine.
- Do not change ratings or recommendations.

## User story / job story

As the project owner, I want to copy or open the selected path from Storage Scan, so that I can inspect suspicious or promising folders directly before deciding whether they are bloat.

## Current behavior

The app shows scan results, filters, evidence, and child breakdowns, but selected paths are not directly actionable for manual inspection.

## Desired behavior

- A selected row enables Copy path and Open in Explorer.
- Copy path copies the selected path to the clipboard.
- Open in Explorer opens selected folders directly and selects selected files.
- The app states that no files were modified.
- Behavior remains read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Path Inspection | Added for read-only manual inspection actions. | yes |

## Decisions made

- Keep inspection actions distinct from Cleanup Actions.
- Build Explorer launch details in core so behavior can be tested without launching Explorer.
- Enable inspection actions only when a scan result is selected.

## Implementation plan

1. Add `PathInspectionPlan` and `PathInspectionPlanBuilder`.
2. Add Copy path and Open in Explorer buttons to the detail pane.
3. Add tests for folder and file Explorer arguments.
4. Verify build and tests.

## Test plan

Automated checks:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Manual checks:

- Select a folder and use Copy path.
- Select a folder and use Open in Explorer.
- Select a file and confirm File Explorer selects it.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added selected-path copy and Explorer inspection actions.
- Added testable Explorer launch plan construction.
- Added fixture tests for folder and file inspection plans.

Files changed:

- `src/WindowsFileCleaner.Core/PathInspectionPlan.cs`
- `src/WindowsFileCleaner.Core/PathInspectionPlanBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- This feature brief

ADRs added or skipped:

- No ADR added. This is a read-only inspection UI improvement.

Follow-up work:

- Use manual inspection feedback to refine Protected Locations and Bloat Categories.
- Add Quarantine only after review and inspection are trustworthy.

Open questions:

- Should selected path inspection also support opening a terminal at the selected folder later?

Risky assumptions:

- Opening File Explorer is acceptable as a read-only inspection action.
