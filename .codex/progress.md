# Progress Log

Last updated: 2026-06-04

This file is now the compact current progress log. Historical packet evidence from May and early June 2026 lives in `.codex/archive/progress-2026-05-2026-06.md`.

## Current Status

Read first: `docs/codex/current-state.md`.

The latest tooling/evidence packet is `2026-06-04-local-release-recorder-commit-evidence-guard`: package acceptance recording now refuses missing verifier evidence and requires explicit `-RecordCommitMismatch` before package/current-HEAD mismatch evidence is marked recorded.

The latest package candidate remains `2026-06-04-verified-portable-package-candidate`: package `.local\releases\windows-file-cleaner-v20260604-121922` was cut from app commit `e6ac3eb` after the real-profile inline status wording fix, verified with current-commit package checks, and left pending human package acceptance.

The latest live-product evidence remains `2026-06-04-second-real-profile-quarantine-batch`: the user approved and clicked one exact `C:\Users\moxhe` WPF Quarantine batch for a single `pip\cache\http-v2` `.body` file. The app remains a local Windows File Cleaner for `C:\Users\moxhe`; Storage Scan is read-only, exact real-profile movement is human-clicked only, and unavailable workflows remain broad/all-manifest real-profile Undo Quarantine, custom/non-exact real-profile Quarantine, custom selected restore, permanent deletion, persisted cleanup history, installed shortcut automation, and installer behavior.

The latest docs/workflow baseline before these packets is `1ea1b76 Reduce workflow markdown bloat`. It archived long historical evidence, added compact current-state/safety/runbook docs, and reduced active startup-doc bloat without changing app behavior.

Fresh current-head next-batch evidence passed on `655e075` with `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd` before the user-clicked WPF batch. That wrapper ran full MVP preflight, accepted package verification, optional Fixture Acceptance Notes status, exact-profile Restore Manifest display, recovery-review focus, undo-work focus, and manual WPF checklist printing without WPF launch, real-profile scan, movement, restore, deletion, approval, manifest writes, shortcut creation, installer behavior, or cleanup history.

The second tiny exact real-profile WPF batch moved one `pip\cache\http-v2` `.body` file, `28.93 MB`, with `moved 1`, `failed 0`, and `Recovery review: no`. Read-only terminal summary afterward showed manifest `quarantine-action-draft-20260604014901-b7b402a2`, exact-profile display 5 of 11 manifests, displayed undo work `1`, and displayed recovery review `2`.

Latest working app packet fixed the WPF inline post-execution status wording so exact real-profile Quarantine execution is no longer summarized as fixture Quarantine execution.

Current package candidate `.local\releases\windows-file-cleaner-v20260604-121922` is verified but not accepted. Ignored acceptance notes `.local\release-acceptance\release-acceptance-20260604-122009.md` record verifier and commit evidence, but normal launch, fixture launch, overall result, and the remaining checklist items are not recorded. Accepted package baseline remains `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`.

Accepted-package tooling now selects the latest complete acceptance notes by default. This keeps daily readiness and accepted-package launch commands on the completed `bc9b869` package while the newer `e6ac3eb` candidate notes remain incomplete and inspectable by explicit path.

Package acceptance recording now requires verifier evidence and explicit package/current-HEAD mismatch evidence. This keeps future docs-drifted candidate acceptance notes from looking recorded while still failing completion checks.

## Next Recommended Work

1. Stop after the second tiny exact real-profile batch; do not chain another real-profile Quarantine batch.
2. Do not click real-profile Quarantine from Codex.
3. Do not run or treat another next-batch review as movement evidence while exact-profile displayed undo work is present unless a new Grill with Docs pass decides that outstanding selected-manifest undo work is acceptable.
4. If packaging is the next focus, complete human package acceptance for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it over the accepted `bc9b869` baseline.
5. Use `docs/operations/daily-use.md`, `docs/operations/portable-release.md`, `docs/operations/manual-fixture-review.md`, and `docs/operations/restore-manifest-review.md` for command detail.
6. Start an ADR 0020 shortcut/installer follow-up only if the user explicitly asks for installed shortcut or installer automation.

## Recent Completed Packets

### 2026-06-04: Local Release Recorder Commit Evidence Guard

Status: completed

Goal:

- Make portable release acceptance recording safer when a package candidate is intentionally behind current `HEAD` because later commits are docs/tooling-only.

Safety profile:

- `terminal-readonly`; recorder verification used ignored test-note copies only. No WPF launch, scan, movement, restore, deletion, approval, installed shortcut, installer behavior, or cleanup history.

Changes:

