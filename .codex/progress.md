# Progress Log

Last updated: 2026-06-03

This file is now the compact current progress log. Historical packet evidence from May and early June 2026 lives in `.codex/archive/progress-2026-05-2026-06.md`.

## Current Status

Read first: `docs/codex/current-state.md`.

The latest product/evidence packet before this docs cleanup is `3dad056 Record current-head next-batch review evidence`. The app remains a local Windows File Cleaner for `C:\Users\moxhe`; Storage Scan is read-only, exact real-profile movement is human-clicked only, and unavailable workflows remain broad/all-manifest real-profile Undo Quarantine, custom/non-exact real-profile Quarantine, custom selected restore, permanent deletion, persisted cleanup history, installed shortcut automation, and installer behavior.

Fresh current-head next-batch evidence passed on `c7cb545` with `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd`. That wrapper ran full MVP preflight, accepted package verification, optional Fixture Acceptance Notes status, exact-profile Restore Manifest display, recovery-review focus, undo-work focus, and manual WPF checklist printing without WPF launch, real-profile scan, movement, restore, deletion, approval, manifest writes, shortcut creation, installer behavior, or cleanup history.

## Next Recommended Work

1. Ask the user to do the manual WPF next-batch review if they are ready.
2. Do not click real-profile Quarantine from Codex.
3. Do not guide a Quarantine click unless the user explicitly chooses a specific tiny exact `C:\Users\moxhe` batch after WPF readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, and immediate Pre-Execution Revalidation are visible.
4. Use `docs/operations/daily-use.md`, `docs/operations/portable-release.md`, `docs/operations/manual-fixture-review.md`, and `docs/operations/restore-manifest-review.md` for command detail.
5. Start an ADR 0020 shortcut/installer follow-up only if the user explicitly asks for installed shortcut or installer automation.

## Recent Completed Packets

### 2026-06-03: Workflow And Markdown Bloat Reduction

Status: completed

Goal:

- Reduce Codex startup/context load by archiving long evidence docs, adding compact current-state/runbook files, and replacing repeated safety boilerplate with reusable safety profiles.

Safety profile:

- `docs-only`

Docs updated:

- Archived the historical progress log at `.codex/archive/progress-2026-05-2026-06.md`.
- Archived the previous long handoff at `docs/codex/archive/thread-handoff-2026-06-02.md`.
- Added compact startup docs: `docs/codex/current-state.md`, `docs/codex/safety-profiles.md`, and a replacement `docs/codex/thread-handoff.md`.
- Added operational runbooks under `docs/operations/`.
- Added `docs/features/index.md`, `docs/features/archive/README.md`, and `docs/features/2026-06-03-workflow-markdown-bloat-reduction.md`.
- Trimmed duplicated command/checklist detail from `README.md`, `docs/domain/context.md`, and `docs/domain/glossary.md`.

Verification:

- `git diff --check` passed with expected CRLF warnings.
- Active doc size scan showed `.codex/progress.md` at 56 lines, `docs/codex/thread-handoff.md` at 87 lines, and no active long lines over 900 characters in `README.md`, `docs/domain/glossary.md`, or `docs/domain/context.md`.
- Stale startup/checklist phrase search found only archived handoff matches.
- No app tests, preflight, WPF launch, scan, movement, restore, deletion, or cleanup history commands were run because this packet is docs-only.

ADRs:

- Skipped; this was a documentation/workflow organization packet, not a new durable product, persistence, security, deployment, data-model, or core UX decision.

Follow-up:

- Consider a later link-preserving archive/index pass for older feature briefs if the top-level `docs/features/` folder remains noisy.
- Leave historical safety boilerplate in older packet notes unless a related task touches those files.

### 2026-06-02: Current-Head Next-Batch Review Evidence

Status: completed

Evidence:

- `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd` passed on current `main` at `c7cb545`.
- Full MVP preflight passed: restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and whitespace diff checking.
- Accepted package verification passed with the expected package/current-HEAD warning (`bc9b869` package versus `c7cb545` current `HEAD`).
- Exact-profile Restore Manifest display showed 4 of 10 manifests, displayed undo work `0`, and displayed recovery review `2`.
- Focused recovery-review evidence showed the two older failed NVIDIA `DXCache` attempts, focused undo-work evidence showed zero exact-profile matches, and the manual WPF checklist printed.

Safety profile:

- `terminal-readonly`

## Archived Evidence

- `.codex/archive/progress-2026-05-2026-06.md`: historical progress log and completed packet evidence through `3dad056`.
- `docs/codex/archive/thread-handoff-2026-06-02.md`: previous long-form handoff and startup prompt.
