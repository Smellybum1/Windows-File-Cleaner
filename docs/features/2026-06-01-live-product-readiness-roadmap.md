# Feature: Live Product Readiness Roadmap

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make the remaining path to a safe live product explicit without enabling any new cleanup, restore, delete, or history behavior.

The roadmap should help future packets choose the next safest work while preserving the current fixture-first and read-only real-profile boundary.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile selected restore execution.
- Do not enable broad real-profile Undo Quarantine.
- Do not enable permanent deletion.
- Do not add persisted cleanup history.
- Do not scan, move, restore, delete, create, or rewrite real-profile files.
- Do not replace ADR 0017, ADR 0018, ADR 0019, README, or the fixture checklist.

## User Story / Job Story

As the local app owner, I want one compact readiness roadmap, so that the project can keep moving toward a safe live cleanup product without confusing fixture proof, read-only readiness evidence, and real-profile movement approval.

## Current Behavior

The app is a local WPF desktop reviewer with:

- read-only Storage Scan,
- fixture-only Quarantine execution,
- current-fixture Undo Quarantine,
- fixture-only selected restore,
- read-only real-profile/custom Quarantine readiness and approval evidence,
- read-only selected restore revalidation evidence,
- a full `.cmd` MVP preflight,
- a manual fixture review checklist.

Real-profile movement, permanent deletion, and persisted cleanup history remain intentionally unavailable.

## Desired Behavior

Future work should follow this readiness sequence.

| Readiness track | Current evidence | Done when | Next likely packet |
|---|---|---|---|
| Manual fixture acceptance | Full `.cmd` MVP preflight and checklist-only output are current. Header/tab UI has user visual approval. | The visible fixture pass is run through Quarantine Preview, fixture execution, current-session review, undo, discovery, selected restore gate, fixture selected restore, and ADR 0017/0018/0019 blocker wording. | Support `.\tools\Start-MvpFixtureReview.cmd` when the user is ready, then record results. |
| Fresh real-profile read-only retest | User previously confirmed real-profile scan works and debounced search fixed sluggish typing. | After a fresh preflight, `C:\Users\moxhe` scan is manually retested for scan gate, no-file-modified status, performance, filters/search/focus, Review Shortlist context, and preview-only blockers. | Run only after the user intentionally starts a real-profile retest. |
| Real-profile selected restore implementation | ADR 0019 and read-only selected restore revalidation evidence exist. | Exactly one selected real-profile Restore Manifest can restore through `UndoQuarantineExecutor` after fresh discovery, selected review, immediate revalidation, exact `RESTORE`, no original-path overwrite, result guidance, and explicit user-approved implementation. | Grill with Docs implementation packet for ADR 0019, with core and WPF tests, only after user approval. |
| First real-profile Quarantine execution | ADR 0017/0018, root execution safety, pre-execution revalidation, restore readiness, and approval evidence exist as read-only models/output. | Exact `C:\Users\moxhe` can run a first limited real-profile Quarantine action after selected restore recovery is trusted, all readiness dimensions pass, exact `QUARANTINE` is typed, the batch is within 10 rows / 1 GB, and the user explicitly approves crossing the movement boundary. | Only after selected real-profile restore is implemented and manually trusted. |
| Real-profile recovery confidence | Fixture current undo and fixture selected restore are implemented. Real-profile restore is currently read-only evidence only. | After a real-profile Quarantine action, the app can prove selected restore recovery on the created Restore Manifest without broad all-manifest restore or cleanup history. | Manual recovery proof after the first approved real-profile Quarantine packet. |
| Release/packaging readiness | Local debug run and desktop shortcut target exist. | A repeatable release build or local install/update path exists, with README instructions and preflight/recovery docs still matching behavior. | Add packaging only after reversible real-profile cleanup is trusted. |
| Later full cleanup expansion | Permanent deletion and persisted cleanup history are intentionally unavailable. | Only if the user chooses them after reversible Quarantine and recovery are trusted, with new ADRs, tests, recovery wording, and explicit approval. | Separate Grill with Docs packets; do not bundle with first real-profile Quarantine. |

## Domain Language Changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing terms are reused. | n/a |

## Open Questions

Questions that must be answered before implementation:

- None for this roadmap packet.

Questions that can be deferred:

