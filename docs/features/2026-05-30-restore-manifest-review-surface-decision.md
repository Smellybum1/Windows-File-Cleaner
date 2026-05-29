# Feature: Restore Manifest Review Surface Decision

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Record where older discovered Restore Manifest review belongs before broad real-profile restore/history work begins.

## Non-goals

- Do not change WPF layout or controls.
- Do not add a new tab, main-grid mode, persisted cleanup history, broad restore/history view, or real-profile restore.
- Do not change fixture Quarantine execution, current-fixture undo, fixture-only selected restore, Quarantine Manifest Discovery, Restore Readiness Preview, or selected restore behavior.
- Do not scan or modify real-profile files.

## User story / job story

As the project owner, I want the app to keep current-session quarantined rows separate from older discovered manifests, so that a review grid does not imply broad history or real-profile restore support before it exists.

## Current behavior

- `Current quarantined` switches the main grid to current in-memory fixture Restore Manifest entries still in `Moved` state.
- Older/discovered Restore Manifests are reviewed through Quarantine Manifest Discovery, Selected Restore Manifest Review, Selected Restore Execution Gate, Fixture-only Selected Restore Execution, and Restore Readiness Preview.
- Real-profile selected restore remains unavailable.

## Desired behavior

- Keep `Current quarantined` current-session-only.
- Keep older/discovered Restore Manifest review in manifest discovery/readiness panes for now.
- Require a future Grill with Docs packet before adding a dedicated broad restore/history surface or reusing the main grid for discovered manifests.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Current-Session Quarantined Review | Clarified it should not become all-manifest history. | yes |
| Quarantine Manifest Discovery | Clarified discovered manifests stay in discovery/readiness panes for now. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is a documentation/decision packet that preserves current behavior.

Questions that can be deferred:

- If the user wants broader restore/history browsing, should it be a dedicated tab, a separate grid under manifest discovery, or a new route/view?

## Grill notes

### Scenarios discussed

- Current fixture Quarantine execution: moved entries remain visible through `Current quarantined`.
- Older action-scoped Restore Manifests: discovered through Quarantine Manifest Discovery and reviewed with selected/all-manifest readiness.
- Future real-profile restore/history: still blocked until a separate design.

### Edge cases

- After a rescan, moved current-session rows may disappear from Storage Scan results but stay available through `Current quarantined`.
- Older manifests may contain moved, restored, failed, or recovery-review entries; those remain discovery/readiness evidence, not cleanup history.
- Real-profile/custom selected restore stays unavailable even when `RESTORE` matches.

### Dependencies between decisions

- Builds on ADRs 0011 through 0015.
- Preserves the current-session scope of `Current quarantined`.

## Evidence and validation gate

Evidence gathered:

- User previously asked for a separate area for quarantined files after moved files disappeared from Storage Scan rows after rescan.
- Current implementation added Current-Session Quarantined Review to solve current fixture visibility.
- Existing domain docs already distinguish older/discovered manifests from Current-Session Quarantined Review.
- Handoff listed the discovered-manifest surface question as a good next work item.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not silently expand `Current quarantined` into all quarantined history.
- Do not add a broad restore/history tab before manual fixture review and real-profile restore design.

## Decisions made

Small feature-level decisions:

- Update domain docs and handoff to point future work away from overloading `Current quarantined`.

ADR-worthy decisions:

- [x] ADR added: `docs/decisions/0016-keep-discovered-manifests-in-manifest-panes.md`

## Implementation plan

1. Add ADR 0016.
2. Update domain docs and glossary notes to reinforce current-session versus discovered-manifest scope.
3. Update progress and handoff.
4. Run documentation checks.

## Files expected to change

Expected:

- `docs/decisions/0016-keep-discovered-manifests-in-manifest-panes.md`
- `docs/features/2026-05-30-restore-manifest-review-surface-decision.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- `README.md`

## Test plan

Manual checks:

- During the next fixture review, confirm `Current quarantined` still reads as current-session-only and manifest discovery/readiness panes remain the place for older manifests.

Automated tests:

- `rg -n "Current quarantined|cleanup history|all quarantined history" README.md docs`
- `git diff --check`

## Risks and assumptions

Risks:

- A future dedicated restore/history surface may still be needed if manifest panes feel too cramped.

Assumptions:

- Preserving the current-session boundary is safer than broadening the main grid before real-profile restore design.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added ADR 0016 accepting that older/discovered Restore Manifests stay in manifest discovery/readiness panes for now.
- Clarified that `Current quarantined` must remain current-session-only and should not silently become all quarantined history.
- Updated handoff/progress to make a future broad restore/history surface a separate Grill with Docs packet.
- No WPF layout, scan behavior, Quarantine behavior, selected restore behavior, real-profile/custom restore availability, permanent deletion, or cleanup history changed.

Files changed:

- `docs/decisions/0016-keep-discovered-manifests-in-manifest-panes.md`
- `docs/features/2026-05-30-restore-manifest-review-surface-decision.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `rg -n "Current quarantined|cleanup history|all quarantined history" README.md docs`
- `git diff --check`

Docs updated:

- ADRs, domain context, glossary, handoff, progress log, and this feature brief.

ADRs added or skipped:

- Added ADR 0016 because the choice affects core UX flow, has multiple plausible options, and reversing later would require WPF/docs/test changes.

Follow-up work:

- Run manual fixture review and decide whether manifest panes are comfortable enough for older action review.
- If broader browsing is needed, design a dedicated restore/history surface before implementation.

Open questions:

- Should a future broad restore/history surface be a dedicated tab, a separate grid under manifest discovery, or another view?

Risky assumptions:

- The manifest discovery/readiness panes remain sufficient until the user asks for broader restore/history browsing.
