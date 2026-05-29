# Feature: Current Quarantined Count Label

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the current-session quarantined grid switch show the number of moved entries available after fixture Quarantine execution.

## Non-goals

- Do not merge discovered Restore Manifest entries into the current-session quarantined grid.
- Do not add cleanup history.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, or permanent deletion.
- Do not change fixture execution, fixture undo, selected restore, manifest discovery, or restore behavior.

## User story / job story

As the project owner, I want `Current quarantined` to show a count when moved current-session entries exist, so I can tell at a glance whether the button will switch to something useful.

## Current behavior

The visible button always says `Current quarantined`; the moved-entry count is available only in tooltip/help text and Review Grid Mode Status.

## Desired behavior

- Before any current-session moved entries exist, keep the compact label `Current quarantined`.
- When current-session moved entries exist, show `Current quarantined (N)`.
- Keep tooltip/help text as the detailed explanation of current-session-only, read-only, no-restore, no-history boundaries.

## Domain language changes

No new terms. This refines visible wording for the existing Current-Session Quarantined Review term.

## Decisions made

- Add the count only when at least one current-session moved entry exists.
- Keep `Current quarantined` unchanged for empty and disabled states so the toolbar stays compact before fixture execution.
- Keep older/discovered manifests in `Discover manifests` and Restore Manifest readiness panes.

## Test plan

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Completion notes

Completed on: 2026-05-30

What changed:

- `Current quarantined` now becomes `Current quarantined (N)` when current-session moved Restore Manifest entries are available.
- WPF smoke coverage now asserts the empty label remains compact before execution and the post-execution label shows the moved-entry count.
- Manual fixture checklist wording now prompts the count-label check.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-30-current-quarantined-count-label.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the moved-entry count prompt without preflight, fixture creation, or WPF launch.

Docs updated:

- README, domain context, glossary, feature brief, progress log, and handoff.

ADRs added or skipped:

- Skipped. This is reversible UI wording and test coverage for existing current-session-only behavior, with no persistence, cleanup execution, restore rule, data-model, or security change.

Follow-up work:

- Manual fixture review should confirm the count label is useful without crowding the Quarantine Shortlist toolbar.

Open questions:

- Later ADR 0016 decided that older/discovered Restore Manifest review stays in manifest discovery/readiness panes for now, and `Current quarantined` remains current-session-only rather than all quarantined history.

Risky assumptions:

- Adding a count only when moved entries exist gives enough visibility without making the toolbar feel busier before fixture execution.