- Should the first "live product" release stop at reversible real-profile Quarantine plus selected restore, or should it also include packaging before the first real cleanup?
- Should permanent deletion ever exist in this app, or remain outside the product?
- Should persisted cleanup history exist after Restore Manifest-only recovery is trusted?
- Should all-manifest real-profile restore ever exist, or should real-profile recovery remain selected-manifest-only?

## Grill Notes

### Scenarios Discussed

- The user wants the project to continue toward a safe full live product.
- The UI was messy, then header/tabs/panels were cleaned up and visually approved.
- The user explicitly said not to move, delete, quarantine, or restore real-profile files unless asked after a Grill with Docs pass.

### Edge Cases

- A roadmap must not imply that real-profile movement is available.
- "Live product" can mean multiple milestones; this roadmap keeps reversible real-profile cleanup separate from later deletion/history expansion.
- The next visible fixture pass can find layout or wording issues that should be fixed before real-profile retest.

### Dependencies Between Decisions

- Real-profile Quarantine depends on selected real-profile restore readiness.
- Selected real-profile restore depends on ADR 0019 implementation and explicit user approval.
- Permanent deletion and cleanup history depend on trusted reversible Quarantine/recovery first.
- Packaging is lower risk after behavior is stable, but should not distract from safety gates.

## Evidence and Validation Gate

Evidence gathered:

- User answers:
  - Real-profile scan works.
  - Debounced search fixed large-scan sluggish typing.
  - The non-`D:` acknowledgement row feels clear.
  - The tabbed/header UI looks much better.
  - Do not move, delete, quarantine, or restore real-profile files without explicit approval after Grill with Docs.
- Existing code/docs inspected:
  - `README.md`
  - `.codex/progress.md`
  - `docs/codex/thread-handoff.md`
  - `docs/domain/context.md`
  - `docs/domain/glossary.md`
  - `docs/features/2026-05-28-mvp-readiness-audit.md`
  - `docs/features/2026-05-31-real-profile-quarantine-design-pass.md`
  - `docs/features/2026-05-31-real-profile-selected-restore-execution-contract.md`
  - ADR 0017, ADR 0018, and ADR 0019
- Tests/checks planned:
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not treat the roadmap as approval to implement real-profile movement.
- Do not collapse selected real-profile restore and forward real-profile Quarantine into one packet.
- Do not bundle permanent deletion or cleanup history into the first live cleanup action.

## Decisions Made

Small feature-level decisions:

- Use a feature brief rather than an ADR because this packet organizes existing accepted decisions and evidence.
- Treat visible fixture acceptance and fresh real-profile read-only retest as gates before any real-profile movement implementation.
- Treat selected real-profile restore as the recovery prerequisite before first real-profile Quarantine execution.

ADR-worthy decisions:

- [x] None. ADR 0017, ADR 0018, and ADR 0019 already hold the durable movement/restore decisions.

## Implementation Plan

1. Add this roadmap feature brief.
2. Link it from README and handoff docs.
3. Record the packet in `.codex/progress.md`.
4. Run checklist-only and whitespace checks.

## Files Expected To Change

Expected:

- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test Plan

Manual checks:

- Review the roadmap wording and confirm it does not imply real-profile movement is currently available.

Automated checks:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks And Assumptions

Risks:

- A roadmap can become stale if real-profile restore or Quarantine execution lands without updating it.
- The phrase "full live product" may later need a sharper definition from the user once reversible cleanup is trusted.

Assumptions:

- Reversible Quarantine plus selected restore is the right first live cleanup milestone before deletion or cleanup history.
- The next best product evidence is still a visible fixture pass, not real-profile movement.

## Completion Notes

Completed on: 2026-06-01

What changed:

- Added this roadmap with explicit readiness tracks from fixture acceptance through later deletion/history decisions.
- Linked the roadmap from README and handoff docs.
- Recorded the packet in the progress log.

Files changed:

- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- README, feature brief, handoff, and progress log.

ADRs added or skipped:

- No ADR added. This roadmap organizes existing accepted decisions without changing architecture, persistence, cleanup execution, restore behavior, data model, or security policy.

Follow-up work:

- Run the visible fixture pass when the user is ready.
- Record manual fixture results and any UI/readiness polish needed before real-profile retest.
- Start real-profile selected restore implementation only after explicit user approval.

Open questions:

- Whether first live release requires packaging before or after the first reversible real-profile cleanup action.
- Whether permanent deletion, persisted cleanup history, or all-manifest restore should ever be added.

Risky assumptions:

- Roadmap-level clarity helps future packets avoid unsafe scope jumps without adding too much process.
