# Daily Readiness Latest Notes Isolation

Date: 2026-06-04

Status: completed

## Goal

Let daily readiness latest package notes regression coverage use explicit synthetic notes under its private ignored `.local` test folder instead of temporarily shadowing the default package acceptance notes search root.

## Safety Profile

`terminal-readonly`. The change adds an explicit latest package acceptance notes path for the daily readiness informational block, keeps the latest-package regression's synthetic notes under `.local\daily-readiness-latest-package-notes-test`, and updates committed documentation. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, write real acceptance notes, write real Restore Manifests, or create cleanup history.

## Problem

`Test-DailyReadinessLatestPackageNotes.cmd` previously wrote temporary synthetic notes under `.local\release-acceptance` because daily readiness had no way to point the informational latest-notes block at a specific notes file. The test cleaned up after itself, but while it was running those temporary notes could shadow normal daily readiness default selection if another read-only daily check ran at the same time.

## Changes

- `Invoke-DailyLocalReadiness.cmd` now accepts `-LatestPackageAcceptanceNotesPath` for the informational latest package notes block.
- `-SyntheticRestoreManifestOnly` rejects `-LatestPackageAcceptanceNotesPath` along with other package or fixture acceptance notes parameters.
- `tools\Test-DailyReadinessLatestPackageNotes.ps1` now writes its complete, incomplete, and malformed-looking package acceptance notes under `.local\daily-readiness-latest-package-notes-test` and passes explicit latest-notes paths.
- `tools\Test-DailyReadinessExactProfileUndoSpotlight.ps1` now asserts the synthetic Restore Manifest-only mode rejects the new parameter.

## Verification

- `cmd.exe /c tools\Test-DailyReadinessLatestPackageNotes.cmd`
- `cmd.exe /c tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd`
- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -LatestPackageAcceptanceNotesPath ".local\release-acceptance\release-acceptance-20260604-164509.md"`
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

## ADRs

No ADR added. This is terminal-only test isolation and explicit informational-summary routing for existing daily readiness tooling and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or app behavior.

## Follow-Up

- Keep the normal daily readiness default on latest ignored package acceptance notes.
- Use `-LatestPackageAcceptanceNotesPath` for focused tests or explicit pending-note inspection when avoiding default-search side effects matters.
