# Feature: WPF Real-Profile Restore Readiness Evidence

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Wire the existing read-only Real-Profile Restore Readiness model into WPF selected-restore evidence and forward Quarantine readiness display.

This packet should make the recovery prerequisite visible without enabling real-profile restore or real-profile Quarantine execution.

## Non-goals

- Do not enable real-profile selected restore execution.
- Do not enable real-profile Quarantine execution.
- Do not enable all-manifest real-profile restore.
- Do not create folders, write manifests, move files, restore files, delete files, or add cleanup history.
- Do not treat selected restore readiness or exact `RESTORE` as restore approval.

## Desired behavior

After a selected Restore Manifest review and selected restore gate preview exist, WPF should:

- build Real-Profile Restore Readiness from the selected manifest review, confirmation draft, and selected restore gate,
- show compact restore-readiness evidence in the selected restore gate output,
- pass the result into Real-Profile Quarantine Execution Readiness display when available,
- remove the generic restore-readiness-not-checked blocker when restore readiness was actually checked,
- keep selected real-profile restore unavailable because selected-manifest real-profile Undo is not implemented in this build,
- keep forward real-profile Quarantine unavailable.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- ADR 0018 requires Real-Profile Restore Readiness before forward real-profile Quarantine movement.
- ADR 0019 records the future selected-manifest real-profile restore contract but keeps implementation unavailable.
- The core Real-Profile Restore Readiness model already exists and is read-only.
- WPF selected restore gate already has the review, confirmation, and exact `RESTORE` evidence the core builder consumes.

Validation gate:

- [x] Existing domain terms are sufficient.
- [x] Permission boundary is clear: restore readiness is read-only and does not call Undo Quarantine.
- [x] Narrowest relevant check is the WPF app test harness.

## Implementation

- Added WPF storage for the current Real-Profile Restore Readiness result.
- Built restore readiness from selected manifest review, selected restore confirmation draft, and selected restore gate evidence.
- Added compact restore-readiness evidence lines to Selected Restore Execution Gate output.
- Passed restore readiness into `QuarantineExecutionReadinessBuilder` for forward readiness display when available.
- Updated WPF smoke assertions for the synthetic real-profile selected Restore Manifest blocker path.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

## ADRs

No ADR added. ADR 0018 and ADR 0019 already record the durable readiness and selected restore boundaries.

## Open questions

- None for this display-only packet.

## Follow-up work

- Design and implement selected-manifest real-profile restore only after an explicit Grill with Docs pass.
- Keep forward real-profile Quarantine blocked until selected real-profile restore execution is implemented and tested.

## Risky assumptions

- Showing restore-readiness evidence in the selected restore gate is clear enough for now, even before a dedicated readiness pane exists.
