# Feature: Portable Release Verifier

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Add a repeatable local verifier for the latest Portable Release Package so package health can be checked without launching WPF, scanning, moving, restoring, deleting, or creating cleanup history.

## Non-goals

- Do not create an installer or desktop shortcut.
- Do not launch the packaged app.
- Do not scan fixture or real-profile files.
- Do not move, restore, delete, or clean up files.
- Do not enable permanent deletion, persisted cleanup history, broad/all-manifest restore, custom real-profile Quarantine, or non-exact real-profile movement.

## Current Behavior

`tools\Publish-LocalRelease.cmd` creates a self-contained portable folder and zip under ignored `.local\releases`, and the user manually verified the packaged fixture scan. There was no quick command to re-check the latest local package structure, metadata, zip contents, and safety-boundary metadata after later docs-only commits.

## Implementation

- Added `tools\Test-LocalRelease.ps1` and `.cmd`.
- The verifier finds the latest `.local\releases\windows-file-cleaner-vYYYYMMDD-HHMMSS` folder by default, or checks an explicit `-ReleasePath`.
- It verifies the release folder, stamp-format name, published executable, release metadata, sibling zip, metadata branch/runtime/self-contained/preflight/worktree fields, executable and zip metadata paths, safety-boundary lines, and required zip entries.
- It warns by default when the package commit differs from current `HEAD`, because a previously verified package can be behind docs-only commits.
- It fails the commit mismatch when `-RequireCurrentCommit` is supplied.
- It reads ignored local release files only and prints that it does not launch WPF, scan, move, restore, delete, or create cleanup history.
- Later packet `Portable Release Launch Scripts` extended the verifier to check release-local launch scripts, their metadata paths, fixture-scope launch wording, and zip entries.

## Verification

- `cmd.exe /c tools\Test-LocalRelease.cmd`
- `cmd.exe /c tools\Test-LocalRelease.cmd -RequireCurrentCommit` failed as expected because the latest local package was created at commit `5c2142d` and current `HEAD` was the later docs-only verification commit `f3a0acb`.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is local verification tooling for an existing Portable Release Package, with no architecture, persistence, cleanup execution, restore scope, or security-boundary change.

## Follow-up Work

- Use `tools\Test-LocalRelease.cmd -RequireCurrentCommit` after cutting a new package when the package must exactly match current `HEAD`.
- Consider ignored per-release launch scripts later only if copied terminal launch commands remain awkward.

## Risks And Assumptions

- A metadata/zip verifier is useful even though it does not replace a human fixture UI launch pass.
- Commit mismatch should be a warning by default because docs-only verification commits can happen after a package is already manually checked.
