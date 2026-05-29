# Feature: Visible Row Shortlist Automation Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Mirror Review Shortlist visible-row bulk action safety tooltips into WPF automation help text.

## Non-goals

- Do not change which rows are added or removed.
- Do not persist the Review Shortlist.
- Do not change Quarantine Preview eligibility.
- Do not move, restore, delete, create, or modify scanned files.
- Do not enable real-profile Quarantine execution.

## User story / job story

As the project owner, I want keyboard and assistive-technology surfaces to expose the same visible-row Review Shortlist safety boundaries as hover tooltips, so current-window bulk review does not look like cleanup approval.

## Current behavior

`Shortlist visible rows` and `Remove visible rows` have disabled-state tooltips that explain they apply only to currently visible review rows and do not modify files. They did not expose matching WPF automation help text.

## Desired behavior

- `Shortlist visible rows` has matching automation help text for visible-row scope, review-only context, not-cleanup-approval wording, and no-file-modified behavior.
- `Remove visible rows` has matching automation help text for visible-row scope, review-only context, not-cleanup-approval wording, and no-file-modified behavior.
- WPF smoke tests assert the help text.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Review Shortlist | Existing wording already requires visible-row control tooltips and automation help text. | already current |

## Open questions

Questions that must be answered before implementation:

- None. This is metadata parity for existing safety wording.

Questions that can be deferred:

- During visible fixture review, are hover tooltips and automation help text enough, or should visible-row bulk controls get always-visible help?

## Decisions made

Small feature-level decisions:

- Reuse existing tooltip wording as automation help text.
- Keep the packet scoped to the two visible-row bulk controls.

ADR-worthy decisions:

- [x] None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During visible fixture review, confirm the toolbar still fits and the visible-row scope wording remains clear.

## Risks and assumptions

Risks:

- Automation help text improves non-hover discoverability but does not make safety text always visible.

Assumptions:

- Reusing tooltip wording keeps keyboard and screen-reader context aligned without changing Review Shortlist behavior.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added automation help text to `Shortlist visible rows` and `Remove visible rows`.
- Added WPF smoke assertions for visible-row scope, not-cleanup-approval, and no-file-modified wording.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- README, progress log, handoff, and this feature brief.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible WPF metadata polish with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Confirm during fixture review whether the visible-row bulk controls need always-visible help.

Open questions:

- Should visible-row bulk controls eventually get always-visible help or is tooltip plus automation help text enough?

Risky assumptions:

- The existing tooltip wording remains the correct safety text for automation help text.
