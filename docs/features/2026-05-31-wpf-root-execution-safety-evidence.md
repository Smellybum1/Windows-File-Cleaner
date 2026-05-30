# Feature: WPF Root Execution Safety Evidence

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Wire the existing read-only Quarantine Root Execution Safety model into WPF Quarantine Preview and Quarantine Execution Gate output.

This packet should make one real-profile readiness dimension more concrete without enabling any new cleanup execution.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile Undo Quarantine or selected restore.
- Do not run Pre-Execution Revalidation from WPF.
- Do not run Real-Profile Restore Readiness from WPF.
- Do not create folders, write manifests, move files, restore files, delete files, or add cleanup history.
- Do not add non-`D:` Quarantine Root acknowledgement UI yet.

## Desired behavior

After a Quarantine Preview can build a Quarantine Action Draft, WPF should:

- build Quarantine Root Execution Safety from that action draft,
- pass the result into Real-Profile Quarantine Execution Readiness display,
- show a compact root-safety evidence line in both preview and gate output,
- remove the generic root-safety-not-checked blocker when root safety was actually checked,
- keep Pre-Execution Revalidation and Real-Profile Restore Readiness blockers visible until those evidence models are wired,
- keep `CanExecuteQuarantine` false for real-profile and custom non-fixture scopes.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- ADR 0018 requires Quarantine Root Execution Safety as one readiness dimension before any real-profile movement.
- The core Quarantine Root Execution Safety model already exists and is read-only.
- Previous WPF readiness output showed root safety as missing even when an action draft had enough information to evaluate it.

Validation gate:

- [x] Existing domain terms are sufficient.
- [x] Permission boundary is clear: root safety checks are read-only and do not create folders or manifests.
- [x] Narrowest relevant check is the WPF app test harness.

## Implementation

- Added WPF storage for the current Quarantine Root Execution Safety result.
- Built root safety from the current Quarantine Action Draft after Quarantine Preview.
- Passed root safety into `QuarantineExecutionReadinessBuilder`.
- Added compact root-safety evidence lines to Quarantine Preview and Quarantine Execution Gate output.
- Updated WPF smoke assertions for fixture and synthetic real-profile readiness output.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

## ADRs

No ADR added. ADR 0018 already records Quarantine Root Execution Safety as a required readiness dimension.

## Open questions

- None for this display-only packet.

## Follow-up work

- Wire WPF Pre-Execution Revalidation evidence as a later read-only packet while keeping real-profile execution disabled.
- Add non-`D:` acknowledgement UI only after a small Grill with Docs pass, because that affects real-profile approval semantics.

## Risky assumptions

- Showing root safety evidence in the existing preview/gate panes is clear enough for now, without a dedicated readiness pane.
