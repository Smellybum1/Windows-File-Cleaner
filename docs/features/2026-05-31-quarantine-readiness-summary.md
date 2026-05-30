# Feature: Quarantine Readiness Summary

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add a compact, read-only WPF readiness summary near Quarantine Preview and Quarantine Execution Gate controls so the most important gate state is visible without reading the full constrained gate text.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile selected restore or broad Undo Quarantine.
- Do not create a new approval surface.
- Do not add a new help cue or increase help-cue count.
- Do not create folders, write manifests, move files, restore files, delete files, or add cleanup history.

## Desired behavior

- Before preview, show that the summary is waiting for `Preview shortlist quarantine`.
- After clean fixture preview, show 0 readiness blockers and whether exact `QUARANTINE` is still needed or has opened the fixture-only action.
- For custom and real-profile previews, show preview-only/current-build-no execution state plus compact missing readiness dimensions.
- After fixture execution or undo, switch the summary to stale-scan or undo-completed wording.
- Mirror the summary into tooltip and automation help text with `Summary state:` and no-create/no-move/no-restore/no-delete/no-manifest-write/not-cleanup-approval boundaries.

## Files changed

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/features/2026-05-31-wpf-execution-readiness-output.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added `QuarantineReadinessSummaryText` between inline Quarantine Preview status and exact confirmation controls.
- Added semantic neutral/success/warning styling for waiting, fixture-ready/open, preview-only, stale-executed, and undo-completed summary states.
- Added WPF smoke assertions for fixture preview/open/executed/undo summary states, custom preview-only summary, real-profile preview-only missing dimensions, and tooltip/automation help text.
- Updated manual fixture checklist wording so visible review includes the new summary line.

Open questions:

- Manual fixture review should confirm whether the summary line is enough or whether a later dedicated readiness pane is still useful.

Risky assumptions:

- A single compact line plus the existing detailed gate text is less crowded than adding another panel or help cue.
