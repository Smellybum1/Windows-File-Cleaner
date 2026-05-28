# Feature: Quarantine Root Preview Selection

Date started: 2026-05-29  
Status: completed  
Owner: project-owner

## Goal

Let the user choose the Quarantine Preview destination root before generating read-only preview paths, while keeping Quarantine execution unimplemented.

## Non-goals

- Do not create the quarantine folder.
- Do not move, delete, rename, or modify scanned files.
- Do not write a Restore Manifest.
- Do not persist the Quarantine Root Selection as a setting.
- Do not treat a selected root or preview as cleanup approval.

## User story / job story

As the project owner, I want the preview to show destinations under the `D:` quarantine root I intend to use, so that future quarantine behavior is easier to review before any file-moving code exists.

## Desired behavior

- The WPF review toolbar shows a Quarantine root text box.
- The default value remains `D:\WindowsFileCleanerQuarantine`.
- Quarantine Preview destination paths use the typed root.
- Changing the root after preview clears stale Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft output.
- No folders are created and no scanned files are modified.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Root Selection | Added as the typed read-only preview destination root. | yes |
| Quarantine Preview | Clarified that it uses the current Quarantine Root Selection. | yes |

## Decisions made

- Keep the root as a typed WPF field rather than a persisted preference.
- Keep the default root on `D:` to match the user's earlier quarantine preference.
- Clear stale preview output when the root changes because existing destination paths and draft readiness would no longer match.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added a WPF Quarantine root text box with the existing `D:\WindowsFileCleanerQuarantine` default.
- Routed Quarantine Preview through the typed root.
- Added WPF smoke coverage for custom preview roots and stale-preview clearing.
- Updated the README, domain context, glossary, existing Quarantine Preview brief, and progress log.

ADRs added or skipped:

- No ADR. This is a reversible read-only preview UI refinement and does not add cleanup execution, persistence, deployment, or manifest writes.

Follow-up work:

- In the next manual fixture or real-profile pass, confirm the Quarantine root field remains readable in the toolbar and that preview destinations match the intended `D:` location.

Later note:

- Follow-up packet `2026-05-29-quarantine-root-safety-note.md` added a read-only safety note and fully qualified preview-root gate.
