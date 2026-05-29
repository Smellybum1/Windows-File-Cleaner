# ADR 0016: Keep Discovered Manifests in Manifest Panes

Date: 2026-05-30
Status: accepted
Owner: project-owner

## Context

The WPF app now has two different ways to review quarantined material:

- Current-Session Quarantined Review switches the main grid to entries still in `Moved` state from the current in-memory fixture Restore Manifest.
- Quarantine Manifest Discovery, Selected Restore Manifest Review, Selected Restore Confirmation Draft, Selected Restore Execution Gate, Fixture-only Selected Restore Execution, and Restore Readiness Preview handle discovered action-scoped Restore Manifests under the selected Quarantine Root.

The user previously asked for a separate area for files that have been quarantined because moved files disappear from a fresh Storage Scan after fixture Quarantine execution. The app answered that immediate need with Current-Session Quarantined Review. A future broader restore/history design could either extend that main-grid switch to older discovered manifests or keep older discovered manifests in the manifest discovery/readiness panes.

Constraints:

- Real-profile WPF Quarantine execution remains unavailable.
- Real-profile WPF Undo Quarantine remains unavailable.
- Permanent deletion and persisted cleanup history remain unavailable.
- Current-Session Quarantined Review is read-only and current-session-only.
- Quarantine Manifest Discovery is read-only and not cleanup history.
- Selected restore execution is fixture-only.
- The main Storage Scan grid should not become a mixed history/restore grid without a separate design.

## Decision

Keep older/discovered Restore Manifest review in the manifest discovery/readiness panes for now.

Do not extend Current-Session Quarantined Review into a broad all-manifest history grid. The main grid switch remains scoped to current in-memory fixture moved entries only.

Future real-profile or broader discovered-manifest restore/history work should use a separate design packet. That future design may introduce a dedicated restore/history view, but it should not silently reuse `Current quarantined` as all quarantined history.

## Options considered

### Option A: Reuse Current-Session Quarantined Review for all discovered manifests

Pros:

- One grid-like review surface for quarantined entries.
- Faster path to showing older manifest entries in rows.

Cons:

- Blurs current-session fixture state with older discovered manifests from disk.
- Makes `Current quarantined` misleading.
- Risks implying cleanup history or real-profile Undo Quarantine exists.
- Mixes read-only discovery, fixture selected restore, and future broad restore semantics in one grid.

### Option B: Keep discovered manifests in manifest discovery/readiness panes

Pros:

- Preserves the current-session meaning of `Current quarantined`.
- Keeps discovered manifest review tied to Quarantine Manifest Discovery, Selected Restore Manifest Review, Selected Restore Execution Gate, and Restore Readiness Preview.
- Avoids implying broad restore/history support before real-profile restore is designed.
- Keeps future broad restore/history work free to choose a dedicated surface after manual review.

Cons:

- Older discovered manifests are not shown in the main grid.
- The app still lacks a broad restore/history view.
- The user must use manifest discovery controls rather than the current-session grid for older actions.

### Option C: Add a dedicated restore/history tab now

Pros:

- Creates a clear separate place for discovered manifests.
- Could eventually support broader recovery review.

Cons:

- Larger UX and safety design before real-profile restore is ready.
- Risks making cleanup history feel available before persistence and restore rules exist.
- Adds navigation and layout complexity before the manual fixture review pass validates the current dense surface.

## Why this decision

The safest current boundary is to keep each surface named for what it actually does. Current-Session Quarantined Review answers the immediate fixture execution visibility problem. Quarantine Manifest Discovery and the selected/all-manifest readiness panes are the safer place for older discovered manifests until a broader restore/history workflow is designed.

## Consequences

Positive consequences:

- `Current quarantined` stays accurate and current-session-only.
- Discovered Restore Manifests remain under read-only discovery/readiness language.
- Future broad restore/history work has an explicit decision point instead of inheriting a misleading grid label.

Negative consequences:

- Users cannot use the main grid to browse all discovered quarantined files.
- Future restore/history work still needs a separate UX decision.
- Manifest panes may feel less table-like than the main grid until a dedicated surface exists.

## Reversal cost

Medium. Reusing the main grid later would require WPF view-model, wording, tooltip/help text, smoke-test, and docs changes to separate current-session state from discovered-manifest history.

## Follow-up work

- During manual fixture review, confirm whether manifest discovery/readiness panes are enough for older action review.
- If the user wants broader restore/history browsing, create a new Grill with Docs packet before implementing a dedicated view.
- Keep real-profile restore blocked until its safety/undo story is reviewed again.

## Supersedes

- None.

## Superseded by

- None.
