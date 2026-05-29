# Feature: Selected Restore Scope Status

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the Selected Restore Execution Gate visibly explain fixture-only versus preview-only selected restore behavior.

## Non-goals

- Do not enable real-profile selected restore.
- Do not enable custom non-fixture selected restore.
- Do not change selected restore confirmation semantics.
- Do not add broad WPF Undo Quarantine.
- Do not add permanent deletion or cleanup history.

## User story / job story

As the project owner, I want the selected restore gate to state its execution scope plainly, so that fixture-only restore proof cannot be mistaken for real-profile restore approval.

## Current behavior

The Selected Restore Execution Gate shows `Execution implemented: yes/no` and `Can execute: yes/no`.

That is behaviorally correct, but it asks the user to infer whether the selected Restore Manifest is fixture-executable or preview-only.

## Desired behavior

- The Selected Restore Execution Gate shows `Execution scope status`.
- The gate shows an `Approval boundary` line.
- Fixture selected manifests state that fixture-only selected restore requires selected readiness and exact `RESTORE`.
- Real-profile and custom non-fixture selected manifests state that selected restore remains unavailable.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Restore Execution Gate | Clarify that WPF panes should show scope status and approval-boundary wording. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is a wording/readiness clarity packet.

Questions that can be deferred:

- Should the selected restore gate eventually hide the technical `Execution implemented` line after manual fixture review?

## Grill notes

### Scenarios discussed

- Fixture selected manifest: gate can open only after selected readiness and exact `RESTORE`.
- Custom or real-profile selected manifest: gate stays preview-only even when `RESTORE` matches.

### Edge cases

- The user sees `Can execute: no` before typing exact `RESTORE` for a fixture selected manifest.
- The user sees `Can execute: no` after typing exact `RESTORE` for a custom non-fixture selected manifest.

### Dependencies between decisions

- Mirrors the Quarantine Execution Scope Status and approval-boundary wording pattern.
- Depends on ADR 0015 keeping selected restore execution fixture-only.

## Evidence and validation gate

Evidence gathered:

- User asked to prefer review/readiness clarity before any real cleanup execution.
- Existing code/docs inspected: WPF Selected Restore Execution Gate formatting, WPF smoke tests, README, domain context, glossary, thread handoff, progress log, ADR 0015.
- Tests/checks planned: WPF smoke tests for fixture selected restore and custom non-fixture selected restore blockers; build; diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not enable real-profile selected restore in a wording packet.
- Do not remove existing technical gate fields before manual review.

## Decisions made

Small feature-level decisions:

- Use the same visible labels as Quarantine gates: `Execution scope status` and `Approval boundary`.
- Keep wording concise enough for the existing WPF pane.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add selected restore scope-status and approval-boundary formatting helpers.
2. Show those lines in Selected Restore Execution Gate output.
3. Extend fixture and custom WPF smoke assertions.
4. Update README, domain docs, feature brief, handoff, and progress log.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-restore-scope-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- During the next fixture review, preview selected restore gate and confirm fixture-only scope status plus approval-boundary wording fits the pane.
- During custom or real-profile manifest review, confirm selected restore stays preview-only after typing `RESTORE`.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- The additional lines may make the selected restore gate pane denser during visible review.

Assumptions:

- Existing `IsExecutionImplemented` remains the correct source for fixture-only versus preview-only selected restore wording.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added `Execution scope status` and `Approval boundary` lines to the Selected Restore Execution Gate pane.
- Added WPF smoke assertions for fixture-only and preview-only selected restore wording.
- Kept selected restore execution fixture-only and kept real-profile/custom selected restore blocked.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-restore-scope-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, thread handoff, progress log, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible WPF wording/readiness clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Check visible fixture layout with `.\tools\Start-MvpFixtureReview.ps1`.
- Decide later whether the technical `Execution implemented` line should remain.

Open questions:

- Does the extra selected restore gate wording make the pane easier to scan during visible fixture review?

Risky assumptions:

- `IsExecutionImplemented` remains the correct source for selected restore scope-status wording.
