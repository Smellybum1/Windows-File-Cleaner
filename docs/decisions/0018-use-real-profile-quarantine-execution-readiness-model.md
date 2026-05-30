# ADR 0018: Use Real-Profile Quarantine Execution Readiness Model

Date: 2026-05-31
Status: proposed
Owner: project-owner

## Context

ADR 0017 keeps real-profile WPF Quarantine execution unavailable until the app has a richer readiness contract. The current WPF gate is intentionally fixture-first: a clean Quarantine Preview, matching Restore Manifest Draft, exact `QUARANTINE`, and `IsExecutionImplemented` flag can open execution only for recognized synthetic fixture Cleanup Scopes.

That shape is good proof for fixture movement, but it is too coarse for real-profile movement from `C:\Users\moxhe`. A real-profile cleanup action needs to prove more than "the preview looked clean when it was generated." It must re-check the live filesystem immediately before moving files, prove the Quarantine Root is safe for execution, preserve recovery evidence, and make Undo Quarantine trustworthy enough before the first forward action is exposed.

This ADR does not enable real-profile movement. It records the proposed model future implementation should build before any real-profile Quarantine execution can be considered.

## Decision

Introduce a composite Real-Profile Quarantine Execution Readiness model before enabling real-profile WPF Quarantine execution.

Future real-profile `CanExecute` must not be unlocked by flipping the current fixture-only `IsExecutionImplemented` flag. It must require all of these readiness dimensions to pass in the same execution attempt:

- Scope eligibility: the Cleanup Scope is a recognized real-profile scope such as `C:\Users\moxhe`. Custom non-fixture scopes remain preview-only for the first real-profile phase unless a later ADR includes them.
- Review readiness: the Review Shortlist, Quarantine Preview, Restore Manifest Draft, Quarantine Confirmation Draft, and Quarantine Action Draft agree and have no blocked, redundant, stale, high-risk, protected, inaccessible, reparse-point, outside-scope, or destination-mismatch blockers.
- Quarantine Root Execution Safety: the chosen Quarantine Root is fully qualified, execution-eligible, on the preferred `D:` storage for the first real-profile phase, outside the Cleanup Scope, not a parent of the Cleanup Scope, has enough free space for the planned move plus manifest overhead, and has no action-root or item destination collision.
- Pre-Execution Revalidation: immediately before executing, the app re-checks every included source and planned destination against the live filesystem and blocks movement if the evidence no longer matches the approved preview/action draft.
- Recovery readiness: real-profile Restore Manifest writing, manifest failure handling, recovery-review wording, and at least selected-manifest real-profile Undo Quarantine readiness are designed and tested before exposing forward real-profile movement.
- Explicit real-profile approval: exact `QUARANTINE` alone is not enough for real-profile movement. The UI must require a real-profile-specific approval step or phrase after readiness is clean.

The first implementation packets should build this model and its tests while keeping the WPF real-profile button disabled. Only a later explicit user-approved packet may wire real-profile file movement.

Permanent deletion, persisted cleanup history, real-profile all-manifest restore, and custom non-fixture execution stay outside this decision.

## Options considered

### Option A: Flip the existing fixture-only execution flag for real-profile scopes

Pros:

- Smallest code change.
- Reuses the current Quarantine Preview, confirmation draft, and execution gate path.

Cons:

- Does not satisfy ADR 0017.
- Cannot name which real-profile readiness requirement is missing.
- Does not prove the filesystem still matches the preview immediately before movement.
- Does not distinguish preview-root safety from execution-root safety.
- Treats exact `QUARANTINE` as sufficient real-profile approval.

### Option B: Add a composite readiness model and keep real-profile movement blocked until every dimension passes

Pros:

- Makes the safety contract explicit and testable.
- Gives WPF a clear way to show why real-profile execution is unavailable.
- Keeps fixture proof while adding real-profile-specific validation.
- Creates seams for root safety, revalidation, recovery, and approval tests.

Cons:

- More code and documentation before the first real-profile move.
- Requires new UI wording for readiness states.
- Delays reclaiming real-profile space.

### Option C: Implement real-profile Undo Quarantine first, then revisit forward Quarantine

Pros:

- Improves recovery confidence before any forward movement.
- Exercises manifest discovery and selected restore paths.

Cons:

- Still does not define the forward execution gate.
- Could create restore capability for scenarios the app cannot yet create safely.
- Leaves Quarantine Root execution safety and pre-execution revalidation unresolved.

## Why this decision

Option B best matches the local-first safety goal. It turns the current fixture-only boundary into an implementation shape instead of a single boolean. It also lets future packets advance safely: each readiness dimension can be added and tested without crossing the real-profile file-movement boundary.

## Consequences

Positive consequences:

- Real-profile movement remains unavailable until the app can explain and test every readiness blocker.
- The UI can show precise "not ready because..." messages instead of a generic disabled button.
- Fixture execution can be migrated onto the same model, reducing the chance that fixture and real-profile gates drift.
- Future implementation can test root safety and revalidation without moving real files.

Negative consequences:

- The first real-profile cleanup action requires several preparatory packets.
- Existing gate builders will need a careful refactor from `IsExecutionImplemented` toward a named readiness result.
- The user will have more readiness information to review before execution.

## Reversal cost

Medium. If this model proves too heavy, reverting would require simplifying the execution gate, WPF readiness output, and tests. The reversal should still keep ADR 0017's core safety requirements: immediate revalidation, root safety, recovery readiness, and explicit approval.

## Follow-up work

- Add a core `QuarantineExecutionReadiness` model that represents fixture, real-profile, and custom non-fixture scope eligibility without enabling new movement.
- Add `QuarantineRootExecutionSafety` checks separate from the existing preview-only Quarantine Root Safety Note.
- Add Pre-Execution Revalidation tests over synthetic fixtures that simulate missing, changed, reparse, destination-collision, stale-preview, and action-collision cases.
- Add WPF readiness output that names missing real-profile prerequisites while keeping the execution button disabled.
- Design and test real-profile selected-manifest Undo Quarantine readiness before any forward real-profile execution.
- Ask the project owner to decide the first real-profile batch cap, real-profile approval phrase, and whether non-`D:` quarantine roots are blocked or require an explicit override in the first real-profile phase.

## Supersedes

- None.

## Superseded by

- None.
