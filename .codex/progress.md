# Progress Log

This file is the running evidence log for Codex work in this repository.

Use it to preserve what was completed, what was verified, what was rejected, and what should happen next. Keep entries compact and factual.

## Current status

Storage Scan MVP packet implemented and tested by the user against `C:\Users\moxhe`. Cleanup Scope Selection with folder browsing, Cleanup Scope Safety Note, Cleanup Scope Scan Gate, Cleanup Scope Root classification, review filters, Review View Reset, Storage Review Search with field prefixes, Storage Entry Type Filter, Storage Size Threshold Filter, Storage Review Display Limit wording, Storage Review Display Window navigation, Storage Review Size Note, selected-folder child breakdown, selected-path inspection actions, Selected Path Hierarchy Context including relative-path grid/detail context, Selected Row Contents Context including a grid Contents column, explicit Access Status, Access Status Search, Selected File Content Preview with Credential Data preview blocking, Selected Path Review Guidance including cache-specific guidance and scope-root guidance, CSV export including active search, searched filenames, hierarchy/contents/access/relative-path context and type- and size-filtered rows, Review Mix, Storage Scan Safety Summary with bounded access issue, Quarantine candidate, and No category examples, Safety Summary review shortcuts, Access issues filtering, Bloat Category Filter, Large old file classification, No category filtering, specific rebuildable cache candidate classification, conservative app/game/mod-manager/cloud-sync/credential data classification, Review Shortlist, Shortlist shown, Remove shown, Quarantine Preview with typed/browsable Quarantine Root Selection, Quarantine Root Safety Note, fully qualified preview-root gating, protected-descendant blocking, and relative blocker examples, Quarantine Preview CSV export, Restore Manifest Draft, Quarantine Confirmation Draft, Quarantine Readiness UI, Quarantine Execution Gate, Quarantine Action Draft, read-only safety regression checks, the MVP runbook, the MVP readiness audit, fixture-driven WPF launch support, WPF shell smoke testing, WPF fixture scan smoke testing, WPF display-limit smoke testing, WPF review interaction smoke testing, WPF review toolbar layout polish including a separate shortlist/quarantine toolbar, the MVP preflight script, CI MVP preflight workflow, and the MVP fixture review launcher are implemented and verified. Quarantine remains preview-only; no cleanup execution, manifest writing, or Undo Quarantine execution exists.

## Next recommended work

1. Run `.\tools\Start-MvpFixtureReview.ps1`, confirm the launched app shows Fixture Cleanup Scope, click `Scan`, and manually inspect layout, visible wording, Storage Review Search, Storage Review Display Window controls, the `Relative path` and `Parent` columns, Selected Path Hierarchy Context, Selected File Content Preview, Selected Path Review Guidance, export dialogs, Safety Summary shortcuts, Review Shortlist, Shortlist shown, Remove shown, typed/browsed Quarantine Root Selection, Quarantine Root Safety Note, Quarantine Preview, Quarantine Execution Gate, Quarantine Action Draft, Review Mix, Access issues filter, category filter, No category filter, Size filter, and filter wording.
2. Use `README.md` and `docs/features/2026-05-28-mvp-readiness-audit.md` to rerun the WPF app against `C:\Users\moxhe`; confirm `Scan` is disabled until the real-profile preflight acknowledgement is checked.
3. Run `.\tools\Invoke-MvpPreflight.ps1` before any later real-profile scan if the worktree changes.
4. Rerun the real scan and check whether the cleanup scope root row, `Relative path`, `Parent`, `Contents`, and `Access` columns, Size filter, `access:readable` / `access:access issue` search, Previous rows / Next rows, Safety Summary candidate and no-category examples, selected-row relative/parent/depth/access context, cache-specific Review guidance, specific rebuildable cache candidates such as `DXCache` and `pip\Cache`, conservative game/mod-manager labels such as OptiFine/CurseForge/Vortex, Cloud sync data and Credential data labels, and `Preview file` action make unfamiliar rows easier to triage.
5. Retest the Quarantine Readiness UI with a real scan and confirm typed Quarantine root destinations, broad-parent protected descendant blockers, readable relative examples, and draft/readiness wording are understandable.
6. Defer actual Quarantine and Undo Quarantine execution until scan review, preview semantics, confirmation semantics, restore rules, manifest write order, and failure handling are trustworthy.
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

### 2026-05-28: Add Storage Review Display Limit wording

Status: completed

Evidence:

- The user reran the WPF app against `C:\Users\moxhe` and provided a screenshot showing the scan completed.
- The real scan contained 188,580 files, 37,740 folders, and 3 access issues.
- The WPF grid caps visible rows at 2,000, but prior wording did not clearly distinguish displayed rows from matched review rows.

Implementation:

- Split active matched review rows from WPF displayed rows.
- Updated completed-scan status to say `Showing 2,000 of ... paths` when the display cap is reached.
- Updated Filter Summary to say `2,000 shown of ... matched`, label largest-row triage as the largest matched row, and suggest narrowing with filters/search.
- Added a WPF smoke test with a large synthetic fixture that exceeds the 2,000-row display limit.
- No scanner traversal, classification, export, cleanup execution, Quarantine execution, Undo Quarantine, or manifest-writing behavior was changed.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-display-limit.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF review wording and smoke coverage; it does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future UI add paging or a virtualized tree/grid for all matched rows?

Rejected ideas buffer:

- Do not lower scan depth or skip files to fit the grid.
- Do not call displayed rows the complete scan result.
- Do not add cleanup execution while improving review wording.

### 2026-05-28: Add Shortlist shown action

Status: completed

Evidence:

- The real scan result set is large enough that adding one row at a time to Review Shortlist is cumbersome.
- Storage Review Display Limit wording now makes the visible grid boundary explicit, so bulk shortlisting can safely target only visible rows.

Implementation:

- Added `StorageReviewShortlist.AddMany` for unique bulk additions.
- Added a WPF `Shortlist shown` action that adds only currently displayed rows to Review Shortlist.
- Status text states that Review Shortlist is not cleanup approval and that no files were modified.
- `Shortlist shown` disables when all currently displayed rows are already shortlisted.
- No hidden matched rows, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-shortlist-shown-review-rows.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a later review UI support selecting a specific subset of displayed rows before bulk shortlisting?

Rejected ideas buffer:

- Do not add all matched rows when the grid is capped.
- Do not treat Review Shortlist as cleanup approval.
- Do not add cleanup execution from the shortlist toolbar.

### 2026-05-28: Add Remove shown shortlist action

Status: completed

Evidence:

- `Shortlist shown` makes visible-window bulk review easier, but correcting a broad visible-window shortlist previously required clearing the whole Review Shortlist or removing one selected row at a time.
- Review Shortlist remains an in-memory review aid, so visible-window removal is reversible and does not touch scanned files.

Implementation:

