# Feature: All-Manifest Readiness Label Polish

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the Restore Readiness Preview action visibly distinct from selected manifest readiness by naming its all-manifest scope.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not enable all-manifest restore.
- Do not change fixture-only selected restore execution.
- Do not change manifest discovery, Restore Readiness Preview semantics, selected manifest readiness, or selected restore gate behavior.
- Do not add permanent deletion or cleanup history.

## User story / job story

As the project owner, I want the readiness buttons to distinguish all-manifest readiness from selected manifest readiness, so that I can choose the right read-only evidence pane during fixture review.

## Current behavior

The WPF button says `Preview restore readiness`, while the selected one-manifest button now says `Preview selected manifest readiness`. The shorter all-manifest label is accurate but less parallel.

## Desired behavior

- The WPF all-manifest readiness button says `Preview all-manifest readiness`.
- Placeholder text, README, checklist output, feature notes, and WPF smoke assertions use the same wording.
- Restore Readiness Preview remains read-only and does not expose all-manifest restore.

## Domain language changes

No new durable domain term.

| Term | Change | Docs updated? |
|---|---|---|
| Restore Readiness Preview | Use clearer visible text for the existing all-manifest readiness action. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is a wording refinement.

Questions that can be deferred:

- During visible fixture review, does the longer all-manifest readiness button label fit comfortably?

## Grill notes

### Scenarios discussed

- Restore Readiness Preview evaluates discovered Restore Manifests under the selected Quarantine Root.
- Selected manifest readiness evaluates one selected Restore Manifest.
- Neither readiness action is restore approval.

### Edge cases

- No manifests discovered.
- Multiple manifests discovered.
- A fixture selected manifest can still restore only through selected manifest readiness, selected restore gate, and exact `RESTORE`.
- Real-profile and custom non-fixture restore remain unavailable.

### Dependencies between decisions

- Depends on ADR 0012 read-only Restore Readiness Preview.
- Depends on the later selected manifest readiness label polish.
- Depends on all-manifest restore remaining unavailable.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: ADR 0012, Restore Readiness Preview feature brief, WPF readiness controls, WPF smoke tests, README, domain docs, fixture launcher checklist, progress log, and handoff.
- Tests/checks planned: WPF smoke tests, build, checklist-only output, and diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not rename the domain concept away from Restore Readiness Preview.
- Do not add an all-manifest restore action while polishing a read-only readiness label.

## Decisions made

Small feature-level decisions:

- Use `Preview all-manifest readiness` for the visible Restore Readiness Preview action.
- Use `all-manifest readiness preview` in checklist and current docs when contrasting this with selected manifest readiness.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update WPF button and placeholder text.
2. Add WPF smoke assertions for the button label and placeholder.
3. Update README, domain docs, fixture checklist, progress, and handoff.
4. Run narrow verification.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Possible:

- Related feature notes.

## Test plan

Manual checks:

- During fixture review, confirm the all-manifest readiness button label fits and remains visually distinct from selected manifest readiness.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- The longer all-manifest label may need visual adjustment after a manual fixture pass.

Assumptions:

- The explicit all-manifest wording helps distinguish root-wide readiness from selected manifest readiness.

## Completion notes

Completed on: 2026-05-29

What changed:

- Renamed the visible Restore Readiness Preview action to `Preview all-manifest readiness`.
- Updated placeholder text, README, checklist, domain docs, feature notes, progress, handoff, and WPF smoke assertions.
- Kept Restore Readiness Preview read-only and kept all restore execution availability boundaries unchanged.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-all-manifest-readiness-label-polish.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, progress log, handoff, MVP readiness audit, Restore Readiness Preview note, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible wording clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Confirm the longer label fits during the next visible fixture review.

Open questions:

- Does the longer all-manifest readiness label fit comfortably at the current WPF window width?

Risky assumptions:

- The explicit all-manifest wording is clearer than the shorter `Preview restore readiness` label.
