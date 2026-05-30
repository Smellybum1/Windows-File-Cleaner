# Feature: Real-Profile Selected Restore Execution Contract

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Define the safety contract for a future selected-manifest real-profile Undo Quarantine path without enabling real-profile restore or forward Quarantine execution.

This packet turns the recovery prerequisite from ADR 0018 into a concrete design boundary for the later implementation packet.

## Non-goals

- Do not enable real-profile selected restore execution.
- Do not enable real-profile WPF Quarantine execution.
- Do not enable all-manifest real-profile restore.
- Do not enable custom non-fixture selected restore.
- Do not add permanent deletion.
- Do not add persisted cleanup history.
- Do not clean up empty quarantine action folders.
- Do not move, restore, delete, create, or rewrite real-profile files.

## User story / job story

As the local app owner, I want the app to define exactly how selected real-profile restore must work before it can restore anything, so that future real-profile cleanup has a trusted recovery path before forward movement is exposed.

## Current behavior

- WPF can restore selected discovered fixture Restore Manifests after selected readiness and exact `RESTORE`.
- WPF keeps selected restore unavailable for real-profile and custom non-fixture Restore Manifests.
- Core Real-Profile Restore Readiness can model selected-manifest recovery evidence, but it does not restore files and is not wired to WPF execution.
- Real-profile Quarantine execution, broad Undo Quarantine, permanent deletion, and persisted cleanup history remain unavailable.

## Desired behavior

Future Real-Profile Selected Restore Execution must:

- operate on one selected Restore Manifest at a time,
- allow only exact `C:\Users\moxhe` Cleanup Scope manifests in the first phase,
- require fresh discovery, selected manifest review, selected readiness, selected confirmation draft, and selected execution gate evidence,
- require exact `RESTORE` confirmation,
- immediately revalidate original-path collisions, quarantine path existence, manifest state, and selected readiness before movement,
- call `UndoQuarantineExecutor` for movement and manifest updates,
- block recovery-review, failed, restoring, not-moved, blocked, all-restored, missing-quarantine-path, and original-path-collision states,
- keep Restore Manifest as the only durable record,
- show result and stale-state guidance that asks the user to rediscover manifests and rescan manually,
- leave all-manifest restore, action-folder cleanup, permanent deletion, and cleanup history out of scope.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Selected Restore Execution | Added as the future selected-manifest real-profile Undo Quarantine execution path. | yes |
| Real-Profile Restore Readiness | Clarified that it feeds this future selected execution contract and forward Quarantine readiness, but still does not execute in the current build. | yes |

## Open questions

Questions that must be answered before implementation:

- None for this design packet. The first implementation can use the selected-only, exact-scope, exact-`RESTORE`, manual rediscover/rescan, Restore Manifest-only contract.

Questions that can be deferred:

- Should all-manifest real-profile restore ever exist?
- Should successful restore ever offer empty action-folder cleanup?
- Should the app add a richer cleanup history after Restore Manifest-only recovery is trusted?
- Should custom non-fixture restore ever become executable?

## Grill notes

### Scenarios discussed

- The user chose selected Restore Manifest recovery first, not all-manifest restore.
- The user accepted manual rescan after future real-profile Quarantine rather than auto-rescan.
- The user chose Restore Manifest as the only durable cleanup record for now.
- ADR 0018 requires selected-manifest real-profile Undo readiness before forward real-profile movement.

### Edge cases

- Original path reappears after selected readiness preview.
- Quarantine path is missing or inaccessible.
- Manifest contains recovery-review, partial failure, restoring, failed, not-moved, or already-restored-only states.
- Selected manifest no longer belongs to current discovery after Quarantine Root changes.
- User types `RESTORE` after stale readiness output.
- Action folder has empty folders after restore; first phase leaves them alone.

### Dependencies between decisions

- Depends on ADR 0013 Selected Restore Manifest Review.
- Depends on ADR 0014 Selected Restore Confirmation Gate.
- Depends on ADR 0015 Fixture-only Selected Restore Execution.
- Depends on ADR 0018 Real-Profile Quarantine Execution Readiness.
- Adds ADR 0019 Real-Profile Selected Restore Execution Contract.