- Added `StorageReviewShortlist.RemoveMany` for unique bulk removals.
- Added a WPF `Remove shown` action that removes only currently displayed rows from Review Shortlist.
- `Remove shown` disables when no currently displayed rows are shortlisted.
- Updated WPF smoke coverage for add shown, remove shown, selected-row add, preview generation, and read-only status text.
- No hidden matched rows, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-shortlist-shown-review-rows.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a later review UI support selecting a specific subset of displayed rows before bulk shortlisting/removal?

Rejected ideas buffer:

- Do not add or remove all matched rows when the grid is capped.
- Do not treat Review Shortlist as cleanup approval.
- Do not add cleanup execution from the shortlist toolbar.

### 2026-05-28: Align Scan Report Export with Storage Review Search

Status: completed

Evidence:

- Storage Review Search narrows the WPF grid and filter summary, but the main Scan Report Export path still used only Storage Review Filter and Bloat Category Filter.
- A searched review should export the same active review lens, while still exporting all matched rows rather than only the 2,000 displayed rows.

Implementation:

- Added a WPF export-row helper that applies Storage Review Filter, Bloat Category Filter, and Storage Review Search.
- Updated Export CSV to use that helper.
- Added WPF smoke coverage that searched export rows honor the active `pip` search.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only report alignment fix and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should export filenames include a sanitized search segment later?

Rejected ideas buffer:

- Do not export only displayed rows; export all rows matched by the active review lens.
- Do not treat exported reports as cleanup history or restore manifests.

### 2026-05-28: Add searched Scan Report Export filenames

Status: completed

Evidence:

- Scan Report Export now honors Storage Review Search, but the suggested filename still did not show when a search was active.
- Report filenames should help the user distinguish searched CSV exports without becoming persisted scan history.

Implementation:

- Added a sanitized `search-...` segment to the main Scan Report Export filename when Storage Review Search is active.
- Search filename segments use lowercase letters/digits separated by hyphens and are capped in length.
- Added WPF smoke coverage for searched and cleared-search export filename behavior.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only report naming behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should exports include all rows, active filter rows, or both as separate options?

Rejected ideas buffer:

- Do not treat report filenames as scan history.
- Do not include raw user paths in generated filenames.

### 2026-05-28: Add hierarchy context to Scan Report Export

Status: completed

Evidence:

- Recursive Storage Scan rows are flattened in the grid and CSV report.
- Full paths are present, but spreadsheet review is easier when each row also carries its immediate parent and depth.

Implementation:

- Added `Parent path` and `Depth` columns to `StorageScanCsvExporter`.
- Kept root-level parent path blank so the export does not invent a parent outside the reviewed hierarchy.
- Extended CSV fixture coverage for the new hierarchy columns.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only report schema improvement and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future export include a separate cleanup-scope-relative path column?

Rejected ideas buffer:

- Do not make Scan Report Export a persisted scan history feature.
- Do not include cleanup approval or restore-manifest data in this CSV.

### 2026-05-28: Add Storage Review Size Note

Status: completed

Evidence:

- The real scan screenshot showed large parent and child folders together in the flattened review grid.
- Review Mix and filter summaries avoid summing rows internally, but the WPF review surface did not state the recursive size rule near the grid.

Implementation:

- Added a visible Storage Review Size Note below the filter summary.
- The note says folder sizes include children, parent/child rows can overlap, and row sizes are triage clues rather than Storage Savings.
- Added WPF smoke coverage for the note text.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-size-note.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible UI wording and smoke coverage.

Open questions:

- Is the note visually readable in the next manual fixture UI pass?

Rejected ideas buffer:

- Do not hide recursive parent or child rows to avoid overlap confusion.
- Do not label row sizes as cleanup savings before Quarantine Preview or a future explicit cleanup plan computes non-overlapping bytes.

### 2026-05-28: Add Storage Review field-prefix search

Status: completed

Evidence:

- The real scan scale makes broad text search useful but sometimes noisy.
- The Storage Review Search feature brief left `path:`, `category:`, and `rating:` prefixes as a deferred question.

Implementation:

- Added `StorageReviewSearchField` and prefix parsing in `StorageReviewSearch`.
- Supported `path:`, `name:`, `category:`/`cat:`, `rating:`/`importance:`, `recommendation:`/`rec:`, `evidence:`, and `issue:`/`access:`.
- Updated `StorageScanReview` to restrict matching to the parsed field when a recognized prefix is used.
- Preserved unprefixed broad search and treated unrecognized prefixes as literal broad search text.
- Added WPF search tooltip examples and smoke coverage for prefixed search summary/export filename behavior.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible in-memory search behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Are the tooltip examples discoverable enough during the next manual fixture UI pass?

Rejected ideas buffer:

- Do not make prefixed search rescan the filesystem or persist search history.
- Do not use prefixed search matches as cleanup approval.

### 2026-05-28: Add Large old file classification

Status: completed

Evidence:

- The real scan screenshot included large files with little category context.
- `No category` remains useful, but old multi-gigabyte files deserve a conservative triage label when last-modified evidence is stale.

Implementation:

- Added `BloatCategory.LargeOldFile`.
- Passed file size into `CleanupCandidateClassifier`.
- Labeled files at least 1 GB and older than 90 days as `Large old file`.
- Kept unknown large old files as `Caution` / `Inspect`; size and age alone do not approve cleanup.
- Kept large old files with stronger cleanup evidence, such as old Downloads or installer evidence, on the existing likely-safe/quarantine-candidate path.
- Added display/export labels and classifier coverage.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-read-only-user-profile-scan.md`
- `docs/features/2026-05-28-large-old-file-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible classifier triage behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does the real scan now surface helpful `Large old file` rows?

Rejected ideas buffer:

- Do not classify directories as Large old file from recursive size because parent and child rows overlap.
- Do not treat large old files as cleanup approval without stronger category evidence.

### 2026-05-28: Add Storage Entry Type Filter

Status: completed

Evidence:

- Recursive scan review mixes folders and files in one flattened grid.
- The user needs to inspect both container folders and individual files, especially after adding Large old file classification.

Implementation:

