# ADR 0019: Use Real-Profile Selected Restore Execution Contract

Date: 2026-05-31
Status: accepted
Owner: project-owner

## Context

ADR 0018 requires selected-manifest real-profile Undo Quarantine readiness before forward real-profile Quarantine movement. The app now has the pieces that make this design possible without enabling real-profile file movement:

- read-only Quarantine Manifest Discovery,
- read-only Selected Restore Manifest Review,
- Selected Restore Confirmation Draft and Selected Restore Execution Gate with exact `RESTORE`,
- fixture-only Selected Restore Execution through `UndoQuarantineExecutor`,
- read-only Real-Profile Restore Readiness evidence consumed by Real-Profile Quarantine Execution Readiness.

The tempting next implementation would be to let selected real-profile Restore Manifests restore as soon as selected readiness is clean and the user types `RESTORE`. That is still too broad unless the app records the execution contract first. Restore movement writes manifest state and can recreate folders or move files back into `C:\Users\moxhe`; it needs immediate revalidation, collision blockers, recovery guidance, and a narrow selected-manifest scope.

Constraints:

- Real-profile WPF Quarantine execution remains unavailable.
- Real-profile selected restore execution remains unavailable in this ADR.
- All-manifest real-profile restore remains unavailable.
- Permanent deletion and persisted cleanup history remain unavailable.
- The first real-profile restore path should be a recovery prerequisite for later forward Quarantine, not a broad cleanup history feature.

## Decision

Require a Real-Profile Selected Restore Execution contract before WPF can restore selected discovered real-profile Restore Manifests.

The future first real-profile selected restore implementation must:

- act on exactly one selected Restore Manifest at a time,
- allow only Restore Manifests whose Cleanup Scope is exactly `C:\Users\moxhe`,
- require a fresh Quarantine Manifest Discovery result and a fresh Selected Restore Manifest Review for the selected manifest,
- recompute Restore Readiness Preview, Selected Restore Confirmation Draft, and Selected Restore Execution Gate immediately before execution,
- require exact `RESTORE` typed confirmation in the same selected-manifest context,
- require the selected manifest to have restorable `Moved` entries and no blocked, recovery-review, not-moved, already-restored-only, or manifest-level blockers,
- refuse to overwrite any original path that exists at execution time,
- refuse execution when a quarantine source path is missing, changed into an unsafe state, inaccessible, or outside the selected manifest's action layout,
- use `UndoQuarantineExecutor` for movement and manifest updates rather than implementing restore movement in WPF,
- keep Restore Manifest as the only durable cleanup/recovery record for the first real-profile phase,
- show result and stale-state guidance that asks the user to rediscover manifests and rescan manually after a restore attempt,
- avoid empty action-folder cleanup in the first real-profile phase,
- avoid all-manifest restore, permanent deletion, and persisted cleanup history.

This ADR is design-only. WPF must continue to keep selected restore execution unavailable for real-profile and custom non-fixture manifests until a later implementation packet adds tests and the user explicitly approves crossing the real-profile restore boundary.

## Options considered

### Option A: Enable selected real-profile restore from the existing fixture path

Pros:

- Small implementation change.
- Reuses selected restore gate and `UndoQuarantineExecutor`.

Cons:

- Skips an explicit real-profile restore contract.
- Risks treating clean readiness plus `RESTORE` as sufficient without immediate execution-time checks.
- Does not record selected-only, original-path collision, all-manifest, action-folder cleanup, or stale-state rules.

### Option B: Record a selected real-profile restore contract before implementation

Pros:

- Preserves the safety ladder while advancing toward real recovery.
- Makes selected-manifest scope, exact `RESTORE`, execution-time revalidation, and no-overwrite behavior explicit.
- Keeps future forward Quarantine tied to trusted recovery behavior.
- Gives tests and WPF wording a clear target for the implementation packet.

Cons:

- Adds another docs/design packet before visible real-profile restore is available.
- Still leaves real-profile cleanup and restore unavailable in the app.

### Option C: Implement all-manifest real-profile restore first

Pros:

- Broadest recovery surface.
- Could restore every action under a Quarantine Root in one pass.

Cons:

- Too wide for the first real-profile movement path.
- Increases stale-state, collision, partial restore, and wrong-manifest risk.
- Conflicts with the user's current preference for Restore Manifest-only selected recovery.

## Why this decision

Option B matches the local-first safety model. It advances the recovery side of the app without enabling movement yet, and it keeps the first real-profile restore path intentionally narrower than broad Undo Quarantine or cleanup history.

## Consequences

Positive consequences:

- Future implementation can add real-profile selected restore with clear testable blockers.
- Forward real-profile Quarantine remains blocked until recovery behavior is trustworthy.
- All-manifest restore and permanent deletion stay out of the first real-profile path.

Negative consequences:

- The visible app still cannot restore selected real-profile manifests after this ADR.
- The first real-profile restore implementation must recompute readiness and may block often when filesystem state has changed.
- Result wording must be careful so selected restore does not look like broad history or cleanup approval.

## Reversal cost

Medium. A later ADR could choose all-manifest restore or a richer cleanup history, but future implementations that follow this contract will likely add WPF tests and wording around selected-only restore, exact `RESTORE`, and execution-time revalidation.

## Follow-up work

- Add core and WPF tests proving real-profile selected restore remains unavailable until the implementation packet.
- Implement Real-Profile Selected Restore Execution as a narrow selected-manifest path after explicit user approval.
- Wire immediate selected-restore revalidation into the WPF execution path before calling `UndoQuarantineExecutor`.
- Decide later whether all-manifest real-profile restore or cleanup history is needed.

## Supersedes

- None.

## Superseded by

- None.
