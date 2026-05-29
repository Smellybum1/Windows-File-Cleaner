# Feature: Review Grid Mode Status Help Text

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make Review Grid Mode Status expose the same read-only and no-cleanup boundary through tooltip and WPF automation help text that nearby review controls already expose.

## Non-goals

- Do not change Storage Scan rows, Current-Session Quarantined Review rows, Quarantine Preview, fixture execution, undo, selected restore, or manifest behavior.
- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not add permanent deletion or persisted cleanup history.
- Do not add another visible row, badge, modal, or help icon.

## Desired behavior

- Review Grid Mode Status tooltip mirrors the current status text.
- Review Grid Mode Status automation help text mirrors the current status text.
- Both help surfaces say the grid-mode status is read-only review context and does not rescan, modify files, restore files, or approve cleanup.
- Dynamic help text updates when the main grid switches between Storage Scan rows and Current-Session Quarantined Review rows, after fixture execution, after returning to scan rows, and after undo.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Review Grid Mode Status | Clarified that tooltip/help text should mirror dynamic status text and keep read-only/no-rescan/no-restore/not-cleanup-approval boundaries visible. | yes |

## Decisions made

- Add tooltip and automation help text to the existing status line rather than another visible control.
- Keep the status line itself unchanged so the grid does not lose vertical space.

## Test plan

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Completion notes

Completed on: 2026-05-30

What changed:

- Added static startup tooltip and automation help text to `ReviewGridModeText`.
- Mirrored dynamic Review Grid Mode Status text into tooltip and automation help text whenever the status updates.
- Added WPF smoke coverage for startup, normal scan rows, stale scan rows after fixture execution, Current-Session Quarantined Review rows, returning to scan rows, and empty current-session view after undo.

Docs updated:

- README, domain context, glossary, fixture checklist, Review Grid Mode Status feature briefs, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible WPF help text with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm whether Review Grid Mode Status help text is discoverable enough without a visible help icon.

Risky assumptions:

- Mirrored tooltip/help text is enough for this safety boundary because the status line is already visible and styled.
