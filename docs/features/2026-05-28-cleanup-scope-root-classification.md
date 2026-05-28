# Feature: Cleanup Scope Root Classification

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make the top row of a Storage Scan clearly read as the scanned scope itself, not as an ordinary cleanup candidate.

## Non-goals

- Do not change scanner traversal.
- Do not change child-row cache, AppData, browser, app, game, or download classification.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not make the Cleanup Scope Root a recoverable storage-savings estimate.

## User story / job story

As the project owner, I want the scan root, such as `C:\Users\moxhe`, to be marked as something to keep and inspect through children, so I do not mistake the whole scanned folder for bloat.

## Desired behavior

- The Storage Scan root row includes `Cleanup scope root` and `Protected location`.
- The root row is rated `High risk`.
- The root row recommendation is `Keep`.
- Selected-row guidance says to inspect children, not the whole scope root.
- Fixture and custom scope roots get the same root-specific protection as the real profile scope.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope Root | Added as the selected scan root row, shown as protected and not a cleanup target. | yes |

## Decisions made

- Pass root context from `StorageScanner` into classification instead of inferring it from path shape alone.
- Represent the root with `BloatCategory.CleanupScopeRoot` plus `Protected location`.
- Keep the root as `High risk` / `Keep` even when the selected scope is a fixture or custom folder.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `BloatCategory.CleanupScopeRoot`.
- Added explicit scan-root classification in `StorageScanner` / `CleanupCandidateClassifier`.
- Added WPF, CSV, and Quarantine Preview CSV category labels for `Cleanup scope root`.
- Added Selected Path Review Guidance for root rows.
- Added core and WPF fixture coverage.

ADRs added or skipped:

- No ADR. This is a conservative scan classification and review-guidance change; it does not change persistence, cleanup execution, security boundaries, deployment, or public APIs.

Follow-up work:

- Check the next real scan to confirm the first row reads as a keep/protected root instead of an ambiguous cleanup candidate.
