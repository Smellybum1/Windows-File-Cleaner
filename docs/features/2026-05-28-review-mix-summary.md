# Feature: Review mix summary

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Make Storage Scan results easier to triage by showing how many rows are Likely safe, Caution, High risk, and Quarantine candidates.

Also fix the summary semantics so the app does not add flattened recursive row sizes together.

## Non-goals

- Do not calculate confirmed Storage Savings.
- Do not create a cleanup selection model.
- Do not add Quarantine.
- Do not modify scanned files.

## User story / job story

As the project owner, I want a quick read-only summary of the scan mix, so that I can decide which kind of result to inspect first.

## Current behavior

The app shows filter counts and a filtered row-size summary. Summing flattened recursive row sizes is misleading because parent folders and child files overlap.

## Desired behavior

- Show Review Mix after Storage Scan.
- Show count and largest-row size for Likely safe, Caution, High risk, and Quarantine candidates.
- Later packet adds Access issues count to the Review Mix so incomplete scan coverage is visible.
- Filter summary should show displayed row count and largest displayed row, not a summed row size.
- Keep all behavior read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Review Mix | Added for counts and largest rows by rating/recommendation. | yes |

## Decisions made

- Do not sum flattened recursive row sizes.
- Use largest row size as a triage clue.
- Defer true non-overlapping savings totals until the app supports explicit selections.

## Implementation plan

1. Change `StorageReviewSummary` byte fields from totals to largest-row sizes.
2. Update review builder to calculate largest rows.
3. Add Review Mix text to the WPF app.
4. Update filter summary wording.
5. Add fixture coverage for largest quarantine candidate semantics.

## Test plan

Automated checks:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Manual checks:

- Run Storage Scan and confirm Review Mix appears after scan.
- Confirm filter summary says largest row rather than summed size.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added Review Mix display.
- Replaced misleading summary byte totals with largest-row fields.
- Updated filter summary wording to largest row.
- Added test coverage for largest quarantine candidate row.
- Later packet `2026-05-30-review-mix-help-text.md` mirrored Review Mix into tooltip and automation help text with no-rescan/no-file-modified/no-storage-savings-proof/not-cleanup-approval wording.

Files changed:

- `src/WindowsFileCleaner.Core/StorageReviewSummary.cs`
- `src/WindowsFileCleaner.Core/StorageScanReviewBuilder.cs`
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

- No ADR added. This is an incremental reporting semantics fix.

Follow-up work:

- Add non-overlapping selected savings only when the app supports explicit candidate selection.
- Keep Storage Savings wording conservative.

Open questions:

- Should the app eventually support an explicit review cart for non-overlapping candidate savings?

Risky assumptions:

- Largest-row triage is more useful and less misleading than summed flattened rows.
