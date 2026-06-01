# Feature: Portable v1 Acceptance Baseline

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Record the first completed local Portable Release Package acceptance baseline in tracked docs without changing app behavior or promoting ignored acceptance notes into app persistence.

## Non-goals

- Do not launch WPF, click `Scan`, scan, move, restore, delete, approve cleanup, or create cleanup history.
- Do not create an installer, installed shortcut, service, scheduled task, or background automation.
- Do not enable permanent deletion, broad/all-manifest restore, custom real-profile Quarantine, non-exact real-profile movement, or persisted cleanup history.
- Do not copy ignored release artifacts into the tracked repo.

## Context

The user completed the portable package acceptance pass for package `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`. The ignored acceptance notes `.local\release-acceptance\release-acceptance-20260602-011743.md` were recorded with `tools\Record-LocalReleaseAcceptanceNotes.cmd -RecordManualAcceptance`.

`tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` now reports verifier/current-commit/normal-launch/fixture-launch evidence recorded, overall result `Pass`, and `6 pass, 0 issue, 0 not checked, 0 not recorded`.

## Implementation

- Updated README release guidance with the current accepted local package baseline.
- Updated the live-product readiness roadmap's release/packaging row.
- Updated handoff and progress docs so a fresh thread lands on the accepted portable v1 baseline.

## Verification

- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Test-LocalRelease.cmd -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Docs

- `README.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-portable-v1-acceptance-baseline.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This records local package acceptance evidence for the existing Portable Release Package boundary and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Follow-up Work

- Use the accepted portable package for local daily review.
- Cut and accept a fresh package after future code changes that should ship.
- Consider installed shortcut or installer automation only as a separate future packet.

## Risks And Assumptions

- Ignored local acceptance notes are sufficient evidence for the current single-user portable v1 baseline.
- Recording the accepted baseline in tracked docs is helpful, but the ignored package and notes remain the authoritative local artifacts.
