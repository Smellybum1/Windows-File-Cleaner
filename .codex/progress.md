# Progress Log

This file is the running evidence log for Codex work in this repository.

Use it to preserve what was completed, what was verified, what was rejected, and what should happen next. Keep entries compact and factual.

## Current status

Storage Scan MVP packet implemented and tested by the user against `C:\Users\moxhe`. Review filters are pushed. Selected-folder child breakdown has been implemented locally and is ready to commit/push.

## Next recommended work

1. Commit and push the selected-folder child breakdown checkpoint.
2. Ask the user to rerun the WPF app and select big folders such as `AppData`, `Local`, `pip`, and `DXCache`.
3. Use the user's child-breakdown feedback to refine Protected Locations and category grouping.
4. Defer Quarantine and Undo Quarantine implementation until scan review is trustworthy.
5. Revisit .NET 10 before packaging or long-term distribution.

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
