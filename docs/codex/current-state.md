# Current State

Last updated: 2026-06-04

Use this as the first compact orientation file for new Codex threads. Historical packet evidence and detailed reference docs are archived or referenced separately; do not load them unless the current task needs old packet detail.

## Snapshot

- Repo: `D:\Codex\Windows File Cleaner`
- Branch: `main`
- Latest live evidence: 2026-06-04 second tiny exact real-profile Quarantine batch.
- Latest working app packet: real-profile Quarantine inline status wording fix.
- Latest package candidate: `.local\releases\windows-file-cleaner-v20260604-121922` at `e6ac3eb`, verified but not human-accepted.
- Latest tooling/evidence packet: package summary malformed notes regression.
- Latest docs/workflow packet: startup context compaction.
- Previous docs/workflow baseline: `1ea1b76 Reduce workflow markdown bloat`
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
- Portable package candidate `.local\releases\windows-file-cleaner-v20260604-121922` was cut from `e6ac3eb` after that app wording fix. Publish ran MVP preflight; `Test-LocalRelease.cmd -RequireCurrentCommit` passed; initial checklist-only acceptance notes were written to `.local\release-acceptance\release-acceptance-20260604-122009.md` but remain incomplete pending human package acceptance.
- Refreshed candidate acceptance notes were generated at `.local\release-acceptance\release-acceptance-20260604-134337.md` after docs-only `HEAD` advanced to `bb82b29`. These refreshed notes stamp exact `-ReleasePath` verifier/checklist commands, record verifier evidence, leave commit evidence unrecorded until the human intentionally accepts the expected package/current-HEAD mismatch, and remain incomplete pending human package acceptance.
- Daily readiness now shows the latest package acceptance notes as informational context after verifying the completed accepted notes; incomplete or malformed-looking candidate notes still do not replace the accepted `bc9b869` baseline.
- Incomplete package acceptance summaries now print guarded next-step recorder and recheck commands; for the current candidate they include `-RecordCommitMismatch` and still say the human package acceptance pass must be completed first.
- `tools\Test-FixtureAcceptanceNotes.cmd` verifies fixture acceptance notes summary and recorder behavior with temporary ignored notes: incomplete summaries show recording guidance, `-RequireComplete` fails with blockers, recorder calls require `-RecordManualAcceptance`, `-WhatIf` does not write, and explicit recording completes synthetic notes.
- MVP preflight now runs the fixture acceptance notes regression by default after the fixture checklist; use `-SkipFixtureAcceptanceNotesCheck` only for focused local loops.
- `tools\Test-DailyReadinessFixtureAcceptanceNotes.cmd` verifies optional and strict daily readiness fixture-note forwarding with temporary ignored synthetic package files, package acceptance notes, fixture acceptance notes, and an empty Restore Manifest root. Incomplete required fixture notes fail before accepted launch-command printing; optional incomplete notes show recording guidance and continue; complete required notes pass the normal print-only daily readiness flow.
- MVP preflight now runs the daily readiness fixture acceptance notes regression by default after the standalone fixture notes regression; use `-SkipDailyReadinessFixtureAcceptanceCheck` only for focused local loops.
- `tools\Test-DailyReadinessLatestPackageNotes.cmd` verifies daily readiness keeps accepted evidence on completed notes while showing a newer incomplete candidate notes file as informational context with guarded next steps, including `-RecordCommitMismatch`; it also verifies a newer malformed-looking notes file reports missing evidence as informational context and still cannot block accepted-package readiness.
- MVP preflight now runs the daily readiness latest package notes regression by default after the daily readiness fixture acceptance notes regression; use `-SkipDailyReadinessLatestPackageNotesCheck` only for focused local loops.
- `tools\Test-AcceptedLocalReleaseSelection.cmd` verifies that accepted-package launch commands ignore newer incomplete and malformed-looking notes when selecting by default, while explicit incomplete or malformed-looking notes paths stop before launch-command printing. It uses temporary ignored notes and synthetic package files under `.local` with `-PrintOnly -SkipVerify`.
- MVP preflight now runs the accepted local release selection regression by default before the package acceptance summary regression; use `-SkipAcceptedLocalReleaseSelectionCheck` only for focused local loops.
- `tools\Test-LocalReleaseAcceptanceSummary.cmd` verifies accepted, incomplete, and malformed-looking package acceptance summary behavior with temporary ignored `.local` notes and cleans up after itself. Malformed-looking notes now report missing checklist structure instead of implying all checklist items passed.
- MVP preflight now runs the local release acceptance summary regression check by default before the local release acceptance recorder regression; use `-SkipLocalReleaseAcceptanceSummaryCheck` only for focused local loops.
- `tools\Test-LocalReleaseAcceptanceRecorder.cmd` verifies the package acceptance recorder rejects missing manual intent, missing verifier evidence, and missing commit evidence without explicit mismatch acceptance; it also verifies `-WhatIf` does not write and explicit `-RecordCommitMismatch` can complete synthetic ignored notes.
- MVP preflight now runs the local release acceptance recorder regression check by default after the package summary regression; use `-SkipLocalReleaseAcceptanceRecorderCheck` only for focused local loops.
- Current full MVP preflight passed with restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, fixture acceptance notes regression, daily readiness fixture acceptance notes regression, daily readiness latest package notes regression, accepted local release selection regression, local release acceptance summary regression, local release acceptance recorder regression, real-profile next-batch stop guard regression, daily readiness exact-profile undo spotlight regression, and whitespace diff.
- `Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` now checks displayed undo work before MVP preflight, so outstanding selected-manifest undo work stops the preset before it can produce fresh preflight evidence.
- `tools\Test-RealProfileNextBatchStopGuard.cmd` verifies that early stop behavior with temporary ignored synthetic Restore Manifests. Its clear synthetic path uses `Invoke-RealProfileQuarantineReadiness.cmd -SyntheticRestoreManifestOnly` so CI and clean runners do not need ignored accepted-package notes or package folders for this focused check. The regression now also asserts synthetic readiness rejects missing or outside-`.local` Quarantine Roots, package/fixture acceptance parameters, and missing `-SkipMvpPreflight`.
- MVP preflight now runs the real-profile next-batch stop guard regression by default before `git diff --check`; use `-SkipRealProfileNextBatchStopGuardCheck` only for focused local loops.
- Daily readiness now prints an exact-profile undo-work stop-state spotlight after the broad Restore Manifest summary. On current evidence it shows one exact-profile undo-work manifest: `quarantine-action-draft-20260604014901-b7b402a2`.
- `tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd` verifies with temporary ignored synthetic Restore Manifests that broad daily readiness can show fixture and exact-profile undo work while the final spotlight displays only exact-profile undo work. The regression uses `Invoke-DailyLocalReadiness.cmd -SyntheticRestoreManifestOnly` so CI and clean runners do not need ignored accepted-package notes or package folders for this focused check. It now also asserts synthetic daily readiness rejects missing or outside-`.local` Quarantine Roots and package/fixture acceptance parameters.
- MVP preflight now runs the daily readiness exact-profile undo spotlight regression by default before `git diff --check`; use `-SkipDailyReadinessUndoSpotlightCheck` only for focused local loops.
- Accepted-package tooling now selects the latest complete acceptance notes by default, so incomplete or malformed-looking candidate notes do not replace the accepted `bc9b869` baseline unless an explicit notes path is used; explicit incomplete or malformed-looking accepted-launcher notes paths stop before launch-command printing.
- Release acceptance recording now refuses missing verifier evidence and requires explicit `-RecordCommitMismatch` before commit mismatch evidence is marked recorded.
- Generated package acceptance notes now stamp the actual `-ReleasePath` verifier/checklist commands, include `-RequireCurrentCommit` only when that switch created the notes, and print the exact commit-mismatch recorder command when current-commit evidence is intentionally not required.
- Startup docs were compacted so `docs/domain/context.md`, `docs/domain/glossary.md`, `README.md`, the active roadmap, and `.codex/progress.md` are short read-first summaries. Detailed prior versions live at `docs/domain/context-reference.md`, `docs/domain/glossary-reference.md`, `docs/operations/readme-full-reference.md`, `docs/features/archive/2026-06-01-live-product-readiness-roadmap-history.md`, and `.codex/archive/progress-2026-06-04-pre-context-compaction.md`.

## Still Unavailable

- Broad/all-manifest real-profile Undo Quarantine.
- Custom or non-exact real-profile Quarantine.
- Custom selected restore.
- Permanent deletion.
- Persisted cleanup history.
- Installed shortcut or installer automation, unless the user explicitly asks for an ADR 0020 follow-up packaging packet.

## Next Best Step

Stop after the second tiny exact real-profile batch. Do not chain another real-profile Quarantine batch while exact-profile displayed undo work is present unless a new Grill with Docs pass decides that outstanding selected-manifest undo work is acceptable. If recovery is needed, use selected-manifest restore only for the exact selected Restore Manifest after readiness, exact `RESTORE`, and immediate selected-restore revalidation pass.

The next-batch evidence preset now enforces this stop state before MVP preflight. Do not use the early-stop output as movement evidence.

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
