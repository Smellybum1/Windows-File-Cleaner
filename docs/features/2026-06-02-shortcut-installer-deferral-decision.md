# Feature: Shortcut Installer Deferral Decision

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Record the packaging decision that the accepted Portable Release Package launch commands remain the daily path and installed shortcut or installer automation is deferred to a later explicit user-approved packet.

## Non-goals

- Do not create, modify, or delete Desktop shortcuts, Start Menu shortcuts, installer files, services, scheduled tasks, or background automation.
- Do not publish or accept a new package.
- Do not launch WPF.
- Do not click `Scan`, scan, move, restore, delete, quarantine, approve cleanup, write Restore Manifests, or create cleanup history.
- Do not enable broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, persisted cleanup history, or non-exact real-profile movement.

## Context

The README and handoff already point ordinary local use at `tools\Start-AcceptedLocalRelease.cmd -PrintOnly` and `tools\Invoke-DailyLocalReadiness.cmd`. Several recent packets deferred shortcut or installer automation as a possible later packaging packet, but there was no ADR explaining why.

## Decision

ADR 0020 now records that:

- accepted package launch commands are the daily path for v1;
- debug-build shortcuts are development-only context, not accepted-package evidence;
- installed shortcuts or installers remain unavailable unless a later explicit user-approved packaging packet defines target selection, stale target handling, removal behavior, and read-only verification.

## Verification

Planned checks:

- `rg -n "ADR 0020|installed shortcut|installer|accepted package launch commands|debug-build" README.md docs .codex\progress.md`
- `rg -n "0020-defer-installed-shortcut-and-installer" docs\decisions docs\features README.md .codex\progress.md`
- `git diff --check`

## Docs

- `docs/decisions/0020-defer-installed-shortcut-and-installer.md`
- `docs/features/2026-06-02-shortcut-installer-deferral-decision.md`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-accepted-package-launch-path-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

Added ADR 0020 because the decision affects deployment and daily-use UX, has multiple plausible options, and creates a known downside by keeping daily launch command-driven for now.

## Follow-up Work

- Start a new packaging packet only if the user explicitly wants an installed shortcut or installer.
- Keep using daily readiness and accepted package print-only commands until that later packet exists.

## Risks And Assumptions

- The accepted package launch commands are good enough for daily local use while live-product safety is still maturing.
- The accepted-package/current-HEAD warning remains expected when the accepted package predates docs-only commits.
