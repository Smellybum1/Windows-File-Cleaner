# Windows File Cleaner

Windows File Cleaner is a local Windows-only WPF desktop app for reviewing storage under `C:\Users\moxhe`.

The current MVP centers on a read-only Storage Scan. It can also execute and undo Quarantine from the visible WPF app against synthetic fixture Cleanup Scopes only, discover action-scoped Restore Manifests under the selected Quarantine Root, select one discovered Restore Manifest for review, preview selected restore confirmation, and restore selected discovered fixture manifests. Real-profile restore remains unavailable.

Current readiness evidence is tracked in `docs/features/2026-05-28-mvp-readiness-audit.md`.

Fresh-thread handoff notes live in `docs/codex/thread-handoff.md`.

## Safety Status

- Storage Scan does not modify scanned files.
- The visible WPF app can move files only when the Cleanup Scope is a recognized synthetic fixture and the exact Quarantine confirmation gate is open.
- The visible WPF app can undo only the current synthetic fixture Quarantine execution.
- The visible WPF app can discover action-scoped Restore Manifests under the selected Quarantine Root without restoring them.
- The visible WPF app can select one discovered Restore Manifest and preview selected manifest readiness without restoring it.
- The visible WPF app can restore a selected discovered fixture Restore Manifest after selected manifest readiness and exact `RESTORE` confirmation.
- The visible WPF app keeps selected restore execution unavailable for real-profile and custom non-fixture Restore Manifests.
- Discovery, selected-manifest review, and restore-readiness panes do not expose all-manifest restore actions; fixture selected restore goes through selected manifest readiness and the selected restore gate.
- Selected restore gate panes show fixture-only versus preview-only scope status and keep selected manifest readiness separate from restore approval.
- The visible WPF app can preview all-manifest readiness for discovered manifests without restoring them.
- Real-profile WPF Quarantine execution and broad WPF Undo Quarantine remain unavailable.
- The visible WPF app does not delete files.
- CSV exports write only to a path selected by the user.
- Review Shortlist is an in-memory review aid, not cleanup approval.
- Review Shortlist export and clear controls use tooltips to keep report-only and in-memory-only boundaries visible.
- Review Shortlist bulk actions label their scope as visible rows and include tooltips so they apply only to the current displayed review window, not cleanup approval.
- Review Shortlist Safety Mix summarizes shortlisted rows from completed scan data only; it is review context, not cleanup approval or storage-savings proof.
- Quarantine Preview is a dry run only, and its preview/gate panes keep Review Shortlist and Quarantine Preview separate from cleanup approval.
- Quarantine Preview and preview export controls use tooltips to keep dry-run and report-only boundaries visible.
- Restore Manifest Draft and Quarantine Confirmation Draft are in-memory readiness evidence only.
- Quarantine Execution Gate enables execution only for fixture Cleanup Scopes after preview readiness and exact `QUARANTINE` confirmation, and its scope-status wording keeps real-profile/custom execution visibly preview-only.
- Quarantine and selected-restore execution controls have disabled-state tooltips that keep fixture-only gates and real-profile/custom blockers visible.
- All-manifest and selected-manifest readiness controls have scope tooltips that keep read-only/no-restore and selected-only/not-approval boundaries visible.
- Quarantine Action Draft shows action-scoped item and manifest paths before execution creates them.
- Write-ahead Restore Manifest modeling shows planned status/write order before fixture execution writes the manifest.
- Restore Manifest File Store writes action-scoped manifest JSON during fixture-tested execution paths.
- Quarantine Executor is fixture-tested in the core library and wired to the WPF app for fixture scopes only.
- Undo Quarantine Executor is fixture-tested in the core library and wired to the WPF app for current-fixture undo and fixture-only selected restore.
- Quarantine Manifest Discovery is read-only and does not move, restore, delete, create, or clean up files or folders.
- Selected Restore Manifest Review, Selected Restore Confirmation Draft, and Restore Readiness Preview are read-only and do not call Undo Quarantine execution.
- Fixture-only Selected Restore Execution calls Undo Quarantine Executor for selected discovered fixture manifests only.
- Fixture tests include a source-level guard against accidental cleanup-execution filesystem calls.
- Real-profile scans require an explicit acknowledgement that MVP preflight and fixture review were run.
- Scan-gate ready wording is scope-specific: fixture scopes point later cleanup actions back to preview and exact confirmation, while real-profile/custom scopes keep cleanup execution unavailable.
- User-typed Storage Review Search is debounced for large real-profile scans; the status bar is the only pending-search indicator for now.

