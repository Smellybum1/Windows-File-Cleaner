# Progress Log

This file is the running evidence log for Codex work in this repository.

Use it to preserve what was completed, what was verified, what was rejected, and what should happen next. Keep entries compact and factual.

## Current status

Storage Scan MVP packet implemented and tested by the user against `C:\Users\moxhe`. Cleanup Scope Safety Note, review filters, Storage Review Search, selected-folder child breakdown, selected-path inspection actions, Selected Path Review Guidance, CSV export, Review Mix, Storage Scan Safety Summary, Safety Summary review shortcuts, Access issues filtering, Bloat Category Filter, No category filtering, Review Shortlist, Quarantine Preview, Quarantine Preview CSV export, Restore Manifest Draft, Quarantine Confirmation Draft, Quarantine Readiness UI, conservative app data classification, read-only safety regression checks, the MVP runbook, the MVP readiness audit, fixture-driven WPF launch support, WPF shell smoke testing, WPF fixture scan smoke testing, WPF review interaction smoke testing, WPF review toolbar layout polish, the MVP preflight script, and the MVP fixture review launcher are implemented and verified. Quarantine remains preview-only; no cleanup execution, manifest writing, or Undo Quarantine execution exists.

## Next recommended work

1. Run `.\tools\Start-MvpFixtureReview.ps1`, confirm the launched app shows Fixture Cleanup Scope, click `Scan`, and manually inspect layout, visible wording, Storage Review Search, Selected Path Review Guidance, export dialogs, Safety Summary shortcuts, Review Shortlist, Quarantine Preview, Review Mix, Access issues filter, category filter, No category filter, and filter wording.
2. Use `README.md` and `docs/features/2026-05-28-mvp-readiness-audit.md` to rerun the WPF app against `C:\Users\moxhe`.
3. Run `.\tools\Invoke-MvpPreflight.ps1` before any later real-profile scan if the worktree changes.
4. Rerun the real scan and check whether Windows app data, installed applications, and game data labels make the large app/game rows easier to triage.
5. Retest the Quarantine Readiness UI with a real scan and confirm the draft/readiness wording is understandable.
6. Defer actual Quarantine and Undo Quarantine execution until scan review, preview semantics, confirmation semantics, and restore rules are trustworthy.
7. Revisit .NET 10 before packaging or long-term distribution.

## Completed packets

### 2026-05-28: Create Grill with Docs scaffold

Status: completed

What changed:

- Added the Grill with Docs documentation scaffold.
- Added a SkillOpt-inspired workflow note for evidence-driven, bounded documentation improvement.
- Added this progress log to preserve task evidence and rejected ideas across sessions.

Verification:

- Verified scaffold files and folders with `rg --files` plus a forced recursive listing for hidden `.codex/`.
- `git status --short` could not run because this folder is not currently a Git repository.

Docs updated:

- `AGENTS.md`
- `README-codex-grill-with-docs.md`
- `MANIFEST.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/domain/context-map.md`
- `docs/decisions/`
- `docs/features/`
- `docs/codex/`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0001-use-grill-with-docs-workflow.md`.

Open questions:

- What is this product/app?
- Who is the target user?
- What problem does it solve?
- What stack or framework will it use?
- What is the first feature or workflow?

Rejected ideas buffer:

- Do not write production code before the project summary and initial domain language are captured.

## Verification history

| Date | Check | Result | Notes |
|---|---|---|---|
| 2026-05-28 | Scaffold creation | passed | Verified visible scaffold files with `rg --files`; verified hidden `.codex/progress.md` with `Get-ChildItem -Force -Recurse`. |
| 2026-05-28 | Git status | not available | Folder is not currently a Git repository. |

### 2026-05-28: Capture initial project summary

Status: completed

Evidence:

- Product/app: local Windows cleanup app to trim unwanted bloat from `C:\Users`.
- Target user: project owner only.
- Problem: `C:` is a 250 GB Windows 11 operating-system partition, and `C:\Users` is using about 75 GB.
- Stack/framework: unknown.
- First feature/workflow: unknown.

Docs updated:

- `AGENTS.md`: added project-specific safety rules for read-only scanning, explicit confirmation, reversible cleanup, and conservative path handling.
- `docs/domain/context.md`: replaced template product summary and added initial domain concepts, business rules, lifecycle states, permissions assumptions, and deletion policy.
- `docs/domain/glossary.md`: replaced example terms with initial cleanup domain terms and forbidden synonyms.

ADRs:

- No new ADR added. The current decision is domain framing, not a durable architecture or persistence choice.

Open questions:

- What should count as unwanted bloat?
- Which paths are in scope and out of scope?
- What cleanup action should be available first?
- What stack should the app use?
- What is the first feature brief?

Rejected ideas buffer:

- Do not equate "large" with removable.
- Do not start with permanent deletion as the default first cleanup action.
- Do not treat all of `AppData` as safe bloat.

## Known risks

- Project commands are placeholders until the stack is known.
- First workflow is still unknown.
- Cleanup behavior has destructive potential and needs explicit safety decisions before implementation.

### 2026-05-28: Capture first Grill with Docs answers

Status: completed

Evidence:

- First workflow: read-only scan/report.
- Protected and sensitive folders should still be shown for inspection, with the app helping rate importance and whether deletion is advisable.
- Initial bloat categories: old downloads, temp folders, installer caches, app caches, duplicate files, old game files, Node/Python package caches, and Windows app leftovers.
- Safety constraint: cleanup should not break current apps, including Codex.
- Preferred eventual cleanup path: Quarantine on `D:` with an easy undo workflow.
- Preferred product shape: desktop app.

Docs updated:

- `AGENTS.md`: added first workflow, desktop preference, quarantine preference, and current-app safety rule.
- `docs/domain/context.md`: added Bloat Category, Importance Rating, Deletion Recommendation, Quarantine, Undo Quarantine, and rules for inspection and preserving current apps.
- `docs/domain/glossary.md`: added the new terms and clarified forbidden generic labels.
- `docs/features/2026-05-28-read-only-user-profile-scan.md`: created first draft feature brief.

ADRs:

- No new ADR yet. Stack and Quarantine architecture decisions are still open.

Open questions:

- What desktop stack should be used?
- Should the first scan target all of `C:\Users` or only the current user's profile folder?
- How deep should the first scan inspect folders by default?
- What Importance Rating labels should the UI use?
- What should the first workflow be called?

Rejected ideas buffer:

- Do not hide Protected Locations entirely; show them with conservative warnings.
- Do not build cleanup execution before the read-only scan/review workflow.
- Do not use permanent deletion as the first cleanup mechanism.

### 2026-05-28: Capture Storage Scan implementation choices

Status: completed

Evidence:

- Initial Cleanup Scope: `C:\Users\moxhe`.
- Scan mode: recursive scan of everything accessible within the Cleanup Scope.
- Importance Rating labels: `Likely safe`, `Caution`, `High risk`.
- First workflow name: Storage Scan.
- Desktop stack choice delegated to Codex.
- Local environment has .NET SDK 8.0.421 and 9.0.314, plus Windows Desktop runtimes for .NET 8 and .NET 9.
- Official .NET support policy shows .NET 10 is active LTS through November 14, 2028; .NET 8 and .NET 9 are supported through November 10, 2026.
- Microsoft Learn documents WPF as a .NET Windows desktop UI framework.

Decision:

- Use C# WPF for the desktop app.
- Initial target framework recommendation: .NET 8 for immediate local buildability.
- Revisit .NET 10 after installing its SDK.

Docs updated:

- `AGENTS.md`: replaced placeholder project commands with .NET/WPF commands.
- `docs/domain/context.md`: updated initial Cleanup Scope, recursive Storage Scan workflow, WPF product shape, and Importance Rating labels.
- `docs/domain/glossary.md`: added Storage Scan and WPF terms; updated Cleanup Scope and Importance Rating.
- `docs/features/2026-05-28-read-only-user-profile-scan.md`: updated validation gate and implementation plan.
- `docs/decisions/0002-use-dotnet-wpf-desktop-stack.md`: added stack ADR.

ADRs:

- Added `docs/decisions/0002-use-dotnet-wpf-desktop-stack.md`.

Rejected ideas buffer:

- Do not choose Electron/Tauri unless WPF proves inadequate.
- Do not implement a command-line-only product as the primary UX because the user prefers a desktop app.
- Do not scan all of `C:\Users` in the first version; start with `C:\Users\moxhe`.
- Do not install .NET 10 before the first implementation; use installed .NET 8 for now.

### 2026-05-28: Implement Storage Scan MVP packet

Status: completed

Evidence:

- Created `WindowsFileCleaner.sln`.
- Added `src/WindowsFileCleaner.Core` for read-only scanning and recommendation rules.
- Added `src/WindowsFileCleaner.App` as a WPF desktop app.
- Added `tests/WindowsFileCleaner.Tests` as a dependency-free console test harness.
- Initialized local Git repository and added origin `https://github.com/Smellybum1/Windows-File-Cleaner.git`.
- Remote currently has no refs from `git ls-remote`.

Implementation:

- Storage Scan recursively scans a Cleanup Scope without modifying files.
- Initial UI defaults to `C:\Users\moxhe`.
- Scanner refuses paths outside Cleanup Scope.
- Reparse points are not followed.
- Inaccessible paths are recorded instead of crashing the scan.
- Classifier assigns Bloat Categories, Importance Ratings, Deletion Recommendations, and evidence.
- WPF UI displays totals, top 2,000 largest paths, ratings, recommendations, categories, and evidence details.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- Committed checkpoint: `de7b854 Build initial Storage Scan MVP`.
- Pushed `main` to `https://github.com/Smellybum1/Windows-File-Cleaner.git`.

Docs updated:

- `AGENTS.md`
- `docs/features/2026-05-28-read-only-user-profile-scan.md`
- `.codex/progress.md`

ADRs:

- No new ADR. The implementation follows ADR 0002.

Open questions:

- User needs to run the desktop app and confirm the first real Storage Scan output.
- Quarantine path and undo workflow remain deferred.

Rejected ideas buffer:

- Do not run the real `C:\Users\moxhe` scan automatically from the background.
- Do not add deletion or quarantine buttons before the user reviews real scan output.

### 2026-05-28: Add Storage Scan review filters from real scan feedback

Status: completed

Evidence:

- User ran the WPF app against `C:\Users\moxhe`.
- Real scan completed successfully.
- Reported totals from screenshot:
  - Total size: 58.02 GB.
  - Folders: 37,740.
  - Files: 188,580.
  - Access issues: 3.
