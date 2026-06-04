# Portable Release Runbook

Last updated: 2026-06-04

Portable v1 is the local self-contained WPF app package. It is not an installer and does not create installed shortcuts.

## Accepted Baseline

- Package: `.local\releases\windows-file-cleaner-v20260602-011556`
- Package commit: `bc9b869`
- Acceptance notes: `.local\release-acceptance\release-acceptance-20260602-011743.md`

## Verified Candidate Pending Acceptance

- Package: `.local\releases\windows-file-cleaner-v20260604-121922`
- Package commit: `e6ac3eb`
- Initial acceptance notes: `.local\release-acceptance\release-acceptance-20260604-122009.md`
- Current pending acceptance notes: `.local\release-acceptance\release-acceptance-20260604-164509.md`
- Status: verified with `Test-LocalRelease.cmd -RequireCurrentCommit`, but not human-accepted.

This candidate includes app behavior through the real-profile Quarantine inline status wording fix. Its initial ignored acceptance notes record verifier and commit evidence from the package cut time, but normal launch, fixture launch, overall result, and the remaining checklist items are not recorded.

The current pending acceptance notes were refreshed after package/readiness tooling commits advanced `HEAD` beyond the packaged app commit. They record verifier evidence, intentionally leave commit evidence unrecorded until the human accepts the expected package/current-HEAD mismatch, and stamp exact `-ReleasePath` commands. Keep using the accepted baseline until the human package acceptance pass is completed and recorded.

## Verify Accepted Notes

```powershell
.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete
```

Without `-Path`, `-RequireComplete` selects the latest completed acceptance notes. Use an explicit `-Path` when inspecting a pending candidate notes file.

Incomplete notes summaries print guarded next-step commands. For a behind-current-`HEAD` candidate, the recorder command includes `-RecordCommitMismatch` and should be used only after the human package acceptance pass and intentional package/current-HEAD mismatch acceptance.

## Print Accepted Launch Commands

```powershell
.\tools\Start-AcceptedLocalRelease.cmd -PrintOnly
.\tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly
```

Without `-AcceptanceNotesPath`, accepted-package launch commands select the latest completed acceptance notes, so incomplete or malformed-looking candidate notes do not replace the accepted baseline.

## Publish A New Local Package

```powershell
.\tools\Publish-LocalRelease.cmd
```

Use this only when app behavior changes should ship as a new accepted package baseline. Publishing runs MVP preflight by default and writes ignored `.local\releases` artifacts.

## Verify A Local Package

```powershell
.\tools\Test-LocalRelease.cmd
.\tools\Test-LocalRelease.cmd -RequireCurrentCommit
```

Targeted accepted package launcher selection regression:

```powershell
.\tools\Test-AcceptedLocalReleaseSelection.cmd
```

This writes temporary ignored complete, incomplete, and malformed-looking notes under `.local\release-acceptance`, writes synthetic print-only package placeholder files under `.local\accepted-release-selection-test`, verifies accepted launcher selection behavior, then removes its temporary files.

Targeted generated acceptance command stamping regression:

```powershell
.\tools\Test-LocalReleaseAcceptanceCommandStamping.cmd
```

This writes temporary synthetic release folders under `.local\release-acceptance-command-stamping-test`, generates ignored package acceptance notes under `.local\release-acceptance`, verifies `-ReleasePath`, `-RequireCurrentCommit`, and `-RecordCommitMismatch` command stamping behavior, then removes the temporary release folders and notes.

Targeted package acceptance summary regression:

```powershell
.\tools\Test-LocalReleaseAcceptanceSummary.cmd
```

This writes temporary ignored complete, incomplete, and malformed-looking notes under `.local\release-acceptance-summary-test` and `.local\release-acceptance`, verifies summary behavior including missing-checklist wording, default completed-notes selection, and explicit non-`.local` path rejection, then removes the test notes.

Targeted daily readiness latest package notes regression:

```powershell
.\tools\Test-DailyReadinessLatestPackageNotes.cmd
```

This writes temporary ignored complete, incomplete, and malformed-looking notes under `.local\release-acceptance`, verifies daily readiness keeps accepted evidence on completed notes while showing newer latest notes as informational context, then removes the test notes.

Targeted package acceptance recorder regression:

```powershell
.\tools\Test-LocalReleaseAcceptanceRecorder.cmd
```

This writes temporary ignored notes under `.local\release-acceptance-recording-test`, verifies recorder guardrails, then removes the test notes.

MVP preflight also runs these regressions by default. Use `.\tools\Invoke-MvpPreflight.cmd -SkipLocalReleaseAcceptanceCommandStampingCheck`, `.\tools\Invoke-MvpPreflight.cmd -SkipAcceptedLocalReleaseSelectionCheck`, `.\tools\Invoke-MvpPreflight.cmd -SkipDailyReadinessLatestPackageNotesCheck`, `.\tools\Invoke-MvpPreflight.cmd -SkipLocalReleaseAcceptanceSummaryCheck`, or `.\tools\Invoke-MvpPreflight.cmd -SkipLocalReleaseAcceptanceRecorderCheck` only for focused local loops where those package acceptance checks are not in scope.

## Acceptance Notes

```powershell
.\tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes
.\tools\Record-LocalReleaseAcceptanceNotes.cmd -RecordManualAcceptance
```

Generated notes stamp the actual `-ReleasePath` used for verifier and checklist commands, and include `-RequireCurrentCommit` only when the notes were created with that switch.

Use `-RequireCurrentCommit` when cutting notes for a package that should match current `HEAD`. For an already verified package candidate that is behind docs-only commits, inspect the package verifier warning and existing metadata instead of requiring current `HEAD`.

Record acceptance notes only after the human package acceptance pass is complete. If the notes do not already have commit evidence recorded because the package intentionally differs from current `HEAD`, pass `-RecordCommitMismatch` only after reviewing and accepting that package/current-HEAD mismatch.

For the current pending candidate, inspect and complete:

```powershell
.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-164509.md"
.\tools\Record-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-164509.md" -RecordManualAcceptance -RecordCommitMismatch
.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-164509.md" -RequireComplete
```

Use the recorder command only after README review, normal launch, fixture launch/read-only fixture Scan, portable boundary review, real-profile stop-boundary confirmation, and intentional acceptance of the `e6ac3eb` package versus docs-only current-`HEAD` mismatch.

## Boundaries

- No shortcut creation or installer behavior in v1.
- Debug-build shortcuts are development-only context.
- Accepted-package/current-HEAD warnings are expected when newer commits are docs-only.
- Permanent deletion, persisted cleanup history, broad/all-manifest restore, custom real-profile Quarantine, and non-exact real-profile movement remain unavailable.
