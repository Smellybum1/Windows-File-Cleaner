# Feature: Readiness Scope Tooltip Clarity

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add scope and approval-boundary tooltips to the all-manifest and selected-manifest readiness controls.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not enable all-manifest restore.
- Do not change fixture-only selected restore execution.
- Do not change manifest discovery, Restore Readiness Preview, Selected Restore Manifest Review, selected restore gate, or readiness semantics.
- Do not add permanent deletion or cleanup history.

## User story / job story

As the project owner, I want readiness button tooltips to explain what each readiness preview covers, so that the longer labels are easier to understand during fixture review.

## Current behavior

The readiness buttons now say `Preview all-manifest readiness` and `Preview selected manifest readiness`, but neither control has a dedicated tooltip. The selected manifest readiness control starts disabled before discovery.

## Desired behavior

- `Preview all-manifest readiness` tooltip says the action is read-only for discovered Restore Manifests under the selected Quarantine Root and restores no files.
- `Preview selected manifest readiness` tooltip says the action is read-only for the selected Restore Manifest only and is not restore approval.
- The selected-manifest readiness tooltip appears even when the control is disabled.
- WPF smoke tests cover both tooltips.

## Domain language changes

No new durable domain term.

| Term | Change | Docs updated? |
|---|---|---|
| Restore Readiness Preview | Clarify WPF tooltip scope/no-restore wording. | yes |
| Selected Restore Manifest Review | Clarify WPF tooltip selected-only/not-approval wording. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is a tooltip wording refinement.

Questions that can be deferred:

- During visible fixture review, are tooltips discoverable enough or should future polish add inline help icons?

## Grill notes

### Scenarios discussed

- All-manifest readiness evaluates discovered Restore Manifests under the selected Quarantine Root.
- Selected manifest readiness evaluates one selected Restore Manifest only.
- Neither readiness preview is restore approval or execution.

### Edge cases

- Selected manifest readiness is disabled before discovery and selection.
- Fixture selected restore still requires selected manifest readiness, selected restore gate, and exact `RESTORE`.
- Real-profile and custom non-fixture restore remain unavailable.

### Dependencies between decisions

- Depends on all-manifest readiness label polish.
- Depends on selected manifest readiness label polish.
- Depends on all readiness previews remaining read-only.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: WPF readiness controls, WPF smoke tests, README, domain context, glossary, fixture checklist, progress log, and handoff.
- Tests/checks planned: WPF smoke tests, build, checklist-only output, and diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add restore execution while polishing read-only readiness controls.
- Do not rely only on long labels when a tooltip can carry scope details.

## Decisions made

Small feature-level decisions:

- Add tooltips directly to both readiness buttons.
- Use `ToolTipService.ShowOnDisabled` for selected manifest readiness because it starts disabled before discovery.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF readiness tooltips and test accessors.
2. Add WPF smoke assertions for both tooltips.
3. Update README, domain docs, fixture checklist, progress, and handoff.
4. Run narrow verification.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Possible:

- None.

## Test plan

Manual checks:

- During fixture review, hover both readiness buttons and confirm their tooltips distinguish all-manifest versus selected-manifest readiness.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- Tooltips may still be less discoverable than inline help text during manual review.

Assumptions:

- Scope tooltips are enough for this small packet before a visible fixture review decides whether inline help is needed.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added scope/approval-boundary tooltips to `Preview all-manifest readiness` and `Preview selected manifest readiness`.
- Added disabled-state tooltip support for selected manifest readiness.
- Added WPF smoke assertions for the tooltip text.
- Updated docs and checklist wording.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-readiness-scope-tooltip-clarity.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible UI tooltip clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Confirm tooltip discoverability during the next visible fixture review.

Open questions:

- Are tooltips enough, or should future polish add inline help icons for readiness controls?

Risky assumptions:

- Tooltips are discoverable enough for this stage of review polish.
