# Feature: Selected Manifest Readiness Label Polish

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the selected Restore Manifest readiness action explicit in visible WPF labels, manual review docs, checklist output, and tests.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not enable all-manifest restore.
- Do not change fixture-only selected restore execution.
- Do not change manifest discovery, selected review, selected restore gate, or readiness semantics.
- Do not add permanent deletion or cleanup history.

## User story / job story

As the project owner, I want the selected readiness button to name the selected manifest, so that I can tell it reviews one Restore Manifest rather than all discovered manifests.

## Current behavior

The selected manifest review control says `Preview selected readiness`. That is compact but asks the user to infer that the selected object is the selected Restore Manifest.

## Desired behavior

- The WPF button says `Preview selected manifest readiness`.
- Placeholder, status-adjacent pane text, disabled tooltips, README, checklist output, and tests use the same wording.
- Fixture selected restore remains routed through selected manifest readiness and the selected restore gate.

## Domain language changes

No new durable domain term.

| Term | Change | Docs updated? |
|---|---|---|
| Selected Restore Manifest Review | Use clearer button text for the existing selected manifest readiness action. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is a wording refinement.

Questions that can be deferred:

- During visible fixture review, does the longer button label still fit comfortably?

## Grill notes

### Scenarios discussed

- One Restore Manifest is selected from discovery.
- The user previews readiness for that selected Restore Manifest only.
- Fixture restore remains available only after selected manifest readiness, selected restore gate preview, and exact `RESTORE`.

### Edge cases

- All-manifest Restore Readiness Preview remains separate.
- Real-profile and custom non-fixture selected restore stay unavailable.

### Dependencies between decisions

- Depends on Selected Restore Manifest Review.
- Depends on Selected Restore Confirmation Draft and Selected Restore Execution Gate.
- Depends on fixture-only Selected Restore Execution.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: WPF selected manifest controls, WPF/core tests, README, domain docs, fixture launcher checklist, progress log, handoff, and relevant feature briefs.
- Tests/checks planned: core tests, WPF smoke tests, build, checklist-only output, and diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not rename core types or workflows; this is visible wording polish.
- Do not shorten the label back to `selected readiness` if manual review still finds it ambiguous.

## Decisions made

Small feature-level decisions:

- Use `Preview selected manifest readiness` for the visible selected manifest readiness button.
- Use `selected manifest readiness` in current docs and checklist wording when referring to that one-manifest readiness step.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update WPF button, placeholder, pane, and tooltip text.
2. Update WPF/core tests for the label and wording.
3. Update README, domain docs, fixture checklist, progress, and handoff.
4. Run narrow verification.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `src/WindowsFileCleaner.Core/SelectedRestoreManifestReviewBuilder.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Possible:

- Related current feature notes.

## Test plan

Manual checks:

- During fixture review, confirm the selected manifest readiness button label fits and clearly applies to one selected Restore Manifest.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- The longer button label may need visual adjustment after a manual fixture pass.

Assumptions:

- Naming the selected manifest directly is clearer than the shorter `selected readiness` label.

## Completion notes

Completed on: 2026-05-29

What changed:

- Renamed the visible selected readiness button to `Preview selected manifest readiness`.
- Updated placeholder, pane, tooltip, checklist, README, domain docs, and tests to use selected manifest readiness wording.
- Kept all execution and restore availability boundaries unchanged.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `src/WindowsFileCleaner.Core/SelectedRestoreManifestReviewBuilder.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-manifest-readiness-label-polish.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-restore-manifest-wording-polish.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, progress log, handoff, MVP readiness audit, restore-manifest wording note, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible wording clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Confirm the longer label fits during the next visible fixture review.

Open questions:

- Does the longer label fit comfortably at the current WPF window width?

Risky assumptions:

- The explicit selected manifest wording is worth the extra button width.
