# ADR 0009: Use Fixture-Only WPF Quarantine Execution

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

The core Quarantine Executor and Undo Quarantine Executor are fixture-tested. The visible WPF app still keeps `Execute quarantine` disabled for every Cleanup Scope.

The project needs to prove the visible UI can pass a reviewed Quarantine Preview into the core executor, write a Restore Manifest, and move files. That proof should happen before real-profile execution is enabled.

Constraints:

- Real-profile WPF execution must remain unavailable in this packet.
- Fixture execution must still require Quarantine Preview, Quarantine Confirmation Draft readiness, and exact `QUARANTINE` confirmation.
- The WPF app must not invent separate movement behavior; it should call the core `QuarantineExecutor`.
- After fixture execution, the app should treat the current scan/review state as stale and require a rescan before more cleanup review.
- Undo Quarantine UI remains a separate packet.

## Decision

Enable WPF Quarantine execution only for synthetic fixture Cleanup Scopes recognized by `CleanupScopeSafetyNoteBuilder`.

For fixture scopes, a clean Quarantine Preview can mark execution as implemented, allowing `QuarantineExecutionGate.CanExecute` to become true after the exact `QUARANTINE` text is entered. The WPF click handler should call `QuarantineExecutor.Execute` with the planned Restore Manifest already shown in the gate.

For real-profile and custom non-fixture scopes, `CanExecute` remains false and the gate should explain that WPF execution is not available for that Cleanup Scope in this build.

This ADR does not enable real-profile cleanup execution, WPF Undo Quarantine, permanent deletion, persistent cleanup history, or quarantine-folder cleanup.

## Options considered

### Option A: Keep WPF execution disabled everywhere

Pros:

- Preserves the strictest possible visible-app safety boundary.

Cons:

- Does not test the actual UI-to-executor path.
- Leaves the next real-profile step too large.

### Option B: Enable fixture-only WPF execution

Pros:

- Proves the visible app can execute a reviewed action through the tested core executor.
- Keeps real-profile files protected.
- Lets WPF smoke tests verify the end-to-end movement and manifest path.
- Makes the later real-profile execution packet smaller.

Cons:

- The visible app can move synthetic files after confirmation.
- Manual fixture review wording must be updated because "no files modified" is no longer true after fixture execution.
- The post-execution UI must clearly warn that scan/review state is stale.

### Option C: Enable WPF execution for all scopes now

Pros:

- Fastest path to user-visible cleanup.

Cons:

- Too much risk before WPF stale-state checks, real-profile copy, manifest discovery, and undo UI exist.
- Could move real user-profile files before the visible recovery workflow is ready.

## Why this decision

Fixture-only WPF execution is the smallest visible step that genuinely advances toward the requested cleanup MVP while preserving the project's safety discipline. It proves the UI path without making real-profile cleanup available yet.

## Consequences

Positive consequences:

- Automated WPF tests can verify visible execution against synthetic files.
- The app can show actual completed Restore Manifest evidence after execution.
- Real-profile execution remains blocked until a later packet explicitly designs it.

Negative consequences:

- WPF status wording must distinguish fixture execution from read-only preview.
- A fixture execution makes the current scan stale because the selected fixture paths moved.
- The app still cannot undo from WPF until a future manifest discovery/undo packet.

## Reversal cost

Low before real-profile execution is enabled. Reverting would disable the fixture-only gate and leave the core executors intact.

## Follow-up work

- Add WPF Undo Quarantine with manifest discovery and stale-state checks.
- Design real-profile WPF Quarantine execution only after fixture execution remains green and the user reviews the visible behavior.
- Add recovery UI for incomplete Moving, Failed, Restoring, and RestoreFailed manifests.

## Supersedes

- Implements the WPF execution follow-up from ADR 0007 while preserving ADR 0008's separate WPF undo follow-up.

## Superseded by

- None.