- The scan surfaced UX/classification issues:
  - Results table was too broad without filters.
  - Large container folders such as `AppData`, `Local`, `Roaming`, `Google`, `Chrome`, and `pip` showed too many `None` categories.
  - Cache subfolders such as `NVIDIA\DXCache` and Python cache paths need clearer but still conservative labeling.

Implementation:

- Added `StorageReviewFilter`, `StorageReviewEntry`, `StorageReviewSummary`, `StorageScanReview`, and `StorageScanReviewBuilder`.
- Added WPF filter buttons for All, Likely safe, Caution, High risk, and Quarantine candidates.
- Added filter counts and displayed-size summary.
- Lightened DataGrid row presentation.
- Added conservative categories for Profile container, AppData area, Browser data, and GPU shader cache.
- Improved Python `pip` cache recognition.
- Preserved read-only behavior; no cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-review-filters.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental review/filter improvement.

Open questions:

- User should rerun the app and confirm whether filters help.
- Need user feedback on which real rows should be explicit Protected Locations.

Rejected ideas buffer:

- Do not make AppData-derived cache rows "Likely safe" just because they are caches.
- Do not add cleanup buttons based only on the first successful scan.

### 2026-05-28: Add selected-folder child breakdown

Status: completed

Evidence:

- Real scan screenshot showed large container rows such as `moxhe`, `AppData`, `Local`, `pip`, and browser folders.
- The user originally asked for the app to show what is inside folders before cleanup decisions.

Implementation:

- Added `StorageChildSummaryEntry`.
- Added `StorageChildSummaryBuilder`.
- Updated the WPF detail pane with Evidence and Largest immediate children sections.
- Child breakdown shows immediate children with name, size, importance, recommendation, and categories.
- Files explicitly show they have no immediate children.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-folder-child-breakdown.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible review UI improvement.

Open questions:

- User should rerun the app and confirm whether the child breakdown makes large folders understandable.

Rejected ideas buffer:

- Do not replace the main flat table with a tree view until the detail-pane approach is tested.

### 2026-05-28: Add selected-path inspection actions

Status: completed

Evidence:

- Storage Scan now shows enough data that the user needs to inspect selected paths manually.
- Copying and opening selected paths supports review without cleanup execution.

Implementation:

- Added `PathInspectionPlan`.
- Added `PathInspectionPlanBuilder`.
- Added Copy path and Open in Explorer buttons to the selected-row detail pane.
- Folder paths open directly in Explorer.
- File paths ask Explorer to select the file.
- Status messages state that no files were modified.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-inspection-actions.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a read-only inspection UI improvement.

Open questions:

- User should verify Copy path and Open in Explorer on real scan results.

Rejected ideas buffer:

- Do not add destructive selected-row actions next to read-only inspection actions yet.

### 2026-05-28: Add Scan Report CSV export

Status: completed

Evidence:

- Storage Scan now produces enough review data that exporting filtered results will help manual analysis.
- Exporting a report keeps the app read-only with respect to scanned files.

Implementation:

- Added `StorageScanCsvExporter`.
- Added Export CSV button to the Storage Scan toolbar.
- Export uses the current Storage Review Filter.
- Export includes path, name, type, size, importance, recommendation, categories, modified time, evidence, and access issue.
- Export writes a user-selected CSV report file.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only report feature.

Open questions:

- User should verify CSV export after a real scan.

Rejected ideas buffer:

- Do not export file contents.
- Do not treat CSV export as scan history or restore metadata.

### 2026-05-28: Add Review Mix summary and fix flattened-size semantics

Status: completed

Evidence:

- Filtered recursive scan rows overlap because parent folders include child sizes.
- Adding flattened row sizes would overstate storage and imply false savings.

Implementation:

- Added Review Mix display to WPF.
- Changed `StorageReviewSummary` byte fields from summed totals to largest-row sizes.
- Updated filter summary wording to show largest displayed row.
- Added test coverage for largest quarantine candidate row.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental reporting semantics fix.

Open questions:

- User should confirm whether largest-row triage is useful in the real scan.

Rejected ideas buffer:

- Do not report summed bytes for flattened recursive rows.
- Do not call filtered row totals Storage Savings until selections are non-overlapping.

### 2026-05-28: Add Access issues review filter

Status: completed

Evidence:

- The first real scan reported 3 access issues.
- The app showed the count but did not provide a direct way to filter to those paths.

Implementation:

- Added `AccessIssues` to `StorageReviewFilter`.
- Added access issue count and largest-row summary fields to `StorageReviewSummary`.
- Added Access issues filter behavior in `StorageScanReview`.
- Added Access issues button and Review Mix count in WPF.
- Added fixture-style coverage with a synthetic inaccessible row.
- No elevated scan, permission change, cleanup execution, or retry workflow was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-access-issues-review-filter.md`
- `docs/features/2026-05-28-storage-scan-review-filters.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review filter.

Open questions:

- Should access issues remain informational only, or should a future separate workflow retry scanning as administrator?

Rejected ideas buffer:

- Do not request elevation automatically.
- Do not change permissions to resolve access issues.

### 2026-05-28: Add Bloat Category Filter

Status: completed

Evidence:

- The real scan surfaced many category-relevant rows such as app caches, Python package caches, GPU shader caches, browser data, protected locations, and access issues.
- Risk filters alone do not let the user inspect one category of evidence at a time.

Implementation:

