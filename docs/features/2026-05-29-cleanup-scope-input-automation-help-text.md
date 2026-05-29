# Feature: Cleanup Scope Input Automation Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add tooltip and WPF automation help text to the typed Cleanup Scope Selection field.

## Non-goals

- Do not change Cleanup Scope classification.
- Do not change Cleanup Scope Scan Gate behavior.
- Do not run MVP preflight from the WPF app.
- Do not create fixtures from the WPF app.
- Do not auto-start a scan after editing the path.
- Do not scan or modify real-profile files.
- Do not enable cleanup execution.

## User story / job story

As the project owner, I want the typed Cleanup Scope field to expose the same path-only and scan-gate boundaries as the Browse action, so typing a path cannot be mistaken for scan approval, real-profile gate bypass, or cleanup approval.

## Current behavior

The Cleanup Scope Browse action has tooltip and automation help text saying browsing selects a path only and does not start a scan, bypass the real-profile gate, or approve cleanup. The typed `ScopePathBox` did not have equivalent tooltip or automation help text.

## Desired behavior

- The Cleanup Scope text box has a tooltip explaining path-only selection, no auto-scan, no real-profile gate bypass, and no cleanup approval.
- The same wording is available through WPF automation help text.
- WPF smoke tests assert both tooltip and automation help text.
- Scan gate and scan execution behavior stay unchanged.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope Selection | Clarified that typed-path and browse controls should keep path-only/no-scan/no-gate-bypass/no-approval boundaries available through tooltip and automation help text. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is metadata parity for existing Cleanup Scope Selection wording.

Questions that can be deferred:

- During visible fixture review, does the header stay readable after accumulated safety wording?

## Decisions made

Small feature-level decisions:

- Add concise tooltip/help text directly to `ScopePathBox`.
- Use wording parallel to the existing Browse action while replacing browsing with editing.

ADR-worthy decisions:

- [x] None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During fixture or real-profile review, confirm the header still fits and the typed scope tooltip is discoverable.

## Risks and assumptions

Risks:

- Tooltip and automation help text improve discoverability but do not make safety wording always visible.

Assumptions:

- The typed Cleanup Scope field should carry the same boundary as Browse because both are Cleanup Scope Selection.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added tooltip and automation help text to `ScopePathBox`.
- Added WPF smoke assertions for typed Cleanup Scope path-only, no-auto-scan, no-real-profile-gate-bypass, and no-cleanup-approval wording.

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

- Confirm during visible fixture/real-profile review whether the header remains comfortable.

Open questions:

- Does the header need visual tightening after the next manual review?

Risky assumptions:

- Tooltip plus automation help text is sufficient for typed Cleanup Scope Selection for now.
