# ADR 0005: Use Write-Ahead Restore Manifest

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

The app is approaching actual Quarantine execution. Before any file-moving code exists, the project needs one durable answer for manifest write order and partial-failure recovery.

Existing docs had a conflict:

- Domain context said to persist a restore manifest before moving files.
- ADR 0003 said a future executed Restore Manifest would be persisted after file moves are attempted.

Constraints:

- Quarantine must be reversible when feasible.
- The app must not move a file before it has durable recovery metadata.
- Partial failures should leave enough state to inspect what moved and what failed.
- Preview, draft, confirmation, and action-layout code must remain read-only.
- The first implementation should stay local, fixture-testable, and human-readable.

## Decision

Use a write-ahead Restore Manifest for Quarantine execution.

After explicit confirmation opens a future execution flow, the app should write a planned Restore Manifest to:

```txt
<quarantine-root>\actions\<action-id>\restore-manifest.json
```

before the first file or folder move.

The Restore Manifest records:

- schema version `restore-manifest.v1`
- manifest id
- Restore Manifest Draft id
- Quarantine Action id
- created and updated timestamps
- Cleanup Scope, Quarantine root, action root, items root, and manifest path
- overall Restore Manifest Action Status
- one entry per planned move
- original path
- action-scoped quarantine path
- file/folder type
- size and last modified time when known
- Importance Rating, Deletion Recommendation, Bloat Categories, and evidence
- per-entry Restore Manifest Entry Status
- move timestamps and failure message when applicable

Future execution should use this order:

1. Write the planned Restore Manifest with all entries in Planned status.
2. Before each move, update that entry to Moving and write the manifest again.
3. Attempt the move.
4. After each move attempt, update that entry to Moved or Failed and write the manifest again.
5. Set the overall action status to Completed, Partial failure, or Failed based on entry outcomes.

Undo Quarantine should restore entries recorded as Moved. Moving and Failed entries require recovery review because the app may need to inspect source and destination paths before acting.

This ADR does not implement file moves, folder creation, manifest writing, or Undo Quarantine.

## Options considered

### Option A: Write manifest only after all moves succeed

Pros:

- Simpler final manifest shape.
- Avoids repeated writes during execution.

Cons:

- A crash or failure after moving files but before writing the manifest can lose undo metadata.
- Partial failures are harder to diagnose.
- Violates the project's safety preference for recoverability before modification.

### Option B: Write manifest after moves are attempted

Pros:

- Records actual outcomes rather than planned outcomes.
- Avoids writing a manifest for an action that never moves anything.

Cons:

- Still leaves a dangerous gap before the first durable write.
- Makes crash recovery ambiguous.
- Conflicts with the domain rule to preserve restore metadata before moving files.

### Option C: Write-ahead manifest with per-entry status

Pros:

- Ensures recovery metadata exists before the first move.
- Supports partial-failure evidence without adding a database.
- Keeps the action-scoped manifest near the quarantined items.
- Can be modeled and tested before file-moving code exists.

Cons:

- Requires repeated manifest writes during execution.
- Requires recovery-review wording for Moving and Failed entries.
- Adds status fields that future Undo Quarantine must respect.

## Why this decision

Write-ahead manifesting is the safest local model for a cleanup app that will modify user-profile files. It favors recoverability over implementation simplicity and resolves the manifest timing conflict before file-moving code exists.

## Consequences

Positive consequences:

- Future Quarantine execution has a clear recovery contract.
- Partial failures can be represented without guessing from the filesystem alone.
- Undo Quarantine can focus on Moved entries first.
- The current app can show the planned write order while execution remains unavailable.

Negative consequences:

- The future executor must update the manifest repeatedly.
- Manifest writing itself becomes part of the failure surface.
- Recovery UI will need to explain Moving and Failed states carefully.

## Reversal cost

Moderate after the first executed manifest is written. Reversing later would require supporting or migrating existing manifests with action and entry status fields.

## Follow-up work

- Manifest file writing is covered by ADR 0006.
- Core fixture-first file-moving is covered by ADR 0007.
- Core fixture-first Undo Quarantine is covered by ADR 0008.
- Wire WPF execution only after stale-state and confirmation checks are explicit.
- Wire WPF Undo Quarantine only after manifest discovery, stale-state, confirmation, and recovery-review checks are explicit.

## Supersedes

- Refines the write-order follow-up in ADR 0003.
- Refines the execution follow-up in ADR 0004.

## Superseded by

- None.
