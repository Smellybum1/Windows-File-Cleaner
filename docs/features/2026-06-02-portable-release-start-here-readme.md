# Feature: Portable Release Start Here Readme

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make each Portable Release Package self-explanatory after unzip by adding a package-local `README-FIRST.txt` that names the launch options and reversible-only v1 safety boundary.

## Non-goals

- Do not create an installer, desktop shortcut, Start Menu entry, service, scheduled task, or background automation.
- Do not launch WPF from the publisher or verifier.
- Do not scan fixture or real-profile files from the publisher or verifier.
- Do not move, restore, delete, approve cleanup, or create cleanup history.
- Do not change Quarantine, selected restore, permanent deletion, broad restore, or history availability.

## Context

The portable release folder already contains the packaged app, metadata, zip, and release-local normal/fixture launch scripts. After unzip, a human still benefits from a plain text "start here" file that travels with the package and restates what portable v1 does and does not do.

## Implementation

- `tools\Publish-LocalRelease.ps1` now writes `README-FIRST.txt` beside `release-metadata.txt`.
- The README names normal launch, fixture launch, and direct executable launch.
- The README states fixture launch only prefills the Cleanup Scope and does not create fixtures or click `Scan`.
- The README repeats that portable v1 is not an installer and excludes permanent deletion, persisted cleanup history, broad/all-manifest restore, custom real-profile Quarantine, and non-exact real-profile movement.
- Release metadata records the README path.
- `tools\Test-LocalRelease.ps1` verifies the README exists, metadata path matches, key safety-boundary lines are present, and the zip includes it.

## Verification

- `cmd.exe /c tools\Publish-LocalRelease.cmd -SkipPreflight`
- `cmd.exe /c tools\Test-LocalRelease.cmd -AllowDirtyPublish -AllowSkippedPreflight`
- `cmd.exe /c tools\Start-LocalRelease.cmd -PrintOnly -AllowDirtyPublish -AllowSkippedPreflight`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-portable-v1-release-packaging.md`
- `docs/features/2026-06-01-portable-release-verifier.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is package usability text and verifier coverage for the existing Portable Release Package, with no installer, persistence, cleanup execution, restore-scope, data-model, or security-boundary change.

## Follow-up Work

- After commit, cut a fresh ignored portable package from clean `main` so the latest strict package includes `README-FIRST.txt` and matches current `HEAD`.
- Consider installed shortcut or installer automation only as a later explicit packaging decision.

## Risks And Assumptions

- A plain text start-here file is enough package-local guidance for v1.
- Repeating safety boundaries in the package helps more than it clutters the folder.
