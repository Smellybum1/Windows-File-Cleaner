# Feature: Local Release Launcher

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make the latest Portable Release Package easy to start from the repo root while preserving a print-only verification path for Codex and terminal checks.

## Non-goals

- Do not create an installer, desktop shortcut, Start Menu entry, service, scheduled task, or background automation.
- Do not launch WPF during automated verification.
- Do not create fixtures, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, or create cleanup history.
- Do not change Quarantine, selected restore, permanent deletion, broad restore, or history availability.

## Context

Portable v1 packages already include release-local launch scripts, but using the latest package still requires browsing into `.local\releases` or copying a printed path. A repo-level launcher can improve daily use while keeping release artifacts ignored and preserving verification before launch.

## Implementation

- Added `tools\Start-LocalRelease.ps1` and `.cmd`.
- The launcher finds the latest ignored release folder or accepts `-ReleasePath`.
- It verifies the package through `Test-LocalRelease.ps1` by default.
- `-RequireCurrentCommit`, `-AllowDirtyPublish`, and `-AllowSkippedPreflight` pass through to verification.
- `-PrintOnly` prints the exact launch command without launching WPF.
- `-Fixture` adds the repo-local smoke fixture Cleanup Scope and states that fixture launch only prefills the Cleanup Scope.
- `-SkipVerify` exists for intentional focused local use but is not the default.
- Later packet `Local Release Launcher Start-Here Output` made the launcher output also print the package-local `README-FIRST.txt` path and the matching release-local launch script path.
- Later packet `Local Release Acceptance Checklist` added `-ChecklistOnly`, which verifies the package by default and prints package-level normal/fixture acceptance steps without launching WPF.

## Verification

- `cmd.exe /c tools\Start-LocalRelease.cmd -PrintOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-LocalRelease.cmd -PrintOnly -Fixture -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check` passed with existing LF-to-CRLF working-copy warnings.

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is portable release ergonomics, not a packaging architecture change or cleanup/restore behavior change.

## Follow-up Work

- After launcher docs/scripts change, cut a fresh ignored portable package so the latest package commit again matches current `HEAD`.
- Keep installed shortcut or installer automation as a separate future packaging decision.

## Risks And Assumptions

- The repo-level launcher is useful enough even though each package already contains release-local launch scripts.
- Verification before launch is the right default for local daily use.
