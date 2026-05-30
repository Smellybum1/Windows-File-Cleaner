# ADR 0018: Use Real-Profile Quarantine Execution Readiness Model

Date: 2026-05-31
Status: accepted
Owner: project-owner

## Context

ADR 0017 keeps real-profile WPF Quarantine execution unavailable until the app has a richer readiness contract. The current WPF gate is intentionally fixture-first: a clean Quarantine Preview, matching Restore Manifest Draft, exact `QUARANTINE`, and `IsExecutionImplemented` flag can open execution only for recognized synthetic fixture Cleanup Scopes.

That shape is good proof for fixture movement, but it is too coarse for real-profile movement from `C:\Users\moxhe`. A real-profile cleanup action needs to prove more than "the preview looked clean when it was generated." It must re-check the live filesystem immediately before moving files, prove the Quarantine Root is safe for execution, preserve recovery evidence, and make Undo Quarantine trustworthy enough before the first forward action is exposed.

This ADR does not enable real-profile movement. It records the accepted model future implementation must build before any real-profile Quarantine execution can be considered.

## Decision

Introduce a composite Real-Profile Quarantine Execution Readiness model before enabling real-profile WPF Quarantine execution.

Future real-profile `CanExecute` must not be unlocked by flipping the current fixture-only `IsExecutionImplemented` flag. It must require all of these readiness dimensions to pass in the same execution attempt:

- Scope eligibility: the first real-profile phase is limited to the exact recognized Cleanup Scope `C:\Users\moxhe`. Child scopes, `ProgramData`, `Program Files`, and custom non-fixture scopes remain preview-only unless a later design includes them. Future `ProgramData` support should be a separate cautious mode; future `Program Files` support should prefer report/uninstall guidance before file movement.
- Review readiness: the Review Shortlist, Quarantine Preview, Restore Manifest Draft, Quarantine Confirmation Draft, and Quarantine Action Draft agree and have no blocked, redundant, stale, high-risk, protected, inaccessible, reparse-point, outside-scope, no-category, or destination-mismatch blockers. First-phase real-profile execution is limited to `Likely safe` rows with `Quarantine candidate` recommendation.
- Batch limits: a single first-phase real-profile Quarantine action is capped at 10 included rows and 1 GB total previewed size.
- Folder eligibility: files are allowed, and folders are allowed only when they are narrow, `Likely safe`, `Quarantine candidate`, readable, and strict descendant checks find no protected, high-risk, inaccessible, reparse-point, no-category, outside-scope, or otherwise blocked descendants.
- Quarantine Root Execution Safety: the chosen Quarantine Root is fully qualified, execution-eligible, outside the Cleanup Scope, not a parent of the Cleanup Scope, has enough free space for the planned move plus manifest overhead, and has no action-root or item destination collision. `D:` remains the default/preferred root. Non-`D:` roots are allowed only with an extra acknowledgement after all other root safety checks pass. Unsafe roots are blocked with no override.
- Pre-Execution Revalidation: immediately before executing, the app re-checks every included source and planned destination against the live filesystem and blocks movement if the evidence no longer matches the approved preview/action draft.
- Recovery readiness: real-profile Restore Manifest writing, manifest failure handling, recovery-review wording, and at least selected-manifest real-profile Undo Quarantine readiness are designed and tested before exposing forward real-profile movement.
- Explicit confirmation and approval: the typed confirmation phrase remains exact `QUARANTINE` for real-profile and fixture workflows. The phrase is necessary but not sufficient; every readiness dimension must still pass before future real-profile movement can be enabled.
- Post-execution scan behavior: after a future real-profile Quarantine action, the app should show stale-scan guidance and ask the user to rescan manually rather than auto-rescanning.
- Durable record: Restore Manifest remains the only durable cleanup record for the first real-profile phase. Persisted cleanup history remains unavailable.

The first implementation packets should build this model and its tests while keeping the WPF real-profile button disabled. Only a later explicit user-approved packet may wire real-profile file movement.

Permanent deletion, persisted cleanup history, real-profile all-manifest restore, `ProgramData` execution, `Program Files` execution, and custom non-fixture execution stay outside this decision.

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
- Non-`D:` roots add one more acknowledgement path to design and test.

## Reversal cost

Medium. If this model proves too heavy, reverting would require simplifying the execution gate, WPF readiness output, and tests. The reversal should still keep ADR 0017's core safety requirements: immediate revalidation, root safety, recovery readiness, and explicit approval.

## Follow-up work

- Add a core `QuarantineExecutionReadiness` model that represents fixture, real-profile, and custom non-fixture scope eligibility without enabling new movement.
- Add `QuarantineRootExecutionSafety` checks separate from the existing preview-only Quarantine Root Safety Note. Core model added 2026-05-31; WPF remains unwired.
- Add Pre-Execution Revalidation tests over synthetic fixtures that simulate missing, changed, destination-collision, stale-preview/action mismatch, and action-collision cases. Core model added 2026-05-31; WPF remains unwired.
- Add Real-Profile Restore Readiness for selected-manifest Undo Quarantine evidence before forward movement. Core model added 2026-05-31; WPF remains unwired and selected real-profile restore remains unavailable.
- Add WPF readiness output that names missing real-profile prerequisites while keeping the execution button disabled.
- Design and test real-profile selected-manifest Undo Quarantine execution before any forward real-profile execution.
- Keep the real-profile confirmation phrase as `QUARANTINE` while preserving readiness blockers that make the phrase insufficient by itself.

## Supersedes

- None.

## Superseded by

- None.