## Requirements

- Windows 11
- .NET 8 SDK and Windows Desktop runtime
- Local repo path: `D:\Codex\Windows File Cleaner`

## Verify Before Real Scan

Run the MVP preflight from the repository root before scanning real user files:

```powershell
.\tools\Invoke-MvpPreflight.ps1
```

The preflight restores, builds, runs both test harnesses, runs the fixture generator in `-WhatIf` mode, and runs `git diff --check`. It fails if any child command exits non-zero. It does not scan `C:\Users\moxhe`.

The individual commands are:

```powershell
dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config
dotnet build WindowsFileCleaner.sln --no-restore
dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build
dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf
git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check
```

Expected test output:

```txt
All WindowsFileCleaner.Tests checks passed.
All WindowsFileCleaner.App.Tests checks passed.
```

## CI Preflight

Pushes and pull requests to `main` run the same MVP preflight on GitHub Actions with a Windows runner and .NET 8. The CI job restores, builds, runs both test harnesses, runs the fixture generator in `-WhatIf` mode, and runs `git diff --check`; any non-zero child command fails the job. It does not scan `C:\Users\moxhe`.

## WPF Fixture Smoke

Use the fixture review launcher for the manual fixture UI pass:

```powershell
.\tools\Start-MvpFixtureReview.ps1
```

The launcher runs preflight, creates a small synthetic Cleanup Scope inside the repo, and launches the WPF app with that scope. The app does not auto-scan; click `Scan` yourself after it opens.
Before launching, the script prints a compact fixture review checklist with the main safety, search, Review Shortlist labels/tooltips, preview/report tooltips, preview approval-boundary, fixture execution, undo, manifest-review, all-manifest restore boundary, and selected-restore scope-status checks.

To print only that checklist without running preflight, creating fixture files, or launching WPF:

```powershell
.\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly
```

For focused troubleshooting, the individual fixture commands are:

```powershell
.\tools\New-StorageScanSmokeFixture.ps1
```

Then run the printed command, which has this shape:

```powershell
dotnet run --project src\WindowsFileCleaner.App -- --scope "D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture"
```

This only fills the Cleanup Scope box. Click `Scan` yourself after the app opens.

Use `-SkipChecklist` only when you intentionally want the launcher output without the reminder checklist.

The automated `WindowsFileCleaner.App.Tests` project also scans a synthetic fixture through the WPF shell, exercises read-only review interactions, proves fixture-only Quarantine execution and undo, verifies manifest discovery, selected manifest review, selected restore confirmation gate, fixture-only selected restore execution, and all-manifest readiness preview, verifies custom non-fixture execution remains blocked, and checks that the review toolbars use wrapping layout, but it does not replace checking the visible layout and controls by eye.

## Run The App

```powershell
dotnet run --project src\WindowsFileCleaner.App
```

Default Cleanup Scope:

```txt
C:\Users\moxhe
```

When the app is opened against the default real-profile Cleanup Scope, `Scan` stays disabled until you tick the preflight and fixture-review acknowledgement in the header.
The header also shows a scan-gate status line and Scan button tooltip so the locked or ready state is visible before scanning, with scope-specific wording for fixture, real-profile, and custom scopes.

## Manual MVP Check

After the app opens:

