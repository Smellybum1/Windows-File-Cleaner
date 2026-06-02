# Daily Use Runbook

Last updated: 2026-06-03

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

Exact-profile displayed undo-work evidence:

```powershell
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayed -RequireNoDisplayedUndoWork
```

Exact-profile recovery-review focus:

```powershell
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries
```

Safety profile: `terminal-readonly`.