- Added `StorageCategorySummaryEntry`.
- Added category summaries to `StorageScanReview`.
- Added combined filtering for `StorageReviewFilter` plus optional `BloatCategory`.
- Added a WPF Category dropdown below the filter buttons.
- CSV export now uses the current review filter and selected category filter.
- Added fixture coverage for category summaries and combined filtering.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-bloat-category-filter.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review feature.

Open questions:

- Should uncategorized rows get a `None` category filter option?
- Which category labels need stronger Protected Location behavior before any cleanup preview?

Rejected ideas buffer:

- Do not treat category matches as cleanup approval.
- Do not sum category rows as Storage Savings while recursive parent/child rows overlap.

### 2026-05-28: Add No category filter

Status: completed

Evidence:

- The first real scan showed many rows with `None` in the Categories column.
- The Bloat Category Filter packet left uncategorized rows as an open question.

Implementation:

- Added `StorageCategoryFilter` and `StorageCategoryFilterKind`.
- Added core filtering for No category rows.
- Added WPF No category dropdown option when uncategorized rows exist.
- Added fixture coverage for No category filtering and combined review/category filtering.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-no-category-filter.md`
- `docs/features/2026-05-28-bloat-category-filter.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review feature.

Open questions:

- Which uncategorized real-scan folders should become explicit Bloat Categories or Protected Locations?

Rejected ideas buffer:

- Do not turn uncategorized rows into `Unknown` categories just to make them filterable.
- Do not treat No category rows as safe or unsafe by default.

### 2026-05-28: Add Review Shortlist

Status: completed

Evidence:

- The user ran the WPF app and confirmed that Storage Scan completed against `C:\Users\moxhe`.
- Real scan output is large enough that the user needs a smaller follow-up set before any cleanup preview.

Implementation:

- Added `StorageReviewShortlist` as an in-memory, per-scan selection model.
- Added Add to shortlist, Remove, Clear shortlist, and Export shortlist controls to the WPF UI.
- Added a Shortlist column to visible Storage Scan rows.
- Review Shortlist export uses the existing CSV exporter and writes only a report.
- Starting a new Storage Scan clears the Review Shortlist.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-shortlist.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review feature.

Open questions:

- Should Review Shortlist remain per-scan only, or should a later persistence model be added after cleanup approval and manifest rules are designed?

Rejected ideas buffer:

- Do not treat Review Shortlist as Quarantine approval.
- Do not persist shortlisted paths until restore-manifest and cleanup-preview semantics are defined.

### 2026-05-28: Add Quarantine Preview

Status: completed

Evidence:

- The user requested a quarantine folder preferably on `D:` with easy undo.
- Review Shortlist now provides a smaller user-selected set to preview before any cleanup action.
- Safety docs require dry-run or preview behavior before file-moving code.

Implementation:

- Added `QuarantinePreview`, `QuarantinePreviewEntry`, `QuarantinePreviewDisposition`, and `QuarantinePreviewBuilder`.
- Added default preview root `D:\WindowsFileCleanerQuarantine`.
- Added WPF Preview quarantine control and preview summary display.
- Preview output shows included, blocked, and redundant rows, non-overlapping previewed bytes, and destination paths for included rows.
- Preview blocks high-risk, protected, inaccessible, reparse-point, outside-scope, and non-quarantine-candidate rows.
- Preview marks child rows redundant when a selected parent is already included.
- No folder creation, file move, deletion, manifest write, or cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only preview feature. Actual Quarantine execution and Restore Manifest design may need ADR coverage later.

Open questions:

- Should actual Quarantine execution use `D:\WindowsFileCleanerQuarantine` by default, or ask the user to choose a path?
- Should a later preview export a restore-manifest-shaped draft?

Rejected ideas buffer:

- Do not add an Execute, Move, Delete, or Quarantine button in this packet.
- Do not write a Restore Manifest during preview.
- Do not count overlapping parent/child rows as separate Storage Savings.

### 2026-05-28: Add Quarantine Preview CSV export

Status: completed

Evidence:

- Quarantine Preview now produces more detail than the bounded UI preview pane can comfortably show.
- The product still needs review/report workflows before any cleanup execution.

Implementation:

- Added `QuarantinePreviewCsvExporter`.
- Added WPF Export preview control enabled only after a current Quarantine Preview exists.
- Exported cleanup scope, quarantine root, disposition, source path, destination path, size, importance, recommendation, categories, reasons, evidence, access issue, and no-files-modified note.
- Changing the Review Shortlist clears the current preview and disables preview export.
- No folder creation, file move, deletion, manifest write, or cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only report feature. Restore Manifest and actual Quarantine execution remain future design work.

Open questions:

- Should a later preview export support JSON for machine-readable restore-manifest drafting?
- Should preview exports include a summary row or remain row-only CSV?

Rejected ideas buffer:

- Do not call the preview export a manifest.
- Do not export an executable cleanup script.
- Do not auto-save the preview without a user-selected report path.

### 2026-05-28: Add Storage Scan Safety Summary

Status: completed

Evidence:

- The first real scan surfaced access issues, high-risk/protected rows, and many rows requiring review.
- Future cleanup work needs the read-only safety boundary to stay visible.

Implementation:

