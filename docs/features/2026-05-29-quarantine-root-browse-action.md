# Feature: Quarantine Root Browse Action

Date started: 2026-05-29  
Status: completed  
Owner: project-owner

## Goal

Let the user choose the Quarantine Root Selection with a folder browser as well as by typing, while keeping Quarantine Preview read-only.

## Non-goals

- Do not create the quarantine folder.
- Do not move, delete, rename, or modify scanned files.
- Do not write a Restore Manifest.
- Do not persist the Quarantine Root Selection as a setting.
- Do not treat browsing to a folder as cleanup approval.

## User story / job story

As the project owner, I want to browse for the preview quarantine root, so that destination paths can be reviewed under the intended `D:` location without hand-typing a long path.

## Desired behavior

- The WPF review toolbar shows a `Browse...` action next to the Quarantine root field.
- The browse action opens a folder picker for Quarantine Root Selection.
- Choosing a folder updates the preview root and preserves read-only status wording.
- No folders are created and no scanned files are modified.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Root Selection | Clarified that it can be typed or browsed. | yes |

## Decisions made

- Reuse the native folder picker already used by Cleanup Scope Selection.
- Keep browsing as a preview setting only, not a persisted preference or cleanup approval.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added `BrowseQuarantineRootButton` beside the Quarantine root text box.
- Added a WPF browse handler that updates Quarantine Root Selection only.
- Added WPF smoke coverage that the browse action is present and enabled before scanning.
- Updated README, domain context, glossary, and progress log.

ADRs added or skipped:

- No ADR. This is a reversible read-only UI refinement and does not add cleanup execution, persistence, deployment, or manifest writes.

Follow-up work:

- In the next manual fixture pass, browse to a temporary `D:` folder and confirm the preview destinations use that root without creating folders.
