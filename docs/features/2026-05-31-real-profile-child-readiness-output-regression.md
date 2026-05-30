# Feature: Real-Profile Child Readiness Output Regression

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add WPF regression coverage proving a Cleanup Scope under `C:\Users\moxhe` remains preview-only for Quarantine readiness output in the first real-profile phase.

## Non-goals

- Do not scan `C:\Users\moxhe` or any child folder under it.
- Do not enable real-profile child Quarantine execution.
- Do not enable real-profile Quarantine execution, Undo Quarantine, selected restore, permanent deletion, or cleanup history.
- Do not create Quarantine Root folders, write Restore Manifests, move files, restore files, or delete files.

## Desired behavior

The WPF app test suite should load synthetic scan-result metadata for a child Cleanup Scope shaped like `C:\Users\moxhe\AppData\Local`, shortlist a synthetic likely-safe Quarantine candidate, create a dry-run Quarantine Preview, type exact `QUARANTINE`, and prove:

- the readiness scope is `real-profile child`,
- the readiness disposition is preview-only,
- the exact first-phase scope blocker names `C:\Users\moxhe`,
- `CanExecuteQuarantine` remains false,
- no Quarantine Root folder is created.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- ADR 0018 limits the first real-profile Quarantine phase to exact `C:\Users\moxhe`.
- Core tests already prove real-profile child scopes stay preview-only.
- WPF readiness-output coverage now proves exact real-profile candidate output, but did not yet prove child-scope output.

Validation gate:

- [x] Existing domain terms are sufficient.
- [x] Permission boundary is clear: synthetic metadata only, no real-profile scan.
- [x] Narrowest relevant check is the WPF app test harness.

## Implementation

- Added `MainWindowShowsRealProfileChildReadinessContractForSyntheticPreview`.
- Reused the synthetic WPF scan-result application path without touching the filesystem under the real profile.
- Renamed the synthetic scan-result builder to clarify it can build real-profile-shaped exact or child scope metadata.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

## ADRs

No ADR added. ADR 0018 already records the exact first-phase real-profile scope rule.

## Open questions

- None for this regression packet.

## Follow-up work

- Keep real-profile child scopes preview-only until a later Grill with Docs pass explicitly designs a broader execution mode.

## Risky assumptions

- Synthetic real-profile child scan-result metadata is enough to cover WPF readiness-output formatting without scanning or touching `C:\Users\moxhe`.