1. Confirm the scope box shows the intended Cleanup Scope; use `Browse...` if you want to choose a fixture or custom folder before scanning.
2. Confirm the Cleanup Scope Safety Note matches the path: fixture first for smoke testing, real profile only after preflight.
3. For the real profile, confirm the scan-gate status says the scan is locked, then tick the acknowledgement that MVP preflight and fixture review were run; fixture scopes do not require this real-profile acknowledgement.
4. Click `Scan`.
5. Confirm the status says no files were modified.
6. Review the summary cards for total size, folders, files, and access issues.
7. If the status or filter summary says `rows 1-2,000 of ... matched`, use `Next rows` / `Previous rows` to move through matched rows, or narrow with search and filters.
8. Treat row sizes as triage clues, not storage savings; folder rows include children and can overlap with child rows.
9. Use Review Mix, Matched Review Mix, and Safety Summary to inspect the cleanup scope root, high-risk, protected, access issue examples, Quarantine candidate examples, No category examples, reparse point, quarantine candidate, and no-category rows. Review Mix summarizes the whole scan; Matched Review Mix summarizes the current filters/search/focus.
10. Use Storage Review Search for specific names such as `pip`, `NVIDIA`, `Codex`, app names, or game/mod-manager folders; use prefixes such as `path:pip`, `parent:C:\Users\moxhe\AppData`, `under:C:\Users\moxhe\AppData`, `category:Python package cache`, `rating:High risk`, `recommendation:Quarantine candidate`, `access:readable`, or `access:access issue` when you want one field.
    Search typed into the box applies after a short pause so large scan results do not re-filter on every keystroke.
11. Use the `Relative path`, `Parent`, `Contents`, and `Access` columns for short, hashed, container, or unreadable row names; sort `Contents` when you want to compare rows by total contained items, then select large folders and inspect relative path, parent/depth context, Evidence, cache-specific Review guidance, Descendant review summary, Largest immediate children, and the Largest hotspot trail.
12. Use the Descendant review summary to see the selected folder's descendant mix by rating, quarantine candidate count, protected rows, access issues, reparse points, and no-category rows; these counts are review context, not cleanup approval.
13. Use the Largest hotspot trail to see the biggest nested path through a selected folder; the sizes overlap down the trail and are not storage savings.
14. Use `Show children` on a selected folder to focus the grid on its immediate children; this applies a `parent:` search, resets other review lenses to All, and remains read-only.
15. Use `Show descendants` on a selected folder to focus the grid on every scanned descendant; this applies an `under:` search, excludes the selected folder itself, resets other review lenses to All, updates Matched Review Mix, and remains read-only.
16. Use the Type filter to switch between all rows, files only, and folders only; use the Size filter to focus on rows such as `100 MB+`, `1 GB+`, or `5 GB+`.
17. Select small text files and use `Preview file` only when you intentionally want a bounded read-only text snippet; binary and unsupported files should not render as text.
18. Try category filters such as Cleanup scope root, App cache, Python package cache, GPU shader cache, Large old file, Cloud sync data, Credential data, Windows app data, Installed application, Game data, Protected location, and No category.
19. Use `Reset view` after stacking filters/search; it clears the review lens but keeps Review Shortlist.
20. Add a likely-safe cleanup candidate to the Review Shortlist; specific rebuildable cache rows such as `DXCache` or `pip\Cache` may appear here, while broad parent folders should stay inspection-first. Check Review Shortlist Safety Mix for high-risk, protected, access issue, no-category, and largest-row context. Use `Shortlist visible rows` / `Remove visible rows` only after narrowing or paging the grid to rows you intentionally want to review, and confirm their tooltips keep visible-row scope, no-file-modified behavior, and not-cleanup-approval wording clear. Confirm `Export shortlist` and `Clear shortlist` tooltips keep report-only and in-memory-only wording clear.
21. Confirm the Quarantine root points to the intended fully qualified preview/execution destination and that the safety note matches it, typing or browsing if needed, then click `Preview quarantine`; broad parent rows should be blocked when protected descendants are present, blocked descendant examples should use relative paths, confirmation readiness blockers should be separate from preview row details, approval-boundary wording should keep shortlist/preview separate from cleanup approval, Preview quarantine and Export preview tooltips should keep dry-run/report-only wording visible, execution scope status and disabled control tooltips should distinguish fixture-only from preview-only real/custom scopes, the Quarantine Action Draft should show action-scoped item and manifest paths, and the write-ahead Restore Manifest should show planned write-before-move ordering.
22. On a fixture Cleanup Scope only, typing `QUARANTINE` should enable `Execute quarantine`; clicking it moves the selected synthetic file/folder into the action-scoped quarantine path, writes `restore-manifest.json`, clears stale shortlist state, enables `Undo fixture quarantine`, and tells you to rescan.
23. On that same fixture execution, clicking `Undo fixture quarantine` should restore the synthetic file/folder from quarantine, update the Restore Manifest, disable repeat undo, and keep stale-state wording visible.
24. Use `Discover manifests` against the selected Quarantine Root; it should show read-only Restore Manifest summaries or discovery issues and should state that no all-manifest restore action is available.
25. Select a discovered Restore Manifest and use `Preview selected manifest readiness`; it should show readiness for that manifest only without moving files, its tooltip should keep selected-only/not-approval wording visible, and it should route any fixture-only restore through the selected restore gate.
26. Use `Preview selected restore gate`, then type `RESTORE`; for a fixture Restore Manifest it should show fixture-only scope status, approval-boundary wording, disabled-control tooltip wording, and `Can execute: yes`, and `Restore selected fixture manifest` should restore the synthetic file while telling you to rediscover and rescan.
27. Use `Preview all-manifest readiness` against the selected Quarantine Root; it should show restorable, blocked, already-restored, or recovery-review rows across discovered manifests without moving files, and its tooltip should keep read-only/no-restore wording visible.
28. On `C:\Users\moxhe` or a custom non-fixture Cleanup Scope, typing `QUARANTINE` should still leave `Execute quarantine` disabled with a scope-specific blocker and no undo action; selected restore should also show preview-only scope status and stay unavailable for non-fixture manifests even when `RESTORE` is typed.
29. Export CSV reports only when you intentionally choose an output file; the main report export follows the active filters/type/size/search, includes relative path, parent/depth, and access-status context for recursive rows, and the suggested filename includes the search term when one is active.

