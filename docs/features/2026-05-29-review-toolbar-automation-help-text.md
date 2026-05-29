# Feature: Review Toolbar Automation Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the existing review navigation and Scan Report Export safety boundaries available through WPF automation help text, not only hover tooltips.

## Non-goals

- Do not change Storage Review Search behavior.
- Do not change Review View Reset behavior.
- Do not change Storage Review Display Window behavior.
- Do not change Scan Report Export behavior.
- Do not add visible helper text or layout changes.
- Do not move, restore, delete, create, or modify scanned files.
- Do not enable real-profile Quarantine execution or real-profile restore.

## User story / job story

As the project owner, I want the primary read-only review toolbar boundaries to be available to keyboard and assistive-technology paths, so that report-only, no-rescan, in-memory, and no-file-modified semantics are not hover-only.

## Current behavior

`Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows` have disabled-state tooltips with the right safety boundaries. Those boundaries are not explicitly exposed through `AutomationProperties.HelpText`.

## Desired behavior

- `Export CSV` automation help text says it exports a CSV report, is not cleanup approval, and does not modify scanned files.
- `Clear search` automation help text says it clears only Storage Review Search, does not rescan or modify files, and keeps Review Shortlist.
- `Reset view` automation help text says it clears filters/search, keeps Review Shortlist, and does not rescan or modify files.
- `Previous rows` and `Next rows` automation help text say they move through the in-memory Storage Review Display Window and do not rescan or modify files.
- WPF smoke tests prove the automation help text carries the same safety boundaries as the tooltips.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Scan Report Export | Clarify that report-only boundaries should be available through tooltip and automation help text. | yes |
| Storage Review Search | Clarify that clear-search boundaries should be available through tooltip and automation help text. | yes |
| Review View Reset | Clarify that reset boundaries should be available through tooltip and automation help text. | yes |
| Storage Review Display Window | Clarify that Previous/Next rows boundaries should be available through tooltip and automation help text. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This reuses existing approved tooltip wording and does not change behavior.

Questions that can be deferred:

- Should the remaining safety-critical tooltips also receive automation help text in later focused packets?

## Grill notes

### Scenarios discussed

- User has prioritized manual review polish and safety context before any real cleanup execution.
- The previous packet left a follow-up question about non-hover discoverability for review navigation/export boundaries.

### Edge cases

- Disabled controls still need help text because many safety-critical controls are disabled before scan state changes.

### Dependencies between decisions

- Help text must mirror the existing tooltip semantics instead of introducing a new explanation path.

## Evidence and validation gate

Evidence gathered:

- Recent packets clarified hover tooltips for review navigation/export controls.
- README and domain docs already define these controls as read-only, in-memory, or report-only.
- WPF smoke tests already cover startup tooltips for the same controls.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: WPF metadata and tests only, no filesystem mutation.
- [x] The narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add always-visible helper text in this packet.
- Do not broaden the packet to every tooltiped control.
- Do not change keyboard navigation, focus order, or control enablement.

## Decisions made

Small feature-level decisions:

- Mirror existing tooltip safety wording into `AutomationProperties.HelpText` for the five review navigation/export controls.
- Add direct WPF smoke assertions for the automation help text boundaries.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF automation help text to `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows`.
2. Add test-facing help-text accessors and WPF smoke assertions.
3. Update README, domain docs, progress, and handoff.
4. Run WPF smoke tests, build, and diff check.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-review-toolbar-automation-help-text.md`

Possible:

- None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During fixture review, confirm visible review toolbar wording still fits; automation help text is covered by smoke tests.

## Risks and assumptions

Risks:

- Help text can drift from tooltip wording if future packets update only one surface.

Assumptions:

- Reusing the exact tooltip wording is the least surprising way to expose the same safety boundary to automation paths.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added WPF automation help text to `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows`.
- Added test-facing accessors and WPF smoke assertions for report-only, no-rescan, in-memory display-window, Review Shortlist-preserving, no-file-modified, and not-cleanup-approval help text.
- Updated README, domain docs, progress, and handoff.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-toolbar-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible WPF metadata/test polish with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Consider adding automation help text to other safety-critical tooltiped controls in later focused packets if this pattern proves useful.

Open questions:

- Should a later accessibility pass cover all safety-critical disabled controls with automation help text?

Risky assumptions:

- Reusing the tooltip wording as automation help text is clear enough and avoids introducing a second safety-copy surface.
