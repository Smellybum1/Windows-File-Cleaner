# Feature: Access Status Review Field

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make each Storage Scan row's scan access state explicit in the WPF review UI and CSV exports.

## Non-goals

- Do not retry access issues.
- Do not request elevated permissions.
- Do not change scanner traversal or access handling.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, deletion, or manifest writing.

## User story / job story

As the project owner, I want each row to say whether it was readable or had an access issue, so that I can trust the review evidence and spot incomplete scan coverage without inferring it from categories or error text.

## Desired behavior

- WPF review rows expose `Access` as `Readable` or `Access issue`.
- Selected-row details include `Access: ...` in the metadata line.
- Scan Report CSV includes `Access status` and `Access issue` columns.
- Quarantine Preview CSV includes `Access status` and `Access issue` columns.
- Access issues remain informational and read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Access Status | Added as the readable/access-issue label for scan rows and exports. | yes |

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `StorageEntryRow.AccessStatus`.
- Added an `Access` column to the WPF Storage Scan grid.
- Added selected-row access status metadata.
- Added `Access status` columns to Scan Report CSV and Quarantine Preview CSV exports.
- Added core CSV and WPF fixture coverage.

ADRs added or skipped:

- No ADR. This is a reversible read-only display/export field.

Follow-up work:

- In the next real scan, confirm the three access issue rows are easy to find and understand.
