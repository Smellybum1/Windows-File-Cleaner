# Feature: Storage Review Field-Prefix Search

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make large Storage Scan reviews easier to narrow by allowing the user to search one field at a time when broad search is too noisy.

## Non-goals

- Do not rescan the filesystem.
- Do not search outside the completed Storage Scan result.
- Do not persist search history.
- Do not change Bloat Categories, Importance Ratings, Deletion Recommendations, or cleanup eligibility based on search text.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.

## User story / job story

As the project owner, I want searches like `path:pip` and `category:Python package cache`, so that I can separate path matches from category/rating/recommendation matches while reviewing a large profile scan.

## Desired behavior

- Unprefixed search keeps the existing broad behavior.
- Recognized prefixes restrict matching to one field:
  - `path:`
  - `name:`
  - `category:` / `cat:`
  - `rating:` / `importance:`
  - `recommendation:` / `rec:`
  - `evidence:`
  - `issue:` / `access:`
- The WPF search box tooltip shows examples.
- Filter summary and report filenames preserve the typed query, including the prefix.
- All behavior remains in-memory and read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Search | Expanded with optional field prefixes. | yes |
| Storage Review Search Field | Added to represent the parsed search field. | yes |

## Decisions made

- Preserve unprefixed broad search so existing workflows keep working.
- Treat unrecognized prefixes as literal broad search text.
- Treat recognized prefixes with a blank term as inactive search.
- Keep field-prefix parsing in core so WPF and report exports share the same behavior.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `StorageReviewSearchField`.
- Added prefix parsing to `StorageReviewSearch`.
- Updated `StorageScanReview` search matching to honor the parsed field.
- Added WPF tooltip examples.
- Added core and WPF smoke coverage for prefixed search.

ADRs added or skipped:

- No ADR. This is reversible in-memory search behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Try `path:`, `category:`, `rating:`, and `recommendation:` in the next manual fixture/real scan UI pass.
