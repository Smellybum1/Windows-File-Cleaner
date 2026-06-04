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
- Acceptance notes: `.local\release-acceptance\release-acceptance-20260604-122009.md`
- Status: verified with `Test-LocalRelease.cmd -RequireCurrentCommit`, but not human-accepted.

This candidate includes app behavior through the real-profile Quarantine inline status wording fix. Its ignored acceptance notes record verifier and commit evidence, but normal launch, fixture launch, overall result, and the remaining checklist items are not recorded. Keep using the accepted baseline until the human package acceptance pass is completed and recorded.

## Verify Accepted Notes

```powershell
.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete
```

## Print Accepted Launch Commands

```powershell
.\tools\Start-AcceptedLocalRelease.cmd -PrintOnly
.\tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly
```

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

## Acceptance Notes

```powershell
.\tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes
.\tools\Record-LocalReleaseAcceptanceNotes.cmd -RecordManualAcceptance
```

Record acceptance notes only after the human package acceptance pass is complete.

## Boundaries

- No shortcut creation or installer behavior in v1.
- Debug-build shortcuts are development-only context.
- Accepted-package/current-HEAD warnings are expected when newer commits are docs-only.
- Permanent deletion, persisted cleanup history, broad/all-manifest restore, custom real-profile Quarantine, and non-exact real-profile movement remain unavailable.
