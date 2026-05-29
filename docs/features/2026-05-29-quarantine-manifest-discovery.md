# Feature: Quarantine Manifest Discovery

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Add read-only discovery of action-scoped Restore Manifests under the selected Quarantine Root.

## Non-goals

- Do not restore old manifests.
- Do not enable real-profile WPF Quarantine execution.
- Do not enable broad WPF Undo Quarantine.
- Do not create, move, delete, or clean up files or folders.
- Do not add persisted cleanup history.
- Do not recursively scan arbitrary quarantine-root contents outside the `actions` layout.

## User story / job story

As the project owner, I want the app to show which Restore Manifests exist under the selected Quarantine Root, so that future undo/recovery work has visible evidence before restore execution is added.

## Current behavior

WPF can undo only the current fixture execution from its in-memory Restore Manifest. After restart, old manifests on disk are not visible in the app.

## Desired behavior

- A read-only core discovery component looks under `<quarantine-root>\actions\*\restore-manifest.json`.
- Discovery returns summaries for valid Restore Manifests.
- Discovery returns issues for missing `actions` folders, invalid JSON, unsupported schemas, unreadable files, and path-shape blockers.
- WPF exposes a status-only discovery action for the current Quarantine Root.
- WPF does not restore old manifests or imply real-profile cleanup is available.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Manifest Discovery | Add as read-only discovery of action-scoped Restore Manifests. | yes |
| Restore Manifest Summary | Add as compact read-only status for one discovered Restore Manifest. | yes |

## Open questions

Questions that must be answered before implementation:

- None. ADR 0011 selects read-only discovery first.

Questions that can be deferred:

- What UI should select one old Restore Manifest for future Undo Quarantine?
- Should discovery support browsing directly to a single manifest file?
- Should successful undo offer to clean up empty action folders?

## Grill notes

### Scenarios discussed

- The user wants quarantine on `D:` with an easy undo path.
- Current WPF undo works only for the fixture action in memory.
- Manifest discovery/history is repeatedly listed as the next prerequisite before broad undo.

### Edge cases

- Quarantine Root is invalid or relative.
- Quarantine Root exists but has no `actions` folder.
- An action folder is missing `restore-manifest.json`.
- A manifest file is unreadable or invalid JSON.
- A manifest uses an unsupported schema version.
- A manifest path does not match its action-scoped location.

### Dependencies between decisions

- Depends on ADR 0004 action-scoped quarantine layout.
- Depends on ADR 0005 write-ahead Restore Manifest.
- Depends on ADR 0006 Restore Manifest File Store.
- Depends on ADR 0011 read-only discovery boundary.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: Restore Manifest JSON serializer, Restore Manifest model, PathSafety, Quarantine Root Selection, WPF execution/undo UI, source-level filesystem write regression, ADRs 0004-0010, README, progress log.
- Tests/checks planned: core discovery fixture tests, WPF smoke coverage for status-only discovery, build, both test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add old-manifest restore in this packet.
- Do not call this persisted cleanup history.
- Do not scan the entire quarantine root recursively.
- Do not clean up empty action folders.

## Decisions made

Small feature-level decisions:

- Discover only direct action manifests under `<quarantine-root>\actions\*\restore-manifest.json`.
- Treat missing actions folder as an informational no-manifests state.
- Sort valid summaries by newest `UpdatedAtUtc` first.
- Show discovery as read-only WPF evidence.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0011-use-read-only-quarantine-manifest-discovery.md`

## Implementation plan

1. Add ADR 0011 and this feature brief.
2. Add Restore Manifest JSON deserialize support.
3. Add read-only Quarantine Manifest Discovery models and builder.
4. Add core tests for valid, missing, invalid, and path-blocked discovery.
5. Add WPF status-only discovery UI.
6. Add WPF smoke coverage proving discovery shows old fixture manifests without enabling broad restore.
7. Update README, domain docs, glossary, audit, and progress.
8. Run preflight, commit, push, and verify CI.

## Files expected to change

Expected:

- `docs/decisions/0011-use-read-only-quarantine-manifest-discovery.md`
- `docs/features/2026-05-29-quarantine-manifest-discovery.md`
- `src/WindowsFileCleaner.Core/RestoreManifestJsonSerializer.cs`
- `src/WindowsFileCleaner.Core/QuarantineManifestDiscovery*.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

Possible:

- Small helper changes if manifest path validation needs shared code.

## Test plan

Manual checks:

- Run fixture execute/undo, restart WPF with the same Quarantine Root, and use manifest discovery to see the old Restore Manifest summary.
- Confirm no restore action is available for discovered old manifests.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- Discovery could be mistaken for a restore workflow if wording is too vague.
- Invalid manifest files may reveal edge cases in JSON compatibility.

Assumptions:

- Action-scoped manifest discovery is enough for the next broad undo design step.
- Reading manifest metadata under a user-selected Quarantine Root is acceptable because it does not touch original user-profile files.

## Completion notes

Completed on: YYYY-MM-DD

What changed:

- Added ADR 0011 for read-only Quarantine Manifest Discovery.
- Added Restore Manifest JSON deserialization.
- Added read-only core discovery models and builder.
- Added WPF `Discover manifests` status-only action and pane.
- Added core discovery coverage for valid manifests, missing/invalid manifests, and path mismatches.
- Added WPF smoke coverage proving discovery can show a persisted fixture manifest after a new window starts without enabling broad Undo Quarantine.

Files changed:

- `docs/decisions/0011-use-read-only-quarantine-manifest-discovery.md`
- `docs/features/2026-05-29-quarantine-manifest-discovery.md`
- `src/WindowsFileCleaner.Core/RestoreManifestJsonSerializer.cs`
- `src/WindowsFileCleaner.Core/QuarantineManifestDiscovery.cs`
- `src/WindowsFileCleaner.Core/QuarantineManifestDiscoveryBuilder.cs`
- `src/WindowsFileCleaner.Core/QuarantineManifestDiscoveryIssue.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestSummary.cs`
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

- ADR 0011, README, domain context, glossary, MVP readiness audit, this feature brief, and progress log.

ADRs added or skipped:

- Added `docs/decisions/0011-use-read-only-quarantine-manifest-discovery.md`.

Follow-up work:

- Add WPF selection and restore execution for discovered old manifests only after read-only discovery is manually reviewed.
- Later selected-restore packets added fixture-only selected restore; discovery pane wording now says no all-manifest restore action is available from discovery and routes fixture selected restore through selected readiness and the selected restore gate.
- Decide whether successful undo should offer empty action-folder cleanup.
- Decide whether discovery should support browsing directly to a single manifest file.

Open questions:

- What UI should select one old Restore Manifest for future Undo Quarantine?
- Should discovery support browsing directly to a single manifest file?
- Should successful undo offer to clean up empty action folders?

Risky assumptions:

- Action-scoped manifest discovery is enough for the next broad undo design step.
- Reading manifest metadata under a selected Quarantine Root is acceptable because it does not touch original user-profile files.
