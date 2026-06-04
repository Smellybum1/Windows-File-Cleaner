# Fixture Root Path Guard Regression

Date: 2026-06-04

Status: completed

## Goal

Require synthetic fixture creation and fixture review launch roots to stay under ignored `.local` before fixture writes, checklist output, or WPF launch can happen.

## Safety Profile

`terminal-readonly`. The regression uses committed `README.md` only as a non-`.local` rejection target and does not write test files. It does not launch WPF, create fixture files, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, write acceptance notes, write Restore Manifests, install anything, or create cleanup history.

## Problem

`New-StorageScanSmokeFixture.cmd` and `Start-MvpFixtureReview.cmd` already resolved fixture roots and rejected paths outside the repository, but they still allowed arbitrary in-repo roots. Because the fixture creator writes fixed synthetic files under the requested root, an accidental non-`.local` in-repo root could create or overwrite repository-local files before the user saw fixture output.

## Changes

- `New-StorageScanSmokeFixture.cmd` now requires `-Root` to resolve under ignored `.local`.
- `Start-MvpFixtureReview.cmd` now requires `-FixtureRoot` to resolve under ignored `.local`.
- Added `tools\Test-FixtureRootPathGuard.cmd` and `.ps1`.
- The regression verifies `New-StorageScanSmokeFixture.cmd -Root README.md -WhatIf` fails before WhatIf fixture-file output.
- The regression verifies `Start-MvpFixtureReview.cmd -FixtureRoot README.md -ChecklistOnly` fails before fixture checklist output.
- `Invoke-MvpPreflight.cmd` now runs the regression by default before fixture dry-run output, with `-SkipFixtureRootPathGuardCheck` for focused local loops.

## Verification

- `cmd.exe /c tools\Test-FixtureRootPathGuard.cmd`
- `cmd.exe /c tools\New-StorageScanSmokeFixture.cmd -WhatIf`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

## ADRs

No ADR added. This tightens terminal fixture tooling guardrails and preflight coverage without changing app cleanup behavior, restore behavior, persistence, package acceptance, deployment, or real-profile movement policy.

## Follow-Up

- Keep synthetic fixture writes under ignored `.local`.
- Continue using fixture review only as synthetic fixture evidence, not real-profile movement approval.
