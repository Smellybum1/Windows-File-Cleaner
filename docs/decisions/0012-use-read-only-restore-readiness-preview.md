# ADR 0012: Use Read-Only Restore Readiness Preview

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

The app can discover action-scoped Restore Manifests under the selected Quarantine Root, but discovered manifests are currently status-only. The next safety step before broad WPF Undo Quarantine is to show whether discovered manifests look restorable without actually restoring them.

Restore readiness is safety-sensitive because it checks both quarantine paths and original paths. It must not create original parent folders, move files, restore files, delete quarantine folders, or write updated manifests.

Constraints:

- Real-profile WPF Quarantine execution remains unavailable.
- Broad WPF Undo Quarantine remains unavailable.
- Restore readiness must be read-only.
- Restore readiness should build on action-scoped Quarantine Manifest Discovery.
- Restore execution for discovered manifests remains a separate decision.

## Decision

Add a read-only Restore Readiness Preview that evaluates discovered Restore Manifests and reports restorable entries, blocked entries, skipped entries, manifest blockers, and discovery issues.

The preview may check whether quarantine paths still exist, whether original paths already exist, whether quarantine paths became reparse points, and whether manifest path relationships still look valid. It must not call `UndoQuarantineExecutor.Undo` or any file-moving/write APIs.

Expose the result in WPF as status-only text. Do not add old-manifest restore selection or execution in this packet.

## Options considered

### Option A: Go straight to old-manifest restore

Pros:

- Faster path to broad Undo Quarantine.

Cons:

- Adds restore execution before the app can show readiness blockers.
- Mixes selection, confirmation, restore movement, manifest writes, and recovery review in one packet.

### Option B: Read-only Restore Readiness Preview

Pros:

- Shows restore blockers before any old-manifest restore execution exists.
- Keeps the workflow consistent with preview-before-action.
- Adds a testable bridge from discovery to future restore selection.

Cons:

- Does not restore discovered manifests yet.
- Adds another read-only status pane that needs clear wording.

### Option C: Preview only the newest manifest

Pros:

- Simpler UI.

Cons:

- Hides recovery issues in older manifests.
- Makes future selection semantics harder to reason about.

## Why this decision

Read-only Restore Readiness Preview advances the undo/recovery workflow while preserving the project's safest pattern: inspect and preview before any action that modifies files.

## Consequences

Positive consequences:

- The app can show whether discovered manifests appear restorable before enabling restore.
- Future broad WPF Undo Quarantine can reuse the readiness semantics.
- Original-path collisions and missing quarantine files become visible without changing files.

Negative consequences:

- The preview reads filesystem metadata for quarantine and original paths.
- A later packet still needs manifest selection, confirmation, restore execution, and recovery-review UI.

## Reversal cost

Low before broad WPF Undo Quarantine depends on this preview shape.

## Follow-up work

- Add manifest selection for one discovered Restore Manifest.
- Add explicit confirmation and restore execution for selected manifests only after readiness preview is verified.
- Decide whether successful restore should offer empty action-folder cleanup.

## Supersedes

- None.

## Superseded by

- None.
