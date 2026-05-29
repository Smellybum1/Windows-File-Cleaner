# Feature: Quarantine Preview Status Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the inline Quarantine Preview readiness/status help easier to discover by adding a compact visible `?` cue beside the status line.

## Non-goals

- Do not add real-profile Quarantine execution.
- Do not add real-profile Undo Quarantine.
- Do not add permanent deletion or persisted cleanup history.
- Do not change Quarantine Preview eligibility, fixture execution, undo, selected restore, or manifest behavior.
- Do not add a modal, popup, or larger approval-looking badge.

## User story / job story

As the project owner, I want the Quarantine Preview readiness/status tooltip to have a small visible help cue, so I can find the dry-run and approval-boundary wording without guessing that the status text itself is hoverable.

## Current behavior

The inline Quarantine Preview status mirrors dynamic text into tooltip and automation help text and names the current status state, but the hover target is visually implicit.

## Desired behavior

- Show a small circular `?` cue beside the inline Quarantine Preview readiness/status line.
- Keep the cue non-action-like and compact so it does not read as cleanup approval.
- Mirror the same dynamic tooltip and WPF automation help text onto the cue.
- Preserve existing neutral/success/warning/error status styling and all preview/execution/undo behavior.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Preview | Added `QuarantinePreviewStatusHelpCue` as the visible hover cue for the inline preview readiness/status tooltip/help text. | yes |

## Open questions

Questions that must be answered before implementation:

- None. The user already preferred compact circular `?` help cues for hidden safety tooltips.

Questions that can be deferred:

- During the next visible fixture pass, confirm the cue is noticeable without crowding the Quarantine Shortlist area.

## Grill notes

### Scenarios discussed

- User previously missed Quarantine Preview success when feedback lived only in the status bar.
- Later packets made the feedback inline, styled it, and mirrored state wording into help text.
- User then preferred visible `?` cues for hidden safety tooltips.

### Edge cases

- Cue text must stay synchronized across empty shortlist, needs-preview, invalid root, ready preview, stale preview, blocked preview, fixture execution, and undo states.
- Cue must not create folders, move files, restore files, delete files, or imply cleanup approval.

### Dependencies between decisions

- This depends on the existing `SetQuarantinePreviewStatus` path so dynamic state is updated from one place.

## Evidence and validation gate

Evidence gathered:

- User answers: compact circular `?` hover cue is preferable for hidden safety tooltips.
- Existing code/docs inspected: inline Quarantine Preview status, status styling, status help text, status-state help text, ADRs 0009-0012, domain context/glossary, README manual checklist.
- Tests/checks planned: focused WPF app test build/run, checklist-only output, solution build, whitespace diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add a popup, modal, or larger preview badge unless the compact cue and styled inline status are still insufficient after manual fixture review.

## Decisions made

Small feature-level decisions:

- Place the cue next to the existing inline status, not in the verbose gate output or status bar.
- Mirror cue help text from `SetQuarantinePreviewStatus` so every existing state remains synchronized.
- Keep the cue non-clickable and visually consistent with the other review safety `?` cues.

ADR-worthy decisions:

- [x] None expected. This is reversible WPF help-cue polish with no persistence, file movement rule, restore rule, data-model, or security change.

## Implementation plan

1. Wrap the inline preview status in a compact layout with a right-aligned `?` cue.
2. Add test hooks for cue tooltip, automation name, and automation help text.
3. Extend existing Quarantine Preview status smoke assertions so every exercised state verifies the cue.
4. Update README, domain docs, checklist output, feature briefs, progress, and handoff.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-30-quarantine-preview-status-help-cue.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- Existing Quarantine Preview status feature briefs.

## Test plan

Manual checks:

- During the next fixture visual pass, confirm the cue is visible but does not make Quarantine Preview look like cleanup approval.

Automated tests:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- The Quarantine Shortlist area is already dense; another cue could add small layout pressure.

Assumptions:

- A compact `?` cue improves discoverability more than it distracts because it matches the existing Review Mix, Matched Review Mix, Review Shortlist Safety Mix, and Review Grid Mode Status cue pattern.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a visible circular `?` help cue beside the inline Quarantine Preview readiness/status line.
- Mirrored the dynamic tooltip and automation help text onto the help cue from the existing status update path.
- Added WPF smoke assertions for the cue automation name, cue tooltip, and cue automation help text across existing preview status states.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-30-quarantine-preview-status-help-cue.md`
- Existing Quarantine Preview/status feature briefs
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated inline Quarantine Preview `?` help-cue prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- README, domain context, glossary, this feature brief, related Quarantine Preview/status feature briefs, fixture checklist launcher, progress log, and handoff.

ADRs added or skipped:

- Skipped. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Follow-up work:

- Manual fixture visual pass to confirm all compact safety `?` cues read clearly without crowding the review surface.

Open questions:

- During the next visible fixture pass, confirm the cue is noticeable without crowding the Quarantine Shortlist area.

Risky assumptions:

- A compact non-clickable cue is enough discoverability for this inline status and avoids the approval-looking weight of a popup or badge.
