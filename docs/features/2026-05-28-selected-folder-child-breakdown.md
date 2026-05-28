# Feature: Selected folder child breakdown

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Improve Storage Scan inspection by showing the largest immediate children for the currently selected folder.

This helps answer the user's original need to see what is inside large folders before deciding whether a path might be unwanted bloat.

## Non-goals

- Do not add deletion.
- Do not add Quarantine.
- Do not automatically expand every folder in the main table.
- Do not treat a folder as safe because its children look cache-like.

## User story / job story

As the project owner, I want selecting a folder like `AppData`, `Local`, or `pip` to show the largest things inside it, so that I can understand the storage usage before making cleanup decisions.

## Current behavior

The Storage Scan table shows large rows and filters, but selecting a folder only shows path, evidence, and recommendation details.

## Desired behavior

- Detail pane shows evidence for the selected path.
- Detail pane also shows the largest immediate children for selected folders.
- Child breakdown includes name, size, importance, recommendation, and categories.
- Files explicitly show that they do not have children.
- Behavior remains read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Child Breakdown | Added as a read-only selected-folder summary. | yes |

## Decisions made

- Show immediate children rather than recursive descendants in the detail pane.
- Sort child breakdown by size descending.
- Cap child breakdown entries for readability.
- Keep the main result table flat for now; a tree view remains possible later.

## Implementation plan

1. Add core child summary entries and builder.
2. Update WPF detail pane with Evidence and Largest immediate children sections.
3. Add fixture coverage for child summary ordering, cap, and immediate-child behavior.
4. Verify build and tests.

## Test plan

Automated checks:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Manual checks:

- User should rerun Storage Scan and select large folders such as `AppData`, `Local`, `pip`, and `DXCache`.
- Confirm the detail pane makes it easier to understand what is inside selected folders.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `StorageChildSummaryEntry` and `StorageChildSummaryBuilder`.
- Added Largest immediate children section to the WPF detail pane.
- Added fixture coverage proving child summaries are immediate-child based, size-sorted, and capped.

Files changed:

- `src/WindowsFileCleaner.Core/StorageChildSummaryEntry.cs`
- `src/WindowsFileCleaner.Core/StorageChildSummaryBuilder.cs`
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

- No ADR added. This is a reversible review UI improvement.

Follow-up work:

- Consider a grouped category view.
- Consider a proper expandable tree view if the detail-pane breakdown is not enough.
- Add Quarantine only after review confidence improves.

Open questions:

- How many child rows should be shown by default after real use?
- Would a tree view be more useful than the flat result table plus detail breakdown?

Risky assumptions:

- Immediate-child summaries are more useful than showing deepest descendants in the detail pane.
