# Feature: Quarantine Root Input Automation Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Mirror the typed Quarantine Root Selection safety tooltip into WPF automation help text.

## Non-goals

- Do not create the quarantine folder.
- Do not move, delete, rename, or modify scanned files.
- Do not write a Restore Manifest.
- Do not persist the Quarantine Root Selection as a setting.
- Do not treat a typed root as cleanup approval.
- Do not change Quarantine Preview eligibility.

## User story / job story

As the project owner, I want the typed Quarantine Root field to expose the same preview-only boundary to keyboard and assistive-technology surfaces as it does on hover, so typing a destination root cannot be mistaken for folder creation or cleanup approval.

## Current behavior

The Quarantine Root text box has a tooltip that says it is a read-only preview destination root and preview does not create the folder or move files. The adjacent Browse action already has tooltip and automation help text coverage, but the typed root field did not expose matching automation help text.

## Desired behavior

- The Quarantine Root text box exposes automation help text matching its preview-only/no-folder-created/no-file-moved tooltip.
- WPF smoke tests assert both tooltip and automation help text wording.
- Typed Quarantine Root behavior and preview gating stay unchanged.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Root Selection | Clarified that typed-root and browse controls should keep preview-only/no-folder-creation boundaries available through tooltip and automation help text. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is metadata parity for existing safety wording.

Questions that can be deferred:

- During visible fixture review, does the Quarantine Root toolbar remain readable with the safety note below it?

## Decisions made

Small feature-level decisions:

- Reuse the existing Quarantine Root text-box tooltip as automation help text.
- Keep the packet scoped to the typed root field because browse already has automation help text.

ADR-worthy decisions:

- [x] None

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During fixture review, confirm typed Quarantine Root wording remains discoverable and the toolbar still fits.

## Risks and assumptions

Risks:

- Automation help text improves non-hover discoverability but does not make safety text always visible.

Assumptions:

- The existing text-box tooltip remains the correct concise safety wording for typed Quarantine Root Selection.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added automation help text to the Quarantine Root text box.
- Added WPF smoke assertions for Quarantine Root text-box tooltip and automation help text.

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

- Skipped. This is reversible WPF metadata polish with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Confirm the Quarantine Root toolbar and safety note remain comfortable during visible fixture review.

Open questions:

- Does the Quarantine Root toolbar need visual tightening after fixture review?

Risky assumptions:

- Matching the existing tooltip text is enough for typed-root automation help text.
