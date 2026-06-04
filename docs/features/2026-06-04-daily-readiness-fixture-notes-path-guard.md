# Daily Readiness Fixture Notes Path Guard

Date: 2026-06-04

Status: completed

## Goal

Cover the daily readiness fixture acceptance notes path boundary so explicit fixture notes paths outside ignored `.local` fail before accepted launch-command printing.

## Safety Profile

`terminal-readonly`. The regression writes temporary ignored synthetic package files, package acceptance notes, fixture acceptance notes, and an empty Restore Manifest root under `.local\daily-readiness-fixture-acceptance-test`, uses committed `README.md` only as a non-`.local` rejection target, and removes the temporary test folder. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, write real acceptance notes, write Restore Manifests, or create cleanup history.

## Problem

Fixture acceptance summaries already rejected explicit notes paths outside ignored `.local`, but the daily readiness wrapper forwards `-FixtureAcceptanceNotesPath` before printing accepted launch commands. Without a composed regression, that wrapper could accidentally loosen the fixture-note boundary or print launch-command guidance after a rejected fixture notes path.

## Changes

- `tools\Test-DailyReadinessFixtureAcceptanceNotes.ps1` now verifies a non-`.local` explicit fixture notes path fails during the daily readiness `Fixture acceptance notes evidence` step.
- The regression asserts the fixture summary guard message is visible in daily readiness output.
- The regression asserts daily readiness stops before `== Accepted normal launch command ==`.
- Daily-use, manual fixture review, compact handoff docs, and the feature index now record the composed path-boundary coverage.

## Verification

- `cmd.exe /c tools\Test-DailyReadinessFixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Test-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes`
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

## ADRs

No ADR added. This tightens terminal-only regression coverage for existing fixture acceptance notes and daily readiness tooling and does not change cleanup execution, restore execution, persistence, deployment, package acceptance policy, or app behavior.

## Follow-Up

- Keep explicit fixture acceptance notes paths under ignored `.local` for both standalone summaries and daily readiness.
- Keep formal fixture notes human-owned until an all-pass visible fixture review is intentionally recorded.
