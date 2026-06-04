# Accepted Launcher Notes Path Guard

Date: 2026-06-04

Status: completed

## Goal

Cover the accepted-package launcher boundary that explicit acceptance notes paths must stay under ignored `.local` before any launch-command guidance is printed.

## Safety Profile

`terminal-readonly`. The regression writes temporary ignored acceptance notes and synthetic print-only package placeholder files under `.local`, uses committed `README.md` only as a non-`.local` rejection target, and removes the temporary files. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, write real acceptance notes, write Restore Manifests, or create cleanup history.

## Problem

`Start-AcceptedLocalRelease.cmd` already requires explicit `-AcceptanceNotesPath` values to resolve under ignored `.local`, but the accepted launcher regression only covered incomplete and malformed-looking notes under `.local`. A future edit could accidentally loosen the explicit-path boundary and still pass the default-selection regression.

## Changes

- `tools\Test-AcceptedLocalReleaseSelection.ps1` now invokes the accepted launcher with `README.md` as an explicit non-`.local` notes path.
- The regression asserts the launcher reports the ignored `.local` path boundary.
- It also asserts the launcher stops before accepted launcher output, launch-command printing, or print-only WPF-not-launched output.
- Compact package docs and handoff notes now record the composed explicit-path guard coverage.

## Verification

- `cmd.exe /c tools\Test-AcceptedLocalReleaseSelection.cmd`
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

## ADRs

No ADR added. This tightens terminal-only regression coverage for existing accepted-package launcher tooling under ADR 0020 and does not change package acceptance policy, package promotion, cleanup, restore, persistence, deployment, or app behavior.

## Follow-Up

- Keep accepted-package default selection on the latest completed ignored notes.
- Keep explicit pending-candidate inspection under ignored `.local`; non-`.local` files are not package acceptance notes.