- Added `StorageEntryTypeFilter` with `All`, `Files`, and `Folders`.
- Added core filtering that combines entry type with review filter, Bloat Category Filter, and Storage Review Search.
- Added a WPF Type filter combo box.
- Included active type in Filter Summary and Scan Report Export filenames.
- Added core and WPF smoke coverage.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `docs/features/2026-05-28-storage-entry-type-filter.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review filtering and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does Files plus Large old file help the next real scan review?

Rejected ideas buffer:

- Do not treat file-only rows as automatically safer than folders.
- Do not hide the underlying completed scan rows; type is only an active review lens.

### 2026-05-28: Add Review View Reset

Status: completed

Evidence:

- Review filters now include rating, category, type, search, and Safety Summary shortcuts.
- Stacked review lenses can make it cumbersome to return to full review without losing Review Shortlist state.

Implementation:

- Added `Reset view` to the WPF review toolbar.
- Added `ResetReviewView` to restore All, All categories, All types, and empty search.
- Kept Review Shortlist intact during reset.
- Added reset-enabled state handling and WPF smoke coverage.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-view-reset.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Is Reset view discoverable enough during manual fixture review?

Rejected ideas buffer:

- Do not combine Reset view with Clear shortlist.
- Do not treat reset as a rescan or cleanup action.

### 2026-05-28: Add Selected Path Hierarchy Context

Status: completed

Evidence:

- Real scan output included deeply nested cache rows with short names such as one-letter folders and hash fragments.
- Those rows are hard to interpret from `Name` alone, even though the selected-row full path exists in the detail pane.

Implementation:

- Added `StorageEntryRow.ParentLocation`.
- Added a `Parent` column to the WPF Storage Scan grid.
- Added selected-row parent path, hierarchy depth, and modified-time context to the detail pane.
- Added WPF smoke coverage for parent/depth context.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-hierarchy-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does the `Parent` column make the next real scan easier to review without overcrowding the table?

Rejected ideas buffer:

- Do not classify short or deeply nested names as safe solely from their hierarchy context.

### 2026-05-28: Add Selected File Content Preview

Status: completed

Evidence:

- The original product request asked the app to show what is in files before rating importance or recommending cleanup.
- The current selected-row detail pane showed metadata, evidence, guidance, and child breakdowns, but could not preview selected file content.

Implementation:

- Added `SelectedFileContentPreview` and `SelectedFileContentPreviewBuilder`.
- Added an explicit WPF `Preview file` action for selected files.
- Added a `File preview` section to the selected-row detail pane.
- Kept preview bounded to a small text sample and avoided rendering binary-looking content as text.
- Added core coverage for text, binary, and folder preview outcomes.
- Added WPF smoke coverage for previewing a selected fixture text file.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-file-content-preview.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only review action and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does selected file preview help with real scan rows without exposing too much sensitive text in the review pane?

Rejected ideas buffer:

- Do not automatically preview file content when row selection changes.
- Do not treat previewed text as cleanup approval.

### 2026-05-28: Tune Cache-Specific Review Guidance

Status: completed

Evidence:

- The real scan showed large cache-heavy rows such as `NVIDIA\DXCache` and Python package cache paths.
- The classifier intentionally keeps these rows conservative, but generic guidance was less useful than category-specific review wording.

Implementation:

- Added GPU shader cache guidance that mentions rebuildability and temporary shader recompile delays.
- Added Python package cache guidance that protects active development tooling and Codex-related paths.
- Added Node package cache guidance that warns about active project dependencies.
- Added app cache guidance that prefers specific child rows over broad app folders.
- Added generic AppData guidance for rows with AppData evidence but no narrower cache category.
- No Bloat Category, Importance Rating, Deletion Recommendation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/features/2026-05-28-cache-specific-review-guidance.md`
- `docs/features/2026-05-28-selected-path-review-guidance.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible wording and triage guidance, not architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does cache-specific guidance make the next real scan easier to act on without making cache rows feel automatically safe?

Rejected ideas buffer:

- Do not promote GPU shader caches or package caches to likely-safe cleanup candidates solely because they are cache-like.

### 2026-05-28: Add Real Profile Scan Gate

Status: completed

Evidence:

- The project requires fixture-based verification before scanning real user files.
- The previous Cleanup Scope Safety Note reminded the user to run preflight and fixture review, but docs explicitly noted that the reminder was not proof and did not block scanning.

Implementation:

- Added `CleanupScopeScanGate` and `CleanupScopeScanGateBuilder`.
- Added a WPF acknowledgement checkbox for real-profile Cleanup Scopes.
- Disabled `Scan` for `C:\Users\moxhe` and child scopes until the acknowledgement is checked.
- Kept fixture Cleanup Scopes scan-ready without the real-profile acknowledgement.
- Enforced the gate in `RunStorageScanForCurrentScopeAsync`, not only through button state.
- Reset acknowledgement when the Cleanup Scope changes.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, preflight execution from WPF, fixture creation from WPF, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-safety-note.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-real-profile-scan-gate.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible local scan-start gate and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future app version display the last successful preflight time if a trustworthy local marker exists?

Rejected ideas buffer:

- Do not run preflight or create fixture files from WPF.
- Do not persist real-profile scan acknowledgement yet.

### 2026-05-28: Add Selected Row Contents Context

Status: completed

Evidence:

- Large recursive scan rows need more context than size alone because parent and child row sizes overlap.
- The app already shows largest immediate children, but selected rows did not show contained file/folder counts.

Implementation:

- Added contained file and descendant folder counts to `StorageEntryRow`.
- Added contents context to the WPF selected-row detail pane.
- Added `Contained files` and `Contained folders` columns to Scan Report CSV export.
- Added WPF fixture coverage for selected-folder contents context.
- Added CSV coverage for exported contents counts.
- No scanner traversal, Bloat Category, Importance Rating, Deletion Recommendation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `docs/features/2026-05-28-selected-row-contents-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review/export context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Do contents counts make the next real scan easier to review alongside largest-child breakdowns?

Rejected ideas buffer:

- Do not treat contained file/folder counts as recoverable storage savings.

### 2026-05-28: Add Cleanup Scope Root Classification

Status: completed

Evidence:

- The first real scan screenshot showed the top `C:\Users\moxhe` row as an ordinary caution/inspect row with no category.
- The scan root should be reviewed through child rows and should never look like a cleanup target.
- Path-shape inference is not enough because fixture and custom Cleanup Scope roots also need root-specific treatment.

Implementation:

- Added `BloatCategory.CleanupScopeRoot`.
- Passed explicit scan-root context from `StorageScanner` into `CleanupCandidateClassifier`.
- Classified the Cleanup Scope Root as `High risk` / `Keep` with `Cleanup scope root` and `Protected location` categories.
- Added Selected Path Review Guidance for scope-root rows.
- Added WPF, Scan Report CSV, and Quarantine Preview CSV labels for `Cleanup scope root`.
- Added core scanner coverage and WPF fixture coverage.
- No child classification, scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-root-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a conservative scan classification and review-guidance change, not a persistence, cleanup execution, security, deployment, or public API decision.

Open questions:

- Does the next real scan make the first row clearly read as the reviewed scope rather than bloat?

Rejected ideas buffer:

- Do not infer Cleanup Scope Root solely from `C:\Users` path shape.
- Do not allow the scope root into cleanup execution or quarantine.

### 2026-05-28: Add Quarantine Preview Protected Descendant Blocker

Status: completed

Evidence:

- Broad cache-like rows can contain protected descendants, including Codex runtime data.
- A broad parent row may otherwise appear to be a Quarantine candidate even though moving it would also move protected child data.
- Quarantine Preview already has the scanned child tree in memory, so it can block broad parent preview without touching the filesystem again.

