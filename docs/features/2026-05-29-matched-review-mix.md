# Feature: Matched Review Mix

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the active review lens easier to trust by showing a compact mix summary for the currently matched Storage Scan rows.

This helps after searches such as `under:C:\Users\moxhe\AppData`, category filters, size filters, and selected-folder focus actions, where the whole-scan Review Mix no longer explains the rows currently being reviewed.

## Non-goals

- Do not rescan the filesystem.
- Do not change Bloat Categories, Importance Ratings, Deletion Recommendations, or cleanup eligibility.
- Do not add cleanup execution, real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or cleanup history.
- Do not add new filter controls or bucket shortcut buttons in this packet.
- Do not present matched counts as cleanup approval or storage savings.

## User story / job story

As the project owner, I want the app to summarize the current matched rows by rating and safety flags, so that I can understand a focused subtree or filtered result before shortlisting anything.

## Current behavior

The WPF app has a whole-scan Review Mix and a Filter Summary. Selected folders also show Descendant review summary.

After applying `Show descendants`, `parent:`, category, size, or access filters, the grid changes but there is no compact current-matches mix that explains the active lens across rating, quarantine candidates, protected rows, access issues, and no-category rows.

## Desired behavior

- Add a read-only Matched Review Mix line near Filter Summary.
- The line recomputes whenever the active review lens changes.
- It counts the full matched set, not only the current Storage Review Display Window.
- It includes rows, Likely safe, Caution, High risk, Quarantine candidates, Protected, Access issues, and No category counts.
- It states that the summary is review context, not cleanup approval.
- It stays disabled/placeholder before a scan.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Matched Review Mix | Added as read-only summary of the active matched review rows. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should future bucket shortcuts be added from Matched Review Mix after visible layout review?
- Should Matched Review Mix eventually include largest-row sizes per bucket?

## Grill notes

### Scenarios discussed

- The user's real scan screenshot showed a large flat result set where current review context matters.
- Selected Folder Descendant Focus makes subtrees easier to inspect, but the active grid needs a compact mix summary.

### Edge cases

- Zero matched rows should still show clear zero counts.
- Display-window paging should not change Matched Review Mix counts.
- Matched Review Mix should not clear or mutate Review Shortlist.

### Dependencies between decisions

- This depends on existing in-memory Storage Scan review filtering.
- This complements Review Mix, Filter Summary, Storage Review Display Window, and Selected Folder Descendant Focus.

## Evidence and validation gate

Evidence gathered:

- Existing `UpdateFilterSummary`, `ApplyCurrentReviewFilters`, WPF smoke tests, and selected-folder focus behavior.
- Project safety rules require inspection before cleanup and no cleanup approval from counts.

Tests/checks planned:

- WPF smoke coverage for placeholder text before scan.
- WPF smoke coverage for Matched Review Mix after a scan.
- WPF smoke coverage that `Show descendants` updates the mix for the focused subtree.
- Build, both test harnesses, MVP preflight, and diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add clickable bucket shortcuts in this packet.
- Do not show matched byte sums as savings.
- Do not use Matched Review Mix to auto-shortlist or recommend cleanup.

## Decisions made

Small feature-level decisions:

- Count the full matched set, not only displayed rows.
- Place the readout next to Filter Summary because it describes the active review lens.
- Include explicit no-cleanup-approval wording.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF Matched Review Mix text.
2. Recompute it from the currently matched review rows in `UpdateFilterSummary`.
3. Add WPF smoke assertions for startup, scan, and descendant focus.
4. Update README, MVP audit, feature brief, and progress after verification.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-matched-review-mix.md`
- `.codex/progress.md`

Possible:

- `docs/features/2026-05-28-mvp-readiness-audit.md`

## Test plan

Manual checks:

- Run the fixture app, scan, and confirm Matched Review Mix appears under Filter Summary.
- Use `Show descendants` on `AppData` and confirm Matched Review Mix changes with the focused subtree.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- Another summary line may add visual density near the review toolbar.
- Counts can help triage but still require row inspection.

Assumptions:

- A compact current-match mix is more useful now than another control or button.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added a WPF Matched Review Mix readout under Filter Summary.
- The readout counts the current full matched set by rows, Likely safe, Caution, High risk, Quarantine candidates, Protected, Access issues, and No category.
- The readout recomputes through the existing filter-summary refresh path, so searches, filters, `Show children`, and `Show descendants` update it.
- Added smoke coverage for startup placeholder text, scan-time summary, descendant focus updates, and prefixed search recomputation.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-matched-review-mix.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-matched-review-mix.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is a reversible read-only UI summary that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Follow-up work:

- In the next visible fixture pass, confirm the added summary line does not crowd the review toolbar.
- In the next real scan, use Matched Review Mix after `Show descendants` on `AppData`, `NVIDIA`, `pip`, and browser folders.

Open questions:

- Should future bucket shortcuts be added from Matched Review Mix after visible layout review?
- Should Matched Review Mix eventually include largest-row sizes per bucket?

Risky assumptions:

- A compact current-match mix is more useful now than another control or button.
- The extra line is acceptable in the dense WPF review surface.
