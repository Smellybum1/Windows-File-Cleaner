# Feature: Storage Review Size Note

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make the Storage Scan review surface clearer about recursive folder sizes so the user does not mistake overlapping row sizes for confirmed cleanup savings.

## Non-goals

- Do not change scanner traversal.
- Do not change row ordering, filters, categories, exports, shortlist, or Quarantine Preview behavior.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.

## User story / job story

As the project owner, I want the app to remind me that parent and child rows can overlap, so that I use size values for triage instead of adding them as expected savings.

## Desired behavior

- Show a visible note near the Storage Scan review controls.
- Say that folder sizes include child files.
- Say that parent and child rows can overlap.
- Say that row sizes are triage clues, not Storage Savings.
- Keep the note read-only and fixture-testable.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Size Note | Added as visible wording for recursive size semantics. | yes |

## Decisions made

- Place the note under the filter summary so it is visible while the user filters, searches, shortlists, and previews rows.
- Keep the wording separate from Review Mix so it applies to the whole grid, not just summary counts.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `ReviewSizeNoteText` to the WPF review surface.
- Added a WPF smoke assertion that the note explains overlap and avoids treating row sizes as savings.
- Updated README review steps and domain docs.

ADRs added or skipped:

- No ADR. This is reversible UI copy and smoke coverage.

Follow-up work:

- Confirm the note is visually readable in the next manual fixture UI pass.
