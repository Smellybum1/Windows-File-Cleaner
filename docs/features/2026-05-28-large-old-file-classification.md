# Feature: Large Old File Classification

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make very large stale files easier to find during Storage Scan review without treating them as automatically safe to clean.

## Non-goals

- Do not mark every large file as bloat.
- Do not change directory classification based on recursive size.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not turn size alone into cleanup approval.

## User story / job story

As the project owner, I want old multi-gigabyte files to be labeled for review, so that I can quickly inspect likely space hogs that would otherwise stay under `No category`.

## Desired behavior

- Files at least 1 GB and older than 90 days get the `Large old file` category.
- Unknown large old files stay `Caution` with `Inspect`.
- Large old files that also have stronger cleanup evidence, such as old Downloads or installer-cache evidence, can keep the existing likely-safe/quarantine-candidate recommendation.
- Folders are not labeled from recursive size because parent and child rows can overlap.
- All behavior remains read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Bloat Category | Added `Large old file` as conservative triage evidence. | yes |

## Decisions made

- Use a 1 GB threshold so the category focuses on meaningful space recovery candidates.
- Require stale last-modified evidence so a newly created large file is not treated as bloat.
- Keep unknown large old files at `Caution`/`Inspect` because size and age alone are not enough to approve cleanup.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `BloatCategory.LargeOldFile`.
- Passed file size into the classifier.
- Added conservative classification logic and classifier test coverage for large old files.
- Added display/export labels and domain docs.

ADRs added or skipped:

- No ADR. This is reversible classifier triage behavior and does not add persistence, execution, or deployment changes.

Follow-up work:

- Rerun the real scan and inspect whether large old files reduce useful `No category` rows.
