# ADR 0011: Use Read-Only Quarantine Manifest Discovery

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

The WPF app can now execute and undo a Quarantine action for the current synthetic fixture execution. It still cannot discover older Restore Manifests from disk.

Manifest discovery is safety-sensitive because it creates the path toward broad Undo Quarantine. Before any old-manifest restore action exists, the app needs a read-only way to show what manifests are present, what state they are in, and whether they require recovery review.

Constraints:

- Real-profile WPF Quarantine execution remains unavailable.
- Broad WPF Undo Quarantine remains unavailable.
- Discovery must not restore, move, delete, create, or clean up files or folders.
- Discovery should be limited to action-scoped `restore-manifest.json` files below the selected Quarantine Root.
- Invalid, unreadable, or unsupported manifest files should be reported as discovery issues instead of crashing the app.

## Decision

Add a read-only Quarantine Manifest Discovery component that scans the selected Quarantine Root's `actions` folder for action-scoped `restore-manifest.json` files and returns compact summaries plus discovery issues.

Expose the discovery result in WPF as a read-only review pane/button. The UI may show discovered manifest counts, status, action ids, manifest paths, entry counts, total size, and recovery-review flags. It must not expose an old-manifest restore button in this packet.

## Options considered

### Option A: Keep old manifest discovery unavailable

Pros:

- No new file-reading path.

Cons:

- Undo remains limited to the current in-memory fixture execution.
- The user cannot inspect existing Restore Manifests after restarting the app.

### Option B: Read-only Quarantine Manifest Discovery

Pros:

- Builds recovery visibility before adding restore execution.
- Keeps the change bounded to file reads and status summaries.
- Provides a safer foundation for future manifest picker and broad Undo Quarantine.

Cons:

- Does not restore old manifests yet.
- Needs clear wording so discovery is not mistaken for cleanup history or approval.

### Option C: Full manifest picker with restore

Pros:

- Closer to the eventual broad Undo Quarantine workflow.

Cons:

- Mixes discovery, selection, restore execution, stale-state handling, and recovery review in one larger safety-sensitive packet.
- Enlarges the risk surface before read-only discovery is proven.

## Why this decision

Read-only discovery advances the undo/recovery workflow while preserving the project's fixture-first and review-before-action safety model.

## Consequences

Positive consequences:

- The app can inspect older action-scoped Restore Manifests without moving files.
- Future broad WPF Undo Quarantine can build on tested discovery results.
- Recovery-review states become visible outside the current execution session.

Negative consequences:

- The app gains another filesystem read path that must stay bounded.
- The first discovery UI may be status-only until restore selection is designed.

## Reversal cost

Low before broad WPF Undo Quarantine depends on this shape.

## Follow-up work

- Add manifest selection and old-manifest restore only after read-only discovery is verified.
- Decide whether discovery should support explicit custom manifest-file selection.
- Decide whether successful undo should offer empty action-folder cleanup.

## Supersedes

- None.

## Superseded by

- None.
