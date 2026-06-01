# Feature: Real-Profile Selected Restore Execution

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Implement the first real-profile restore movement path behind ADR 0019 without enabling real-profile Quarantine execution, all-manifest restore, permanent deletion, or cleanup history.

## Non-goals

- Do not run real-profile restore during automated tests or Codex verification.
- Do not enable real-profile Quarantine execution.
- Do not enable broad or all-manifest real-profile Undo Quarantine.
- Do not enable custom non-fixture selected restore.
- Do not add permanent deletion.
- Do not add persisted cleanup history.
- Do not clean up empty quarantine action folders.

## Current Behavior

- WPF can restore selected discovered fixture Restore Manifests after selected manifest readiness and exact `RESTORE`.
- WPF can now open exact real-profile selected restore only for Restore Manifests whose Cleanup Scope is exactly `C:\Users\moxhe`, after selected manifest readiness, exact `RESTORE`, and passing Selected Restore Pre-Execution Revalidation.
- WPF folds failing real-profile selected-restore revalidation into the executable gate so stale missing quarantine paths, original-path collisions, recovery-review rows, not-moved rows, and other readiness blockers keep the button disabled.
- `ExecuteSelectedRestoreForCurrentSelection` reruns Selected Restore Pre-Execution Revalidation immediately before calling `UndoQuarantineExecutor.Undo` for an exact real-profile selected manifest.
- Custom non-fixture selected restore and non-exact real-profile selected restore remain unavailable.
- Real-profile Quarantine execution, all-manifest restore, permanent deletion, action-folder cleanup, and persisted cleanup history remain unavailable.

## Domain Language Changes

No new durable terms.

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Selected Restore Execution | Moved from future contract to implemented selected-manifest WPF path under ADR 0019. | yes |
| Selected Restore Pre-Execution Revalidation | Clarified that WPF shows it as preview evidence and reruns it immediately before exact real-profile selected restore movement. | yes |

## Grill Notes

### Scenarios Discussed

- The user approved starting items 6 and 7 from the live-product path: ADR 0019 selected real-profile restore implementation first, before real-profile Quarantine execution.
- The first implementation remains selected-manifest-only and exact `C:\Users\moxhe` only.
- Real-profile Quarantine execution waits until selected real-profile restore is implemented and manually trusted.

### Edge Cases

- Exact `RESTORE` is not sufficient when revalidation detects stale or unsafe state.
- Missing quarantine source paths keep exact real-profile selected restore disabled.
- Custom non-fixture manifests stay preview-only even if selected readiness is clean and exact `RESTORE` is typed.
- Automated WPF coverage may open the gate with a synthetic exact real-profile manifest, but must not click restore and write into `C:\Users\moxhe`.

## Decisions Made

Small feature-level decisions:

- Reuse `SelectedRestorePreExecutionRevalidationBuilder` instead of adding a second real-profile restore checker.
- Keep `SelectedRestoreExecutionGate` as the user-facing gate, but add revalidation blockers to the WPF gate when exact real-profile revalidation fails.
- Keep the restore button label scope-neutral: `Restore selected manifest`.
- Keep custom selected restore unavailable.

ADR-worthy decisions:

- [x] No new ADR. ADR 0019 already records the selected real-profile restore execution contract.

## Implementation Notes

- WPF selected restore execution availability is now limited to fixture scopes or the exact default real-profile Cleanup Scope.
- Exact real-profile selected restore requires `_currentSelectedRestorePreExecutionRevalidation.CanProceed` before the button enables.
- Execution reruns revalidation immediately before `UndoQuarantineExecutor.Undo`.
- If immediate revalidation fails, WPF refreshes the gate text and status with no file movement.
- The WPF smoke tests prove:
  - custom selected restore remains unavailable,
  - exact real-profile selected restore can open after clean readiness, exact `RESTORE`, and passing revalidation without executing movement in the test,
  - stale exact real-profile selected restore remains blocked when the quarantine source disappears,
  - WPF movement executor calls remain inside the known gated bridge methods.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

The first parallel app-test attempt failed because both test projects tried to rebuild the same core DLL at the same time. Rerunning the WPF app tests by themselves passed.

## Follow-up Work

- The user completed the sacrificial selected real-profile restore trust test on 2026-06-01 and reported steps 1-9 all succeeded.
- The user later tried selected restore for the first live Quarantine manifest and hit a cross-volume directory restore failure; the follow-up packet `2026-06-01-cross-volume-selected-restore-retry.md` fixed directory restore and retryability for this selected-manifest recovery case.
- Run a full `.cmd` MVP preflight before any manual real-profile restore attempt.
- Have the user manually inspect the exact selected Restore Manifest and click restore only when they explicitly choose to test real-profile selected restore.
- Keep real-profile Quarantine execution deferred until a separate ADR 0017/0018 first-movement Grill with Docs packet and explicit user approval for the specific batch.
- Keep all-manifest restore, action-folder cleanup, permanent deletion, and cleanup history as later separate Grill with Docs decisions.

## Risks And Assumptions

Risks:

- A real-profile restore can recreate parent folders and move files into `C:\Users\moxhe`; the immediate revalidation and exact selected-manifest gate reduce but do not remove that operational risk.
- Restore Manifest-only history may feel sparse after real movement.

Assumptions:

- Selected-manifest restore is the right first recovery path before forward real-profile Quarantine.
- Manual rediscover/rescan after restore remains acceptable.
