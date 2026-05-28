# ADR 0007: Use Fixture-First Quarantine Executor

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

The app now has:

- Quarantine Preview for read-only candidate review.
- Quarantine Action Draft for action-scoped item paths.
- Write-ahead Restore Manifest state.
- Restore Manifest File Store for writing `restore-manifest.json`.

The next MVP step is the first file-moving component. This is the first point where production code can modify selected files, so it needs a narrow boundary and fixture proof before any WPF execution wiring or real-profile use.

Constraints:

- Keep the WPF `Execute quarantine` button disabled until a separate UI wiring packet.
- Move only entries represented in a validated Restore Manifest.
- Write the planned Restore Manifest before any move.
- Update the manifest before and after each move attempt.
- Do not overwrite existing destination paths.
- Do not move sources that disappeared, are outside the Cleanup Scope, or became reparse points after preview.
- Keep scanner, preview, draft, confirmation, gate, and UI code blocked from cleanup-execution filesystem APIs.

## Decision

Add a narrow core `QuarantineExecutor` that executes a Restore Manifest against the local filesystem, but do not wire it to WPF yet.

The executor should:

1. Validate that manifest paths and entries remain within their allowed roots.
2. Require a Planned Restore Manifest.
3. Write the planned manifest before the first move.
4. For each entry:
   - write the entry as Moving,
   - verify source still exists and is not a reparse point,
   - verify destination does not already exist,
   - create only the destination parent folder,
   - move the file or directory,
   - write the entry as Moved or Failed.
5. Continue after per-entry failures so the final manifest can represent partial failure.
6. Return a `QuarantineExecutionResult` summarizing moved, failed, and manifest status.

The source-level filesystem-call regression test should allow move/create APIs only in `RestoreManifestFileStore` and `QuarantineExecutor`.

This ADR does not wire WPF execution, implement Undo Quarantine, permanent deletion, cleanup history, or real-profile automation.

## Options considered

### Option A: Wire WPF execution immediately

Pros:

- Faster path to a usable cleanup button.

Cons:

- Mixes UI confirmation, stale-preview checks, and file-moving risk in one packet.
- Harder to isolate fixture execution failures.
- Raises real-profile risk before core semantics are proven.

### Option B: Core fixture-first executor

Pros:

- Proves actual move semantics against synthetic files first.
- Keeps the UI gate closed while execution behavior matures.
- Keeps the filesystem-call allowlist narrow and auditable.
- Makes follow-up WPF wiring smaller.

Cons:

- The app still cannot execute Quarantine from the UI after this packet.
- Requires another integration packet before manual cleanup use.

### Option C: Script-only fixture executor

Pros:

- Avoids production move APIs for one more packet.

Cons:

- Does not prove the app's actual execution boundary.
- Creates throwaway behavior that would need to be rewritten.

## Why this decision

A core fixture-first executor advances the MVP toward real quarantine while preserving the project's safety discipline. It lets tests prove write-ahead manifesting, partial failure, destination collision handling, and source preservation before user-profile execution is possible.

## Consequences

Positive consequences:

- Future WPF execution can call one tested core boundary.
- Partial failure semantics become concrete.
- The source-level write/move allowlist remains strict.

Negative consequences:

- The executor can move files when called directly by tests or future code.
- The UI still needs a separate stale-state and confirmation integration packet.
- WPF Undo Quarantine is still needed before real cleanup should feel complete.

## Reversal cost

Moderate after WPF execution is wired. Low while the executor is only tested against fixtures.

## Follow-up work

- Wire WPF execution only after the executor passes fixture preflight and stale-state checks are explicit.
- Core Undo Quarantine for Moved entries is covered by ADR 0008; add WPF Undo Quarantine before permanent deletion exists.
- Add recovery UI for Moving, Failed, and leftover temp manifest states.

## Supersedes

- None. Implements the file-moving follow-up from ADR 0005 and ADR 0006.

## Superseded by

- None.
