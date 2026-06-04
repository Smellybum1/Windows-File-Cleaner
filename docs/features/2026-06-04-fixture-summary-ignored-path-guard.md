# Fixture Summary Ignored-Path Guard

Date: 2026-06-04

Status: completed

## Goal

Make fixture acceptance summaries enforce the same ignored `.local` notes boundary that fixture acceptance recording already enforces.

## Safety Profile

`terminal-readonly`. The change tightens terminal summary path validation, extends the synthetic fixture-notes regression, and updates committed documentation only. The regression writes temporary ignored notes under `.local\fixture-acceptance-notes-test` and uses committed `README.md` only as a non-`.local` path rejection target. It does not launch WPF, create fixtures, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, create shortcuts, install anything, write real acceptance notes, write Restore Manifests, or create cleanup history.

## Problem

`Summarize-FixtureAcceptanceNotes.cmd` described its output as a read-only summary of local ignored notes, and `Record-FixtureAcceptanceNotes.cmd` already rejected paths outside ignored `.local`. The summary helper only required explicit paths to stay inside the repository, so a committed non-`.local` markdown file could be summarized before recorder guidance was suppressed.

## Changes

- `tools\Summarize-FixtureAcceptanceNotes.ps1` now rejects explicit notes paths outside ignored `.local`.
- `tools\Test-FixtureAcceptanceNotes.ps1` now verifies an explicit in-repo non-`.local` path is rejected before summary output or recorder guidance is printed.
- Fixture acceptance runbook and compact handoff docs now call out that fixture summary paths must stay under ignored `.local`.

## Verification

- `cmd.exe /c tools\Test-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Test-DailyReadinessFixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes`
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

## ADRs

No ADR added. This tightens terminal-only validation for existing fixture acceptance notes tooling and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

## Follow-Up

- Keep fixture acceptance summaries and recorders on ignored `.local` notes only.
- Keep formal fixture notes human-owned until an all-pass visible fixture review is intentionally recorded.
