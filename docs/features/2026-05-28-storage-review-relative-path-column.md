# Feature: Storage Review Relative Path Column

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make real-profile Storage Scan rows easier to read in the WPF grid by showing a cleanup-scope-relative path alongside the row name and parent path.

## Non-goals

- Do not change scanner traversal, classification, recommendations, Review Shortlist, Quarantine Preview, or cleanup eligibility.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, manifest writing, or scan history.
- Do not scan `C:\Users\moxhe` from tests or automation.
- Do not treat relative path as cleanup approval.

## User story / job story

As the project owner, I want rows like `AppData\Local\pip\Cache` visible directly in the grid, so that I can understand short names and deep cache paths without mentally subtracting `C:\Users\moxhe` from full parent paths.

## Desired behavior

- The Storage Scan grid shows a `Relative path` column after `Name`.
- The selected-row detail pane shows `Relative:` above `Parent:`.
- Relative paths are derived from the completed Cleanup Scope.
- The Cleanup Scope root displays `.` as its relative path.
- Rows outside the completed Cleanup Scope or invalid paths display an empty relative path.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Path Hierarchy Context | Expanded to include cleanup-scope-relative path. | yes |

## Decisions made

- Reuse the existing relative-path semantics already used by Scan Report Export.
- Keep the full `Parent` column because it still helps inspect absolute locations and unusual paths.
- Put `Relative path` in the grid, not only the detail pane, because the real scan contains many repeated prefixes and short names.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `StorageEntryRow.RelativePath`.
- Added a WPF `Relative path` grid column.
- Added selected-row `Relative:` detail context.
- Added WPF smoke coverage for the column, Cleanup Scope root relative path, folder relative path, and detail-pane relative path wording.

ADRs added or skipped:

- No ADR. This is reversible read-only UI review context and does not change architecture, persistence, security, deployment, or cleanup behavior.

Follow-up work:

- Check the next real scan to confirm the extra column improves readability without making the grid too crowded.
