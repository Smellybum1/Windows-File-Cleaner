# Feature: Accepted Local Release Launcher

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make daily use of the latest accepted Portable Release Package straightforward after docs-only commits move `main` ahead of the accepted app package.

## Non-goals

- Do not publish a new package.
- Do not launch WPF during verification or print-only checks.
- Do not click `Scan`, scan, move, restore, delete, approve cleanup, or create cleanup history.
- Do not create an installer, installed shortcut, service, scheduled task, or background automation.
- Do not treat accepted notes as current-HEAD package proof.

## Context

After the Portable v1 Acceptance Baseline packet, the accepted package `.local\releases\windows-file-cleaner-v20260602-011556` is intentionally one docs-only commit behind `main`. `Start-LocalRelease.cmd -RequireCurrentCommit` is still useful when a package must match current `HEAD`, but daily use should be able to choose the latest package with completed local acceptance notes.

## Implementation

- Added `tools\Start-AcceptedLocalRelease.ps1` and `.cmd`.
- The launcher defaults to the latest ignored `.local\release-acceptance\release-acceptance-*.md` file or accepts `-AcceptanceNotesPath`.
- It requires completed acceptance evidence, overall result `Pass` or `Pass with issues noted`, and all portable release checklist items marked `Pass`.
- It reads the accepted release folder from the notes, requires it to stay under ignored `.local`, then delegates to `Start-LocalRelease.ps1`.
- It supports `-PrintOnly`, `-Fixture`, and `-ChecklistOnly` so accepted-package command review remains possible without launching WPF.

## Verification

- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -ChecklistOnly`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-accepted-local-release-launcher.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is local launch ergonomics for an already accepted Portable Release Package and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Follow-up Work

- Use `Start-AcceptedLocalRelease.cmd -PrintOnly` when the user wants the accepted package command.
- Cut and accept a fresh package after future code changes that should ship as the next accepted app package.
- Consider installed shortcut or installer automation only as a separate future packet.

## Risks And Assumptions

- The latest completed ignored acceptance notes are the right default for daily local package use.
- A verifier warning about package commit differing from newer docs-only `HEAD` is acceptable when using the accepted-package launcher.
