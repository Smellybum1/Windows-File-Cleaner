# Local Release Path Guard Regression

Date: 2026-06-04

Status: completed

## Goal

Cover the portable release verifier and launcher boundary that explicit `-ReleaseRoot` and `-ReleasePath` values must stay under ignored `.local` before verifier or launch-command output is printed.

## Safety Profile

`terminal-readonly`. The regression uses committed `README.md` only as a non-`.local` rejection target and does not write test files. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, write acceptance notes, write Restore Manifests, or create cleanup history.

## Problem

`Test-LocalRelease.cmd` and `Start-LocalRelease.cmd` already require local release roots and release folders to resolve under ignored `.local`, but MVP preflight did not have a focused regression proving explicit non-`.local` package paths stop before verifier, launcher, or launch-command output. A future edit could loosen the package path boundary while package acceptance note checks still passed.

## Changes

- Added `tools\Test-LocalReleasePathGuards.cmd` and `.ps1`.
- The regression verifies `Test-LocalRelease.cmd -ReleaseRoot README.md` fails before verifier output.
- It verifies `Test-LocalRelease.cmd -ReleasePath README.md` fails before verifier output.
- It verifies `Start-LocalRelease.cmd -ReleaseRoot README.md -PrintOnly -SkipVerify` fails before launcher or launch-command output.
- It verifies `Start-LocalRelease.cmd -ReleasePath README.md -PrintOnly -SkipVerify` fails before launcher or launch-command output.
- `Invoke-MvpPreflight.cmd` now runs the regression by default before local release acceptance command stamping, with `-SkipLocalReleasePathGuardCheck` for focused local loops.

## Verification

- `cmd.exe /c tools\Test-LocalReleasePathGuards.cmd`
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

## ADRs

No ADR added. This tightens terminal-only regression coverage for existing portable release tooling under ADR 0020 and does not change package creation, package acceptance, package promotion, cleanup, restore, persistence, deployment, or app behavior.

## Follow-Up

- Keep package verification and repo-level package launch paths under ignored `.local`.
- Use the accepted-package launcher for daily use until a new package is human-accepted.
