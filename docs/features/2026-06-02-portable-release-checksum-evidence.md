# Feature: Portable Release Checksum Evidence

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Add local checksum evidence to each Portable Release Package so the packaged executable and zip can be verified from ignored release artifacts without launching WPF or scanning files.

## Non-goals

- Do not create an installer, desktop shortcut, Start Menu entry, service, scheduled task, or background automation.
- Do not launch WPF from the publisher or verifier.
- Do not scan fixture or real-profile files from the publisher or verifier.
- Do not move, restore, delete, approve cleanup, or create cleanup history.
- Do not change Quarantine, selected restore, permanent deletion, broad restore, or history availability.
- Do not introduce signing, certificate management, or external distribution.

## Context

The portable release verifier already checks structure, metadata, launch scripts, README, zip contents, and commit evidence. A small integrity packet can make the local release artifact more trustworthy by recording and recomputing hashes while keeping artifacts ignored and local-only.

## Implementation

- `tools\Publish-LocalRelease.ps1` records the packaged `WindowsFileCleaner.App.exe` SHA-256 in `release-metadata.txt`.
- `tools\Publish-LocalRelease.ps1` writes a sibling `.zip.sha256` sidecar for the release zip after the zip is created.
- `tools\Test-LocalRelease.ps1` verifies the zip checksum sidecar exists.
- `tools\Test-LocalRelease.ps1` recomputes the packaged executable SHA-256 and compares it with metadata.
- `tools\Test-LocalRelease.ps1` recomputes the release zip SHA-256 and compares it with the sidecar.

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
- `docs/features/2026-06-02-portable-release-checksum-evidence.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is local release integrity evidence for ignored Portable Release Package artifacts, with no installer, signing infrastructure, persistence, cleanup execution, restore-scope, data-model, or security-boundary change.

## Follow-up Work

- After commit, cut a fresh ignored portable package so the latest strict package includes checksum evidence and matches current `HEAD`.
- Treat code signing or external distribution as a separate future decision if the package ever leaves this local machine.

## Risks And Assumptions

- SHA-256 evidence is useful for local package sanity even without code signing.
- A sidecar avoids circular metadata inside the zip while still making the zip hash easy to inspect.
