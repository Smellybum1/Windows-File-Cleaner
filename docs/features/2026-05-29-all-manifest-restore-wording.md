# Feature: All-Manifest Restore Wording

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the unavailable restore path wording more concrete by using `all-manifest restore action` instead of `broad restore action`.

## Non-goals

- Do not enable all-manifest restore.
- Do not enable real-profile selected restore.
- Do not enable custom non-fixture selected restore.
- Do not change fixture-only selected restore execution.
- Do not change discovery, readiness, or selected restore gate behavior.
- Do not add permanent deletion or cleanup history.

## User story / job story

As the project owner, I want the restore panes to say exactly which restore action is unavailable, so that I can distinguish all-manifest restore from fixture-only selected restore.

## Current behavior

Discovery, readiness, and checklist wording says no `broad restore action` is available.

That is accurate but a little internal. The unavailable action is more concretely the all-manifest restore path.

## Desired behavior

- WPF discovery and readiness panes say no `all-manifest restore action` is available.
- Fixture selected restore is still routed through selected readiness and the selected restore gate.
- The fixture launcher checklist uses the same wording.

## Domain language changes

No new durable domain term.

| Term | Change | Docs updated? |
|---|---|---|
| None | Use clearer user-facing wording for the unavailable all-manifest restore action. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is a wording refinement.

Questions that can be deferred:

- If all-manifest restore is ever designed, should the button use `Restore all discovered manifests` or another explicit label?

## Grill notes

### Scenarios discussed

- Discovery/readiness panes can show multiple manifests but must not expose an all-manifest restore action.
- Selected fixture restore remains available only through selected readiness and exact `RESTORE` confirmation.

### Edge cases

- A fixture manifest is restorable, but all-manifest restore still remains unavailable.
- A real-profile or custom manifest is restorable in preview, but selected restore and all-manifest restore remain unavailable.

### Dependencies between decisions

- Depends on ADR 0011 read-only discovery.
- Depends on ADR 0012 read-only restore readiness.
- Depends on ADR 0015 fixture-only selected restore execution.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: WPF discovery/readiness formatters, WPF smoke tests, README, domain docs, fixture launcher checklist, progress log, thread handoff.
- Tests/checks planned: WPF smoke tests, build, checklist-only output, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add a new all-manifest restore domain term until an execution design exists.
- Do not keep user-facing `broad restore action` wording if `all-manifest restore action` is clearer.

## Decisions made

Small feature-level decisions:

- Use `all-manifest restore action` in WPF and manual checklist wording.
- Preserve historical feature names that already used broad-restore wording.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update WPF discovery/readiness strings.
2. Update WPF smoke assertions.
3. Update README, domain docs, fixture checklist, related feature notes, progress, and handoff.
4. Run WPF smoke tests, build, checklist-only output, and diff check.

## Test plan

Manual checks:

- During fixture review, confirm discovery/readiness wording says no all-manifest restore action is available.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- `All-manifest restore action` is clearer than `broad restore action`, but it is still a new phrase for the user to react to during visible review.

Assumptions:

- Naming the unavailable path by scope helps distinguish it from fixture selected restore.

## Completion notes

Completed on: 2026-05-29

What changed:

- Replaced user-facing `broad restore action` wording with `all-manifest restore action` in WPF discovery/readiness panes and the fixture checklist.
- Updated WPF smoke assertions and docs.
- Kept discovery/readiness read-only and kept selected restore fixture-only.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, related feature briefs, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible wording clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Check during visible fixture review whether `all-manifest restore action` is understandable.

Open questions:

- If all-manifest restore is ever designed, should the button use `Restore all discovered manifests` or another explicit label?

Risky assumptions:

- `All-manifest restore action` is clearer than `broad restore action` for this UI surface.
