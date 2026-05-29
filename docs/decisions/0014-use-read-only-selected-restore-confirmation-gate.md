# ADR 0014: Use Read-Only Selected Restore Confirmation Gate

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

Selected Restore Manifest Review lets the WPF app focus on one discovered Restore Manifest and preview readiness for that manifest only. The next step toward broad Undo Quarantine is to prove the confirmation and gate semantics for the selected manifest before any old-manifest restore execution exists.

Restore execution is safety-sensitive because it can recreate original folders, move files out of quarantine, and write updated Restore Manifest state. The app must not expose selected old-manifest restore execution until selection, readiness, exact confirmation text, stale-state expectations, and fixture-first execution behavior are verified.

Constraints:

- Real-profile WPF Quarantine execution remains unavailable.
- Broad WPF Undo Quarantine remains unavailable.
- Selected restore confirmation must be read-only in this packet.
- Selected restore confirmation must depend on the current Selected Restore Manifest Review.
- The visible WPF app must not call `UndoQuarantineExecutor.Undo` for discovered older manifests.

## Decision

Add a read-only Selected Restore Confirmation Draft and Selected Restore Execution Gate.

The draft summarizes the selected manifest, restorable entry counts, restorable bytes, required confirmation text, readiness blockers, and execution availability. The gate combines that draft with typed confirmation text and implementation availability. The required confirmation text is `RESTORE`.

In this packet, WPF must pass selected restore execution as unavailable. Therefore, the gate can show whether the exact `RESTORE` text matches, but it must keep execution closed and must not expose a restore button.

## Options considered

### Option A: Execute selected old-manifest restore immediately

Pros:

- Faster path to broad Undo Quarantine.

Cons:

- Combines selected readiness, confirmation, restore movement, manifest writes, stale-state handling, and recovery review in one packet.
- Risks restoring older manifests before the selected restore gate is proven.

### Option B: Read-only selected restore confirmation gate

Pros:

- Proves confirmation semantics before any old-manifest restore action exists.
- Keeps selection/readiness/confirmation separate from execution.
- Gives WPF and tests a clear place to verify exact confirmation text and blockers.

Cons:

- Adds another read-only step before selected restore execution is available.
- The visible app will still not restore older manifests after this packet.

### Option C: Reuse Quarantine Execution Gate directly

Pros:

- Less new model code.

Cons:

- Quarantine and restore have different confirmation text, counts, blockers, and user intent.
- Reusing the same gate would blur file-moving directions and increase naming confusion.

## Why this decision

Read-only Selected Restore Confirmation Gate advances the undo workflow while preserving the project rule that file-moving actions require explicit confirmation and fixture-first proof. It establishes the selected restore contract without allowing any discovered old-manifest restore execution yet.

## Consequences

Positive consequences:

- Future selected restore execution can consume a focused confirmation/gate model.
- WPF can show that `RESTORE` must be typed before selected restore execution can ever open.
- Tests can prove exact-confirmation behavior without moving files.

Negative consequences:

- The selected manifest area gains another read-only status pane.
- Future selected restore execution must still recompute readiness immediately before moving files because the filesystem can change after preview.

## Reversal cost

Low before selected old-manifest restore execution depends on this gate. Later, changing the confirmation phrase or blocker semantics would require WPF wording and test updates.

## Follow-up work

- Add fixture-first selected restore execution after the gate is verified.
- Recompute selected readiness immediately before any future restore movement.
- Decide whether selected restore execution should require zero blocked, recovery-review, and not-moved readiness rows.
- Decide whether successful restore should offer empty action-folder cleanup.

## Supersedes

- None.

## Superseded by

- ADR 0015 supersedes the WPF execution-availability part for fixture Restore Manifests only. The selected restore confirmation draft and gate model remain in use.
