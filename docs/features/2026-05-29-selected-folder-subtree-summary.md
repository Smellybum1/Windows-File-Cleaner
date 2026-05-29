# Feature: Selected Folder Subtree Summary

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make selected-folder inspection safer by summarizing the descendant review mix inside a folder before the user shortlists or previews anything.

This helps the user understand whether a large folder is mostly cache-looking rows, protected/high-risk rows, uncategorized rows, or access issue rows.

## Non-goals

- Do not rescan the filesystem.
- Do not calculate confirmed storage savings.
- Do not change Bloat Categories, Importance Ratings, Deletion Recommendations, Review Shortlist, Quarantine Preview, or cleanup eligibility.
- Do not add real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or cleanup history.
- Do not replace Child Breakdown, Storage Hotspot Trail, Selected Folder Child Focus, or Quarantine Preview blockers.

## User story / job story

As the project owner, I want selecting a folder like `AppData`, `NVIDIA`, `pip`, or `Chrome` to summarize the descendant review mix, so that I can see whether it contains protected/high-risk data or likely cleanup candidates before acting on the parent.

## Current behavior

The selected-row detail pane shows Evidence, Review guidance, Child Breakdown, Storage Hotspot Trail, and file preview text. `Show children` can focus the grid on immediate children.

The user still has to manually infer the descendant risk mix from the flat table, filters, and Quarantine Preview blockers.

## Desired behavior

- Add a read-only Selected Folder Subtree Summary for selected folders.
- Exclude the selected folder itself from descendant counts.
- Show descendant file/folder rows, Importance Rating counts, Quarantine candidate count, Protected Location count, Access issue count, Reparse point count, Uncategorized Result count, and largest descendant row.
- Show bounded largest examples for Quarantine candidates, Protected Location rows, Access issue rows, and Uncategorized Results when present.
- Explain that recursive row sizes overlap and are not storage savings or cleanup approval.
- Files explicitly show that they do not have descendant subtree summaries.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Folder Subtree Summary | Added as a read-only selected-folder summary. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should the summary eventually include clickable review shortcuts for each descendant bucket?
- Should the summary appear as a compact table instead of text after manual layout review?

## Grill notes

### Scenarios discussed

- The user's real scan screenshot showed large containers where the important question is not just size, but what kind of descendants are inside.
- Broad parent folders should remain inspection-first when they contain protected, high-risk, inaccessible, or uncategorized rows.

### Edge cases

- Files have no descendants.
- Empty folders have no descendants.
- The selected folder's own row should not be counted, so broad parent category labels do not hide the child mix.
- Recursive folder row sizes overlap, so largest descendant is only a triage clue.

### Dependencies between decisions

- This depends on the completed Storage Scan tree.
- This complements Child Breakdown, Storage Hotspot Trail, Selected Folder Child Focus, Review Mix, Safety Summary, and Quarantine Preview.
- This remains read-only and does not affect cleanup workflows.

## Evidence and validation gate

Evidence gathered:

- User screenshot of a large real-profile scan.
- Existing `StorageEntry.Children`, Review Mix, Safety Summary, Child Breakdown, Storage Hotspot Trail, Selected Folder Child Focus, Quarantine Preview descendant blockers, WPF detail pane, and smoke tests.
- Project rule: inspect before recommending removal.

Tests/checks planned:

- Core coverage for descendant counts, selected-folder exclusion, largest examples, and file behavior.
- WPF smoke coverage that selected folder details show the subtree summary and selected files explain the boundary.
- Build, both test harnesses, and MVP preflight.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not turn descendant summary counts into cleanup approval.
- Do not sum flattened recursive row sizes as savings.
- Do not add clickable bulk shortcuts before manual review proves the wording/layout works.

## Decisions made

Small feature-level decisions:

- Exclude the selected folder itself from descendant counts.
- Include bounded examples for the riskiest/most actionable descendant buckets.
- Keep the first UI as text to match the existing detail pane.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add `StorageSubtreeReviewSummary` and `StorageSubtreeReviewSummaryBuilder`.
2. Add WPF detail-pane subtree summary text.
3. Add core and WPF smoke coverage.
4. Update README/manual checklist and progress after verification.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/StorageSubtreeReviewSummary.cs`
- `src/WindowsFileCleaner.Core/StorageSubtreeReviewSummaryBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Possible:

- `docs/features/2026-05-28-mvp-readiness-audit.md`

## Test plan

Manual checks:

- Run the fixture app, scan, select `AppData`, `Downloads`, and a file, and confirm the detail pane explains descendant mix without implying savings.
- In the next real scan, try the summary on `AppData`, `Local`, `NVIDIA`, `pip`, browser folders, and broad parent cache folders.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

## Risks and assumptions

Risks:

- The detail pane may become text-heavy before a visible manual layout pass.
- Descendant counts may be useful, but still require child-row inspection.

Assumptions:

- A count-and-example summary is useful enough to test before adding heavier tree or bucket navigation.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added `StorageSubtreeReviewSummary` and `StorageSubtreeReviewSummaryBuilder`.
- Added a Descendant review summary section to the WPF selected-row detail pane.
- The summary excludes the selected folder itself, counts descendant rows by file/folder type, Importance Rating, Quarantine candidates, Protected Location rows, Access issues, Reparse points, and Uncategorized Results, and shows bounded examples.
- The WPF text says recursive row sizes overlap and are not storage savings or cleanup approval.
- Files explicitly report that they do not have descendant subtree summaries.
- Added core and WPF smoke coverage.

Files changed:

- `src/WindowsFileCleaner.Core/StorageSubtreeReviewSummary.cs`
- `src/WindowsFileCleaner.Core/StorageSubtreeReviewSummaryBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-selected-folder-subtree-summary.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`
- This feature brief

ADRs added or skipped:

- No ADR added. This is a reversible read-only review aid that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Follow-up work:

- Try the summary during the next fixture and real-profile review pass on `AppData`, `Local`, `NVIDIA`, `pip`, browser folders, and broad parent cache folders.
- Consider clickable review shortcuts or a compact table only after manual layout review.

Open questions:

- Should the summary eventually include clickable review shortcuts for each descendant bucket?
- Should the summary appear as a compact table instead of text after manual layout review?

Risky assumptions:

- A count-and-example summary is useful enough to test before adding heavier tree or bucket navigation.
- The detail pane can tolerate one more text section until the next visible layout pass.
