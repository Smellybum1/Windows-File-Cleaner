# Feature: Restore Manifest Wording Polish

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Replace current-facing `old manifest` wording with existing `Restore Manifest` language where the app, tests, and manual checklist describe discovered manifests and unavailable restore actions.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not enable all-manifest restore.
- Do not change fixture-only selected restore execution.
- Do not change discovery, readiness, or confirmation gate behavior.
- Do not add permanent deletion or cleanup history.

## User story / job story

As the project owner, I want manifest wording to use the glossary term, so that manual fixture review and test failures describe the same restore boundary the UI shows.

## Current behavior

Some current-facing surfaces still say `old manifest` even though the glossary term is `Restore Manifest`.

## Desired behavior

- Current fixture undo tooltip says discovered Restore Manifests remain unavailable.
- README manual checks refer to the unavailable all-manifest restore action without introducing `old manifest`.
- WPF smoke failure messages use Restore Manifest language.
- Domain docs use Restore Manifest language for current undo and selected restore boundaries.

## Domain language changes

No new durable domain term.

| Term | Change | Docs updated? |
|---|---|---|
| Restore Manifest | Prefer this existing term over current-facing `old manifest` wording. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is a wording refinement.

Questions that can be deferred:

- If all-manifest restore is designed later, what should the final button label be?

## Grill notes

### Scenarios discussed

- Current fixture undo is only for the current fixture execution and should not imply discovered Restore Manifest restore is available.
- Discovery and readiness can show multiple Restore Manifests but still must not expose an all-manifest restore action.

### Edge cases

- Selected fixture restore remains available only through selected manifest readiness and exact `RESTORE` confirmation.
- Real-profile and custom non-fixture Restore Manifests remain preview-only.

### Dependencies between decisions

- Depends on ADR 0011 read-only discovery.
- Depends on ADR 0012 read-only restore readiness.
- Depends on ADR 0015 fixture-only selected restore execution.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: WPF tooltip, WPF smoke tests, README, domain context, glossary, progress log, thread handoff, and relevant discovery/restore ADRs.
- Tests/checks planned: WPF smoke tests, build, checklist-only output, and diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not rename historical ADR rationale wholesale; keep the packet focused on current-facing wording.
- Do not introduce a new `discovered-manifest restore` term when `Restore Manifest` already exists.

## Decisions made

Small feature-level decisions:

- Use `Restore Manifest` for current-facing manifest references.
- Keep `all-manifest restore action` as the unavailable broad action wording.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update WPF tooltip wording.
2. Update README and domain docs.
3. Update WPF smoke assertion messages.
4. Run the narrowest relevant verification.

## Files expected to change

Expected:

- `README.md`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Possible:

- None.

## Test plan

Manual checks:

- During fixture review, confirm disabled current-fixture undo tooltip refers to discovered Restore Manifests.
- During discovery/readiness review, confirm no all-manifest restore action is shown.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- This only improves wording; it does not replace visible manual fixture review.

Assumptions:

- Existing `Restore Manifest` language is clearer than carrying `old manifest` forward on current surfaces.

## Completion notes

Completed on: 2026-05-29

What changed:

- Replaced current-facing `old manifest` wording with `Restore Manifest` language in WPF tooltip, smoke-test messages, README, and domain docs.
- Kept all execution and restore availability boundaries unchanged.

Files changed:

- `README.md`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-restore-manifest-wording-polish.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible wording clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Confirm the tooltip and discovery/readiness language during the next visible fixture review.

Open questions:

- If all-manifest restore is designed later, what should the final button label be?

Risky assumptions:

- Current-facing `Restore Manifest` wording is less confusing than `old manifest` for this UI surface.
