# Feature: Local Release Launcher Start-Here Output

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make `tools\Start-LocalRelease.cmd` surface the package-local `README-FIRST.txt` and release-local launch script paths when it prints or starts a verified Portable Release Package.

## Non-goals

- Do not create an installer, desktop shortcut, Start Menu entry, service, scheduled task, or background automation.
- Do not launch WPF during automated verification.
- Do not create fixtures, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, or create cleanup history.
- Do not change Quarantine, selected restore, permanent deletion, broad restore, or history availability.

## Context

Portable packages now include `README-FIRST.txt` and release-local normal/fixture launch scripts. The repo-level launcher is the easiest way to find the latest package, so its output should point to those package-local artifacts instead of only printing the executable command.

## Implementation

- `tools\Start-LocalRelease.ps1` computes the latest package's `README-FIRST.txt`, `Launch-WindowsFileCleaner.cmd`, and `Launch-WindowsFileCleaner-Fixture.cmd` paths.
- Normal print/start output shows the start-here README path and normal release-local launch script path.
- Fixture print/start output shows the start-here README path and fixture release-local launch script path.
- The existing verifier still runs by default before output or launch.

## Verification

- `cmd.exe /c tools\Start-LocalRelease.cmd -PrintOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-LocalRelease.cmd -Fixture -PrintOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-local-release-launcher.md`
- `docs/features/2026-06-02-portable-release-start-here-readme.md`
- `docs/features/2026-06-02-local-release-launcher-start-here-output.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is portable release ergonomics and terminal output wording, with no installer, persistence, cleanup execution, restore scope, data-model, or security-boundary change.

## Follow-up Work

- After commit, cut a fresh ignored portable package so the latest strict package commit again matches current `HEAD`.
- Keep installed shortcut or installer automation as a later explicit packaging decision.

## Risks And Assumptions

- Printing package-local artifact paths makes the portable folder easier to use without making the package feel installed.
