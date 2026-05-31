# Feature: WPF Real-Profile First-Phase Blocker Output

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Keep ADR 0018 first-phase real-profile Quarantine blockers visible in WPF preview and gate output even when many lower-level readiness blockers exist.

## Non-goals

- Do not scan `C:\Users\moxhe`.
- Do not enable real-profile Quarantine execution.
- Do not enable real-profile Undo Quarantine, selected real-profile restore, permanent deletion, or cleanup history.
- Do not create Quarantine Root folders, write Restore Manifests, move files, restore files, or delete files.

## Desired behavior

- The WPF Quarantine Preview and Quarantine Execution Gate continue to cap detailed readiness blockers.
- When the blocker list is long, representative high-signal blockers stay visible before truncation.
- ADR 0018 blockers for the first real-profile phase remain visible: 10-row cap, 1 GB cap, no-category rows, and narrow-folder strict descendant checks.
- Exact `QUARANTINE` remains necessary but insufficient, and `Can approve real-profile movement: no` remains visible for non-fixture scopes.

## Implementation

- Added representative readiness-blocker selection before the WPF pane truncates detailed blocker lines.
- Added `MainWindowShowsRealProfileFirstPhaseBlockersForSyntheticPreview`.
- The test loads synthetic real-profile-shaped scan metadata for exact `C:\Users\moxhe`, shortlists rows that trigger the first-phase batch, no-category, and strict-descendant blockers, and proves preview/gate output keeps those blockers visible.
- The synthetic paths are never created under the real profile; the test does not scan, move, restore, delete, write manifests, or create the Quarantine Root.
- Later checklist alignment added the ADR 0018 first-phase limits to the manual fixture review prompt without changing WPF behavior.
- Later summary-key-blocker alignment added the same first-phase examples to the compact Quarantine Readiness Summary without enabling movement.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

## ADRs

No ADR added. This packet implements visible regression coverage for accepted ADR 0018 without changing the durable decision.

## Open questions

- Manual fixture and real-profile preview review can still decide whether the existing pane is enough or whether a dedicated readiness pane is worth adding later.

## Follow-up work

- Keep real-profile movement unavailable until a later explicit user-approved packet completes the remaining ADR 0018 and ADR 0019 execution prerequisites.

## Risky assumptions

- Synthetic real-profile-shaped metadata is sufficient to cover WPF output formatting without scanning or touching `C:\Users\moxhe`.
