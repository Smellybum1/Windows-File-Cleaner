# Feature: Selected Restore Manifest Review

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add a read-only selected-manifest review step after Quarantine Manifest Discovery so one discovered Restore Manifest can be selected and previewed for restore readiness without restoring files.

## Non-goals

- Do not restore discovered manifests.
- Do not call `UndoQuarantineExecutor.Undo`.
- Do not enable real-profile WPF Quarantine execution.
- Do not enable broad WPF Undo Quarantine.
- Do not write manifests.
- Do not create, move, delete, or clean up files or folders.
- Do not add persisted cleanup history.
- Do not treat selected readiness as approval to restore.

## User story / job story

As the project owner, I want to select one discovered Restore Manifest and see readiness for that one action, so that future undo work is tied to an explicit manifest rather than a broad root-level result.

## Current behavior

Quarantine Manifest Discovery shows summaries for discovered Restore Manifests. Restore Readiness Preview evaluates all discovered manifests under the selected Quarantine Root. The WPF app does not yet let the user choose one discovered manifest as the focus for future restore work.

## Desired behavior

- After `Discover manifests`, WPF exposes a read-only selection control populated from discovered Restore Manifest summaries.
- Selecting a Restore Manifest does not modify files and does not approve restore.
- `Preview selected readiness` evaluates only the selected Restore Manifest.
- The selected review reports selection issues when discovery has not run, no manifest is selected, or the selected path is not in the current discovery result.
- Existing all-manifest Restore Readiness Preview remains available.
- No broad old-manifest restore action is exposed.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Restore Manifest Review | Add as read-only review of one discovered Restore Manifest and its readiness. | yes |

## Open questions

Questions that must be answered before implementation:

- None. ADR 0013 selects read-only selected-manifest review before restore execution.

Questions that can be deferred:

- What exact confirmation phrase should selected broad Undo Quarantine require?
- Should future restore execution allow only manifests with zero readiness blockers?
- Should successful restore offer empty action-folder cleanup?

## Grill notes

### Scenarios discussed

- The app can already discover older action-scoped Restore Manifests.
- The app can already preview readiness across all discovered manifests.
- The next safest step is picking one discovered manifest and proving the selected review path without restoring.

### Edge cases

- Discovery has not run.
- Discovery finds zero valid manifests.
- The selected path is blank.
- The selected path no longer belongs to current discovery after Quarantine Root changes.
- The selected manifest has restorable entries.
- The selected manifest has blocked or recovery-review entries.

### Dependencies between decisions

- Depends on ADR 0011 read-only Quarantine Manifest Discovery.
- Depends on ADR 0012 read-only Restore Readiness Preview.
- Adds ADR 0013 read-only Selected Restore Manifest Review.
- Precedes broad WPF Undo Quarantine for discovered manifests.

## Evidence and validation gate

Evidence gathered:

- User answers: the project owner wants quarantine on `D:` with an easy undo path, but cleanup must not break current apps or user data.
- Existing code/docs inspected: Quarantine Manifest Discovery, Restore Readiness Preview, WPF discovery/readiness UI, core/app tests, README, domain docs, progress log.
- Tests/checks planned: core selected-manifest review tests; WPF smoke coverage for selection and selected readiness; build, both test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not restore selected old manifests in this packet.
- Do not treat selection as approval.
- Do not add persisted cleanup history.
- Do not remove the all-manifest readiness preview.

## Decisions made

Small feature-level decisions:

- Populate selection from current Quarantine Manifest Discovery results.
- Auto-select the newest discovered manifest for convenience, but require an explicit `Preview selected readiness` action before showing selected readiness details.
- Keep all selected-review output status-only.
- Keep broad Undo Quarantine unavailable.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0013-use-read-only-selected-restore-manifest-review.md`

## Implementation plan

1. Add ADR 0013 and this feature brief.
2. Add Selected Restore Manifest Review model and builder in core.
3. Add core tests for selected-only readiness and stale/invalid selection issues.
4. Add WPF selection control and selected readiness preview action.
5. Extend WPF smoke coverage to prove selected readiness is read-only.
6. Update README, domain docs, glossary, audit, and progress.
7. Run verification, commit, push, and verify CI.

## Files expected to change

Expected:

- `docs/decisions/0013-use-read-only-selected-restore-manifest-review.md`
- `docs/features/2026-05-29-selected-restore-manifest-review.md`
- `src/WindowsFileCleaner.Core/SelectedRestoreManifestReview*.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

## Test plan

Manual checks:

- Run fixture execute without undo, restart WPF with the same Quarantine Root, discover manifests, select the discovered manifest, preview selected readiness, and confirm files are not restored.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- Selected readiness could be mistaken for restore approval if wording is vague.
- The selected readiness result can become stale if files change after preview.

Assumptions:

- Selecting from discovered Restore Manifest summaries is sufficient for the first selected-review workflow.
- Future restore execution will recompute readiness immediately before moving files.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added ADR 0013 for read-only Selected Restore Manifest Review.
- Added core `SelectedRestoreManifestReview` model and builder.
- Added WPF Restore Manifest selection, default newest-manifest selection after discovery, and `Preview selected readiness`.
- Kept selected review status-only with no restore action.
- Added core and WPF coverage proving selected readiness reviews only the selected manifest and does not restore files.

Files changed:

- `docs/decisions/0013-use-read-only-selected-restore-manifest-review.md`
- `docs/features/2026-05-29-selected-restore-manifest-review.md`
- `src/WindowsFileCleaner.Core/SelectedRestoreManifestReview.cs`
- `src/WindowsFileCleaner.Core/SelectedRestoreManifestReviewBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- Final build, no-build test harnesses, MVP preflight, and `git diff --check` are recorded in `.codex/progress.md`.

Docs updated:

- ADR 0013, README, domain context, glossary, MVP readiness audit, this feature brief, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0013-use-read-only-selected-restore-manifest-review.md`.

Follow-up work:

- Add explicit confirmation and restore execution for selected manifests only after selected readiness is verified.
- Later selected-restore packets added fixture-only selected restore; selected review wording now calls itself readiness evidence and routes fixture selected restore through the selected restore gate.
- Decide stale-state handling between selected readiness preview and future restore execution.
- Decide whether successful restore should offer empty action-folder cleanup.

Open questions:

- What exact confirmation phrase should selected broad Undo Quarantine require?
- Should future restore execution allow only manifests with zero readiness blockers?
- Should successful restore offer empty action-folder cleanup?

Risky assumptions:

- Selecting from discovered Restore Manifest summaries is sufficient for the first selected-review workflow.
- Future restore execution will recompute readiness immediately before moving files.
