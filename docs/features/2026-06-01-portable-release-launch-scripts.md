# Feature: Portable Release Launch Scripts

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make each Portable Release Package easier to run by writing ignored release-local launch scripts for normal app launch and fixture-scope launch.

## Non-goals

- Do not create an installer, service, scheduled task, Start Menu entry, or desktop shortcut.
- Do not launch WPF from the publisher or verifier.
- Do not scan fixture or real-profile files from the publisher or verifier.
- Do not move, restore, delete, or create cleanup history.
- Do not change cleanup, Quarantine, restore, deletion, or history availability.

## Current Behavior

The publisher prints normal and fixture launch commands, but the user has to copy those terminal commands or browse directly to the packaged executable.

## Implementation

- `tools\Publish-LocalRelease.ps1` now writes `Launch-WindowsFileCleaner.cmd` beside `release-metadata.txt`.
- `tools\Publish-LocalRelease.ps1` now writes `Launch-WindowsFileCleaner-Fixture.cmd` beside `release-metadata.txt`; this launches the packaged executable with the local smoke fixture Cleanup Scope.
- Release metadata records both launch script paths and the fixture Cleanup Scope.
- The release zip includes both launch scripts.
- `tools\Test-LocalRelease.ps1` verifies the launch scripts exist, metadata paths match, the fixture launch script has an explicit fixture Cleanup Scope, and both launch scripts are included in the zip.
- The scripts are ignored local release artifacts, not installed shortcuts.

## Verification

- `cmd.exe /c tools\Publish-LocalRelease.cmd -SkipPreflight`
- `cmd.exe /c tools\Test-LocalRelease.cmd -AllowDirtyPublish -AllowSkippedPreflight`
- `cmd.exe /c tools\Test-LocalRelease.cmd` failed as expected against the focused dirty/skipped-preflight package because normal verification requires clean publish metadata and preflight.
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

No ADR added. Release-local launch scripts are packaging ergonomics and do not change architecture, persistence, cleanup execution, restore scope, or security boundaries.

## Follow-up Work

- After this packet is committed, cut a clean package with full preflight and verify it with `tools\Test-LocalRelease.cmd -RequireCurrentCommit`.
- Consider desktop shortcut automation only as a later explicit packaging decision.

## Risks And Assumptions

- Release-local `.cmd` scripts are enough to reduce launch friction without creating installer-like behavior.
- The fixture launch script intentionally points to the repo-local smoke fixture path and does not create the fixture or click Scan.
