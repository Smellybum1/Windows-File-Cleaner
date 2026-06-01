# Feature: Accepted Package Launch Path Clarity

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Clarify that daily local use should start from the accepted Portable Release Package launcher, while any local desktop shortcut pointing at the debug build is development-only context.

## Non-goals

- Do not create, update, or delete any shortcut.
- Do not create an installer, Start Menu entry, service, scheduled task, or background automation.
- Do not publish or accept a new package.
- Do not launch WPF, click `Scan`, scan, move, restore, delete, approve cleanup, or create cleanup history.
- Do not enable broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, or persisted cleanup history.

## Context

The handoff still listed a desktop shortcut target under `src\WindowsFileCleaner.App\bin\Debug\net8.0-windows`. That is useful as a development reference, but the accepted package daily-use path is now `tools\Start-AcceptedLocalRelease.cmd -PrintOnly` and `tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly`.

## Implementation

- Updated README Daily Local Use to state that debug-build shortcuts are development conveniences only.
- Updated the handoff current-state and startup prompt so new threads start from the accepted package launcher rather than a debug shortcut.
- Recorded the packet in progress docs without changing app or tool behavior.

## Verification

- `rg -n "Desktop shortcut target|debug build|Start-AcceptedLocalRelease|accepted package|accepted daily path" README.md docs\codex\thread-handoff.md .codex\progress.md docs\features\2026-06-02-accepted-package-launch-path-clarity.md docs\features\2026-06-02-accepted-package-daily-use-guide.md docs\features\2026-06-01-live-product-readiness-roadmap.md`
- `git diff --check`

## Docs

- `README.md`
- `docs/features/2026-06-02-accepted-package-daily-use-guide.md`
- `docs/features/2026-06-02-accepted-package-launch-path-clarity.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is launch-path documentation clarity for existing portable package tooling and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Follow-up Work

- Consider installed shortcut or installer automation only as a later explicit user-approved packaging packet.
- Cut and accept a fresh package only after future behavior changes should ship as the next accepted app package.

## Risks And Assumptions

- The accepted package remains the right daily-use baseline until a fresh package is intentionally cut and accepted.
- A debug-build shortcut may still exist locally, but it should not be treated as package acceptance evidence.