Implementation:

- Added descendant blocker checks to `QuarantinePreviewBuilder`.
- Blocked parent preview when descendants are protected, high-risk, inaccessible, reparse points, or Cleanup Scope Roots.
- Added blocked reason text with example descendant paths and guidance to select narrower reviewed child rows.
- Added fixture coverage for a synthetic `.cache` parent containing protected `codex-runtimes` data.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or additional filesystem reads were added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a conservative read-only preview rule within existing Quarantine Preview behavior.

Open questions:

- In the next real scan, are broad cache-parent blocker reasons readable enough when paths are long?

Rejected ideas buffer:

- Do not rescan the filesystem from Quarantine Preview to discover blockers.
- Do not allow a broad parent to be included just because the parent row itself is a Quarantine candidate.

### 2026-05-28: Add WPF Proof for Quarantine Preview Protected Descendant Blocker

Status: completed

Evidence:

- Core tests proved the protected-descendant blocker, but the WPF workflow also needs to show the blocked reason and readiness blockers clearly.
- The app should prove the same boundary through selection, Review Shortlist, Preview quarantine, and the preview pane.

Implementation:

- Added a WPF smoke fixture with `.cache` containing protected `codex-runtimes` data.
- Added WPF smoke coverage that selects the broad `.cache` parent, adds it to Review Shortlist, runs Quarantine Preview, and asserts:
  - `0 included`
  - `1 blocked`
  - blocked preview readiness wording
  - `codex-runtimes` descendant evidence
  - narrower-row guidance
  - no-files-modified wording
- No production code, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `docs/features/2026-05-28-wpf-review-interaction-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is WPF smoke coverage for an existing read-only preview rule.

Open questions:

- Should the visible preview pane separate row-specific blockers from confirmation-readiness blockers more clearly?

Rejected ideas buffer:

- Do not weaken confirmation readiness just to make the blocked-row count read as a single blocker.

### 2026-05-28: Improve Quarantine Preview Pane Readability

Status: completed

Evidence:

- The WPF preview pane showed confirmation-readiness blockers and row-level blocked reasons close together.
- A blocked broad parent can have both confirmation blockers and row-specific reasons, so the visible wording should keep those concepts separate.

Implementation:

- Renamed the readiness count line to `Confirmation readiness blockers`.
- Labeled readiness entries as `Confirmation blocker`.
- Added a `Preview rows:` section before row-level included/blocked/redundant entries.
- Labeled each row entry as `Preview row | Included`, `Preview row | Blocked`, or `Preview row | Redundant`.
- Updated WPF smoke assertions for both included-row preview and protected-descendant blocked preview.
- No Quarantine Preview eligibility rules, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-quarantine-preview-pane-readability.md`
- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `docs/features/2026-05-28-wpf-review-interaction-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF wording and smoke coverage.

Open questions:

- Does the real scan need a more structured preview table instead of plain text for long blocked paths?

Rejected ideas buffer:

- Do not change preview eligibility or confirmation readiness semantics as part of wording polish.

### 2026-05-28: Add Access Status Review Field

Status: completed

Evidence:

- The real scan showed access issues, and incomplete scan coverage should stay visible in the main review and exported reports.
- Access issue rows were countable/filterable, but normal review rows did not expose a simple readable/access-issue label.

Implementation:

- Added `StorageEntryRow.AccessStatus` with user-facing values `Readable` and `Access issue`.
- Added an `Access` column to the WPF Storage Scan grid.
- Added `Access: ...` to selected-row metadata.
- Added `Access status` columns to Scan Report CSV and Quarantine Preview CSV exports.
- Added core CSV coverage for readable and access issue statuses.
- Added WPF fixture coverage for readable row access status and selected-row detail metadata.
- No access retry, permission change, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-access-status-review-field.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `docs/features/2026-05-28-quarantine-preview-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI/report context.

Open questions:

- In the next real scan, does the Access column make the three access issue rows easier to find?

Rejected ideas buffer:

- Do not add retry-as-admin or permission-changing behavior as part of access status display.

### 2026-05-28: Add Access Status Search

Status: completed

Evidence:

- The real scan screenshot showed readable rows and 3 access issue rows.
- Access Status is now visible in the grid, selected-row metadata, and CSV exports, so it should also participate in the existing search workflow.

Implementation:

- Added Access Status matching to broad Storage Review Search.
- Added Access Status matching to `access:` and `issue:` field-prefix search.
- Preserved access issue message search for `access:<error text>` and `issue:<error text>`.
- Updated the WPF search tooltip to include `access:readable`, `access:access issue`, and `issue:denied` examples.
- Added core coverage for `access:readable`, `access:access issue`, and access issue message searches.
- Added WPF fixture coverage for `access:readable` search, searched export filename hints, and access-prefix tooltip guidance.
- No access retry, permission change, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-access-status-search.md`
- `docs/features/2026-05-28-access-status-review-field.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible in-memory review behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do `access:readable`, `access:access issue`, and `issue:<error text>` make access review easier alongside the Access issues filter?

Rejected ideas buffer:

- Do not add retry-as-admin or permission-changing behavior as part of access status search.

### 2026-05-28: Add Storage Review Display Window

Status: completed

Evidence:

- The real scan contained 188,580 files and 37,740 folders, while the WPF grid intentionally displays at most 2,000 rows at once.
- The app explained the cap but did not let the user move beyond the first matched row window inside the app.

Implementation:

- Added read-only Previous rows and Next rows controls to the WPF review toolbar.
- Added a Storage Review Display Window label showing active row ranges such as `rows 1-2,000 of N matched`.
- Added `_currentDisplayStartIndex` to page through matched in-memory review rows without rescanning.
- Reset the display window to the first matched rows when filters, type filters, category filters, search, safety shortcuts, or Review View Reset change the active review lens.
- Updated completed-scan status and filter summary wording to use active row-window ranges.
- Corrected Scan Report Export row selection to include the active Storage Entry Type Filter.
- Added WPF fixture coverage for next/previous row windows, display-window reset, read-only status wording, and type-filtered exports.
- No scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-display-window.md`
- `docs/features/2026-05-28-storage-review-display-limit.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF review behavior and does not change architecture, persistence, security, deployment, cleanup execution, or public APIs.

Open questions:

- In the next real scan, are row-window controls enough, or would a virtualized tree/grid still be worth designing later?

Rejected ideas buffer:

- Do not rescan or discard matched rows when changing display windows.
- Do not make `Shortlist shown` apply to rows outside the current display window.

### 2026-05-28: Tighten Display Window Reset for Combo Filters

Status: completed

Evidence:

- Storage Review Display Window should reset when the active review lens changes.
- Programmatic filter helpers already reset the display window, but the actual WPF category/type combo-box event handlers needed the same reset path for manual UI selection.

