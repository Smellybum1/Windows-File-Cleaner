# Feature: Review Lens Filter Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add tooltip and WPF automation help text to the main Storage Scan review lens filters.

## Non-goals

- Do not change Storage Review Filter behavior.
- Do not change Bloat Category Filter behavior.
- Do not change Storage Entry Type Filter or Storage Size Threshold Filter behavior.
- Do not rescan the filesystem from filters.
- Do not retry access issue paths, request elevation, or change permissions.
- Do not modify, move, delete, quarantine, or restore scanned files.
- Do not treat filters as cleanup approval, Quarantine approval, or Storage Savings proof.

## User story / job story

As the project owner, I want the review lens filter controls to explain that they narrow completed scan rows only, so manual fixture and real-profile review stays clearly separate from rescans, permission changes, file modification, and cleanup approval.

## Current behavior

Adjacent review controls such as Search, Clear search, Reset view, row-window navigation, CSV export, Review Shortlist, preview, and execution/readiness controls have tooltip and automation help text. The top Storage Review Filter buttons plus Type, Size, and Category filters did not expose the same boundary text.

## Desired behavior

- Storage Review Filter buttons expose tooltip and automation help text.
- Type, Size, and Category filter combo boxes expose tooltip and automation help text.
- Help text keeps filters framed as read-only review lenses over completed Storage Scan rows.
- Access issues wording says filtering does not retry, elevate, change permissions, modify files, or approve cleanup.
- Size wording says row size is a triage clue, not Storage Savings or cleanup approval.
- WPF smoke tests assert the new boundary text.
- Filtering behavior, export behavior, search debounce, cleanup execution, restore, deletion, and cleanup history stay unchanged.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Filter | Clarified tooltip/help text boundaries for review-only, no-rescan, no-file-modified, no-permission-change, and not-cleanup-approval behavior. | yes |
| Storage Entry Type Filter | Clarified tooltip/help text boundaries for read-only file/folder filtering. | yes |
| Storage Size Threshold Filter | Clarified tooltip/help text boundaries that size is triage context, not Storage Savings or cleanup approval. | yes |
| Bloat Category Filter | Clarified tooltip/help text boundaries for read-only category filtering. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is metadata parity for existing review lens behavior.

Questions that can be deferred:

- During visible fixture or real-profile review, do the filter tooltips make the toolbar feel too dense, or are they enough without always-visible help icons?

## Decisions made

Small feature-level decisions:

- Add concise tooltip and automation help text directly to each filter control.
- Keep the packet scoped to metadata and WPF smoke assertions.
- Keep filter labels, enablement, and behavior unchanged.

ADR-worthy decisions:

- [x] None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During fixture or real-profile review, confirm review lens filter tooltips are discoverable and do not crowd visible layout.

## Risks and assumptions

Risks:

- Tooltip and automation help text improve discoverability but do not make filter boundaries always visible.

Assumptions:

- Matching the existing tooltip/help-text pattern is enough for review lens filters before adding any visible help affordance.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added tooltip and automation help text to Storage Review Filter buttons.
- Added tooltip and automation help text to Type, Size, and Category filter combo boxes.
- Added WPF smoke assertions for review-only, no-rescan, no-file-modified, no-permission-change, Storage Savings, and not-cleanup-approval boundaries.

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

- Confirm during visible fixture/real-profile review whether filter tooltip/help text is enough or always-visible help would reduce ambiguity.

Open questions:

- Do the filter controls need always-visible help icons after manual review?

Risky assumptions:

- The toolbar remains comfortable with tooltip/help text added to filter controls.
