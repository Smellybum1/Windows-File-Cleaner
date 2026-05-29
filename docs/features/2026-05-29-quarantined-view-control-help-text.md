# Feature: Quarantined View Control Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the `Current quarantined` and `Back to scan rows` controls explain their current-session scope and disabled states while the main grid can switch between scan rows and quarantined rows.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not add permanent deletion.
- Do not add persisted cleanup history.
- Do not merge discovered Restore Manifests into the current-session quarantined grid.

## User story / job story

As the project owner, I want the quarantined-view buttons to explain why they are enabled or disabled, so I can understand whether current-session quarantined rows are available without guessing from button state alone.

## Desired behavior

- Before fixture execution, disabled `Current quarantined` help text explains that current-session rows appear only after fixture Quarantine execution records moved Restore Manifest entries.
- When moved current-session entries exist, enabled `Current quarantined` help text summarizes the count and read-only/no-restore/no-history boundary.
- While already showing quarantined rows, disabled `Current quarantined` help text explains that the view is already active and points to `Back to scan rows`.
- `Back to scan rows` help text explains that returning does not rescan, modify files, or run undo.
- After undo clears moved entries, disabled help text explains that no moved entries are available and points back to scan rows when needed.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Current-Session Quarantined Review | Clarified button help text expectations for current-session scope and disabled states. | yes |

## Open questions

- Should future discovered-manifest review get a separate grid switch or stay in the existing manifest panes?

## Decisions made

Small feature-level decisions:

- Keep dynamic help text on the existing buttons instead of adding a new visible hint.
- Route older Restore Manifest review to `Discover manifests` in disabled `Current quarantined` help text.

ADR-worthy decisions:

- [x] None expected. This is reversible WPF help text with no persistence, file movement rule, restore rule, data-model, or security change.

## Implementation plan

1. Update quarantined-view control state to set dynamic tooltips and automation help text.
2. Cover before-execution, after-execution, active-view, return-to-scan, and post-undo states in WPF smoke tests.
3. Update README, domain docs, progress, and handoff.

## Test plan

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- Button help text can become verbose, especially because it carries safety boundaries.

Assumptions:

- Dynamic disabled-state help is preferable to another always-visible line because Review Grid Mode Status already labels the active grid.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added dynamic tooltip and automation help text for `Quarantined`; later wording polish changed the visible label to `Current quarantined`.
- Added dynamic tooltip and automation help text for `Back to scan rows`.
- Help text now distinguishes before execution, active quarantined view, available current-session entries, and empty post-undo state.
- Older Restore Manifest review remains routed to `Discover manifests`; current-session quarantined view remains read-only.
- Later label polish made the visible control name carry current-session scope before the tooltip is opened.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantined-view-control-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, this feature brief, progress log, and handoff.

ADRs added or skipped:

- Skipped. This is reversible WPF help text.

Follow-up work:

- Manual fixture visual pass to confirm the help text and grid-mode text together are understandable without visual clutter.

Open questions:

- Should discovered manifests eventually get a separate grid view, or remain in discovery/readiness panes?

Risky assumptions:

- Button help text is enough for this step without adding another visible callout.
