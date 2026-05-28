# ADR 0008: Use Fixture-First Undo Quarantine

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

The project owner asked for quarantine on `D:` with an easy undo path. The core Quarantine Executor can now move fixture files into action-scoped quarantine paths and write Restore Manifest status updates, but Undo Quarantine does not exist yet.

Undo is safety-sensitive because restoring files can overwrite newer files, recreate folders, or make recovery state ambiguous.

Constraints:

- WPF execution remains disabled until a separate wiring packet.
- Undo Quarantine must be fixture-tested before real-profile use.
- Undo must restore only entries that the manifest records as `Moved`.
- Undo must not overwrite an existing original path.
- Undo must write recovery state before and after restore attempts.
- Undo must not permanently delete files or clean up quarantine folders in the first implementation.

## Decision

Add a core `UndoQuarantineExecutor` that restores moved Restore Manifest entries from quarantine paths back to original paths, but do not wire it to WPF yet.

Extend Restore Manifest status values:

- `RestoreManifestEntryStatus.Restoring`
- `RestoreManifestEntryStatus.Restored`
- `RestoreManifestEntryStatus.RestoreFailed`
- `RestoreManifestActionStatus.Restoring`
- `RestoreManifestActionStatus.Restored`
- `RestoreManifestActionStatus.RestorePartialFailure`
- `RestoreManifestActionStatus.RestoreFailed`

The undo executor should:

1. Require at least one `Moved` entry.
2. Restore only `Moved` entries.
3. Write `Restoring` before each restore attempt.
4. Revalidate that the quarantine path exists, original path does not exist, and the quarantine source is not a reparse point.
5. Create only the original parent folder when needed.
6. Move the file or folder from quarantine path to original path.
7. Write `Restored` or `RestoreFailed` after each attempt.
8. Continue after per-entry failures so partial restore state is visible.
9. Stop before later restore attempts when a manifest write fails.

This ADR does not implement WPF Undo Quarantine, permanent deletion, quarantine-folder cleanup, or automatic rollback.

## Options considered

### Option A: Wire Undo into WPF immediately

Pros:

- Faster path to visible undo.

Cons:

- Mixes UI discovery, manifest selection, stale-state handling, and restore risk in one packet.
- Harder to prove restore behavior with fixtures first.

### Option B: Core fixture-first Undo Quarantine

Pros:

- Proves restore behavior against synthetic files first.
- Keeps UI disabled while restore semantics mature.
- Preserves strict filesystem-call allowlists.
- Builds toward the user's requested easy undo path.

Cons:

- The app still cannot undo from the visible WPF UI after this packet.
- Requires a later manifest picker/history UI.

### Option C: Same-execution rollback inside Quarantine Executor

Pros:

- Could attempt to reverse partial failures immediately.

Cons:

- Mixes cleanup execution and undo semantics.
- Can hide failure evidence.
- Risks overwriting original paths without a separate restore review.

## Why this decision

Fixture-first Undo Quarantine gives the project an actual recovery primitive before WPF cleanup execution is enabled. It keeps the next UI packet smaller and safer because both forward and reverse filesystem moves will already be covered by synthetic tests.

## Consequences

Positive consequences:

- The core library can prove quarantine and undo as separate, tested operations.
- Restore Manifest status captures restore attempts and failures.
- Undo refuses to overwrite original paths.

Negative consequences:

- Restore Manifest schema grows before real manifests exist.
- Future UI must explain both cleanup and restore status values.
- Quarantine folder cleanup remains separate follow-up work.

## Reversal cost

Low before WPF execution and real manifests exist. Moderate after real manifests with restore status values are written.

## Follow-up work

- Add WPF manifest discovery and Undo Quarantine UI only after fixture undo remains green.
- Add recovery UI for `Restoring`, `RestoreFailed`, and leftover temp manifest states.
- Add optional quarantine folder cleanup only after restore behavior is trusted.

## Supersedes

- None. Implements the undo follow-up from ADR 0005 and ADR 0007.

## Superseded by

- None.
