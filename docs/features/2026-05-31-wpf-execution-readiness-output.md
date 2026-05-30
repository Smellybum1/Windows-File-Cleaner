# Feature: WPF Execution Readiness Output

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Show the core Quarantine Execution Readiness contract in the existing WPF Quarantine Preview and Quarantine Execution Gate panes.

This packet made readiness blockers visible by dimension before any real-profile execution was enabled. Later packets wire read-only Quarantine Root Execution Safety, Pre-Execution Revalidation, and Real-Profile Restore Readiness evidence into WPF output while keeping real-profile execution disabled.

A later clarity packet adds a compact Quarantine Readiness Summary above the confirmation controls so the high-level readiness state is visible without reading the full gate text.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile selected restore.
- Do not run live pre-execution revalidation from WPF.
- Do not create folders, write manifests, move files, restore files, delete files, or add cleanup history.
- Do not add a new WPF panel; keep the detailed output in the existing preview/gate panes.

## Desired behavior

- Quarantine Preview shows an `Execution readiness contract` section after draft/confirmation readiness.
- Quarantine Execution Gate shows the same contract alongside exact-confirmation gate state.
- Fixture scopes show fixture-executable readiness while reminding that exact confirmation and the fixture-only gate still control movement.
- Custom and real-profile scopes show preview-only/current-build-no execution state and grouped readiness blockers.
- Real-profile missing prerequisites are grouped as Quarantine Root Execution Safety, Pre-Execution Revalidation, Real-Profile Restore Readiness, Scope and policy, or Review readiness.
- A compact Quarantine Readiness Summary mirrors fixture-ready/open, preview-only, stale-executed, and undo-completed states without replacing the detailed gate.

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
- Later packet `2026-05-31-wpf-real-profile-restore-readiness-evidence.md` wired read-only Real-Profile Restore Readiness evidence into WPF selected restore gate output and forward readiness consumption while keeping real-profile restore and Quarantine execution disabled.
- Later packet `2026-05-31-quarantine-readiness-summary.md` added a compact WPF readiness summary line and tooltip/help text so fixture-ready, preview-only, stale-executed, and undo-completed states are easier to see without enabling real-profile execution.

Open questions:

- None for this packet.

Risky assumptions:

- Showing readiness evidence inside the existing preview/gate panes plus a compact summary line is useful, but a later manual review may still decide a dedicated readiness pane is clearer.
