# Feature: WPF Non-D Quarantine Root Acknowledgement

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Expose the existing non-`D:` Quarantine Root acknowledgement in WPF as read-only readiness evidence for Quarantine Root Execution Safety.

This advances ADR 0018's future real-profile readiness path without enabling real-profile Quarantine execution, selected real-profile restore, permanent deletion, or cleanup history.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable custom non-fixture Quarantine execution.
- Do not enable real-profile Undo Quarantine.
- Do not treat non-`D:` acknowledgement as cleanup approval.
- Do not create folders, write manifests, move files, restore files, delete files, or add cleanup history.

## Desired behavior

- Fully qualified non-`D:` Quarantine Roots enable a local acknowledgement checkbox.
- Preferred `D:` roots and invalid/relative roots keep the checkbox disabled and unchecked.
- Changing the Quarantine Root clears the acknowledgement.
- Changing the acknowledgement clears stale Quarantine Preview/gate state and asks the user to preview again.
- Quarantine Root Execution Safety receives the acknowledgement when building read-only WPF preview/gate evidence.
- Unacknowledged non-`D:` roots show a root-safety blocker.
- Acknowledged safe non-`D:` roots clear only the non-`D:` acknowledgement blocker; unsafe roots remain blocked by containment, collision, capacity, or layout blockers.

## Files changed

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-31-quarantine-root-execution-safety.md`
- `docs/features/2026-05-31-wpf-root-execution-safety-evidence.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added `NonPreferredQuarantineRootAcknowledgementBox` under the Quarantine Root Safety Note.
- Added dynamic enablement/help text for fully qualified non-`D:` roots.
- Passed the acknowledgement into `QuarantineRootExecutionSafetyBuilder` for WPF readiness evidence.
- Added WPF smoke coverage for unacknowledged and acknowledged non-`D:` root safety evidence, stale preview clearing, no-folder-creation behavior, and reset when returning to a preferred `D:` root.
- Later packet `2026-05-31-non-d-root-acknowledgement-help-cue.md` added a visible hoverable `?` cue beside the acknowledgement and mirrored the checkbox tooltip/help text onto that cue.

Open questions:

- Manual fixture review should check whether the checkbox feels clear enough or visually crowds the Quarantine Root area.

Risky assumptions:

- Showing the acknowledgement before real-profile execution exists is useful because it explains a future readiness blocker early without changing movement behavior.
