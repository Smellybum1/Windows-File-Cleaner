# Feature: Quarantine Preview Protected Descendant Blocker

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Prevent broad parent folders from appearing eligible in Quarantine Preview when their scanned subtree contains protected, high-risk, inaccessible, or reparse-point descendants.

## Non-goals

- Do not add Quarantine execution, Undo Quarantine execution, permanent deletion, or manifest writing.
- Do not change scanner traversal.
- Do not change child-row Bloat Categories, Importance Ratings, or Deletion Recommendations.
- Do not inspect real user files outside fixture-driven tests.

## User story / job story

As the project owner, I want a broad cache-looking parent such as `.cache` to be blocked if it contains protected Codex-related descendants, so that I do not accidentally preview moving active tooling data while trying to clean bloat.

## Desired behavior

- Quarantine Preview blocks a selected parent row when any descendant is protected, high-risk, inaccessible, or a reparse point.
- The blocked reason includes cleanup-scope-relative example descendant paths so the user can narrow review to safer child rows.
- Included preview bytes exclude blocked broad parent rows.
- Preview remains read-only and does not create the quarantine root.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Preview | Clarified that parent rows are blocked when their subtree contains blocked descendants. | yes |

## Decisions made

- Use the existing in-memory `StorageEntry.Children` tree instead of touching the filesystem again.
- Block at preview time rather than changing the parent row's scan classification, because the parent may still be useful as review context.
- Treat protected, high-risk, inaccessible, reparse-point, and Cleanup Scope Root descendants as blockers for broad parent preview.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after adding WPF preview coverage.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added descendant blocker checks to `QuarantinePreviewBuilder`.
- Added fixture coverage for `.cache` containing protected `codex-runtimes` data.
- Added WPF smoke coverage that shortlists the broad cache parent and confirms the preview pane shows the blocked descendant evidence.
- Later readability packet separated confirmation-readiness blockers from row-level preview details in the WPF pane.
- Later relative-blocker packet changed protected-descendant examples from absolute paths to cleanup-scope-relative paths.
- Updated README and domain docs to describe the broad-parent blocker.
- No files are moved, deleted, copied, restored, or written by preview generation.

ADRs added or skipped:

- No ADR. This is a conservative preview rule inside the existing read-only Quarantine Preview design and does not change persistence, cleanup execution, deployment, or public APIs.

Follow-up work:

- Retest Quarantine Preview against a real scan and confirm broad cache parents with protected descendants are easy to understand.
