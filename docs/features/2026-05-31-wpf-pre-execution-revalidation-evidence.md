# Feature: WPF Pre-Execution Revalidation Evidence

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Wire the existing read-only Pre-Execution Revalidation model into WPF Quarantine Preview and Quarantine Execution Gate output.

This packet should make another real-profile readiness dimension visible without enabling any new cleanup execution.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile Undo Quarantine or selected restore.
- Do not run Real-Profile Restore Readiness from WPF.
- Do not create folders, write manifests, move files, restore files, delete files, or add cleanup history.
- Do not treat preview-time revalidation as final approval; future real-profile movement must rerun revalidation immediately before movement.

## Desired behavior

After a Quarantine Preview can build a Quarantine Action Draft and Quarantine Root Execution Safety, WPF should:

- build Pre-Execution Revalidation from the current preview, confirmation draft, action draft, and root-safety evidence,
- pass the result into Real-Profile Quarantine Execution Readiness display,
- show a compact revalidation evidence line in both preview and gate output,
- remove the generic revalidation-not-checked blocker when revalidation was actually checked,
- show path-specific revalidation blockers such as missing source paths,
- keep Real-Profile Restore Readiness blocked until that evidence model is wired,
- keep `CanExecuteQuarantine` false for real-profile and custom non-fixture scopes.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- ADR 0018 requires immediate Pre-Execution Revalidation before any real-profile movement.
- The core Pre-Execution Revalidation model already exists and is read-only.
- WPF now has enough action-draft and root-safety evidence after preview to run a read-only revalidation check for display.

Validation gate:

- [x] Existing domain terms are sufficient.
- [x] Permission boundary is clear: revalidation reads path state only and does not create folders or manifests.
- [x] Narrowest relevant check is the WPF app test harness.

## Implementation

- Added WPF storage for the current Pre-Execution Revalidation result.
- Built revalidation after Quarantine Preview when action draft and root-safety evidence exist.
- Passed revalidation into `QuarantineExecutionReadinessBuilder`.
- Added compact revalidation evidence lines to Quarantine Preview and Quarantine Execution Gate output.
- Updated WPF smoke assertions for fixture and synthetic real-profile readiness output.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

## ADRs

No ADR added. ADR 0018 already records Pre-Execution Revalidation as a durable readiness dimension.

## Open questions

- None for this display-only packet.

## Follow-up work

- Wire WPF Real-Profile Restore Readiness evidence later while keeping real-profile execution disabled.
- Rerun Pre-Execution Revalidation immediately before any future real-profile move, not only at preview time.

## Risky assumptions

- Showing preview-time revalidation evidence is useful as readiness context as long as the UI states it must run again immediately before future movement.
