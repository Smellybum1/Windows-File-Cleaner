# Feature: Storage Entry Type Filter

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make recursive Storage Scan results easier to review by letting the user switch between all rows, files only, and folders only.

## Non-goals

- Do not change scanner traversal.
- Do not hide files or folders from the completed scan result.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not treat files-only or folders-only views as cleanup approval.

## User story / job story

As the project owner, I want to filter the review grid to files or folders, so that I can inspect individual large files separately from container folders.

## Desired behavior

- Add a Type filter with `All types`, `Files`, and `Folders`.
- Combine Type with Storage Review Filter, Bloat Category Filter, and Storage Review Search.
- Include the active Type filter in the filter summary and Scan Report Export filename.
- Keep Review Shortlist actions scoped to the currently displayed rows after all active filters.
- Keep all behavior read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Entry Type Filter | Added as read-only file/folder review lens. | yes |

## Decisions made

- Implement the filter in `StorageScanReview` so WPF display, shortlist-shown behavior, and CSV exports use the same active review lens.
- Reset to `All types` after each completed scan.
- Use a compact WPF combo box to avoid another wide button group.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `StorageEntryTypeFilter`.
- Added core filtering for files and folders.
- Added a WPF Type filter combo box.
- Updated Scan Report Export filenames to include the active type segment.
- Added core and WPF smoke coverage.

ADRs added or skipped:

- No ADR. This is reversible read-only review filtering and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Try `Files` plus `Large old file` on the next real scan.
- Try `Folders` plus protected/no-category filters while inspecting containers.