Implementation:

- Reset `_currentDisplayStartIndex` in `CategoryFilterBox_SelectionChanged`.
- Reset `_currentDisplayStartIndex` in `EntryTypeFilterBox_SelectionChanged`.
- Added WPF test hooks that select type/category options through the real combo boxes.
- Added WPF fixture coverage proving combo-driven type and no-category filter changes reset the display window to the first matched rows.
- No scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `docs/features/2026-05-28-storage-review-display-window.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a small WPF state-consistency fix inside an existing read-only review behavior.

Open questions:

- None.

Rejected ideas buffer:

- Do not rely only on programmatic helper methods when the visible WPF controls use separate event handlers.

### 2026-05-28: Add Contents Column to Storage Review Grid

Status: completed

Evidence:

- The app already computed contained file/folder counts and exported them to CSV, but the main review grid only showed those counts after selecting a row.
- Large real-profile container rows are easier to compare when contents counts are visible before selection.

Implementation:

- Added a WPF grid `Contents` column bound to `StorageEntryRow.Contents`.
- Kept contents counts read-only and derived from the completed Storage Scan result.
- Added WPF fixture coverage proving a folder row exposes file/folder counts in the visible row data.
- No scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-row-contents-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF display of existing read-only scan context.

Open questions:

- In the next real scan, is the grid `Contents` column useful without making the table feel too crowded?

Rejected ideas buffer:

- Do not treat contents counts as storage savings or cleanup approval.

### 2026-05-28: Sort Contents Column by Contained Count

Status: completed

Evidence:

- The WPF grid `Contents` column displays formatted text, which is useful to read but weak as a sort value.
- The user-tested real scan surfaced many large containers, so comparing rows by total contained items is useful review context before selecting a row.

Implementation:

- Added `StorageEntryRow.ContainedTotalCount` as the numeric sort value for contained files plus descendant folders.
- Set the WPF grid `Contents` column `SortMemberPath` to `ContainedTotalCount`.
- Added WPF fixture coverage for the column sort contract and a folder row's numeric contained-item total.
- Kept the change read-only; no scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-row-contents-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF review behavior for an existing read-only context signal.

Open questions:

- In the next real scan, does sorting by `Contents` help separate broad buckets from focused cleanup candidates?

Rejected ideas buffer:

- Do not rank cleanup safety by contained-item count alone.

### 2026-05-28: Add Relative Paths to Scan Report Export

Status: completed

Evidence:

- Real-profile rows all share the long `C:\Users\moxhe` prefix, which can make spreadsheet review harder than the in-app tree context.
- Earlier progress left a relative-path export column as an open follow-up for Scan Report Export.

Implementation:

- Added a `Relative path` CSV column to `StorageScanCsvExporter`.
- Derived relative paths from the completed Cleanup Scope when available; unsupported or outside-scope rows leave the relative path blank.
- Updated WPF export and Review Shortlist export to pass the completed scan scope to CSV generation.
- Added a WPF test hook for the current Scan Report Export CSV and fixture coverage for relative paths.
- Kept this report-only; no scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible CSV report shape improvement.

Open questions:

- None.

Rejected ideas buffer:

- Do not replace full paths in exports; relative paths are additional review context.

### 2026-05-28: Show Access Issue Examples in Safety Summary

Status: completed

Evidence:

- The first real scan reported 3 access issues.
- The app already counted and filtered access issues, but the top safety summary did not show any concrete example paths before the user clicked into the filtered review.

Implementation:

- Added bounded access issue examples to `StorageScanSafetySummary`.
- Derived up to three cleanup-scope-relative examples from completed scan rows, including scanner error text when available.
- Updated WPF Safety Summary text to show access examples when incomplete scan coverage exists.
- Added core coverage for relative access issue examples and scanner error text.
- Kept this read-only; no scanner retry, permission change, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only summary display improvement.

Open questions:

- In the next real scan, are the three access issue examples enough context before using the Access issues filter?

Rejected ideas buffer:

- Do not add automatic elevated retries or permission changes for access issues.

### 2026-05-28: Add CI MVP Preflight Workflow

Status: completed

Evidence:

- Local MVP preflight already restores, builds, runs core tests, runs WPF app tests, runs the synthetic fixture generator in `-WhatIf` mode, and runs `git diff --check`.
- Remote pushes should use the same read-only gate so the command list does not drift from local verification.

Implementation:

- Added `.github/workflows/mvp-preflight.yml`.
- Configured the workflow for pushes and pull requests targeting `main`.
- Used a Windows runner, read-only repository permissions, .NET SDK `8.0.421`, and the existing `.\tools\Invoke-MvpPreflight.ps1` script.
- Documented CI preflight behavior in `README.md`.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- GitHub Actions `MVP Preflight` run `26575441204` for commit `711cfb6` completed with conclusion `success`.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-ci-mvp-preflight.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible repository verification workflow using the existing MVP preflight.

Open questions:

- None.

Rejected ideas buffer:

- Do not duplicate the local preflight command list in CI YAML.

### 2026-05-28: Add Cleanup Scope Browse Action

Status: completed

Evidence:

- The WPF app allowed typing a Cleanup Scope path, but manual fixture/custom scope review is less error-prone with native folder selection.
- Cleanup Scope Selection should remain separate from Storage Scan so fixture review and real-profile preflight gates stay explicit.

Implementation:

- Added a `Browse...` button beside the Cleanup Scope path box.
- Used WPF's native `OpenFolderDialog` to choose a folder.
- Folder selection updates `ScopePathBox`, which keeps existing Cleanup Scope Safety Note and Scan Gate behavior authoritative.
- Disabled browsing while scanning and re-enabled it after fixture scans.
- Added WPF smoke assertions for browse visibility and availability.
- No scanner traversal changes, auto-scan behavior, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-browse-action.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible WPF usability improvement that does not change scan, persistence, security, deployment, or cleanup behavior.

Open questions:

- Does the extra header button fit comfortably during the next manual fixture UI pass?

Rejected ideas buffer:

- Do not auto-start a scan after folder selection.

### 2026-05-28: Promote Specific Rebuildable Cache Candidates

Status: completed

Evidence:

- The real scan screenshot showed large cache rows such as `NVIDIA\DXCache` and `pip\Cache`.
- Broad AppData-adjacent folders should stay conservative, but specific rebuildable cache rows should be easier to find through Quarantine candidates.

Implementation:

- Added a narrow `HasSpecificRebuildableCacheEvidence` classifier rule.
- Promoted specific GPU shader cache rows and package-cache rows with app-cache evidence to `Likely safe` / `Quarantine candidate`.
- Kept broad parent folders such as `pip`, `NVIDIA`, generic `AppData`, browser data, installed apps, Windows app data, game data, source-code paths, and Codex-related paths conservative.
- Reordered Selected Path Review Guidance so likely-safe cache rows still show cache-specific warning text and Review Shortlist / Quarantine Preview wording.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cache-specific-review-guidance.md`
- `docs/features/2026-05-28-specific-rebuildable-cache-candidates.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible classification and review-guidance refinement that does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do `DXCache` and `pip\Cache` appearing under Quarantine candidates make review easier without making broad parent folders look safe?

Rejected ideas buffer:

- Do not promote generic `AppData`, app, profile, package, source-code, Codex, or browser parent folders just because they contain a cache-like descendant.

### 2026-05-28: Add Storage Review Relative Path Column

Status: completed

Evidence:

- The real scan includes many rows under the same `C:\Users\moxhe` prefix and many short or repeated names, such as cache folders and hashed files.
- Scan Report Export already includes cleanup-scope-relative paths for spreadsheet review; the WPF grid did not show that same compact context directly.

Implementation:

- Added `StorageEntryRow.RelativePath`, derived from the completed Cleanup Scope.
- Added a WPF `Relative path` grid column after `Name`.
- Added `Relative:` to the selected-row detail context above `Parent:`.
- Kept full parent path context and all cleanup recommendations unchanged.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-hierarchy-context.md`
- `docs/features/2026-05-28-storage-review-relative-path-column.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI review context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does the extra grid column fit comfortably during the next manual fixture and real-profile review passes?

Rejected ideas buffer:

- Do not remove the full `Parent` column yet; relative path and parent path answer different review questions.

### 2026-05-28: Show Safety Summary Candidate Examples

Status: completed

Evidence:

- Storage Scan Safety Summary already counted Quarantine candidates and showed access issue examples.
- Real-profile review benefits from concrete candidate examples before using filters, search, Review Shortlist, or Quarantine Preview.

Implementation:

- Added `StorageScanSafetySummary.QuarantineCandidateExamples`.
- Built up to three largest Quarantine candidate examples using cleanup-scope-relative paths and row sizes.
- Added `Candidate examples:` to WPF Safety Summary text.
- Kept examples read-only and separate from Review Shortlist, Quarantine Preview, and cleanup approval.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `docs/features/2026-05-28-safety-summary-candidate-examples.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only summary context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Do the candidate examples help real-profile review, or does the Safety Summary become too dense?

