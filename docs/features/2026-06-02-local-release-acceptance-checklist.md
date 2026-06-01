# Feature: Local Release Acceptance Checklist

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make portable package acceptance steps easy to print from the repo root without launching WPF.

## Non-goals

- Do not create an installer, desktop shortcut, Start Menu entry, service, scheduled task, or background automation.
- Do not launch WPF during checklist-only review.
- Do not create fixtures, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, or create cleanup history.
- Do not change Quarantine, selected restore, permanent deletion, broad restore, or history availability.

## Context

Portable packages can already be published, verified, and launched through repo-level tooling. A package-level acceptance checklist gives the user and Codex a repeatable final review path for the latest strict package without opening the app or touching files.

## Implementation

- `tools\Start-LocalRelease.ps1` now supports `-ChecklistOnly`.
- Checklist-only mode still verifies the package by default through the existing local release verifier.
- The checklist prints the package-local `README-FIRST.txt`, normal launch script, fixture launch script, executable path, fixture scope, and release folder.
- The checklist names normal launch review, fixture launch review, portable/no-installer boundaries, and the stop-before-real-profile-movement boundary.
- Checklist-only mode exits before WPF launch and explicitly states that it did not launch WPF, click `Scan`, move, restore, delete, approve cleanup, or create cleanup history.

## Verification

- `cmd.exe /c tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-LocalRelease.cmd -Fixture -ChecklistOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-local-release-launcher.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is portable release acceptance ergonomics, not a packaging architecture change or cleanup/restore behavior change.

## Follow-up Work

- After committing this packet, cut a fresh ignored portable package so the latest package commit again matches current `HEAD`.
- Keep installed shortcut or installer automation as a separate future packaging decision.

## Risks And Assumptions

- A terminal acceptance checklist is enough for v1 package review alongside the package-local `README-FIRST.txt`.
- The latest ignored package exists before checklist-only mode is run.