- `Record-LocalReleaseAcceptanceNotes.ps1` now refuses to record manual package acceptance unless verifier evidence is already recorded.
- If commit evidence is not already recorded, the recorder now requires explicit `-RecordCommitMismatch` before it marks commit mismatch evidence recorded.
- `Start-LocalRelease.ps1` now prints and embeds the recorder command in generated notes, plus the explicit `-RecordCommitMismatch` variant when notes are created without `-RequireCurrentCommit`.

Verification:

- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-missing-verifier.md" -RecordManualAcceptance` exited `1` with the expected missing-verifier blocker.
- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-missing-commit.md" -RecordManualAcceptance` exited `1` with the expected missing-commit blocker.
- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-missing-commit.md" -RecordManualAcceptance -RecordCommitMismatch -Summary "Test-only recorded portable release acceptance with explicit commit mismatch evidence."`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-missing-commit.md" -RequireComplete`
- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-missing-commit.md" -RecordManualAcceptance -RecordCommitMismatch -WhatIf`
- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd` exited `1` before writing because `-RecordManualAcceptance` was missing.
- `cmd.exe /c tools\Start-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -ChecklistOnly`
- `cmd.exe /c tools\Start-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -ChecklistOnly -WriteAcceptanceNotes`; generated ignored test notes included `-RecordCommitMismatch` guidance and were removed after confirming the generated path stayed under `.local\release-acceptance`.

Docs updated:

- `docs/features/2026-06-04-local-release-recorder-commit-evidence-guard.md`
- `docs/features/index.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/current-state.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/operations/portable-release.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`

ADRs:

- Skipped; this is a release-acceptance tooling guard under ADR 0020 and does not add installed shortcut or installer behavior.

Follow-up:

- Complete the human package acceptance pass for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it over the accepted `bc9b869` baseline.

### 2026-06-04: Accepted Package Complete Notes Selection

Status: completed

Goal:

- Keep accepted-package daily commands stable after a newer package candidate writes incomplete ignored acceptance notes.

Safety profile:

- `terminal-readonly`; no WPF launch, scan, movement, restore, deletion, approval, installed shortcut, installer behavior, or cleanup history.

Changes:

- `Summarize-LocalReleaseAcceptanceNotes.ps1 -RequireComplete` now selects the latest complete acceptance notes by default when no explicit `-Path` is supplied.
- `Start-AcceptedLocalRelease.ps1` now selects the latest complete acceptance notes by default.
- Explicit paths still inspect pending candidate notes and still fail `-RequireComplete` when those notes are incomplete.

Verification:

- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-122009.md"`
- Expected incomplete check: `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-122009.md" -RequireComplete` exited `1` with the expected missing acceptance evidence.
- Expected incomplete accepted-launcher check: `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -AcceptanceNotesPath ".local\release-acceptance\release-acceptance-20260604-122009.md" -PrintOnly` exited `1` before launch-command printing.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `cmd.exe /c tools\Test-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922"`

Docs updated:

- `docs/features/2026-06-04-accepted-package-complete-notes-selection.md`
- `docs/features/index.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/current-state.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/operations/portable-release.md`
- `docs/domain/context.md`
- `README.md`

ADRs:

- Skipped; this is a tooling correction under ADR 0020 and does not add installed shortcut or installer behavior.

Follow-up:

- Complete the human package acceptance pass for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it over the accepted `bc9b869` baseline.

### 2026-06-04: Verified Portable Package Candidate

Status: completed

Goal:

- Cut and verify a portable package candidate from the latest app-code commit without promoting it before a human package acceptance pass.

Safety profile:

- `terminal-readonly` for verification and acceptance-note summarization; package publishing wrote ignored `.local\releases` artifacts only and ran MVP preflight before publishing. No WPF launch, real-profile scan, movement, restore, deletion, approval, installed shortcut, installer behavior, or cleanup history.

Evidence:

- Package: `.local\releases\windows-file-cleaner-v20260604-121922`
- Package commit: `e6ac3eb467bffb4e41bd5697da702f649292315d`
- Executable SHA-256: `0E733191EF0B52742F35BAC5C86B34CE7075731DFB4A849EDD9E0EAA330025C1`
- Ignored acceptance notes: `.local\release-acceptance\release-acceptance-20260604-122009.md`
- Notes summary: verifier and commit evidence recorded; normal launch, fixture launch, overall result, and five checklist items not recorded.

Verification:

- `cmd.exe /c tools\Publish-LocalRelease.cmd`
- `cmd.exe /c tools\Test-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -RequireCurrentCommit`
- `cmd.exe /c tools\Start-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-122009.md"`

Docs updated:

- `docs/features/2026-06-04-verified-portable-package-candidate.md`
- `docs/features/index.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/current-state.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/operations/portable-release.md`
- `docs/operations/daily-use.md`
- `docs/operations/restore-manifest-review.md`
- `README.md`

ADRs:

- Skipped; this follows ADR 0020 and does not add installed shortcut or installer behavior.

Follow-up:

- Complete the human package acceptance pass and record notes before promoting this package over the accepted `bc9b869` baseline.

### 2026-06-04: Real-Profile Quarantine Inline Status Wording

Status: completed

Goal:

- Fix the Quarantine tab inline post-execution status so exact real-profile Quarantine results are not described as fixture Quarantine results.

Safety profile:

- Code/test wording packet only; no WPF launch, scan, movement, restore, deletion, approval, manifest writes, shortcut creation, install, or cleanup history.

Changes:

- `UpdateQuarantinePreviewStatus` now reuses the existing real-profile-aware Quarantine execution status formatter.
- Added a WPF app regression test that injects a synthetic exact-profile `QuarantineExecutionResult` into in-memory window state and verifies real-profile wording plus the absence of fixture Undo wording.
- Updated the second-batch feature note to mark the wording follow-up fixed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

Docs updated:

- `docs/features/2026-06-04-real-profile-quarantine-inline-status-wording.md`
- `docs/features/2026-06-04-second-real-profile-quarantine-batch.md`
- `docs/features/index.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`

ADRs:

- Skipped; this is UI wording under existing ADR 0017, ADR 0018, and ADR 0019 gates.

### 2026-06-04: Second Real-Profile Quarantine Batch

Status: completed

Goal:

- Record the user-clicked second tiny exact `C:\Users\moxhe` Quarantine batch and its read-only post-action evidence.

Safety profile:

- `real-profile-user-click-only` for the WPF movement; `terminal-readonly` for Codex post-action evidence capture.

Evidence:

- `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd` passed on `655e075` before WPF movement.
- The user shortlisted one exact `C:\Users\moxhe` `pip\cache\http-v2` `.body` file, `28.93 MB`.
- WPF preview showed `1 included`, `0 blocked`, `0 redundant`, no readiness blockers, clean Quarantine Root Execution Safety, clean Pre-Execution Revalidation, selected real-profile restore trust, and `Can execute: yes` after exact `QUARANTINE`.
- The user clicked `Quarantine included shortlist`; WPF reported `Moved 1`, `failed 0`, `Recovery review: no`.
- Read-only terminal summary showed manifest `D:\WindowsFileCleanerQuarantine\actions\quarantine-action-draft-20260604014901-b7b402a2\restore-manifest.json`, status `Completed`, entry `Moved`, exact-profile displayed undo work `1`, and exact-profile displayed recovery review `2`.

Verification:

- `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -ShowEntries`
- `git status --short --branch`

Docs updated:

- `docs/features/2026-06-04-second-real-profile-quarantine-batch.md`
- `docs/features/index.md`
- `docs/codex/current-state.md`
- `.codex/progress.md`

ADRs:

- Skipped; the batch followed ADR 0017, ADR 0018, and ADR 0019 and did not introduce a new durable product, persistence, security, deployment, data-model, or core UX decision.

Follow-up:

- Keep recovery selected-manifest-only if the user wants to restore this file later.
- WPF inline status wording follow-up was fixed in the next packet.

### 2026-06-04: New-Thread Closeout

Status: completed

Goal:

- Refresh active startup/workflow docs only where the new thread would otherwise miss the `1ea1b76` docs cleanup baseline or follow stale read-first pointers.

Safety profile:

- `docs-only`

Docs updated:

- `AGENTS.md` and `docs/codex/grill-with-docs.md` now point fresh threads through `docs/features/index.md` before opening feature briefs.
- `README.md` now points current state to `docs/codex/current-state.md` and active evidence to `docs/features/index.md` instead of naming the older readiness audit as the current evidence source.
- `docs/codex/current-state.md`, `docs/codex/thread-handoff.md`, and `docs/features/index.md` now reflect the `1ea1b76` docs/workflow cleanup baseline.

Verification:

- `git diff --check` passed with expected CRLF warnings.
- Active stale-marker search found no matches for old startup/checklist/current-evidence phrases outside archived or historical feature files.
- Active startup/workflow doc size scan found no active lines over 900 characters in the reviewed files.
- Reviewed tracked workflow config files `global.json` and `NuGet.Config`; no changes needed.
- No app tests, preflight, WPF launch, scan, movement, restore, deletion, or cleanup history commands were run because this packet is docs-only.

ADRs:

- Skipped; this is a closeout/startup-doc polish packet, not a durable product, persistence, security, deployment, data-model, or core UX decision.

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