Rejected ideas buffer:

- Do not use Safety Summary candidate examples as cleanup approval or as a replacement for Review Shortlist and Quarantine Preview.

### 2026-05-28: Use Relative Paths for Quarantine Preview Blocker Examples

Status: completed

Evidence:

- Quarantine Preview protected-descendant blockers were useful, but absolute descendant paths are noisy in real-profile review where every row shares the same Cleanup Scope prefix.
- The app now uses relative paths elsewhere in the grid, detail pane, safety summary, and CSV reports.

Implementation:

- Updated protected-descendant blocker reasons to format descendant examples relative to the Cleanup Scope.
- Kept absolute source/destination paths in preview row details for precise identity.
- Added core and WPF smoke coverage for `.cache\codex-runtimes` relative blocker evidence.
- No scanner traversal changes, preview eligibility changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `docs/features/2026-05-28-quarantine-preview-relative-blocker-examples.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only preview wording and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, are relative blocker examples enough, or does the preview pane need a structured blocked-descendant table?

Rejected ideas buffer:

- Do not remove absolute source paths from preview row details; they remain useful for precise identity.

### 2026-05-28: Show Safety Summary No Category Examples

Status: completed

Evidence:

- Storage Scan Safety Summary counted Uncategorized Results and exposed a No category review shortcut, but it did not show concrete examples.
- The real scan surfaced many unfamiliar rows where the first step is classification, not cleanup.

Implementation:

- Added `StorageScanSafetySummary.UncategorizedExamples`.
- Built up to three largest no-category examples using cleanup-scope-relative paths and row sizes.
- Added `No category examples:` to WPF Safety Summary text.
- Kept examples read-only and separate from cleanup recommendations, Review Shortlist, and Quarantine Preview.
- No scanner traversal changes, classification changes, recommendation changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `docs/features/2026-05-28-safety-summary-no-category-examples.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only summary context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do no-category examples help target classification work, or does the Safety Summary become too dense?

Rejected ideas buffer:

- Do not treat no-category examples as cleanup candidates; they are classification prompts.

### 2026-05-28: Add Storage Size Threshold Filter

Status: completed

Evidence:

- User's real scan screenshot showed a 58.02 GB profile with many large rows, including multi-GB app/cache containers and many smaller recursive rows.
- The app already had review/category/type/search lenses, but no direct way to focus on rows above a chosen size.

Implementation:

- Added `StorageSizeThresholdFilter` with All sizes, 1 MB+, 100 MB+, 1 GB+, 5 GB+, and 10 GB+.
- Applied size-threshold filtering in `StorageScanReview` so WPF display, review-window paging, and Scan Report Export use the same active review lens.
- Added a WPF Size combo box, filter-summary wording, reset behavior, and export filename segment.
- Kept the change read-only; no scanner traversal changes, classification changes, recommendation changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-size-threshold-filter.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review filtering and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do `1 GB+`, `5 GB+`, and `10 GB+` make large-row triage easier without overemphasizing size as safety evidence?

Rejected ideas buffer:

- Do not make large rows more likely to be cleanup candidates solely because they match a size threshold.

### 2026-05-28: Extend Game Mod Data Protection Hints

Status: completed

Evidence:

- The real scan surfaced game and mod-related folders that should be clearer than generic uncategorized rows before any cleanup execution exists.
- Conservative app-data classification already treats game data as Protected Location / High risk / Keep.

Implementation:

- Extended game data hints to include Minecraft, OptiFine, CurseForge, Modrinth, Vortex, and Nexus Mods paths.
- Added fixture coverage proving OptiFine, CurseForge, and Vortex folders are `Game data`, `High risk`, and `Keep`.
- Kept the change conservative and read-only; no scanner traversal changes, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-conservative-app-data-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This extends an existing conservative classifier rule and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Which specific game or mod-manager cache subfolders, if any, should become cleanup exceptions after manual review?

Rejected ideas buffer:

- Do not treat mod-manager downloads or profiles as safe bloat just because they can be large.

### 2026-05-29: Add Cloud Sync and Credential Protection

Status: completed

Evidence:

- The app is intended to review `C:\Users\moxhe`, where cloud sync roots and credential/key/password-manager paths can be large or hidden but should not be cleanup targets by default.
- Selected File Content Preview should help with unfamiliar text files without exposing secrets from credential rows.

Implementation:

