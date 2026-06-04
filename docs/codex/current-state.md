# Current State

Last updated: 2026-06-04

Use this as the first compact orientation file for new Codex threads. Historical packet evidence lives in `.codex/archive/progress-2026-05-2026-06.md`; do not load that archive unless the current task needs old packet detail.

## Snapshot

- Repo: `D:\Codex\Windows File Cleaner`
- Branch: `main`
- Latest live evidence: 2026-06-04 second tiny exact real-profile Quarantine batch.
- Latest working app packet: real-profile Quarantine inline status wording fix.
- Latest package candidate: `.local\releases\windows-file-cleaner-v20260604-121922` at `e6ac3eb`, verified but not human-accepted.
- Latest tooling packet: accepted-package helpers select latest complete acceptance notes by default.
- Latest docs/workflow baseline before these packets: `1ea1b76 Reduce workflow markdown bloat`
- App stack: C# / WPF / .NET 8
- Product: local Windows cleanup reviewer for `C:\Users\moxhe`
- Storage Scan: read-only
- Accepted portable package: `.local\releases\windows-file-cleaner-v20260602-011556`
- Accepted package commit: `bc9b869`
- Completed ignored acceptance notes: `.local\release-acceptance\release-acceptance-20260602-011743.md`

## Available Workflows

- Read-only Storage Scan for `C:\Users\moxhe`.
- Fixture Quarantine execution, current-fixture undo, and fixture selected restore.
- Exact real-profile selected restore for one selected `C:\Users\moxhe` Restore Manifest after ADR 0019 gates.
- First-phase exact real-profile Quarantine for `C:\Users\moxhe` after ADR 0017/0018 readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, selected restore trust, and immediate Pre-Execution Revalidation.
- Restore Manifest discovery/review in the Quarantine tab's `Restore Manifest Review` panel.
- Portable v1 package launch through accepted package print commands.
- Terminal-only daily readiness, Restore Manifest summary, and next-batch review evidence.

## User-Verified Evidence

- Fixture Quarantine, current-fixture undo, and fixture selected restore worked.
- Exact real-profile selected restore trust test worked.
- First tiny exact real-profile Quarantine batch moved one `pip\cache\http\b\c` row with `moved 1, failed 0`.
- Selected restore recovery for that batch worked on retry with `Restored 1, failed 0`.
- Rediscovery/rescan confirmed the restored path appeared again.
- Second tiny exact real-profile Quarantine batch moved one `pip\cache\http-v2` `.body` file with `moved 1, failed 0`, `Recovery review: no`.
- Accepted portable package launched and scanned the fixture successfully.

## Current Evidence

- `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd` passed on current `main` at `655e075` before the second tiny exact real-profile WPF batch.
- Full MVP preflight passed in that wrapper: restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and whitespace check.
- Accepted package verification passed with the expected accepted-package/current-HEAD warning.
- The user-approved second WPF batch moved one exact `C:\Users\moxhe` `pip\cache\http-v2` `.body` file into Quarantine with `moved 1`, `failed 0`, `Recovery review: no`.
- Read-only terminal summary after the action showed 5 of 11 exact-profile manifests, displayed exact-profile undo work `1`, and displayed exact-profile recovery review `2`.
- New manifest: `D:\WindowsFileCleanerQuarantine\actions\quarantine-action-draft-20260604014901-b7b402a2\restore-manifest.json`.
- Latest working app packet fixed WPF inline post-execution status so exact real-profile Quarantine results are no longer described as fixture Quarantine results.
- Portable package candidate `.local\releases\windows-file-cleaner-v20260604-121922` was cut from `e6ac3eb` after that app wording fix. Publish ran MVP preflight; `Test-LocalRelease.cmd -RequireCurrentCommit` passed; checklist-only acceptance notes were written to `.local\release-acceptance\release-acceptance-20260604-122009.md` but remain incomplete pending human package acceptance.
- Accepted-package tooling now selects the latest complete acceptance notes by default, so incomplete candidate notes do not replace the accepted `bc9b869` baseline unless an explicit notes path is used.

## Still Unavailable

- Broad/all-manifest real-profile Undo Quarantine.
- Custom or non-exact real-profile Quarantine.
- Custom selected restore.
- Permanent deletion.
- Persisted cleanup history.
- Installed shortcut or installer automation, unless the user explicitly asks for an ADR 0020 follow-up packaging packet.

## Next Best Step

Stop after the second tiny exact real-profile batch. Do not chain another real-profile Quarantine batch while exact-profile displayed undo work is present unless a new Grill with Docs pass decides that outstanding selected-manifest undo work is acceptable. If recovery is needed, use selected-manifest restore only for the exact selected Restore Manifest after readiness, exact `RESTORE`, and immediate selected-restore revalidation pass.

If packaging is the next focus, complete a human acceptance pass for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it. Until then, keep using the accepted package baseline from `bc9b869` for daily accepted-package launch commands.

## High-Value Commands

```powershell
.\tools\Invoke-DailyLocalReadiness.cmd
.\tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes
.\tools\Start-AcceptedLocalRelease.cmd -PrintOnly
.\tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly
.\tools\Invoke-RealProfileNextBatchReview.cmd
.\tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence
.\tools\Show-RealProfileNextBatchChecklist.cmd
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -ShowEntries
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly -ShowEntries
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries
```

All commands above are terminal-only or print-only unless `-PrintOnly` is deliberately removed from an accepted package launcher by the human user.
