# Feature: Undo Quarantine Domain Consistency

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Correct current domain docs so Undo Quarantine Executor wiring matches the app's current fixture-only behavior.

## Non-goals

- Do not change code behavior.
- Do not enable real-profile WPF Undo Quarantine.
- Do not enable all-manifest discovered-manifest restore.
- Do not add cleanup history.
- Do not move, restore, delete, create, or modify files.

## User story / job story

As the project owner, I want the current domain docs to reflect that WPF uses the Undo Quarantine Executor for fixture-only undo and selected restore, so future packets do not rediscover an already-completed fixture boundary.

## Current behavior

The WPF app has current-fixture undo and fixture-only selected restore. `docs/domain/context.md` still said Undo Quarantine Executor was fixture-tested but not wired to WPF yet.

## Desired behavior

- Domain context says Undo Quarantine Executor is used by WPF current-fixture undo and fixture-only selected restore.
- Domain context still keeps real-profile WPF Undo Quarantine and all-manifest discovered-manifest restore unavailable.
- ADR 0010 context is clarified as decision-time context rather than current-state wording.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Undo Quarantine Executor | Corrected current WPF wiring status while preserving real-profile/all-manifest blockers. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is a docs consistency correction.

Questions that can be deferred:

- Should future docs distinguish current-state domain context from historical ADR/feature context more explicitly?

## Grill notes

### Scenarios discussed

- Current fixture execution can be undone from the visible app.
- Selected discovered fixture Restore Manifests can be restored through selected readiness and exact `RESTORE`.
- Real-profile WPF Undo and all-manifest discovered-manifest restore remain unavailable.

### Edge cases

- Historical feature briefs may still describe packet-start behavior and should not be rewritten broadly.
- ADR context should remain historical but not read like current state.

### Dependencies between decisions

- Depends on ADR 0010 current-fixture WPF undo.
- Depends on ADR 0015 fixture-only selected restore execution.

## Evidence and validation gate

Evidence gathered:

- Current README, handoff, glossary, WPF tests, ADR 0010, and feature briefs show current-fixture undo and fixture-only selected restore exist.
- `docs/domain/context.md` contained stale current-state wording.
- Tests/checks planned: targeted stale-wording search and diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not rewrite old feature briefs that accurately describe packet-start behavior.
- Do not turn this docs correction into a real-profile undo design packet.

## Decisions made

Small feature-level decisions:

- Update current domain context.
- Clarify ADR 0010 as decision-time context without changing the accepted decision.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update `docs/domain/context.md` Undo Quarantine Executor section.
2. Clarify ADR 0010 context sentence as decision-time language.
3. Add this feature brief and progress/handoff notes.
4. Run stale-wording search and diff check.

## Test plan

Automated checks:

- `rg -n "not wired to the WPF|not wired to WPF|does not expose undo yet|Keep WPF Undo Quarantine unavailable" docs/domain docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- Review the updated Undo Quarantine Executor section for current-state accuracy.

## Risks and assumptions

Risks:

- Historical ADR/feature wording can still appear stale if read without packet context.

Assumptions:

- Current domain context should be the authoritative current-state wording, while older feature briefs can remain historical.

## Completion notes

Completed on: 2026-05-29

What changed:

- Updated Undo Quarantine Executor current-state language in domain context.
- Clarified ADR 0010 context as decision-time wording.
- Preserved real-profile WPF Undo Quarantine and all-manifest discovered-manifest restore blockers.

Files changed:

- `docs/domain/context.md`
- `docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`
- `docs/features/2026-05-29-undo-quarantine-domain-consistency.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Tests run:

- `rg -n "not wired to the WPF|not wired to WPF|does not expose undo yet|Keep WPF Undo Quarantine unavailable" docs/domain docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- Domain context, ADR 0010, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is a docs consistency correction, not a new durable decision.

Follow-up work:

- Keep future current-state docs aligned after fixture-only restore/undo packets.

Open questions:

- Should future docs distinguish current-state domain context from historical ADR/feature context more explicitly?

Risky assumptions:

- Leaving older feature briefs historical is less risky than broad rewriting.
