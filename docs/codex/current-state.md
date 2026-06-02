# Current State

Last updated: 2026-06-03

Use this as the first compact orientation file for new Codex threads. Historical packet evidence lives in `.codex/archive/progress-2026-05-2026-06.md`; do not load that archive unless the current task needs old packet detail.

## Snapshot

- Repo: `D:\Codex\Windows File Cleaner`
- Branch: `main`
- Latest product/evidence packet before this docs cleanup: `3dad056 Record current-head next-batch review evidence`
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
- Accepted portable package launched and scanned the fixture successfully.

## Current Evidence

- `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd` passed on `c7cb545`.
- Full MVP preflight passed in that wrapper: restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and whitespace check.
- Accepted package verification passed with the expected accepted-package/current-HEAD warning.
- Exact-profile Restore Manifest display showed 4 of 10 manifests.
- Displayed exact-profile undo work was `0`.
- Displayed exact-profile recovery review was `2`, both older failed NVIDIA `DXCache` attempts with no moved entries.
- Manual WPF next-batch checklist printed.

## Still Unavailable

- Broad/all-manifest real-profile Undo Quarantine.
- Custom or non-exact real-profile Quarantine.
- Custom selected restore.
- Permanent deletion.
- Persisted cleanup history.
- Installed shortcut or installer automation, unless the user explicitly asks for an ADR 0020 follow-up packaging packet.

## Next Best Step

Ask the user to do the manual WPF next-batch review if they are ready. Do not click Quarantine from Codex, and do not guide a Quarantine click unless the user explicitly chooses a specific tiny exact `C:\Users\moxhe` batch after WPF readiness, exact `QUARANTINE`, approval evidence, and immediate Pre-Execution Revalidation are visible.

## High-Value Commands

```powershell
.\tools\Invoke-DailyLocalReadiness.cmd
.\tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes
.\tools\Start-AcceptedLocalRelease.cmd -PrintOnly
.\tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly
.\tools\Invoke-RealProfileNextBatchReview.cmd
.\tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence
.\tools\Show-RealProfileNextBatchChecklist.cmd
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayed -RequireNoDisplayedUndoWork
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries
```

All commands above are terminal-only or print-only unless `-PrintOnly` is deliberately removed from an accepted package launcher by the human user.