- Added `CloudSyncData` and `CredentialData` Bloat Category values.
- Added conservative classifier hints for common cloud sync providers and credential/password-manager/key paths.
- Added user-facing labels in WPF rows, category filters, Scan Report Export, and Quarantine Preview CSV.
- Blocked Selected File Content Preview for Credential Data.
- Added fixture coverage for OneDrive, Dropbox, SSH keys, Bitwarden, KeePass vault files, and credential preview blocking.
- Kept the change conservative and read-only; no scanner traversal changes, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-file-content-preview.md`
- `docs/features/2026-05-29-cloud-sync-credential-protection.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible conservative classification and preview-safety refinement that does not change architecture, persistence, deployment, or cleanup execution.

Open questions:

- Which cloud-provider cache folders, if any, should become cleanup exceptions after manual review?

Rejected ideas buffer:

- Do not preview credential file contents merely because the file is small or text-like.
- Do not classify cloud sync roots as cleanup candidates without provider-specific review.

### 2026-05-29: Add Quarantine Root Preview Selection

Status: completed

Evidence:

- The user requested a quarantine folder preferably on `D:` and easy undo later.
- Quarantine Preview already showed destination paths, but the preview root was an invisible fixed default.
- Keeping the root visible and editable improves review without adding cleanup execution.

Implementation:

- Added a WPF Quarantine root text box defaulting to `D:\WindowsFileCleanerQuarantine`.
- Routed Quarantine Preview destination paths through the typed Quarantine Root Selection.
- Cleared stale Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft output when the root changes.
- Added WPF smoke coverage proving custom preview roots affect destination paths and do not create folders.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview.md`
- `docs/features/2026-05-29-quarantine-root-preview-selection.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only preview UI refinement and does not change cleanup execution, persistence, deployment, or manifest writes.

Open questions:

- In the next manual fixture or real-profile pass, is the Quarantine root field readable in the wrapped review toolbar?

Rejected ideas buffer:

- Do not persist the Quarantine Root Selection until actual Quarantine execution needs local settings.
- Do not create or validate the quarantine folder by touching the filesystem during preview.

### 2026-05-29: Add Quarantine Root Browse Action

Status: completed

Evidence:

- The user prefers a quarantine location on `D:`.
- Typing a root path works, but browsing is less error-prone during manual review.
- Browsing can update preview destinations without creating folders or moving files.

Implementation:

- Added `BrowseQuarantineRootButton` next to the Quarantine root text box.
- Added a native folder picker for Quarantine Root Selection.
- Added WPF smoke coverage that the browse action is present and enabled before scanning.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-root-browse-action.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only UI refinement and does not change cleanup execution, persistence, deployment, or manifest writes.

Open questions:

- In the next manual fixture pass, does browsing to a `D:` root make preview destination review easier?

Rejected ideas buffer:

- Do not treat browsing to a root as approval to execute Quarantine.
- Do not create the selected folder during browse or preview.

### 2026-05-29: Split Review Shortlist and Quarantine Toolbar

Status: completed

Evidence:

- Quarantine Root Selection and browse controls made the review action toolbar wider.
- The next recommended work is a visible fixture pass, so keeping review controls easy to scan is useful before more manual testing.

Implementation:

- Kept search, row-window, type, size, and category controls in the existing wrapping Review Action toolbar.
- Moved Review Shortlist and Quarantine Preview controls into a separate wrapping `ReviewShortlistToolbar`.
- Extended the WPF smoke layout assertion to require the new wrapping toolbar.
- Kept the change UI-only and read-only; no scanner behavior, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `docs/features/2026-05-28-wpf-review-toolbar-layout-polish.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible UI layout polish and does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- In the next manual fixture pass, does the separated shortlist/quarantine toolbar make the Quarantine root controls easier to read?

Rejected ideas buffer:

- Do not treat structural layout assertions as proof of visible polish; keep the manual fixture pass as the visual check.

### 2026-05-29: Add Quarantine Root Safety Note

Status: completed

Evidence:

- Quarantine Root Selection is now typed or browsable, so obvious relative-path mistakes should be caught before Quarantine Preview builds destination paths.
- The user prefers quarantine on `D:`, but fixture review can still benefit from fully qualified non-`D:` preview roots.

Implementation:

- Added `QuarantineRootSafetyNote` and `QuarantineRootSafetyNoteBuilder`.
- Added WPF safety-note text under the Quarantine root controls.
- Disabled `Preview quarantine` when the current root is relative or invalid.
- Kept blank roots falling back to `D:\WindowsFileCleanerQuarantine`.
- Kept fully qualified non-`D:` roots usable for preview with non-preferred wording.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-root-safety-note.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only preview guard and does not change cleanup execution, persistence, deployment, or manifest writes.

Open questions:

- Should actual Quarantine execution require an existing `D:` folder, offer to create it, or support a stricter destination policy?

Rejected ideas buffer:

- Do not create or probe the quarantine folder just to decide whether Quarantine Preview can run.

### 2026-05-29: Add Quarantine Execution Gate

Status: completed

Evidence:

- Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft already prove preview and undo metadata shape.
- Actual Quarantine execution still needs explicit confirmation semantics before file-moving code exists.

Implementation:

- Added `QuarantineExecutionGate` and `QuarantineExecutionGateBuilder`.
- Added WPF confirmation text, disabled `Execute quarantine` button, and execution gate readout.
- Required exact `QUARANTINE` text while preserving confirmation-readiness blockers.
- Kept execution unavailable because Quarantine execution is not implemented.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-execution-gate.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only gate and does not decide file-moving layout, manifest write order, or Undo Quarantine behavior.

Open questions:

- What exact manifest write order should actual Quarantine execution use?
- Should future execution require a selected manifest path, a generated action id, or both?

Rejected ideas buffer:

- Do not add real file-moving code in the same packet as the first visible execution gate.
- Do not let matching confirmation text override data blockers or unimplemented execution support.

### 2026-05-29: Add Quarantine Action Draft

Status: completed

Evidence:

- Quarantine execution needs a concrete action-scoped destination and manifest layout before file-moving code exists.
- Preview paths should remain preview-only and separate from future executed quarantine paths.

Implementation:

- Added ADR 0004 for action-scoped quarantine layout.
- Added `QuarantineActionDraft`, `QuarantineActionEntryDraft`, and `QuarantineActionDraftBuilder`.
- Mapped future item paths under `<quarantine-root>\actions\<action-id>\items\...`.
- Mapped the future Restore Manifest path to `<quarantine-root>\actions\<action-id>\restore-manifest.json`.
- Added consistency checks across Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft metadata.
- Added WPF Quarantine Execution Gate readout for action items root and restore manifest path.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0004-use-action-scoped-quarantine-layout.md`
- `docs/features/2026-05-29-quarantine-action-draft.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0004-use-action-scoped-quarantine-layout.md`.

Open questions:

- What exact manifest write order should actual Quarantine execution use?
- How should partial move failures update the executed Restore Manifest?

Rejected ideas buffer:

- Do not reuse preview paths as executed quarantine paths.
- Do not use a flat quarantine root for all moved items.

### 2026-05-29: Add Write-Ahead Restore Manifest Model

Status: completed

Evidence:

