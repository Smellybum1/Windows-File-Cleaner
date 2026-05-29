# Feature: Selected Row Action Automation Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the existing selected-row action safety boundaries available through WPF automation help text, not only hover tooltips.

## Non-goals

- Do not change selected-row action behavior.
- Do not change Review Shortlist behavior.
- Do not change Selected Folder Child Focus or Selected Folder Descendant Focus behavior.
- Do not change Selected File Content Preview behavior.
- Do not change File Explorer or clipboard behavior.
- Do not add visible helper text or layout changes.
- Do not move, restore, delete, create, or modify scanned files.
- Do not enable real-profile Quarantine execution or real-profile restore.

## User story / job story

As the project owner, I want selected-row shortlist, focus, preview, copy, and Explorer boundaries to be available to keyboard and assistive-technology paths, so that selected-row controls are not hover-only safety guidance.

## Current behavior

`Add to shortlist`, `Remove`, `Copy path`, `Show children`, `Show descendants`, `Preview file`, and `Open in Explorer` have disabled-state tooltips with the right safety boundaries. Those boundaries were not explicitly exposed through `AutomationProperties.HelpText`.

## Desired behavior

- `Add to shortlist` and `Remove` automation help text explains selected-row scope, review context, not-cleanup-approval wording, and no-file-modified behavior.
- `Copy path` automation help text explains manual inspection only and no-file-modified/no-approval behavior.
- `Show children` automation help text explains read-only `parent:` focus, no rescan, no file modification, and no cleanup approval.
- `Show descendants` automation help text explains read-only `under:` focus, no rescan, no file modification, and no cleanup approval.
- `Preview file` automation help text explains bounded text preview, Credential Data guardrails, and no file modification.
- `Open in Explorer` automation help text explains manual inspection only, not cleanup approval, and no app-side file modification.
- WPF smoke tests prove the automation help text carries the same safety boundaries as the tooltips.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Review Shortlist | Clarify that selected-row add/remove boundaries should be available through tooltip and automation help text. | yes |
| Selected Folder Child Focus | Clarify that `parent:` focus boundaries should be available through tooltip and automation help text. | yes |
| Selected Folder Descendant Focus | Clarify that `under:` focus boundaries should be available through tooltip and automation help text. | yes |
| Selected Path Inspection | Clarify that copy/Explorer boundaries should be available through tooltip and automation help text. | yes |
| Selected File Content Preview | Clarify that bounded preview boundaries should be available through tooltip and automation help text. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This reuses existing approved tooltip wording and does not change behavior.

Questions that can be deferred:

- Should a later accessibility pass add automation help text to browse, execution-gate, and restore-readiness controls?

## Grill notes

### Scenarios discussed

- User has prioritized manual review polish and Review Shortlist safety context before any real cleanup execution.
- Recent automation-help-text packets proved the non-hover pattern on main review toolbar and report/preview controls.

### Edge cases

- Disabled controls still need help text because selected-row actions are disabled until a row of the right type is selected.

### Dependencies between decisions

- Help text must mirror existing tooltip semantics instead of creating a second safety-copy source.

## Evidence and validation gate

Evidence gathered:

- `docs/features/2026-05-29-selected-row-action-tooltip-clarity.md` already defines the approved tooltip boundaries for this control group.
- README and domain docs already define these selected-row actions as review-only, focus-only, inspection-only, or bounded read-only preview.
- WPF smoke tests already cover startup tooltips for the same controls.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: WPF metadata and tests only, no filesystem mutation.
- [x] The narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add always-visible helper text in this packet.
- Do not broaden the packet to browse, execution, or restore controls.
- Do not change keyboard navigation, focus order, control enablement, selected-row actions, clipboard behavior, Explorer behavior, or file-preview behavior.

## Decisions made

Small feature-level decisions:

- Mirror existing tooltip safety wording into `AutomationProperties.HelpText` for the seven selected-row action controls.
- Add direct WPF smoke assertions for the automation help text boundaries.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF automation help text to selected-row action buttons.
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
- `docs/features/2026-05-29-selected-row-action-automation-help-text.md`

Possible:

- None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During fixture review, confirm visible selected-row action wording still fits; automation help text is covered by smoke tests.

## Risks and assumptions

Risks:

- Help text can drift from tooltip wording if future packets update only one surface.

Assumptions:

- Reusing the exact tooltip wording is the least surprising way to expose the same safety boundary to automation paths.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added WPF automation help text to `Add to shortlist`, `Remove`, `Copy path`, `Show children`, `Show descendants`, `Preview file`, and `Open in Explorer`.
- Added test-facing accessors and WPF smoke assertions for selected-only, read-only focus, bounded preview, manual inspection, no-file-modified, and not-cleanup-approval help text.
- Updated README, domain docs, progress, and handoff.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-row-action-automation-help-text.md`
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

- Consider browse, execution-gate, and restore-readiness automation help text in later focused packets if this pattern continues to prove useful.

Open questions:

- Should a later accessibility pass cover all safety-critical disabled controls with automation help text?

Risky assumptions:

- Reusing tooltip wording as automation help text is clear enough and avoids introducing a second safety-copy source.
