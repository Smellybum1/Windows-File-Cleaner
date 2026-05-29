# Feature: Preflight Fixture Checklist Step

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the full MVP preflight verify that the fixture review checklist can still be printed before the user starts a manual fixture pass.

## Non-goals

- Do not launch WPF from preflight.
- Do not create fixture files from this checklist step.
- Do not scan `C:\Users\moxhe`.
- Do not change Storage Scan, Quarantine Preview, fixture execution, undo, restore, or manifest behavior.

## Desired behavior

- Full preflight runs `Start-MvpFixtureReview.ps1 -ChecklistOnly` after fixture `-WhatIf`.
- Checklist-only output remains a terminal-only prompt: no preflight loop, no fixture creation, and no WPF launch.
- A `-SkipFixtureChecklist` switch exists for focused local loops.
- CI uses the same script and therefore covers the checklist-only path.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a `Fixture checklist` step to `tools/Invoke-MvpPreflight.ps1`.
- Added `-SkipFixtureChecklist` for focused preflight runs.
- Updated README and readiness/preflight docs to include checklist-only coverage.

Tests run:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore` passed.
- `git diff --check` passed with line-ending normalization warnings only.

ADRs added or skipped:

- Skipped. This is local verification tooling only and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None for this packet.

Risky assumptions:

- Printing the checklist during preflight is acceptable noise because it protects the manual review prompts from syntax/path regressions.
