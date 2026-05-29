# Feature: Execution and Readiness Automation Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Mirror execution-gate and restore-readiness safety tooltips into WPF automation help text.

## Non-goals

- Do not change Quarantine Preview eligibility.
- Do not change Quarantine Execution Gate behavior.
- Do not change Restore Readiness Preview, Selected Restore Manifest Review, or Selected Restore Execution Gate behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, custom selected restore, permanent deletion, or cleanup history.

## User story / job story

As the project owner, I want keyboard and assistive-technology surfaces to expose the same safety boundaries as hover tooltips, so fixture-only execution and read-only readiness remain clear outside mouse hover review.

## Current behavior

Execution and readiness controls have safety tooltips, but the same wording is not available as WPF automation help text. `Preview selected restore gate` also lacked a direct tooltip even though it opens the selected restore confirmation readout.

## Desired behavior

- Quarantine confirmation, Execute quarantine, Undo fixture quarantine, selected restore confirmation, and Restore selected fixture manifest expose matching automation help text.
- Preview all-manifest readiness and Preview selected manifest readiness expose matching automation help text.
- Preview selected restore gate exposes a tooltip and matching automation help text.
- WPF smoke tests assert the safety wording.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Execution Scope Status | Clarify that disabled execution controls should carry matching automation help text. | yes |
| Selected Restore Manifest Review | Clarify that selected-only/not-approval wording should be available in automation help text. | yes |
| Selected Restore Execution Gate | Clarify that fixture-only/preview-only selected restore wording should be available in automation help text. | yes |
| Restore Readiness Preview | Clarify that all-manifest/no-restore wording should be available in automation help text. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is metadata parity for existing safety wording.

Questions that can be deferred:

- During visible fixture review, are hover tooltips and automation help text enough, or should some gate controls get always-visible help icons?

## Decisions made

Small feature-level decisions:

- Reuse existing tooltip wording as automation help text.
- Add a concise tooltip to `Preview selected restore gate` because it opens a safety-critical confirmation readout.

ADR-worthy decisions:

- [x] None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During visible fixture review, confirm execution/readiness controls still show the same hover wording and no execution availability changed.

## Risks and assumptions

Risks:

- Automation help text improves non-hover discoverability but does not make safety text always visible.

Assumptions:

- Reusing tooltip wording keeps keyboard and screen-reader context aligned without adding new product language.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added automation help text to Quarantine execution, current-fixture undo, all-manifest readiness, selected-manifest readiness, selected restore gate, selected restore confirmation, and fixture selected restore controls.
- Added a tooltip to `Preview selected restore gate`.
- Added WPF smoke assertions for execution/readiness automation help text.

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

- Skipped. This is reversible WPF metadata/tooltip clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Confirm during fixture review whether the remaining gate wording is discoverable enough.

Open questions:

- Should future polish add always-visible help icons for safety-critical gates?

Risky assumptions:

- Automation help text parity is the right next accessibility/readiness step before visible layout review.