## Current Workflow

The intended review flow is:

1. Run fixture tests.
2. Run the WPF app smoke tests.
3. Confirm the Cleanup Scope Safety Note before scanning.
4. Run Storage Scan.
5. Inspect high-risk and protected rows first.
6. Check whether the grid is showing all matched rows or one 2,000-row display window.
7. Use `Next rows` / `Previous rows`, Storage Review Search, `parent:` search, `under:` search, Type filter, Size filter, category filters, and Matched Review Mix to understand large buckets and specific app/tool paths.
8. Use `Reset view` when the active review lens becomes too narrow; it does not clear Review Shortlist.
9. Use Selected Path Hierarchy Context, Selected File Content Preview, Selected Path Review Guidance, Selected Folder Subtree Summary, Child Breakdown, Storage Hotspot Trail, Selected Folder Child Focus, Selected Folder Descendant Focus, and Open in Explorer for manual inspection.
10. Add interesting rows to Review Shortlist; use Review Shortlist Safety Mix to sanity-check the shortlisted row mix, and use `Shortlist visible rows` / `Remove visible rows` only for the currently displayed review window. Their tooltips should repeat that this is review context, not cleanup approval, while export/clear tooltips keep report-only and in-memory-only boundaries visible.
11. Check or browse the Quarantine root and generate Quarantine Preview for readiness review; preview/export tooltips should keep dry-run and report-only boundaries visible.
12. For fixture scopes, optionally type `QUARANTINE`, run fixture-only Quarantine execution, then use `Undo fixture quarantine` to prove the reversible visible workflow.
13. Use `Discover manifests` when you want read-only status for action-scoped Restore Manifests under the selected Quarantine Root.
14. Select one discovered Restore Manifest and use `Preview selected manifest readiness` when you want one-action blocker evidence before any future broad Undo Quarantine.
15. Use `Preview selected restore gate` and type `RESTORE`; fixture selected manifests can be restored, while real-profile/custom selected restore stays unavailable.
16. Use `Preview all-manifest readiness` when you want read-only blocker evidence across all discovered manifests.
17. For real-profile scopes, confirm `Execute quarantine` and broad Undo stay unavailable.
18. Stop before real-profile cleanup execution.

## Not Implemented Yet

- Real-profile WPF Quarantine execution.
- Real-profile WPF Undo Quarantine.
- WPF restore execution for selected discovered real-profile quarantine actions.
- Permanent deletion.
- Persisted cleanup history.
- Writing executed Restore Manifest files from the WPF app for real-profile Cleanup Scopes.

Those workflows require separate design, explicit confirmation semantics, stale-state checks, restore-manifest selection, tests, and ADR review before the visible app can move or restore real-profile files.
