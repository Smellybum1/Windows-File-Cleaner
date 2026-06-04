# Real-Profile Selected Restore Trust Helper Path Guard

Date: 2026-06-04

Status: completed

## Goal

Keep the sacrificial real-profile selected restore trust helper's `QuarantineRoot` under the default `D:\WindowsFileCleanerQuarantine` root or ignored repo `.local`, and keep generated quarantine sources inside the action `items` root even when the requested `RelativePath` normalizes back inside `C:\Users\moxhe`.

## Non-Goals

- Do not run real-profile restore.
- Do not create a real trust manifest.
- Do not launch WPF.
- Do not change ADR 0019 selected-restore execution policy.
- Do not enable broad/all-manifest restore, custom selected restore, permanent deletion, action-folder cleanup, or cleanup history.

## Safety Profile

`terminal-readonly`. The regression runs the helper only with `-WhatIf`, an ignored `.local` Quarantine Root for safe preview, and committed `README.md` as a non-`.local` rejection target. It does not launch WPF, scan, move, restore, delete, approve cleanup, write Restore Manifests, modify real-profile files, install anything, or create cleanup history.

## Problem

`New-RealProfileSelectedRestoreTrustManifest.cmd` already rejects absolute `RelativePath` values and verifies the restore target resolves under the current `C:\Users\moxhe` profile. A path such as `..\moxhe\...` can still normalize to a restore target inside the profile while causing the generated quarantine source to escape the action `items` folder. That would violate the action-scoped Restore Manifest layout expected by ADR 0019 revalidation.

The helper is also write-capable outside `-WhatIf`, so explicit `QuarantineRoot` values should stay inside the normal default Quarantine Root or ignored `.local` test area before any preview, manifest, or app-command output.

## Changes

- `New-RealProfileSelectedRestoreTrustManifest.cmd` now rejects explicit `QuarantineRoot` values outside the default `D:\WindowsFileCleanerQuarantine` root and ignored repo `.local`.
- `New-RealProfileSelectedRestoreTrustManifest.cmd` now rejects generated quarantine source paths that do not resolve under the action `items` root.
- Added `tools\Test-RealProfileSelectedRestoreTrustManifestPathGuard.cmd` and `.ps1`.
- The regression verifies a safe relative path passes in `-WhatIf` mode with an ignored `.local` Quarantine Root.
- The regression verifies committed `README.md` as a non-`.local` `QuarantineRoot` fails before preview, manifest, or app-command output.
- The regression verifies an escaped `..\moxhe\...` relative path fails before preview, manifest, or app-command output.
- The regression asserts no ignored test Quarantine Root is created in `-WhatIf` mode and bounds any cleanup under repo `.local`.

## Verification

- `cmd.exe /c tools\Test-RealProfileSelectedRestoreTrustManifestPathGuard.cmd`
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

## ADRs

No ADR added. ADR 0019 already requires real-profile selected restore to refuse quarantine source paths that are outside the selected manifest's action layout. This packet enforces that invariant in the sacrificial trust-helper generator and does not change restore execution policy.

## Follow-Up

- Keep this helper as a human-intent trust-test tool only.
- Do not use the helper as cleanup approval or as approval for another real-profile Quarantine batch.