- Added `StorageScanSafetySummary`.
- Added `StorageScanSafetySummaryBuilder`.
- Added WPF Safety Summary text under Review Mix.
- Summary displays Cleanup Scope/read-only notes, high-risk count, Protected Location count, access issue count, reparse point count, Quarantine candidate count, and Uncategorized Result count.
- Starting a new scan clears the previous safety summary.
- No permission change, cleanup execution, quarantine execution, manifest write, or rescan behavior was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only reporting feature.

Open questions:

- Should the summary be exportable as part of a future scan report bundle?
- Should summary notes become clickable filters later?

Rejected ideas buffer:

- Do not use the summary as a Cleanup Action gate yet.
- Do not make the summary a safety score.
- Do not hide access issues or protected rows from review.

### 2026-05-28: Add Safety Summary review shortcuts

Status: completed

Evidence:

- Storage Scan Safety Summary exposes warning counts that should be easy to inspect.
- Existing review and category filters already provide safe read-only lenses.

Implementation:

- Added `StorageScanSafetyShortcut`.
- Added `StorageScanSafetyShortcutFilter`.
- Added `StorageScanSafetyShortcutFilterBuilder`.
- Added WPF shortcut buttons for High risk, Protected, Access issues, Reparse points, Quarantine candidates, and No category.
- Shortcuts apply existing Storage Review Filter and Bloat Category Filter combinations.
- Shortcut buttons are disabled before scans, during scans, and for zero-count buckets.
- No cleanup execution, quarantine execution, manifest write, permission change, or rescan behavior was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-safety-summary-review-shortcuts.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review UI feature.

Open questions:

- Should shortcut clicks scroll to the first matching row or preserve selection if possible?
- Should shortcuts be visually grouped with warning severity later?

Rejected ideas buffer:

- Do not create cleanup-specific shortcut actions.
- Do not treat shortcuts as approvals.
- Do not add new category synonyms for Uncategorized Results.

### 2026-05-28: Add Restore Manifest Draft

Status: completed

Evidence:

- The user wants quarantine on `D:` with easy undo.
- Quarantine Preview now proves eligible destination paths, but future Undo Quarantine needs a versioned metadata contract before file-moving code exists.

Decision:

- Added ADR 0003: use JSON Restore Manifest with schema version `restore-manifest.v1`.

Implementation:

