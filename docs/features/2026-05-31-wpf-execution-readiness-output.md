# Feature: WPF Execution Readiness Output

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Show the core Quarantine Execution Readiness contract in the existing WPF Quarantine Preview and Quarantine Execution Gate panes.

This packet made readiness blockers visible by dimension before any real-profile execution was enabled. Later packets wire read-only Quarantine Root Execution Safety and Pre-Execution Revalidation evidence into WPF preview/gate output; Real-Profile Restore Readiness remains absent WPF evidence and still shows as a blocker.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile selected restore.
- Do not run live pre-execution revalidation from WPF.
- Do not create folders, write manifests, move files, restore files, delete files, or add cleanup history.
- Do not add a new WPF panel; keep the output in the existing preview/gate panes.

## Desired behavior

- Quarantine Preview shows an `Execution readiness contract` section after draft/confirmation readiness.
- Quarantine Execution Gate shows the same contract alongside exact-confirmation gate state.
- Fixture scopes show fixture-executable readiness while reminding that exact confirmation and the fixture-only gate still control movement.
- Custom and real-profile scopes show preview-only/current-build-no execution state and grouped readiness blockers.
- Real-profile missing prerequisites are grouped as Quarantine Root Execution Safety, Pre-Execution Revalidation, Real-Profile Restore Readiness, Scope and policy, or Review readiness.

## Files changed

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/features/2026-05-31-wpf-execution-readiness-output.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added read-only execution-readiness contract text to WPF Quarantine Preview and Quarantine Execution Gate output.
- Grouped readiness blockers by dimension in display text.
- Added WPF smoke assertions for fixture and custom preview/gate readiness contract output.
- Later packet `2026-05-31-real-profile-readiness-output-regression.md` added synthetic real-profile preview/gate readiness-output coverage without scanning or touching `C:\Users\moxhe`.
- Later packet `2026-05-31-real-profile-child-readiness-output-regression.md` added synthetic real-profile child preview/gate readiness-output coverage for the exact-scope blocker.
- Later packet `2026-05-31-wpf-root-execution-safety-evidence.md` wired read-only Quarantine Root Execution Safety evidence into WPF preview/gate output while keeping real-profile execution disabled.
- Later packet `2026-05-31-wpf-pre-execution-revalidation-evidence.md` wired read-only Pre-Execution Revalidation evidence into WPF preview/gate output while keeping real-profile execution disabled.

Open questions:

- None for this packet.

Risky assumptions:

- Showing missing restore readiness as a blocker is useful before that WPF check is wired.
