# Feature: Execution Control Tooltip Clarity

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Keep Quarantine and selected-restore execution boundaries visible on disabled controls.

## Non-goals

- Do not change Quarantine Preview eligibility.
- Do not change Quarantine Execution Gate behavior.
- Do not change Selected Restore Execution Gate behavior.
- Do not enable real-profile or custom cleanup execution.
- Do not enable real-profile or custom selected restore.
- Do not move, restore, delete, create, or modify real-profile files.

## User story / job story

As the project owner, I want disabled execution controls to explain why they are gated, so that fixture-only paths do not imply real-profile/custom execution is available and stale wording does not hide fixture selected restore.

## Current behavior

The visible gate panes explain fixture-only versus preview-only scope status. Some disabled controls have sparse tooltips, and the selected restore confirmation tooltip still says discovered-manifest restore execution is unavailable even though fixture selected restore now exists after selected readiness and exact `RESTORE`.

## Desired behavior

- Quarantine confirmation and execution controls explain fixture-only execution and real-profile/custom blockers in tooltips.
- Undo fixture quarantine explains it is limited to the current fixture execution.
- Selected restore confirmation and execution controls explain fixture selected restore and real-profile/custom blockers in tooltips.
- Disabled controls show these tooltips.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Execution Scope Status | Clarified that disabled confirmation/execution tooltips should repeat the scope boundary. | yes |
| Selected Restore Execution Gate | Clarified that disabled-control tooltips should align with fixture-only selected restore scope. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is tooltip clarity for existing gates.

Questions that can be deferred:

- Should future layout polish add inline help icons for these gates if tooltips are not discoverable enough?

## Grill notes

### Scenarios discussed

- Startup/default real-profile scope: execution controls are disabled and should explain unavailable real-profile/custom execution.
- Fixture selected restore: exact `RESTORE` can enable `Restore selected fixture manifest`, and tooltip wording should not say all discovered-manifest restore execution is unavailable.

### Edge cases

- Disabled WPF controls need `ToolTipService.ShowOnDisabled` for the tooltip to be discoverable.
- Tooltip wording must not imply approval or bypass gate requirements.

### Dependencies between decisions

- Depends on Quarantine Execution Gate and Selected Restore Execution Gate semantics.
- Does not affect execution availability or filesystem behavior.

## Evidence and validation gate

Evidence gathered:

- Existing XAML inspected: Quarantine confirmation tooltip existed, execution/undo buttons had no tooltip, and selected restore confirmation tooltip was stale.
- Existing WPF smoke tests cover startup disabled state and selected fixture restore flow.
- Tests/checks planned: WPF smoke tests, build, checklist-only output, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add new execution paths.
- Do not rely only on the pane text when disabled controls can carry their own safety context.

## Decisions made

Small feature-level decisions:

- Add `ToolTipService.ShowOnDisabled` to execution-related confirmation fields and buttons.
- Keep tooltip wording short but explicit about fixture-only and real/custom blockers.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update Quarantine execution and selected-restore tooltips.
2. Add WPF smoke assertions for startup disabled tooltips and enabled fixture selected restore tooltip.
3. Update README, domain docs, fixture checklist, progress, handoff, and this feature brief.
4. Run WPF smoke tests, build, checklist-only output, and diff check.

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During visible fixture review, hover disabled execution/restore controls and confirm the tooltips appear.

## Risks and assumptions

Risks:

- Tooltip text helps discoverability only when the user hovers or focuses the controls.

Assumptions:

- Aligning disabled-control tooltips with visible gate panes reduces confusion before any real cleanup execution design.

## Completion notes

Completed on: 2026-05-29

What changed:

- Updated Quarantine confirmation, Quarantine included shortlist, Undo fixture quarantine, selected restore confirmation, and Restore selected fixture manifest tooltips.
- Enabled disabled-control tooltip display for those gate controls.
- Added WPF smoke assertions for tooltip wording.
- Updated fixture checklist wording to include execution and restore tooltip review.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- README, domain docs, progress log, handoff, and this feature brief.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible WPF tooltip clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Check during visible fixture review whether the tooltips are discoverable enough or whether inline help icons are needed.

Open questions:

- Should future layout polish add inline help icons for execution gates?

Risky assumptions:

- Tooltips are a useful enough discoverability layer for disabled execution controls.
