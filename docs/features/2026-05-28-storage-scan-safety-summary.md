# Feature: Storage Scan Safety Summary

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Add a read-only safety summary that makes the current Storage Scan boundary and review warnings visible after every scan.

## Non-goals

- Do not add cleanup execution.
- Do not change permissions or retry access failures.
- Do not mark any row safe to delete.
- Do not replace row-level inspection, Review Shortlist, or Quarantine Preview.

## User story / job story

As the project owner, I want the app to summarize safety-relevant scan signals, so that I can tell what still needs review before any cleanup action exists.

## Current behavior

The app shows Review Mix, filters, row evidence, Review Shortlist, and Quarantine Preview. It does not have a compact read-only safety summary that ties together access issues, Protected Locations, high-risk rows, reparse points, and preview-only cleanup boundaries.

## Desired behavior

After a Storage Scan, the app shows a Storage Scan Safety Summary with the Cleanup Scope, read-only status, high-risk count, Protected Location count, access issue count, bounded access issue examples, reparse point count, Quarantine candidate count, Uncategorized Result count, and short safety notes.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Scan Safety Summary | Added as a read-only health readout for safety-relevant scan signals. | yes |

## Open questions

Questions that must be answered before implementation:

- None. The feature is read-only and derived from existing scan data.

Questions that can be deferred:

- Should the summary be exportable as part of a future scan report bundle?
- Should summary notes become clickable filters later?

## Grill notes

### Scenarios discussed

- The first real scan had access issues and many high-risk/protected-looking rows.
- Future cleanup work needs explicit review boundaries before file changes.

### Edge cases

- Access issues should be informational only.
- Access issue examples should be bounded and should not trigger permission changes or retries.
- Reparse points should remain skipped and visible.
- Quarantine candidates should be described as preview-only until explicit approval exists.
- Uncategorized rows should not be treated as safe or unsafe.

### Dependencies between decisions

- This summary depends on Storage Scan and Storage Scan Review.
- It supports future Restore Manifest and cleanup approval design by keeping safety state visible.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: Storage Scan Review, Review Mix, category filters, Quarantine Preview, safety rules.
- Tests/checks planned: core summary fixture test, build, test harness, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not use the summary as a Cleanup Action gate yet.
- Do not make the summary a safety score.
- Do not hide access issues or protected rows from review.

## Decisions made

Small feature-level decisions:

- Keep the summary in memory with the current scan.
- Show a compact text summary in the existing Review Mix area.
- Derive notes from existing row categories, ratings, recommendations, and access status.
- Show up to three access issue examples in the summary when incomplete scan coverage exists.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add core Storage Scan Safety Summary records and builder.
2. Add WPF display text after Storage Scan completes.
3. Add fixture coverage for protected, access issue, quarantine candidate, reparse point, and uncategorized counts.
4. Update docs and run checks.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/StorageScanSafetySummary.cs`
- `src/WindowsFileCleaner.Core/StorageScanSafetySummaryBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Run Storage Scan and confirm the safety summary appears under Review Mix.

Automated tests:

- Verify summary counts high-risk, Protected Location, access issue, reparse point, Quarantine candidate, and Uncategorized Result rows.
- Verify notes keep cleanup/read-only boundaries explicit.
- Later access-example packet verified bounded access issue examples include the relative path and scanner error text.

## Risks and assumptions

Risks:

- The summary could be mistaken for a safety guarantee if wording is too confident.

Assumptions:

- A compact text summary is enough for the current MVP stage.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added a core Storage Scan Safety Summary model and builder.
- Added WPF safety summary text below Review Mix.
- Summarized Cleanup Scope, read-only status, high-risk rows, Protected Locations, access issues, reparse points, Quarantine candidates, Uncategorized Results, and safety notes.
- Later access-example packet added up to three relative access issue examples to the summary text.
- Cleared the previous safety summary when a new scan starts.

Files changed:

- `src/WindowsFileCleaner.Core/StorageScanSafetySummary.cs`
- `src/WindowsFileCleaner.Core/StorageScanSafetySummaryBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- Added Storage Scan Safety Summary to domain context and glossary.
- Added this feature brief.
- Updated the progress log.

ADRs added or skipped:

- Skipped. This is a reversible read-only reporting feature.

Follow-up work:

- Retest real scan UI and confirm Safety Summary wording is helpful.
- Consider making summary counts clickable filters later.

Open questions:

- Should the summary be exportable as part of a future scan report bundle?
- Should summary notes become clickable filters later?

Risky assumptions:

- A compact text summary is enough for the current MVP stage.
