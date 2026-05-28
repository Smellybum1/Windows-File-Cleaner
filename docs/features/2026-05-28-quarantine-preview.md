# Feature: Quarantine Preview

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Show a read-only dry run for the current Review Shortlist before any future Quarantine action exists.

## Non-goals

- Do not create the quarantine folder.
- Do not move, delete, rename, or modify scanned files.
- Do not write a Restore Manifest.
- Do not persist the preview as a cleanup job.
- Do not treat the Review Shortlist or preview as cleanup approval.

## User story / job story

As the project owner, I want to preview which shortlisted paths could safely be quarantined, so that I can review destination paths, warnings, and expected storage impact before approving any file changes.

## Current behavior

The user can scan, filter, inspect paths, export reports, and maintain a Review Shortlist. There is no preview of how shortlisted rows would map to a quarantine destination.

## Desired behavior

The user can click `Preview quarantine` after adding rows to the Review Shortlist. The app shows a read-only summary of included, blocked, and redundant rows, the non-overlapping previewed bytes, and the quarantine root used for destination paths. The preview must clearly state that no files were modified.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Preview | Added as a read-only dry run before Quarantine execution. | yes |
| Quarantine | Clarified current preview default root as `D:\WindowsFileCleanerQuarantine`. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This packet stays read-only and uses the existing Review Shortlist.

Questions that can be deferred:

- Should the actual Quarantine execution root use the typed preview root, require a fresh confirmation-time choice, or become a persisted local setting?
- Should a later Quarantine Preview export a restore-manifest-shaped draft?

## Grill notes

### Scenarios discussed

- The user wants a quarantine folder preferably on `D:` with easy undo.
- The app has already scanned the real profile and now supports Review Shortlist.
- Before any file move, the user should be able to inspect exact source paths, destination paths, and warnings.

### Edge cases

- High-risk or protected rows should be blocked.
- Inaccessible rows should be blocked.
- Reparse-point rows should be blocked.
- Rows outside the Cleanup Scope should be blocked.
- Rows that are not Quarantine candidates should be blocked.
- If a selected child is already covered by a selected parent, the child should be marked redundant rather than double-counted.

### Dependencies between decisions

- Quarantine Preview depends on Review Shortlist.
- Actual Quarantine execution depends on a future explicit confirmation workflow and Restore Manifest design.

## Evidence and validation gate

Evidence gathered:

- User answers: prefers quarantine on `D:` with easy undo; app should avoid breaking current apps.
- Existing code/docs inspected: Review Shortlist, Storage Scan filters, domain context, glossary, safety rules.
- Tests/checks planned: core preview builder fixture tests, build, test harness, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add an Execute, Move, Delete, or Quarantine button in this packet.
- Do not create a Restore Manifest draft that future code might mistake for an executed action.
- Do not count overlapping parent/child rows as separate storage savings.

## Decisions made

Small feature-level decisions:

- Use `D:\WindowsFileCleanerQuarantine` as the default preview root.
- Keep preview state in memory only.
- Build previews from the current Review Shortlist.
- Report non-overlapping included bytes only.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add core Quarantine Preview records and builder.
2. Add fixture tests for included, blocked, redundant, and destination-path behavior.
3. Add WPF `Preview quarantine` control and preview summary display.
4. Update progress docs and run checks.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/QuarantinePreview.cs`
- `src/WindowsFileCleaner.Core/QuarantinePreviewEntry.cs`
- `src/WindowsFileCleaner.Core/QuarantinePreviewDisposition.cs`
- `src/WindowsFileCleaner.Core/QuarantinePreviewBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Possible:

- `src/WindowsFileCleaner.Core/StorageReviewShortlist.cs`

## Test plan

Manual checks:

- Run a scan, shortlist at least one Quarantine candidate, click `Preview quarantine`, and confirm the summary says no files were modified.

Automated tests:

- Verify included entries get destination paths under the quarantine root.
- Verify high-risk rows are blocked.
- Verify non-quarantine-candidate rows are blocked.
- Verify child rows covered by an included parent are redundant and not double-counted.

## Risks and assumptions

Risks:

- The word "quarantine" could make the feature feel executable before it is. UI and status text must say preview and no files modified.

Assumptions:

- `D:\WindowsFileCleanerQuarantine` is an acceptable preview default because the user requested quarantine preferably on `D:`.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added a read-only Quarantine Preview model for shortlisted rows.
- Added preview dispositions for included, blocked, and redundant rows.
- Added WPF `Preview quarantine` control and preview summary text.
- Blocked high-risk, protected, inaccessible, reparse-point, outside-scope, and non-quarantine-candidate rows.
- Avoided double-counting when a selected parent already covers a selected child.

Files changed:

- `src/WindowsFileCleaner.Core/QuarantinePreview.cs`
- `src/WindowsFileCleaner.Core/QuarantinePreviewBuilder.cs`
- `src/WindowsFileCleaner.Core/QuarantinePreviewDisposition.cs`
- `src/WindowsFileCleaner.Core/QuarantinePreviewEntry.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- Added Quarantine Preview to the domain context and glossary.
- Added this feature brief.
- Updated the progress log.

ADRs added or skipped:

- Skipped. This is a reversible read-only preview feature; actual cleanup execution and restore manifest design may need ADR coverage later.

Follow-up work:

- Let the user retest Review Shortlist plus Quarantine Preview against the real scan.
- Design Restore Manifest and explicit approval semantics before any file-moving code.

Open questions:

- Should the actual Quarantine execution root use the typed preview root, require a fresh confirmation-time choice, or become a persisted local setting?
- Should a later preview export a restore-manifest-shaped draft?

Risky assumptions:

- `D:\WindowsFileCleanerQuarantine` is acceptable as the preview default because the user asked for quarantine preferably on `D:`.
- Later packet `2026-05-29-quarantine-root-preview-selection.md` added a typed read-only Quarantine Root Selection for preview destinations.
