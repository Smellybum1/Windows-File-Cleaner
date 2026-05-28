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

Run these from the repository root before scanning real user files:

```powershell
dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config
dotnet build WindowsFileCleaner.sln --no-restore
dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build
dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build
```

Expected test output:

```txt
All WindowsFileCleaner.Tests checks passed.
All WindowsFileCleaner.App.Tests checks passed.
```

## WPF Fixture Smoke

Create a small synthetic Cleanup Scope inside the repo:

```powershell
.\tools\New-StorageScanSmokeFixture.ps1
```

Then run the printed command, which has this shape:

```powershell
dotnet run --project src\WindowsFileCleaner.App -- --scope "D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture"
```

This only fills the Cleanup Scope box. Click `Scan` yourself after the app opens.

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
2. Click `Scan`.
3. Confirm the status says no files were modified.
4. Review the summary cards for total size, folders, files, and access issues.
5. Use Review Mix and Safety Summary to inspect high-risk, protected, access issue, reparse point, quarantine candidate, and no-category rows.
6. Select large folders and inspect Evidence plus Largest immediate children.
7. Try category filters such as App cache, Python package cache, GPU shader cache, Windows app data, Installed application, Game data, Protected location, and No category.
8. Add a likely-safe cleanup candidate to the Review Shortlist.
9. Click `Preview quarantine` and confirm the preview, Restore Manifest Draft, and Quarantine Confirmation Draft all say no files were modified and execution is not implemented.
10. Export CSV reports only when you intentionally choose an output file.

## Current Workflow

The intended review flow is:

1. Run fixture tests.
2. Run the WPF app shell smoke test.
3. Run Storage Scan.
4. Inspect high-risk and protected rows first.
5. Use category filters to understand large buckets.
6. Use Child Breakdown and Open in Explorer for manual inspection.
7. Add interesting rows to Review Shortlist.
8. Generate Quarantine Preview for read-only readiness review.
9. Stop before cleanup execution.

## Not Implemented Yet

- Actual Quarantine execution.
- Undo Quarantine.
- Permanent deletion.
- Persisted cleanup history.
- Exporting executed Restore Manifest files.

Those workflows require separate design, explicit confirmation semantics, restore rules, tests, and ADR review before any file-moving code is added.
