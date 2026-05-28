# Feature: Selected Path Hierarchy Context

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make deeply nested Storage Scan rows easier to understand during manual review.

## Non-goals

- Do not change scanning, classification, or cleanup eligibility.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not treat hierarchy depth, relative path, or parent path as deletion approval.

## User story / job story

As the project owner, I want one-letter cache folders and hashed files to show where they live, so that I can decide what to inspect without guessing from the row name alone.

## Desired behavior

- Show each row's parent path in the Storage Scan grid.
- Later packet: also show each row's cleanup-scope-relative path in the Storage Scan grid.
- Show selected-row relative path, parent path, hierarchy depth, and modified time in the detail pane.
- Keep this context read-only and derived from the completed scan result.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Path Hierarchy Context | Added as read-only relative-path, parent/depth context for a selected or displayed scan row. | yes |

## Decisions made

- Use the full parent path in the grid and detail pane, because real scan rows can have short names such as `e`, `c`, or hash fragments.
- Later relative-path packet: also show the cleanup-scope-relative path, because repeated `C:\Users\moxhe` prefixes and long parent paths can hide the meaningful path segment during review.
- Keep the existing CSV parent/depth context as-is.
- Skip an ADR because this is reversible UI review context and not an architecture or persistence decision.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added a `Parent` column to the Storage Scan grid.
- Later relative-path packet added a `Relative path` column to the Storage Scan grid.
- Added selected-row relative path, parent path, hierarchy depth, and modified-time context to the detail pane.
- Added WPF smoke coverage for parent/depth detail context.

ADRs added or skipped:

- No ADR. This is read-only UI context and does not change cleanup behavior, persistence, build tooling, deployment, or cross-module data flow.

Follow-up work:

- Check the next real scan to confirm deep cache rows are easier to interpret.
