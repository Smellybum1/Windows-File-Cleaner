# Feature: Real-Profile Selected Restore Regression

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add focused WPF regression coverage proving selected real-profile Restore Manifests still cannot execute while ADR 0019 remains design-only.

## Non-goals

- Do not enable real-profile selected restore execution.
- Do not call selected restore execution on a real-profile manifest.
- Do not enable real-profile Quarantine execution.
- Do not enable all-manifest restore, permanent deletion, action-folder cleanup, or cleanup history.
- Do not move, restore, delete, create, or rewrite real-profile files.

## Current behavior

The WPF app already blocks selected restore for custom non-fixture Restore Manifests and allows selected restore for fixture Restore Manifests. ADR 0019 records that real-profile selected restore remains unavailable until a later implementation packet.

## Desired behavior

The app test suite should create a synthetic discovered Restore Manifest whose `CleanupScopePath` is exact `C:\Users\moxhe`, preview selected readiness, type exact `RESTORE`, and prove:

- selected readiness can show restorable evidence,
- the selected restore gate still says `Can execute: no`,
- the scope wording says real-profile/custom selected restore remains unavailable,
- the fixture-only restore button stays disabled,
- no restore method is called,
- the synthetic quarantine source remains in place,
- no real-profile original path is created.

## Decisions made

- The regression does not call `ExecuteSelectedRestoreForCurrentSelection` for a real-profile manifest. If a future bug opened that gate, calling execute could write under `C:\Users\moxhe`, which this packet must not risk.
- The synthetic Restore Manifest is stored under the test fixture's quarantine root, while the manifest cleanup scope and original path are exact real-profile-shaped paths.
- Existing ADR 0019 covers the durable decision; no new ADR is needed.

## Files changed

- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `docs/features/2026-05-31-real-profile-selected-restore-regression.md`
- `README.md`
- `docs/features/2026-05-31-real-profile-selected-restore-execution-contract.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added `MainWindowKeepsSelectedRestoreUnavailableForRealProfileManifest`.
- Added a synthetic real-profile Restore Manifest helper that avoids touching real-profile files.
- Updated docs and handoff notes.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

ADRs added or skipped:

- No ADR added. ADR 0019 already records the selected real-profile restore contract.

Open questions:

- None for this regression packet.

Risky assumptions:

- A synthetic manifest with exact real-profile Cleanup Scope metadata is sufficient to guard the WPF selected-restore availability boundary without touching real-profile files.
