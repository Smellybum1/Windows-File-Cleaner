# Feature: Quarantine Readiness UI

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Show Restore Manifest Draft and Quarantine Confirmation Draft readiness in the WPF app after the user creates a Quarantine Preview.

## Non-goals

- Do not create the quarantine folder.
- Do not move, rename, delete, or modify scanned files.
- Do not write a Restore Manifest file.
- Do not add a Quarantine execution button.
- Do not ask for final cleanup approval.

## User story / job story

As the project owner, I want the app to show whether a quarantine preview has matching undo metadata and readiness blockers, so that I can review exact safety evidence before any future cleanup action exists.

## Current behavior

The core library can build Restore Manifest Draft and Quarantine Confirmation Draft objects. The WPF app only shows the Quarantine Preview summary.

## Desired behavior

After clicking `Preview quarantine`, the detail pane shows:

- Quarantine Preview included, blocked, redundant, and previewed bytes.
- Restore Manifest Draft id, entry count, total bytes, and executed-manifest status.
- Quarantine Confirmation Draft id, required future confirmation text, execution-implemented status, and readiness blockers.
- A clear no-files-modified line.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing terms used. | n/a |

## Open questions

Questions that must be answered before implementation:

- None. This packet is display-only.

Questions that can be deferred:

- Should future execution use a modal confirmation dialog or a dedicated review screen?
- Should Restore Manifest Draft JSON be exportable before execution, or stay in memory only?

## Grill notes

### Scenarios discussed

- The user wants Quarantine on `D:` with easy undo.
- Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft already exist in core.
- The WPF app should expose readiness evidence without writing files.

### Edge cases

- Shortlist changes should clear the preview and both drafts.
- Scan changes should clear the preview and both drafts.
- Blocked or redundant rows should appear as readiness blockers rather than approval.

### Dependencies between decisions

- Depends on Quarantine Preview.
- Depends on Restore Manifest Draft.
- Depends on Quarantine Confirmation Draft.
- Future Quarantine execution still needs a separate design and ADR review.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: WPF preview handler, Quarantine Preview display, Restore Manifest Draft, Quarantine Confirmation Draft, progress log.
- Tests/checks planned: WPF build, test harness, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add an execute button beside preview controls.
- Do not auto-save Restore Manifest Draft JSON from the UI.
- Do not hide readiness blockers behind a single pass/fail label.

## Decisions made

Small feature-level decisions:

- Generate draft ids in memory when the user clicks `Preview quarantine`.
- Show up to six readiness blockers in the existing detail pane.
- Keep Quarantine Preview CSV export unchanged as a report, not a manifest.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF state for current Restore Manifest Draft and Quarantine Confirmation Draft.
2. Build both drafts after Quarantine Preview creation.
3. Update the detail pane formatter to include draft summaries and readiness blockers.
4. Clear draft state when the preview is cleared.
5. Update docs and run checks.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `docs/features/2026-05-28-quarantine-readiness-ui.md`
- `.codex/progress.md`

Possible:

- Existing Restore Manifest Draft and Quarantine Confirmation Draft feature briefs.

## Test plan

Manual checks:

- Run a scan, add at least one Quarantine candidate to the Review Shortlist, click `Preview quarantine`, and confirm the detail pane shows preview, manifest draft, confirmation draft, readiness blockers, and no-files-modified text.

Automated tests:

- Build WPF project to verify XAML and event-handler wiring.
- Run the existing fixture test harness for core preview/draft behavior.

## Risks and assumptions

Risks:

- The detail pane can become dense until a dedicated readiness panel exists.

Assumptions:

- Showing draft ids and blockers in the existing pane is acceptable for this MVP stage.

## Completion notes

Completed on: 2026-05-28

What changed:

- WPF `Preview quarantine` now builds Restore Manifest Draft and Quarantine Confirmation Draft in memory.
- The detail pane shows draft ids, entry counts, bytes, required future confirmation text, execution status, readiness blocker count, and blocker details.
- Clearing scan or shortlist state clears preview and draft state.
- No execution, folder creation, file moving, deletion, or manifest writing was added.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `docs/features/2026-05-28-quarantine-readiness-ui.md`
- `.codex/progress.md`
- `docs/features/2026-05-28-restore-manifest-draft.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- Added this feature brief.
- Updated prior draft feature follow-up notes.
- Updated progress log.

ADRs added or skipped:

- Skipped. This is display-only UI for existing draft artifacts; execution flow remains future ADR material.

Follow-up work:

- Retest the WPF app with a real scan and review whether the readiness text is clear enough.
- Design actual Quarantine execution only after preview, confirmation, restore, and undo semantics are reviewed.

Open questions:

- Should future execution use a modal confirmation dialog or dedicated review screen?
- Should Restore Manifest Draft JSON be exportable before execution?

Risky assumptions:

- Existing detail pane density is acceptable for previewing readiness evidence in the MVP.
