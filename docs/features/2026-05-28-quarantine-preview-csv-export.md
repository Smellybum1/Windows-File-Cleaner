# Feature: Quarantine Preview CSV Export

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Let the user export the current Quarantine Preview as a read-only CSV report for review outside the app.

## Non-goals

- Do not create the quarantine folder.
- Do not move, delete, rename, or modify scanned files.
- Do not write a Restore Manifest.
- Do not treat the export as approval to execute cleanup.
- Do not persist the preview as a cleanup job.

## User story / job story

As the project owner, I want to export the previewed quarantine rows, so that I can inspect included, blocked, and redundant paths before any file-moving workflow exists.

## Current behavior

The app can create a Quarantine Preview and show a bounded summary in the detail pane. The full preview cannot be exported directly.

## Desired behavior

After creating a Quarantine Preview, the user can click `Export preview` and save a CSV report. The report includes source paths, destination paths for included rows, disposition, reasons, size, recommendation, categories, evidence, access status, access issue text, cleanup scope, quarantine root, and a clear no-files-modified note.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Preview | Added CSV report exporter as part of the preview workflow. | yes |
| Scan Report Export | Clarified that Quarantine Preview export is a report, not a Restore Manifest. | yes |

## Open questions

Questions that must be answered before implementation:

- None. The export is a user-selected report file only.

Questions that can be deferred:

- Should a later export support JSON for machine-readable restore-manifest drafting?
- Should preview exports include a summary row or remain row-only CSV?

## Grill notes

### Scenarios discussed

- The user wants a reversible quarantine path on `D:`.
- Quarantine Preview now separates included, blocked, and redundant rows.
- Exporting the preview supports review before any cleanup execution.

### Edge cases

- Export should be disabled until a preview exists.
- Changing the Review Shortlist clears the preview and disables preview export.
- Exported rows must include blocked reasons, not just included destination paths.

### Dependencies between decisions

- Depends on Quarantine Preview.
- Must remain separate from Restore Manifest design and cleanup execution.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: Quarantine Preview, Review Shortlist, CSV exporter, safety docs.
- Tests/checks planned: CSV exporter fixture test, build, test harness, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not call the export a manifest.
- Do not export an executable cleanup script.
- Do not auto-save the preview without a user-selected report path.

## Decisions made

Small feature-level decisions:

- Add a dedicated `QuarantinePreviewCsvExporter`.
- Enable `Export preview` only after the current preview exists.
- Keep the export row-oriented rather than summary-plus-row for compatibility with spreadsheet review.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add core Quarantine Preview CSV exporter.
2. Add WPF Export preview button and handler.
3. Add fixture coverage for disposition, destination, reasons, and no-files-modified note.
4. Update docs and run checks.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/QuarantinePreviewCsvExporter.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview-csv-export.md`
- `.codex/progress.md`

Possible:

- `docs/features/2026-05-28-quarantine-preview.md`

## Test plan

Manual checks:

- Create a Quarantine Preview, export it, and confirm the CSV opens with disposition, reasons, and destination path fields.

Automated tests:

- Verify CSV headers.
- Verify included and blocked rows are exported.
- Verify reasons and quoted CSV cells are escaped.
- Verify the report states no files were modified.

## Risks and assumptions

Risks:

- Users may confuse preview export with a restore manifest unless labels stay explicit.

Assumptions:

- CSV is sufficient for manual review before any machine-readable manifest exists.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added a dedicated Quarantine Preview CSV exporter.
- Later access-status packet added an `Access status` column separate from access issue error text.
- Added WPF `Export preview` control enabled only after a current preview exists.
- Exported source path, destination path, disposition, reasons, size, recommendation, categories, evidence, cleanup scope, quarantine root, and no-files-modified note.
- Changing the Review Shortlist clears the preview and disables preview export.

Files changed:

- `src/WindowsFileCleaner.Core/QuarantinePreviewCsvExporter.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview-csv-export.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- Clarified that Quarantine Preview exports are reports, not Restore Manifests.
- Added this feature brief.
- Updated the progress log.

ADRs added or skipped:

- Skipped. This is a reversible read-only report feature; Restore Manifest and actual cleanup execution remain future design work.

Follow-up work:

- Retest Review Shortlist, Quarantine Preview, and Export preview with a real scan.
- Design Restore Manifest and explicit approval semantics before any file-moving code.

Open questions:

- Should a later export support JSON for machine-readable restore-manifest drafting?
- Should preview exports include a summary row or remain row-only CSV?

Risky assumptions:

- CSV is sufficient for manual review before any machine-readable manifest exists.
