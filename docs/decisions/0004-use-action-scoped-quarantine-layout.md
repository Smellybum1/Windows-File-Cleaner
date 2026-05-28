# ADR 0004: Use Action-Scoped Quarantine Layout

Date: 2026-05-29
Status: accepted
Owner: project-owner

## Context

The app is moving toward a reversible Quarantine workflow on `D:`. Quarantine Preview currently shows dry-run destination paths and Restore Manifest Draft proves the undo metadata shape, but actual file-moving code still needs a clear on-disk layout before it exists.

Constraints:

- Preview paths must not be confused with executed quarantine storage.
- Multiple future quarantine actions should not collide with each other.
- A Restore Manifest must live close to the files it can restore.
- The layout must be predictable enough for local diagnostics and fixture tests.
- This decision does not implement file moves, folder creation, manifest writing, or Undo Quarantine.

## Decision

Use an action-scoped layout under the selected Quarantine root:

```txt
<quarantine-root>\actions\<action-id>\items\<cleanup-scope-relative-path>
<quarantine-root>\actions\<action-id>\restore-manifest.json
```

The current read-only Quarantine Action Draft builds these paths in memory only. The action id must be a path-safe segment containing only letters, digits, hyphens, or underscores.

The existing preview paths under `<quarantine-root>\preview\...` remain preview-only comparison paths. Future execution code should use action-scoped item paths, not preview paths, when moving files.

## Options considered

### Option A: Action-scoped layout

Pros:

- Avoids collisions between separate quarantine actions.
- Keeps items and restore manifest together.
- Makes Undo Quarantine discovery simpler.
- Makes fixture verification straightforward.

Cons:

- Adds another path concept beside preview destinations.
- Requires UI wording to distinguish preview paths from action paths.

### Option B: Flat root layout

Pros:

- Simpler path shape.
- Easier to browse at first glance.

Cons:

- Higher collision risk.
- Harder to pair files with the manifest for one action.
- Harder to undo one action independently.

### Option C: Manifest-only layout decision later

Pros:

- Avoids deciding path layout before execution code.

Cons:

- Leaves file-moving work with a hard-to-reverse design question.
- Makes current confirmation UI less concrete.

## Why this decision

Action-scoped layout gives each future Quarantine Cleanup Action a bounded folder and manifest. It is conservative, local-readable, and aligns with the need for Undo Quarantine without introducing a database.

## Consequences

Positive consequences:

- Future execution can move files into a deterministic action folder.
- Future Undo Quarantine can start from an action-level manifest.
- The app can show the future layout before file-moving code exists.

Negative consequences:

- The UI now has to distinguish Quarantine Preview destinations from Quarantine Action Draft destinations.
- Future cleanup history may need to index action folders if many actions accumulate.

## Reversal cost

Moderate after the first executed manifest is written. Reversing later would require supporting or migrating existing action folders and manifests.

## Follow-up work

- Decide manifest write order and failure handling before adding file-moving code.
- Implement Quarantine execution only after action layout, confirmation gate, and restore behavior are tested against fixtures.
- Implement Undo Quarantine using action-scoped restore manifests.

## Supersedes

- None.

## Superseded by

- None.
