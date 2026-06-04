# Daily Readiness Fixture Acceptance Regression

Date: 2026-06-04

Status: completed

## Goal

Cover daily readiness fixture acceptance notes forwarding so optional fixture-note visibility stays read-only, strict fixture-note completion fails before launch-command printing, and complete notes can pass the normal daily readiness flow without relying on ignored real package artifacts.

## Safety Profile

`terminal-readonly`. The regression writes temporary ignored package files, complete package acceptance notes, fixture acceptance notes, and an empty synthetic Restore Manifest root under `.local\daily-readiness-fixture-acceptance-test`, runs daily readiness with explicit paths, and removes the test folder. It does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

## Problem

Daily readiness can include fixture acceptance notes with `-IncludeFixtureAcceptanceNotes` or require them with `-RequireFixtureAcceptanceComplete`, but default preflight did not cover that forwarding path. The standalone fixture notes tools were covered, while the daily readiness composition could still regress.

## Changes

- Added `tools\Test-DailyReadinessFixtureAcceptanceNotes.cmd` and `.ps1`.
- The regression creates a verifier-valid synthetic local release package and complete package acceptance notes under ignored `.local`.
- It verifies `Invoke-DailyLocalReadiness.cmd -RequireFixtureAcceptanceComplete` fails on incomplete fixture notes during the fixture-notes step and before accepted launch-command printing.
- It verifies optional incomplete fixture notes still print recording guidance and allow the read-only daily flow to continue.
- It verifies complete fixture notes pass strict daily readiness, accepted launch commands remain print-only, and Restore Manifest summary uses the synthetic empty root.
- `Invoke-MvpPreflight.cmd` now runs this regression by default after the standalone fixture acceptance notes regression.
- Added `Invoke-MvpPreflight.cmd -SkipDailyReadinessFixtureAcceptanceCheck` for focused local loops.

## Verification

- `cmd.exe /c tools\Test-DailyReadinessFixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Test-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

## ADRs

No ADR added. This adds terminal-only regression coverage for existing daily readiness and fixture acceptance notes tooling and does not change cleanup execution, restore execution, persistence, deployment, package acceptance policy, or app behavior.

## Follow-Up

- Keep using `Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes` when fixture notes should be visible in daily evidence, and `-RequireFixtureAcceptanceComplete` only when incomplete fixture notes should fail the check.
