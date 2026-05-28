# Feature: Access Status Search

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make the explicit Access Status field searchable so readable rows and access issue rows can be narrowed without relying on the separate Access issues filter.

## Non-goals

- Do not retry unreadable paths.
- Do not request elevated permissions.
- Do not change scanner traversal or access handling.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, deletion, or manifest writing.

## User story / job story

As the project owner, I want searches such as `access:readable` and `access:access issue`, so that I can review readable rows, incomplete scan coverage, and access issue messages from the same search box used for path and category review.

## Desired behavior

- Broad Storage Review Search matches Access Status values.
- `access:readable` matches rows with `Readable` Access Status.
- `access:access issue` matches rows with `Access issue` Access Status.
- `issue:<error text>` and `access:<error text>` still match scanner access issue messages.
- WPF filter summaries and suggested export filenames preserve the typed search term.
- All behavior remains in-memory and read-only.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Search | Expanded to include Access Status. | yes |
| Access Status | Marked as searchable through broad search and `access:` / `issue:` prefixes. | yes |

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Included Access Status in broad search matching.
- Included Access Status in `access:` / `issue:` field-prefix matching.
- Added core coverage for `access:readable`, `access:access issue`, and access issue message search.
- Added WPF fixture coverage for `access:readable` search and searched export filename hints.

ADRs added or skipped:

- No ADR. This is reversible in-memory review behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Try `access:readable`, `access:access issue`, and `issue:<error text>` in the next manual fixture or real-profile scan.
