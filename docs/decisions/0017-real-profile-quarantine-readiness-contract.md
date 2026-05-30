# ADR 0017: Use Real-Profile Quarantine Readiness Contract

Date: 2026-05-30
Status: accepted
Owner: project-owner

## Context

The app now has fixture-only WPF Quarantine execution, current-fixture undo, fixture-only selected restore, and read-only manifest discovery/readiness surfaces. Those prove the UI can call the core executors, but they do not make real-profile movement safe.

The tempting next change would be to let real-profile or custom Cleanup Scopes set the current execution-availability flag to true after a clean Quarantine Preview and exact `QUARANTINE`. That would skip several safety questions that matter before moving files from `C:\Users\moxhe`.

Constraints:

- Storage Scan remains read-only.
- Real-profile WPF Quarantine execution remains unavailable until the user explicitly asks for it after a Grill with Docs pass.
- Real-profile WPF Undo Quarantine and selected real-profile restore remain unavailable.
- Permanent deletion and persisted cleanup history remain unavailable.
- The app must preserve write-ahead Restore Manifest recovery evidence before any future real-profile move.

## Decision

Require a Real-Profile Quarantine Readiness Contract before enabling real-profile WPF Quarantine execution.

Real-profile WPF Quarantine must remain unavailable until a future design explicitly covers:

- a richer execution availability/readiness model than the current fixture-only boolean,
- immediate pre-execution revalidation of scan rows, preview rows, source paths, destination paths, risk blockers, overlap blockers, and stale state,
- Quarantine Root safety checks, including fully qualified path, preferred `D:` destination, not inside the Cleanup Scope, capacity/error handling, and destination collision handling,
- trusted Undo Quarantine and recovery-review behavior for real-profile manifests before movement is exposed,
- clear user approval semantics beyond Review Shortlist, Quarantine Preview, and exact `QUARANTINE`,
- visible failure/recovery guidance for `Moving`, `Failed`, `Restoring`, `RestoreFailed`, partial manifests, and manifest write failures.

Until that contract is implemented and reviewed, WPF can execute Quarantine only for recognized fixture Cleanup Scopes. Real-profile and custom non-fixture Cleanup Scopes remain preview-only even when Quarantine Preview is clean and exact `QUARANTINE` is typed.

## Options considered

### Option A: Enable real-profile execution by reusing the current boolean

Pros:

- Small code change.
- Fastest route to moving real files.

Cons:

- Treats clean preview plus exact text as enough for real-profile movement.
- Skips immediate stale-state and filesystem revalidation design.
- Leaves Undo Quarantine and recovery UX behind the riskier forward action.
- Makes it easier to accidentally move real-profile files without a new safety review.

### Option B: Keep fixture-only execution and add a readiness contract

Pros:

- Preserves the current safety boundary while still advancing full-app readiness.
- Names the future prerequisites that must be implemented before real movement.
- Gives tests and docs a stable target for preventing accidental real-profile execution.
- Keeps permanent deletion and cleanup history out of the path until reversible cleanup is trusted.

Cons:

- The app still cannot reclaim real-profile storage through WPF Quarantine yet.
- Adds another design gate before the user gets real cleanup execution.

### Option C: Block all WPF execution again

Pros:

- Simplest safety story.

Cons:

- Loses useful fixture proof already covered by tests and manual review.
- Does not advance the real-profile readiness story.
- Would remove the current reversible fixture workflow without reducing the future design work.

## Why this decision

The safest next step is to preserve fixture proof while making the real-profile threshold explicit. Moving real-profile files is a higher-risk core UX and recovery decision than fixture movement; it should not be enabled by flipping the existing fixture-only gate.

## Consequences

Positive consequences:

- Future real-profile work has a checklist of prerequisites instead of an implicit "turn it on" path.
- Regression tests can guard the current real/custom preview-only boundary.
- Undo Quarantine, recovery review, and Quarantine Root safety remain first-class requirements.

Negative consequences:

- Real-profile Quarantine execution remains unavailable for at least one more design and implementation packet.
- Future code will need a richer model than the current `IsExecutionImplemented` boolean.
- Some UI wording may need to evolve when real-profile readiness states become more granular.

## Reversal cost

Medium. Reversing this decision would mean either enabling real-profile movement without the readiness contract or replacing the contract with a different durable gate. After real-profile manifests exist, reversal would also need migration and recovery compatibility review.

## Follow-up work

- Design the richer real-profile execution availability/readiness contract.
- Add immediate pre-execution revalidation for real-profile Quarantine.
- Add real-profile Undo Quarantine/readiness/recovery UX before or alongside forward movement.
- Decide whether real-profile execution requires a fresh preflight run or user acknowledgement per execution session.
- Keep permanent deletion deferred until real-profile Quarantine and Undo are trusted.

## Supersedes

- None.

## Superseded by

- None.
