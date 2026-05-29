# Feature: Review Report and Preview Automation Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the existing Review Shortlist report/clear and Quarantine Preview/export safety boundaries available through WPF automation help text, not only hover tooltips.

## Non-goals

- Do not change Review Shortlist behavior.
- Do not change Quarantine Preview eligibility or preview output.
- Do not change CSV export behavior.
- Do not add visible helper text or layout changes.
- Do not move, restore, delete, create, or modify scanned files.
- Do not enable real-profile Quarantine execution or real-profile restore.

## User story / job story

As the project owner, I want report-only, in-memory-only, and dry-run toolbar boundaries to be available to keyboard and assistive-technology paths, so that the Review Shortlist and Quarantine Preview controls are not hover-only safety guidance.

## Current behavior

`Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview` have disabled-state tooltips with the right safety boundaries. Those boundaries were not explicitly exposed through `AutomationProperties.HelpText`.

## Desired behavior

- `Export shortlist` automation help text says it exports the current Review Shortlist as a CSV report only, is not cleanup approval, and does not modify scanned files.
- `Clear shortlist` automation help text says it clears only the in-memory Review Shortlist and does not delete, move, quarantine, or restore files.
- `Preview quarantine` automation help text says it builds a read-only Quarantine Preview and does not create folders, move files, or approve cleanup.
- `Export preview` automation help text says it exports a CSV report only and does not execute Quarantine or modify scanned files.
- WPF smoke tests prove the automation help text carries the same safety boundaries as the tooltips.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Review Shortlist | Clarify that export/clear boundaries should be available through tooltip and automation help text. | yes |
| Quarantine Preview | Clarify that preview/export boundaries should be available through tooltip and automation help text. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This reuses existing approved tooltip wording and does not change behavior.

Questions that can be deferred:

- Should a later accessibility pass add automation help text to selected-row, browse, execution-gate, and restore-readiness controls?

## Grill notes

### Scenarios discussed

- User has prioritized manual review polish, Quarantine Preview/readiness clarity, and Review Shortlist safety context before any real cleanup execution.
- The previous automation-help-text packet proved the pattern on the main review navigation/export controls.

### Edge cases

- Disabled controls still need help text because `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview` are often disabled until scan/shortlist/preview state exists.

### Dependencies between decisions

- Help text must mirror existing tooltip semantics instead of creating a second safety-copy source.

## Evidence and validation gate

Evidence gathered:

- `docs/features/2026-05-29-review-toolbar-report-preview-tooltip-clarity.md` already defines the approved tooltip boundaries for this control group.
- README and domain docs already define Review Shortlist as in-memory review context and Quarantine Preview as a dry run.
- WPF smoke tests already cover startup tooltips for the same controls.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: WPF metadata and tests only, no filesystem mutation.
- [x] The narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add always-visible helper text in this packet.
- Do not broaden the packet to selected-row, browse, execution, or restore controls.
- Do not change keyboard navigation, focus order, control enablement, CSV export behavior, or preview eligibility.

## Decisions made

Small feature-level decisions:

- Mirror existing tooltip safety wording into `AutomationProperties.HelpText` for the four Review Shortlist report/clear and Quarantine Preview/export controls.
- Add direct WPF smoke assertions for the automation help text boundaries.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF automation help text to `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview`.
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
- `docs/features/2026-05-29-review-report-preview-automation-help-text.md`

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

- Added WPF automation help text to `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview`.
- Added test-facing accessors and WPF smoke assertions for report-only, in-memory-only, dry-run, no-file-modified, and not-cleanup-approval help text.
- Updated README, domain docs, progress, and handoff.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-report-preview-automation-help-text.md`
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

- Consider selected-row, browse, execution-gate, and restore-readiness automation help text in later focused packets if this pattern continues to prove useful.

Open questions:

- Should a later accessibility pass cover all safety-critical disabled controls with automation help text?

Risky assumptions:

- Reusing tooltip wording as automation help text is clear enough and avoids introducing a second safety-copy source.
