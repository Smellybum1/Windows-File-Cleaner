# Real-Profile Selected Restore Trust Helper Preflight Regression

Date: 2026-06-04

Status: completed

## Goal

Run the sacrificial real-profile selected restore trust helper path guard regression in normal MVP preflight, and make its escaped-path case portable across local Windows and GitHub Actions users.

## Non-Goals

- Do not run real-profile restore.
- Do not create a real trust manifest.
- Do not launch WPF.
- Do not change ADR 0019 selected-restore execution policy.
- Do not enable broad/all-manifest restore, custom selected restore, permanent deletion, action-folder cleanup, or cleanup history.

## Safety Profile

`terminal-readonly`. The regression runs the helper only with `-WhatIf`, an ignored `.local` Quarantine Root for safe preview, and committed `README.md` as a non-`.local` rejection target. It does not launch WPF, scan, move, restore, delete, approve cleanup, write Restore Manifests, modify real-profile files, install anything, or create cleanup history.

## Problem

The trust-helper path guard was useful as a local targeted check, but it was not part of the normal preflight contract. Its escaped `RelativePath` also named `..\moxhe\...`, which proved the local profile case but would not exercise the same action-`items` guard on hosted runners with a different user profile name.

## Changes

- `tools\Test-RealProfileSelectedRestoreTrustManifestPathGuard.cmd` now derives the escaped path's profile segment from the helper's safe preview output.
- `tools\Invoke-MvpPreflight.cmd` now runs the trust-helper path guard regression by default.
- Added `-SkipRealProfileSelectedRestoreTrustHelperPathGuardCheck` for focused local preflight loops.
- Compact docs and runbooks now list the trust-helper path guard as default MVP preflight coverage.

## Verification

- `cmd.exe /c tools\Test-RealProfileSelectedRestoreTrustManifestPathGuard.cmd`
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

## ADRs

No ADR added. This only broadens terminal-only regression coverage for an existing helper invariant under ADR 0019 and does not change selected-restore execution policy.

## Follow-Up

- Keep this helper as a human-intent trust-test tool only.
- Do not use the helper as cleanup approval or as approval for another real-profile Quarantine batch.