## Evidence and validation gate

Evidence gathered:

- User answers:
  - Limit first real-profile execution work to `C:\Users\moxhe`.
  - Use exact `RESTORE` for selected restore.
  - Use Restore Manifest only for now.
  - Ask the user to rescan/rediscover manually after movement.
- Existing code/docs inspected:
  - ADRs 0013, 0014, 0015, 0017, and 0018.
  - Selected Restore Manifest Review, Selected Restore Confirmation Gate, Fixture-only Selected Restore Execution, Real-Profile Restore Readiness, and WPF Execution Readiness Output feature briefs.
- Tests/checks planned:
  - Docs-only whitespace diff check for this packet.
  - Future implementation should run core and WPF tests before any behavior change.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not enable selected real-profile restore by only flipping the fixture-scope availability check.
- Do not use all-manifest restore as the first real-profile restore workflow.
- Do not clean up action folders in the first real-profile restore path.
- Do not treat exact `RESTORE` as enough without immediate selected-readiness revalidation.

## Decisions made

Small feature-level decisions:

- This is a docs/design packet only.
- The first real-profile restore path is selected-manifest-only and exact `C:\Users\moxhe` only.
- The future execution packet must reuse `UndoQuarantineExecutor`.
- Manual rediscover/rescan guidance is preferred after restore attempts.

ADR-worthy decisions:

- [x] ADR needed: ADR 0019 records the Real-Profile Selected Restore Execution contract.

## Implementation plan

1. Add ADR 0019 for the selected real-profile restore execution contract. Completed in this packet.
2. Add domain and glossary language for Real-Profile Selected Restore Execution. Completed in this packet.
3. Update progress and handoff docs so future work starts from the new contract. Completed in this packet.
4. Future packet: add regression coverage showing selected real-profile restore remains unavailable until implementation.
5. Future packet: implement selected real-profile restore behind the ADR 0019 contract after explicit approval.

## Files expected to change

Expected:

- `docs/decisions/0019-use-real-profile-selected-restore-execution-contract.md`
- `docs/features/2026-05-31-real-profile-selected-restore-execution-contract.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/features/2026-05-31-real-profile-restore-readiness.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

Manual checks:

- None for this docs-only contract packet.

Automated tests:

- `git diff --check`

## Risks and assumptions

Risks:

- Future WPF wording could make selected real-profile restore look like broad Undo Quarantine if selected-only scope is not repeated.
- Immediate selected-restore revalidation may block often when the filesystem changes between preview and execution.

Assumptions:

- Selected-manifest real-profile restore is enough recovery readiness for the first real-profile Quarantine phase.
- Restore Manifest-only durable recovery remains acceptable until cleanup history is intentionally designed.

## Completion notes

Completed on: 2026-05-31

What changed:

- Added ADR 0019 for the Real-Profile Selected Restore Execution contract.
- Added this feature brief.
- Updated domain/glossary, README, ADR 0018, Real-Profile Restore Readiness follow-up, progress, and handoff docs.
- Kept all real-profile movement unavailable.

Files changed:

- `docs/decisions/0019-use-real-profile-selected-restore-execution-contract.md`
- `docs/features/2026-05-31-real-profile-selected-restore-execution-contract.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/features/2026-05-31-real-profile-restore-readiness.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Tests run:

- `git diff --check`

Docs updated:

- ADR, feature brief, domain context, glossary, README, progress log, and thread handoff.

ADRs added or skipped:

- Added ADR 0019.

Follow-up work:

- Real-profile selected restore regression coverage was added in `docs/features/2026-05-31-real-profile-selected-restore-regression.md`; execution remains blocked.
- Implement the selected real-profile restore path only after explicit approval.

Open questions:

- Whether all-manifest real-profile restore, action-folder cleanup, cleanup history, or custom non-fixture restore should ever exist.

Risky assumptions:

- The first recovery implementation can stay selected-manifest-only without adding all-manifest restore.
