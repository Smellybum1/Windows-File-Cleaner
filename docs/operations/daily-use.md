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

When strict fixture notes completion passes, completed fixture notes report that evidence is complete and no recorder action is pending.

Explicit package acceptance notes and fixture acceptance notes paths used by daily readiness must stay under ignored `.local`. Package notes paths outside `.local` fail during accepted-package evidence before latest-notes or launch-command printing; fixture notes paths outside `.local` fail during fixture notes evidence before accepted launch-command printing.

Safety profile: `terminal-readonly` from `docs/codex/safety-profiles.md`.

The daily readiness output first verifies the latest completed accepted package notes, then shows the latest package acceptance notes as informational context. Incomplete or malformed-looking candidate notes do not replace the accepted package baseline. Completed accepted notes report that evidence is complete and no recorder action is pending. Package acceptance summaries include current repository `HEAD`, notes/current-HEAD status, and package/current-HEAD status so package/current-HEAD mismatch context is visible.

When the latest candidate notes are incomplete, that informational summary also prints the guarded recorder and recheck commands for the human package acceptance pass. If a latest notes file is malformed-looking, the informational summary reports missing evidence instead of changing accepted-package readiness.

Daily readiness ends with a broad Restore Manifest summary and an exact-profile undo-work stop-state spotlight. After the 2026-06-04 second exact-profile Quarantine batch, the spotlight is expected to show displayed undo work `1` until the selected manifest is restored. MVP preflight runs a synthetic regression for the spotlight by default; that regression uses a focused `.local` Restore Manifest-only mode so CI and clean runners do not need accepted-package evidence. The synthetic mode is guard-tested and is not a substitute for normal daily accepted-package readiness.

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

Full MVP preflight includes restore/build/test coverage, fixture dry-run/checklist output, the fixture acceptance notes regression check, the daily readiness fixture acceptance notes regression check, the daily readiness latest package notes regression check, the local release acceptance command stamping regression check, the accepted local release selection regression check, the local release acceptance summary regression check, the local release acceptance recorder regression check, the real-profile next-batch stop guard regression check, the daily readiness exact-profile undo spotlight regression check, the documentation consistency regression check, and the whitespace diff check. The daily readiness fixture acceptance notes regression includes the explicit non-`.local` fixture notes path guard, and the daily readiness latest package notes regression includes the explicit non-`.local` package notes path guard.

Current stop state: after the 2026-06-04 second exact-profile Quarantine batch, exact-profile displayed undo work is expected to be `1`. Do not run or treat another next-batch review as movement evidence while that selected-manifest undo work is present unless a new Grill with Docs pass decides that outstanding undo work is acceptable for another tiny batch.

The next-batch evidence preset now checks displayed undo work before MVP preflight, so a blocked run exits before producing fresh preflight evidence. MVP preflight also runs the synthetic stop-guard and daily readiness undo spotlight regressions by default; both use focused `.local` Restore Manifest-only modes for CI and clean-runner portability. Those synthetic modes are guard-tested and are not substitutes for normal daily or next-batch evidence. Use `.\tools\Invoke-MvpPreflight.cmd -SkipRealProfileNextBatchStopGuardCheck`, `.\tools\Invoke-MvpPreflight.cmd -SkipDailyReadinessUndoSpotlightCheck`, or `.\tools\Invoke-MvpPreflight.cmd -SkipDocumentationConsistencyCheck` only for focused local loops where that guard, spotlight, or docs check is not in scope.

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
