# Feature: Non-D Root Acknowledgement Help Cue

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Make the WPF Non-D Quarantine Root Acknowledgement boundary easier to discover by adding the same visible hoverable `?` help cue pattern used by nearby safety and execution controls.

This is readability and manual-review polish only. It does not enable real-profile Quarantine execution, selected real-profile restore, permanent deletion, folder creation, manifest writing, or cleanup history.

## Non-goals

- Do not change when the non-`D:` acknowledgement is enabled.
- Do not treat non-`D:` acknowledgement as cleanup approval.
- Do not enable real-profile or custom non-fixture Quarantine execution.
- Do not enable real-profile selected restore or broad Undo Quarantine.
- Do not create folders, write manifests, move files, restore files, delete files, or add cleanup history.

## Desired behavior

- The non-`D:` acknowledgement checkbox has a nearby hoverable `?` help cue.
- The cue mirrors the checkbox tooltip and automation help text in enabled and disabled states.
- The cue uses the same Help cursor and prompt tooltip delay as the other circular help cues.
- Changing the acknowledgement still clears stale Quarantine Preview/gate state and requires a fresh preview.
- Quarantine Root Execution Safety still receives the acknowledgement as read-only evidence only.

## Files changed

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-31-wpf-non-d-root-acknowledgement.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Grouped the Non-D Quarantine Root Acknowledgement checkbox with a visible circular `?` help cue.
- Mirrored the checkbox tooltip and automation help text onto the cue.
- Expanded WPF smoke affordance coverage from eighteen to nineteen tracked circular help cues.
- Updated fixture checklist and durable docs so manual review checks the new cue.

Open questions:

- Manual fixture review should still check whether the acknowledgement row feels clear and not crowded.

Risky assumptions:

- One more compact cue is worthwhile here because this acknowledgement is safety-sensitive and currently appears next to several other readiness controls.
