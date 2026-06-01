# Feature: Fixture Checklist Grid-Mode Cue Wording

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make the fixture checklist's current-session review step unambiguous before the next visible fixture pass.

## Non-goals

- Do not change WPF behavior.
- Do not change Storage Scan, Review Shortlist, Quarantine Preview, fixture execution, undo, selected restore, manifest discovery, or readiness behavior.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not scan, create, move, restore, delete, write, or clean up real-profile files.

## Implementation

- Clarified checklist step 7 so the hoverable `?` cue and state-naming tooltip/help text are explicitly attached to Review Grid Mode Status.
- Clarified that Main Grid Active Review Lens Summary appears for Storage Scan rows and hides for current-session quarantined rows.
- Kept checklist-only mode terminal-output-only.

## Test Plan

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `rg -n "Main Grid active review lens summary when scan rows are showing, and its hoverable|Main Grid active review lens summary.*and its hoverable" tools\Start-MvpFixtureReview.ps1 README.md docs\codex\thread-handoff.md`
- `git diff --check`

## Completion Notes

Completed on: 2026-06-01

ADRs:

- No ADR added. This is fixture checklist wording only, with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision.

Open questions:

- During the next visible fixture pass, confirm whether Review Grid Mode Status plus Main Grid Active Review Lens Summary are clear enough together.
