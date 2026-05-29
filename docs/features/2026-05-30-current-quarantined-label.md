# Feature: Current Quarantined Label

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the current-session-only scope of the quarantined grid switch visible in the button label.

## Non-goals

- Do not merge discovered Restore Manifest entries into the current-session quarantined grid.
- Do not add cleanup history.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, or permanent deletion.
- Do not change fixture execution, fixture undo, selected restore, or manifest discovery behavior.

## User story / job story

As the project owner, I want the quarantined-row switch to say it is for current-session entries, so I do not mistake it for all quarantined history or discovered Restore Manifests.

## Desired behavior

- Rename the visible `Quarantined` button to `Current quarantined`.
- Keep the existing current-session read-only behavior unchanged.
- Keep older Restore Manifest review routed through `Discover manifests`.
- Keep `Back to scan rows` unchanged.

## Domain language changes

No new terms. This uses the existing Current-Session Quarantined Review term more explicitly in visible UI.

## Decisions made

- Use `Current quarantined` as the visible button label for the current build.
- Keep the internal WPF control name `ShowQuarantinedButton` because the behavior did not change and the domain term remains Current-Session Quarantined Review.
- Leave discovered manifests in the existing manifest discovery/readiness panes until a separate broader restore/history design exists.

## Test plan

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Completion notes

Completed on: 2026-05-30

What changed:

- Renamed the visible grid-switch button from `Quarantined` to `Current quarantined`.
- Updated Review Grid Mode Status wording to point to `Current quarantined` when current-session moved entries are available.
- Added WPF smoke coverage for the new visible label.
- Updated README, domain docs, feature docs, fixture checklist, handoff, and progress notes.
- Later packet `2026-05-30-current-quarantined-count-label.md` keeps the empty label compact and adds `Current quarantined (N)` when current-session moved entries are available.

ADRs added or skipped:

- Skipped. This is reversible UI wording for existing current-session-only behavior under ADR 0010, with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- A future broader design still needs to decide whether discovered Restore Manifest entries appear in a separate tab/grid or remain in manifest discovery/readiness panes.

Risky assumptions:

- `Current quarantined` is clearer than `Quarantined` without making the button too wide for the current toolbar.
