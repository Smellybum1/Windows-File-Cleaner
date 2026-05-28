# Feature: Review Shortlist

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Let the user mark interesting Storage Scan rows for follow-up review without approving or executing cleanup.

## Non-goals

- Do not quarantine, delete, move, or otherwise modify scanned files.
- Do not persist selections across scans.
- Do not treat shortlisted rows as cleanup approval.
- Do not build the future Quarantine preview in this packet.

## User story / job story

As the project owner, I want to mark paths that look worth revisiting, so that I can review a smaller set before deciding whether any cleanup action is safe.

## Current behavior

The app can filter and inspect scan rows, but the user has to remember or export broad filtered sets when a row looks worth later review.

## Desired behavior

After a Storage Scan, the selected row detail pane lets the user add or remove that row from a Review Shortlist. The table shows which visible rows are shortlisted. The user can clear the shortlist or export only shortlisted rows as CSV. All behavior remains read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Review Shortlist | Added as a temporary read-only selection model for follow-up review. | yes |

## Open questions

Questions that must be answered before implementation:

- None. The behavior is read-only and reversible within the current scan.

Questions that can be deferred:

- Should a future Quarantine preview start from the Review Shortlist?
- Should shortlist persistence exist after a separate review of manifest/storage rules?

## Grill notes

### Scenarios discussed

- The user ran a real Storage Scan and confirmed the app scanned `C:\Users\moxhe`.
- Real scan output is large enough that the user needs a way to hold a smaller review set.

### Edge cases

- Starting a new scan clears the Review Shortlist.
- Adding the same path twice does not duplicate it.
- Exporting the Review Shortlist writes a report only and does not modify scanned files.

### Dependencies between decisions

- This selection layer should exist before any Quarantine preview so future cleanup approval can remain explicit.

## Evidence and validation gate

Evidence gathered:

- User answers: the app should help inspect files and rate whether they should be deleted, but not break current apps.
- Existing code/docs inspected: Storage Scan, filters, selected-path inspection, CSV export, and safety docs.
- Tests/checks planned: fixture coverage for shortlist uniqueness and read-only projection, build, test harness, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add a delete/quarantine button beside shortlist controls.
- Do not persist the shortlist until restore-manifest and cleanup-approval rules exist.

## Decisions made

Small feature-level decisions:

- Keep the Review Shortlist in memory for the current scan only.
- Export the shortlist through the existing CSV exporter.
- Mark shortlisted rows with a simple table column.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add a core `StorageReviewShortlist` selection model.
2. Add WPF controls to add, remove, clear, and export the shortlist.
3. Add table visibility for shortlisted rows.
4. Add fixture coverage and update docs.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/StorageReviewShortlist.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `src/WindowsFileCleaner.App/StorageEntryRow.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Possible:

- `docs/features/2026-05-28-scan-report-csv-export.md`

## Test plan

Manual checks:

- Run Storage Scan, add and remove selected rows, clear shortlist, and export shortlist CSV.

Automated tests:

- Verify shortlist uniqueness, removal, clearing, projection order, and that review entries are not modified.

## Risks and assumptions

Risks:

- A user could misread "shortlist" as approval unless future cleanup preview keeps a separate confirmation step.

Assumptions:

- In-memory selection is enough for the current review phase.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added an in-memory Review Shortlist.
- Added WPF controls for add, remove, clear, and shortlist CSV export.
- Added a Shortlist column to visible scan rows.

Files changed:

- `src/WindowsFileCleaner.Core/StorageReviewShortlist.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `src/WindowsFileCleaner.App/StorageEntryRow.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-shortlist.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- Added Review Shortlist to domain context and glossary.
- Added this feature brief.

ADRs added or skipped:

- Skipped. This is a reversible read-only review feature.

Follow-up work:

- Use the Review Shortlist as an input candidate for a future Quarantine preview, after explicit approval semantics are designed.

Open questions:

- Should shortlist persistence be added later, or should it remain per-scan only?

Risky assumptions:

- The user prefers a lightweight per-scan marker before a heavier cleanup-planning workflow.
