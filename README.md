# Windows File Cleaner

Windows File Cleaner is a local Windows-only WPF desktop app for reviewing storage under `C:\Users\moxhe`.

The current MVP is a read-only Storage Scan. It recursively scans the selected Cleanup Scope, classifies cleanup candidates, shows importance ratings, and helps inspect large folders before any cleanup action exists.

Current readiness evidence is tracked in `docs/features/2026-05-28-mvp-readiness-audit.md`.

## Safety Status

- Storage Scan does not modify scanned files.
- The app does not delete, move, quarantine, or restore files.
- CSV exports write only to a path selected by the user.
- Review Shortlist is an in-memory review aid, not cleanup approval.
- Quarantine Preview is a dry run only.
- Restore Manifest Draft and Quarantine Confirmation Draft are in-memory readiness evidence only.
- Fixture tests include a source-level guard against accidental cleanup-execution filesystem calls.
- Real-profile scans require an explicit acknowledgement that MVP preflight and fixture review were run.

## Requirements

- Windows 11
- .NET 8 SDK and Windows Desktop runtime
- Local repo path: `D:\Codex\Windows File Cleaner`

## Verify Before Real Scan

Run the MVP preflight from the repository root before scanning real user files:

```powershell
.\tools\Invoke-MvpPreflight.ps1
```

The preflight restores, builds, runs both test harnesses, runs the fixture generator in `-WhatIf` mode, and runs `git diff --check`. It does not scan `C:\Users\moxhe`.

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

Pushes and pull requests to `main` run the same MVP preflight on GitHub Actions with a Windows runner and .NET 8. The CI job restores, builds, runs both test harnesses, runs the fixture generator in `-WhatIf` mode, and runs `git diff --check`. It does not scan `C:\Users\moxhe`.

## WPF Fixture Smoke

Use the fixture review launcher for the manual fixture UI pass:

```powershell
.\tools\Start-MvpFixtureReview.ps1
```

The launcher runs preflight, creates a small synthetic Cleanup Scope inside the repo, and launches the WPF app with that scope. The app does not auto-scan; click `Scan` yourself after it opens.

For focused troubleshooting, the individual fixture commands are:

```powershell
.\tools\New-StorageScanSmokeFixture.ps1
```

Then run the printed command, which has this shape:

```powershell
dotnet run --project src\WindowsFileCleaner.App -- --scope "D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture"
```

This only fills the Cleanup Scope box. Click `Scan` yourself after the app opens.

The automated `WindowsFileCleaner.App.Tests` project also scans a synthetic fixture through the WPF shell, exercises read-only review interactions, and checks that the review toolbars use wrapping layout, but it does not replace checking the visible layout and controls by eye.

## Run The App

```powershell
dotnet run --project src\WindowsFileCleaner.App
```

Default Cleanup Scope:

```txt
C:\Users\moxhe
```

When the app is opened against the default real-profile Cleanup Scope, `Scan` stays disabled until you tick the preflight and fixture-review acknowledgement in the header.

## Manual MVP Check

After the app opens:

1. Confirm the scope box shows the intended Cleanup Scope; use `Browse...` if you want to choose a fixture or custom folder before scanning.
2. Confirm the Cleanup Scope Safety Note matches the path: fixture first for smoke testing, real profile only after preflight.
3. For the real profile, tick the acknowledgement that MVP preflight and fixture review were run; fixture scopes do not require this real-profile acknowledgement.
4. Click `Scan`.
5. Confirm the status says no files were modified.
6. Review the summary cards for total size, folders, files, and access issues.
7. If the status or filter summary says `rows 1-2,000 of ... matched`, use `Next rows` / `Previous rows` to move through matched rows, or narrow with search and filters.
8. Treat row sizes as triage clues, not storage savings; folder rows include children and can overlap with child rows.
9. Use Review Mix and Safety Summary to inspect the cleanup scope root, high-risk, protected, access issue examples, Quarantine candidate examples, No category examples, reparse point, quarantine candidate, and no-category rows.
10. Use Storage Review Search for specific names such as `pip`, `NVIDIA`, `Codex`, app names, or game/mod-manager folders; use prefixes such as `path:pip`, `category:Python package cache`, `rating:High risk`, `recommendation:Quarantine candidate`, `access:readable`, or `access:access issue` when you want one field.
11. Use the `Relative path`, `Parent`, `Contents`, and `Access` columns for short, hashed, container, or unreadable row names; sort `Contents` when you want to compare rows by total contained items, then select large folders and inspect relative path, parent/depth context, Evidence, cache-specific Review guidance, and Largest immediate children.
12. Use the Type filter to switch between all rows, files only, and folders only; use the Size filter to focus on rows such as `100 MB+`, `1 GB+`, or `5 GB+`.
13. Select small text files and use `Preview file` only when you intentionally want a bounded read-only text snippet; binary and unsupported files should not render as text.
14. Try category filters such as Cleanup scope root, App cache, Python package cache, GPU shader cache, Large old file, Cloud sync data, Credential data, Windows app data, Installed application, Game data, Protected location, and No category.
15. Use `Reset view` after stacking filters/search; it clears the review lens but keeps Review Shortlist.
16. Add a likely-safe cleanup candidate to the Review Shortlist; specific rebuildable cache rows such as `DXCache` or `pip\Cache` may appear here, while broad parent folders should stay inspection-first. Use `Shortlist shown` / `Remove shown` only after narrowing or paging the grid to rows you intentionally want to review.
17. Confirm the Quarantine root points to the intended read-only preview destination, then click `Preview quarantine`; broad parent rows should be blocked when protected descendants are present, blocked descendant examples should use relative paths, confirmation readiness blockers should be separate from preview row details, and Restore Manifest Draft / Quarantine Confirmation Draft should still say no files were modified and execution is not implemented.
18. Export CSV reports only when you intentionally choose an output file; the main report export follows the active filters/type/size/search, includes relative path, parent/depth, and access-status context for recursive rows, and the suggested filename includes the search term when one is active.

## Current Workflow

The intended review flow is:

1. Run fixture tests.
2. Run the WPF app smoke tests.
3. Confirm the Cleanup Scope Safety Note before scanning.
4. Run Storage Scan.
5. Inspect high-risk and protected rows first.
6. Check whether the grid is showing all matched rows or one 2,000-row display window.
7. Use `Next rows` / `Previous rows`, Storage Review Search, Type filter, Size filter, and category filters to understand large buckets and specific app/tool paths.
8. Use `Reset view` when the active review lens becomes too narrow; it does not clear Review Shortlist.
9. Use Selected Path Hierarchy Context, Selected File Content Preview, Selected Path Review Guidance, Child Breakdown, and Open in Explorer for manual inspection.
10. Add interesting rows to Review Shortlist; use `Shortlist shown` and `Remove shown` only for the currently displayed review window.
11. Check the Quarantine root and generate Quarantine Preview for read-only readiness review.
12. Stop before cleanup execution.

## Not Implemented Yet

- Actual Quarantine execution.
- Undo Quarantine.
- Permanent deletion.
- Persisted cleanup history.
- Exporting executed Restore Manifest files.

Those workflows require separate design, explicit confirmation semantics, restore rules, tests, and ADR review before any file-moving code is added.
