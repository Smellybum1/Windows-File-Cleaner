# Feature: Selection Browse Automation Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the existing Cleanup Scope and Quarantine Root browse safety boundaries available through WPF automation help text, not only hover tooltips.

## Non-goals

- Do not change Cleanup Scope Selection behavior.
- Do not change Quarantine Root Selection behavior.
- Do not run a Storage Scan from browse actions.
- Do not create fixture or quarantine folders.
- Do not add visible helper text or layout changes.
- Do not move, restore, delete, create, or modify scanned files.
- Do not enable real-profile Quarantine execution or real-profile restore.

## User story / job story

As the project owner, I want path-selection and preview-root boundaries to be available to keyboard and assistive-technology paths, so that browse controls are not hover-only safety guidance.

## Current behavior

Cleanup Scope `Browse...` and Quarantine Root `Browse...` have disabled-state tooltips with the right safety boundaries. Those boundaries were not explicitly exposed through `AutomationProperties.HelpText`.

## Desired behavior

- Cleanup Scope `Browse...` automation help text says it chooses only a Cleanup Scope path and does not start a scan, bypass the real-profile gate, or approve cleanup.
- Quarantine Root `Browse...` automation help text says it chooses only a Quarantine Root for preview paths and does not create folders, move files, or approve cleanup.
- WPF smoke tests prove the automation help text carries the same safety boundaries as the tooltips.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope Selection | Clarify that browse boundaries should be available through tooltip and automation help text. | yes |
| Quarantine Root Selection | Clarify that preview-root browse boundaries should be available through tooltip and automation help text. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This reuses existing approved tooltip wording and does not change behavior.

Questions that can be deferred:

- Should a later accessibility pass add automation help text to execution-gate and restore-readiness controls?

## Grill notes

### Scenarios discussed

- User has prioritized scan-gate discoverability, Quarantine Preview/readiness clarity, and manual fixture review polish before any real cleanup execution.
- Recent automation-help-text packets proved the non-hover pattern on review toolbar, report/preview, and selected-row controls.

### Edge cases

- Disabled browse controls still need help text while scanning because the path-selection boundary remains relevant.

### Dependencies between decisions

- Help text must mirror existing tooltip semantics instead of creating a second safety-copy source.

## Evidence and validation gate

Evidence gathered:

- `docs/features/2026-05-29-selection-browse-tooltip-clarity.md` already defines the approved tooltip boundaries for this control group.
- README and domain docs already define Cleanup Scope browsing as path selection only and Quarantine Root browsing as preview-root selection only.
- WPF smoke tests already cover startup tooltips for both controls.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: WPF metadata and tests only, no filesystem mutation.
- [x] The narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add modal warnings to browse buttons in this packet.
- Do not add folder creation or probing for Quarantine Root browsing.
- Do not weaken the real-profile scan gate.
- Do not change keyboard navigation, focus order, control enablement, or browse behavior.

## Decisions made

Small feature-level decisions:

- Mirror existing tooltip safety wording into `AutomationProperties.HelpText` for Cleanup Scope and Quarantine Root `Browse...` controls.
- Add direct WPF smoke assertions for the automation help text boundaries.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF automation help text to Cleanup Scope and Quarantine Root `Browse...` buttons.
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
- `docs/features/2026-05-29-selection-browse-automation-help-text.md`

Possible:

- None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During fixture review, confirm visible browse tooltip wording still fits; automation help text is covered by smoke tests.

## Risks and assumptions

Risks:

- Help text can drift from tooltip wording if future packets update only one surface.

Assumptions:

- Reusing the exact tooltip wording is the least surprising way to expose the same safety boundary to automation paths.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added WPF automation help text to Cleanup Scope and Quarantine Root `Browse...` controls.
- Added test-facing accessors and WPF smoke assertions for path-only, preview-only, no-scan, no-folder-creation, no-file-modified, scan-gate, and no-approval help text.
- Updated README, domain docs, progress, and handoff.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selection-browse-automation-help-text.md`
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

- Consider execution-gate and restore-readiness automation help text in later focused packets if this pattern continues to prove useful.

Open questions:

- Should a later accessibility pass cover all safety-critical disabled controls with automation help text?

Risky assumptions:

- Reusing tooltip wording as automation help text is clear enough and avoids introducing a second safety-copy source.
