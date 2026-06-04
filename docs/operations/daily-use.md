# Daily Use Runbook

Last updated: 2026-06-04

This runbook keeps operational command detail out of the stable domain docs.

## Read-Only Daily Readiness

```powershell
.\tools\Invoke-DailyLocalReadiness.cmd
```

Optional fixture-notes visibility:

```powershell
.\tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes
```

Strict fixture-notes completion check:

```powershell
.\tools\Invoke-DailyLocalReadiness.cmd -RequireFixtureAcceptanceComplete
```

Safety profile: `terminal-readonly` from `docs/codex/safety-profiles.md`.

The daily readiness output first verifies the latest completed accepted package notes, then shows the latest package acceptance notes as informational context. Incomplete candidate notes do not replace the accepted package baseline.

When the latest candidate notes are incomplete, that informational summary also prints the guarded recorder and recheck commands for the human package acceptance pass.

## Accepted Package Launch Commands

Print the accepted package command without launching WPF:

```powershell
.\tools\Start-AcceptedLocalRelease.cmd -PrintOnly
```

Print the accepted fixture command without launching WPF:

```powershell
.\tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly
```

Remove `-PrintOnly` only when the human user intentionally wants to launch the accepted package. A verifier warning that the accepted package commit differs from newer docs-only commits is expected.

## Next-Batch Review Evidence

Use this before asking the user to do another tiny exact `C:\Users\moxhe` WPF batch review:

```powershell
.\tools\Invoke-RealProfileNextBatchReview.cmd
```

That wrapper runs full MVP preflight by default, then exact-profile readiness evidence, then prints the manual WPF checklist. It is not cleanup approval and does not prove a future WPF batch is executable.

Full MVP preflight includes restore/build/test coverage, fixture dry-run/checklist output, the local release acceptance summary regression check, and the whitespace diff check.

Current stop state: after the 2026-06-04 second exact-profile Quarantine batch, exact-profile displayed undo work is expected to be `1`. Do not run or treat another next-batch review as movement evidence while that selected-manifest undo work is present unless a new Grill with Docs pass decides that outstanding undo work is acceptable for another tiny batch.

The next-batch evidence preset now checks displayed undo work before MVP preflight, so a blocked run exits before producing fresh preflight evidence.

Evidence-only preset:

```powershell
.\tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence
```

Checklist-only:

```powershell
.\tools\Show-RealProfileNextBatchChecklist.cmd
```

Safety profile: `real-profile-user-click-only`.

## Restore Manifest Summary

Default summary:

```powershell
.\tools\Summarize-RestoreManifests.cmd
```

Exact-profile display:

```powershell
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -ShowEntries
```

Exact-profile undo-work focus:

```powershell
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly -ShowEntries
```

Exact-profile recovery-review focus:

```powershell
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries
```

Safety profile: `terminal-readonly`.
