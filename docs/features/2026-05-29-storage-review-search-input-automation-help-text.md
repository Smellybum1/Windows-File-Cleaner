# Feature: Storage Review Search Input Automation Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Mirror the Storage Review Search input tooltip examples and safety boundary into WPF automation help text.

## Non-goals

- Do not change Storage Review Search matching.
- Do not change search debounce timing or pending-search status wording.
- Do not add another visible pending-search indicator.
- Do not rescan the filesystem from search.
- Do not modify, move, delete, quarantine, or restore scanned files.
- Do not treat search text as cleanup approval.

## User story / job story

As the project owner, I want the Storage Review Search input to expose prefix examples and read-only boundaries through non-hover help text, so search remains discoverable without implying cleanup approval or file changes.

## Current behavior

The Storage Review Search input has a tooltip with examples such as `path:`, `parent:`, `under:`, `category:`, `access:readable`, and `issue:denied`. Clear search already has tooltip and automation help text, but the search input itself did not expose matching automation help text.

## Desired behavior

- The Storage Review Search input exposes WPF automation help text with the same prefix examples as the tooltip.
- The help text says search is read-only and does not rescan, modify files, or approve cleanup.
- WPF smoke tests assert the prefix examples and read-only boundary.
- Search matching, debounce behavior, status-bar pending-search wording, filters, cleanup execution, restore, deletion, and cleanup history stay unchanged.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Search | Clarified that the search input should expose prefix examples and read-only/no-rescan/no-cleanup-approval boundaries through tooltip and automation help text. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is metadata parity for existing Storage Review Search examples and safety wording.

Questions that can be deferred:

- During visible fixture or real-profile review, does the search input need an always-visible help affordance, or are tooltip plus automation help text enough?

## Decisions made

Small feature-level decisions:

- Add concise automation help text directly to `SearchBox`.
- Keep the status-bar pending-search indicator unchanged because the user already confirmed it is enough.

ADR-worthy decisions:

- [x] None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During fixture or real-profile review, confirm the search tooltip/help wording remains discoverable and the status-bar pending-search message is still sufficient.

## Risks and assumptions

Risks:

- Automation help text improves non-hover discoverability but does not make examples always visible.

Assumptions:

- Matching the existing tooltip examples is enough for search input help text in this packet.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added automation help text to `SearchBox`.
- Added WPF smoke assertions for prefix examples plus read-only/no-rescan/no-cleanup-approval wording.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- README, domain docs, progress log, handoff, and this feature brief.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible WPF metadata/test polish with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Confirm during visible fixture/real-profile review whether the search input needs always-visible help.

Open questions:

- Does visible review show tooltip plus automation help text is enough for the search input?

Risky assumptions:

- The search input can remain compact while carrying detailed examples in tooltip and automation help text.
