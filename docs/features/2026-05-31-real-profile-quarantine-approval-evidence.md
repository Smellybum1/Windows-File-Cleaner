# Feature: Real-Profile Quarantine Approval Evidence

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Make ADR 0018's explicit approval rule testable without enabling real-profile file movement.

Exact `QUARANTINE` should be recorded as necessary but not sufficient: real-profile movement still requires exact scope, clean readiness, immediate revalidation, root safety, recovery readiness, and a future explicit movement-availability signal.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not change fixture-only Quarantine execution.
- Do not enable real-profile selected restore, broad Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not scan, move, restore, delete, create, rewrite, or clean up real-profile files.
- Do not add WPF controls in this packet.

## Current behavior

- Real-Profile Quarantine Execution Readiness can name fixture, real-profile, real-profile-child, and custom states while keeping real-profile movement blocked.
- The current WPF Quarantine Execution Gate can open only for fixture Cleanup Scopes.
- Exact `QUARANTINE` remains the shared confirmation phrase.

## Implementation

- Added `RealProfileQuarantineApprovalEvidence` and `RealProfileQuarantineApprovalEvidenceBuilder`.
- The builder trims typed confirmation text like the existing execution gate, records exact confirmation matches, carries readiness blockers forward, rejects non-exact real-profile scopes, and defaults movement availability to unavailable.
- Added the approval-evidence builder to the read-only readiness-builder source guard so it cannot call movement executors, write manifests, or perform direct filesystem movement/write operations.

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added the read-only approval-evidence core model.
- Added core tests proving exact `QUARANTINE` plus current real-profile readiness still cannot approve movement in this build.
- Added core tests proving wrong confirmation text, custom scopes, and missing current-build movement availability block approval evidence, while a synthetic future clean readiness plus explicit availability can pass the model.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/features/2026-05-31-real-profile-quarantine-approval-evidence.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No new ADR added. This implements one accepted ADR 0018 follow-up without changing the durable decision or enabling movement.

Follow-up work:

- WPF now surfaces this evidence in the Quarantine Execution Gate for non-fixture preview-only scopes without enabling movement.
- Keep real-profile movement blocked until a later explicit user-approved packet wires execution after readiness, approval evidence, and recovery behavior are complete.

Open questions:

- None for this core-only packet.

Risky assumptions:

- A core approval-evidence seam is useful before WPF movement exists because it makes the future approval boundary testable without crossing the file-movement boundary.
