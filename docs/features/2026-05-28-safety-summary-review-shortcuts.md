# Feature: Safety Summary Review Shortcuts

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Make Storage Scan Safety Summary counts actionable through read-only review shortcuts.

## Non-goals

- Do not add cleanup execution.
- Do not approve quarantine or deletion.
- Do not change permissions or retry access failures.
- Do not replace the existing filter/category controls.

## User story / job story

As the project owner, I want to jump from a safety warning count to the matching rows, so that I can inspect high-risk, protected, inaccessible, reparse-point, quarantine-candidate, and uncategorized paths faster.

## Current behavior

The Safety Summary displays warning counts, but the user must manually choose the matching review filter or category filter.

## Desired behavior

After a Storage Scan, shortcut buttons appear under the Safety Summary. Each enabled shortcut applies the matching read-only filter/category lens. Disabled shortcuts indicate that no rows are present for that bucket.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Scan Safety Summary | Clarified that it may expose read-only review shortcuts. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This packet only applies existing read-only filters.

Questions that can be deferred:

- Should shortcut clicks scroll to the first matching row or preserve selection if possible?
- Should shortcuts be visually grouped with warning severity later?

## Grill notes

### Scenarios discussed

- The first real scan showed access issues and high-risk/protected areas.
- Safety Summary now exposes the counts that need inspection.
- Shortcuts should reduce friction without implying cleanup approval.

### Edge cases

- Shortcuts should be disabled before a scan and for zero-count buckets.
- Protected Location and Reparse point shortcuts should use Bloat Category Filters.
- No category should use the No category lens, not a fake `Unknown` category.
- Access issues should remain informational and should not request elevation.

### Dependencies between decisions

- Depends on Storage Scan Safety Summary, Storage Review Filter, and Bloat Category Filter.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: Safety Summary, review filters, category filters, WPF filter controls.
- Tests/checks planned: core shortcut mapping tests, build, test harness, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not create cleanup-specific shortcut actions.
- Do not treat shortcuts as approvals.
- Do not add new category synonyms for uncategorized rows.

## Decisions made

Small feature-level decisions:

- Add core shortcut-to-filter mapping so behavior is testable outside WPF.
- Keep shortcut buttons in the existing Safety Summary area.
- Use existing review/category filters rather than new result projections.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add core shortcut enum and filter mapping.
2. Add WPF safety shortcut buttons and handlers.
3. Add tests for shortcut mappings.
4. Update docs and run checks.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/StorageScanSafetyShortcut.cs`
- `src/WindowsFileCleaner.Core/StorageScanSafetyShortcutFilter.cs`
- `src/WindowsFileCleaner.Core/StorageScanSafetyShortcutFilterBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-safety-summary-review-shortcuts.md`
- `.codex/progress.md`

Possible:

- `docs/features/2026-05-28-storage-scan-safety-summary.md`

## Test plan

Manual checks:

- Run Storage Scan and use each enabled safety shortcut to verify the result table changes to the matching rows.

Automated tests:

- Verify each shortcut maps to the expected Storage Review Filter and Bloat Category Filter.

## Risks and assumptions

Risks:

- Shortcuts could be mistaken for cleanup actions unless labels and status text stay review-oriented.

Assumptions:

- Shortcut buttons are acceptable in the existing summary area for the MVP.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added core Storage Scan Safety shortcut-to-filter mapping.
- Added WPF shortcut buttons for High risk, Protected, Access issues, Reparse points, Quarantine candidates, and No category.
- Shortcuts apply existing read-only review/category filters and update the results table.
- Shortcut buttons are disabled before scans, during scans, and for zero-count buckets.

Files changed:

- `src/WindowsFileCleaner.Core/StorageScanSafetyShortcut.cs`
- `src/WindowsFileCleaner.Core/StorageScanSafetyShortcutFilter.cs`
- `src/WindowsFileCleaner.Core/StorageScanSafetyShortcutFilterBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-safety-summary-review-shortcuts.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- Clarified Safety Summary can expose read-only review shortcuts.
- Added this feature brief.
- Updated the progress log.

ADRs added or skipped:

- Skipped. This is a reversible read-only review UI feature.

Follow-up work:

- Retest shortcut buttons on a real scan.
- Consider stronger visual grouping if the toolbar feels crowded.

Open questions:

- Should shortcut clicks scroll to the first matching row or preserve selection if possible?
- Should shortcuts be visually grouped with warning severity later?

Risky assumptions:

- Shortcut buttons are acceptable in the existing summary area for the MVP.
