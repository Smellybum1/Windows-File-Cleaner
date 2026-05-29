# Feature: Restore Readiness Preview

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add read-only restore-readiness preview for discovered Restore Manifests.

## Non-goals

- Do not restore discovered manifests.
- Do not call `UndoQuarantineExecutor.Undo`.
- Do not enable real-profile WPF Quarantine execution.
- Do not enable broad WPF Undo Quarantine.
- Do not write manifests.
- Do not create, move, delete, or clean up files or folders.
- Do not add persisted cleanup history.

## User story / job story

As the project owner, I want the app to show whether discovered Restore Manifests appear restorable, so that blockers are visible before any broad Undo Quarantine action exists.

## Current behavior

Quarantine Manifest Discovery shows valid action-scoped Restore Manifests and discovery issues. It does not inspect entry-level restore feasibility.

## Desired behavior

- Core Restore Readiness Preview evaluates discovered manifests under the selected Quarantine Root.
- It reports manifest-level blockers, restorable entries, blocked entries, skipped entries, and discovery issues.
- It checks missing quarantine paths, existing original paths, quarantine-path reparse points, non-moved entry states, and recovery-review states read-only.
- WPF exposes a status-only `Preview all-manifest readiness` action.
- WPF does not expose old-manifest restore execution.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Restore Readiness Preview | Add as read-only preview of discovered manifest restore feasibility. | yes |
| Restore Readiness Entry | Add as read-only entry-level preview row for one manifest entry. | yes |

## Open questions

Questions that must be answered before implementation:

- None. ADR 0012 selects read-only readiness preview first.

Questions that can be deferred:

- What UI should select one discovered Restore Manifest for execution?
- What exact confirmation should broad WPF Undo Quarantine require?
- Should successful restore offer empty action-folder cleanup?

## Grill notes

### Scenarios discussed

- The user wants easy undo for quarantined files.
- WPF can undo only the current fixture execution.
- Read-only discovery now shows old action-scoped Restore Manifests.
- The next safest step is previewing restore blockers before broad restore.

### Edge cases

- No discovered manifests.
- Manifest has no Moved entries.
- Quarantine path missing.
- Original path already exists.
- Quarantine path became a reparse point.
- Manifest has Moving, Failed, Restoring, or Restore failed entries.
- Manifest path relationships are invalid.

### Dependencies between decisions

- Depends on ADR 0011 read-only Quarantine Manifest Discovery.
- Adds ADR 0012 read-only Restore Readiness Preview.
- Precedes broad WPF Undo Quarantine.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: Undo Quarantine Executor, Quarantine Manifest Discovery, Restore Manifest model, WPF discovery UI, core/app tests, README, domain docs, progress log.
- Tests/checks planned: core readiness preview tests for restorable, collision, missing quarantine path, and already-restored states; WPF smoke coverage for status-only readiness; build, both test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not restore discovered old manifests in this packet.
- Do not add manifest selection yet.
- Do not treat readiness as approval.
- Do not clean up empty action folders.

## Decisions made

Small feature-level decisions:

- Preview all discovered valid manifests under the selected Quarantine Root.
- Keep WPF output capped and status-only.
- Reuse discovery issues in readiness output.
- Treat moved entries with no blockers as restorable.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0012-use-read-only-restore-readiness-preview.md`

## Implementation plan

1. Add ADR 0012 and this feature brief.
2. Add Restore Readiness Preview models and builder.
3. Extend discovery to retain valid Restore Manifests for readiness preview.
4. Add core readiness preview tests.
5. Add WPF `Preview all-manifest readiness` status-only action.
6. Add WPF smoke coverage proving readiness preview does not restore files.
7. Update README, domain docs, glossary, audit, and progress.
8. Run preflight, commit, push, and verify CI.

## Files expected to change

Expected:

- `docs/decisions/0012-use-read-only-restore-readiness-preview.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `src/WindowsFileCleaner.Core/QuarantineManifestDiscovery.cs`
- `src/WindowsFileCleaner.Core/QuarantineManifestDiscoveryBuilder.cs`
- `src/WindowsFileCleaner.Core/RestoreReadiness*.cs`
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

- Run fixture execute without undo, restart WPF with the same Quarantine Root, discover manifests, preview restore readiness, and confirm the manifest is shown as restorable without restoring files.
- Create an original-path collision and confirm readiness reports a blocker.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- Readiness could be mistaken for approval if wording is vague.
- Filesystem state can change after preview and before a future restore.

Assumptions:

- Checking path existence and attributes is acceptable as a read-only preview.
- Restore readiness should be recomputed immediately before any future restore execution.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added ADR 0012 for read-only Restore Readiness Preview.
- Added core Restore Readiness Preview models and builder.
- Extended Quarantine Manifest Discovery to retain valid Restore Manifest objects for readiness evaluation.
- Added WPF `Preview all-manifest readiness` status-only action and pane.
- Added core coverage for restorable, blocked, missing quarantine path, and already-restored readiness states.
- Added WPF smoke coverage proving readiness preview shows restorable discovered-manifest evidence without restoring files.

Files changed:

- `docs/decisions/0012-use-read-only-restore-readiness-preview.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `src/WindowsFileCleaner.Core/QuarantineManifestDiscovery.cs`
- `src/WindowsFileCleaner.Core/QuarantineManifestDiscoveryBuilder.cs`
- `src/WindowsFileCleaner.Core/RestoreReadinessDisposition.cs`
- `src/WindowsFileCleaner.Core/RestoreReadinessEntryPreview.cs`
- `src/WindowsFileCleaner.Core/RestoreReadinessManifestPreview.cs`
- `src/WindowsFileCleaner.Core/RestoreReadinessPreview.cs`
- `src/WindowsFileCleaner.Core/RestoreReadinessPreviewBuilder.cs`
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

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

Docs updated:

- ADR 0012, README, domain context, glossary, MVP readiness audit, this feature brief, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0012-use-read-only-restore-readiness-preview.md`.

Follow-up work:

- Add WPF selection for one discovered Restore Manifest.
- Add explicit confirmation and restore execution for selected manifests only after readiness preview is manually reviewed.
- Later selected-restore packets added fixture-only selected restore; readiness pane wording now says no all-manifest restore action is available from readiness preview and routes fixture selected restore through selected manifest readiness and the selected restore gate. Later label polish renamed the visible WPF action to `Preview all-manifest readiness`.
- Decide whether successful restore should offer empty action-folder cleanup.

Open questions:

- What UI should select one discovered Restore Manifest for execution?
- What exact confirmation should broad WPF Undo Quarantine require?
- Should successful restore offer empty action-folder cleanup?

Risky assumptions:

- Checking path existence and attributes is acceptable as a read-only preview.
- Restore readiness should be recomputed immediately before any future restore execution.
