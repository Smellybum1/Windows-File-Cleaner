# ADR 0013: Use Read-Only Selected Restore Manifest Review

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

Quarantine Manifest Discovery can find older action-scoped Restore Manifests, and Restore Readiness Preview can evaluate all discovered manifests under a Quarantine Root. The next step toward broad Undo Quarantine is choosing one discovered Restore Manifest and inspecting its readiness in isolation.

Selection is safety-sensitive because it will eventually feed a restore action. The first selected-manifest workflow must prove selection semantics without moving files, creating folders, writing manifests, deleting quarantine folders, or exposing restore execution for older manifests.

Constraints:

- Real-profile WPF Quarantine execution remains unavailable.
- Broad WPF Undo Quarantine remains unavailable.
- Selected-manifest review must use the current read-only Quarantine Manifest Discovery result.
- Selected-manifest readiness must be recomputed from the selected Restore Manifest.
- Restore execution for selected manifests remains a separate decision.

## Decision

Add a read-only Selected Restore Manifest Review that lets the WPF app select one discovered Restore Manifest and preview readiness for that manifest only.

The selected review must not call `UndoQuarantineExecutor.Undo`, write manifests, create folders, move files, delete files, or clean up quarantine folders. It may reuse Restore Readiness Preview logic for the selected manifest and may report selection issues when no discovery exists, no manifest is selected, or the selected path is not part of the current discovery result.

## Options considered

### Option A: Restore the selected manifest immediately

Pros:

- Faster path to broad Undo Quarantine.

Cons:

- Combines selection, confirmation, readiness, restore movement, manifest writes, and recovery review before the selected-manifest review path is proven.
- Raises the risk of restoring the wrong action or stale filesystem state.

### Option B: Read-only selected manifest review

Pros:

- Proves one-manifest selection before any old-manifest restore action exists.
- Keeps selection separate from approval and execution.
- Lets tests prove the app previews only the selected manifest and leaves files untouched.

Cons:

- Adds another read-only step before actual broad undo is available.
- Requires clear UI wording so selection is not mistaken for restore approval.

### Option C: Keep all-manifest readiness only

Pros:

- No new UI control.

Cons:

- Does not establish which Restore Manifest a future restore would act on.
- Makes future broad Undo Quarantine harder to gate safely.

## Why this decision

Selected Restore Manifest Review is the smallest safe bridge from read-only discovery/readiness toward future broad Undo Quarantine. It adds the durable selection concept while preserving the project rule that file-moving actions must come only after explicit design, confirmation, and fixture-backed verification.

## Consequences

Positive consequences:

- The app can focus readiness output on one discovered Restore Manifest.
- Future restore confirmation can depend on an explicit selected manifest.
- Selection issues become visible without filesystem mutation.

Negative consequences:

- The WPF review pane has one more read-only control.
- The selected readiness result can become stale if files change after preview, so future restore execution must recompute readiness immediately before moving files.

## Reversal cost

Low before broad WPF Undo Quarantine depends on selected-manifest review. Later, changing selection semantics may require WPF test updates and user-facing wording changes.

## Follow-up work

- Add explicit confirmation and restore execution for selected discovered manifests after fixture-first selected restore is designed.
- Decide stale-state handling between selected readiness preview and future restore execution.
- Decide whether successful restore should offer empty action-folder cleanup.

## Supersedes

- None.

## Superseded by

- None.
