# Feature: Real-Profile Readiness Output Regression

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add WPF regression coverage proving a real-profile-shaped Quarantine Preview shows the Real-Profile Quarantine Execution Readiness contract while keeping execution disabled.

## Non-goals

- Do not scan `C:\Users\moxhe`.
- Do not enable real-profile Quarantine execution.
- Do not enable real-profile Undo Quarantine or selected restore.
- Do not create Quarantine Root folders, write Restore Manifests, move files, restore files, delete files, or add cleanup history.

## Desired behavior

The WPF app test suite should be able to load synthetic scan-result metadata whose Cleanup Scope is exact `C:\Users\moxhe`, shortlist a synthetic likely-safe Quarantine candidate, create a dry-run Quarantine Preview, type exact `QUARANTINE`, and prove:

- the preview and gate show `real-profile candidate` readiness,
- the readiness scope is `real profile`,
- current-build execution remains `no`,
- read-only Quarantine Root Execution Safety evidence is consumed when available, while missing Pre-Execution Revalidation and Real-Profile Restore Readiness blockers remain grouped visibly,
- `CanExecuteQuarantine` remains false,
- no Quarantine Root folder is created.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- ADR 0018 requires WPF readiness output to name missing real-profile prerequisites while keeping execution disabled.
- `docs/features/2026-05-31-wpf-execution-readiness-output.md` said custom and real-profile scopes should show readiness output, but its completion notes only recorded fixture and custom assertions.
- Testing this with an actual real-profile scan would be too broad for an autonomous packet.

Validation gate:

- [x] Existing domain terms are sufficient.
- [x] Permission boundary is clear: synthetic metadata only, no real-profile scan.
- [x] Narrowest relevant check is the WPF app test harness.

## Implementation

- Added `MainWindowShowsRealProfileReadinessContractForSyntheticPreview`.
- The test invokes the existing WPF scan-result application path with a synthetic `StorageScanResult` for exact `C:\Users\moxhe`.
- The synthetic candidate path is real-profile-shaped but never created, read, moved, restored, or deleted.
- Later WPF root-safety evidence wiring updated the test to assert Quarantine Root Execution Safety is checked while Pre-Execution Revalidation and Real-Profile Restore Readiness blockers remain grouped.
- The test does not call Quarantine execution; it asserts the gate remains closed after exact `QUARANTINE`.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

## ADRs

No ADR added. ADR 0018 already records the durable readiness-output requirement.

## Open questions

- None for this regression packet.

## Follow-up work

- WPF now wires read-only Quarantine Root Execution Safety evidence. Later packets can wire Pre-Execution Revalidation and Real-Profile Restore Readiness evidence without enabling movement.

## Risky assumptions

- Synthetic real-profile scan-result metadata is enough to cover WPF readiness-output formatting without scanning or touching `C:\Users\moxhe`.
