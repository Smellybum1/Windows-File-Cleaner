# Progress Log

Last updated: 2026-06-04

This is the compact current progress log. Historical packet evidence lives in:

- `.codex/archive/progress-2026-05-2026-06.md`
- `.codex/archive/progress-2026-06-04-pre-context-compaction.md`

Read archived evidence only when the current task needs old packet detail.

## Current Status

Read first: `docs/codex/current-state.md`.

Latest docs/workflow packet: `2026-06-04-startup-context-compaction`. It moved oversized read-first docs into reference/archive files and replaced them with compact active docs to reduce Codex thread lag. No app behavior changed.

Latest tooling/evidence packet: `2026-06-04-local-release-acceptance-summary-regression-check`. The package acceptance summary helper now has a targeted terminal-only regression command that uses temporary ignored `.local` notes and cleans up after itself.

Latest package candidate: `.local\releases\windows-file-cleaner-v20260604-121922` at app commit `e6ac3eb`, verified but not human-accepted. Current pending acceptance notes: `.local\release-acceptance\release-acceptance-20260604-134337.md`.

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

### 2026-06-04: Local Release Acceptance Summary Regression Check

Status: completed

Goal:

- Add a targeted regression check for package acceptance summary output, especially the guarded next-step block for incomplete notes.

Safety profile:

- `terminal-readonly`. The check writes temporary ignored notes under `.local\release-acceptance-summary-test`, runs read-only summaries, and removes its test files. No WPF launch, scan, movement, restore, deletion of real-profile files, approval, installed shortcut, installer behavior, package promotion, or cleanup history.

Changes:

- Added `tools\Test-LocalReleaseAcceptanceSummary.cmd` and `.ps1`.
- The test synthesizes incomplete and complete portable release acceptance notes under ignored `.local`.
- It asserts incomplete notes print `Pending acceptance next steps`, `-RecordCommitMismatch`, and the no-launch/no-scan/no-cleanup-history boundary.
- It asserts completed notes pass `-RequireComplete` without printing pending next steps.

Verification:

- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- Confirmed `.local\release-acceptance-summary-test` was absent after the test cleaned up.
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this adds terminal-only regression coverage for existing portable-package acceptance tooling and does not change product, cleanup, restore, persistence, or deployment behavior.

### 2026-06-04: Package Acceptance Summary Next Steps

Status: completed

Goal:

- Make incomplete package acceptance summaries print the exact next commands while preserving manual acceptance and package-promotion boundaries.

Safety profile:

- `terminal-readonly`. This packet changes terminal output and docs only. No WPF launch, scan, movement, restore, deletion, approval, installed shortcut, installer behavior, package promotion, or cleanup history.

Changes:

- `Summarize-LocalReleaseAcceptanceNotes.cmd` now prints `Pending acceptance next steps` when the selected notes are incomplete.
- The next-step block prints the guarded recorder command only after verifier evidence is recorded, and uses `-RecordCommitMismatch` when commit evidence is not recorded.
- The block repeats summary and completion-check commands and says the human package acceptance pass must happen before recording manual acceptance.
- Completed accepted notes remain unchanged: no pending next-step block is printed.

Verification:

- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md"`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- Expected incomplete candidate check: `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md" -RequireComplete` exited `1`.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this is terminal guidance only and preserves ADR 0020 portable-package boundaries.

### 2026-06-04: Daily Readiness Latest Package Notes

Status: completed

Goal:

- Make pending package acceptance state visible from the daily terminal readiness command without promoting the pending candidate or launching WPF.

Safety profile:

- `terminal-readonly`. This packet changes terminal output and docs only. No WPF launch, scan, movement, restore, deletion, approval, installed shortcut, installer behavior, or cleanup history.

Changes:

- `Invoke-DailyLocalReadiness.cmd` now prints an informational `Latest package acceptance notes` block after the completed accepted package evidence block.
- The accepted package evidence block still uses `-RequireComplete`, so incomplete candidate notes do not replace the accepted `bc9b869` baseline.
- The latest-notes block is non-fatal; if a future ignored notes file is malformed, accepted package readiness still relies on completed accepted notes only.
- Daily-use docs and read-first handoff docs now call out that incomplete candidate notes are informational.

Verification:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `git diff --check`

ADRs:

- Skipped; this preserves ADR 0020 by keeping accepted package commands as the daily path and not creating shortcuts, installers, or package promotion behavior.

### 2026-06-04: Pending Package Acceptance Notes Refresh

Status: completed

Goal:

- Refresh the pending candidate acceptance notes after docs-only `HEAD` advanced beyond the packaged app commit, without launching WPF or changing app behavior.

Safety profile:

- `terminal-readonly`. `Start-LocalRelease.cmd -ChecklistOnly -WriteAcceptanceNotes` wrote ignored `.local\release-acceptance` notes only after read-only package verification. No WPF launch, scan, movement, restore, deletion, approval, installed shortcut, installer behavior, or cleanup history.

Changes:

- Generated refreshed pending notes at `.local\release-acceptance\release-acceptance-20260604-134337.md`.
- Refreshed notes stamp exact `-ReleasePath` verifier, checklist, and fixture checklist commands for `.local\releases\windows-file-cleaner-v20260604-121922`.
- Refreshed notes keep verifier evidence recorded and leave commit evidence unrecorded until the human explicitly accepts the expected `e6ac3eb` package versus `bb82b29` docs-only `HEAD` mismatch with `-RecordCommitMismatch`.
- Kept the accepted package baseline on `.local\releases\windows-file-cleaner-v20260602-011556` at `bc9b869`.

Verification:

- `cmd.exe /c tools\Start-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md"`
- Expected incomplete candidate check: `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md" -RequireComplete` exited `1` with the expected missing commit, human launch, overall result, and checklist evidence.
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` still selected the completed accepted `bc9b869` notes.
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly` still printed the accepted `bc9b869` launch command without launching WPF.
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this refreshes ignored acceptance evidence and docs only under ADR 0020 portable-package boundaries.

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
- `2026-06-04-local-release-acceptance-summary-regression-check`: added targeted temporary-note regression coverage for summary next-step behavior.
- `2026-06-04-package-acceptance-summary-next-steps`: incomplete package acceptance summaries print guarded recorder and recheck commands.
- `2026-06-04-daily-readiness-latest-package-notes`: daily readiness surfaces the latest package acceptance notes as informational context.
- `2026-06-04-pending-package-acceptance-notes-refresh`: refreshed pending candidate notes at `.local\release-acceptance\release-acceptance-20260604-134337.md`.
- `2026-06-04-local-release-recorder-commit-evidence-guard`: recorder blocks missing verifier evidence and requires explicit commit-mismatch recording.
- `2026-06-04-accepted-package-complete-notes-selection`: accepted-package helpers select latest complete notes by default.
- `2026-06-04-verified-portable-package-candidate`: candidate package verified but pending human acceptance.
- `2026-06-04-real-profile-quarantine-inline-status-wording`: WPF wording fix completed.
- `2026-06-04-second-real-profile-quarantine-batch`: second tiny exact-profile WPF Quarantine batch completed by user click.

## Archived Evidence

- `.codex/archive/progress-2026-05-2026-06.md`: historical progress log and completed packet evidence through `3dad056`.
- `.codex/archive/progress-2026-06-04-pre-context-compaction.md`: completed packet evidence through release acceptance command stamping.
- `docs/codex/archive/thread-handoff-2026-06-02.md`: previous long-form handoff and startup prompt.
