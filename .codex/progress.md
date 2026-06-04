# Progress Log

Last updated: 2026-06-04

This is the compact read-first progress log. Detailed packet evidence was archived to keep new threads fast.

Historical packet evidence lives in:

- `.codex/archive/progress-2026-05-2026-06.md`
- `.codex/archive/progress-2026-06-04-pre-context-compaction.md`
- `.codex/archive/progress-2026-06-04-pre-closeout.md`

Read archived evidence only when the current task needs old packet detail.

## Current Status

Read first: `docs/codex/current-state.md`.

Latest docs/workflow packet: `2026-06-04-closeout-startup-compaction`. It archived the long progress log, shortened the thread-handoff startup prompt, and refreshed representative CI evidence to MVP Preflight #391 on `9204247`.

Latest tooling/evidence packet: `2026-06-04-daily-readiness-exact-profile-stop-action`. Daily readiness prints a next-action reminder after the exact-profile undo-work spotlight so nonzero displayed exact-profile undo work visibly blocks next-batch movement evidence.

Latest package candidate: `.local\releases\windows-file-cleaner-v20260604-121922` at app commit `e6ac3eb`, verified but not human-accepted. Current pending acceptance notes: `.local\release-acceptance\release-acceptance-20260604-164509.md`.

Accepted package baseline: `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`, with completed ignored notes `.local\release-acceptance\release-acceptance-20260602-011743.md`.

Latest live-product evidence: the 2026-06-04 second tiny exact real-profile WPF Quarantine batch moved one `pip\cache\http-v2` `.body` file, `28.93 MB`, with `moved 1`, `failed 0`, `Recovery review: no`.

Post-action evidence:

- Manifest: `D:\WindowsFileCleanerQuarantine\actions\quarantine-action-draft-20260604014901-b7b402a2\restore-manifest.json`
- Exact-profile displayed undo work: `1`
- Exact-profile displayed recovery review: `2`
- User WPF selected-manifest readiness screenshot showed the selected second-batch manifest has `1` restorable entry and `0` blocked selected entries.

## Next Recommended Work

1. Stop after the second tiny exact real-profile batch; do not chain another real-profile Quarantine batch.
2. Do not click real-profile Quarantine from Codex.
3. Do not run or treat another next-batch review as movement evidence while exact-profile displayed undo work is present unless a new Grill with Docs pass decides outstanding selected-manifest undo work is acceptable.
4. If recovery is needed, use selected-manifest restore only for the exact selected `C:\Users\moxhe` Restore Manifest after readiness, exact `RESTORE`, and immediate selected-restore revalidation.
5. If packaging is the next focus, complete human acceptance for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it over the accepted `bc9b869` baseline.
6. Use `docs/operations/*.md` for command detail.

## Current Evidence

- GitHub Actions MVP Preflight #391 passed on `9204247` in `1m 41s`, validating the normal push path after the daily readiness stop-action reminder joined default preflight coverage.
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed locally before `9204247`.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd` passed locally and printed the exact-profile stop-state action reminder after the undo-work spotlight.
- `cmd.exe /c tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd` passed locally with ignored synthetic Restore Manifests.
- `cmd.exe /c tools\Test-DocumentationConsistency.cmd` passed locally after the latest packet breadcrumbs were aligned.

## Current Live Product And Package Packets

- `2026-06-04-closeout-startup-compaction`: progress is compact again, detailed packet evidence is archived, thread-handoff startup prompt is concise, and CI #391 is the current representative normal push proof after the daily readiness stop action.
- `2026-06-04-daily-readiness-exact-profile-stop-action`: daily readiness prints a next-action reminder after the exact-profile undo-work spotlight so nonzero displayed exact-profile undo work visibly blocks next-batch movement evidence.
- `2026-06-04-real-profile-selected-restore-trust-helper-preflight-regression`: MVP preflight runs the sacrificial real-profile selected restore trust helper path guard regression by default with a `-WhatIf`-only non-`moxhe` override and escaped-path case derived from the helper's safe preview.
- `2026-06-04-ci-evidence-refresh-after-exact-skip-docs-regression`: CI runbook and compact handoff docs recorded earlier #389 proof after exact skip-switch documentation coverage joined documentation consistency.
- `2026-06-04-mvp-preflight-skip-switch-exact-documentation-regression`: documentation consistency verifies the CI runbook focused skip-switch section exactly matches current `Invoke-MvpPreflight.cmd` skip switches, catching missing and stale entries.
- `2026-06-04-real-profile-next-batch-early-undo-guard`: next-batch evidence stops before MVP preflight when displayed undo work exists.
- `2026-06-04-daily-readiness-exact-profile-undo-spotlight`: default daily readiness spotlights exact-profile undo-work stop state.
- `2026-06-04-pending-package-acceptance-notes-refresh-after-tooling-hardening`: current pending candidate notes remain incomplete and human-owned.
- `2026-06-04-verified-portable-package-candidate`: candidate package is verified but pending human acceptance.
- `2026-06-04-real-profile-quarantine-inline-status-wording`: WPF wording fix completed.
- `2026-06-04-second-real-profile-quarantine-batch`: second tiny exact-profile WPF Quarantine batch completed by user click and left exact-profile displayed undo work `1`.

## Archived Evidence

- `.codex/archive/progress-2026-05-2026-06.md`: historical progress log and completed packet evidence through `3dad056`.
- `.codex/archive/progress-2026-06-04-pre-context-compaction.md`: completed packet evidence through release acceptance command stamping.
- `.codex/archive/progress-2026-06-04-pre-closeout.md`: detailed packet evidence through `9204247`.
- `docs/codex/archive/thread-handoff-2026-06-02.md`: previous long-form handoff and startup prompt.
