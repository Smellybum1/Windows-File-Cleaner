# ADR 0015: Use Fixture-Only Selected Restore Execution

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

The WPF app can already undo the current in-memory fixture Quarantine execution. It can also discover older action-scoped Restore Manifests, select one, preview its restore readiness, and preview an exact `RESTORE` confirmation gate. The next step is to prove selected old-manifest restore execution without enabling real-profile restore.

Selected restore execution is safety-sensitive because it moves quarantined files or folders back to their original paths and writes updated Restore Manifest state. The first visible selected restore execution path must therefore be limited to synthetic fixture Cleanup Scopes and must reuse the core Undo Quarantine Executor rather than implementing movement in WPF.

Constraints:

- Real-profile WPF Quarantine execution remains unavailable.
- Real-profile WPF Undo Quarantine remains unavailable.
- Custom non-fixture selected restore execution remains unavailable.
- The selected Restore Manifest must pass Selected Restore Manifest Review, Selected Restore Confirmation Draft, and Selected Restore Execution Gate.
- WPF must not implement restore movement directly.

## Decision

Add WPF fixture-only selected restore execution for discovered Restore Manifests.

The selected restore execution button can open only when:

- A selected Restore Manifest has read-only readiness output.
- The Selected Restore Confirmation Draft has no data blockers.
- The selected Restore Manifest cleanup scope is recognized as a fixture Cleanup Scope.
- The exact `RESTORE` confirmation text is entered.
- No selected restore attempt has already run for the current selected manifest review.

When the gate opens, WPF calls `UndoQuarantineExecutor.Undo` for the selected discovered Restore Manifest and shows the result. Real-profile and custom non-fixture manifests remain blocked.

## Options considered

### Option A: Enable selected restore for all discovered manifests

Pros:

- Directly supports the user's desired broad undo path.

Cons:

- Moves files in real-profile paths before fixture-selected restore is proven.
- Skips the project's fixture-first safety ladder.

### Option B: Fixture-only selected restore execution

Pros:

- Proves the visible selected old-manifest restore path with synthetic files.
- Reuses the fixture-tested core Undo Quarantine Executor.
- Keeps real-profile and custom non-fixture restore unavailable.

Cons:

- The app still cannot restore real-profile discovered manifests.
- The UI gains another fixture-only action and stale-state message.

### Option C: Keep selected restore as a read-only gate

Pros:

- Lowest immediate risk.

Cons:

- Does not exercise the selected old-manifest restore path.
- Leaves the undo workflow short of a visible fixture proof.

## Why this decision

Fixture-only selected restore execution advances toward broad Undo Quarantine while preserving the project's safest execution pattern: prove file-moving behavior with synthetic fixtures before any real-profile workflow can move files.

## Consequences

Positive consequences:

- WPF can restore a discovered older fixture manifest after exact `RESTORE` confirmation.
- The selected restore workflow now has an end-to-end fixture proof.
- Real-profile discovered manifests remain blocked.

Negative consequences:

- Selected restore execution writes Restore Manifest updates and moves fixture files.
- Future real-profile restore still needs additional design, manual review, and stale-state handling.

## Reversal cost

Medium. Removing fixture-only selected restore would require WPF and smoke-test changes, but no user data migration because the workflow is limited to synthetic fixture paths.

## Follow-up work

- Add real-profile selected restore design only after manual fixture review.
- Decide final stale-state wording and rescan behavior after selected restore.
- Decide whether successful selected restore should offer empty action-folder cleanup.

## Supersedes

- ADR 0014's temporary rule that WPF selected restore execution is always unavailable. The selected restore confirmation draft and gate remain in use.

## Superseded by

- None.
