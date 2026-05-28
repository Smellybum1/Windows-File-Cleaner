# Feature: Scan report CSV export

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Add a read-only CSV export for Storage Scan results so the user can inspect, sort, and compare scan data outside the app.

## Non-goals

- Do not export file contents.
- Do not modify scanned files.
- Do not add Quarantine.
- Do not persist automatic scan history.

## User story / job story

As the project owner, I want to export the current Storage Scan review filter to CSV, so that I can inspect candidates in a spreadsheet before deciding what is safe to clean later.

## Current behavior

The app shows scan results, filters, child breakdowns, and selected-path inspection actions, but results cannot be exported.

## Desired behavior

- Export CSV is available after a Storage Scan completes.
- Export uses the active Storage Review Filter.
- Export includes path, name, type, size, importance, recommendation, categories, modified time, evidence, and access issue.
- Export uses user-facing labels such as `Likely safe` and `Quarantine candidate`.
- Export does not modify scanned files.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Scan Report Export | Added for read-only Storage Scan reports. | yes |

## Decisions made

- Export CSV from the active filter, not only the visible 2,000-row UI cap.
- Keep CSV export in the core library for testability.
- Use a Save File dialog so the user controls the report location.
- Keep exports separate from future Restore Manifests or Quarantine metadata.

## Implementation plan

1. Add `StorageScanCsvExporter`.
2. Add an Export CSV button to the Storage Scan toolbar.
3. Export active-filter rows to a user-selected CSV path.
4. Add fixture coverage for CSV escaping and user-facing labels.
5. Verify build and tests.

## Test plan

Automated checks:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Manual checks:

- Run Storage Scan.
- Choose a filter.
- Export CSV.
- Open the CSV and confirm paths, ratings, recommendations, categories, and evidence are readable.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added CSV export for Storage Scan review rows.
- Added Export CSV button in the Storage Scan toolbar.
- Added fixture coverage for CSV header, escaping, labels, categories, and evidence.

Files changed:

- `src/WindowsFileCleaner.Core/StorageScanCsvExporter.cs`
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

- No ADR added. CSV export is a reversible read-only report feature.

Follow-up work:

- Consider JSON export only if CSV is insufficient.
- Consider scan history only after the user confirms useful reports.

Open questions:

- Should exports include all rows, active filter rows, or both as separate options?

Risky assumptions:

- CSV is sufficient for the first report export.
