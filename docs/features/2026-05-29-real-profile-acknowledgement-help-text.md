# Feature: Real Profile Acknowledgement Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add tooltip and WPF automation help text to the real-profile preflight and fixture-review acknowledgement checkbox.

## Non-goals

- Do not change Cleanup Scope Scan Gate behavior.
- Do not run MVP preflight from the WPF app.
- Do not create fixture files from the WPF app.
- Do not start a Storage Scan when the acknowledgement is checked.
- Do not persist acknowledgement state.
- Do not enable cleanup execution, Quarantine execution, real-profile restore, permanent deletion, or cleanup history.
- Do not treat acknowledgement as cleanup approval.

## User story / job story

As the project owner, I want the real-profile acknowledgement checkbox to explain exactly what it does, so unlocking a read-only real-profile scan cannot be mistaken for running preflight, creating fixtures, starting a scan, persisting approval, or approving cleanup.

## Current behavior

The WPF header shows a real-profile acknowledgement checkbox and keeps `Scan` disabled until it is checked. The Scan button already mirrors scan-gate wording through tooltip and automation help text, but the checkbox itself did not expose matching help text.

## Desired behavior

- The real-profile acknowledgement checkbox has tooltip and WPF automation help text.
- Help text says the user is acknowledging MVP preflight and fixture review were already run.
- Help text says checking the box does not run preflight, create fixtures, start scanning, persist approval, or approve cleanup.
- WPF smoke tests assert the help text.
- Scan gate behavior, acknowledgement reset behavior, scan behavior, cleanup execution, restore, deletion, and cleanup history stay unchanged.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope Scan Gate | Clarified that acknowledgement tooltip/help text should mirror the local acknowledgement boundary. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is metadata parity for existing real-profile scan gate behavior.

Questions that can be deferred:

- During visible real-profile review, does the header need an always-visible help icon or shorter checkbox wording?

## Decisions made

Small feature-level decisions:

- Add concise tooltip and automation help text directly to `RealProfilePreflightCheckBox`.
- Keep checkbox content, visibility, enablement, and scan-gate behavior unchanged.
- Keep disabled-state tooltip display available.

ADR-worthy decisions:

- [x] None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During real-profile review, confirm the acknowledgement tooltip is discoverable and the header still fits comfortably.

## Risks and assumptions

Risks:

- Tooltip and automation help text improve discoverability but do not make the acknowledgement boundary always visible.

Assumptions:

- The existing checkbox label remains acceptable when paired with help text.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added tooltip and automation help text to the real-profile preflight/fixture-review acknowledgement checkbox.
- Added WPF smoke assertions for MVP preflight, fixture review, no-preflight-run, no-auto-scan, and no-cleanup-approval wording.
- Later packet `2026-05-30-real-profile-acknowledgement-help-cue.md` added a visible circular `?` help cue beside the checkbox and mirrored the same tooltip/help text there.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- README, domain docs, progress log, handoff, and this feature brief.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible WPF metadata/test polish with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Confirm during visible real-profile review whether the new cue is noticeable and whether the header still needs shorter text.

Open questions:

- Does the real-profile scan-gate header remain comfortable after accumulated safety wording?

Risky assumptions:

- Checkbox tooltip/help text plus a visible cue are enough to explain the acknowledgement boundary for now.
