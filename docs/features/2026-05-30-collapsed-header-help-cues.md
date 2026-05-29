# Feature: Collapsed Header Help Cues

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make Safety Summary and Quarantine Shortlist collapsed-header help easier to discover by adding compact visible `?` cues beside each dynamic header.

## Non-goals

- Do not change Storage Scan results, Review Shortlist membership, Quarantine Preview eligibility, fixture execution, undo, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not add modal help, popup help, or larger approval-looking badges.

## User story / job story

As the project owner, I want collapsed panel header help to have the same visible `?` cue pattern as the other safety summaries, so I can discover header state and approval-boundary wording without guessing that a long truncated header is hoverable.

## Current behavior

Safety Summary and Quarantine Shortlist headers already start with panel names, use lightweight state styling, and mirror dynamic header summary/state into tooltip and automation help text. The hover target is still visually implicit.

## Desired behavior

- Add a small circular `?` cue to the Safety Summary header.
- Add a small circular `?` cue to the Quarantine Shortlist header.
- Mirror each header's dynamic tooltip and automation help text onto its cue.
- Keep the cues compact and non-clickable so they do not read as scan, preview, or cleanup approval controls.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Scan Safety Summary | Added `SafetySummaryHeaderHelpCue` as the visible hover cue for collapsed-header help text. | yes |
| Review Shortlist | Added `QuarantineShortlistHeaderHelpCue` as the visible hover cue for Quarantine Shortlist collapsed-header help text. | yes |

## Open questions

Questions that must be answered before implementation:

- None. The user later preferred compact circular `?` cues for hidden safety tooltips.

Questions that can be deferred:

- During the next visible fixture pass, confirm the two header cues are noticeable without making the Expander headers feel crowded.

## Grill notes

### Scenarios discussed

- User verified collapsible panels worked and asked for useful closed-panel summaries.
- Later feedback preferred visible `?` hover cues over hidden hover-only help text.
- Existing feature notes left open whether collapsed panel headers should get always-visible help affordances.

### Edge cases

- Cues must mirror neutral, warning, ready/completed, and current-session quarantined header states.
- Cues must not imply cleanup approval, file modification, restore approval, or scan safety clearance.

### Dependencies between decisions

- This builds on existing collapsed header help text, header state help text, header styling, and panel-name header summaries.

## Evidence and validation gate

Evidence gathered:

- User answers: compact circular `?` hover cues are preferable for hidden safety tooltips.
- Existing code/docs inspected: Safety Summary and Quarantine Shortlist XAML headers, header update methods, WPF smoke assertions, collapsed panel header feature briefs, domain context/glossary, README manual checklist.
- Tests/checks planned: focused WPF app test build/run, checklist-only output, solution build, whitespace diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add popup help or larger badges for collapsed headers unless manual visual review shows compact cues are still insufficient.

## Decisions made

Small feature-level decisions:

- Place the cues inside the existing Expander headers next to the trimmed header text.
- Mirror cue help text from the existing header update methods so every dynamic header state stays synchronized.
- Keep cues non-clickable and visually consistent with the other review safety `?` cues.

ADR-worthy decisions:

- [x] None expected. This is reversible WPF help-cue polish with no persistence, file movement rule, restore rule, data-model, or security change.

## Implementation plan

1. Wrap the Safety Summary and Quarantine Shortlist header text in compact header layouts with right-side `?` cues.
2. Add test hooks for cue tooltip, automation name, and automation help text.
3. Extend existing header smoke assertions so startup, scan, preview, stale, blocked, current-session quarantined, and undo states verify the cue mirrors the header help text.
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
- `docs/features/2026-05-30-collapsed-header-help-cues.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- Existing collapsed-header feature briefs.

## Test plan

Manual checks:

- During the next fixture visual pass, confirm the header cues are visible but do not crowd the Expander headers or read like approval controls.

Automated tests:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- Expander headers already carry dense summaries; cues could add small layout pressure.

Assumptions:

- The consistent compact `?` cue pattern improves discoverability more than it distracts, especially when the header text is truncated.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added visible circular `?` help cues beside Safety Summary and Quarantine Shortlist collapsed headers.
- Mirrored each dynamic header tooltip and automation help text onto its help cue from the existing header update path.
- Added WPF smoke assertions that the cues keep matching header help text across existing dynamic header states.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-30-collapsed-header-help-cues.md`
- Existing collapsed-header feature briefs
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated collapsed header `?` help-cue prompts without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- README, domain context, glossary, this feature brief, related collapsed-header feature briefs, fixture checklist launcher, progress log, and handoff.

ADRs added or skipped:

- Skipped. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Follow-up work:

- Manual fixture visual pass to confirm all compact `?` cues read clearly without crowding the review surface.

Open questions:

- During the next visible fixture pass, confirm the header cues are noticeable without making the Expander headers feel crowded.

Risky assumptions:

- Compact non-clickable cues are enough discoverability for collapsed-header state/help text and avoid the approval-looking weight of a popup or badge.