- Added `RestoreManifestDraft`.
- Added `RestoreManifestEntryDraft`.
- Added `RestoreManifestDraftBuilder`.
- Added `RestoreManifestDraftJsonSerializer`.
- Drafts include only included Quarantine Preview rows.
- Drafts exclude blocked and redundant preview rows.
- Draft JSON clearly identifies `isExecutedManifest` as false.
- No folder creation, file move, deletion, manifest file write, quarantine execution, or undo execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/decisions/0003-use-json-restore-manifest.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-restore-manifest-draft.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0003-use-json-restore-manifest.md`.

Open questions:

- What exact manifest file path should future Quarantine execution use?
- Should future executed manifests include hashes for files, and if so which hash algorithm?
- Should future Undo Quarantine restore timestamps and attributes?

Rejected ideas buffer:

- Do not write a draft file automatically.
- Do not use CSV as the executed restore manifest format.
- Do not include blocked or redundant preview rows as draft entries.

### 2026-05-28: Add Quarantine Confirmation Draft

Status: completed

Evidence:

- Quarantine Preview and Restore Manifest Draft now exist as read-only core artifacts.
- Actual Quarantine execution still needs an explicit confirmation gate before any file-moving code.

Implementation:

- Added `QuarantineConfirmationDraft`.
- Added `QuarantineConfirmationDraftBuilder`.
- Confirmation Draft records included counts and bytes, blocked/redundant counts, Restore Manifest Draft id, required future confirmation text, data blockers, and review notes.
- Builder checks preview and manifest agreement for Cleanup Scope, Quarantine root, schema version, entry count, bytes, destination paths, missing rows, duplicate rows, and stray manifest rows.
- `IsExecutionImplemented` remains false.
- No folder creation, file move, deletion, manifest file write, quarantine execution, or undo execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a read-only draft gate; actual execution flow should get ADR review when designed.

Open questions:

- What exact UI should ask for the confirmation phrase?
- Should future execution require hashes before moving files?

Rejected ideas buffer:

- Do not call this a cleanup plan.
- Do not make execution availability depend only on a boolean.
- Do not treat the confirmation phrase as sufficient without matching preview and manifest data.

### 2026-05-28: Add Quarantine Readiness UI

Status: completed

Evidence:

- Restore Manifest Draft and Quarantine Confirmation Draft were implemented in core but not visible in the WPF app.
- The progress log identified WPF display as the next safety-review step before any execution flow.

Implementation:

- WPF `Preview quarantine` now builds a Restore Manifest Draft and Quarantine Confirmation Draft in memory after building the Quarantine Preview.
- The detail pane now shows preview counts, Restore Manifest Draft id and entry summary, Quarantine Confirmation Draft id, required future confirmation text, execution status, readiness blocker count, and blocker details.
- Clearing scan, shortlist, or preview state clears both drafts.
- Quarantine Preview CSV export remains unchanged as a report, not a manifest.
- No folder creation, file move, deletion, manifest file write, quarantine execution, or undo execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/features/2026-05-28-quarantine-readiness-ui.md`
- `docs/features/2026-05-28-restore-manifest-draft.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is display-only UI for existing read-only draft artifacts.

Open questions:

- Should future execution use a modal confirmation dialog or dedicated review screen?
- Should Restore Manifest Draft JSON be exportable before execution?

Rejected ideas buffer:

- Do not add an execute button beside preview controls.
- Do not auto-save Restore Manifest Draft JSON from the UI.
- Do not hide readiness blockers behind a single pass/fail label.

### 2026-05-28: Add conservative app data classification

Status: completed

Evidence:

- Real scan evidence included large app/game rows that needed clearer but conservative labels.
- The app must avoid breaking current apps, game saves, app settings, and installed tools.

Implementation:

- Added `WindowsAppData`, `InstalledApplication`, and `GameData` Bloat Category values.
- Added classifier hints for `AppData\Local\Packages`, `AppData\Local\Programs`, Larian/Baldur's Gate, Stellaris, Paradox, and IronyMod-style paths.
- These categories are treated as Protected Location / High risk / Keep by default.
- Added display labels in WPF and CSV exporters.
- Added fixture coverage for Windows app package data, per-user installed app folders, and known game data.
- No cleanup execution, manifest writing, file move, deletion, or Quarantine behavior was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-conservative-app-data-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental conservative classifier refinement.

Open questions:

- Which specific app or game folders should get cleanup exceptions later?
- Should some Windows app package subfolders be downgraded after manual review?

Rejected ideas buffer:

- Do not mark Windows app package data as likely safe by default.
- Do not classify game folders as removable just because they look old.
- Do not make `AppData\Local\Programs` a cleanup candidate.

### 2026-05-28: Add read-only safety regression

Status: completed

Evidence:

- The MVP boundary is read-only Storage Scan plus user-selected CSV reports.
- Future cleanup execution has not been approved or designed.

Implementation:

- Added `ProductionCodeDoesNotContainCleanupExecutionCalls` to the fixture test harness.
- The guard scans production C# files under `src/`.
- The guard fails on obvious file/directory move, delete, replace, write-bytes, set-attributes, and production directory-creation APIs.
- The guard allows exactly three `File.WriteAllText(dialog.FileName, ...)` calls for user-selected CSV report exports.
- Tests still create/delete fixture directories only inside the test harness.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/features/2026-05-28-read-only-safety-regression.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a verification guard for the existing read-only boundary.

Open questions:

- Should future approved Quarantine execution add a more precise allowlist for its own file-moving implementation?

Rejected ideas buffer:

- Do not block user-selected CSV report writes.
- Do not scan docs or tests for banned APIs; fixtures are allowed to create/delete test files.
- Do not treat this guard as a substitute for execution design if cleanup actions are added later.

### 2026-05-28: Add MVP runbook

Status: completed

Evidence:

- The repo had detailed Grill with Docs files but no app-focused root `README.md`.
- Next work requires a safe manual WPF rerun against `C:\Users\moxhe`.

Implementation:

- Added `README.md` for the current Windows File Cleaner MVP.
- Documented safety status, requirements, verification commands, WPF run command, default Cleanup Scope, manual MVP checklist, current workflow, and not-yet-implemented cleanup workflows.
- Kept the existing `README-codex-grill-with-docs.md` scaffold README.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-runbook.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a documentation runbook for the current MVP.

Open questions:

- Should packaging instructions be added after a release build exists?

Rejected ideas buffer:

- Do not document cleanup execution commands before they exist.
- Do not replace the Grill with Docs scaffold docs.
- Do not claim the app is safe to delete files.

### 2026-05-28: Add MVP readiness audit

Status: completed

Evidence:

- The user provided a screenshot showing Storage Scan completed against `C:\Users\moxhe` with 58.02 GB, 37,740 folders, 188,580 files, 3 access issues, and no file modifications.
- The repo already contained WPF, .NET 8, read-only Storage Scan, fixture tests, conservative classification, preview-only Quarantine artifacts, and safety regression evidence.

Implementation:

- Added `docs/features/2026-05-28-mvp-readiness-audit.md`.
- Mapped the original MVP requirements to repo evidence and status.
- Marked latest WPF UI retest as pending manual verification.
- Marked actual Quarantine execution and Undo Quarantine execution as out of MVP.
- Linked the audit from `README.md`.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an evidence audit, not a new durable architecture or persistence decision.

Open questions:

- Should the next packet focus on manual WPF retest feedback, or start designing actual Quarantine execution only after that retest?

Rejected ideas buffer:

- Do not mark the MVP fully complete until the latest WPF review flow is manually retested.
- Do not treat Quarantine Preview, Restore Manifest Draft, or Quarantine Confirmation Draft as cleanup approval.

### 2026-05-28: Add WPF fixture smoke launch

Status: completed

Evidence:

- The MVP readiness audit identified the latest WPF UI manual retest as the main remaining verification gap.
- Core fixture tests existed, but the WPF app always opened with the default real Cleanup Scope.

Implementation:

- Added `StorageScanLaunchOptions` for parsing `--scope`.
- Updated WPF startup to construct `MainWindow` explicitly with the parsed initial Cleanup Scope.
- Added a `MainWindow` constructor overload that fills the Cleanup Scope box without starting a scan.
- Added parser coverage to the console test harness.
- Added `tools/New-StorageScanSmokeFixture.ps1` to create a synthetic fixture under `.local\storage-scan-smoke-fixture`.
- Added `.local/` to `.gitignore`.
- Updated README, AGENTS, the domain context rule, and the MVP readiness audit.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1` passed and created the ignored fixture Cleanup Scope.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed and showed intended fixture writes.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/domain/context.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-fixture-smoke-launch.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible launch/testability improvement and does not change architecture, persistence, cleanup behavior, or security posture.

