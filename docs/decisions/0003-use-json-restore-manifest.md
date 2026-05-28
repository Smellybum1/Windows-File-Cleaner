# ADR 0003: Use JSON Restore Manifest

Date: 2026-05-28  
Status: accepted  
Owner: project-owner

## Context

The preferred future cleanup mechanism is Quarantine on `D:` with easy Undo Quarantine.

Undo requires durable metadata that can reconstruct where each quarantined file or folder came from, where it was moved, what was known at action time, and which app/schema version wrote the record. The app does not yet execute Quarantine, but the manifest shape should be designed before file-moving code exists.

Constraints:

- Cleanup execution must require explicit confirmation.
- Preview and report exports must not be confused with executed cleanup records.
- A future manifest must be readable by humans and by the app.
- The format should be versioned before the first write.
- The first implementation should support local-only use and fixture verification.

## Decision

Use a JSON Restore Manifest schema with an explicit schema version.

The first schema version will be `restore-manifest.v1`. A Restore Manifest Draft may be generated in memory from included Quarantine Preview rows, but it is not an executed manifest and must not be written as proof of cleanup. Future Quarantine execution will persist an executed Restore Manifest only after explicit user confirmation starts the execution flow. ADR 0005 refines the write order: the first durable Restore Manifest write should happen before the first file or folder move, then the manifest should be updated before and after each move attempt.

The manifest shape includes:

- schema version
- manifest/draft id
- drafted or action timestamp
- Cleanup Scope path
- Quarantine root path
- entry list
- action status
- original path
- quarantine path
- entry status
- file/folder type
- size bytes
- last modified UTC when known
- move timestamps and failure message when applicable
- Importance Rating
- Deletion Recommendation
- Bloat Categories
- evidence captured at preview/action time

## Options considered

### Option A: JSON manifest

Pros:

- Human-readable enough for local diagnostics.
- Structured enough for Undo Quarantine.
- Easy to version.
- Supported by the .NET standard library.
- Can be fixture-tested without new dependencies.

Cons:

- Less convenient for casual spreadsheet review than CSV.
- Requires care to avoid treating drafts as executed manifests.

### Option B: CSV manifest

Pros:

- Easy to inspect in spreadsheets.
- Already aligned with report exports.

Cons:

- Awkward for versioning nested or future metadata.
- Easier to confuse with read-only reports.
- More brittle for restore semantics.

### Option C: App-private database

Pros:

- Stronger querying and lifecycle management later.
- Could support richer action history.

Cons:

- Too heavy for the MVP.
- Harder for the user to inspect manually.
- Requires migration/backup policy before first write.

## Why this decision

JSON is the simplest durable structure that supports Undo Quarantine without adding a database or overloading CSV reports. It keeps the preview/export/report workflows separate from future execution records.

## Consequences

Positive consequences:

- Future Undo Quarantine has a clear persistence target.
- Restore Manifest Drafts can be tested before cleanup execution exists.
- The schema can evolve through explicit versioning.

Negative consequences:

- A future UI may need both CSV reports and JSON manifests.
- The app must clearly label drafts versus executed manifests.

## Reversal cost

Moderate after the first executed manifest is written. Reversing later would require migration or support for multiple manifest formats.

## Follow-up work

- Add a Restore Manifest Draft model and tests.
- Design explicit Quarantine confirmation flow before file-moving code.
- Implement executed manifest writing only in the future Quarantine execution packet, following ADR 0005.

## Supersedes

- None.

## Superseded by

- None. ADR 0005 refines write order and partial-failure state without replacing this format decision.
