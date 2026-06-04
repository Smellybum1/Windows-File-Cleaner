# Real-Profile Quarantine Inline Status Wording

Date: 2026-06-04

Status: completed

## Goal

Fix the Quarantine tab inline post-execution status so exact real-profile Quarantine results are not described as fixture Quarantine results.

## Non-Goals

- Do not change Quarantine execution gates.
- Do not widen real-profile movement.
- Do not add broad/all-manifest real-profile Undo Quarantine.
- Do not launch WPF, scan, move, restore, delete, approve cleanup, write Restore Manifests, create shortcuts, install anything, or create cleanup history.

## Safety

This was a code and test wording packet only. Verification used automated WPF app tests with fixture and synthetic in-memory real-profile evidence.

## Implementation

- Updated inline Quarantine Preview status to reuse the existing real-profile-aware execution status formatter.
- Added a WPF app regression test that injects a synthetic exact-profile `QuarantineExecutionResult` into the in-memory window state and verifies the inline status says `Real-profile Quarantine execution completed`, names Discover manifests and selected restore recovery, and does not mention fixture Undo.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

## ADRs

No ADR added. This corrects UI wording under existing ADR 0017, ADR 0018, and ADR 0019 gates without changing cleanup execution, restore behavior, persistence, deployment, data model, or security policy.

## Follow-Up

- Continue to stop after the 2026-06-04 second exact-profile batch while exact-profile displayed undo work is present.