- Sidecar safety review found a manifest timing conflict: domain context said to persist a Restore Manifest before moving files, while ADR 0003 still said after file moves are attempted.
- Actual Quarantine execution needs a recoverable write order and partial-failure states before any file-moving code exists.

Implementation:

- Added ADR 0005 for write-ahead Restore Manifest ordering.
- Added `RestoreManifest`, `RestoreManifestEntry`, `RestoreManifestBuilder`, and `RestoreManifestJsonSerializer`.
- Added `RestoreManifestActionStatus` and `RestoreManifestEntryStatus`.
- Built a planned Restore Manifest from the Quarantine Action Draft using action-scoped quarantine paths, not preview paths.
- Added in-memory status transitions for Moving, Moved, Failed, Completed, Partial failure, and Failed action outcomes.
- Added WPF Quarantine Execution Gate readout for write-ahead manifest status and write order.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest file writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0003-use-json-restore-manifest.md`
- `docs/decisions/0004-use-action-scoped-quarantine-layout.md`
- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/features/2026-05-29-quarantine-action-draft.md`
- `docs/features/2026-05-29-quarantine-execution-gate.md`
- `docs/features/2026-05-29-write-ahead-restore-manifest.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0005-use-write-ahead-restore-manifest.md`.

Open questions:

- What exact recovery UI should handle Moving entries after interruption?
- How should the future app surface leftover temp files after a hard crash?

Rejected ideas buffer:

- Do not write a final-only manifest after all moves succeed.
- Do not keep the old write-after-attempt wording; it creates a recovery gap.
- Do not add file-moving API allowlists until a narrow execution component exists.

### 2026-05-29: Add Restore Manifest File Store

Status: completed

Evidence:

- ADR 0005 requires a planned Restore Manifest to be written before any future move.
- Sidecar safety review recommended introducing write APIs only in a narrow execution component with a strict source allowlist.
- Manifest writing can be proven against fixtures before adding file-moving code.

Implementation:

- Added ADR 0006 for temp-file replacement Restore Manifest writes.
- Added `RestoreManifestFileStore` and `RestoreManifestFileWriteResult`.
- The file store validates that `ManifestPath` stays inside `ActionRootPath` and that the filename is `restore-manifest.json`.
- The file store writes JSON to a temporary file in the same action folder, then replaces or moves it into place.
- Added fixture-backed tests for first write, replacement write, invalid outside paths, invalid filenames, temp cleanup, source preservation, and not creating the action items folder.
- Updated the source-level filesystem-call regression to allow write APIs only for user-selected CSV exports and `RestoreManifestFileStore`.
- Kept WPF execution unavailable; no scanned files are moved, deleted, quarantined, or restored by the app.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `docs/features/2026-05-29-write-ahead-restore-manifest.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`.

Open questions:

- How should the future app surface leftover temp files after a hard crash?
- Should future Undo Quarantine expose a manifest integrity check before restore?

Rejected ideas buffer:

- Do not write manifests directly to the final path.
- Do not loosen the filesystem-call guard globally.
- Do not wire WPF execution in the same packet as the first manifest writer.

### 2026-05-29: Add Fixture-First Quarantine Executor

Status: completed

Evidence:

- Restore Manifest File Store is fixture-tested and can write action-scoped manifests.
- Sidecar safety review recommended a separate executor component, a narrow allowlist, and keeping WPF execution closed.
- MVP needs actual Quarantine movement eventually, but synthetic fixture execution should prove move semantics first.

Implementation:

- Added ADR 0007 for the fixture-first Quarantine Executor boundary.
- Added `QuarantineExecutor`, `QuarantineExecutionResult`, and `QuarantineExecutionEntryResult`.
- The executor writes the planned Restore Manifest before any move, writes Moving before each move attempt, revalidates source/destination/reparse status, moves the file or folder, then writes Moved or Failed.
- The executor continues after per-entry move failures so partial-failure manifests can be produced.
- The executor stops before later moves when a manifest write fails.
- Extended the source-level filesystem-call regression to allow `Directory.CreateDirectory`, `Directory.Move`, and `File.Move` only in `QuarantineExecutor`.
- Kept WPF execution unavailable; `Execute quarantine` remains disabled/status-only.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `docs/decisions/0007-use-fixture-first-quarantine-executor.md`
- `docs/features/2026-05-29-quarantine-executor-fixture-first.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0007-use-fixture-first-quarantine-executor.md`.

Open questions:

- What exact WPF stale-state checks are required before calling the executor?
- What recovery UI should handle Moving entries after interruption?
- How should the app surface leftover temp manifest files after a hard crash?

Rejected ideas buffer:

- Do not wire WPF execution in the same packet as the first executor.
- Do not implement rollback inside Quarantine Executor; Undo Quarantine needs a separate design.
- Do not overwrite existing quarantine destinations.

### 2026-05-29: Add Fixture-First Undo Quarantine

Status: completed

Evidence:

- User requested quarantine on `D:` with an easy undo path.
- Core fixture-first Quarantine Executor can already produce Moved Restore Manifest entries.
- ADR 0008 selects a separate fixture-first Undo Quarantine Executor before WPF execution or WPF undo is wired.

Implementation:

- Added `UndoQuarantineExecutor`, `UndoQuarantineResult`, and `UndoQuarantineEntryResult`.
- Extended Restore Manifest action statuses with Restoring, Restored, RestorePartialFailure, and RestoreFailed.
- Extended Restore Manifest entry statuses with Restoring, Restored, and RestoreFailed.
- Added restore start/completion timestamps to Restore Manifest entries.
- Undo restores only Moved entries, writes Restoring before each restore attempt, refuses original-path collisions, keeps move failures for recovery review, checks missing quarantine paths and reparse points, then writes Restored or RestoreFailed.
- Undo continues after per-entry restore failures and stops before later restore attempts when manifest writing fails.
- Extended the source-level filesystem-call regression to allow `Directory.CreateDirectory`, `Directory.Move`, and `File.Move` only in `UndoQuarantineExecutor` for restore movement.
- Kept WPF Quarantine execution and WPF Undo Quarantine unavailable.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `docs/decisions/0007-use-fixture-first-quarantine-executor.md`
- `docs/decisions/0008-use-fixture-first-undo-quarantine.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-quarantine-executor-fixture-first.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `docs/features/2026-05-29-undo-quarantine-fixture-first.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0008-use-fixture-first-undo-quarantine.md`.

Open questions:

- What UI should discover and select existing Restore Manifests?
- Should successful WPF Undo Quarantine offer to clean up empty action folders?
- How should the app surface leftover temp manifest files after a hard crash?

Rejected ideas buffer:

- Do not overwrite original paths during undo.
- Do not automatically delete quarantine action folders after restore.
- Do not implement same-execution rollback inside Quarantine Executor.
- Do not wire WPF Undo Quarantine in the same packet as core fixture undo.
