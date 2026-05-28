# Feature: Restore Manifest Draft

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Define and test the Restore Manifest draft shape before any Quarantine execution exists.

## Non-goals

- Do not create, move, rename, or delete scanned files.
- Do not create the quarantine folder.
- Do not write an executed Restore Manifest file.
- Do not add a Quarantine execution button.
- Do not treat a draft as proof that files moved.

## User story / job story

As the project owner, I want the app to have a clear undo metadata shape before file-moving code exists, so that future Quarantine work can be reversible by design.

## Current behavior

The app can create and export a read-only Quarantine Preview. It does not yet have a versioned restore metadata shape.

## Desired behavior

The core library can build an in-memory Restore Manifest Draft from included Quarantine Preview rows. The draft uses JSON schema version `restore-manifest.v1`, includes original/quarantine paths and review evidence, and excludes blocked or redundant preview rows. No files are modified and no manifest is written.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Restore Manifest | Clarified as versioned JSON metadata for executed Quarantine undo. | yes |
| Restore Manifest Draft | Added as in-memory proof shape generated from Quarantine Preview rows. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This packet is draft-only and read-only.

Questions that can be deferred:

- What exact manifest file path should future Quarantine execution use?
- Should future executed manifests include hashes for files, and if so which hash algorithm?
- Should future Undo Quarantine restore timestamps and attributes?

## Grill notes

### Scenarios discussed

- The user wants quarantine on `D:` with easy undo.
- The app now has Quarantine Preview but no execution.
- Undo requires metadata before file-moving code exists.

### Edge cases

- Blocked and redundant preview rows should not become manifest draft entries.
- Drafts must clearly identify themselves as drafts.
- Future executed manifests need schema versioning.

### Dependencies between decisions

- Depends on Quarantine Preview.
- Future Quarantine execution depends on this schema decision and explicit confirmation design.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: Quarantine Preview, Quarantine Preview CSV export, Restore Manifest glossary entry, ADR template.
- Tests/checks planned: draft builder fixture test, JSON serializer test, build, test harness, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not write a draft file automatically.
- Do not use CSV as the executed restore manifest format.
- Do not include blocked or redundant preview rows as draft entries.

## Decisions made

Small feature-level decisions:

- Add `RestoreManifestDraft` and `RestoreManifestEntryDraft` records.
- Add `RestoreManifestDraftBuilder` to project included Quarantine Preview rows.
- Add `RestoreManifestDraftJsonSerializer` for schema proof without writing files.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0003-use-json-restore-manifest.md`

## Implementation plan

1. Add ADR for JSON Restore Manifest.
2. Add domain docs for Restore Manifest and Restore Manifest Draft.
3. Add core draft records, builder, and JSON serializer.
4. Add fixture coverage proving drafts include only included preview rows and serialize with schema version.
5. Update progress docs and run checks.

## Files expected to change

Expected:

- `docs/decisions/0003-use-json-restore-manifest.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-restore-manifest-draft.md`
- `src/WindowsFileCleaner.Core/RestoreManifestDraft.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestEntryDraft.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestDraftBuilder.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestDraftJsonSerializer.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- None needed for this core-only draft packet.

Automated tests:

- Verify drafts include only included Quarantine Preview rows.
- Verify original and quarantine paths are preserved.
- Verify schema version is serialized.
- Verify blocked/redundant rows are excluded.
- Verify no quarantine folder or manifest file is created.

## Risks and assumptions

Risks:

- Draft metadata could be mistaken for executed cleanup history unless wording stays explicit.

Assumptions:

- JSON is the right first executed manifest format because it is versioned, structured, and local-readable.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added ADR 0003 for JSON Restore Manifest schema direction.
- Added Restore Manifest and Restore Manifest Draft domain docs.
- Added core Restore Manifest Draft records, builder, and JSON serializer.
- Drafts include only included Quarantine Preview rows and exclude blocked/redundant rows.
- Draft generation and serialization do not write manifest files or create quarantine folders.

Files changed:

- `docs/decisions/0003-use-json-restore-manifest.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-restore-manifest-draft.md`
- `src/WindowsFileCleaner.Core/RestoreManifestDraft.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestEntryDraft.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestDraftBuilder.cs`
- `src/WindowsFileCleaner.Core/RestoreManifestDraftJsonSerializer.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- Added JSON Restore Manifest ADR.
- Added Restore Manifest domain section.
- Added Restore Manifest Draft glossary term.
- Added this feature brief.
- Updated progress log.

ADRs added or skipped:

- Added `docs/decisions/0003-use-json-restore-manifest.md`.

Follow-up work:

- Quarantine Confirmation Draft and its WPF display now exist; design actual Quarantine execution before file-moving code.
- Decide exact executed manifest file path.
- Decide whether executed manifests should include file hashes.

Open questions:

- What exact manifest file path should future Quarantine execution use?
- Should future executed manifests include hashes for files, and if so which hash algorithm?
- Should future Undo Quarantine restore timestamps and attributes?

Risky assumptions:

- JSON is the right first executed manifest format because it is versioned, structured, and local-readable.
