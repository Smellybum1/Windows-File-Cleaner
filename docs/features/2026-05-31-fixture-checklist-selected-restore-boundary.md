# Feature: Fixture Checklist Selected Restore Boundary

Date started: 2026-05-31  
Status: completed  
Owner: project-owner

## Goal

Keep the manual fixture checklist aligned with the current real-profile safety contracts by naming both ADR 0017 Quarantine blockers and ADR 0019 selected-restore blockers in the final real-profile/custom boundary prompt.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile selected restore execution.
- Do not change WPF behavior, tests, manifests, or cleanup execution.
- Do not scan, move, restore, delete, create, or rewrite real-profile files.

## Desired behavior

`Start-MvpFixtureReview.cmd -ChecklistOnly` should remind the reviewer that:

- exact `QUARANTINE` plus clean Quarantine Preview does not unlock real-profile/custom Quarantine execution,
- exact `RESTORE` does not unlock real-profile/custom selected restore,
- ADR 0017 and ADR 0019 blockers must both stay visible before any real-profile movement.

README manual-review wording should mirror the same boundary so the checklist and durable docs do not drift.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- ADR 0017 blocks real-profile Quarantine execution until the readiness contract is implemented.
- ADR 0019 blocks selected real-profile restore until the selected-manifest execution contract is implemented.
- The launcher checklist already mentioned exact `RESTORE`, but its final blocker sentence only named ADR 0017.

Validation gate:

- [x] Existing domain terms are sufficient.
- [x] Permission boundary is clear: checklist output and docs only.
- [x] Narrowest relevant check is `Start-MvpFixtureReview.cmd -ChecklistOnly`.

## Implementation

- Updated fixture checklist step 10 to name ADR 0017 Quarantine blockers and ADR 0019 selected-restore blockers.
- Updated README fixture-smoke/manual-check wording and the MVP readiness-audit follow-up to keep the same ADR 0017/0019 boundary visible.
- Kept WPF behavior and execution availability unchanged.

## Verification

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## ADRs

No ADR added. ADR 0017 and ADR 0019 already record the durable safety decisions.

## Open questions

- None for this checklist alignment packet.

## Follow-up work

- Run the next visible manual fixture review and confirm the final real-profile/custom boundary prompt is understandable without making the checklist feel too dense.

## Risky assumptions

- Naming both ADR blockers in the compact checklist is clearer than keeping the selected-restore blocker implicit under ADR 0017.
