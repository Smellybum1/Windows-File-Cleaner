# Feature: Access issues review filter

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Make inaccessible Storage Scan paths directly reviewable after a scan.

The first real scan showed 3 access issues. The app currently shows the count, but those rows can be hard to find in the flat largest-path table.

## Non-goals

- Do not request elevated permissions.
- Do not retry inaccessible paths automatically.
- Do not modify file permissions.
- Do not add cleanup execution.

## User story / job story

As the project owner, I want to filter directly to scan access issues, so that I can see which paths were not fully read before trusting cleanup recommendations.

## Desired behavior

- Add an Access issues Storage Review Filter.
- Show the Access issues filter count after a scan.
- Keep filtered rows read-only and exportable via the existing CSV export.
- Keep access issue detail in the selected-row evidence area.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Filter | Expanded to include Access issues. | yes |

## Decisions made

- Treat Access issues as a review filter, not a Cleanup Candidate approval.
- Do not elevate, retry, or change permissions in this packet.
- Count rows with `IsAccessible == false` or the `AccessIssue` category as Access issues.

## Implementation plan

1. Add `AccessIssues` to `StorageReviewFilter`.
2. Add access issue count and largest-row summary fields.
3. Wire an Access issues button into the WPF filter toolbar.
4. Update filter filename/export labels.
5. Add fixture-style test coverage with a synthetic inaccessible row.

## Test plan

Automated checks:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Manual checks:

- Run Storage Scan and confirm the Access issues filter shows the same count as the Access issues summary card.
- Select an access issue row and confirm the evidence includes the access error.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added Access issues as a Storage Review Filter.
- Added access issue count and largest-row fields to `StorageReviewSummary`.
- Added an Access issues button to the WPF filter toolbar.
- Added Access issues to Review Mix as a count.
- Added fixture-style coverage for synthetic inaccessible rows.

Files changed:

- `src/WindowsFileCleaner.Core/StorageReviewFilter.cs`
- `src/WindowsFileCleaner.Core/StorageScanReview.cs`
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
- `docs/features/2026-05-28-storage-scan-review-filters.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `.codex/progress.md`
- This feature brief

ADRs added or skipped:

- No ADR added. This is an incremental read-only review filter.

Follow-up work:

- Let the user rerun Storage Scan and confirm the Access issues filter count matches the summary card.
- Decide later whether access issues should remain informational only or support a separate elevated retry workflow.

Open questions:

- Should the app eventually offer a separate "retry as administrator" workflow, or keep access issues as informational only?

Risky assumptions:

- Access issue rows are useful enough as a review filter without adding elevated scan behavior.
