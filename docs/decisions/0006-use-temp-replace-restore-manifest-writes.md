# ADR 0006: Use Temp Replace Restore Manifest Writes

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

ADR 0005 selected a write-ahead Restore Manifest model. The next step toward Quarantine execution is a narrow file writer for `restore-manifest.json`, without moving files or wiring the WPF Execute button.

Manifest writing is safety-sensitive because a corrupt, missing, or stale manifest can weaken Undo Quarantine and partial-failure recovery.

Constraints:

- The writer must only write the action-scoped Restore Manifest path.
- Preview, draft, gate, scanner, and UI code must remain read-only with respect to cleanup execution.
- The first writer must be fixture-tested before any file-moving code exists.
- The project does not need a database for the MVP.
- The write pattern should reduce the chance of leaving a partially written manifest file.

## Decision

Write Restore Manifest JSON through a temporary file in the same action folder, then replace the target manifest path.

The writer should:

1. Validate that `ManifestPath` is inside `ActionRootPath`.
2. Validate that the manifest filename is `restore-manifest.json`.
3. Create the manifest directory when needed.
4. Serialize the Restore Manifest JSON to a uniquely named temporary file in the same directory.
5. If `restore-manifest.json` already exists, replace it with the temporary file.
6. If `restore-manifest.json` does not exist, move the temporary file into place.
7. Delete the temporary file on failure when possible.

This file store may use filesystem write APIs, but only in the narrow Restore Manifest file-store component. The source-level filesystem-call regression test must allow those calls only in that component and keep preview/draft/gate/UI/scanner code blocked from cleanup execution writes.

This ADR does not implement file moves, folder moves, Undo Quarantine, WPF execution wiring, or permanent deletion.

## Options considered

### Option A: Direct `File.WriteAllText` to the manifest path

Pros:

- Simple implementation.
- Easy to test.

Cons:

- A failed or interrupted write can leave a partial manifest.
- Repeated writes during future execution are more fragile.

### Option B: Temp-file write then replace

Pros:

- Reduces the chance of a partially written target manifest.
- Keeps temp and target on the same volume and folder.
- Does not require a database.
- Fits the action-scoped layout.

Cons:

- Uses more filesystem APIs.
- Requires stricter allowlist tests.
- Still needs recovery handling if the replace itself fails.

### Option C: Append-only journal file

Pros:

- Preserves historical state transitions.
- Can be robust against some interrupted writes.

Cons:

- More complex to read and compact.
- Adds another artifact beside the Restore Manifest.
- Too heavy before the first MVP executor.

## Why this decision

Temp-file replacement is a pragmatic middle path. It improves durability over direct writes without introducing an app-private database or a separate journal format.

## Consequences

Positive consequences:

- The future executor can write the planned manifest before moving files.
- Repeated status updates have one narrow persistence path.
- The source-level guard can stay strict through a component allowlist.

Negative consequences:

- The file store must clean up temporary files carefully.
- Future recovery UI should account for the possibility of a leftover temp file after a hard crash.
- The allowlist test becomes more nuanced than a blanket denial of write APIs.

## Reversal cost

Low before actual Quarantine execution is wired. Moderate after executed manifests exist because recovery tooling may depend on this file shape and write behavior.

## Follow-up work

- Add fixture-backed tests for manifest write failure and temp-file cleanup.
- Core fixture-first file/folder moving is covered by ADR 0007.
- Add Undo Quarantine after moved-entry manifests are produced by execution.

## Supersedes

- None. Refines ADR 0005's manifest-write follow-up.

## Superseded by

- None.
