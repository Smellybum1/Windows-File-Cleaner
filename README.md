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

## Manual MVP Check

After the app opens:

1. Confirm the scope box shows the intended Cleanup Scope.
2. Confirm the Cleanup Scope Safety Note matches the path: fixture first for smoke testing, real profile only after preflight.
3. Click `Scan`.
4. Confirm the status says no files were modified.
5. Review the summary cards for total size, folders, files, and access issues.
6. If the status or filter summary says `2,000 shown of ... matched`, treat the grid as the first review window and narrow with search or filters.
7. Treat row sizes as triage clues, not storage savings; folder rows include children and can overlap with child rows.
8. Use Review Mix and Safety Summary to inspect high-risk, protected, access issue, reparse point, quarantine candidate, and no-category rows.
9. Use Storage Review Search for specific names such as `pip`, `NVIDIA`, `Codex`, app names, or game folders.
10. Select large folders and inspect Evidence, Review guidance, and Largest immediate children.
11. Try category filters such as App cache, Python package cache, GPU shader cache, Windows app data, Installed application, Game data, Protected location, and No category.
12. Add a likely-safe cleanup candidate to the Review Shortlist, or use `Shortlist shown` / `Remove shown` only after narrowing the grid to rows you intentionally want to review.
13. Click `Preview quarantine` and confirm the preview, Restore Manifest Draft, and Quarantine Confirmation Draft all say no files were modified and execution is not implemented.
14. Export CSV reports only when you intentionally choose an output file; the main report export follows the active filters/search, includes parent/depth context for recursive rows, and the suggested filename includes the search term when one is active.

## Current Workflow

The intended review flow is:

1. Run fixture tests.
2. Run the WPF app smoke tests.
3. Confirm the Cleanup Scope Safety Note before scanning.
4. Run Storage Scan.
5. Inspect high-risk and protected rows first.
6. Check whether the grid is showing all matched rows or the first 2,000 matched rows.
7. Use Storage Review Search and category filters to understand large buckets and specific app/tool paths.
8. Use Selected Path Review Guidance, Child Breakdown, and Open in Explorer for manual inspection.
9. Add interesting rows to Review Shortlist; use `Shortlist shown` and `Remove shown` only for the currently displayed review window.
10. Generate Quarantine Preview for read-only readiness review.
11. Stop before cleanup execution.

## Not Implemented Yet

- Actual Quarantine execution.
- Undo Quarantine.
- Permanent deletion.
- Persisted cleanup history.
- Exporting executed Restore Manifest files.

Those workflows require separate design, explicit confirmation semantics, restore rules, tests, and ADR review before any file-moving code is added.