Open questions:

- Should the fixture include more real-scan-inspired app/game examples after the next manual pass?

Rejected ideas buffer:

- Do not auto-scan on startup.
- Do not create fixture files from production app code.
- Do not treat fixture WPF smoke testing as a substitute for one final real-profile retest.

### 2026-05-28: Add WPF shell smoke test

Status: completed

Evidence:

- Core fixture tests already covered scan/review/preview logic.
- WPF fixture launch support already covered argument parsing.
- The remaining automated gap was proving the WPF shell consumes the launch Cleanup Scope without starting a scan.

Implementation:

- Added read-only `MainWindow` startup-state properties.
- Added `tests/WindowsFileCleaner.App.Tests` targeting `net8.0-windows` with `UseWPF`.
- Added smoke coverage for default Cleanup Scope, launch Cleanup Scope, idle startup state, enabled scan action, and disabled CSV export before scan.
- Added the app test project to `WindowsFileCleaner.sln`.
- Updated README, AGENTS, the MVP readiness audit, and this progress log.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed after restore.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-shell-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a testability improvement and does not change architecture, persistence, security posture, or cleanup behavior.

Open questions:

- Should future UI automation cover actual button/filter interactions after the manual retest?

Rejected ideas buffer:

- Do not treat WPF shell construction as proof of the full interaction flow.
- Do not add visible GUI automation dependencies before the manual fixture smoke pass identifies a real need.

### 2026-05-28: Add WPF fixture scan smoke test

Status: completed

Evidence:

- Core tests already covered scanner and review logic.
- WPF shell smoke tests covered construction and launch-scope wiring.
- The next evidence gap was proving a fixture scan updates WPF state without touching real profile data.

Implementation:

- Refactored the Scan button path to call `RunStorageScanForCurrentScopeAsync`.
- Added read-only WPF state properties for smoke-test assertions.
- Added `WindowsFileCleaner.App.Tests` coverage that creates a synthetic fixture, runs Storage Scan through `MainWindow`, and asserts visible status, summary, filter, row, rating, recommendation, and category state.
- Verified fixture marker files remain after the scan.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-fixture-scan-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a testability improvement and does not change architecture, persistence, security posture, or cleanup behavior.

Open questions:

- Should future WPF automation cover Review Shortlist and Quarantine Preview interactions?

Rejected ideas buffer:

- Do not treat WPF fixture scan state as proof of visible layout quality.
- Do not automate real-profile scans in the test harness.

### 2026-05-28: Add WPF review interaction smoke test

Status: completed

Evidence:

- README manual MVP checklist asks for filters, Review Shortlist, and Quarantine Preview to be checked before relying on the app against real profile data.
- Core tests already covered review and preview logic.
- WPF fixture scan tests covered scan state but not review-control interactions.

Implementation:

- Added WPF command methods for Storage Review Filters, Bloat Category Filters, Safety Summary review shortcuts, displayed-row selection, Review Shortlist changes, and Quarantine Preview generation.
- Added `WindowsFileCleaner.App.Tests` coverage that runs a synthetic fixture scan through `MainWindow`, applies review filters and safety shortcuts, shortlists a likely-safe candidate, creates a Quarantine Preview, checks Restore Manifest Draft and Quarantine Confirmation Draft text, and verifies fixture files remain.
- Kept export dialogs manual and no cleanup execution was added.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-review-interaction-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is testability and smoke coverage for existing read-only UI behavior, not a durable architecture, persistence, security, deployment, or cleanup-execution decision.

Open questions:

- Should visible GUI automation or screenshot verification be added after the manual fixture pass?

Rejected ideas buffer:

- Do not automate file export dialogs in this packet.
- Do not treat WPF review interaction state as proof of visible layout quality.
- Do not automate real-profile scans in the test harness.

### 2026-05-28: Polish WPF review toolbar layout

Status: completed

Evidence:

- README and MVP audit still identify the visible fixture UI pass as the next manual verification step.
- The review toolbar previously used fixed grid columns and a horizontal action stack that could crowd as labels and counts grow.
- WPF app smoke tests can verify the intended wrapping toolbar structure without launching a visible desktop window.

Implementation:

- Replaced the fixed review toolbar grid with two named `WrapPanel` toolbars.
- Kept Filter Summary as its own wrapping line between filters and action controls.
- Added a small read-only WPF layout property for smoke-test assertions.
- Added `WindowsFileCleaner.App.Tests` coverage that verifies the review controls use wrapping toolbars.
- No scanner, classifier, cleanup, quarantine execution, or manifest-writing behavior was changed.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-review-toolbar-layout-polish.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible UI layout polish and does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- Should the entire app later get a broader visual design pass after the real-profile retest?

Rejected ideas buffer:

- Do not introduce a new UI framework or dependency for this polish.
- Do not treat wrapping layout structure as proof of visual quality.

### 2026-05-28: Add MVP preflight script

Status: completed

Evidence:

- README verification commands existed individually, but the full pre-real-scan sequence was easy to run incompletely.
- The fixture generator already supported `-WhatIf`.
- Progress history repeatedly used the same restore, build, core test, app test, and fixture dry-run sequence.

