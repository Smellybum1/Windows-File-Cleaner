# Feature: Review View Reset

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make it easy to recover from stacked review filters while preserving the user's Review Shortlist.

## Non-goals

- Do not clear Review Shortlist.
- Do not clear or execute Quarantine Preview.
- Do not rescan the filesystem.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.

## User story / job story

As the project owner, I want one action that clears active filters and search, so that I can quickly return to the full review after narrowing the real scan.

## Desired behavior

- Add `Reset view` to the review toolbar.
- Enable it only when a review lens is active.
- Reset Storage Review Filter to `All`.
- Reset Bloat Category Filter to `All categories`.
- Reset Storage Entry Type Filter to `All types`.
- Reset Storage Size Threshold Filter to `All sizes`.
- Clear Storage Review Search.
- Keep Review Shortlist entries.
- Report that no files were modified.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Review View Reset | Added as read-only reset action for the active review lens. | yes |

## Decisions made

- Keep reset separate from `Clear shortlist`; the shortlist is deliberate review state.
- Disable reset when the review is already at the default lens.
- Do not clear Quarantine Preview from reset because reset does not change shortlisted rows.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `Reset view` button.
- Added `ResetReviewView` WPF command.
- Added reset-enabled state handling.
- Added WPF smoke coverage that reset clears filters/search, including Storage Size Threshold Filter, and keeps Review Shortlist.

ADRs added or skipped:

- No ADR. This is reversible read-only UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Try Reset view during the next manual fixture and real scan review.
