# Feature: First Real-Profile Quarantine Execution

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Enable the first forward real-profile Quarantine execution path for the exact Cleanup Scope `C:\Users\moxhe` after ADR 0017/0018 readiness passes, without enabling custom cleanup scopes, broad Undo Quarantine, permanent deletion, persisted cleanup history, or Codex-driven real-profile movement.

## Non-goals

- Do not run real-profile Quarantine from Codex or automated tests.
- Do not enable custom non-fixture or real-profile-child Quarantine execution.
- Do not enable broad or all-manifest real-profile Undo Quarantine.
- Do not enable permanent deletion.
- Do not add persisted cleanup history.
- Do not clean up action folders.

## Current Behavior

- Fixture Quarantine execution remains available after preview readiness and exact `QUARANTINE`.
- Exact real-profile Quarantine can now execute only when the current Cleanup Scope is exactly `C:\Users\moxhe`, all first-phase readiness blockers are clear, exact `QUARANTINE` is typed, Real-Profile Quarantine Approval Evidence can approve movement, and immediate pre-execution revalidation passes again at click time.
- First-phase real-profile execution remains capped at 10 included rows and 1 GB, limited to `Likely safe` + `Quarantine candidate` rows, with folders allowed only after strict descendant checks.
- Exact real-profile selected restore implementation and the user-reported sacrificial selected-restore trust test are recorded as the recovery prerequisite for forward Quarantine.
- After real-profile Quarantine, the app shows stale-scan guidance and routes recovery through `Discover manifests` plus selected restore. Current-fixture Undo Quarantine remains fixture-only.

## Domain Language Changes

No new durable terms.

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Quarantine Execution Readiness | Updated from display-only readiness to the exact first-phase execution gate for `C:\Users\moxhe`. | yes |
| Real-Profile Quarantine Approval Evidence | Updated from display-only evidence to one input in the exact real-profile execution guard. | yes |

## Grill Notes

### Scenarios Discussed

- The user explicitly chose to start item 6 from the live-product path after successful fixture review, real-profile read-only retest, and selected real-profile restore trust testing.
- The first forward movement remains a user-clicked batch in the WPF app, not a Codex-run action.

### Edge Cases

- Exact `QUARANTINE` alone does not enable movement.
- Synthetic exact real-profile tests without live source files must stay blocked by immediate Pre-Execution Revalidation.
- Custom scopes and real-profile child scopes stay preview-only even when `QUARANTINE` is typed.
- Real-profile execution does not unlock current-fixture Undo Quarantine; selected restore remains the recovery path.

## Decisions Made

Small feature-level decisions:

- Reuse `QuarantineExecutor.Execute` instead of adding a second movement implementation.
- Keep `QuarantineExecutionGate` as the typed-confirmation gate, then add a scope-aware WPF guard that requires Real-Profile Quarantine Approval Evidence for exact real-profile movement.
- Rerun Quarantine Root Execution Safety and Pre-Execution Revalidation immediately before real-profile execution.
- Treat the recorded selected real-profile restore trust test as the recovery prerequisite for the first owner-only forward Quarantine phase.

ADR-worthy decisions:

- [x] No new ADR. ADR 0017 and ADR 0018 already record the first real-profile Quarantine execution contract.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

## Follow-up Work

- Run full `.cmd` MVP preflight before the user performs a real-profile Quarantine batch.
- Have the user review a very small exact real-profile batch, confirm the readiness output, type exact `QUARANTINE`, and click the WPF button only for that specific batch.
- After the first real-profile Quarantine, use `Discover manifests` and selected restore readiness to prove recovery for the created Restore Manifest if needed.
- Keep all-manifest restore, action-folder cleanup, permanent deletion, and persisted cleanup history as separate Grill with Docs decisions.

## Risks And Assumptions

Risks:

- Moving real-profile files is operationally risky even with readiness gates; a user-reviewed small batch is still required.
- Restore Manifest-only recovery may feel sparse until a dedicated history surface exists.

Assumptions:

- The first live cleanup milestone is reversible Quarantine plus selected-manifest restore, not deletion or cleanup history.
- The selected real-profile restore trust test is sufficient recovery confidence for the first owner-only forward batch.