Implementation:

- Added `tools/Invoke-MvpPreflight.ps1`.
- Preflight runs restore, build, core tests, WPF app tests, the fixture generator in `-WhatIf` mode, and `git diff --check`.
- Preflight prints the next fixture review launcher command.
- Added `-SkipRestore`, `-SkipFixtureWhatIf`, and `-SkipDiffCheck` switches for focused local loops.
- No real profile scan, WPF launch, fixture creation, cleanup execution, quarantine execution, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a local verification wrapper around existing commands and does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- Should the app later expose a built-in diagnostics/preflight screen?

Rejected ideas buffer:

- Do not make preflight launch the WPF app or scan `C:\Users\moxhe`.
- Do not create fixture files by default from preflight.
- Do not hide individual commands from README; keep them visible for troubleshooting.

### 2026-05-28: Add MVP fixture review launcher

Status: completed

Evidence:

- Progress log identified the manual fixture UI pass as the next recommended work.
- Preflight and fixture scripts existed, but the user still had to run multiple commands and copy the fixture launch command manually.
- WPF launch-scope support only pre-fills the Cleanup Scope and does not auto-scan.

Implementation:

- Added `tools/Start-MvpFixtureReview.ps1`.
- The launcher runs MVP preflight by default, creates the synthetic fixture Cleanup Scope inside the repo, and launches the WPF app with that fixture scope.
- The launcher prints that the app will not auto-scan and that the user must click `Scan`.
- Added `-SkipPreflight`, `-SkipLaunch`, and `-WhatIf` support for safe verification and focused local loops.
- No real profile scan, auto-scan, cleanup execution, quarantine execution, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf` passed and showed intended preflight, fixture creation, and WPF launch actions without executing them.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a local workflow launcher around existing preflight, fixture, and WPF launch paths; it does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- Should the app later include an in-app fixture/demo mode for visual QA?

Rejected ideas buffer:

- Do not make the launcher scan automatically.
- Do not point the launcher at `C:\Users\moxhe`.
- Do not make fixture creation happen from production app code.

### 2026-05-28: Add Selected Path Review Guidance

Status: completed

Evidence:

- The user-tested real scan showed very large rows where `Caution` plus passive evidence was not enough to guide review.
- Existing selected-row detail already showed evidence and Child Breakdown, so the smallest improvement was next-step wording for the selected path.

Implementation:

- Added `SelectedPathReviewGuidance` and `SelectedPathReviewGuidanceBuilder`.
- Added a Review guidance section to the WPF selected-row detail pane.
- Guidance covers access issues, reparse points, profile containers, protected/high-risk rows, quarantine candidates, cache/package rows, Uncategorized Results, and generic evidence review.
- Added core guidance coverage and WPF smoke coverage that selected Quarantine candidates show guidance before shortlisting.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-review-guidance.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible selected-row review improvement and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should guidance become user-customizable after actual cleanup execution exists?

Rejected ideas buffer:

- Do not turn selected-row guidance into cleanup approval language.
- Do not hide High risk rows; explain the safest next review step.
- Do not use Selected Path Review Guidance as a cleanup executor.

### 2026-05-28: Add Cleanup Scope Safety Note

Status: completed

Evidence:

- The project requires fixture-based verification before real-profile scans.
- The app previously showed only the Cleanup Scope path field, so fixture-vs-real scope context was documented but not visible in the WPF shell.

Implementation:

- Added `CleanupScopeSafetyNote` and `CleanupScopeSafetyNoteBuilder`.
- Added a WPF note below the Cleanup Scope controls.
- The note distinguishes Fixture Cleanup Scope, Real Profile Cleanup Scope, Custom Cleanup Scope, Choose Cleanup Scope, and Check Cleanup Scope.
- Added core tests for real-profile, fixture, custom, and blank scope notes.
- Added WPF smoke assertions for default real-profile startup and fixture launch startup notes.
- No scan blocking, preflight execution, fixture creation, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-safety-note.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only UI reminder and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future release record the last successful preflight timestamp for local-only display?

Rejected ideas buffer:

- Do not treat the note as proof that preflight ran.
- Do not make the note run shell commands, create fixtures, or block scanning.
- Do not add a modal pre-scan gate before the visible fixture workflow is manually tested.

### 2026-05-28: Add Storage Review Search

Status: completed

Evidence:

- The real scan scale was 188,580 files and 37,740 folders.
- Existing filters helped broad triage, but finding a specific app, tool, cache, or game path still required scrolling or CSV export.

Implementation:

- Added `StorageReviewSearch`.
- Extended `StorageScanReview` filtering to combine Storage Review Filter, Bloat Category Filter, and Storage Review Search.
- Added a WPF Search field and Clear search action.
- Search matches path, name, category, Importance Rating, Deletion Recommendation, evidence, and access issue text.
- Search normalizes whitespace, hyphens, and underscores so spaced search terms can match enum-style labels such as `HighRisk` and `PythonPackageCache`.
- Search resets after each new Storage Scan.
- No filesystem rescan, persistence, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible in-memory review feature and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should search later support explicit field prefixes such as `path:`, `category:`, or `rating:`?

Rejected ideas buffer:

- Do not make search rescan, watch the filesystem, or search outside completed scan results.
- Do not persist search history.
- Do not use search results as cleanup approval.
