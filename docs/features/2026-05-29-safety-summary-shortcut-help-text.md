# Feature: Safety Summary Shortcut Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add tooltip and WPF automation help text to Storage Scan Safety Summary review shortcuts.

## Non-goals

- Do not add new Safety Summary shortcuts.
- Do not change shortcut-to-filter mapping.
- Do not change Storage Scan Safety Summary counts or examples.
- Do not rescan the filesystem from shortcuts.
- Do not retry access issue paths, request elevation, or change permissions.
- Do not follow reparse points.
- Do not modify, move, delete, quarantine, or restore scanned files.
- Do not treat shortcuts as cleanup approval, Quarantine approval, or a safety guarantee.

## User story / job story

As the project owner, I want Safety Summary shortcut controls to explain their read-only scope even while disabled, so high-risk, protected, access, reparse, candidate, and no-category review shortcuts cannot be mistaken for permission fixes, rescans, link following, or cleanup approval.

## Current behavior

Safety Summary shortcuts apply existing read-only review/category filters, but the buttons did not expose tooltip or automation help text. Neighboring review filters, search, shortlist, preview, and execution/readiness controls already expose those boundaries.

## Desired behavior

- Each Safety Summary shortcut exposes tooltip and WPF automation help text.
- Tooltips remain available while shortcuts are disabled.
- Help text keeps shortcuts framed as read-only review lenses over completed Storage Scan rows.
- Access issue wording says shortcuts do not retry, elevate, change permissions, modify files, or approve cleanup.
- Reparse point wording says shortcuts do not follow links.
- Quarantine candidate wording says shortcuts are not Quarantine approval.
- WPF smoke tests assert the new boundary wording.
- Shortcut behavior, filter behavior, scan behavior, exports, cleanup execution, restore, deletion, and cleanup history stay unchanged.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Storage Scan Safety Summary | Clarified that review shortcut tooltips and automation help text should keep read-only/no-rescan/no-file-modified/no-permission-change/no-link-following/not-cleanup-approval boundaries available. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is metadata parity for existing read-only shortcut behavior.

Questions that can be deferred:

- During visible fixture or real-profile review, do Safety Summary shortcuts need always-visible help icons or stronger grouping?

## Decisions made

Small feature-level decisions:

- Add concise tooltip and automation help text directly to each Safety Summary shortcut button.
- Enable disabled-state tooltip display because shortcuts are disabled before scans and for zero-count buckets.
- Keep shortcut labels, enablement, mapping, and behavior unchanged.

ADR-worthy decisions:

- [x] None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During fixture or real-profile review, confirm Safety Summary shortcut tooltips remain discoverable and do not crowd the summary area.

## Risks and assumptions

Risks:

- Tooltip and automation help text improve discoverability but do not make shortcut boundaries always visible.

Assumptions:

- Matching the existing tooltip/help-text pattern is enough before adding any visible help affordance.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added disabled-state tooltip and automation help text to Safety Summary shortcut buttons.
- Added WPF smoke assertions for read-only shortcut scope, no-rescan, no-file-modified, no-permission-change, no-link-following, and not-cleanup/Quarantine-approval boundaries.

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

- Confirm during visible fixture/real-profile review whether shortcut tooltip/help text is enough or always-visible help would reduce ambiguity.

Open questions:

- Do Safety Summary shortcuts need always-visible help icons or stronger grouping after manual review?

Risky assumptions:

- The Safety Summary shortcut row remains comfortable with tooltip/help text added to every shortcut.
