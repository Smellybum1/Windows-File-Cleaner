# Feature: Broad Restore Action Wording

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Clarify Restore Manifest discovery and readiness panes now that fixture-only selected restore exists.

## Non-goals

- Do not enable broad WPF Undo Quarantine.
- Do not enable real-profile selected restore.
- Do not enable custom non-fixture selected restore.
- Do not change fixture-only selected restore execution.
- Do not change Restore Readiness Preview behavior.
- Do not add permanent deletion or cleanup history.

## User story / job story

As the project owner, I want discovery and readiness panes to say that broad restore is unavailable while showing the route for fixture-only selected restore, so that read-only evidence panes are not confused with restore approval or execution.

## Current behavior

Quarantine Manifest Discovery, Selected Restore Manifest Review, and Restore Readiness Preview say that no restore action is available from their panes.

That was correct before fixture-only selected restore existed, but it can now sound broader than intended.

## Desired behavior

- Quarantine Manifest Discovery says no broad restore action is available from discovery.
- Restore Readiness Preview says no broad restore action is available from readiness preview.
- Selected Restore Manifest Review says it is readiness evidence only.
- The panes route fixture selected restore through selected readiness and the selected restore gate.

## Domain language changes

No new terms.

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Manifest Discovery | Clarify WPF no-broad-restore wording. | yes |
| Restore Readiness Preview | Clarify WPF no-broad-restore wording. | yes |
| Selected Restore Manifest Review | Clarify WPF readiness-evidence wording. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is wording alignment with the existing fixture-only selected restore boundary.

Questions that can be deferred:

- Should broad WPF Undo Quarantine use separate button text from selected restore if it is ever designed?

## Grill notes

### Scenarios discussed

- Discovery pane shows old manifests but should not expose broad restore.
- Restore Readiness Preview shows blockers across manifests but should not expose broad restore.
- Selected Restore Manifest Review shows selected readiness but should route fixture execution through the selected restore gate.

### Edge cases

- Fixture selected manifest has restorable entries.
- Real-profile or custom selected manifest has restorable entries but selected restore remains unavailable.

### Dependencies between decisions

- Depends on ADR 0011 read-only Quarantine Manifest Discovery.
- Depends on ADR 0012 read-only Restore Readiness Preview.
- Depends on ADR 0013 read-only Selected Restore Manifest Review.
- Depends on ADR 0015 fixture-only selected restore execution.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: WPF discovery/readiness/selected-review formatters, WPF smoke tests, README, domain docs, related feature briefs, progress log.
- Tests/checks planned: WPF smoke tests for discovery/readiness/selected review wording, build, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add broad restore execution in a wording packet.
- Do not make discovery or readiness panes responsible for selected restore approval.

## Decisions made

Small feature-level decisions:

- Use `broad restore action` for the unavailable all-manifest restore path.
- Use `selected restore gate` for the fixture-only execution route.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update WPF discovery, readiness, and selected-manifest review wording.
2. Update WPF smoke assertions.
3. Update README, domain docs, related feature briefs, progress, and handoff.
4. Run the narrow WPF smoke tests, build, and diff check.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-broad-restore-action-wording.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Possible:

- Related feature briefs for discovery/readiness/selected review.

## Test plan

Manual checks:

- During fixture review, confirm discovery/readiness panes do not imply broad restore and route fixture restore through selected readiness/gate.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- The phrase `broad restore action` may need user-facing refinement after visible review.

Assumptions:

- Distinguishing broad restore from fixture selected restore is clearer than the previous generic no-restore wording.

## Completion notes

Completed on: 2026-05-29

What changed:

- Updated Quarantine Manifest Discovery, Selected Restore Manifest Review, and Restore Readiness Preview wording.
- Updated WPF smoke assertions for the new wording.
- Kept discovery, readiness, and selected review read-only.
- Kept fixture-only selected restore unchanged and kept real-profile/custom selected restore blocked.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, related feature briefs, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible WPF wording/readiness clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Check during visible fixture review whether `broad restore action` is understandable.
- Later packet `2026-05-29-fixture-checklist-broad-restore-wording.md` added the broad-restore boundary to the launcher checklist.

Open questions:

- Should broad WPF Undo Quarantine use separate button text from selected restore if it is ever designed?

Risky assumptions:

- The distinction between broad restore and fixture selected restore is clear enough for this WPF text surface.
