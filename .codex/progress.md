# Progress Log

Last updated: 2026-06-04

This is the compact current progress log. Historical packet evidence lives in:

- `.codex/archive/progress-2026-05-2026-06.md`
- `.codex/archive/progress-2026-06-04-pre-context-compaction.md`

Read archived evidence only when the current task needs old packet detail.

## Current Status

Read first: `docs/codex/current-state.md`.

Latest docs/workflow packet: `2026-06-04-startup-context-compaction`. It moved oversized read-first docs into reference/archive files and replaced them with compact active docs to reduce Codex thread lag. No app behavior changed.

Latest tooling/evidence packet before compaction: `2026-06-04-local-release-acceptance-command-stamping`. Generated package acceptance notes stamp actual `-ReleasePath` commands and only include `-RequireCurrentCommit` when that switch created the notes.

Latest package candidate: `.local\releases\windows-file-cleaner-v20260604-121922` at app commit `e6ac3eb`, verified but not human-accepted.

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

## Recent Packet Summaries

### 2026-06-04: Startup Context Compaction

Status: completed

Goal:

- Reduce thread lag by shrinking read-first documentation while preserving detailed reference material.

Safety profile:

- `docs-only`. No WPF launch, scan, movement, restore, deletion, approval, installed shortcut, installer behavior, or cleanup history.

Changes:

- Preserved the full prior domain context as `docs/domain/context-reference.md`.
- Preserved the full prior glossary as `docs/domain/glossary-reference.md`.
- Preserved the full prior README as `docs/operations/readme-full-reference.md`.
- Preserved the full prior roadmap as `docs/features/archive/2026-06-01-live-product-readiness-roadmap-history.md`.
- Preserved the pre-compaction progress log as `.codex/archive/progress-2026-06-04-pre-context-compaction.md`.
- Replaced read-first docs with compact active summaries.
- Tightened startup instructions so domain/reference docs are opened only when relevant.

Verification:

- Read-first size scan before compaction: 11 files, 337,831 chars, about 84,458 approximate tokens.
- Read-first size scan after compaction: 11 files, 55,911 chars, about 13,978 approximate tokens.
- Active feature brief scan before compaction: 17 listed files, 88,034 chars, about 22,008 approximate tokens.
- Active feature brief scan after compaction: 5 active feature files, 15,275 chars, about 3,819 approximate tokens.
- Stale read-first instruction search found no active instruction still requiring bulk reads of both domain reference docs; the only stale bulk-read prompt match was in archived handoff evidence.
- Active long-line scan found no lines over 900 characters in the reviewed startup docs.
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this reorganizes documentation surfaces and does not introduce product, persistence, security, deployment, data-model, or core UX behavior.

### Current Live Product And Package Packets

- `2026-06-04-local-release-acceptance-command-stamping`: completed and pushed at `5a4115e`.
- `2026-06-04-local-release-recorder-commit-evidence-guard`: recorder blocks missing verifier evidence and requires explicit commit-mismatch recording.
- `2026-06-04-accepted-package-complete-notes-selection`: accepted-package helpers select latest complete notes by default.
- `2026-06-04-verified-portable-package-candidate`: candidate package verified but pending human acceptance.
- `2026-06-04-real-profile-quarantine-inline-status-wording`: WPF wording fix completed.
- `2026-06-04-second-real-profile-quarantine-batch`: second tiny exact-profile WPF Quarantine batch completed by user click.

## Archived Evidence

- `.codex/archive/progress-2026-05-2026-06.md`: historical progress log and completed packet evidence through `3dad056`.
- `.codex/archive/progress-2026-06-04-pre-context-compaction.md`: completed packet evidence through release acceptance command stamping.
- `docs/codex/archive/thread-handoff-2026-06-02.md`: previous long-form handoff and startup prompt.
