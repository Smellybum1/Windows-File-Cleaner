# ADR 0010: Use Current-Fixture-Execution WPF Undo

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

The visible WPF app can execute Quarantine for recognized fixture Cleanup Scopes only. The core Undo Quarantine Executor can restore moved entries from a Restore Manifest, but the WPF app does not expose undo yet.

Undo is safety-sensitive because restoring can collide with new original paths or reveal ambiguous recovery state. A broad manifest picker/history UI is not needed to prove the first visible undo path.

Constraints:

- Real-profile WPF Undo Quarantine must remain unavailable in this packet.
- WPF undo should use the Restore Manifest produced by the current fixture execution only.
- WPF should call the core `UndoQuarantineExecutor`; it should not implement restore movement directly.
- Undo must not permanently delete files or clean up quarantine folders.
- Manifest discovery and history remain separate follow-up work.

## Decision

Add a WPF `Undo fixture quarantine` action that becomes available only after the current fixture-only WPF Quarantine execution produces a Restore Manifest with moved entries.

The action should call `UndoQuarantineExecutor.Undo` for the current in-memory Restore Manifest, show restore results, update the current Restore Manifest status, and disable repeat undo for the current execution attempt.

This ADR does not implement real-profile WPF Undo Quarantine, manifest discovery, persistent cleanup history, quarantine-folder cleanup, or permanent deletion.

## Options considered

### Option A: Keep WPF undo unavailable

Pros:

- Avoids adding another visible file-moving path.

Cons:

- Fixture-only execution remains visibly one-way.
- The user's requested easy undo path remains unproven in the WPF app.

### Option B: Current-fixture-execution WPF undo

Pros:

- Proves the visible app can call the tested core undo path.
- Keeps the scope bounded to synthetic files and the current manifest.
- Avoids manifest discovery complexity.
- Builds confidence before any real-profile execution.

Cons:

- Does not restore older manifests.
- Does not clean up empty quarantine action folders.
- Still needs stale-state wording because the scan snapshot may not match the filesystem.

### Option C: Full manifest picker/history UI

Pros:

- Closer to a complete undo UX.

Cons:

- Mixes manifest discovery, history, recovery review, and restore execution in one packet.
- Larger risk surface before fixture-only visible undo is proven.

## Why this decision

Current-fixture-execution WPF undo proves reversibility in the same visible workflow that now moves synthetic files, while keeping real-profile and old-manifest recovery out of scope.

## Consequences

Positive consequences:

- WPF can prove execute and undo against synthetic files.
- The Restore Manifest restore lifecycle is visible from the app.
- Real-profile cleanup remains blocked.

Negative consequences:

- The first WPF undo path only exists immediately after a fixture execution.
- Future manifest discovery still needs design.
- The current scan remains stale after execute and undo.

## Reversal cost

Low before real-profile execution and persistent cleanup history exist.

## Follow-up work

- Add WPF manifest discovery/history for older fixture and eventual real-profile manifests.
- Add real-profile WPF Undo Quarantine only after real-profile execution is designed.
- Decide whether successful undo should offer empty action-folder cleanup.

## Supersedes

- Implements the WPF undo follow-up from ADR 0008 and ADR 0009 for the current fixture execution only.

## Superseded by

- None.
