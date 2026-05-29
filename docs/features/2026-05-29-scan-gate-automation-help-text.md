# Feature: Scan Gate Automation Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Mirror the dynamic `Scan` button gate tooltip into WPF automation help text and keep the disabled tooltip discoverable.

## Non-goals

- Do not change when `Scan` is enabled.
- Do not run MVP preflight from the WPF app.
- Do not create fixtures from the WPF app.
- Do not persist acknowledgement state.
- Do not scan or modify real-profile files.
- Do not enable real-profile or custom cleanup execution.

## User story / job story

As the project owner, I want keyboard and assistive-technology surfaces to expose the same scan-gate boundary as the `Scan` button tooltip, so locked real-profile scans and read-only ready states are clear before scanning.

## Current behavior

The `Scan` button tooltip is dynamic and scope-specific, but its wording was not mirrored into WPF automation help text. The disabled real-profile `Scan` button also did not explicitly set disabled-state tooltip display in XAML.

## Desired behavior

- Disabled real-profile `Scan` keeps its locked/acknowledgement wording available through tooltip and automation help text.
- Acknowledged real-profile `Scan` keeps read-only/unavailable-execution wording available through tooltip and automation help text.
- Fixture `Scan` keeps read-only plus later preview/exact-confirmation gate wording available through tooltip and automation help text.
- Custom `Scan` keeps read-only/unavailable-execution wording available through tooltip and automation help text.
- Scan enablement stays unchanged.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope Scan Gate | Clarified that the `Scan` button tooltip and automation help text should mirror the gate boundary. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is metadata parity for existing scan-gate wording.

Questions that can be deferred:

- During visible fixture/real-profile review, does the longer scan-gate header still fit comfortably?

## Decisions made

Small feature-level decisions:

- Reuse `FormatScanButtonToolTip` as the single source for the `Scan` tooltip and automation help text.
- Add disabled-state tooltip support to the `Scan` button.

ADR-worthy decisions:

- [x] None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During visible fixture or real-profile review, confirm the disabled or enabled `Scan` tooltip appears and matches the gate state.

## Risks and assumptions

Risks:

- Automation help text improves non-hover discoverability but does not make scan-gate safety text always visible.

Assumptions:

- Reusing the existing dynamic tooltip wording avoids drift between hover and assistive-technology surfaces.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added disabled-state tooltip support to `Scan`.
- Mirrored the dynamic `Scan` tooltip into `AutomationProperties.HelpText`.
- Added WPF smoke assertions for locked real-profile, acknowledged real-profile, fixture, and custom Scan automation help text.

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

- Skipped. This is reversible WPF metadata/tooltip polish with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Confirm the scan-gate header and disabled tooltip behavior during the next visible fixture/real-profile review.

Open questions:

- Does the scan-gate header wording need tightening after visible review?

Risky assumptions:

- The existing dynamic `Scan` tooltip is the right wording to mirror into automation help text.
