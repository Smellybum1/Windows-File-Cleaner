# Progress Log

This file is the running evidence log for Codex work in this repository.

Use it to preserve what was completed, what was verified, what was rejected, and what should happen next. Keep entries compact and factual.

## Current status

Storage Scan MVP packet implemented and tested by the user against `C:\Users\moxhe`. The app has a broad read-only review workflow, fixture WPF Quarantine execution, current-fixture undo, fixture selected restore, exact real-profile selected restore under ADR 0019 after selected readiness, exact `RESTORE`, and immediate revalidation, and first-phase exact real-profile Quarantine execution under ADR 0017/0018 after readiness, exact `QUARANTINE`, approval evidence, selected restore trust, and immediate pre-execution revalidation. The user completed the sacrificial selected real-profile restore trust test on 2026-06-01 and reported steps 1-9 all succeeded. The user later reported the first exact real-profile folder Quarantine attempt failed safely with `moved 0, failed 1` because Windows `Directory.Move` cannot move directories from `C:` to the preferred `D:` Quarantine Root; the next packet added a guarded cross-volume directory fallback and a taller WPF Quarantine Execution Gate details area. The user then retried and reported another safe `moved 0, failed 1` result because a descendant NVIDIA `.nvph` file was in use by another process; the next packet added in-use source checks to Pre-Execution Revalidation and expanded the WPF Quarantine Execution Gate details area again. The user then approved a different tiny exact real-profile batch and reported a successful first live Quarantine on 2026-06-01: one row moved from `C:\Users\moxhe\AppData\Local\pip\cache\http\b\c`, one Restore Manifest completed, `moved 1, failed 0`, zero readiness blockers, and no Codex-clicked movement. The user then attempted selected restore for that manifest and reported a safe failure, `Restored 0 | Failed 1 | Recovery review: yes`, because directory restore from `D:` back to `C:` hit the same cross-volume limitation. The cross-volume selected restore retry packet fixed directory selected restore with the guarded copy-then-delete fallback, treated still-present `RestoreFailed` selected entries as narrowly retryable when the original path is clear, and added highlighted key-status strips above dense Quarantine and Selected Restore gate text. The user then retried selected restore for that first-live manifest and reported success from the highlighted strip: `selected restore succeeded. Restored 1, failed 0`; follow-up rediscovery showed the manifest restored/already restored, rescan completed normally, and the restored `pip\cache\http\b\c` path appeared again. Portable v1 packaging now exists through `.\tools\Publish-LocalRelease.cmd`, producing ignored self-contained `.local\releases` folder/zip artifacts after MVP preflight; the user reran the publisher from clean `main`, launched the packaged app against the fixture scope, clicked fixture Scan, and confirmed the package, app launch, and read-only fixture scan all worked. Current accepted local package baseline is `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`, with completed ignored acceptance notes at `.local\release-acceptance\release-acceptance-20260602-011743.md`; `.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` reports verifier/current-commit/normal-launch/fixture-launch evidence recorded, overall result `Pass`, and `6 pass, 0 issue, 0 not checked, 0 not recorded`. New packages include executable SHA-256 metadata, zip `.sha256` sidecars, release-local `README-FIRST.txt`, and normal/fixture launch `.cmd` scripts; `.\tools\Test-LocalRelease.cmd` verifies the latest or explicit local package folder, executable, executable checksum, zip checksum, README, metadata, launch scripts, zip, safety-boundary lines, and commit evidence without launching WPF or scanning anything; `.\tools\Start-LocalRelease.cmd` verifies by default, then prints package-local README/script paths, prints a package-level acceptance checklist with exact launch commands using `-ChecklistOnly`, writes ignored package acceptance notes with exact normal/fixture launch commands using `-ChecklistOnly -WriteAcceptanceNotes`, or starts the latest package from the repo root; `.\tools\Start-AcceptedLocalRelease.cmd` requires completed ignored acceptance notes, selects the accepted release, and delegates to `Start-LocalRelease.cmd` for verification and print/launch behavior without treating later docs-only commits as the package baseline; `.\tools\Invoke-DailyLocalReadiness.cmd` verifies completed accepted package notes, can optionally summarize or strictly require Fixture Acceptance Notes, runs one accepted package verification pass, prints accepted normal and fixture launch commands, and prints Restore Manifest Summary output without creating shortcuts, installing anything, launching WPF, scanning, moving, restoring, deleting, approving cleanup, or creating cleanup history; `.\tools\Summarize-LocalReleaseAcceptanceNotes.cmd` summarizes ignored package acceptance notes and prints those launch commands without launching WPF or scanning; and `.\tools\Record-LocalReleaseAcceptanceNotes.cmd -RecordManualAcceptance` can mark ignored notes complete after a human package acceptance pass without launching WPF, scanning, moving, restoring, deleting, approving cleanup, or creating cleanup history. Generated package acceptance notes pre-record verifier/current-commit evidence only when the launcher has proven those facts; launch and fixture scan evidence stay manual until the recorder is used after human acceptance. README has a top-level Daily Local Use section that surfaces the daily readiness command, optional fixture notes status, accepted package print commands, the accepted evidence check, the real-profile readiness review next-batch preset, the next-batch WPF checklist, the read-only Restore Manifest summary, and the stop boundary before real-profile movement. `.\tools\Summarize-RestoreManifests.cmd` provides a read-only terminal summary of action-scoped Restore Manifests under a selected Quarantine Root without launching WPF, scanning, moving, restoring, deleting, writing manifests, approving cleanup, creating cleanup history, or adding broad/all-manifest restore; it now supports `-CleanupScope` to focus displayed manifests by exact Cleanup Scope, `-RecoveryReviewOnly` to focus displayed manifests that need recovery review, `-UndoWorkOnly` to focus displayed manifests with moved entries still needing selected restore/undo review, `-RequireNoRecoveryReview` / `-RequireNoUndoWork` to fail read-only terminal evidence checks against full selected-root state, and `-RequireAnyDisplayed` / `-RequireNoDisplayedRecoveryReview` / `-RequireNoDisplayedUndoWork` to fail read-only terminal evidence checks against only the displayed manifest set. `.\tools\Record-FixtureAcceptanceNotes.cmd -RecordManualAcceptance` can mark ignored all-pass fixture notes complete after a human visible fixture pass without launching WPF, creating fixtures, scanning, moving, restoring, deleting, approving cleanup, or creating cleanup history. Latest full real-profile next-batch preset evidence passed: full MVP preflight passed restore/build/core tests/WPF app tests/fixture `-WhatIf`/checklist/whitespace, daily readiness included optional fixture notes status and accepted package verification with the expected package/current-HEAD warning, exact-profile Restore Manifest display showed 4 of 10 manifests with zero displayed undo-work manifests and 2 displayed recovery-review manifests, recovery-review focus showed the two older failed NVIDIA `DXCache` attempts, and undo-work focus showed zero exact-profile matches. Latest optional fixture-notes evidence confirms the latest ignored fixture notes `.local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md` remain formally unfilled: preflight/worktree evidence not recorded, overall result not recorded, and 10 checklist items not recorded; strict `-RequireFixtureAcceptanceComplete` fails as expected. Broad/all-manifest WPF Undo Quarantine, custom/non-exact real-profile Quarantine, custom selected restore, permanent deletion, and persisted cleanup history remain unavailable. Fresh-thread handoff notes and a startup prompt live in `docs/codex/thread-handoff.md`; the latest handoff now names the Real-Profile Next-Batch WPF Checklist packet without changing app movement behavior or local package artifacts.

Recent UI correction: the user reported `Discover manifests` was not visible in the Quarantine tab after selected restore recovery; it was still buried in the Main Grid detail pane. The follow-up packet moved Restore Manifest discovery, selected readiness, selected restore gate/result, and all-manifest readiness into a dedicated `Restore Manifest Review` panel under the Quarantine tab, and moved detailed Quarantine Preview output into the Quarantine tab.

Latest readiness tooling update: `.\tools\Invoke-RealProfileQuarantineReadiness.cmd` runs full MVP preflight by default, then daily local readiness plus focused Restore Manifest recovery-review and undo-work summaries before any future tiny exact real-profile Quarantine batch review. Restore Manifest display focus defaults to exact `C:\Users\moxhe`, while `-AllCleanupScopes` preserves the older fixture-inclusive display path. `-RequireNextBatchEvidence` is now the exact-profile-only preset for the next tiny batch review: it includes Fixture Acceptance Notes status, requires displayed Restore Manifest evidence, and requires zero displayed undo-work manifests. Displayed strictness flags such as `-RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` can still be supplied directly for custom evidence runs, and `-IncludeFixtureAcceptanceNotes` / `-RequireFixtureAcceptanceComplete` can forward optional fixture notes evidence through the daily readiness step. It is terminal evidence only and does not launch WPF, click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, approve cleanup, or create cleanup history. It does not replace WPF readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, immediate Pre-Execution Revalidation, or explicit user approval for a specific tiny batch.

Latest next-batch WPF checklist update: `.\tools\Show-RealProfileNextBatchChecklist.cmd` prints the manual WPF review checklist after the next-batch evidence preset. It names exact `C:\Users\moxhe` scope, accepted package launch-command printing, scan/shortlist review, 10-row/1 GB caps, hard blockers, Quarantine tab evidence, the human-only exact `QUARANTINE` click boundary, and rediscover/rescan follow-up without launching WPF, scanning, moving, restoring, deleting, approval, or cleanup history.

Latest Restore Manifest evidence update: `.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayed -RequireNoDisplayedUndoWork` can prove the exact real-profile display exists and has no displayed undo-work manifests while preserving full-root aggregate counts and full-root strictness flags. The same displayed strictness is forwarded through daily readiness and real-profile readiness via `-RequireAnyDisplayedRestoreManifest`, `-RequireNoDisplayedRecoveryReview`, and `-RequireNoDisplayedUndoWork`. This is read-only evidence only and does not launch WPF, scan, move, restore, delete, write manifests, approve cleanup, or create cleanup history.

Latest daily readiness evidence refresh: `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed on current `main` at `b0fc763`. It verified completed accepted package notes, verified the accepted package once with the expected accepted-package/current-HEAD warning, printed accepted normal and fixture launch commands in print-only mode, and showed exact-profile displayed Restore Manifest evidence: 4 displayed exact-profile manifests, 0 displayed undo-work manifests, and 2 displayed recovery-review manifests. Direct read-only recovery-review focus with `-ShowEntries` showed the two older failed NVIDIA `DXCache` attempts, and direct undo-work focus showed zero exact-profile undo-work manifests. No WPF app was launched, no scan was started, and no files were moved, restored, deleted, approved, or added to cleanup history.

Latest daily fixture-notes status: `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed without launching WPF, scanning, moving, restoring, deleting, approval, or cleanup history, and printed optional Fixture Acceptance Notes status. Strict `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RequireFixtureAcceptanceComplete` failed as expected because the latest ignored fixture notes still have preflight/worktree evidence not recorded, overall result not recorded, and 10 checklist items not recorded.

Latest full real-profile next-batch evidence refresh: `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` passed. Full MVP preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, printed the checklist-only fixture guidance, and ran whitespace diff checking. Daily readiness then verified accepted package notes, printed optional fixture notes status, ran one accepted package verifier pass with the expected package/current-HEAD warning, printed accepted normal/fixture launch commands in print-only mode, and showed exact-profile Restore Manifest display `(4 of 10)` with displayed undo work `0` and displayed recovery review `2`. Focused recovery-review evidence showed the two older failed exact-profile NVIDIA `DXCache` attempts, focused undo-work evidence showed zero exact-profile matches, and preset misuse with all-scope or fixture scope failed as expected. No WPF app was launched, no real-profile scan was started, and no files were moved, restored, deleted, approved, or added to cleanup history.

Latest WPF UI packet added a compact Main Grid Active Review Lens Summary above Storage Scan rows, mirroring the existing Filter Summary so default scan, Safety Summary shortcut, and stacked filter/search context remain visible after tab switches; it hides for current-session quarantined rows. User visual review on 2026-06-01 approved the compact wide-header layout with Review Shortlist totals before scan totals, and the user later ran the visible fixture review flow and reported that it looks good. The latest fixture-checklist wording packet clarified that the current-session review step's hoverable `?` cue and `Status state:` wording belong to Review Grid Mode Status, while Main Grid Active Review Lens Summary appears for Storage Scan rows and hides for current-session quarantined rows. Current handoff evidence is the current-evidence baseline: full `.cmd` MVP preflight passed after the Checklist-Only Visible Fixture Next Step packet at `71cf15a`, including restore, build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output with the exact visible fixture next step, whitespace diff, and the notes-enabled next manual fixture command; user-reported manual fixture visual acceptance is now recorded, while the formal latest notes checklist remains unfilled. The live-product readiness roadmap now separates visible fixture acceptance, fresh real-profile read-only retest, selected real-profile restore, first real-profile Quarantine execution, recovery confidence, packaging, and later deletion/history decisions, and its manual fixture acceptance row names user-reported visual acceptance plus the unfilled clean-worktree notes evidence. The fixture launcher can write an ignored `.local` fixture acceptance notes template from the same checklist item source with `-WriteAcceptanceNotes`; the notes are grouped by fixture review area and now include an acceptance evidence header with repo path, Git branch/commit, worktree status at notes creation, .NET SDK, WPF app project/target framework/WPF flag, preflight command, visible fixture command, preflight/worktree checkboxes, local-not-cleanup-history wording, and embedded exact recorder/summary/completion-check commands. The latest ignored notes file is `.local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md` from clean `main` at `433064e` with `Worktree status at notes creation: clean`, but the acceptance evidence and checklist items remain not recorded unless the user fills them manually or intentionally runs the recorder after an all-pass fixture review. A read-only summary helper can summarize the latest or explicit ignored fixture acceptance notes, including metadata, worktree status at notes creation when present, acceptance-evidence checkbox states, overall result, checklist totals, and issue/not-checked/not-recorded items with compact notes or prompt previews, without launching WPF, scanning, moving, restoring, deleting, or creating cleanup history. The helper also supports `-RequireComplete`, which exits non-zero until local acceptance notes have recorded preflight/worktree evidence, an overall result, and no not-checked or not-recorded checklist items. `.\tools\Record-FixtureAcceptanceNotes.cmd -RecordManualAcceptance` can mark ignored all-pass fixture notes complete after a human visible pass; it updates ignored `.local` notes only and does not launch WPF, create fixtures, scan, move, restore, delete, approve cleanup, or create cleanup history. The fixture launcher now also prints exact recorder, summary, and completion-check commands for the notes file it writes, and MVP preflight success output points to that follow-up after `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`. Checklist-only launcher output now also repeats the exact next visible fixture command and the no-preflight/no-fixture/no-WPF/no-scan/no-movement boundary before exiting. No real user files were scanned or modified.

Fresh clean-worktree notes evidence: `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes` generated `.local\fixture-review-acceptance\fixture-acceptance-20260601-131233.md` from `dd86566` with `Worktree status at notes creation: clean`. The read-only summary helper printed that clean stamp, and `-RequireComplete` returned exit code 1 as expected because the checklist-only notes are still unfilled. This did not run preflight, create fixture files, launch WPF, scan, move, restore, delete, or create cleanup history.

User-reported manual fixture visual pass: on 2026-06-01 the user ran the visible fixture review flow and reported that it looks good. The latest local ignored acceptance notes now summarize `.local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md` from clean `main` at `433064e`; its checklist remains unfilled, so this is user-reported visual acceptance rather than formal completed checklist evidence. No real-profile scan, real-profile movement, permanent deletion, or cleanup history was requested or recorded.

User-reported fresh real-profile read-only retest: on 2026-06-01 the user completed the requested full preflight plus WPF run against `C:\Users\moxhe` and reported that everything worked well. This covers the real-profile scan gate, read-only real scan completion, search responsiveness, tab/header usability, Review Shortlist context, and preview-only Quarantine boundary requested in the step-by-step retest. No real-profile Quarantine execution, real-profile restore, permanent deletion, or cleanup history was requested or recorded.

Latest full preflight evidence: `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed after the Checklist-Only Visible Fixture Next Step packet at `71cf15a`. It restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, printed sectioned checklist-only output with the exact visible fixture next step and checklist-only safety boundary, ran whitespace diff checking, and ended with `MVP preflight passed. No real user files were scanned or modified.` No WPF app was launched, no real-profile scan was run, and no files were moved, restored, deleted, or added to cleanup history.

Current Evidence Wording Alignment clarified that the clean notes preview from `f181627` plus the then-current full `.cmd` MVP preflight after `8529a91` were the handoff verification pair before the latest preflight refresh. Older `b9bd33d` and `fd8e1d4` references remain only in historical packet summaries. This was docs-only and did not launch WPF, scan, move, restore, delete, or create cleanup history.

## Next recommended work

Use `.\tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` before considering another tiny exact real-profile Quarantine batch. It focuses Restore Manifest display rows to exact `C:\Users\moxhe`, includes formal fixture-notes status, proves the exact-profile display exists, and requires zero displayed undo-work manifests. Use `.\tools\Show-RealProfileNextBatchChecklist.cmd` to print the post-preset manual WPF checklist without launching WPF. Use `.\tools\Invoke-RealProfileQuarantineReadiness.cmd -AllCleanupScopes` only when fixture manifests should also be displayed outside the preset. Use `.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayed -RequireNoDisplayedUndoWork` for direct exact-profile displayed undo-work evidence, and `.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries` when exact real-profile recovery-review debt needs inspection. These commands are evidence-only; WPF readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, immediate Pre-Execution Revalidation, and explicit user approval for the specific click still remain required.

1. Next safest live-product step is to reuse the latest daily evidence refresh or rerun `.\tools\Invoke-DailyLocalReadiness.cmd` from the README Daily Local Use section when accepted package and Restore Manifest evidence should be refreshed, add `-IncludeFixtureAcceptanceNotes` when formal fixture notes status should be visible, use `.\tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` when another tiny exact real-profile batch is being reviewed, complete formal fixture acceptance notes if desired, decide whether a shortcut/installer is worth a later explicit user-approved packaging packet, or guide only another tiny exact real-profile Quarantine batch after fresh full MVP preflight. The first-live selected restore recovery loop is complete by user report: rediscovery showed restored/already restored, rescan completed normally, and the restored `pip\cache\http\b\c` path appeared again. Keep future movement selected-batch-only, exact `C:\Users\moxhe`, exact confirmation, immediate revalidation, no broad/all-manifest Undo, no permanent deletion, and no cleanup history.
2. Do not run or click real-profile Quarantine or restore execution from Codex; the user must explicitly approve each specific batch or selected Restore Manifest after reviewing the readiness output.
3. If formal fixture acceptance notes are useful before movement work after an all-pass visible fixture review, run `.\tools\Record-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md" -RecordManualAcceptance`, then run `.\tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md" -RequireComplete`; fill notes manually instead when there were issues or not-checked items. The user has already reported that the visible fixture pass looks good, but the local checklist remains unfilled.
4. Revisit .NET 10, installer/shortcut automation, permanent deletion, persisted cleanup history, and broad restore only as separate future decisions.

## Completed packets

### 2026-06-02: Real-Profile Next-Batch WPF Checklist

Status: completed

Evidence:

- The next-batch evidence preset proves terminal readiness evidence, but a future human-clicked tiny batch still depends on manual WPF review.
- The WPF review steps are long enough that a terminal checklist is safer than relying on chat history or memory.
- The helper must remain print-only and must not launch WPF or click real-profile movement.

Implementation:

- Added `tools\Show-RealProfileNextBatchChecklist.ps1` and `.cmd`.
- The helper prints prerequisite evidence, accepted package launch-command printing, exact `C:\Users\moxhe` scope, scan/shortlist review, 10-row/1 GB caps, hard blockers, Quarantine tab evidence, human-only exact `QUARANTINE` click boundary, and rediscover/rescan follow-up.
- Updated README, domain context, glossary, live-product roadmap, next-batch preset feature notes, new feature brief, handoff, and this progress log.
- Did not launch WPF, click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change movement availability.

Verification:

- `cmd.exe /c tools\Show-RealProfileNextBatchChecklist.cmd` passed and printed the manual WPF checklist without launching WPF or touching files.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireNextBatchEvidence` passed and preserved the terminal evidence preset path.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected CRLF conversion warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-real-profile-next-batch-evidence-preset.md`
- `docs/features/2026-06-02-real-profile-next-batch-wpf-checklist.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is read-only terminal guidance over existing ADR 0017/0018/0019 gates, not a cleanup execution, restore scope, persistence, deployment, data-model, or security decision.

Open questions:

- Whether the next user-reviewed tiny exact batch is useful after the user reviews WPF readiness and explicitly approves the specific click.

Rejected ideas buffer:

- Do not make the checklist launch WPF by default.
- Do not treat the checklist as cleanup approval or as proof that a future WPF batch is executable.

### 2026-06-02: Real-Profile Next-Batch Evidence Preset

Status: completed

Evidence:

- The safest terminal review before another tiny exact real-profile Quarantine batch previously required remembering multiple strict flags.
- `Invoke-RealProfileQuarantineReadiness.cmd` already defaulted display focus to exact `C:\Users\moxhe` and could forward displayed strictness plus fixture-notes status.
- The next-batch command should stay exact-profile-only and evidence-only.

Implementation:

- Added `-RequireNextBatchEvidence` to `tools\Invoke-RealProfileQuarantineReadiness.ps1`.
- The preset rejects `-AllCleanupScopes` and non-exact Cleanup Scopes.
- The preset forwards Fixture Acceptance Notes status, requires exact-profile displayed Restore Manifest evidence, and requires zero displayed undo-work manifests.
- Kept recovery-review manifests visible without making them a default blocker.
- Updated README, domain context, glossary, live-product roadmap, real-profile readiness feature notes, new feature brief, handoff, and this progress log.
- Did not launch WPF, click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change movement availability.

Verification:

- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireNextBatchEvidence` passed. It printed optional Fixture Acceptance Notes status, exact-profile displayed Restore Manifest evidence, zero displayed undo-work manifests, two displayed recovery-review manifests, and no WPF launch, scan, movement, restore, deletion, approval, or cleanup history.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -AllCleanupScopes -RequireNextBatchEvidence` failed as expected with an exact-profile-only guard.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -CleanupScope "D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture" -RequireNextBatchEvidence` failed as expected with an exact-profile-only guard.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` passed. Full MVP preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, printed checklist-only fixture guidance, ran whitespace diff checking, then printed accepted package evidence, fixture-notes status, exact-profile Restore Manifest display, recovery-review focus, and undo-work focus.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-real-profile-next-batch-evidence-preset.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a read-only local tooling preset over existing ADR 0017/0018/0019 gates, not a cleanup execution, restore scope, persistence, deployment, data-model, or security decision.

Open questions:

- Whether the next user-reviewed tiny exact batch is useful after the user reviews WPF readiness and explicitly approves the specific click.

Rejected ideas buffer:

- Do not make displayed recovery-review debt a default blocker in the preset while the known exact-profile failed NVIDIA attempts remain historical recovery-review evidence.
- Do not treat the preset as cleanup approval.

### 2026-06-02: Full Real-Profile Readiness After Fixture Status

Status: completed

Evidence:

- `ceed1fc` added optional fixture-notes status to daily readiness and real-profile readiness.
- A full current-head terminal readiness refresh was useful before any future exact real-profile batch review.
- The command remains evidence-only and does not replace WPF readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, immediate Pre-Execution Revalidation, or explicit user approval for a specific tiny batch.

Implementation:

- No app behavior or tooling behavior changed.
- Updated handoff/progress evidence only.
- Did not launch WPF, click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change movement availability.

Verification:

- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -IncludeFixtureAcceptanceNotes -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed on `ceed1fc`.
- Full MVP preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, printed checklist-only fixture guidance, and ran whitespace diff checking.
- Daily readiness verified completed accepted package notes, printed optional fixture notes status, ran one accepted package verifier pass with the expected accepted-package/current-HEAD warning, and printed accepted normal/fixture launch commands in print-only mode.
- Exact-profile Restore Manifest display showed 4 of 10 manifests, displayed undo work `0`, and displayed recovery review `2`.
- Recovery-review focus showed the two older failed exact-profile NVIDIA `DXCache` attempts.
- Undo-work focus showed zero exact-profile matches.
- No WPF app was launched, no real-profile scan was started, and no files were moved, restored, deleted, approved, or added to cleanup history.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is current-state read-only evidence refresh, not a durable product, architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security decision.

Open questions:

- Whether the user wants to record the existing user-reported fixture pass in ignored notes, or only use the recorder for the next visible pass.
- Whether a later WPF restore/history surface should group manifests by Cleanup Scope.

Follow-up work:

- Keep using the full readiness command before considering another tiny exact real-profile Quarantine batch.
- Continue to require WPF readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, immediate Pre-Execution Revalidation, and explicit user approval before any specific click.

Risky assumptions:

- Current full terminal evidence is useful enough to refresh handoff/progress without cutting a new accepted package.

### 2026-06-02: Daily Readiness Fixture Acceptance Status

Status: completed

Evidence:

- Daily readiness already composed accepted-package evidence and Restore Manifest Summary output.
- Formal Fixture Acceptance Notes status was available only as a separate summary helper.
- The latest ignored fixture notes remain intentionally unfilled unless the user records or fills them after a human all-pass visible fixture review.

Implementation:

- Added `-IncludeFixtureAcceptanceNotes`, `-FixtureAcceptanceNotesPath`, and `-RequireFixtureAcceptanceComplete` to `tools\Invoke-DailyLocalReadiness.ps1`.
- `-IncludeFixtureAcceptanceNotes` prints the read-only fixture notes summary.
- `-FixtureAcceptanceNotesPath` points at a specific notes file and implies summary output.
- `-RequireFixtureAcceptanceComplete` forwards `-RequireComplete` and fails when fixture notes are incomplete.
- Forwarded those flags from `tools\Invoke-RealProfileQuarantineReadiness.ps1` into the daily readiness step.
- Updated README, domain docs, glossary, live-product roadmap, daily readiness and real-profile readiness feature briefs, handoff, and this progress log.
- Did not launch WPF, click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change movement availability.

Verification:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed. It printed accepted package evidence, optional fixture notes summary, accepted print-only launch commands, and exact-profile Restore Manifest evidence. Fixture notes summary showed preflight/worktree evidence not recorded, overall result not recorded, and 10 checklist items not recorded.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RequireFixtureAcceptanceComplete` failed as expected during `Fixture acceptance notes evidence` because the latest ignored fixture notes are incomplete.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -IncludeFixtureAcceptanceNotes -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed and forwarded optional fixture notes summary through daily readiness. Preflight was intentionally skipped for this smoke check and the output warned not to use skipped preflight as fresh movement evidence.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed without preflight, fixture creation, WPF launch, scan, movement, restore, deletion, approval, or cleanup history.
- `git diff --check` passed with expected CRLF conversion warnings.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-daily-local-readiness-check.md`
- `docs/features/2026-06-02-daily-readiness-fixture-acceptance-status.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is opt-in read-only terminal evidence plumbing over existing ignored local notes and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

Open questions:

- Whether to record the existing user-reported fixture visual pass into ignored fixture notes, or wait for a future visible pass.

Follow-up work:

- Use `-IncludeFixtureAcceptanceNotes` when formal fixture notes status should be visible in daily or real-profile readiness evidence.
- Use `-RequireFixtureAcceptanceComplete` only when incomplete fixture notes should fail the check.

Risky assumptions:

- Keeping fixture notes status opt-in preserves daily readiness usability while still making the formal notes gap easy to surface.

### 2026-06-02: Daily Readiness Evidence Refresh

Status: completed

Evidence:

- Current `main` is `b0fc763`.
- The accepted package baseline remains `.local\releases\windows-file-cleaner-v20260602-011556` at package commit `bc9b869`, with completed ignored release acceptance notes at `.local\release-acceptance\release-acceptance-20260602-011743.md`.
- Exact-profile Restore Manifest display remains distinct from full-root fixture history.

Implementation:

- No app behavior or tooling behavior changed.
- Updated handoff/progress evidence only.
- Did not launch WPF, click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change movement availability.

Verification:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed. It verified completed accepted package notes, ran one accepted package verifier pass, printed accepted normal and fixture launch commands in print-only mode, and showed exact-profile Restore Manifest display `(4 of 10)` with displayed undo work `0` and displayed recovery review `2`. The expected accepted-package/current-HEAD warning appeared because the accepted package predates newer docs/tooling commits.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries` passed and displayed the two exact-profile recovery-review manifests, both older failed NVIDIA `DXCache` attempts with entry-level error evidence.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly -RequireNoDisplayedUndoWork` passed and showed zero exact-profile undo-work manifests.
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md"` passed and confirmed the latest clean fixture notes remain formally unfilled: preflight/worktree evidence not recorded, overall result not recorded, and `10 not recorded` checklist items.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed without preflight, fixture creation, WPF launch, scan, movement, restore, deletion, approval, or cleanup history.
- `git diff --check` passed with expected CRLF conversion warnings.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is current-state read-only evidence refresh, not a durable product, architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security decision.

Open questions:

- Whether the user wants to record the existing user-reported fixture pass in ignored notes, or only use the recorder for the next visible pass.
- Whether a later WPF restore/history surface should group manifests by Cleanup Scope.

Follow-up work:

- Before any future tiny exact real-profile Quarantine batch, run full `Invoke-RealProfileQuarantineReadiness.cmd` and then have the user review WPF readiness, exact `QUARANTINE`, approval evidence, and immediate revalidation before any specific click.

Risky assumptions:

- Current read-only terminal evidence is useful enough to refresh handoff/progress without cutting a new accepted package.

### 2026-06-02: Fixture Acceptance Notes Recorder

Status: completed

Evidence:

- Fixture acceptance notes could be written and summarized, but completing an all-pass human fixture review still required hand-editing ignored markdown.
- Portable release acceptance already had an explicit manual recorder; fixture notes needed the same friction reduction without weakening the human-review boundary.

Implementation:

- Added `tools\Record-FixtureAcceptanceNotes.ps1` and `.cmd`.
- The recorder defaults to the latest ignored `.local\fixture-review-acceptance\fixture-acceptance-*.md` notes file or accepts `-Path`.
- Requires `-RecordManualAcceptance` before writing.
- Marks preflight/worktree evidence recorded, overall result `Pass`, all fixture checks `Pass`, and writes a summary line for all-pass manual acceptance.
- Refuses paths outside ignored `.local`.
- Updated `Start-MvpFixtureReview.ps1` so generated notes and launcher output print the recorder command before summary and `-RequireComplete` commands.
- Updated README, domain docs, glossary, fixture notes feature briefs, live-product roadmap, handoff, and this progress log.
- Did not launch WPF, click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, approve cleanup, create cleanup history, add broad/all-manifest restore, or change movement availability.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes` passed and printed the recorder plus summary commands without preflight, fixture creation, WPF launch, scan, movement, restore, deletion, approval, or cleanup history.
- Copied the generated ignored notes to `.local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md` for verification.
- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd` failed as expected without `-RecordManualAcceptance`.
- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md"` failed as expected without `-RecordManualAcceptance`.
- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md" -RecordManualAcceptance -Summary "Test-only recorded fixture acceptance for recorder verification."` passed and updated only copied ignored notes.
- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md" -RecordManualAcceptance -WhatIf` passed and reported no notes changes.
- `cmd.exe /c tools\Record-FixtureAcceptanceNotes.cmd -Path README.md -RecordManualAcceptance` failed as expected because the path is outside ignored `.local`.
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-acceptance-recording-test\fixture-acceptance-recording-test.md" -RequireComplete` passed with `10 pass, 0 issue, 0 not checked, 0 not recorded`.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected CRLF conversion warnings.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-02-fixture-acceptance-notes-recorder.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local ignored evidence-note recording and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

Open questions:

- Whether the user wants to record the existing user-reported fixture pass in ignored notes, or only use the recorder for the next visible pass.

Follow-up work:

- Use `Record-FixtureAcceptanceNotes.cmd -RecordManualAcceptance` only after a human completes an all-pass visible fixture review.

Risky assumptions:

- The explicit manual-confirmation flag is enough to keep the recorder from being mistaken for automated fixture acceptance.

### 2026-06-02: Restore Manifest Displayed Strictness

Status: completed

Evidence:

- Exact real-profile Restore Manifest display currently shows four manifests under `C:\Users\moxhe`, with zero displayed undo-work manifests and two displayed recovery-review manifests.
- Full selected-root strictness still reports fixture undo-work, which is useful aggregate context but not the same question as exact-profile displayed evidence.

Implementation:

- Added displayed strict flags to `tools\Summarize-RestoreManifests.ps1`: `-RequireAnyDisplayed`, `-RequireNoDisplayedRecoveryReview`, and `-RequireNoDisplayedUndoWork`.
- Printed displayed undo-work and recovery-review counts when display filters or displayed strict flags are active.
- Forwarded displayed strictness through `tools\Invoke-DailyLocalReadiness.ps1` and `tools\Invoke-RealProfileQuarantineReadiness.ps1`.
- Kept existing full-root strict flags unchanged and kept `-RequireAnyDisplayedRestoreManifest` from blocking the real-profile readiness undo-work focus when zero exact-profile undo-work manifests is the desired evidence.
- Updated README, domain docs, glossary, live-product readiness roadmap, real-profile readiness feature brief, cleanup-scope filter feature brief, new feature brief, handoff, and this progress log.
- Did not launch WPF, click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change movement availability.

Verification:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayed -RequireNoDisplayedUndoWork` passed; exact-profile display showed 4 of 10 manifests, displayed undo work 0, displayed recovery review 2, while full-root aggregates still showed 2 undo-work and 2 recovery-review manifests.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -RequireNoDisplayedRecoveryReview` failed as expected with exit code 1 because two displayed exact-profile manifests need recovery review.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed; daily display showed 4 exact-profile manifests, undo-work focus showed 0 exact-profile undo-work manifests, and no WPF launch, scan, movement, restore, deletion, approval, or cleanup history occurred.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireNoDisplayedRecoveryReview` failed as expected with exit code 1 during daily Restore Manifest summary because two displayed exact-profile manifests need recovery review.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected CRLF conversion warnings.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/features/2026-06-02-restore-manifest-cleanup-scope-filter.md`
- `docs/features/2026-06-02-restore-manifest-displayed-strictness.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is read-only terminal evidence behavior and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

Open questions:

- Whether a later WPF restore/history surface should group manifests by Cleanup Scope.

Follow-up work:

- Use `-RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` with real-profile readiness when exact-profile displayed undo-work evidence should be enforced before future live review.

Risky assumptions:

- The displayed/full-root distinction is clear enough in terminal output and docs.

### 2026-06-02: Real-Profile Readiness Default Scope Focus

Status: completed

Evidence:

- `tools\Invoke-RealProfileQuarantineReadiness.cmd` is specifically for future exact `C:\Users\moxhe` batch review, but previously required an explicit `-CleanupScope "C:\Users\moxhe"` argument to keep fixture manifests out of the displayed readiness focus.
- Exact real-profile undo-work evidence is currently distinct from full-root fixture undo-work evidence.

Implementation:

- Defaulted Restore Manifest display focus in `tools\Invoke-RealProfileQuarantineReadiness.ps1` to exact `C:\Users\moxhe`.
- Added `-AllCleanupScopes` to preserve the previous fixture-inclusive display path.
- Added an early ambiguous-argument guard when both `-CleanupScope` and `-AllCleanupScopes` are supplied.
- Printed the active Restore Manifest display focus near the top of the readiness output.
- Updated README, domain docs, glossary, live-product readiness roadmap, real-profile readiness feature brief, new feature brief, handoff, and this progress log.
- Did not launch WPF, click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change movement availability.

Verification:

- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight` passed; output showed `Restore Manifest display focus: C:\Users\moxhe`, exact real-profile daily display `(4 of 10)`, recovery-review focus `(2 of 10)`, undo-work focus `(0 of 10)`, and no WPF launch, scan, movement, restore, deletion, or cleanup history.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -AllCleanupScopes` passed; output showed `Restore Manifest display focus: all Cleanup Scopes` and fixture-inclusive undo-work focus `(2 of 10)`.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -CleanupScope "C:\Users\moxhe" -AllCleanupScopes` failed as expected with exit code 1 and `Choose either -CleanupScope or -AllCleanupScopes, not both.`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected CRLF conversion warnings.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/features/2026-06-02-real-profile-readiness-default-scope-focus.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is read-only readiness tooling polish over existing ADR 0017/0018/0019 boundaries.

Open questions:

- Whether a later WPF restore/history surface should group manifests by Cleanup Scope.

Follow-up work:

- Use default `Invoke-RealProfileQuarantineReadiness.cmd` before future exact real-profile batch review; use `-AllCleanupScopes` only when fixture manifests should also be displayed.

Risky assumptions:

- Exact `C:\Users\moxhe` is still the right default display focus for real-profile readiness.

### 2026-06-02: Restore Manifest Cleanup Scope Filter

Status: completed

Evidence:

- The default Quarantine Root includes both exact real-profile manifests and fixture manifests.
- Before any future exact real-profile batch, terminal evidence should be able to distinguish `C:\Users\moxhe` recovery-review or undo-work state from fixture undo-work.

Implementation:

- Added `-CleanupScope` to `tools\Summarize-RestoreManifests.ps1` as a display filter.
- Forwarded `-CleanupScope` through `tools\Invoke-DailyLocalReadiness.ps1` and `tools\Invoke-RealProfileQuarantineReadiness.ps1`.
- Kept full-root aggregate counts visible while filtering the displayed manifest list.
- Updated README, domain docs, glossary, Restore Manifest Summary feature brief, live-product readiness roadmap, handoff, the new feature brief, and this progress log.
- Did not launch WPF, click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change movement availability.

Verification:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries` passed; the display filter showed the two exact real-profile recovery-review manifests with failed DXCache entries.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly` passed; the display filter showed zero exact real-profile undo-work manifests while full-root aggregates still reported fixture undo-work.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly` passed; accepted package evidence printed in read-only/print-only mode with the expected docs-only commit warning.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -CleanupScope "C:\Users\moxhe"` passed; no WPF launch, real-profile scan, movement, restore, deletion, or cleanup history occurred.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected CRLF conversion warnings.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-restore-manifest-summary-tool.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-restore-manifest-cleanup-scope-filter.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is read-only terminal evidence for existing Restore Manifest Summary behavior, with no architecture, persistence, cleanup execution, restore rule, data-model, deployment, or security change.

Open questions:

- Whether a later dedicated restore/history surface should group manifests by Cleanup Scope in WPF.

Follow-up work:

- Use the scope filter when exact real-profile Restore Manifest evidence should be separated from fixture evidence before future live cleanup review.

Risky assumptions:

- Full-root aggregate counts plus display-filter wording are enough to avoid treating filtered output as cleanup clearance.

### 2026-06-02: Real-Profile Quarantine Readiness Review

Status: completed

Evidence:

- The first live exact real-profile Quarantine and selected restore recovery loop succeeded by user report, but any future batch still needs fresh preflight, accepted-package, Restore Manifest, WPF readiness, exact confirmation, and explicit user approval.
- Existing evidence commands were safe but split across MVP preflight, daily local readiness, and focused Restore Manifest summaries.

Implementation:

- Added `tools\Invoke-RealProfileQuarantineReadiness.ps1` and `.cmd`.
- The command runs full MVP preflight by default, then daily local readiness, recovery-review Restore Manifest focus, and undo-work Restore Manifest focus.
- Added `-SkipMvpPreflight` only for fast smoke checks, with output warning that skipped preflight is not fresh movement evidence.
- Forwarded accepted-notes path, Quarantine Root, Restore Manifest entry display, and strict Restore Manifest evidence flags.
- Updated README, domain docs, glossary, live-product readiness roadmap, handoff, the new feature brief, and this progress log.
- Did not launch WPF, click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, widen real-profile movement, create shortcuts, or install anything.

Verification:

- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight` passed. It verified accepted package notes, ran one accepted package verifier pass, printed accepted normal and fixture launch commands, summarized the default Restore Manifest root as 10 valid manifests / 13 entries / 8 restored / 3 moved / 2 failed / 2 undo-work / 2 recovery-review, then printed focused recovery-review and undo-work summaries. It did not launch WPF, scan the real profile, move, restore, delete, approve cleanup, or create cleanup history.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAnyRestoreManifest -RequireNoRecoveryReview -RequireNoUndoWork` passed, proving explicit root and strict Restore Manifest forwarding on a repo-local smoke root with 1 restored manifest and no recovery/undo work.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd` passed. Full MVP preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, printed the fixture checklist, and ran whitespace diff checking before the daily and focused Restore Manifest evidence steps.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is read-only local tooling over existing ADR 0017/0018/0019 gates, with no architecture, persistence, cleanup execution, restore rule, data-model, deployment, or security change.

Open questions:

- Whether a later installed shortcut or installer should exist.
- Whether permanent deletion, persisted cleanup history, or all-manifest restore should ever be added.

Follow-up work:

- Run the default readiness review before considering another tiny exact real-profile Quarantine batch, then still require WPF readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, immediate Pre-Execution Revalidation, and explicit user approval for the specific click.

Risky assumptions:

- Verbose terminal evidence is acceptable before future live cleanup.

### 2026-06-02: Daily Readiness Single Package Verification

Status: completed

Evidence:

- `.\tools\Invoke-DailyLocalReadiness.cmd` was safe but noisy because it delegated to the accepted local release launcher twice, causing the same accepted package verifier output to print twice in one run.
- Direct accepted-package launch commands should remain verified by default; duplicate verifier skipping should be narrow and explicit.

Implementation:

- Added `-SkipVerify` forwarding to `tools\Start-AcceptedLocalRelease.ps1`, matching existing `Start-LocalRelease.ps1` behavior.
- Added an accepted-launcher note when delegated package verification is skipped, warning to use it only after package verification already passed in the same local readiness or acceptance flow.
- Updated `tools\Invoke-DailyLocalReadiness.ps1` so the normal accepted print command verifies the package and the fixture print command skips duplicate verification in that same wrapper run.
- Updated README, domain docs, glossary, live-product readiness roadmap, handoff, the new feature brief, and this progress log.
- Did not create shortcuts, install anything, launch WPF, click `Scan`, scan, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change app movement behavior.

Verification:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd` passed. The output printed exactly one `Portable v1 release verifier` heading, then printed the fixture command with `Delegated package verification: skipped by request`; no WPF app was launched and no scan, movement, restore, delete, approval, or cleanup history occurred.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RecoveryReviewOnly -ShowRestoreEntries` passed and preserved recovery-review filtering, showing the 2 older failed real-profile `DXCache` recovery-review manifests.
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly -SkipVerify` passed and printed the skip-verification boundary plus the fixture launch command without launching WPF.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-daily-readiness-single-package-verification.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is read-only local tooling polish over existing package verification and print-only launcher behavior.

Open questions:

- Whether a later installed shortcut or installer should target the accepted package launcher, a release-local script, or a future installer flow.

Follow-up work:

- Keep direct `Start-AcceptedLocalRelease.cmd` use verified by default.
- Keep shortcut/installer automation as a later explicit user-approved packaging packet.

Risky assumptions:

- One package verifier run is enough for a single daily readiness wrapper invocation that immediately prints both accepted launch commands.

### 2026-06-02: Daily Local Readiness Check

Status: completed

Evidence:

- Accepted portable package daily use was safe but split across accepted-notes summary, accepted normal print-only launch, accepted fixture print-only launch, and Restore Manifest Summary commands.
- Shortcut/installer automation was considered too broad for this packet because it would create installed/profile artifacts and needs explicit user approval.
- The accepted package remains `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`, with completed ignored acceptance notes at `.local\release-acceptance\release-acceptance-20260602-011743.md`; package/current-HEAD commit warnings are expected after newer docs/tooling commits.

Implementation:

- Added `tools\Invoke-DailyLocalReadiness.ps1` and `.cmd`.
- The command verifies completed accepted package notes, prints accepted normal and fixture launch commands, and prints Restore Manifest Summary output in one read-only sequence.
- Forwarded optional Restore Manifest display and strictness flags: `-ShowRestoreEntries`, `-RecoveryReviewOnly`, `-UndoWorkOnly`, `-RequireNoRecoveryReview`, `-RequireNoUndoWork`, `-RequireAnyRestoreManifest`, and explicit `-QuarantineRoot`.
- Updated README, domain docs, glossary, live-product readiness roadmap, handoff, the new feature brief, and this progress log.
- Did not create shortcuts, install anything, launch WPF, click `Scan`, scan, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change app movement behavior.

Verification:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd` passed. It verified accepted package notes, printed accepted normal/fixture launch commands, and summarized the default Restore Manifest root as 10 valid manifests, 13 entries, 8 restored entries, 3 moved entries, 2 failed entries, 2 undo-work manifests, and 2 recovery-review manifests.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RecoveryReviewOnly -ShowRestoreEntries` passed and displayed the 2 recovery-review manifests, both older failed real-profile `DXCache` attempts, with entry-level error evidence.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -UndoWorkOnly -ShowRestoreEntries` passed and displayed the 2 undo-work fixture manifests with 3 moved entries.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAnyRestoreManifest -RequireNoRecoveryReview -RequireNoUndoWork` passed, proving explicit root and strict Restore Manifest forwarding on a repo-local smoke root with 1 restored manifest and no recovery/undo work.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-daily-local-readiness-check.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is read-only local tooling that composes existing accepted package and Restore Manifest evidence; it does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

Open questions:

- Whether a later installed shortcut or installer should target the accepted package launcher, a release-local script, or a future installer flow.

Follow-up work:

- Use `.\tools\Invoke-DailyLocalReadiness.cmd` when one compact daily accepted-package and Restore Manifest evidence check is useful.
- Keep shortcut/installer automation as a later explicit user-approved packaging packet.

Risky assumptions:

- Verbose verifier output is acceptable for a safety-first daily readiness command.

### 2026-06-02: Restore Manifest Undo Work Filter

Status: completed

Evidence:

- `.\tools\Summarize-RestoreManifests.cmd` already reports undo-work counts, but the default `D:\WindowsFileCleanerQuarantine` root has multiple historical manifests, making outstanding moved entries harder to inspect quickly.
- The first-live real-profile manifest is restored by user report and terminal summary evidence, while two older fixture manifests still have moved entries useful for recovery/undo visibility.

Implementation:

- Added `-UndoWorkOnly` to display only valid manifests with moved entries while keeping aggregate counts for the full selected Quarantine Root.
- Added `-RequireNoUndoWork` to exit non-zero when any valid manifest still has undo work.
- Kept `-UndoWorkOnly` and `-RecoveryReviewOnly` composable as an intersection display filter.
- Updated README, domain docs, glossary, Restore Manifest Summary feature brief, live-product readiness roadmap, handoff, and this progress log.
- Did not launch WPF, scan, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change selected restore gates.

Verification:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAny -RequireNoRecoveryReview -RequireNoUndoWork`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -UndoWorkOnly -ShowEntries` reported 10 valid manifests, 2 matching the undo-work display filter, and showed two fixture manifests with 3 moved entries.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -RecoveryReviewOnly -UndoWorkOnly` reported zero manifests matching both filters while preserving full-root aggregate counts.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -RecoveryReviewOnly -ShowEntries` still reported 2 recovery-review manifests with the older failed real-profile `DXCache` error evidence.
- Expected-failure check for `-RequireNoUndoWork` against the default root returned exit code 1 while 2 undo-work manifests remain.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-restore-manifest-summary-tool.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-restore-manifest-undo-work-filter.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is read-only terminal evidence for existing Restore Manifest Summary behavior and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

Open questions:

- Whether a later dedicated restore/history surface should group outstanding undo work in WPF.

Follow-up work:

- Use `-UndoWorkOnly -ShowEntries` when inspecting moved-entry evidence before future live cleanup batches.
- Keep actual recovery selected-manifest-only through WPF gates unless a later ADR expands restore/history behavior.

Risky assumptions:

- Outstanding moved fixture entries should remain visible as undo-work evidence rather than being hidden or auto-restored.

### 2026-06-02: Restore Manifest Recovery Review Filter

Status: completed

Evidence:

- `.\tools\Summarize-RestoreManifests.cmd` already reports recovery-review counts, but the default `D:\WindowsFileCleanerQuarantine` root has multiple historical manifests, making recovery-review debt harder to inspect quickly.
- The first-live real-profile manifest is restored by user report and terminal summary evidence, while two older failed real-profile attempts remain useful recovery-review evidence.

Implementation:

- Added `-RecoveryReviewOnly` to display only valid manifests that need recovery review while keeping aggregate counts for the full selected Quarantine Root.
- Added `-RequireNoRecoveryReview` to exit non-zero when any valid manifest still needs recovery review.
- Updated README, domain docs, glossary, Restore Manifest Summary feature brief, live-product readiness roadmap, handoff, and this progress log.
- Did not launch WPF, scan, move, restore, delete, write Restore Manifests, approve cleanup, create cleanup history, add broad/all-manifest restore, or change selected restore gates.

Verification:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAny -RequireNoRecoveryReview`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -RecoveryReviewOnly -ShowEntries` reported 10 valid manifests, 2 matching the recovery-review display filter, and showed both older failed real-profile `DXCache` attempts with entry-level error evidence.
- Expected-failure check for `-RequireNoRecoveryReview` against the default root returned exit code 1 while 2 recovery-review manifests remain.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-restore-manifest-summary-tool.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-restore-manifest-recovery-review-filter.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is read-only terminal evidence for existing Restore Manifest Summary behavior and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

Open questions:

- Whether a later dedicated restore/history surface should group recovery-review manifests in WPF.

Follow-up work:

- Use `-RecoveryReviewOnly -ShowEntries` when inspecting older failed manifests before future live cleanup batches.
- Keep actual recovery selected-manifest-only through WPF gates unless a later ADR expands restore/history behavior.

Risky assumptions:

- The failed older real-profile attempts should remain visible as recovery-review evidence rather than being hidden or auto-cleaned.

### 2026-06-02: Accepted Package Daily Use Guide

Status: completed

Evidence:

- The accepted package launcher and Restore Manifest summary helper are the safest current daily-use/recovery-evidence tools, but their README details were spread across longer release and restore sections.
- The user asked the next thread to prefer accepted-package daily-use guidance, Restore Manifest summary/recovery evidence, or similarly small safe packets.

Implementation:

- Added a top-level README Daily Local Use section with accepted package print commands, accepted evidence check, read-only Restore Manifest summary, and the real-profile movement stop boundary.
- Added `docs/features/2026-06-02-accepted-package-daily-use-guide.md`.
- Updated the live-product readiness roadmap and fresh-thread handoff to name the daily-use guide.
- Did not change app behavior, package artifacts, ignored acceptance notes, movement gates, Restore Manifest format, cleanup history, or release tooling.

Verification:

- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd` reported 10 valid manifests, 13 entries, 8 restored entries, 3 still moved fixture entries, 2 failed real-profile attempts needing recovery review, and the first-live real-profile manifest restored with no undo work.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-accepted-package-daily-use-guide.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is docs-only daily-use guidance for existing accepted local tooling and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

Open questions:

- Whether installed shortcut or installer automation is worth a separate packaging packet later.

Follow-up work:

- Cut and accept a fresh package only after future behavior changes should ship as the next accepted app package.
- Use Restore Manifest summary as read-only terminal evidence when compact recovery state is useful.

Risky assumptions:

- The accepted local package remains the right daily-use baseline until the user intentionally cuts and accepts a fresh package.

### 2026-06-02: Fresh Thread Handoff Polish After Accepted Local Release Launcher

Status: completed

Evidence:

- The thread is getting slow, and the user intends to archive it and start a new one.
- The current accepted package baseline, safety boundary, and best next work were already present, but the handoff docs needed to name the handoff-polish packet itself so a new thread starts from the latest committed state.

Implementation:

- Updated the fresh-thread handoff packet name and startup prompt.
- Added this compact progress entry for the docs-only handoff pass.
- Kept README, domain docs, glossary, feature briefs, ADRs, code, release artifacts, and ignored `.local` acceptance notes unchanged because they were already current for this pass.

Verification:

- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is docs-only handoff polish and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

Open questions:

- None for the handoff pass.

Follow-up work:

- Start the next thread with the refreshed startup prompt in `docs/codex/thread-handoff.md`.
- Use the accepted package launcher or pick the next small product packet after inspecting git status.

Risky assumptions:

- The accepted local package remains the right daily-use baseline until the user intentionally cuts and accepts a fresh package.

### 2026-06-02: Accepted Local Release Launcher

Status: completed

Evidence:

- The accepted package baseline is useful for daily local review, but after docs-only commits the package commit can intentionally differ from current `HEAD`.
- `Start-LocalRelease.cmd -RequireCurrentCommit` remains right for current-HEAD packages, while accepted-package daily use needs to resolve from completed ignored acceptance notes.

Implementation:

- Added `tools\Start-AcceptedLocalRelease.ps1` and `.cmd`.
- The launcher defaults to the latest ignored release acceptance notes or accepts `-AcceptanceNotesPath`.
- It requires recorded verifier, commit, normal launch, fixture launch, acceptable overall result, and all checklist items marked `Pass`.
- It resolves the release folder from notes, requires it under ignored `.local`, then delegates to `Start-LocalRelease.ps1`.
- It supports `-PrintOnly`, `-Fixture`, and `-ChecklistOnly`.

Verification:

- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -ChecklistOnly`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-accepted-local-release-launcher.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local launch ergonomics for an already accepted Portable Release Package and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

Open questions:

- Whether installed shortcut or installer automation is desirable later.

Follow-up work:

- Use `Start-AcceptedLocalRelease.cmd -PrintOnly` when the user wants the accepted package command.
- Cut and accept a fresh package after future code changes that should ship as the next accepted app package.

Risky assumptions:

- The latest completed ignored acceptance notes are the right default for daily local package use.

### 2026-06-02: Portable v1 Acceptance Baseline

Status: completed

Evidence:

- User confirmed the portable package acceptance checks were completed successfully.
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` reported `.local\release-acceptance\release-acceptance-20260602-011743.md`, package `.local\releases\windows-file-cleaner-v20260602-011556`, commit `bc9b869`, clean notes-creation worktree, verifier/current-commit/normal-launch/fixture-launch evidence recorded, overall result `Pass`, and `6 pass, 0 issue, 0 not checked, 0 not recorded`.

Implementation:

- Recorded the accepted local package baseline in README, the live-product readiness roadmap, handoff docs, and this progress log.
- Added `docs/features/2026-06-02-portable-v1-acceptance-baseline.md`.
- Kept the ignored package and ignored notes as local artifacts; no release output was copied into tracked files.

Verification:

- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Test-LocalRelease.cmd -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-portable-v1-acceptance-baseline.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This records local package acceptance evidence for the existing Portable Release Package boundary and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

Open questions:

- Whether installed shortcut or installer automation is desirable later.

Follow-up work:

- Use the accepted portable package for local daily review.
- Cut and accept a fresh package after future code changes that should ship.

Risky assumptions:

- Ignored local acceptance notes are sufficient evidence for the current single-user portable v1 baseline.

### 2026-06-02: Local Release Acceptance Notes Recorder

Status: completed

Evidence:

- Clean portable release acceptance notes can be generated and summarized, but completing them still required hand-editing ignored markdown after the user finished the manual package pass.
- The package acceptance evidence should remain local and ignored, not app persistence or cleanup history.

Implementation:

- Added `tools\Record-LocalReleaseAcceptanceNotes.ps1` and `.cmd`.
- The recorder defaults to the latest ignored package acceptance notes or accepts `-Path`.
- It requires `-RecordManualAcceptance` and refuses paths outside ignored `.local`.
- It marks manual launch evidence, checklist items 2-6, the overall result, and summary text after a human package acceptance pass.
- It prints exact summary and completion-check commands and states that it does not launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history.

Verification:

- `cmd.exe /c tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes`
- `cmd.exe /c tools\Record-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-recording-test.md" -RecordManualAcceptance -Summary "Test-only recorded acceptance for recorder verification."`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance-recording-test\release-acceptance-recording-test.md" -RequireComplete`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-local-release-acceptance-notes.md`
- `docs/features/2026-06-02-local-release-acceptance-notes-recorder.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local ignored evidence-note recording, with no installer, app persistence, cleanup execution, restore scope, data-model, or security-boundary change.

Open questions:

- Whether installed shortcut or installer automation is desirable later.

Follow-up work:

- Use the recorder only after a human completes the package acceptance pass.
- Keep permanent deletion, cleanup history, broad restore, and installer/shortcut automation as separate later decisions.

Risky assumptions:

- A terminal recorder reduces local evidence friction without weakening the human acceptance requirement because `-RecordManualAcceptance` is explicit and no app action is performed.

### 2026-06-02: Local Release Acceptance Notes Launch Commands

Status: completed

Evidence:

- Portable release acceptance notes stamped package paths and scripts, but the exact normal and fixture launch commands still had to be recovered from launcher output or release-local scripts during manual acceptance.
- The summary helper could show evidence state, but not the commands the user needed for the acceptance pass.

Implementation:

- `tools\Start-LocalRelease.ps1` now builds normal and fixture launch commands for checklist-only and notes output.
- Checklist-only output prints exact normal and fixture launch commands without launching WPF.
- Generated ignored release acceptance notes include `Normal launch command` and `Fixture launch command` metadata.
- `tools\Summarize-LocalReleaseAcceptanceNotes.ps1` prints those launch command lines from ignored notes.

Verification:

- `cmd.exe /c tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` returned non-zero as expected because manual launch and fixture evidence remain open.
- `cmd.exe /c tools\Start-LocalRelease.cmd -Fixture -ChecklistOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-local-release-acceptance-notes.md`
- `docs/features/2026-06-02-local-release-acceptance-notes-evidence-prefill.md`
- `docs/features/2026-06-02-local-release-acceptance-notes-launch-commands.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local package acceptance-note ergonomics, with no installer, persistence, cleanup execution, restore scope, data-model, or security-boundary change.

Open questions:

- Whether installed shortcut or installer automation is desirable later.

Follow-up work:

- After committing this packet, cut a fresh ignored portable package so the latest package commit again matches current `HEAD`, then generate clean-worktree acceptance notes for the next human package acceptance pass.

Risky assumptions:

- Printing exact launch commands in ignored notes and summary output makes acceptance easier without implying that either launch command has been run.

### 2026-06-02: Local Release Acceptance Notes Evidence Prefill

Status: completed

Evidence:

- The notes-enabled portable release checklist verifies the package before writing notes, but the generated notes still left verifier/current-commit evidence unchecked.
- The command can objectively prove verifier success and current-commit match when `-RequireCurrentCommit` is used, while launch and fixture scan results still require human review.

Implementation:

- `tools\Start-LocalRelease.ps1` now marks verifier evidence recorded when verification was not skipped and completed successfully.
- It marks current-commit evidence recorded when `-RequireCurrentCommit` was supplied and verification completed successfully.
- Checklist item 1 is marked `Pass` with an automatic note when verification completed successfully.
- Normal launch and fixture launch/read-only-scan evidence remain manual and unrecorded in generated notes.

Verification:

- `cmd.exe /c tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` returned non-zero as expected because manual launch and fixture evidence remain open.
- `cmd.exe /c tools\Start-LocalRelease.cmd -Fixture -ChecklistOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-local-release-acceptance-notes.md`
- `docs/features/2026-06-02-local-release-acceptance-notes-evidence-prefill.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local package acceptance notes evidence ergonomics, with no installer, persistence, cleanup execution, restore scope, data-model, or security-boundary change.

Open questions:

- Whether installed shortcut or installer automation is desirable later.

Follow-up work:

- After committing this packet, cut a fresh ignored portable package so the latest package commit again matches current `HEAD`, then generate clean-worktree acceptance notes for the next human package acceptance pass.

Risky assumptions:

- Pre-recording only objective verifier/current-commit evidence improves notes usefulness without making acceptance look complete.

### 2026-06-02: Local Release Acceptance Notes

Status: completed

Evidence:

- Portable release checklist-only output made package acceptance repeatable, but the result of a manual package acceptance pass was still conversational unless copied into docs.
- The fixture review workflow already uses ignored local notes plus a read-only summary helper; the portable release path benefits from the same pattern without becoming app persistence or cleanup history.

Implementation:

- `tools\Start-LocalRelease.ps1` now supports `-ChecklistOnly -WriteAcceptanceNotes`.
- Generated notes live under ignored `.local\release-acceptance` and stamp repo branch/commit, worktree status at notes creation, release metadata commit, publish worktree/preflight evidence, package paths, evidence checkboxes, safety boundary text, and exact post-pass summary commands.
- Later packet `Local Release Acceptance Notes Evidence Prefill` made generated notes pre-record verifier evidence after `Test-LocalRelease` passes and pre-record current-commit evidence when `-RequireCurrentCommit` was used; launch and fixture scan evidence remain manual.
- Added `tools\Summarize-LocalReleaseAcceptanceNotes.ps1` and `.cmd`.
- The summary helper defaults to the latest ignored package acceptance notes or accepts `-Path`.
- `-RequireComplete` fails until verifier evidence, commit evidence, normal launch evidence, fixture launch/read-only-scan evidence, an acceptable overall result, and all checklist items are recorded.

Verification:

- `cmd.exe /c tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` returned non-zero as expected for an unfilled notes template.
- `cmd.exe /c tools\Start-LocalRelease.cmd -Fixture -ChecklistOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-01-local-release-launcher.md`
- `docs/features/2026-06-02-local-release-acceptance-checklist.md`
- `docs/features/2026-06-02-local-release-acceptance-notes.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local package acceptance evidence tooling, with no installer, persistence, cleanup execution, restore scope, data-model, or security-boundary change.

Open questions:

- Whether installed shortcut or installer automation is desirable later.

Follow-up work:

- After committing this packet, cut a fresh ignored portable package so the latest package commit again matches current `HEAD`.

Risky assumptions:

- Ignored markdown notes are enough local package acceptance evidence for v1.

### 2026-06-02: Local Release Acceptance Checklist

Status: completed

Evidence:

- Portable packages could be verified, printed, or launched from the repo root, but package-level acceptance steps still lived in conversational instructions.
- A checklist-only mode gives a repeatable terminal acceptance path without launching WPF or scanning.

Implementation:

- `tools\Start-LocalRelease.ps1` now supports `-ChecklistOnly`.
- Checklist-only mode verifies the package by default, then prints package-local README, launch scripts, executable path, fixture scope, normal launch review, fixture launch review, portable/no-installer boundaries, and the stop-before-real-profile-movement boundary.
- Checklist-only mode exits before WPF launch and states that it did not launch WPF, click `Scan`, move, restore, delete, approve cleanup, or create cleanup history.

Verification:

- `cmd.exe /c tools\Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-LocalRelease.cmd -Fixture -ChecklistOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-local-release-launcher.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-local-release-acceptance-checklist.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is portable release acceptance ergonomics, with no installer, persistence, cleanup execution, restore scope, data-model, or security-boundary change.

Open questions:

- Whether installed shortcut or installer automation is desirable later.

Follow-up work:

- After committing this packet, cut a fresh ignored portable package so the latest package commit again matches current `HEAD`.

Risky assumptions:

- Printing acceptance steps is useful enough for the local v1 package without making release artifacts feel installed.

### 2026-06-02: Portable Release Checksum Evidence

Status: completed

Evidence:

- Portable package verification checked structure, metadata, launch scripts, README, zip entries, and commit evidence, but not local file integrity evidence.
- SHA-256 metadata and sidecars improve package confidence without adding signing infrastructure, installer behavior, or cleanup history.

Implementation:

- `tools\Publish-LocalRelease.ps1` records the packaged `WindowsFileCleaner.App.exe` SHA-256 in `release-metadata.txt`.
- `tools\Publish-LocalRelease.ps1` writes a sibling `.zip.sha256` sidecar for the release zip after the zip is created.
- `tools\Test-LocalRelease.ps1` verifies the zip checksum sidecar exists.
- `tools\Test-LocalRelease.ps1` recomputes the packaged executable SHA-256 and compares it with metadata.
- `tools\Test-LocalRelease.ps1` recomputes the release zip SHA-256 and compares it with the sidecar.

Verification:

- `cmd.exe /c tools\Publish-LocalRelease.cmd -SkipPreflight`
- `cmd.exe /c tools\Test-LocalRelease.cmd -AllowDirtyPublish -AllowSkippedPreflight`
- `cmd.exe /c tools\Start-LocalRelease.cmd -PrintOnly -AllowDirtyPublish -AllowSkippedPreflight`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-portable-v1-release-packaging.md`
- `docs/features/2026-06-01-portable-release-verifier.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-portable-release-checksum-evidence.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local release integrity evidence for ignored Portable Release Package artifacts, with no installer, signing infrastructure, persistence, cleanup execution, restore scope, data-model, or security-boundary change.

Open questions:

- Whether code signing or external distribution will ever be needed.

Follow-up work:

- After committing this packet, cut a fresh ignored portable package so the latest package includes checksum evidence and matches current `HEAD`.

Risky assumptions:

- SHA-256 evidence is useful for local package sanity even without code signing.

### 2026-06-02: Local Release Launcher Start-Here Output

Status: completed

Evidence:

- Portable packages include `README-FIRST.txt` and release-local launch scripts, but the repo-level launcher still printed only the executable launch command.
- Surfacing package-local paths helps daily use without creating installed shortcuts.

Implementation:

- `tools\Start-LocalRelease.ps1` now computes the latest package's `README-FIRST.txt`, normal launch script, and fixture launch script paths.
- Normal print/start output shows the start-here README path and normal release-local launch script path.
- Fixture print/start output shows the start-here README path and fixture release-local launch script path.
- The launcher still verifies by default before printing or launching.

Verification:

- `cmd.exe /c tools\Start-LocalRelease.cmd -PrintOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-LocalRelease.cmd -Fixture -PrintOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-local-release-launcher.md`
- `docs/features/2026-06-02-portable-release-start-here-readme.md`
- `docs/features/2026-06-02-local-release-launcher-start-here-output.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is portable release ergonomics and terminal output wording, with no installer, persistence, cleanup execution, restore scope, data-model, or security-boundary change.

Open questions:

- Whether installed shortcut or installer automation is desirable later.

Follow-up work:

- After committing this packet, cut a fresh ignored portable package so the latest package commit again matches current `HEAD`.

Risky assumptions:

- Printing package-local artifact paths makes the portable folder easier to use without making the package feel installed.

### 2026-06-02: Portable Release Start Here Readme

Status: completed

Evidence:

- Portable packages already had release-local launch scripts, but an unzipped folder still benefited from a plain text start-here file that travels with the artifact.
- The user confirmed the remaining release verification steps 7 and 9 before this packet.

Implementation:

- `tools\Publish-LocalRelease.ps1` writes `README-FIRST.txt` beside `release-metadata.txt`.
- The README names normal launch, fixture launch, and direct executable launch.
- The README keeps fixture launch scoped to prefilled Cleanup Scope only, with no fixture creation and no automatic Scan click.
- The README repeats that portable v1 is not an installer and excludes permanent deletion, persisted cleanup history, broad/all-manifest restore, custom real-profile Quarantine, and non-exact real-profile movement.
- Release metadata records the README path.
- `tools\Test-LocalRelease.ps1` verifies the README exists, metadata path matches, key safety-boundary lines are present, and the zip includes it.

Verification:

- `cmd.exe /c tools\Publish-LocalRelease.cmd -SkipPreflight`
- `cmd.exe /c tools\Test-LocalRelease.cmd -AllowDirtyPublish -AllowSkippedPreflight`
- `cmd.exe /c tools\Start-LocalRelease.cmd -PrintOnly -AllowDirtyPublish -AllowSkippedPreflight`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-02-portable-release-start-here-readme.md`
- `docs/features/2026-06-01-portable-v1-release-packaging.md`
- `docs/features/2026-06-01-portable-release-verifier.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is package usability text and verifier coverage for the existing Portable Release Package, with no installer, persistence, cleanup execution, restore scope, data-model, or security-boundary change.

Open questions:

- Whether installed shortcut or installer automation is desirable later.

Follow-up work:

- After committing this packet, cut a fresh ignored portable package so the latest package includes `README-FIRST.txt` and matches current `HEAD`.

Risky assumptions:

- A plain text start-here file is enough package-local guidance for v1.

### 2026-06-01: Local Release Launcher

Status: completed

Evidence:

- Fresh strict portable packages can be verified from the terminal, but daily launch still required browsing into `.local\releases` or copying a package-specific command.
- A repo-level launcher improves local use without creating an installer or shortcut.

Implementation:

- Added `tools\Start-LocalRelease.ps1` and `.cmd`.
- The launcher resolves the latest or explicit ignored Portable Release Package, verifies it through `Test-LocalRelease.ps1` by default, then prints or starts the packaged WPF app.
- `-PrintOnly` prints the normal or fixture launch command without launching WPF.
- `-Fixture` passes the repo-local smoke fixture Cleanup Scope and states that fixture launch only prefills the Cleanup Scope; it does not create fixtures or click `Scan`.
- `-RequireCurrentCommit`, `-AllowDirtyPublish`, and `-AllowSkippedPreflight` pass through to package verification.

Verification:

- Initial print-only test found and fixed the verifier argument-splat bug without launching WPF.
- `cmd.exe /c tools\Start-LocalRelease.cmd -PrintOnly -RequireCurrentCommit`
- `cmd.exe /c tools\Start-LocalRelease.cmd -PrintOnly -Fixture -RequireCurrentCommit`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-local-release-launcher.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is portable release ergonomics, with no installer, persistence, cleanup execution, restore scope, or security-boundary change.

Open questions:

- Whether installed shortcut or installer automation is desirable later.

Follow-up work:

- After committing this packet, cut a fresh ignored portable package so the latest package commit again matches current `HEAD`.

Risky assumptions:

- A repo-level launcher is useful despite release-local launch scripts already existing in each package.

### 2026-06-01: Restore Manifest Summary Tool

Status: completed

Evidence:

- The first-live selected restore recovery loop succeeded by user report, but terminal-side recovery evidence still required reading dense WPF text or raw JSON.
- The user reported the remaining requested release verification steps 7 and 9 were all confirmed before this packet.

Implementation:

- Added `tools\Summarize-RestoreManifests.ps1` and `.cmd`.
- The helper reads `actions\*\restore-manifest.json` under a selected Quarantine Root, validates the action-scoped layout, and reports manifest count, discovery issues, total entries/size, entry status counts, cleanup scopes, undo-work count, recovery-review count, per-manifest status/counts/size/path, and optional entry-level paths/errors.
- The helper repeats the selected-only/no-all-manifest restore boundary and does not launch WPF, scan, move, restore, delete, write manifests, approve cleanup, or create cleanup history.

Verification:

- Created an ignored synthetic Restore Manifest under `.local\restore-manifest-summary-smoke`.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAny -ShowEntries`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-restore-manifest-summary-tool.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0003, ADR 0011, ADR 0012, and ADR 0019 already cover JSON Restore Manifest, read-only discovery/readiness, and selected-only real-profile restore boundaries.

Open questions:

- Whether a future richer cleanup history should exist after Restore Manifest-only recovery remains trusted.

Follow-up work:

- Use the helper after future user-clicked Quarantine or selected restore work when compact Restore Manifest evidence is useful.
- Keep broad/all-manifest restore, permanent deletion, and persisted cleanup history as separate future decisions.

Risky assumptions:

- PowerShell summary validation is sufficient for local diagnostic evidence and does not need a new console project.

### 2026-06-01: Portable Release Launch Scripts

Status: completed

Evidence:

- Portable release package verification exists, but normal/fixture launch still depended on copying terminal commands or browsing directly to the executable.
- Release-local launch scripts reduce local-use friction without creating installed shortcuts or changing cleanup behavior.

Implementation:

- `tools\Publish-LocalRelease.ps1` writes `Launch-WindowsFileCleaner.cmd` and `Launch-WindowsFileCleaner-Fixture.cmd` beside `release-metadata.txt`.
- Release metadata records both launch script paths and the fixture Cleanup Scope.
- The release zip includes both launch scripts.
- `tools\Test-LocalRelease.ps1` verifies launch script existence, metadata paths, relative packaged-exe launch wording, fixture-scope launch wording, and zip entries.

Verification:

- `cmd.exe /c tools\Publish-LocalRelease.cmd -SkipPreflight`
- `cmd.exe /c tools\Test-LocalRelease.cmd -AllowDirtyPublish -AllowSkippedPreflight`
- `cmd.exe /c tools\Test-LocalRelease.cmd` failed as expected against the focused dirty/skipped-preflight package because normal verification requires clean publish metadata and preflight.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`
- After commit `fcc54da`, `cmd.exe /c tools\Publish-LocalRelease.cmd` passed full MVP preflight and created `.local\releases\windows-file-cleaner-v20260601-233542` from a clean worktree.
- `cmd.exe /c tools\Test-LocalRelease.cmd -RequireCurrentCommit` passed against `.local\releases\windows-file-cleaner-v20260601-233542`.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-portable-release-launch-scripts.md`
- `docs/features/2026-06-01-portable-release-verifier.md`
- `docs/features/2026-06-01-portable-v1-release-packaging.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is packaging ergonomics for the existing Portable Release Package, with no architecture, persistence, cleanup execution, restore scope, or security-boundary change.

Open questions:

- Whether installed desktop shortcut automation is worth adding later.

Follow-up work:

- Use `.local\releases\windows-file-cleaner-v20260601-233542` as the latest strict local package.

Risky assumptions:

- Release-local `.cmd` scripts are enough to reduce launch friction without creating installer-like behavior.

### 2026-06-01: Portable Release Verifier

Status: completed

Evidence:

- Portable v1 packaging exists and has user-reported fixture launch/scan verification.
- Later docs-only commits can make the latest local package commit differ from current `HEAD`, so a read-only verifier should distinguish warning-level drift from a strict current-commit release check.

Implementation:

- Added `tools\Test-LocalRelease.ps1` and `.cmd`.
- The verifier checks the latest or explicit ignored local release folder, stamp-format name, executable, metadata, sibling zip, metadata branch/runtime/self-contained/preflight/worktree fields, executable and zip path metadata, safety-boundary lines, required zip entries, and package commit.
- Commit mismatch is a warning by default and a failure with `-RequireCurrentCommit`.
- The verifier states that it reads ignored local release files only and does not launch WPF, scan, move, restore, delete, or create cleanup history.

Verification:

- `cmd.exe /c tools\Test-LocalRelease.cmd`
- `cmd.exe /c tools\Test-LocalRelease.cmd -RequireCurrentCommit` failed as expected because the latest package was created at `5c2142d` and current `HEAD` was `f3a0acb`.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-portable-release-verifier.md`
- `docs/features/2026-06-01-portable-v1-release-packaging.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local verification tooling for an existing Portable Release Package, with no architecture, persistence, cleanup execution, restore scope, or security-boundary change.

Open questions:

- Whether ignored per-release launch scripts are worth adding later.

Follow-up work:

- Run `.\tools\Test-LocalRelease.cmd -RequireCurrentCommit` after cutting a package that must exactly match current `HEAD`.

Risky assumptions:

- Metadata/zip/package verification adds useful package confidence even though it does not replace a human fixture UI launch pass.

### 2026-06-01: Portable v1 Release Packaging

Status: completed

Evidence:

- The user chose a self-contained portable folder/zip for v1, with a reversible-only boundary and no installer.
- The app already had full MVP preflight, fixture review tooling, and one trusted tiny real-profile Quarantine/selected-restore recovery loop.

Implementation:

- Added `tools\Publish-LocalRelease.ps1` and `.cmd`.
- The publisher prints git status, runs MVP preflight by default, publishes `WindowsFileCleaner.App` as `Release` / `win-x64` / self-contained to ignored `.local\releases\windows-file-cleaner-vYYYYMMDD-HHMMSS\app`, verifies the executable, writes `release-metadata.txt`, creates a zip beside the folder, and prints launch commands.
- The publisher checks for a running Debug `WindowsFileCleaner.App` process before preflight so a stale open app gets a clear close-and-rerun message instead of a long MSBuild file-lock failure.
- Added Portable Release Package domain/glossary wording and README release instructions.
- Updated the live-product roadmap and handoff to mark local portable v1 packaging available.

Verification:

- `cmd.exe /c tools\Publish-LocalRelease.cmd`
- Confirmed `.local\releases\...\app\WindowsFileCleaner.App.exe` exists.
- Confirmed `.local\releases\...\zip` exists.
- Confirmed the packaged executable can start against the fixture Cleanup Scope without scanning or moving files automatically.
- User reran the publisher from clean `main`, launched the packaged app with the printed fixture launch command, clicked fixture Scan, and confirmed the packaged app opened and the fixture scan completed normally.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-portable-v1-release-packaging.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0002 already selects WPF/.NET 8; this packet adds local publish tooling without changing architecture, persistence, cleanup execution, restore scope, or security boundaries.

Open questions:

- Whether to add desktop shortcut or installer automation later.

Follow-up work:

- Use the portable package for local v1 review.
- Keep deletion, persisted cleanup history, broad/all-manifest restore, and shortcut/installer automation as separate future decisions.

Risky assumptions:

- A self-contained `win-x64` portable folder/zip is enough for v1 on the project owner's Windows machine.

### 2026-06-01: User-Reported Selected Restore Rediscovery Rescan Confirmation

Status: completed

Evidence:

- After selected restore recovery succeeded with `Restored 1, failed 0`, the user completed the requested follow-up checks and answered yes to all three:
  - rediscovery showed the manifest as restored/already restored,
  - the rescan completed normally,
  - the restored `pip\cache\http\b\c` path appeared again.
- Codex did not click restore, quarantine, delete, or modify real-profile files.

Implementation:

- Recorded the user-reported evidence in README, feature briefs, progress, and handoff docs.
- No code or workflow behavior changed.

Verification:

- User-reported manual WPF evidence.
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-cross-volume-selected-restore-retry.md`
- `docs/features/2026-06-01-user-reported-first-real-profile-quarantine-success.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0019 already owns the selected real-profile restore contract.

Open questions:

- Whether to pause for packaging/formal acceptance or continue with another tiny exact real-profile Quarantine batch after fresh full MVP preflight.

Follow-up work:

- Start any further live cleanup work with fresh preflight and a newly reviewed tiny exact `C:\Users\moxhe` batch.

Risky assumptions:

- The user-reported WPF rediscovery, rescan, and restored-path observations are accepted as manual recovery-loop evidence.

### 2026-06-01: Quarantine Tab Restore Manifest Review Panel

Status: completed

Evidence:

- The user reported that `Discover manifests` was not in the Quarantine tab and had to be found in Main Grid.
- The app had split Quarantine and Main Grid into separate pages, but the manifest recovery surface still lived in the Main Grid detail pane.

Implementation:

- Added a Quarantine-tab `Restore Manifest Review` panel.
- Moved `Discover manifests`, all-manifest readiness, selected manifest selection/readiness, selected restore gate/result, and Restore Readiness output into that panel.
- Moved detailed Quarantine Preview output into the Quarantine tab.
- Added WPF smoke assertions that the Restore Manifest Review panel is expanded inside Quarantine and not inside Main Grid.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-quarantine-tab-restore-manifest-review-panel.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0016 already owns the discovered-manifest surface boundary; this packet changes placement only.

Open questions:

- Whether the Quarantine tab still feels roomy enough after adding the Restore Manifest Review panel below Quarantine Shortlist.

Follow-up work:

- Run a visual pass and confirm `Discover manifests` is immediately findable in Quarantine.

Risky assumptions:

- A vertically scrollable Quarantine tab is preferable to splitting recovery into another top-level tab.

### 2026-06-01: User-Reported Selected Restore Recovery Success

Status: completed

Evidence:

- After the cross-volume selected restore retry and highlighted gate evidence packets, the user retried selected restore for the first-live Restore Manifest.
- The user reported the highlighted selected-restore result: `Key status: selected restore succeeded. Restored 1, failed 0. Rediscover manifests and rescan.`
- Codex did not click restore or move/restore/delete real-profile files.

Implementation:

- Docs-only evidence update. No app behavior changed in this packet.

Verification:

- User-reported WPF result screenshot.
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-cross-volume-selected-restore-retry.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-01-user-reported-first-real-profile-quarantine-success.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0019 still governs selected-manifest exact real-profile restore.

Open questions:

- Whether the next live-product step should be another tiny real-profile Quarantine batch, packaging, or formal fixture acceptance notes.

Follow-up work:

- Rediscover manifests and rescan before further cleanup review.

Risky assumptions:

- User-reported screenshot is accepted as live recovery evidence for the one selected first-live manifest.

### 2026-06-01: Cross-Volume Selected Restore Retry and Gate Highlights

Status: completed

Evidence:

- The user attempted selected restore for the first live exact real-profile Quarantine manifest and reported `Restored 0 | Failed 1 | Recovery review: yes`.
- The failed row attempted to restore `D:\WindowsFileCleanerQuarantine\actions\quarantine-action-draft-20260601112432-ab98a4a0\items\AppData\Local\pip\cache\http\b\c` back to `C:\Users\moxhe\AppData\Local\pip\cache\http\b\c`.
- The error was the Windows cross-volume directory move limitation: source and destination roots must be identical for `Directory.Move`.
- The user also reported the dense gate text made it hard to find the relevant status.

Implementation:

- `UndoQuarantineExecutor` now routes directory restore through the guarded `QuarantineDirectoryMove` copy-then-delete fallback.
- Restore Manifest entries already marked `RestoreFailed` are narrowly retryable through selected restore when the quarantine path still exists and the original path is absent.
- WPF Quarantine and Selected Restore panes now show highlighted key-status strips above dense gate text, summarizing ready, blocker, result, and next-action states.
- User visual review confirmed the selected-restore ready strip was easier to see, but asked for the exact `Can execute: yes` / `Can proceed: yes` evidence to be highlighted too; the follow-up polish added those exact labels to the selected-restore highlight.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check` passed with only existing LF-to-CRLF working-copy warnings.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-cross-volume-selected-restore-retry.md`
- `docs/features/2026-06-01-real-profile-selected-restore-execution.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0019 still governs selected-manifest exact real-profile restore; this packet fixes directory restore and retryability inside that selected-manifest recovery path.

Open questions:

- Resolved by later user report: selected restore retry succeeded with `Restored 1, failed 0`.

Follow-up work:

- Rediscover manifests and rescan before further cleanup review.

Risky assumptions:

- Treating `RestoreFailed` entries as retryable is safe only when selected readiness confirms the quarantine path still exists, the original path is absent, and the selected manifest remains exact real-profile.

### 2026-06-01: User-Reported First Real-Profile Quarantine Success

Status: completed

Evidence:

- After the cross-volume directory fallback and in-use source revalidation packets, the user approved a different tiny exact real-profile batch and reported success from the WPF app.
- The reported batch moved one row from `C:\Users\moxhe\AppData\Local\pip\cache\http\b\c` into `D:\WindowsFileCleanerQuarantine\actions\quarantine-action-draft-20260601112432-ab98a4a0\items\AppData\Local\pip\cache\http\b\c`.
- The reported Restore Manifest was `restore-manifest-20260601112432-a715565e`, status `Completed`, entries `1`, bytes `13.97 MB`.
- The reported execution result was `moved 1, failed 0, blockers 0, recovery review: no`, with zero readiness blockers.
- Codex did not click the real-profile movement path or move/restore/delete real-profile files.

Implementation:

- Docs-only evidence capture. No code or app behavior changed.

Verification:

- User-reported WPF execution success for the specific approved first real-profile Quarantine batch.
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-first-real-profile-quarantine-execution.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-01-user-reported-first-real-profile-quarantine-success.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0017 and ADR 0018 already govern this exact first real-profile Quarantine path; this packet records manual trust evidence.

Open questions:

- Whether the user wants to immediately run selected restore recovery proof for the created Restore Manifest before any further real-profile Quarantine batches.

Follow-up work:

- Guide the user through selected-manifest restore proof for this specific Restore Manifest if requested.
- Keep any future real-profile Quarantine batch tiny, exact, user-approved, readiness-gated, and preceded by relevant preflight after code/workflow changes.

Risky assumptions:

- The screenshot/output supplied by the user is accepted as the manual live-trust evidence for this first real-profile Quarantine action.

### 2026-06-01: In-Use Source Pre-Execution Revalidation

Status: completed

Evidence:

- After the cross-volume directory fallback, the user retried the exact real-profile `DXCache` folder and reported `moved 0, failed 1` because a descendant NVIDIA `.nvph` cache file was being used by another process.
- This proved the cross-volume path progressed far enough to hit active-file access, but the blocker was still discovered during execution.

Implementation:

- Pre-Execution Revalidation now probes included source files with a read-only exclusive-read check before movement.
- For included folder rows, Pre-Execution Revalidation probes descendant files and reports bounded in-use or inaccessible blockers.
- The WPF Quarantine Execution Gate details scroller is larger again so dense readiness and failure evidence is easier to read.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-in-use-source-revalidation.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0018 already requires immediate live-filesystem revalidation before exact real-profile movement.

Open questions:

- Whether the user wants to retry a different tiny cache row, or retry this one only after the owning NVIDIA/graphics process releases the file.

Follow-up work:

- Run full `.cmd` MVP preflight before any retry.
- Consider future visible guidance to skip active GPU shader cache folders while graphics/NVIDIA processes are running.

Risky assumptions:

- Conservative exclusive-read probing is acceptable because Quarantine must remove the original path, not merely copy it.

### 2026-06-01: Cross-Volume Directory Quarantine Fallback

Status: completed

Evidence:

- The user reported a first exact real-profile folder Quarantine attempt against `C:\Users\moxhe\AppData\LocalLow\NVIDIA\DXCache` failed with `moved 0, failed 1`, recovery review required, and the Windows error that source and destination roots must be identical.
- The reported action targeted the preferred `D:\WindowsFileCleanerQuarantine` root, so the failure was the expected `Directory.Move` cross-volume limitation rather than a readiness-gate bypass.

Implementation:

- Added `QuarantineDirectoryMove` for directory movement under `QuarantineExecutor`.
- Same-volume folders still use `Directory.Move`; cross-volume folders use a staged copy-then-delete fallback after write-ahead Restore Manifest evidence is already in place.
- The fallback refuses source and descendant reparse points, uses an action-scoped staging folder, and leaves failure/recovery-review evidence if deleting the original fails after copy placement.
- Expanded the WPF Quarantine Execution Gate scroller from a compact strip to a taller bounded details area and updated its tooltip/help text for the exact real-profile phase.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-cross-volume-directory-quarantine.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0017 and ADR 0018 already cover the exact real-profile Quarantine contract and preferred `D:` root; this packet fixes directory movement inside that accepted path.

Open questions:

- Whether the user wants to retry the same tiny folder batch after full `.cmd` MVP preflight and app rebuild.

Follow-up work:

- Run full `.cmd` MVP preflight before any retry.
- After any successful live Quarantine, use manifest discovery and selected restore readiness for the created Restore Manifest if recovery proof is needed.

Risky assumptions:

- A guarded copy-then-delete fallback is acceptable for narrow, reviewed rebuildable-cache folders because Windows cannot do an atomic cross-volume directory rename.

### 2026-06-01: First Real-Profile Quarantine Execution

Status: completed

Evidence:

- The user explicitly chose to start the ADR 0017/0018 first real-profile Quarantine packet after successful fixture review, real-profile read-only retest, and selected real-profile restore trust testing.
- ADR 0017/0018 already define the first-phase contract: exact `C:\Users\moxhe`, exact `QUARANTINE`, 10 rows / 1 GB, Likely safe + Quarantine candidate only, strict descendant checks, root safety, immediate pre-execution revalidation, selected restore recovery, manual rescan, Restore Manifest-only, no permanent deletion, no all-manifest restore, and no cleanup history.

Implementation:

- WPF now treats exact `C:\Users\moxhe` as an implemented Quarantine execution scope only through the scope-aware execution guard.
- Real-profile execution requires the existing Quarantine Execution Gate, Real-Profile Quarantine Approval Evidence, selected real-profile restore trust, and immediate root-safety plus pre-execution revalidation immediately before `QuarantineExecutor.Execute`.
- Synthetic exact real-profile missing-source execution attempts rerun immediate revalidation and report no movement.
- Custom and real-profile-child scopes remain preview-only.
- Current-fixture Undo Quarantine remains fixture-only; real-profile recovery stays selected-manifest-only through discovery and selected restore.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-first-real-profile-quarantine-execution.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0017 and ADR 0018 already cover this exact first real-profile Quarantine execution contract.

Open questions:

- Which specific tiny real-profile batch the user wants to approve and click first after full `.cmd` MVP preflight.

Follow-up work:

- Run full `.cmd` MVP preflight before any live real-profile Quarantine click.
- Guide the user through one exact first batch, then rediscover manifests/rescan and optionally prove selected restore recovery for that created manifest.

Risky assumptions:

- The user-reported selected real-profile restore trust test is sufficient recovery confidence for the first owner-only forward Quarantine batch.

### 2026-06-01: User-Reported Real-Profile Selected Restore Trust Test

Status: completed

Evidence:

- The user ran the documented selected restore trust test and reported that steps 1 through 9 all succeeded.
- The trust test used the sacrificial Restore Manifest helper path and the WPF `Restore selected manifest` flow.
- This is user-reported manual trust evidence for exact real-profile selected restore, not approval for real-profile Quarantine execution.

Implementation:

- Recorded the successful manual selected real-profile restore trust test in current progress, roadmap, and handoff docs.
- Kept real-profile Quarantine execution, broad/all-manifest Undo Quarantine, permanent deletion, and cleanup history unavailable.

Verification:

- User-reported completion of steps 1-9 with all success.
- `git diff --check`

Docs updated:

- `.codex/progress.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-01-real-profile-selected-restore-execution.md`

ADRs:

- No ADR added. ADR 0019 already covers selected real-profile restore; first real-profile Quarantine remains governed by ADR 0017/0018.

Open questions:

- Whether the user wants to start the first real-profile Quarantine execution Grill with Docs packet next.

### 2026-06-01: Real-Profile Selected Restore Trust Helper

Status: completed

Evidence:

- The user reported the real-profile read-only pass still looked good and explicitly chose to proceed with a restore test.
- There was no existing helper for creating a safe exact real-profile Restore Manifest without first enabling real-profile Quarantine execution.

Implementation:

- Added `tools\New-RealProfileSelectedRestoreTrustManifest.ps1` plus a `.cmd` wrapper.
- The helper creates one sacrificial Restore Manifest under `D:\WindowsFileCleanerQuarantine` by default and targets `C:\Users\moxhe\WindowsFileCleanerRestoreTrustTest\restore-target.txt`.
- The helper refuses to proceed if the restore target already exists, uses exact `C:\Users\moxhe` cleanup scope, and does not quarantine existing personal files.
- README now has a manual trust-test checklist that leaves the restore click to the user.

Verification:

- `cmd.exe /c tools\New-RealProfileSelectedRestoreTrustManifest.cmd -WhatIf`
- `cmd.exe /c tools\New-RealProfileSelectedRestoreTrustManifest.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\real-profile-selected-restore-trust-helper-smoke"`
- Inspected the generated ignored `.local` Restore Manifest and confirmed ISO timestamp JSON.

Docs updated:

- `README.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs:

- No ADR added. This is a helper for manually trusting the ADR 0019 path, not a new restore policy.

Open questions:

- Whether the manual trust click restores the sacrificial file cleanly on the user's machine.

### 2026-06-01: Real-Profile Selected Restore Execution

Status: completed

Evidence:

- The user explicitly approved starting the ADR 0019 selected real-profile restore implementation path before real-profile Quarantine execution.
- ADR 0019 already defined selected-manifest-only, exact `C:\Users\moxhe`, exact `RESTORE`, immediate revalidation, no original-path overwrite, Restore Manifest-only, manual rediscover/rescan, no all-manifest restore, no action-folder cleanup, no permanent deletion, and no cleanup history.

Implementation:

- WPF selected restore execution availability is limited to fixture Restore Manifests or exact real-profile Restore Manifests.
- Exact real-profile selected restore requires passing Selected Restore Pre-Execution Revalidation before the button enables.
- `ExecuteSelectedRestoreForCurrentSelection` reruns revalidation immediately before calling `UndoQuarantineExecutor.Undo`.
- Custom non-fixture and non-exact real-profile selected restore remain unavailable.
- Real-profile Quarantine execution remains unavailable.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- The first parallel app-test attempt failed on a shared core DLL build lock; rerunning the app tests by themselves passed.

Docs updated:

- `docs/features/2026-06-01-real-profile-selected-restore-execution.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0019 already records this execution contract.

Open questions:

- When the user wants to manually trust the path, which exact real-profile Restore Manifest should be selected for the first click-through?

### 2026-06-01: User-Reported Fresh Real-Profile Read-Only Retest

Status: completed

Evidence:

- After the user-reported manual fixture visual pass, the user completed the requested fresh real-profile read-only retest and reported that everything worked well.
- The requested retest covered full preflight, launching the WPF app with `--scope "C:\Users\moxhe"`, real-profile scan-gate acknowledgement, read-only scan completion, search responsiveness, tab/header usability, Review Shortlist context, and preview-only Quarantine boundary.
- No real-profile movement was requested or approved.

Implementation:

- Recorded the user-reported real-profile read-only retest in current progress, roadmap, and handoff docs.
- Advanced the next recommended work from real-profile read-only retest to the ADR 0019 selected real-profile restore implementation decision point.
- Kept real-profile Quarantine execution, real-profile selected restore, real-profile Undo Quarantine, permanent deletion, and persisted cleanup history unavailable.

Verification:

- User-reported completion of the requested real-profile read-only retest steps.
- `git diff --check`

Docs updated:

- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0019 already records the selected real-profile restore execution contract, and this packet records readiness evidence only.

Open questions:

- Whether the user wants to start the ADR 0019 selected real-profile restore implementation packet next.

### 2026-06-01: User-Reported Manual Fixture Visual Pass

Status: completed

Evidence:

- The latest full `.cmd` MVP preflight passed after the Checklist-Only Visible Fixture Next Step packet at `71cf15a`.
- The user ran the visible fixture review flow and reported: "looks good!"
- The latest local ignored notes file is `.local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md`, stamped from clean `main` at `433064e`, but the checklist remains unfilled.

Implementation:

- Recorded user-reported visual fixture acceptance in the current progress and handoff docs.
- Kept the formal acceptance-notes status explicit: the local notes are available but not checked off.
- No real-profile scan, real-profile movement, permanent deletion, or cleanup history was requested or enabled.

Verification:

- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd`
- Summary output reported `.local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md`, `Git commit: 433064e`, `Worktree at notes creation: clean`, `Overall result: Not recorded`, and `0 pass, 0 issue, 0 not checked, 10 not recorded`.
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md" -RequireComplete` returned exit code 1 as expected because the notes remain unfilled.

Docs updated:

- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is docs/evidence capture for manual fixture acceptance, with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Whether to complete the ignored fixture acceptance notes checklist before the fresh real-profile read-only retest, or treat the user's visual acceptance as enough for the next read-only gate.

### 2026-06-01: Fixture Acceptance Current Baseline Notes Preview

Status: completed

Evidence:

- The latest pushed baseline added a full `.cmd` MVP preflight after checklist-only next-step guidance.
- The ignored checklist-only notes preview still pointed at an older clean commit, so the next manual fixture pass benefits from a current clean-worktree notes preview.

Implementation:

- Generated a checklist-only fixture acceptance notes template from clean `main` at commit `dd86566`.
- Confirmed the ignored notes header stamped `Worktree status at notes creation: clean`, `.NET SDK: 8.0.421`, WPF app project `src\WindowsFileCleaner.App\WindowsFileCleaner.App.csproj`, target framework `net8.0-windows`, and `WPF enabled: true`.
- Summarized the notes with the read-only helper and confirmed the completion check remains incomplete until the visible fixture pass is filled.
- No preflight, fixture creation, WPF launch, real-profile scan, movement, restore, deletion, or cleanup history occurred.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- Inspected `.local\fixture-review-acceptance\fixture-acceptance-20260601-131233.md`; it stamped `Git commit: dd86566` and `Worktree status at notes creation: clean`.
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-131233.md"`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-131233.md" -RequireComplete` returned exit code 1 as expected for unfilled notes, with blockers for unrecorded preflight evidence, unrecorded worktree evidence, missing overall result, and 10 not-recorded checklist items.
- `git diff --check`
- `git diff --cached --check`

Docs updated:

- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is ignored-notes evidence only, with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Whether the visible fixture pass records any Quarantine/readiness or selected-restore UI polish before a fresh real-profile read-only retest.

### 2026-06-01: Full Local MVP Preflight After Checklist-Only Next Step

Status: completed

Evidence:

- The Checklist-Only Visible Fixture Next Step packet changed launcher output that is printed during MVP preflight.
- A fresh full `.cmd` MVP preflight on current `main` re-establishes the verification baseline before the visible fixture pass.

Implementation:

- Ran the full `.cmd` MVP preflight from the repository root after `71cf15a`.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the sectioned fixture checklist with the exact visible fixture next-step block, and ran whitespace diff checking.
- No WPF app was launched, no real-profile scan was run, and no files were moved, restored, deleted, or added to cleanup history.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight printed the next manual fixture command: `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`
- `git diff --check`
- `git diff --cached --check`

Docs updated:

- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Whether the next visible fixture pass records any Quarantine/readiness or selected-restore UI polish before a fresh real-profile read-only retest.

### 2026-06-01: Checklist-Only Visible Fixture Next Step

Status: completed

Evidence:

- User visually approved the wide header layout with Review Shortlist totals before scan totals.
- The next safe gate remains a visible fixture pass, but plain checklist-only output did not say exactly which notes-enabled command to run next.

Implementation:

- Added a checklist-only next-step block to `Start-MvpFixtureReview.ps1`.
- `.\tools\Start-MvpFixtureReview.cmd -ChecklistOnly` now prints the exact post-preflight visible fixture command, `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`.
- The same block states that checklist-only mode did not run preflight, create fixture files, launch WPF, scan, move, restore, delete, or create cleanup history.
- No WPF app was launched, no real-profile scan was run, and no files were moved, restored, deleted, or added to cleanup history.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore`
- `git diff --check`
- `git diff --cached --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local fixture-review launcher output only, with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- None.

### 2026-06-01: Full Local MVP Preflight After Current Evidence Wording Alignment

Status: completed

Evidence:

- The Current Evidence Wording Alignment packet changed handoff/progress docs after the previous full preflight evidence.
- A fresh full `.cmd` MVP preflight on current `main` re-establishes the verification baseline before the visible fixture pass.

Implementation:

- Ran the full `.cmd` MVP preflight from the repository root after `466ad79`.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the sectioned fixture checklist in checklist-only mode, and ran whitespace diff checking.
- No WPF app was launched, no real-profile scan was run, and no files were moved, restored, deleted, or added to cleanup history.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight printed the next manual fixture command: `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`
- `git diff --check`
- `git diff --cached --check`

Docs updated:

- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Whether the next visible fixture pass records any Quarantine/readiness or selected-restore UI polish before a fresh real-profile read-only retest.

### 2026-06-01: Current Evidence Wording Alignment

Status: completed

Evidence:

- The current progress and handoff docs already recorded the clean notes preview and latest full preflight evidence.
- A current-status paragraph and startup packet list still mentioned older `b9bd33d` / `fd8e1d4` evidence in ways that could be mistaken for the active baseline.

Implementation:

- Updated the progress current status to name the clean-notes baseline from `f181627` and the full preflight after `8529a91`.
- Updated the thread handoff latest-packet/current-state labels and startup prompt to clarify that older evidence references are historical.
- Kept the next visible fixture pass as `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`.
- Did not launch WPF, scan, move, restore, delete, or create cleanup history.

Verification:

- `git diff --check`
- `git diff --cached --check`

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is handoff/progress wording alignment with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- None.

### 2026-06-01: Full Local MVP Preflight After Clean Notes Preview

Status: completed

Evidence:

- The worktree-stamp and clean-notes preview packets changed fixture acceptance tooling/docs after the previous full preflight evidence.
- A current full `.cmd` MVP preflight re-establishes the verification baseline before the next visible fixture pass.

Implementation:

- Ran the full `.cmd` MVP preflight from the repository root after `8529a91`.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the sectioned fixture checklist in checklist-only mode, and ran whitespace diff checking.
- No WPF app was launched, no real-profile scan was run, and no files were moved, restored, deleted, or added to cleanup history.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight printed the next manual fixture command: `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`

Docs updated:

- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Whether the next visible fixture pass records any Quarantine/readiness or selected-restore UI polish before a fresh real-profile read-only retest.

### 2026-06-01: Fixture Acceptance Clean Worktree Notes Preview

Status: completed

Evidence:

- The worktree-stamp packet proved generated notes can record dirty worktree state while that packet was intentionally in progress.
- The next manual fixture pass benefits from a fresh ignored notes artifact created from the current pushed commit while the repository is clean.

Implementation:

- Generated a checklist-only fixture acceptance notes template from clean `main` at commit `f181627`.
- Confirmed the ignored notes header stamped `Worktree status at notes creation: clean`.
- Summarized the notes with the read-only helper and confirmed the completion check remains incomplete until the visible fixture pass is filled.
- Kept the packet free of preflight, fixture creation, WPF launch, real-profile scan, movement, restore, deletion, and cleanup history.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- Inspected `.local\fixture-review-acceptance\fixture-acceptance-20260601-122940.md`; it stamped `Git commit: f181627` and `Worktree status at notes creation: clean`.
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-122940.md"`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-122940.md" -RequireComplete` returned exit code 1 as expected for unfilled notes, with blockers for unrecorded preflight evidence, unrecorded worktree evidence, missing overall result, and 10 not-recorded checklist items.

Docs updated:

- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is ignored-notes evidence and handoff cleanup with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Whether the next visible fixture pass records any Quarantine/readiness or selected-restore UI polish before the fresh real-profile read-only retest.

### 2026-06-01: Fixture Acceptance Notes Worktree Stamp

Status: completed

Evidence:

- Fixture acceptance notes already stamped branch/commit and included a manual worktree evidence checkbox.
- The notes did not record what `git status --short` looked like at notes creation time, so later summary output could not distinguish a clean launch from an intentionally dirty one.

Implementation:

- Added `Worktree status at notes creation` to generated ignored fixture acceptance notes.
- The launcher records `clean`, `not clean (N status lines)`, or `unknown` without failing notes creation.
- Updated the summary helper to print that stamped worktree status; older notes that lack the line summarize it as `unknown`.
- Kept notes local/ignored and kept checklist-only behavior free of preflight, fixture creation, WPF launch, scan, movement, restore, deletion, and cleanup history.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- Inspected `.local\fixture-review-acceptance\fixture-acceptance-20260601-122156.md`; it stamped `Worktree status at notes creation: not clean (2 status lines)` while this packet was intentionally dirty.
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path ".local\fixture-review-acceptance\fixture-acceptance-20260601-122156.md"`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local ignored-notes evidence polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Whether the visible fixture pass shows that the stamped worktree status plus manual checkbox is enough, or whether notes should also include a copied `git status --short` detail block.

### 2026-06-01: Full Local MVP Preflight After Embedded Notes

Status: completed

Evidence:

- The latest accepted tooling packet embedded exact fixture acceptance summary and completion-check commands in generated ignored notes.
- A full local preflight after that packet re-establishes the current safety baseline before the next visible fixture pass.

Implementation:

- Ran the full `.cmd` MVP preflight from the repository root after `b9bd33d`.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the sectioned fixture checklist in checklist-only mode, and ran whitespace diff checking.
- No WPF app was launched, no real-profile scan was run, and no files were moved, restored, deleted, or added to cleanup history.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight printed the next manual fixture command: `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`

Docs updated:

- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Whether the next visible fixture pass records any Quarantine/readiness or selected-restore UI polish before a fresh real-profile read-only retest.

### 2026-06-01: Fixture Acceptance Notes Embedded Commands

Status: completed

Evidence:

- The launcher prints exact summary commands after writing acceptance notes, but those commands could still be lost in terminal scrollback during a visible fixture pass.
- The notes file already has the exact path available at generation time and is the local artifact the user fills during acceptance.

Implementation:

- Added a `Post-pass summary commands` section to generated fixture acceptance notes.
- The generated section includes exact `.\tools\Summarize-FixtureAcceptanceNotes.cmd -Path "<notes file>"` and `.\tools\Summarize-FixtureAcceptanceNotes.cmd -Path "<notes file>" -RequireComplete` commands.
- The generated section repeats that the commands read ignored notes only and do not launch WPF, scan, move, restore, delete, or create cleanup history.
- Kept launcher, preflight, summary helper, Storage Scan, fixture movement, restore, deletion, cleanup history, and real-profile/custom blockers unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- Inspected the generated notes header and command block.
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path "D:\Codex\Windows File Cleaner\.local\fixture-review-acceptance\fixture-acceptance-20260601-121043.md"`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path "D:\Codex\Windows File Cleaner\.local\fixture-review-acceptance\fixture-acceptance-20260601-121043.md" -RequireComplete` returned exit code 1 as expected for the fresh checklist-only notes file, with blockers for unrecorded preflight evidence, unrecorded worktree evidence, missing overall result, and 10 not-recorded checklist items.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/features/2026-06-01-preflight-fixture-notes-next-step.md`
- `docs/features/2026-06-01-fixture-notes-launcher-wording-alignment.md`
- `docs/features/2026-06-01-fixture-acceptance-current-commit-notes-preview.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local ignored-notes workflow guidance with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- Whether duplicating the commands in both terminal output and the notes file feels helpful or too noisy during the visible pass.

### 2026-06-01: Fixture Acceptance Post-Pass Guidance

Status: completed

Evidence:

- The fixture launcher could write ignored acceptance notes, and the summary helper could review or require completion for those notes.
- The launcher output did not yet print the exact summary commands for the notes file it just wrote, leaving the next human step easier to miss after the visible pass.

Implementation:

- Added post-pass summary guidance to `tools\Start-MvpFixtureReview.ps1` after notes are written in checklist-only and visible-launch paths.
- Updated `tools\Invoke-MvpPreflight.ps1` success output to point users to the printed summary and completion-check commands after the notes-enabled fixture launcher writes notes.
- Kept default launcher behavior, WPF launch behavior, Storage Scan behavior, fixture creation, movement, restore, deletion, cleanup history, and real-profile/custom blockers unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path "D:\Codex\Windows File Cleaner\.local\fixture-review-acceptance\fixture-acceptance-20260601-120216.md"`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path "D:\Codex\Windows File Cleaner\.local\fixture-review-acceptance\fixture-acceptance-20260601-120216.md" -RequireComplete` returned exit code 1 as expected for the fresh checklist-only notes file, with blockers for unrecorded preflight evidence, unrecorded worktree evidence, missing overall result, and 10 not-recorded checklist items.
- Inspected `tools\Invoke-MvpPreflight.ps1` success-output wording.
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/features/2026-06-01-preflight-fixture-notes-next-step.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local terminal-output guidance with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- Whether the visible fixture pass shows that the printed follow-up commands are enough, or whether the notes template itself should include the same commands.

### 2026-06-01: Fixture Acceptance Completion Check

Status: completed

Evidence:

- The summary helper reports open checklist items and evidence checkbox states, but a follow-up command had no way to fail fast when local acceptance notes were still incomplete.
- The current checklist-only preview should fail such a completion check until the visible fixture pass is run and notes are filled.

Implementation:

- Added `-RequireComplete` to `tools\Summarize-FixtureAcceptanceNotes.ps1`.
- The switch exits non-zero when preflight/worktree evidence is not recorded, overall result is not `Pass` or `Pass with issues noted`, no checklist items are found, or any checklist item is `Not checked` / `Not recorded`.
- Kept default summary behavior unchanged and read-only.
- Kept WPF launch, fixture creation, Storage Scan, movement, restore, deletion, cleanup history, and real-profile/custom execution behavior unchanged.

Verification:

- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path .local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -RequireComplete` returned exit code 1 as expected for the current checklist-only preview, with blockers for unrecorded preflight evidence, unrecorded worktree evidence, missing overall result, and 10 not-recorded checklist items.
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local read-only terminal tooling with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- Whether a future stricter mode should require zero `Issue` items, or whether recorded issues should remain acceptable for the notes-complete check.

### 2026-06-01: Fixture Acceptance Evidence Checkbox Summary

Status: completed

Evidence:

- Fixture acceptance notes include local evidence checkboxes for preflight passed and clean/intentional worktree state.
- The summary helper did not surface those evidence fields, so a reviewer could miss that the visible fixture pass still needs those checkboxes recorded.

Implementation:

- Added acceptance-evidence checkbox parsing to `tools\Summarize-FixtureAcceptanceNotes.ps1`.
- Summary output now shows `preflight passed` and `worktree clean/intentional` states as `Recorded`, `Not recorded`, or `Missing`.
- Kept notes summary read-only; it still does not launch WPF, create fixtures, scan, move, restore, delete, or create cleanup history.

Verification:

- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path .local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md`
- Both runs summarized `.local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md` and showed `Acceptance evidence: preflight passed: Not recorded; worktree clean/intentional: Not recorded`.
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local read-only terminal-output polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- Whether future acceptance notes should include more evidence checkboxes, such as exact WPF build artifact/version evidence, before packaging.

### 2026-06-01: Fixture Acceptance Summary Prompt Preview

Status: completed

Evidence:

- The summary helper parsed each checklist prompt but open-item output only named section and status when no notes were recorded.
- The next visible fixture pass benefits from enough prompt context to choose the next manual item without reopening the long notes file.

Implementation:

- Added compact text formatting to `tools\Summarize-FixtureAcceptanceNotes.ps1`.
- Open checklist items now show recorded notes when present, or a trimmed prompt preview when notes are blank.
- Kept notes summary read-only; it still does not launch WPF, create fixtures, scan, move, restore, delete, or create cleanup history.

Verification:

- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path .local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md`
- Both runs summarized `.local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md` and showed compact prompt previews for the 10 not-recorded checklist items.
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local read-only terminal-output polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- Whether the visible fixture pass shows that 180-character prompt previews are too long or too short.

### 2026-06-01: Fixture Acceptance Notes Summary Helper

Status: completed

Evidence:

- The latest ignored fixture acceptance notes preview is detailed enough for a full manual pass but benefits from a quick open-items readout.
- Manual fixture evidence should remain local and read-only until relevant results are copied into tracked progress docs.

Implementation:

- Added `tools\Summarize-FixtureAcceptanceNotes.ps1` and `tools\Summarize-FixtureAcceptanceNotes.cmd`.
- The helper defaults to the newest `.local\fixture-review-acceptance\fixture-acceptance-*.md` file and accepts a specific repo-local `-Path`.
- The helper prints notes metadata, Git/WPF build context, overall result, checklist totals, and issue/not-checked/not-recorded checklist items.
- Kept WPF launch, fixture creation, Storage Scan, file movement, restore, deletion, cleanup history, and real-profile/custom execution behavior unchanged.

Verification:

- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Summarize-FixtureAcceptanceNotes.cmd -Path .local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md`
- Both runs summarized `.local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md`, including `main`, commit `fd8e1d4`, WPF project context, `Overall result: Not recorded`, and `0 pass, 0 issue, 0 not checked, 10 not recorded`.
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-summary-helper.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a local read-only review helper with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- Whether completed notes from the visible fixture pass show the checklist still needs shortening or splitting.

### 2026-06-01: Fixture Acceptance Current Commit Notes Preview

Status: completed

Evidence:

- Full `.cmd` MVP preflight passed after the Fixture Acceptance Build Context Header packet.
- The next visible fixture pass benefits from a current-commit notes template that stamps the exact code/build context and checklist wording to review.

Implementation:

- Ran the notes-enabled checklist-only launcher from clean, synced `main`.
- Inspected the generated ignored `.local` acceptance notes header.
- Updated feature, roadmap, handoff, and progress docs so the next step is the visible fixture pass, not another checklist-only preparation step.
- Kept preflight execution, fixture creation, WPF launch, Storage Scan, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- Inspected `.local\fixture-review-acceptance\fixture-acceptance-20260601-112248.md`; the evidence header stamped `main`, commit `fd8e1d4`, `.NET SDK: 8.0.421`, `WPF app project: src\WindowsFileCleaner.App\WindowsFileCleaner.App.csproj`, `WPF app target framework: net8.0-windows`, `WPF enabled: true`, the required preflight command, the visible fixture command, preflight/worktree checkboxes, and local-not-cleanup-history wording.

Docs updated:

- `docs/features/2026-06-01-fixture-acceptance-current-commit-notes-preview.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local fixture-review evidence preparation with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- The visible manual fixture pass is still needed to prove the full Quarantine/readiness and selected-restore flow by eye.
- Whether that visible pass finds any checklist section that should be shortened or split further.

### 2026-06-01: Full Local MVP Preflight After Build Context Header

Status: completed

Evidence:

- Fixture Acceptance Build Context Header changed fixture launcher note generation and current-facing docs after the previous full `.cmd` MVP preflight.
- A fresh full preflight gives current evidence before the next visible fixture acceptance pass.

Implementation:

- Ran the full `.cmd` MVP preflight after the build-context-header packet.
- Recorded the updated full-preflight evidence in project roadmap, handoff, and progress docs.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
  - Restore passed.
  - Build passed with 0 warnings and 0 errors.
  - Core tests passed.
  - WPF app tests passed.
  - Fixture dry run passed with `-WhatIf`.
  - Fixture checklist-only output passed and printed grouped section headers plus the latest manual fixture checklist wording.
  - Whitespace diff check passed.
  - The preflight reported that no real user files were scanned or modified and printed `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` as the next manual fixture step.

Docs updated:

- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-01-fixture-acceptance-build-context-header.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only; no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- The visible manual fixture pass is still needed to prove the full Quarantine/readiness and selected-restore flow by eye.

### 2026-06-01: Fixture Acceptance Build Context Header

Status: completed

Evidence:

- Fixture acceptance notes already stamped repository path, Git branch/commit, required preflight, post-preflight visible fixture command, and local evidence checkboxes.
- The evidence-header feature brief left open whether to capture WPF app build/version evidence later.
- The next visible fixture pass benefits from local .NET/WPF project context, but release packaging and executable hashes do not exist yet.

Implementation:

- Added .NET SDK, WPF app project, WPF app target framework, and WPF enabled fields to generated fixture acceptance notes.
- Read WPF project evidence from the `.csproj` XML and fall back to `unknown` when evidence cannot be read.
- Updated README, fixture acceptance feature notes, live-product roadmap, progress, and handoff docs.
- Kept checklist-only behavior, fixture creation, WPF launch, Storage Scan, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- Inspected `.local\fixture-review-acceptance\fixture-acceptance-20260601-111237.md`; the evidence header included `.NET SDK: 8.0.421`, `WPF app project: src\WindowsFileCleaner.App\WindowsFileCleaner.App.csproj`, `WPF app target framework: net8.0-windows`, and `WPF enabled: true`.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-build-context-header.md`
- `docs/features/2026-06-01-fixture-acceptance-evidence-header.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local fixture-review evidence capture with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- Whether a later packaging packet should add built executable version/file hash evidence.
- The visible manual fixture pass is still needed to prove the full Quarantine/readiness and selected-restore flow by eye.

### 2026-06-01: Live Product Roadmap Evidence Alignment

Status: completed

Evidence:

- The fixture launcher notes now include repository path, Git branch/commit, required preflight, post-preflight visible fixture command, preflight/worktree checkboxes, and local-not-cleanup-history wording.
- Full `.cmd` MVP preflight passed after that evidence-header packet.
- The live-product readiness roadmap's manual fixture acceptance row still summarized older notes-template/grouping evidence without naming the evidence header and post-header preflight evidence together.

Implementation:

- Updated the live-product readiness roadmap manual fixture acceptance evidence and completion notes to name the evidence header and the full-preflight-after-header gate.
- Updated progress and handoff docs so fresh threads land on the same current acceptance gate.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- Inspected `.local\fixture-review-acceptance\fixture-acceptance-20260601-110616.md`; the evidence header stamped `main`, commit `884009d`, the required preflight command, the post-preflight visible fixture command, and local-not-cleanup-history wording.
- `git diff --check`

Docs updated:

- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is documentation evidence alignment with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- The visible manual fixture pass is still needed to prove the full Quarantine/readiness and selected-restore flow by eye.

### 2026-06-01: Full Local MVP Preflight After Evidence Header

Status: completed

Evidence:

- Fixture Acceptance Evidence Header changed the fixture launcher notes template and docs after the previous full `.cmd` MVP preflight.
- A fresh full preflight gives current evidence before the next visible fixture acceptance pass.

Implementation:

- Ran the full `.cmd` MVP preflight after the evidence-header packet was pushed.
- Recorded the updated full-preflight evidence in project handoff/progress docs.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
  - Restore passed.
  - Build passed with 0 warnings and 0 errors.
  - Core tests passed.
  - WPF app tests passed.
  - Fixture dry run passed with `-WhatIf`.
  - Fixture checklist-only output passed and printed grouped section headers plus the latest manual fixture checklist wording.
  - Whitespace diff check passed.
  - The preflight reported that no real user files were scanned or modified and printed `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` as the next manual fixture step.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only; no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- The visible manual fixture pass is still needed to prove the full Quarantine/readiness and selected-restore flow by eye.

### 2026-06-01: Fixture Acceptance Evidence Header

Status: completed

Evidence:

- The fixture launcher can write ignored `.local` acceptance notes for the next visible fixture pass.
- The notes template captured checklist results but did not stamp branch/commit or a preflight evidence checkbox near the reviewer notes.
- The next manual acceptance gate should tie visual results back to the exact code state being reviewed.

Implementation:

- Added an acceptance evidence header to generated fixture acceptance notes with repository path, Git branch, Git commit, required preflight command, recommended visible fixture command, preflight/worktree checkboxes, and local-not-cleanup-history wording.
- Added a compact feature brief for the evidence-header packet.
- Kept launcher behavior, preflight behavior, Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes` passed, printed the sectioned checklist, and wrote an ignored `.local\fixture-review-acceptance\fixture-acceptance-*.md` notes template.
- Inspected `.local\fixture-review-acceptance\fixture-acceptance-20260601-105046.md`; the evidence header included repository, branch, commit, preflight command, visible fixture command, evidence checkboxes, and local-not-cleanup-history wording.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes` passed and did not write notes under `-WhatIf`.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-acceptance-evidence-header.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local fixture-review evidence capture with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Open questions:

- Should a future packet capture WPF app build/version evidence in the notes header, or is Git branch/commit enough for the next acceptance pass?

### 2026-06-01: Fixture Notes Launcher Wording Alignment

Status: completed

Evidence:

- The preflight success output now suggests `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`.
- Several current-facing docs still mentioned `.\tools\Start-MvpFixtureReview.cmd -WriteAcceptanceNotes` as the next visible fixture command.
- The next manual acceptance gate is a post-preflight visible fixture pass with local notes.

Implementation:

- Updated README, live-product roadmap, and fixture-notes feature briefs so current next-step wording recommends `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` after successful preflight.
- Added a compact feature brief for the wording alignment.
- Kept launcher behavior, preflight behavior, Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes` passed, printed the sectioned checklist, and wrote an ignored `.local\fixture-review-acceptance\fixture-acceptance-*.md` notes template.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-checklist-section-grouping.md`
- `docs/features/2026-06-01-fixture-notes-launcher-wording-alignment.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is docs/workflow wording alignment with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Open questions:

- None for this packet.

### 2026-06-01: Preflight Fixture Notes Next Step

Status: completed

Evidence:

- The fixture launcher can write ignored `.local` fixture acceptance notes with `-WriteAcceptanceNotes`.
- The live-product readiness roadmap names visible fixture acceptance as the next gate.
- The previous preflight success output still suggested the older no-notes fixture launcher command.

Implementation:

- Updated `tools/Invoke-MvpPreflight.ps1` so successful preflight output suggests `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`.
- Updated README and the MVP preflight feature brief to match the notes-enabled fixture-review workflow.
- Kept preflight steps, fixture launcher behavior, Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore` passed and printed the notes-enabled next manual fixture step after build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output, and whitespace diff.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes` passed, printed the sectioned checklist, and wrote an ignored `.local\fixture-review-acceptance\fixture-acceptance-*.md` notes template.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/features/2026-06-01-preflight-fixture-notes-next-step.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local workflow-output polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Open questions:

- None for this packet.

### 2026-06-01: Full Local MVP Preflight After Section Grouping

Status: completed

Evidence:

- Fixture Checklist Section Grouping changed the fixture launcher checklist and generated acceptance notes.
- The previous full `.cmd` MVP preflight evidence was from before the sectioned checklist output.
- A fresh full local preflight gives current evidence before the next visible fixture acceptance pass.

Implementation:

- Ran the full `.cmd` MVP preflight after the section grouping packet was pushed.
- Recorded the updated full-preflight evidence in project handoff/progress docs.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
  - Restore passed.
  - Build passed with 0 warnings and 0 errors.
  - Core tests passed.
  - WPF app tests passed.
  - Fixture dry run passed with `-WhatIf`.
  - Fixture checklist-only output passed and printed grouped section headers for Scan header and gate, Safety Summary / Review / Main Grid, Quarantine Preview / fixture Quarantine, Restore Manifest review / selected restore, and Real-profile / custom blockers.
  - Whitespace diff check passed.
  - The preflight reported that no real user files were scanned or modified.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only; no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- The visible manual fixture pass is still needed to prove the full Quarantine/readiness and selected-restore flow by eye.

### 2026-06-01: Fixture Checklist Section Grouping

Status: completed

Evidence:

- The live-product readiness roadmap names visible fixture acceptance as the next gate.
- The tabbed/header UI is now user-approved, so the manual acceptance prompt should be easier to follow by workflow area.
- The fixture checklist and acceptance notes already existed, but they read as one long sequence of dense prompts.

Implementation:

- Added section headers to `Start-MvpFixtureReview.ps1` checklist output: Scan header and gate; Safety Summary, Review, and Main Grid; Quarantine Preview and fixture Quarantine; Restore Manifest review and selected restore; Real-profile and custom blockers.
- Added matching section headings to generated `.local\fixture-review-acceptance\fixture-acceptance-*.md` templates.
- Kept the existing ten numbered prompts and safety wording unchanged.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the sectioned checklist without preflight, fixture creation, WPF launch, scan, move, restore, delete, or cleanup history.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes` passed, printed the sectioned checklist, and wrote an ignored `.local\fixture-review-acceptance\fixture-acceptance-*.md` notes template with matching section headings.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes` passed and did not write notes under `-WhatIf`.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-fixture-checklist-section-grouping.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is fixture-review workflow output polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Open questions:

- After a real manual pass, should any section be split or shortened further?

### 2026-06-01: Fixture Acceptance Notes Template

Status: completed

Evidence:

- The live-product readiness roadmap names visible fixture acceptance as the next gate.
- The fixture launcher already prints the current checklist, but the pass needed an easy local place to record results.
- Checklist-only output is the safest verification path because it does not run preflight, create fixture files, launch WPF, scan, move, restore, delete, or create cleanup history.

Implementation:

- Added `-WriteAcceptanceNotes` to `tools/Start-MvpFixtureReview.ps1`.
- Refactored the terminal checklist text into a shared checklist item source.
- Added timestamped markdown notes templates under ignored `.local\fixture-review-acceptance` with safety boundary, overall result, and pass/issue/not-checked slots for each checklist item.
- Kept normal launcher and checklist-only behavior unchanged unless `-WriteAcceptanceNotes` is passed.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the checklist without preflight, fixture creation, WPF launch, scan, move, restore, delete, or cleanup history.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes` passed, printed the checklist, and wrote an ignored `.local\fixture-review-acceptance\fixture-acceptance-*.md` notes template.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes` passed and did not write notes under `-WhatIf`.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local fixture-review evidence capture with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Open questions:

- After a real manual pass, should any section be split or shortened further?

### 2026-06-01: Live Product Readiness Roadmap

Status: completed

Evidence:

- The active project goal is a safe full live product, but current implementation intentionally stops before real-profile movement.
- ADR 0017, ADR 0018, and ADR 0019 define the safety contracts, while README/progress/handoff tracked current evidence and next steps across several places.
- A compact roadmap makes the remaining gates explicit without implying real-profile movement is available.

Implementation:

- Added a live-product readiness roadmap feature brief.
- Linked it from README and refreshed handoff/progress notes.
- Printed the current fixture checklist in checklist-only mode as the safe first step before a visible fixture pass.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the manual fixture review checklist without preflight, fixture creation, WPF launch, scan, move, restore, delete, or cleanup history.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This roadmap organizes existing accepted decisions without changing architecture, persistence, cleanup execution, restore behavior, data-model, or security policy.

Open questions:

- Whether first live release requires packaging before or after the first reversible real-profile cleanup action.
- Whether permanent deletion, persisted cleanup history, or all-manifest restore should ever be added.

### 2026-06-01: Full Local MVP Preflight After Fixture Checklist Cue Wording

Status: completed

Evidence:

- The Fixture Checklist Grid-Mode Cue Wording packet touched the fixture launcher checklist, feature notes, handoff, and progress log.
- The previous full `.cmd` MVP preflight evidence was still from the Main Grid active-lens packet.
- A fresh full local preflight gives current evidence before a visible fixture pass or later real-profile retest.

Implementation:

- Ran the full `.cmd` MVP preflight after the fixture-checklist cue wording packet was pushed.
- Recorded the updated full-preflight evidence in project handoff/progress docs.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
  - Restore passed.
  - Build passed with 0 warnings and 0 errors.
  - Core tests passed.
  - WPF app tests passed.
  - Fixture dry run passed with `-WhatIf`.
  - Fixture checklist-only output passed and includes the clarified Review Grid Mode Status / Main Grid active review lens wording.
  - Whitespace diff check passed.
  - The preflight reported that no real user files were scanned or modified.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only; no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- A visible manual fixture pass is still useful for Quarantine/readiness and selected-restore flow, but automated/full preflight evidence is current after the latest checklist wording.

### 2026-06-01: Fixture Checklist Grid-Mode Cue Wording

Status: completed

Evidence:

- The next recommended work is a visible fixture pass through Quarantine/readiness and selected restore.
- Checklist step 7 mentioned styled Review Grid Mode Status, Main Grid Active Review Lens Summary, and `its hoverable ? help cue plus state-naming tooltip/help text` in one sentence.
- Code and tests show the visible `?` cue and `Status state:` tooltip/help text belong to Review Grid Mode Status, while Main Grid Active Review Lens Summary has tooltip/help text only and hides for current-session quarantined rows.

Implementation:

- Clarified checklist step 7 so Review Grid Mode Status owns the hoverable `?` cue and state-naming tooltip/help text.
- Clarified that Main Grid Active Review Lens Summary appears when Storage Scan rows are showing and hides for current-session quarantined rows.
- Added a focused feature brief and recorded the wording alignment in the fixture checklist feature trail.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the clarified step 7 without preflight, fixture creation, WPF launch, scan, move, restore, delete, or cleanup history.
- `rg -n "Main Grid active review lens summary when scan rows are showing, and its hoverable|Main Grid active review lens summary.*and its hoverable" tools\Start-MvpFixtureReview.ps1 README.md docs\codex\thread-handoff.md` found no remaining copy of the old ambiguous checklist wording in the active checklist/handoff surfaces.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-06-01-fixture-checklist-grid-mode-cue-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is fixture checklist wording only, with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision.

Open questions:

- During the next visible fixture pass, confirm whether Review Grid Mode Status plus Main Grid Active Review Lens Summary are clear enough together.

### 2026-06-01: Header Layout Visual Approval Notes

Status: completed

Evidence:

- User visually reviewed the latest wide WPF header and said it looks good.
- The screenshot showed Review Shortlist size/folder/file totals positioned before scan totals, with the Cleanup Scope path and Scan controls still readable.
- Some manual-review wording still said the Main Grid active-lens summary appeared `above the rows`, which is imprecise because Main Grid can also show current-session quarantined rows.

Implementation:

- Recorded the user-approved wide-header direction in the handoff, progress, and relevant feature briefs.
- Tightened README and feature wording so Main Grid Active Review Lens Summary is scoped to `above Storage Scan rows` and hides for current-session quarantined rows.
- Kept remaining manual fixture review focused on Quarantine/readiness and selected-restore flow instead of re-reviewing the already approved wide header layout.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the existing fixture checklist without preflight, fixture creation, WPF launch, scan, move, restore, delete, or cleanup history.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-31-wpf-header-shortlist-metrics.md`
- `docs/features/2026-05-31-wpf-tabbed-workbench.md`
- `docs/features/2026-06-01-main-grid-auto-focus.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is docs/handoff alignment from user visual feedback with no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision.

Open questions:

- A narrower-window fixture pass may still be useful if header metric wrapping feels tight.
- The next visible fixture pass should focus on Quarantine/readiness and selected-restore flow rather than the already approved wide header layout.

### 2026-06-01: Full Local MVP Preflight After Active Lens

Status: completed

Evidence:

- The Main Grid Active Review Lens Summary packet touched WPF layout, WPF smoke assertions, fixture checklist wording, and durable docs.
- The latest full `.cmd` MVP preflight evidence was still from the earlier header-polish state.
- A full local preflight gives stronger current evidence before the next manual fixture visual pass or any later real-profile retest.

Implementation:

- Ran the full `.cmd` MVP preflight after the Main Grid Active Review Lens Summary packet was pushed.
- Recorded the updated full-preflight evidence in project handoff/progress docs.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
  - Restore passed.
  - Build passed with 0 warnings and 0 errors.
  - Core tests passed.
  - WPF app tests passed.
  - Fixture dry run passed with `-WhatIf`.
  - Fixture checklist-only output passed and includes the latest Main Grid active-lens wording.
  - Whitespace diff check passed.
  - The preflight reported that no real user files were scanned or modified.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only; no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- A visible manual fixture pass is still useful to judge final layout feel, but the automated/full preflight evidence is current.

### 2026-06-01: Main Grid Active Review Lens Summary

Status: completed

Evidence:

- User liked the cleaner tabbed/header layout after several visual polish packets.
- Main Grid auto-focus made shortcut results visible, but the active review lens details still lived mainly on the Review tab.
- A compact row-level orientation line can reduce tab bouncing without changing filters, search, shortlist, preview, or execution behavior.

Implementation:

- Added compact `MainGridReviewLensText` above Storage Scan rows.
- Mirrored the existing Filter Summary into Main Grid so default scan, Safety Summary shortcut, no-category shortcut, and stacked shortcut/search states are visible beside the rows.
- Added tooltip and automation help text that keep the readout scoped to completed Storage Scan filters/search/focus and not rescan, file modification, or cleanup approval.
- Hid the readout while current-session quarantined rows are showing because those rows are not driven by Storage Scan filters/search/focus.
- Kept Review Grid Mode Status responsible for row-source wording and kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-31-wpf-tabbed-workbench.md`
- `docs/features/2026-06-01-main-grid-auto-focus.md`
- `docs/features/2026-06-01-main-grid-active-review-lens-summary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF orientation text with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- During visible fixture review, is the extra Main Grid line helpful enough to keep, or should it be visually tighter?

### 2026-06-01: Main Grid Auto-Focus for Grid-Switching Actions

Status: completed

Evidence:

- The tabbed workbench gave Safety Summary, Review, Quarantine, and Main Grid separate pages.
- Safety Summary shortcuts and current-session quarantined grid switches changed which rows the Main Grid showed, but without auto-focus the user could remain on Safety Summary or Quarantine and miss the changed rows.
- The open WPF tabbed-workbench question asked whether Safety Summary shortcuts or Quarantine actions should switch back to Main Grid afterward.

Implementation:

- Named the WPF workbench tab items and added a narrow Main Grid selection helper.
- Safety Summary shortcuts now select Main Grid after applying their read-only review lens.
- `Current quarantined` and `Back to scan rows` now select Main Grid after switching the visible row set.
- Kept ordinary Review-tab search/filter controls from auto-switching while a review lens is being set up.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-31-wpf-tabbed-workbench.md`
- `docs/features/2026-06-01-main-grid-auto-focus.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF navigation polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Should ordinary Review-tab filters remain on the Review tab after changing the row lens, or should some of them also auto-focus Main Grid after visible fixture review?

Rejected ideas buffer:

- Do not auto-switch tabs on every search keystroke or filter adjustment unless visible fixture review shows the extra movement is helpful.

### 2026-06-01: Full Local MVP Preflight After Header Polish

Status: completed

Evidence:

- Several WPF header and tabbed-workbench polish packets had passed narrow WPF/checklist/diff checks, while the latest full `.cmd` MVP preflight evidence was still from an earlier readiness-label packet.
- The current pushed UI state needed broad local verification before more live-product work.

Implementation:

- Ran the full `.cmd` MVP preflight after the WPF Header Shortlist Metric Placement packet.
- Recorded the updated full-preflight evidence in project handoff/progress docs.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
  - Restore passed.
  - Build passed.
  - Core tests passed.
  - WPF app tests passed.
  - Fixture dry run passed with `-WhatIf`.
  - Fixture checklist-only output passed and includes the latest header shortlist-placement wording.
  - Whitespace diff check passed.

Docs updated:

- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs:

- No ADR added. This is verification evidence only; no architecture, persistence, cleanup execution, restore behavior, data-model, or security decision changed.

Open questions:

- A visible manual fixture pass is still useful to judge final layout feel, but the automated/full preflight evidence is current.

### 2026-05-31: WPF Header Shortlist Metrics

Status: completed

Evidence:

- User asked to add another compact wrapping header strip near the scan totals that shows how many files/folders are in the Review Shortlist and the shortlist size.
- The Review tab already had detailed Shortlist Safety Mix text, but shortlist scope was not visible from the header while using other tabs.

Implementation:

- Added compact header Review Shortlist metrics for shortlist size, shortlisted folder rows, and shortlisted file rows.
- Populated the metrics from the existing in-memory `StorageReviewShortlist` applied to the current scan review.
- Added tooltip and automation help text that keeps row-size totals read-only, not storage savings, not cleanup approval, and no-file-modified.
- Added WPF smoke coverage for the wrapping header layout, empty startup totals, populated file-row totals, tooltip boundary text, and reset-after-removal behavior.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-31-wpf-header-shortlist-metrics.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF layout/readout polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Should the header metric strips be tightened further for narrower windows after visible fixture review?
- Should selecting safety/review shortcuts or Quarantine actions automatically switch to Main Grid?

Rejected ideas buffer:

- Do not treat Review Shortlist size as storage savings; parent and child row sizes can overlap.

### 2026-05-31: WPF Header Shortlist Metric Placement

Status: completed

Evidence:

- User asked whether the new Review Shortlist boxes could be positioned to the left side of `Total size` by default when possible.
- The previous layout stacked Review Shortlist totals beneath the scan totals, which used extra vertical space and separated related header metrics.

Implementation:

- Reordered the header metric area so the Review Shortlist metric strip appears before the scan-total strip.
- Kept both metric strips compact and wrapping so narrower windows can still wrap the groups.
- Added WPF smoke coverage proving Review Shortlist totals are ordered before scan totals.
- Kept Storage Scan, Review Shortlist membership behavior, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-31-wpf-header-shortlist-metrics.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF layout polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Should the header metric strips be tightened further for narrower windows after visible fixture review?

### 2026-05-31: WPF Header Scan Metrics

Status: completed

Evidence:

- User confirmed the tabbed workbench looked much better, then noted the Scan tab was sparse and asked to fit those totals into the header space to the left of the directory box.
- The previous Scan tab contained only total size, folders, files, and access issues.

Implementation:

- Added a compact wrapping header metric strip for total size, folders, files, and access issues.
- Removed the sparse Scan tab so the workbench now contains Safety Summary, Review, Quarantine, and Main Grid.
- Kept Main Grid selected by default and preserved the existing scan metric text updates.
- Added WPF smoke coverage for the compact header metric layout and updated the tab-header assertion.
- Kept Storage Scan, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-31-wpf-header-scan-metrics.md`
- `docs/features/2026-05-31-wpf-tabbed-workbench.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF layout polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Should header metrics be tightened further for narrower windows after visible fixture review?
- Should selecting safety/review shortcuts or Quarantine actions automatically switch to Main Grid?

Rejected ideas buffer:

- Do not restore the sparse Scan tab unless future visible testing shows header metrics reduce clarity.

### 2026-05-31: WPF Tabbed Workbench

Status: completed

Evidence:

- User confirmed the panel cleanup looked better, then asked for individual horizontal tabs/sections for Scan, Review, Quarantine, Safety Summary, and possibly the main grid so each section has its own page.
- The previous stacked layout still put large safety/readiness surfaces near the main grid.

Implementation:

- Added a horizontal `WorkbenchTabs` WPF tab control below the global Cleanup Scope and scan-gate header.
- Moved scan metrics to a Scan tab, review filters/shortlist controls to a Review tab, Quarantine controls/readiness/gate output to a Quarantine tab, Safety Summary to its own tab, and the Storage Scan / Current-Session Quarantined Review grids plus selected-path detail to a Main Grid tab.
- Kept Main Grid selected by default so scan rows retain a full-width review page.
- Kept Safety Summary and Quarantine Shortlist expanders available but expanded by default inside their dedicated tabs.
- Added WPF smoke coverage for the tab headers, default selected tab, and expanded-by-default tab-page sections.
- Kept Storage Scan, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-31-wpf-tabbed-workbench.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF navigation/layout polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Should the tab order change after visible review?
- Should selecting safety/review shortcuts or Quarantine actions automatically switch to Main Grid?

Rejected ideas buffer:

- Do not use tab navigation as a reason to hide real-profile/custom movement blockers; blockers remain visible in Quarantine/readiness/gate output and through tooltips/help cues.

### 2026-05-31: WPF Panel Cleanup

Status: completed

Evidence:

- User reported that the WPF UI looks messy and suggested panels.
- The current surface exposed scan controls, summary cards, safety text, review filters, shortlist controls, Quarantine readiness, grid mode, grid rows, and selected-path detail in one dense view.

Implementation:

- Added a shared WPF panel border style and section-heading style.
- Put the scan header, metric cards, review filters/shortlist controls, Quarantine Shortlist, and selected-path detail area into quieter consistent panels.
- Added a `Review filters and shortlist` panel label.
- Set Safety Summary and Quarantine Shortlist to start collapsed, preserving their panel-name header summaries, state styling, tooltips, automation help text, and hoverable `?` help cues.
- Updated WPF smoke coverage for the new collapsed-by-default startup behavior.
- Kept Storage Scan, Quarantine Preview, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-31-wpf-panel-cleanup.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF layout polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Open questions:

- Should Safety Summary or Quarantine Shortlist auto-expand after scan/shortlist changes, or is manual expansion clearer?

Rejected ideas buffer:

- Do not use this panel cleanup as a reason to hide real-profile/custom movement blockers; the blockers remain visible through headers, summaries, tooltips, and expanded details.

### 2026-05-31: Fixture Checklist Fixture-First Boundary

Status: completed

Evidence:

- The printed fixture checklist's final real-profile/custom blocker item named exact real-profile readiness evidence and could read like part of the fixture visual pass.
- The next manual fixture review should stay fixture-first and should not require scanning `C:\Users\moxhe` just to satisfy that checklist item.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist step 10 to say the final real-profile/custom blocker check should be done without scanning `C:\Users\moxhe` as part of the fixture pass.
- Updated README fixture-smoke and manual-review wording with the same fixture-first boundary.
- Added a feature brief for the checklist clarification.
- Kept WPF behavior, Storage Scan, fixture execution, selected restore, real-profile/custom movement blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-31-fixture-checklist-fixture-first-boundary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is checklist and README clarification only; it does not change architecture, persistence, cleanup execution, restore behavior, or safety policy.

Open questions:

- None for this checklist clarification.

Rejected ideas buffer:

- Do not make the fixture visual pass depend on scanning `C:\Users\moxhe`; separate real-profile retests still require MVP preflight and explicit user intent.

### 2026-05-31: Fresh Thread Handoff Polish After Readiness Labels

Status: completed

Evidence:

- `docs/codex/thread-handoff.md` already described the current readiness-label safety boundary, but the embedded startup prompt did not clearly separate the latest checklist-only evidence from the latest full `.cmd` MVP preflight evidence.
- The next thread also needs the current 19-cue snapshot, non-`D:` acknowledgement user verification, and Codex desktop action-link issue without expanding `AGENTS.md`.

Implementation:

- Refreshed `docs/codex/thread-handoff.md` so the latest completed packet names this handoff polish and the startup prompt starts from the current pushed safety boundary.
- Updated the handoff best-next-work and embedded startup prompt to distinguish checklist-only verification after Fixture Checklist Preview-Only Key Labels from full `.cmd` MVP preflight after Quarantine Readiness Key-Blocker Label Rules.
- Added a short Codex desktop tooling note to the startup prompt so the next thread can rely on terminal output if action-link approval UI errors recur.
- Updated this progress log with the handoff polish packet.
- No WPF behavior, README wording, `AGENTS.md`, fixture tooling, scan behavior, Quarantine execution, restore behavior, deletion behavior, or cleanup history changed.

Verification:

- `git diff --check`

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is handoff/progress documentation polish only; it does not change architecture, persistence, cleanup execution, restore behavior, or safety policy.

Open questions:

- None for this handoff packet.

Rejected ideas buffer:

- Do not expand `AGENTS.md`; keep fresh-thread detail in `docs/codex/thread-handoff.md`.

### 2026-05-31: Fixture Checklist Preview-Only Key Labels

Status: completed

Evidence:

- WPF smoke coverage now asserts specific compact Quarantine Readiness Summary labels for custom preview-only, exact real-profile revalidation/restore/current-build blockers, real-profile-child exact-scope blockers, and first-phase row/size/category/descendant blockers.
- The printed fixture checklist and README manual review wording still asked for generic `Key blockers` labels.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist step 6 to name `custom scope preview-only`, `exact profile scope`, `pre-execution revalidation`, `restore readiness`, `current build unavailable`, `10-row cap`, `1 GB cap`, `no-category rows`, and `strict descendant checks` as labels to inspect when those blockers apply.
- Updated README fixture-smoke and manual-review wording with the same label examples.
- Updated the fixture checklist key-blocker feature brief to record the later specific-label refinement.
- No WPF behavior changed; no real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-31-fixture-checklist-readiness-key-blockers.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is checklist wording alignment for accepted ADR 0018 and existing WPF output.

Open questions:

- None for this checklist packet.

Rejected ideas buffer:

- Do not treat checklist wording as visual verification; it only makes the next manual fixture pass more precise.

### 2026-05-31: Preview-Only Key-Blocker Summary Coverage

Status: completed

Evidence:

- Exact real-profile first-phase WPF coverage already asserted compact `Key blockers:` labels for row cap, byte cap, no-category rows, and strict descendant checks.
- Custom non-fixture, exact real-profile revalidation/restore/current-build, and real-profile-child exact-scope summary labels were not asserted as directly.

Implementation:

- Added WPF smoke assertions that custom non-fixture preview-only summaries include `Key blockers:` and `custom scope preview-only`.
- Added WPF smoke assertions that exact real-profile preview-only summaries include `pre-execution revalidation`, `restore readiness`, and `current build unavailable` key labels.
- Added WPF smoke assertions that real-profile-child preview-only summaries include `exact profile scope`.
- Kept WPF behavior, fixture-only execution, real-profile/custom movement blockers, restore availability, permanent deletion, and cleanup history unchanged.
- No real user files were scanned or modified.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

Docs updated:

- `docs/features/2026-05-31-preview-only-key-blocker-summary-coverage.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is regression coverage for accepted ADR 0018 preview-only readiness wording.

Open questions:

- None.

Rejected ideas buffer:

- Do not treat summary label coverage as manual visual verification; it only proves the WPF text contract.

### 2026-05-31: Full Local MVP Preflight After Label Rules

Status: completed

Evidence:

- The previous packet changed WPF compact Quarantine Readiness Summary internals by consolidating key-blocker label rules while preserving visible wording.
- A full local preflight verifies restore/build/test/checklist behavior through the same `.cmd` wrapper used before real-profile scans.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` from the repository root.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist, and ran whitespace diff checking.
- Checklist output still included compact Quarantine Readiness Summary `Key blockers` wording, ADR 0018 first-phase limits, and read-only Real-Profile Quarantine Approval Evidence expectations.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only.

Open questions:

- None.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates and checklist output only.

### 2026-05-31: Quarantine Readiness Key-Blocker Label Rules

Status: completed

Evidence:

- The compact Quarantine Readiness Summary key-blocker mapping existed once as a priority pattern array and again as a separate fallback `if` chain.
- The duplicated mapping made future ADR 0018 readiness wording easier to drift while the current WPF tests already cover visible key-blocker labels.

Implementation:

- Added a shared `ReadinessKeyBlockerLabelRules` table in `MainWindow.xaml.cs`.
- Updated priority selection and fallback label formatting to use the shared rules.
- Kept visible WPF output, first-phase key-blocker priority, fixture-only execution, and real-profile/custom preview-only blockers unchanged.
- No real user files were scanned or modified.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

Docs updated:

- `docs/features/2026-05-31-quarantine-readiness-key-blocker-label-rules.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is behavior-preserving maintainability cleanup under accepted ADR 0018.

Open questions:

- None.

Rejected ideas buffer:

- Do not change visible readiness wording or real-profile movement availability in this maintainability packet.

### 2026-05-31: Fixture Checklist Readiness Key Blockers

Status: completed

Evidence:

- WPF compact Quarantine Readiness Summary now includes `Key blockers:` labels for preview-only scopes.
- The printed fixture checklist still asked for preview-only `movement unavailable` wording but did not tell reviewers to inspect those key-blocker labels.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist step 6 to include compact Quarantine Readiness Summary key-blocker labels.
- Updated README fixture-review and manual-check wording with the same expectation.
- Updated the key-blocker feature brief to record the checklist alignment.
- No WPF behavior changed; no real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-31-fixture-checklist-readiness-key-blockers.md`
- `docs/features/2026-05-31-quarantine-readiness-summary-key-blockers.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is checklist wording alignment for accepted ADR 0018 and existing WPF output.

Open questions:

- None for this checklist packet.

Rejected ideas buffer:

- Do not treat checklist wording as visual verification; it only makes the next manual fixture pass clearer.

### 2026-05-31: Full Local MVP Preflight After Key Blockers

Status: completed

Evidence:

- The previous packet changed WPF compact Quarantine Readiness Summary wording and WPF smoke coverage for ADR 0018 key-blocker labels.
- A full local preflight verifies restore/build/test/checklist behavior through the same `.cmd` wrapper used before real-profile scans.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` from the repository root.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist, and ran whitespace diff checking.
- Checklist output included the final real-profile/custom prompt with ADR 0018 first-phase limits: 10 rows, 1 GB, no-category, and strict descendant checks.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only.

Open questions:

- None.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates and checklist output only.

### 2026-05-31: Quarantine Readiness Summary Key Blockers

Status: completed

Evidence:

- Detailed preview/gate output keeps representative first-phase blockers visible, but the compact Quarantine Readiness Summary only named broad missing dimensions such as Review readiness.
- Manual review benefits from seeing the most relevant blocker examples before reading the full gate text.

Implementation:

- Added `Key blockers:` wording to preview-only compact Quarantine Readiness Summary output.
- Prioritized concise ADR 0018 first-phase labels such as `10-row cap`, `1 GB cap`, `no-category rows`, and `strict descendant checks` before generic current-build-unavailable wording.
- Extended synthetic exact real-profile WPF smoke coverage to assert those labels in the compact summary while `CanExecuteQuarantine` remains false.
- No real-profile files were scanned, created, moved, restored, deleted, or modified.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-31-quarantine-readiness-summary.md`
- `docs/features/2026-05-31-quarantine-readiness-summary-key-blockers.md`
- `docs/features/2026-05-31-wpf-real-profile-first-phase-blocker-output.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF readability polish under accepted ADR 0018.

Open questions:

- Manual review can still decide whether the compact summary is enough or whether a dedicated readiness pane would be clearer later.

Rejected ideas buffer:

- Do not replace the detailed Quarantine Preview/Gate blockers with the key-blocker summary; it is only a compact orientation line.

### 2026-05-31: Fixture Checklist First-Phase Blocker Alignment

Status: completed

Evidence:

- WPF now keeps ADR 0018 first-phase real-profile blockers visible before long readiness lists are truncated.
- The manual fixture checklist still named ADR 0017/0019 and approval evidence but did not call out those first-phase limits.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist step 10 to include ADR 0018 first-phase limits: 10 rows, 1 GB, no-category rows, and strict descendant checks.
- Updated README fixture-review and manual-check wording with the same expectation.
- Updated the first-phase blocker feature brief to record the checklist alignment.
- No WPF behavior changed; no real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-31-fixture-checklist-first-phase-blockers.md`
- `docs/features/2026-05-31-wpf-real-profile-first-phase-blocker-output.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is checklist wording alignment for accepted ADR 0018 and existing WPF output.

Open questions:

- None for this checklist packet.

Rejected ideas buffer:

- Do not make checklist-only mode launch WPF or synthesize real-profile blocker cases; automated WPF coverage already proves the synthetic output path.

### 2026-05-31: Full Local MVP Preflight After First-Phase Blockers

Status: completed

Evidence:

- The previous packet changed WPF readiness blocker display selection and added synthetic exact real-profile WPF coverage for ADR 0018 first-phase blockers.
- A full local preflight verifies restore/build/test/checklist behavior through the same `.cmd` wrapper used before real-profile scans.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` from the repository root.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist, and ran whitespace diff checking.
- Checklist output still included the real-profile/custom blocker prompt with read-only Real-Profile Quarantine Approval Evidence and `Can approve real-profile movement: no`.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only.

Open questions:

- None.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates and checklist output only.

### 2026-05-31: WPF Real-Profile First-Phase Blocker Output

Status: completed

Evidence:

- ADR 0018 first-phase real-profile blockers can be hidden in WPF output when many stale-source revalidation blockers exist and the pane truncates detailed readiness lines.
- The app needs visible evidence that batch limits, no-category rows, and narrow-folder strict descendant checks are still part of the real-profile boundary.

Implementation:

- Added representative readiness-blocker selection before WPF preview/gate output truncates detailed blocker lines.
- Added synthetic WPF coverage for exact `C:\Users\moxhe` scan metadata with 11 included rows, an oversized row, a no-category row, and a folder with a no-category descendant.
- The test asserts preview/gate output keeps row-cap, byte-cap, no-category, strict-descendant, exact-confirmation, and approval-evidence blockers visible while `CanExecuteQuarantine` remains false.
- No real-profile files were scanned, created, moved, restored, deleted, or modified.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

Docs updated:

- `README.md`
- `docs/features/2026-05-31-wpf-execution-readiness-output.md`
- `docs/features/2026-05-31-wpf-real-profile-first-phase-blocker-output.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is visible output and regression coverage for accepted ADR 0018.

Open questions:

- Manual review can still decide whether the existing WPF pane is enough or whether a dedicated readiness pane is worth adding later.

Rejected ideas buffer:

- Do not treat representative blocker display as real-profile movement approval; it is still read-only evidence only.

### 2026-05-31: Full Local MVP Preflight After Manifest Summary

Status: completed

Evidence:

- The previous packet added a WPF Restore Manifest review summary and updated fixture checklist wording.
- A full local preflight verifies that the app still restores/builds/tests and that the checklist output includes the new summary prompt.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` from the repository root.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist, and ran whitespace diff checking.
- Checklist output included `Restore Manifest review summary states/tooltips`.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only.

Open questions:

- None.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates and checklist output only.

### 2026-05-31: Restore Manifest Review Summary

Status: completed

Evidence:

- Manifest discovery, selected-manifest readiness, all-manifest readiness, and selected restore gate output can be hard to scan together in the details pane.
- Manual fixture review benefits from one compact read-only state line before the detailed panes.

Implementation:

- Added `RestoreManifestReviewSummaryText` under the Restore Manifest controls.
- The summary updates after discovery, selected readiness, selected restore gate preview, exact `RESTORE`, all-manifest readiness preview, and selected fixture restore result.
- The summary uses neutral/information/success/warning styling and mirrors `Summary state:` plus no-create/no-move/no-restore/no-delete/no-manifest-write/no-cleanup-folder/not-restore-approval boundaries into tooltip and automation help text.
- No real-profile restore, real-profile Quarantine, all-manifest restore, permanent deletion, or cleanup history behavior changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-31-restore-manifest-review-summary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF readability polish under ADR 0013, ADR 0015, ADR 0016, and ADR 0019.

Open questions:

- Manual fixture review should confirm whether the compact summary makes manifest discovery/readiness state easier to scan in the details pane.

Rejected ideas buffer:

- Do not use the summary as restore approval or as a replacement for the detailed discovery/readiness/gate panes.

### 2026-05-31: WPF Approval Evidence Display-Only Guard

Status: completed

Evidence:

- WPF now shows Real-Profile Quarantine Approval Evidence in the Quarantine Execution Gate.
- That evidence must not become a second execution gate or make real-profile movement available before an explicit later execution packet.

Implementation:

- Added `WpfRealProfileApprovalEvidenceStaysDisplayOnly` to the core test harness.
- The guard checks that WPF builds approval evidence for Quarantine Execution Gate display, leaves current-build movement availability at the default unavailable value, skips fixture scopes, and keeps `CanApproveForRealProfileMovement` inside display formatting only.
- The guard verifies `ExecuteQuarantineButton.IsEnabled` remains based on `_currentQuarantineExecutionGate.CanExecute`, not approval evidence.
- Broadened the local source-method parser to recognize `private static void` methods so display-formatting source guards can be precise.
- No app behavior changed; no real-profile files were scanned, moved, restored, deleted, created, or rewritten.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-31-wpf-approval-evidence-display-only-guard.md`
- `docs/features/2026-05-31-wpf-real-profile-approval-evidence-output.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is regression coverage for accepted ADR 0018 and existing WPF output.

Open questions:

- None for this guard packet.

Rejected ideas buffer:

- Do not wire `RealProfileQuarantineApprovalEvidence.CanApproveForRealProfileMovement` into WPF execution availability unless a later explicit user-approved real-profile execution packet updates the guard, tests, and safety docs.

### 2026-05-31: Full Local MVP Preflight After Approval Evidence Checklist

Status: completed

Evidence:

- The previous packet aligned the manual fixture checklist with WPF Real-Profile Quarantine Approval Evidence output.
- A full local preflight verifies that updated checklist text through the same `.cmd` wrapper used in local review.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` from the repository root.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist, and ran whitespace diff checking.
- Checklist output included `Real-Profile Quarantine Approval Evidence with Can approve real-profile movement: no`.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only.

Open questions:

- None.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates and checklist output only.

### 2026-05-31: Fixture Checklist Approval Evidence Alignment

Status: completed

Evidence:

- The WPF Quarantine Execution Gate now shows read-only Real-Profile Quarantine Approval Evidence for non-fixture preview-only scopes.
- The fixture checklist still asked only for ADR 0017/0019 blockers, not the new visible `Can approve real-profile movement: no` evidence.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist step 10 to ask reviewers to confirm the gate shows read-only Real-Profile Quarantine Approval Evidence with `Can approve real-profile movement: no`.
- Updated README fixture review and manual check wording with the same expectation.
- Updated the WPF approval-evidence output feature brief to record the checklist alignment.
- No app behavior changed; no real-profile files were scanned, moved, restored, deleted, created, or rewritten.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-31-fixture-checklist-approval-evidence.md`
- `docs/features/2026-05-31-wpf-real-profile-approval-evidence-output.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is checklist/manual-review wording for accepted ADR 0018 and existing WPF output.

Open questions:

- None for this checklist packet.

Rejected ideas buffer:

- Do not make checklist-only mode inspect WPF output; it should continue to print review prompts only.

### 2026-05-31: WPF Real-Profile Approval Evidence Output

Status: completed

Evidence:

- The core Real-Profile Quarantine Approval Evidence model existed, but WPF still relied on readiness/gate text alone to explain why exact `QUARANTINE` was not enough.
- The Quarantine Execution Gate refreshes as confirmation text changes, making it the right first visible surface for approval evidence.

Implementation:

- WPF now builds `RealProfileQuarantineApprovalEvidence` for non-fixture scopes when the Quarantine Execution Gate refreshes.
- The gate output shows exact confirmation match, exact real-profile scope evidence, readiness blocker presence, current-build movement availability, and whether real-profile movement can be approved.
- The boundary line says exact `QUARANTINE` is necessary but not sufficient and that the evidence does not create folders, move files, restore files, delete files, write manifests, persist approval, or approve cleanup.
- Fixture scopes skip this real-profile-only block so the fixture execution path stays focused.
- No execution availability changed; no real-profile files were scanned, moved, restored, deleted, created, or rewritten.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-31-real-profile-quarantine-approval-evidence.md`
- `docs/features/2026-05-31-wpf-execution-readiness-output.md`
- `docs/features/2026-05-31-wpf-real-profile-approval-evidence-output.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible WPF visibility packet under accepted ADR 0018.

Open questions:

- None for this output packet.

Rejected ideas buffer:

- Do not add a new WPF panel for approval evidence until manual review proves the existing gate text is insufficient.

### 2026-05-31: Real-Profile Quarantine Approval Evidence

Status: completed

Evidence:

- ADR 0018 requires exact `QUARANTINE` to remain necessary but insufficient before real-profile movement.
- Real-profile readiness already names readiness blockers, but the approval seam did not have a dedicated read-only core model.

Implementation:

- Added `RealProfileQuarantineApprovalEvidence` and `RealProfileQuarantineApprovalEvidenceBuilder`.
- The builder records checked time, exact confirmation matching, exact real-profile scope evidence, readiness blockers, and current-build movement availability.
- Current-build movement availability defaults to unavailable, so clean readiness plus exact `QUARANTINE` still does not approve movement.
- Added the builder to the source guard that prevents read-only readiness builders from calling movement executors, writing Restore Manifests, or performing direct filesystem movement/write operations.
- No WPF controls or execution availability changed; no real-profile files were scanned, moved, restored, deleted, created, or rewritten.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/features/2026-05-31-real-profile-quarantine-approval-evidence.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This implements an accepted ADR 0018 follow-up without changing the durable decision.

Open questions:

- None for this core-only packet.

Rejected ideas buffer:

- Do not surface approval evidence in WPF as movement approval until a later user-approved execution packet wires real-profile movement.

### 2026-05-31: Full Local MVP Preflight After Movement Summary Checklist

Status: completed

Evidence:

- The previous packet changed fixture checklist and README wording for the compact Quarantine Readiness Summary preview-only `movement unavailable` prompt.
- The full local preflight verifies that updated checklist text through the same `.cmd` wrapper used in local review.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` from the repository root.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist, and ran whitespace diff checking.
- Checklist output included `compact Quarantine Readiness Summary states/tooltips including preview-only movement unavailable wording`.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only.

Open questions:

- None.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates and checklist output only.

### 2026-05-31: Fixture Checklist Movement-Unavailable Summary Alignment

Status: completed

Evidence:

- The WPF compact Quarantine Readiness Summary now uses `movement unavailable` for preview-only scopes.
- WPF smoke coverage proves the wording for custom, exact synthetic real-profile, and synthetic real-profile-child scopes.
- The manual fixture checklist still mentioned generic Quarantine Readiness Summary states/tooltips without naming the new visible wording.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist step 6 to ask reviewers to check preview-only `movement unavailable` wording in the compact Quarantine Readiness Summary.
- Updated README fixture launcher and manual review wording with the same visible wording expectation.
- Updated the preview-only summary wording feature brief.
- No WPF behavior changed; no real-profile files were scanned, moved, restored, deleted, created, or rewritten.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/features/2026-05-31-quarantine-readiness-summary-preview-only-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is manual-review wording alignment with existing WPF behavior.

Open questions:

- None for this checklist packet.

Rejected ideas buffer:

- Do not make checklist-only mode perform visual verification; it should continue to print prompts only.

### 2026-05-31: Real-Profile Child Readiness Summary Wording Coverage

Status: completed

Evidence:

- The prior wording packet updated compact preview-only summary text and covered custom and exact synthetic real-profile summaries.
- The synthetic real-profile-child readiness test already covered preview/gate detailed readiness output, but did not assert the compact summary line.

Implementation:

- Added WPF smoke assertions that synthetic `C:\Users\moxhe\AppData\Local` preview-only readiness summary says preview-only, names `real-profile child`, says `movement unavailable`, lists `Missing: Scope and policy`, uses warning styling, and mirrors the state into tooltip/help text.
- Updated the preview-only summary wording feature brief to include real-profile-child coverage.
- No app behavior changed; no real-profile files were scanned, moved, restored, deleted, created, or rewritten.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-quarantine-readiness-summary-preview-only-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is WPF smoke coverage for existing preview-only wording and ADR 0018 exact-scope behavior.

Open questions:

- None for this coverage packet.

Rejected ideas buffer:

- Do not assume exact real-profile summary coverage also proves child-scope summary wording; keep child-scope blockers explicit.

### 2026-05-31: Quarantine Readiness Summary Preview-Only Wording

Status: completed

Evidence:

- The compact Quarantine Readiness Summary is the most visible readiness line near the confirmation controls.
- For preview-only scopes, it used the technically accurate but implementation-shaped `Current build can execute: no` phrase.

Implementation:

- Changed preview-only compact summary wording to end with `movement unavailable`.
- Kept the detailed Quarantine Preview and Quarantine Execution Gate readiness contract wording unchanged, including `Current build can execute from this readiness model: no`.
- Updated WPF smoke assertions for custom and synthetic real-profile preview-only summaries.
- No execution availability changed; no real-profile files were scanned, moved, restored, deleted, created, or rewritten.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-quarantine-readiness-summary-preview-only-wording.md`
- `docs/features/2026-05-31-quarantine-readiness-summary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF wording polish under existing ADR 0017/0018 boundaries.

Open questions:

- Manual fixture review should confirm whether the compact summary is now clear enough without a dedicated readiness pane.

Rejected ideas buffer:

- Do not remove detailed readiness contract wording from the gate; the compact summary should be simpler while the detailed pane stays auditable.

### 2026-05-31: Full Local MVP Preflight After Source Guards

Status: completed

Evidence:

- The previous packets added source-level guards around read-only readiness builders and WPF movement executor bridge methods.
- A full local preflight gives stronger evidence than the narrow core test run before future manual fixture or real-profile scan review.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` from the repository root.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist, and ran whitespace diff checking.
- Checklist output still included selected restore revalidation evidence wording and the final ADR 0017/0019 real-profile/custom blocker check.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only.

Open questions:

- None.

Rejected ideas buffer:

- Do not treat source guards alone as enough before a later real-profile scan review; keep using full preflight after worktree changes.

### 2026-05-31: WPF Execution Bridge Source Guard

Status: completed

Evidence:

- WPF smoke tests already prove real/custom buttons stay disabled, but the actual UI bridge to movement executors is in `MainWindow.xaml.cs`.
- A source-level guard makes future accidental executor wiring easier to catch before it can bypass the fixture-only boundary.

Implementation:

- Added `WpfExecutionBridgeKeepsExecutorCallsInGatedMethods` to the core test harness.
- The guard scans `MainWindow.xaml.cs` and verifies that `QuarantineExecutor.Execute` and `UndoQuarantineExecutor.Undo` appear only in `ExecuteQuarantineForCurrentPreview`, `ExecuteSelectedRestoreForCurrentSelection`, and `UndoQuarantineForCurrentExecution`.
- The guard verifies forward Quarantine remains behind the current execution gate and Restore Manifest guard, selected restore remains behind the selected restore gate and current discovery lookup, current-fixture undo remains behind `CanUndoCurrentQuarantineExecution`, and forward/selected restore availability remains fixture-scope based.
- No app behavior changed; no real-profile files were scanned, moved, restored, deleted, created, or rewritten.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-wpf-execution-bridge-source-guard.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is regression coverage for existing ADR 0017/0018/0019 safety boundaries.

Open questions:

- None for this guard packet.

Rejected ideas buffer:

- Do not add new WPF executor calls in future packets unless an explicit user-approved execution packet updates this guard and the ADR-backed safety contract.

### 2026-05-31: Read-Only Readiness Builder Guard

Status: completed

Evidence:

- The existing production source guard allowed only known filesystem write locations, but did not directly guard read-only readiness/revalidation builders against calling execution components.
- ADR 0017, ADR 0018, and ADR 0019 depend on readiness evidence staying separate from real-profile movement until a later explicit execution packet.

Implementation:

- Added `ReadOnlyReadinessBuildersDoNotCallExecutionComponents` to the core test harness.
- The guard scans read-only readiness and revalidation builders for calls to `QuarantineExecutor.Execute`, `UndoQuarantineExecutor.Undo`, `RestoreManifestFileStore.Write`, and direct filesystem create/move/delete/write tokens.
- Covered `QuarantineExecutionReadinessBuilder`, `QuarantineRootExecutionSafetyBuilder`, `PreExecutionRevalidationBuilder`, `RealProfileRestoreReadinessBuilder`, `SelectedRestorePreExecutionRevalidationBuilder`, `RestoreReadinessPreviewBuilder`, and `SelectedRestoreManifestReviewBuilder`.
- No app behavior changed; no real-profile files were scanned, moved, restored, deleted, created, or rewritten.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-read-only-readiness-builder-guard.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is regression coverage for existing ADR 0017/0018/0019 boundaries.

Open questions:

- None for this guard packet.

Rejected ideas buffer:

- Do not let future read-only readiness builders invoke movement executors directly; wire execution through an explicit user-approved execution packet instead.

### 2026-05-31: Fixture Checklist Selected Restore Revalidation Alignment

Status: completed

Evidence:

- The WPF selected restore gate now shows read-only Selected Restore Pre-Execution Revalidation evidence for exact real-profile selected Restore Manifests.
- The terminal checklist and README manual review text still named selected restore gate states but did not mention the new revalidation evidence.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist step 9 to include read-only selected restore revalidation evidence for exact real-profile Restore Manifests when available.
- Updated checklist step 10 to make clear that exact `RESTORE` and clean selected-restore revalidation evidence still do not unlock real-profile/custom movement.
- Updated README fixture launcher, automated test coverage, and manual review wording.
- Added this feature brief and refreshed progress/handoff docs.
- No WPF behavior changed; no real-profile files were scanned, moved, restored, deleted, created, or rewritten.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-31-fixture-checklist-selected-restore-revalidation.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is checklist/manual-review alignment with ADR 0019's existing boundary.

Open questions:

- None for this checklist packet.

Rejected ideas buffer:

- Do not add a long separate terminal checklist step for a rare real-profile manifest evidence state unless manual review shows the concise wording is too easy to miss.

### 2026-05-31: Full Local MVP Preflight After Selected Restore Revalidation Checklist

Status: completed

Evidence:

- The fixture launcher checklist and README manual review wording changed after the WPF selected restore revalidation evidence packet.
- The full `.cmd` preflight verifies the updated checklist through the same local user path and reruns the app/core checks.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` from the repository root.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the updated fixture checklist, and ran whitespace diff checking.
- Checklist output included selected restore revalidation evidence in step 9 and the clean selected-restore revalidation blocker boundary in step 10.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`

Docs updated:

- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs:

- No ADR added. This is verification evidence only.

Open questions:

- None.

Rejected ideas buffer:

- Do not treat checklist-only output as enough after visible app wording changes when a full preflight is cheap enough to run.

### 2026-05-31: WPF Selected Restore Revalidation Evidence

Status: completed

Evidence:

- The prior packet added core Selected Restore Pre-Execution Revalidation, but WPF did not expose that evidence yet.
- ADR 0019 requires immediate selected-restore revalidation before any future real-profile selected restore movement.

Implementation:

- WPF selected restore gate output now includes Selected Restore Pre-Execution Revalidation evidence for exact real-profile selected Restore Manifests.
- The evidence is rebuilt when the selected restore gate is previewed and when the `RESTORE` confirmation text changes.
- The output shows whether revalidation can proceed, exact real-profile scope evidence, selected real-profile Undo implementation evidence, exact `RESTORE` match evidence, entry counts, selected manifest path, read-only boundary wording, and path-specific blockers.
- Added WPF smoke coverage for clean synthetic real-profile selected revalidation evidence and stale missing-quarantine-path blockers.
- Real-profile selected restore remains unavailable; no real-profile files were moved, restored, deleted, created, or rewritten.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

Docs updated:

- `README.md`
- `docs/features/2026-05-31-wpf-selected-restore-revalidation-evidence.md`
- `docs/features/2026-05-31-selected-restore-pre-execution-revalidation.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No new ADR added. This packet implements display evidence for ADR 0019's existing revalidation requirement.

Open questions:

- None for this display packet.

Rejected ideas buffer:

- Do not enable real-profile selected restore from this WPF evidence.
- Do not show selected restore revalidation as approval; it must rerun immediately before any future movement.

### 2026-05-31: Selected Restore Pre-Execution Revalidation

Status: completed

Evidence:

- ADR 0019 requires immediate selected-restore revalidation before any future real-profile selected restore movement.
- Existing Restore Readiness Preview and Selected Restore Manifest Review already detect restore blockers; the missing piece was a named final rediscovery/revalidation model for the selected real-profile path.

Implementation:

- Added `SelectedRestorePreExecutionRevalidation` and `SelectedRestorePreExecutionRevalidationBuilder`.
- The builder rediscovers the selected Restore Manifest from the selected Quarantine Root, rebuilds selected readiness, checks exact `C:\Users\moxhe` scope, exact selected restore gate evidence, explicit real-profile selected Undo implementation evidence, stale selected-review/draft mismatches, and entry-level restore blockers.
- Added core tests for clean synthetic real-profile evidence, stale missing quarantine paths, and unavailable/non-real-profile blockers.
- No WPF execution wiring changed. No real-profile files were moved, restored, deleted, created, or rewritten.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`

Docs updated:

- `docs/features/2026-05-31-selected-restore-pre-execution-revalidation.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0019-use-real-profile-selected-restore-execution-contract.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No new ADR added. ADR 0019 already requires immediate selected-restore revalidation; this packet implements that read-only core evidence model.

Open questions:

- None for the core model.

Rejected ideas buffer:

- Do not wire this model to WPF restore execution in the same packet.
- Do not treat clean selected restore pre-execution revalidation as approval while real-profile selected restore remains unavailable.

### 2026-05-31: User Verification for Non-D Root Acknowledgement Row

Status: completed

Evidence:

- User manually reviewed the new non-D acknowledgement row after the visible `?` help cue packet.
- User reported: `the new non-D acknowledgement row feels clear and not crowded.`

Implementation:

- Recorded the manual verification result in progress and handoff docs.
- No code, scan behavior, execution behavior, restore behavior, deletion behavior, folder creation, manifest writing, or cleanup history changed.

Verification:

- User visual verification.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is manual UI verification evidence only, with no durable architecture, persistence, cleanup execution, restore rule, data-model, or security decision.

Open questions:

- None for the non-D acknowledgement row layout.

Rejected ideas buffer:

- Do not keep treating the non-D acknowledgement row as an unresolved crowding concern unless later manual review contradicts this verification.

### 2026-05-31: Full Local MVP Preflight After Non-D Root Cue

Status: completed

Evidence:

- Non-D Root Acknowledgement Help Cue touched WPF layout, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full `.cmd` preflight gives stronger evidence than the narrow WPF app test before the next visible fixture review.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` from the repository root after the help-cue packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, ran fixture checklist-only output, and ran whitespace diff checking.
- Checklist-only output included `non-D root readiness acknowledgement wording plus its hoverable ? help cue`.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight`.
- `git status --short --branch` showed a clean tree before running this docs-only verification packet.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against the compact Quarantine Readiness Summary and existing safety/readiness boundaries.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-31: Non-D Root Acknowledgement Help Cue

Status: completed

Evidence:

- The WPF Non-D Quarantine Root Acknowledgement is safety-sensitive readiness evidence and previously relied on hidden checkbox tooltip/help text.
- Nearby safety and execution boundaries use compact hoverable `?` cues, so this acknowledgement was the next inconsistent visible cue gap.

Implementation:

- Grouped the Non-D Quarantine Root Acknowledgement checkbox with a visible circular `?` help cue.
- Mirrored the checkbox tooltip and automation help text onto the cue for enabled and disabled states.
- Added the cue to hoverable help-cue affordance coverage, expanding the tracked count from eighteen to nineteen.
- Updated the fixture checklist and durable docs so manual review checks the new cue.
- No WPF execution behavior, scan behavior, real-profile/custom execution availability, restore availability, permanent deletion, folder creation, manifest writing, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-non-d-root-acknowledgement-help-cue.md`
- `docs/features/2026-05-31-wpf-non-d-root-acknowledgement.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0018 already records the non-`D:` root acknowledgement requirement; this packet only adds a visible help affordance.

Open questions:

- None for row clarity. User later verified the acknowledgement row feels clear and not crowded.

Rejected ideas buffer:

- Do not treat the help cue as acknowledgement or approval; it is non-clickable discoverability for existing tooltip/help text.

### 2026-05-31: Full Local MVP Preflight After Non-D Acknowledgement

Status: completed

Evidence:

- WPF Non-D Quarantine Root Acknowledgement touched WPF layout, readiness wiring, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full `.cmd` preflight gives stronger evidence than the narrow WPF test run before the next visible fixture review.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` from the repository root after the non-D acknowledgement packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, ran fixture checklist-only output, and ran whitespace diff checking.
- Checklist-only output included `non-D root readiness acknowledgement wording`.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight`.
- `git status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against the non-D acknowledgement, compact Quarantine Readiness Summary, and existing safety/readiness boundaries.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-31: WPF Non-D Quarantine Root Acknowledgement

Status: completed

Evidence:

- ADR 0018 allows safe non-`D:` Quarantine Roots only with an extra acknowledgement.
- Core `QuarantineRootExecutionSafetyBuilder` already supported non-preferred-root acknowledgement.
- WPF previously always built root execution safety without that acknowledgement, so non-`D:` root readiness evidence could not show the acknowledged state.

Implementation:

- Added `NonPreferredQuarantineRootAcknowledgementBox` under the Quarantine Root Safety Note.
- Enabled the acknowledgement only for fully qualified non-`D:` roots and kept it disabled/unchecked for preferred `D:` roots or invalid roots.
- Cleared the acknowledgement when the Quarantine Root changes.
- Cleared stale Quarantine Preview/gate state when acknowledgement changes and requires a fresh preview.
- Passed the acknowledgement into `QuarantineRootExecutionSafetyBuilder` for read-only WPF preview/gate evidence.
- Added WPF smoke coverage for unacknowledged and acknowledged non-`D:` root safety evidence, stale preview clearing, no-folder-creation behavior, and reset on preferred `D:` roots.
- No WPF execution behavior, scan behavior, real-profile/custom execution availability, restore availability, permanent deletion, folder creation, manifest writing, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-wpf-non-d-root-acknowledgement.md`
- `docs/features/2026-05-31-quarantine-root-execution-safety.md`
- `docs/features/2026-05-31-wpf-root-execution-safety-evidence.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0018 already records non-`D:` root acknowledgement as part of the accepted readiness model.

Open questions:

- Manual fixture review should check whether the acknowledgement checkbox is clear enough and does not crowd the Quarantine Root area.

Rejected ideas buffer:

- Do not treat non-`D:` acknowledgement as cleanup approval or as an override for unsafe roots; it clears only the non-preferred-root acknowledgement blocker.

### 2026-05-31: Full Local MVP Preflight After Readiness Summary

Status: completed

Evidence:

- Quarantine Readiness Summary touched WPF layout, WPF lifecycle text, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full `.cmd` preflight gives stronger evidence than the narrow WPF test run before the next visible fixture review.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` from the repository root after the readiness-summary packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, ran fixture checklist-only output, and ran whitespace diff checking.
- Checklist-only output included `compact Quarantine Readiness Summary states/tooltips`.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight`.
- `git status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against the compact Quarantine Readiness Summary and existing safety/readiness boundaries.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-31: Quarantine Readiness Summary

Status: completed

Evidence:

- Recent readiness packets made detailed readiness evidence available in Quarantine Preview and Quarantine Execution Gate output.
- The next recommended work asked whether the execution readiness output should become a dedicated compact pane after manual review.
- A compact summary line can improve visible readiness clarity without adding another panel, help cue, or execution path.

Implementation:

- Added `QuarantineReadinessSummaryText` between inline Quarantine Preview status and exact confirmation controls.
- Summarized waiting, fixture-ready/open, preview-only, stale-executed, and undo-completed states.
- Used lightweight neutral/success/warning styling and mirrored `Summary state:` into tooltip/automation help text.
- Updated WPF smoke coverage for fixture preview/open/executed/undo, custom preview-only, and synthetic real-profile preview-only summary states.
- Updated the fixture checklist so manual review includes compact summary states/tooltips.
- No WPF execution behavior, scan behavior, real-profile/custom execution availability, restore availability, permanent deletion, folder creation, manifest writing, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-quarantine-readiness-summary.md`
- `docs/features/2026-05-31-wpf-execution-readiness-output.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF clarity/readiness wording under ADR 0018, with no new architecture, persistence, cleanup execution, restore rule, or security decision.

Open questions:

- Manual fixture review should decide whether this compact summary is enough or whether a later dedicated readiness pane is still useful.

Rejected ideas buffer:

- Do not add a new help cue or another large readiness panel before manual visual review proves the compact line is insufficient.

### 2026-05-31: WPF Real-Profile Restore Readiness Evidence

Status: completed

Evidence:

- ADR 0018 requires Real-Profile Restore Readiness before any forward real-profile Quarantine movement.
- ADR 0019 records the future selected-manifest real-profile restore contract while keeping execution unavailable.
- The core Real-Profile Restore Readiness model already existed and is read-only.
- WPF selected restore gate already had selected manifest review, confirmation draft, and exact `RESTORE` gate evidence for the builder to consume.

Implementation:

- Added WPF storage for the current Real-Profile Restore Readiness result.
- Built restore readiness after selected restore gate preview and after selected restore confirmation text changes.
- Added compact restore-readiness evidence lines to Selected Restore Execution Gate output.
- Passed restore readiness into `QuarantineExecutionReadinessBuilder` for forward Quarantine readiness display when available.
- Updated WPF smoke assertions so synthetic real-profile selected restore output shows real-profile restore readiness evidence while selected real-profile Undo remains unavailable.
- No WPF execution behavior, scan behavior, real-profile/custom execution availability, restore availability, permanent deletion, folder creation, manifest writing, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-wpf-real-profile-restore-readiness-evidence.md`
- `docs/features/2026-05-31-real-profile-restore-readiness.md`
- `docs/features/2026-05-31-wpf-execution-readiness-output.md`
- `docs/features/2026-05-31-real-profile-readiness-output-regression.md`
- `docs/features/2026-05-31-real-profile-selected-restore-execution-contract.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0018 and ADR 0019 already record the durable readiness and selected restore boundaries.

Open questions:

- None for this display-only packet.

Rejected ideas buffer:

- Do not treat clean-looking selected manifest readiness plus exact `RESTORE` as enough for real-profile restore execution; implementation remains a later explicit safety packet.

### 2026-05-31: WPF Pre-Execution Revalidation Evidence

Status: completed

Evidence:

- ADR 0018 requires Pre-Execution Revalidation before any real-profile movement.
- The core Pre-Execution Revalidation model already existed and is read-only.
- WPF now has enough preview, action-draft, and root-safety evidence to run a read-only revalidation check for display.

Implementation:

- Added WPF storage for the current Pre-Execution Revalidation result.
- Built revalidation after Quarantine Preview when action draft and root-safety evidence exist.
- Passed revalidation into `QuarantineExecutionReadinessBuilder` for preview/gate display.
- Added compact revalidation evidence lines to Quarantine Preview and Quarantine Execution Gate output.
- Updated WPF smoke assertions so fixture output shows clean revalidation evidence and synthetic real-profile output shows source-missing revalidation evidence while still blocking on Real-Profile Restore Readiness and current-build unavailability.
- No WPF execution behavior, scan behavior, real-profile/custom execution availability, restore availability, permanent deletion, folder creation, manifest writing, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-wpf-pre-execution-revalidation-evidence.md`
- `docs/features/2026-05-31-pre-execution-revalidation.md`
- `docs/features/2026-05-31-wpf-root-execution-safety-evidence.md`
- `docs/features/2026-05-31-wpf-execution-readiness-output.md`
- `docs/features/2026-05-31-real-profile-readiness-output-regression.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0018 already records Pre-Execution Revalidation as a durable readiness dimension.

Open questions:

- None for this display-only packet.

Rejected ideas buffer:

- Do not treat preview-time revalidation as final movement approval; it must run again immediately before any future real-profile move.

### 2026-05-31: WPF Root Execution Safety Evidence

Status: completed

Evidence:

- ADR 0018 requires Quarantine Root Execution Safety before any real-profile movement.
- The core Quarantine Root Execution Safety model already existed and is read-only.
- WPF readiness output previously reported root safety as missing even when a Quarantine Action Draft existed and could be checked without moving files.

Implementation:

- Added WPF storage for the current Quarantine Root Execution Safety result.
- Built root safety from the current Quarantine Action Draft after Quarantine Preview.
- Passed root safety into `QuarantineExecutionReadinessBuilder` for preview/gate display.
- Added compact root-safety evidence lines to Quarantine Preview and Quarantine Execution Gate output.
- Updated WPF smoke assertions so fixture output shows root safety evidence and synthetic real-profile output consumes root safety while still blocking on Pre-Execution Revalidation, Real-Profile Restore Readiness, and current-build unavailability.
- No WPF execution behavior, scan behavior, real-profile/custom execution availability, restore availability, permanent deletion, folder creation, manifest writing, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-wpf-root-execution-safety-evidence.md`
- `docs/features/2026-05-31-wpf-execution-readiness-output.md`
- `docs/features/2026-05-31-quarantine-root-execution-safety.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0018 already records Quarantine Root Execution Safety as a durable readiness dimension.

Open questions:

- None for this display-only packet.

Rejected ideas buffer:

- Do not add non-`D:` acknowledgement UI opportunistically; that affects approval semantics and should get its own small Grill with Docs pass.

### 2026-05-31: Real-Profile Child Readiness Output Regression

Status: completed

Evidence:

- ADR 0018 limits the first real-profile Quarantine phase to exact `C:\Users\moxhe`.
- Core tests already proved real-profile child scopes stay preview-only.
- The prior WPF readiness-output regression covered exact `C:\Users\moxhe` but not child scopes under it.

Implementation:

- Added `MainWindowShowsRealProfileChildReadinessContractForSyntheticPreview`.
- The test applies synthetic `C:\Users\moxhe\AppData\Local`-shaped scan-result metadata through the WPF scan-result application path without scanning or touching the real profile.
- The test shortlists a synthetic likely-safe Quarantine candidate, creates a dry-run Quarantine Preview, types exact `QUARANTINE`, and verifies real-profile child readiness remains preview-only.
- The test asserts the preview and gate output show the exact `C:\Users\moxhe` first-phase scope blocker and `Can execute: no`.
- The test does not call Quarantine execution and verifies the synthetic Quarantine Root folder is not created.
- No WPF behavior, scan behavior, fixture Quarantine behavior, real-profile/custom execution availability, restore availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-real-profile-child-readiness-output-regression.md`
- `docs/features/2026-05-31-wpf-execution-readiness-output.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0018 already records the exact first-phase real-profile scope rule.

Open questions:

- None for this regression packet.

Rejected ideas buffer:

- Do not treat child scopes under `C:\Users\moxhe` as execution-eligible just because they look narrower than the profile root; they need a later explicit design.

### 2026-05-31: Real-Profile Readiness Output Regression

Status: completed

Evidence:

- ADR 0018 requires WPF readiness output to name missing real-profile prerequisites while keeping execution disabled.
- The WPF Execution Readiness Output feature brief said custom and real-profile scopes should show readiness output, but completion notes only recorded fixture and custom assertions.
- A real-profile scan is too broad for an autonomous regression packet.

Implementation:

- Added `MainWindowShowsRealProfileReadinessContractForSyntheticPreview`.
- The test applies synthetic `C:\Users\moxhe` scan-result metadata through the WPF scan-result application path without scanning or touching the real profile.
- The test shortlists a synthetic real-profile-shaped likely-safe Quarantine candidate, creates a dry-run Quarantine Preview, types exact `QUARANTINE`, and verifies real-profile candidate readiness remains current-build non-executable.
- A later packet wired read-only Quarantine Root Execution Safety evidence into WPF, so this regression now asserts root safety is checked while Pre-Execution Revalidation and Real-Profile Restore Readiness blockers remain grouped in preview and gate output.
- The test does not call Quarantine execution and verifies the synthetic Quarantine Root folder is not created.
- No WPF behavior, scan behavior, fixture Quarantine behavior, real-profile/custom execution availability, restore availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-real-profile-readiness-output-regression.md`
- `docs/features/2026-05-31-wpf-execution-readiness-output.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0018 already records the WPF readiness-output requirement.

Open questions:

- None for this regression packet.

Rejected ideas buffer:

- Do not scan `C:\Users\moxhe` just to prove WPF readiness-output formatting.

### 2026-05-31: Fixture Checklist Selected Restore Boundary

Status: completed

Evidence:

- ADR 0017 keeps real-profile/custom Quarantine execution blocked.
- ADR 0019 keeps selected real-profile restore blocked until a later implementation packet.
- The launcher checklist already mentioned exact `RESTORE`, but its final blocker sentence only named ADR 0017.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist step 10 to name ADR 0017 Quarantine blockers and ADR 0019 selected-restore blockers together.
- Updated README fixture-smoke/manual-check wording and the MVP readiness-audit follow-up so the durable docs mirror the checklist.
- Added a feature brief for the checklist alignment.
- No WPF behavior, scan behavior, fixture Quarantine behavior, real-profile/custom execution availability, restore availability, permanent deletion, or cleanup history changed.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-fixture-checklist-selected-restore-boundary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0017 and ADR 0019 already record the durable safety boundaries.

Open questions:

- None for this checklist alignment packet.

Rejected ideas buffer:

- Do not let the selected-restore blocker remain implicit under the Quarantine readiness ADR now that ADR 0019 exists.

### 2026-05-31: Real-Profile Selected Restore Regression

Status: completed

Evidence:

- ADR 0019 records the selected real-profile restore contract but intentionally does not enable execution.
- Existing WPF smoke coverage already proved fixture selected restore can execute and custom non-fixture selected restore remains blocked.
- The missing boundary was a selected Restore Manifest whose Cleanup Scope is exact `C:\Users\moxhe`.

Implementation:

- Added `MainWindowKeepsSelectedRestoreUnavailableForRealProfileManifest`.
- The test writes a synthetic Restore Manifest under a test quarantine root with exact real-profile Cleanup Scope metadata and a non-existent GUID original path under `C:\Users\moxhe`.
- The test discovers the manifest, previews selected readiness, types exact `RESTORE`, verifies `Can execute: no`, verifies preview-only real-profile/custom blocker wording, and confirms the synthetic quarantine file remains in place.
- The test does not call selected restore execution for the real-profile manifest, avoiding any risk of writing under `C:\Users\moxhe` if a future bug accidentally opens the gate.
- No WPF behavior, scan behavior, fixture Quarantine behavior, real-profile/custom execution availability, restore availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-real-profile-selected-restore-regression.md`
- `docs/features/2026-05-31-real-profile-selected-restore-execution-contract.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0019 already records this boundary.

Open questions:

- None for this regression packet.

Rejected ideas buffer:

- Do not call selected restore execution for a real-profile manifest in a blocking regression test; a broken gate could modify `C:\Users\moxhe`.

### 2026-05-31: Real-Profile Selected Restore Execution Contract

Status: completed

Evidence:

- ADR 0018 requires trusted selected-manifest real-profile Undo Quarantine behavior before forward real-profile movement.
- Fixture-only selected restore exists, but real-profile selected restore remains unavailable.
- The user chose Restore Manifest-only recovery for now and manual rediscover/rescan guidance after movement.

Implementation:

- Added ADR 0019 for the Real-Profile Selected Restore Execution contract.
- Added a feature brief defining exact `C:\Users\moxhe`, one selected Restore Manifest, exact `RESTORE`, immediate selected-readiness revalidation, no original-path overwrite, `UndoQuarantineExecutor`, Restore Manifest-only durable records, manual rediscover/rescan guidance, no all-manifest restore, no action-folder cleanup, no permanent deletion, and no cleanup history.
- Updated domain/glossary, README, ADR 0018, Real-Profile Restore Readiness follow-up, handoff, and progress docs.
- No WPF behavior, scan behavior, fixture Quarantine behavior, real-profile/custom execution availability, restore availability, permanent deletion, or cleanup history changed.

Verification:

- `git diff --check`

Docs updated:

- `docs/decisions/0019-use-real-profile-selected-restore-execution-contract.md`
- `docs/features/2026-05-31-real-profile-selected-restore-execution-contract.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/features/2026-05-31-real-profile-restore-readiness.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- Added ADR 0019.

Open questions:

- Whether all-manifest real-profile restore, action-folder cleanup, cleanup history, or custom non-fixture restore should ever exist.

Rejected ideas buffer:

- Do not enable selected real-profile restore by flipping fixture-only availability.
- Do not use all-manifest restore as the first real-profile restore path.
- Do not treat exact `RESTORE` as enough without immediate selected-readiness revalidation.

### 2026-05-31: WPF Execution Readiness Output

Status: completed

Evidence:

- The core readiness inputs now exist: `QuarantineExecutionReadiness`, `QuarantineRootExecutionSafety`, `PreExecutionRevalidation`, and `RealProfileRestoreReadiness`.
- ADR 0018 calls for WPF readiness output that names missing real-profile prerequisites while keeping execution disabled.
- The prior WPF panes only exposed the older fixture-only scope status and gate wording.

Implementation:

- Added read-only Execution Readiness contract lines to Quarantine Preview and Quarantine Execution Gate output.
- Displays disposition, scope kind, current-build readiness-model execution availability, first real-profile caps, and a no-approval boundary line.
- Groups blockers into Quarantine Root Execution Safety, Pre-Execution Revalidation, Real-Profile Restore Readiness, Scope and policy, or Review readiness.
- Added WPF smoke assertions for fixture and custom preview/gate readiness contract output.
- No WPF execution behavior, scan behavior, fixture Quarantine behavior, real-profile/custom execution availability, restore availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-wpf-execution-readiness-output.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- ADR 0018 already covers this follow-up; no new ADR added.

Open questions:

- Whether the readiness output should become a dedicated compact pane after manual review.

Rejected ideas buffer:

- Do not run live WPF pre-execution revalidation as part of this display-only packet.
- Do not treat visible readiness output as permission to enable real-profile/custom execution.

### 2026-05-31: Real-Profile Restore Readiness

Status: completed

Evidence:

- ADR 0018 requires selected-manifest real-profile Undo readiness before forward real-profile movement.
- The previous readiness model had only a simple selected-restore prerequisite and no named restore-readiness evidence input.
- Restore readiness must remain read-only and selected-manifest-only before any WPF restore behavior changes.

Implementation:

- Added `RealProfileRestoreReadiness` and `RealProfileRestoreReadinessBuilder`.
- Consumes Selected Restore Manifest Review, Selected Restore Confirmation Draft, and Selected Restore Execution Gate evidence.
- Checks exact real-profile Cleanup Scope, restorable entries, blocked/recovery/not-moved rows, confirmation draft consistency, exact `RESTORE` gate evidence, Restore Manifest-only durable record scope, and whether selected-manifest real-profile Undo implementation is available.
- Updated `QuarantineExecutionReadinessBuilder` so real-profile readiness can consume Real-Profile Restore Readiness when supplied, or keep a restore-readiness-not-checked blocker when absent.
- Added focused core tests and a feature brief.
- No WPF behavior, scan behavior, fixture Quarantine behavior, real-profile/custom execution availability, restore availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-real-profile-restore-readiness.md`
- `docs/features/2026-05-31-pre-execution-revalidation.md`
- `docs/features/2026-05-31-quarantine-execution-readiness-model.md`
- `docs/features/2026-05-31-real-profile-quarantine-design-pass.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- Updated ADR 0018 follow-up status; no new ADR added.

Open questions:

- None for this packet.

Rejected ideas buffer:

- Do not treat fixture selected restore as proof that real-profile selected restore is ready.
- Do not treat clean Real-Profile Restore Readiness as permission to move or restore real-profile files in WPF.
- Do not add all-manifest real-profile restore to the first recovery prerequisite.

### 2026-05-31: Pre-Execution Revalidation

Status: completed

Evidence:

- ADR 0018 requires immediate pre-execution revalidation before any real-profile movement can be considered.
- The previous readiness model could consume root safety but still reported that Pre-Execution Revalidation had not been checked.
- Revalidation must remain read-only and synthetic-fixture-tested before any WPF behavior changes.

Implementation:

- Added `PreExecutionRevalidation` and `PreExecutionRevalidationBuilder`.
- Checks preview/confirmation/action/root-safety consistency, included counts/bytes, action draft rows, live source existence, source file size/timestamp drift, reparse status, action-root collision, Restore Manifest path collision, and item destination collision.
- Updated `QuarantineExecutionReadinessBuilder` so real-profile readiness can consume revalidation when supplied, or keep a revalidation-not-checked blocker when absent.
- Added focused core tests and a feature brief.
- No WPF behavior, scan behavior, fixture Quarantine behavior, real-profile/custom execution availability, restore availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

Docs updated:

- `docs/features/2026-05-31-pre-execution-revalidation.md`
- `docs/features/2026-05-31-quarantine-root-execution-safety.md`
- `docs/features/2026-05-31-quarantine-execution-readiness-model.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- Updated ADR 0018 follow-up status; no new ADR added.

Open questions:

- None for this packet.

Rejected ideas buffer:

- Do not treat clean pre-execution revalidation as permission to move real-profile files.
- Do not wire revalidation into WPF execution until selected-manifest real-profile Undo readiness is designed.

### 2026-05-31: Quarantine Root Execution Safety

Status: completed

Evidence:

- ADR 0018 requires execution-specific root safety separate from preview-only Quarantine Root Safety Note.
- The previous readiness model could only report that root safety had not been checked.
- User approved safe non-`D:` roots with extra acknowledgement, while unsafe roots remain blocked.

Implementation:

- Added `QuarantineRootExecutionSafety` and `QuarantineRootExecutionSafetyBuilder`.
- Checks fully qualified root, root/scope containment, action-scoped layout, action-root collisions, Restore Manifest path collisions, item destination collisions, and capacity/free-space evidence.
- Allows safe non-`D:` roots only when the acknowledgement is present; acknowledgement does not override unsafe containment, collisions, layout, or capacity blockers.
- Updated `QuarantineExecutionReadinessBuilder` so real-profile readiness can consume root safety when supplied, or keep a root-safety-not-checked blocker when absent.
- Added focused core tests and a feature brief.
- No WPF behavior, scan behavior, fixture Quarantine behavior, real-profile/custom execution availability, restore availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `docs/features/2026-05-31-quarantine-root-execution-safety.md`
- `docs/features/2026-05-31-quarantine-execution-readiness-model.md`
- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- Updated ADR 0018 follow-up status; no new ADR added.

Open questions:

- None for this packet.

Rejected ideas buffer:

- Do not treat non-`D:` acknowledgement as an override for unsafe roots.
- Do not wire root safety into WPF execution until Pre-Execution Revalidation is also stable.

### 2026-05-31: Quarantine Execution Readiness Model

Status: completed

Evidence:

- User answered the real-profile design questions: first phase exact `C:\Users\moxhe`, default/preferred `D:` but safe non-`D:` roots allowed with acknowledgement, 10 rows / 1 GB cap, selected-manifest real-profile Undo required first, exact `QUARANTINE`, Likely safe + Quarantine candidate only, files plus narrow folders with strict descendant checks, manual rescan after execution, and Restore Manifest-only durable record.
- ADR 0018 needed to move from proposed/open questions to accepted decisions before code used the terms.
- The current WPF gate still must remain unchanged and fixture-only.

Implementation:

- Accepted ADR 0018 and updated the real-profile design brief with the user's decisions.
- Added `QuarantineExecutionReadiness`, `QuarantineExecutionReadinessBuilder`, `QuarantineExecutionReadinessDisposition`, and `QuarantineExecutionReadinessScopeKind`.
- Added core tests that name fixture-executable, real-profile-candidate, custom-preview-only, and real-profile-child preview-only states.
- Added tests for first-phase caps, exact `QUARANTINE`, safe non-`D:` acknowledgement, Likely safe + Quarantine candidate eligibility, narrow folder descendant blockers, selected-manifest Undo prerequisite, manual rescan guidance, and Restore Manifest-only durable record.
- Added a feature brief for the model packet and refreshed domain/glossary/handoff/progress docs.
- No WPF behavior, scan behavior, fixture Quarantine behavior, real-profile/custom execution availability, restore availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/features/2026-05-31-real-profile-quarantine-design-pass.md`
- `docs/features/2026-05-31-quarantine-execution-readiness-model.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- Updated ADR 0018 from proposed to accepted.

Open questions:

- None for this packet.

Rejected ideas buffer:

- Do not wire `QuarantineExecutionReadiness` into WPF execution as permission to move real-profile files.
- Do not treat non-`D:` acknowledgement as an override for unsafe root containment, collision, or capacity blockers.

### 2026-05-31: Real-Profile Quarantine Design Pass

Status: completed

Evidence:

- User asked for a real-profile Quarantine design pass after fixture review and readiness-contract work.
- ADR 0017 already blocks real-profile movement until the app has a richer execution-readiness contract.
- Code inspection showed the current gate still hinges on fixture-only execution availability and does not yet model real-profile root safety, pre-execution revalidation, or real-profile restore readiness as first-class blockers.

Implementation:

- Added ADR 0018 for a composite Real-Profile Quarantine Execution Readiness model; a later packet accepted it after user decisions.
- Added a feature brief with readiness dimensions, implementation staging, open questions, and explicit non-goals.
- Added draft domain/glossary language for Real-Profile Quarantine Execution Readiness, Quarantine Root Execution Safety, Pre-Execution Revalidation, and Real-Profile Restore Readiness.
- Updated handoff and progress docs so the next packet can start from the design contract.
- No WPF behavior, scan behavior, fixture Quarantine behavior, real-profile/custom execution availability, restore availability, permanent deletion, or cleanup history changed.

Verification:

- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/features/2026-05-31-real-profile-quarantine-design-pass.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- Added ADR 0018; a later packet accepted it after user decisions.

Open questions:

- Decide first real-profile scope limit, root rule, first-batch cap, selected-restore prerequisite, and real-profile approval phrase before implementation.

Rejected ideas buffer:

- Do not flip `IsExecutionImplemented` for real-profile scopes.
- Do not treat preview-only Quarantine Root Safety Note as execution-root safety.
- Do not treat exact `QUARANTINE` alone as sufficient real-profile approval.

### 2026-05-31: Fixture Checklist Readiness-Contract Boundary

Status: completed

Evidence:

- ADR 0017 now requires a richer readiness contract before real-profile WPF Quarantine execution can move files.
- The fixture launcher checklist final step still only asked for broad real-profile/custom unavailability.
- A read-only subagent audit independently identified the same checklist gap and a stale MVP readiness-audit follow-up that still suggested deciding whether to wire real-profile execution after retest.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist step 10 so real-profile/custom scopes stay visibly preview-only and exact `QUARANTINE`, exact `RESTORE`, clean preview, real-profile scan acknowledgement, and Review Shortlist do not unlock execution.
- Updated README fixture launcher/manual-review wording to mirror the ADR 0017 boundary.
- Updated the MVP readiness audit follow-up so real-profile Quarantine/Undo remain unavailable until ADR 0017's readiness contract is designed and verified.
- Added a feature brief for this docs/checklist alignment.
- No WPF behavior, scan behavior, Quarantine behavior, restore behavior, real-profile/custom execution availability, permanent deletion, or cleanup history changed.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-31-fixture-checklist-readiness-contract-boundary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. ADR 0017 already records the durable readiness-contract decision.

Open questions:

- During visible review, is checklist wording enough, or should WPF disabled gate text name more of the ADR 0017 prerequisites?

Rejected ideas buffer:

- Do not treat Review Shortlist, clean preview, exact confirmation, or real-profile scan acknowledgement as cleanup approval for real-profile movement.

### 2026-05-30: Real-Profile Quarantine Readiness Contract

Status: completed

Evidence:

- User asked to implement the readiness-contract plan before moving toward real-profile Quarantine execution.
- Current WPF execution availability is still fixture-only, while real-profile and custom non-fixture scopes remain preview-only.
- The first attempted WPF app-test run was blocked by a running `WindowsFileCleaner.App` process locking build output; that process was stopped and the same test harness passed.

Implementation:

- Added ADR 0017 to require a Real-Profile Quarantine Readiness Contract before any WPF real-profile file movement.
- Added a feature brief that records current fixture-only behavior, non-goals, readiness prerequisites, failure/recovery expectations, and staged follow-up work.
- Added domain/glossary language for Real-Profile Quarantine Readiness Contract.
- Added WPF regression coverage that real-profile acknowledgement unlocks only read-only scanning without scanning the real profile.
- Strengthened the custom non-fixture WPF test so clean preview plus exact `QUARANTINE` still leaves `Quarantine included shortlist` closed and a direct execution attempt reports no file modifications.
- No real-profile Quarantine execution, real-profile Undo Quarantine, selected real-profile restore, permanent deletion, persisted cleanup history, scan behavior, or fixture execution behavior changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed after closing the stale running app process that locked the build output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0017-real-profile-quarantine-readiness-contract.md`
- `docs/features/2026-05-30-real-profile-quarantine-readiness-contract.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- Added ADR 0017 because real-profile cleanup execution is a durable core UX and safety-gate decision with multiple plausible paths and a meaningful downside.

Open questions:

- What exact approval sequence should open real-profile Quarantine after readiness exists?
- Should real-profile execution require a fresh MVP preflight or acknowledgement per execution session?

Rejected ideas buffer:

- Do not enable real-profile movement by simply setting the current fixture-only execution flag true for real/custom scopes.
- Do not treat real-profile scan acknowledgement, Review Shortlist, Quarantine Preview, or exact `QUARANTINE` as enough to move real-profile files.

### 2026-05-30: Trim AGENTS Workflow Doc

Status: completed

Evidence:

- User verified the compact scan header looked good and asked to trim `AGENTS.md` because it was too big.
- `AGENTS.md` duplicated details already covered by `docs/codex/grill-with-docs.md` and `docs/codex/skillopt-inspired-workflow.md`.

Implementation:

- Replaced the long `AGENTS.md` with a compact control-layer version.
- Preserved required reading, key commands, cleanup safety rules, documentation map, naming rules, done criteria, and final response expectations.
- Moved detailed workflow explanation back to linked docs rather than duplicating it in `AGENTS.md`.
- No app code, scan behavior, Quarantine behavior, restore behavior, real-profile/custom execution availability, permanent deletion, or cleanup history changed.

Verification:

- `AGENTS.md` line count dropped from 139 to 74 lines.

Docs updated:

- `AGENTS.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible workflow documentation compression with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- If future threads miss any repeated instruction, promote only that specific rule back into `AGENTS.md` instead of expanding it broadly.

### 2026-05-30: Compact Scan Header Status

Status: completed

Evidence:

- User completed fixture overlap cleanup steps 1-10 successfully.
- User screenshot showed the right-side header status text stacked vertically while a large middle area was unused.
- The request was to flatten the right-side header status so more room remains for the content underneath.

Implementation:

- Moved Cleanup Scope Safety Note, scan-gate summary, and scan-gate detail into one wrapping WPF header status strip.
- Kept Cleanup Scope Safety Note and scan-gate `?` help cues paired with their related text.
- Kept the real-profile preflight acknowledgement as its own conditional row.
- Added WPF smoke coverage that the header status area uses wrapping layout.
- No scan behavior, scan gate behavior, Quarantine behavior, restore behavior, real-profile/custom execution availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the compact wrapping header status prompt without preflight, fixture creation, WPF launch, scanning, or file modification.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-30-compact-scan-header-status.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF layout polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm the compact header gives enough review height back without crowding the safety/status cues.

### 2026-05-30: Redundant Shortlist Overlap Cleanup

Status: completed

Evidence:

- User completed manual fixture review steps 1-9 and reported everything seemed fine.
- During fixture Quarantine testing, `QUARANTINE` did not enable `Quarantine included shortlist` because the preview had `1 redundant preview row(s) must be removed before confirmation.`
- The overlap came from parent/child shortlist rows such as `Cache` and `http-v2`; the gate correctly blocked overlapping execution, but manual cleanup was annoying for bulk shortlist review.

Implementation:

- Added `Remove overlapping parents` beside Quarantine Preview controls.
- The action enables only when the current Quarantine Preview has redundant parent/child overlap.
- Clicking it removes broader included parent rows from Review Shortlist, keeps narrower child rows, clears stale preview/gate state and stale `QUARANTINE` text, and requires `Preview shortlist quarantine` again.
- Added WPF smoke coverage for parent/child overlap, blocked gate, one-click parent removal, stale preview clearing, clean re-preview, and fixture gate opening after exact confirmation.
- Updated README, domain docs, fixture checklist wording, feature brief, and handoff.
- No real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, persisted cleanup history, or cleanup execution semantics changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the `Remove overlapping parents` prompt without preflight, fixture creation, WPF launch, scanning, or file modification.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-30-redundant-shortlist-cleanup.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF review-workflow polish preserving existing Quarantine Preview and fixture-only execution boundaries.

Open questions:

- Should a later packet offer an explicit parent-first choice, or is the safer narrower-row default enough?

### 2026-05-30: Manifest Fixture Checklist Split

Status: completed

Evidence:

- `.\tools\Start-MvpFixtureReview.cmd -ChecklistOnly` printed successfully at the start of the thread without preflight, fixture creation, WPF launch, scanning, or file modification.
- Checklist step 8 had grown into one long line covering manifest discovery, all-manifest readiness, selected manifest readiness, selected restore gate states, and real/custom blockers.
- The WPF manifest row is already grouped so `Discover manifests`, `Preview all-manifest readiness`, and `Selected manifest` controls stay paired with their `?` cues; the next useful polish was making the manual review prompts easier to execute by eye.

Implementation:

- Split the previous manifest checklist step into a dedicated manifest discovery/all-manifest readiness step and a separate selected manifest readiness/selected restore gate step.
- Updated README fixture launcher wording to describe the separated manifest-review and selected-restore gate checks.
- Added a feature brief for the checklist split.
- No WPF app layout, scan behavior, Quarantine behavior, selected restore behavior, real-profile/custom execution availability, permanent deletion, or cleanup history changed.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed separated checklist steps 8, 9, and 10 without preflight, fixture creation, WPF launch, scanning, or file modification.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-30-manifest-fixture-checklist-split.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible manual-review checklist polish with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm whether the separated checklist makes manifest cue/control wrapping easier to inspect.

### 2026-05-30: Fresh Thread Handoff Polish

Status: completed

Evidence:

- The current thread is getting slow and the user plans to archive it after receiving a fresh-thread prompt.
- The handoff already contained the recent safety boundary, but the embedded startup prompt still used a generic latest-packet phrase and the best-next-work section did not explicitly call out the newest manifest cue/control wrapping check.

Implementation:

- Refreshed `docs/codex/thread-handoff.md` so the latest completed packet names this handoff polish.
- Added a verified-recently note for the handoff polish and current safety boundary.
- Updated the handoff best-next-work and embedded startup prompt to mention the recent Restore Manifest selection, `Discover manifests`, and `Preview all-manifest readiness` `?` cues, manifest cue/control grouping, eighteen-cue WPF smoke coverage, and full `.cmd` MVP preflight evidence after manifest control grouping.
- No app code, scan behavior, Quarantine behavior, selected restore behavior, real-profile/custom execution availability, permanent deletion, or cleanup history changed.

Verification:

- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is handoff/documentation alignment only.

Open questions:

- The next thread should still do a manual fixture visual pass before choosing any larger UX or cleanup-execution packet.

### 2026-05-30: Full Local MVP Preflight After Manifest Control Grouping

Status: completed

Evidence:

- Manifest Review Control Grouping touched WPF layout, fixture checklist wording, README, feature notes, progress, and handoff.
- A full `.cmd` MVP preflight was warranted before stopping this packet so the current pushed app state had broad local verification.

Implementation:

- Ran the full local MVP preflight through the execution-policy-friendly `.cmd` wrapper.
- No app code, scan behavior, Quarantine behavior, selected restore behavior, real-profile/custom execution availability, permanent deletion, or cleanup history changed in this verification packet.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Restore passed.
- Build passed with 0 warnings and 0 errors.
- Core tests passed: `All WindowsFileCleaner.Tests checks passed.`
- WPF app tests passed: `All WindowsFileCleaner.App.Tests checks passed.`
- Fixture dry run printed only `What if` fixture file operations.
- Fixture checklist-only output printed the manifest cue/control wrapping check without creating fixtures, launching WPF, or scanning real user files.
- Whitespace diff check passed.
- The preflight reported: `MVP preflight passed. No real user files were scanned or modified.`

Docs updated:

- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only.

Open questions:

- During visible fixture review, confirm the grouped manifest controls fit comfortably at normal window sizes.

### 2026-05-30: Manifest Review Control Grouping

Status: completed

Evidence:

- The manifest-review row now has visible `?` cues for `Discover manifests`, Restore Manifest selection, and `Preview all-manifest readiness`.
- As separate `WrapPanel` siblings, a help cue could wrap away from its related control in a narrower layout.

Implementation:

- Grouped `Discover manifests` with its `?` help cue.
- Grouped `Preview all-manifest readiness` with its `?` help cue.
- Grouped `Selected manifest`, its `?` help cue, and the Restore Manifest selector.
- Updated README and fixture checklist wording so visible review checks cue/control pairing when rows wrap.
- No scan, discovery, readiness, restore, cleanup execution, deletion, or cleanup history behavior changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/features/2026-05-30-all-manifest-readiness-help-cue.md`
- `docs/features/2026-05-30-manifest-review-control-grouping.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF layout polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm the grouped manifest controls fit comfortably at normal window sizes.

### 2026-05-30: All-Manifest Readiness Help Cue

Status: completed

Evidence:

- `Preview all-manifest readiness` is a read-only restore-adjacent action that checks discovered Restore Manifest readiness without restoring files.
- The button already had correct tooltip/help text, and the adjacent manifest discovery/selection controls now have visible `?` cues.

Implementation:

- Added a visible hoverable `?` help cue beside the WPF `Preview all-manifest readiness` action.
- Mirrored the readiness button tooltip and automation help text on the cue.
- Added test-facing accessors and WPF smoke assertions for the cue.
- Expanded hoverable help-cue affordance coverage from seventeen to eighteen tracked cues.
- Updated the fixture checklist prompt to include the all-manifest readiness cue.
- No scan, discovery, readiness, restore, cleanup execution, deletion, or cleanup history behavior changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `docs/features/2026-05-30-all-manifest-readiness-help-cue.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance-coverage.md`
- `docs/features/2026-05-30-quarantine-manifest-discovery-help-cue.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm the manifest discovery/readiness row still fits comfortably with the added cue.

### 2026-05-30: Quarantine Manifest Discovery Help Cue

Status: completed

Evidence:

- `Discover manifests` is the read-only entry point into older/discovered Restore Manifest review.
- The discovery button already had correct tooltip/help text, and the previous packet added the companion selected-manifest `?` cue.

Implementation:

- Added a visible hoverable `?` help cue beside the WPF `Discover manifests` action.
- Mirrored the discovery button tooltip and automation help text on the cue.
- Added test-facing accessors and WPF smoke assertions for the cue.
- Expanded hoverable help-cue affordance coverage from sixteen to seventeen tracked cues.
- Updated the fixture checklist prompt to include the discovery and selected-manifest cues.
- No scan, discovery, selection, readiness, restore, cleanup execution, deletion, or cleanup history behavior changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-manifest-discovery-selection-help-text.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance-coverage.md`
- `docs/features/2026-05-30-quarantine-manifest-discovery-help-cue.md`
- `docs/features/2026-05-30-restore-manifest-selection-help-cue.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm the manifest discovery/readiness row still fits comfortably with the new cue.

### 2026-05-30: Restore Manifest Selection Help Cue

Status: completed

Evidence:

- Restore Manifest selection is safety-sensitive because it starts the selected-manifest review path before selected readiness and fixture-only selected restore.
- The existing selection tooltip/help text already had the right read-only/not-approval wording, but the boundary was less discoverable than nearby `?` cue surfaces.

Implementation:

- Added a visible hoverable `?` help cue beside the WPF Restore Manifest selection control.
- Mirrored the selection tooltip and automation help text on the cue.
- Added test-facing accessors and WPF smoke assertions for the cue.
- Expanded hoverable help-cue affordance coverage from fifteen to sixteen tracked cues.
- No scan, discovery, selection, readiness, restore, cleanup execution, deletion, or cleanup history behavior changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-manifest-discovery-selection-help-text.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance-coverage.md`
- `docs/features/2026-05-30-restore-manifest-selection-help-cue.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm the selected-manifest row still fits comfortably with the new cue.
- Decide later whether `Discover manifests` also needs a visible help cue.

### 2026-05-30: Handoff Confirmation Cue Review Alignment

Status: completed

Evidence:

- Help Cue Affordance Feature Brief Alignment was pushed after the handoff's latest-packet field was last updated.
- The next recommended manual fixture review prompt did not explicitly mention the shortlist confirmation or selected restore confirmation `?` help cues, even though the launcher checklist and README detailed steps did.

Implementation:

- Updated `docs/codex/thread-handoff.md` so the latest completed packet is `Help Cue Affordance Feature Brief Alignment`.
- Added a verified-recently handoff bullet for the feature-brief alignment packet.
- Added shortlist confirmation and selected restore confirmation `?` cue checks to the progress next-work manual fixture review prompt.
- Aligned README Safety Status and Current Workflow wording with the exact-confirmation help cues.
- No WPF behavior, scan behavior, Quarantine behavior, selected restore behavior, real-profile/custom execution availability, permanent deletion, or cleanup history changed.

Verification:

- `rg -n 'shortlist confirmation|selected restore confirmation|exact RESTORE confirmation field|RESTORE confirmation field|Latest completed packet' README.md .codex\progress.md docs\codex\thread-handoff.md` showed the updated handoff latest-packet line and exact-confirmation cue wording.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is handoff/manual-review wording alignment only.

Open questions:

- During visible fixture review, confirm the exact-confirmation `?` cues are noticeable without making confirmation look like approval by itself.

### 2026-05-30: Help Cue Affordance Feature Brief Alignment

Status: completed

Evidence:

- Confirmation Field Help Cues expanded WPF smoke affordance coverage to fifteen circular `?` cues.
- The original hoverable help-cue affordance feature briefs still stopped their later-packet trail at eleven cues, which could mislead future work even though README, handoff, tests, and current progress state were current.

Implementation:

- Updated the two hoverable help-cue affordance feature briefs to say later packets expanded the tracked cue list to fifteen.
- No WPF behavior, scan behavior, Quarantine behavior, selected restore behavior, real-profile/custom execution availability, permanent deletion, or cleanup history changed.

Verification:

- `rg -n "tracks fifteen|tracked cue list to fifteen" docs\features\2026-05-30-hoverable-help-cue-affordance.md docs\features\2026-05-30-hoverable-help-cue-affordance-coverage.md .codex\progress.md` showed the updated fifteen-cue alignment.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `docs/features/2026-05-30-hoverable-help-cue-affordance.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance-coverage.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is bounded feature-brief alignment only.

Open questions:

- During visible fixture review, confirm the full set of compact `?` cues is helpful without making the dense review surface noisy.

### 2026-05-30: Full Local MVP Preflight After Confirmation Field Help Cues

Status: completed

Evidence:

- Confirmation Field Help Cues changed WPF XAML, test-facing cue affordance coverage, WPF app smoke assertions, README/manual checklist wording, domain docs, handoff, and progress.
- A full local preflight through the preferred `.cmd` wrapper verifies the complete restore/build/test/fixture/checklist/diff path after that help-cue packet.

Implementation:

- Ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd`.
- Updated the confirmation field help-cue feature brief and thread handoff with full preflight evidence.
- Corrected `docs/codex/thread-handoff.md` so the latest completed packet points to this full preflight packet.
- No WPF behavior, scan behavior, Quarantine behavior, selected restore behavior, real-profile/custom execution availability, permanent deletion, or cleanup history changed.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight steps passed: restore, build, core tests, WPF app tests, fixture generator `-WhatIf`, fixture checklist-only output, and whitespace diff check.
- The preflight output confirmed no real user files were scanned or modified.

Docs updated:

- `docs/features/2026-05-30-confirmation-field-help-cues.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence and handoff correction only.

Open questions:

- During visible fixture review, confirm the extra confirmation-field cues improve discovery without crowding the Quarantine Shortlist or selected restore rows.

### 2026-05-30: Confirmation Field Help Cues

Status: completed

Evidence:

- User preferred a little question mark in a circle as a hover cue for tooltip-only help.
- The app already uses non-clickable circular `?` help cues for safety and gate text.
- Exact-confirmation fields still relied mostly on hovering the text box itself to discover `QUARANTINE` / `RESTORE` boundaries.

Implementation:

- Added visible circular `?` help cues beside the shortlist confirmation field and selected restore confirmation field.
- Mirrored each confirmation field's existing tooltip and automation help text onto its cue.
- Expanded the WPF hoverable-help-cue affordance snapshot from thirteen to fifteen tracked cues.
- Updated README, domain context, glossary, feature brief, fixture checklist output, handoff, and progress.
- No scan behavior, Quarantine Preview behavior, Quarantine execution behavior, selected restore behavior, real-profile/custom execution availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the updated shortlist confirmation and selected restore confirmation `?` help-cue prompts without preflight, fixture creation, WPF launch, scan, or file modification.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-30-confirmation-field-help-cues.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `tools/Start-MvpFixtureReview.ps1`

ADRs:

- No ADR added. This is reversible WPF affordance/help-text polish with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm the extra confirmation cues improve discoverability without crowding the Quarantine Shortlist or selected restore rows.

### 2026-05-30: Restore Manifest Review Surface Decision

Status: completed

Evidence:

- The handoff listed deciding whether future broader restore/history design should put discovered Restore Manifest entries in a separate tab/grid or keep them in manifest discovery/readiness panes as a good next packet.
- Current-Session Quarantined Review solved the immediate fixture visibility issue for current in-memory moved entries.
- Quarantine Manifest Discovery, Selected Restore Manifest Review, Selected Restore Execution Gate, Fixture-only Selected Restore Execution, and Restore Readiness Preview already cover older/discovered action-scoped manifests without broad real-profile restore.

Implementation:

- Added ADR 0016 accepting that older/discovered Restore Manifest review stays in manifest discovery/readiness panes for now.
- Clarified that `Current quarantined` remains current-session-only and should not silently become all quarantined history or a broad discovered-manifest grid.
- Updated README, domain context, glossary, handoff, progress, and a feature brief.
- No WPF layout, scan behavior, Quarantine behavior, selected restore behavior, real-profile/custom restore availability, permanent deletion, or cleanup history changed.

Verification:

- `rg -n "Current quarantined|cleanup history|all quarantined history" README.md docs` completed and showed the updated boundaries.
- `git diff --check` passed.

Docs updated:

- `README.md`
- `docs/decisions/0016-keep-discovered-manifests-in-manifest-panes.md`
- `docs/features/2026-05-30-restore-manifest-review-surface-decision.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- Added ADR 0016 because the choice affects core UX flow, has multiple plausible options, and reversing it later would require WPF wording/view-model/test/docs changes.

Open questions:

- If broader browsing is needed later, should it be a dedicated tab, a separate grid under manifest discovery, or another view?

Rejected ideas buffer:

- Do not silently expand `Current quarantined` into all quarantined history.
- Do not add a broad restore/history view before a separate Grill with Docs pass.

### 2026-05-30: Fixture Checklist Selected Restore Gate Cue

Status: completed

Evidence:

- Selected Restore Gate Help Cue automated the cue behavior but left a visual-only follow-up: confirm the cue is noticeable without crowding the selected restore gate area.
- The next recommended work remains manual fixture visual polish.
- The fixture checklist is the current user-facing guide for visible fixture review.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist step 8 to check the Selected Restore Execution Gate `?` help cue in waiting, closed, open, and restored states and to watch for crowding.
- Updated README fixture smoke and Manual MVP wording to match the checklist.
- Added a feature brief for the checklist alignment packet.
- No WPF behavior, scan behavior, Quarantine behavior, selected restore behavior, real-profile/custom restore availability, permanent deletion, or cleanup history changed.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed without preflight, fixture creation, WPF launch, scan, or file modification.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed without preflight, fixture creation, WPF launch, scan, or file modification.
- `git diff --check` passed.

Docs updated:

- `README.md`
- `docs/features/2026-05-30-fixture-checklist-selected-restore-gate-cue.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `tools/Start-MvpFixtureReview.ps1`

ADRs:

- No ADR added. This is checklist/documentation wording only and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- The next visible fixture review should decide whether the selected restore gate cue placement needs WPF layout polish.

Rejected ideas buffer:

- Do not add another checklist step for this cue unless the visible review shows step 8 is too dense.

### 2026-05-30: Run Full Local MVP Preflight After Selected Restore Gate Cue

Status: completed

Evidence:

- Selected Restore Gate Help Cue changed WPF XAML, WPF selected restore gate tooltip/help text, WPF smoke assertions, fixture checklist wording, and domain/handoff docs.
- A full local preflight through the preferred `.cmd` wrapper verifies the complete restore/build/test/fixture/checklist/diff path after that help-cue packet.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the selected restore gate help-cue packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist in checklist-only mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight output included `== Fixture checklist ==` and printed checklist step 8 with `Selected Restore Execution Gate ? help cue`.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/features/2026-05-30-selected-restore-gate-help-cue.md`
- `docs/domain/context.md`

ADRs:

- No ADR added. This is verification evidence plus a `Last reviewed` date correction only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains a visible fixture review pass using the updated checklist and Selected Restore Execution Gate help cue.

Rejected ideas buffer:

- Do not treat preflight as a replacement for visible UI review; it proves automated gates and checklist printability only.

### 2026-05-30: Selected Restore Gate Help Cue

Status: completed

Evidence:

- User feedback favored a small circular `?` help cue as a clearer hover target for important gate tooltip text.
- The Selected Restore Execution Gate is another exact-confirmation safety boundary with fixture-only versus preview-only behavior.
- Existing WPF patterns use non-clickable circular `?` cues with Help cursor, prompt tooltip delay, mirrored tooltip/help text, and smoke coverage.

Implementation:

- Added a visible circular `?` cue beside the Selected Restore Execution Gate readout.
- Added concise dynamic gate help text for waiting, closed, open, restored, and custom/real-profile blocked states.
- Mirrored gate help text onto the readout tooltip, readout automation help text, help cue tooltip, and help cue automation help text.
- Expanded WPF smoke affordance coverage from twelve to thirteen tracked circular help cues.
- Updated the fixture checklist to call out the new Selected Restore Execution Gate help cue.
- No scan behavior, Quarantine behavior, selected restore semantics, real-profile/custom restore availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed without preflight, fixture creation, WPF launch, scan, or file modification.
- `git diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-30-selected-restore-gate-help-cue.md`
- `docs/codex/thread-handoff.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`

ADRs:

- No ADR added. This is WPF affordance/help-text polish only and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- A visible fixture review should confirm the new cue is clear without crowding the selected restore gate area.

Rejected ideas buffer:

- Do not turn the gate help cue into a modal or action button unless visual review shows hover help is still insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Gate Help Cue

Status: completed

Evidence:

- Quarantine Execution Gate Help Cue changed WPF XAML, WPF gate tooltip/help text, WPF smoke assertions, and fixture checklist wording.
- A full local preflight through the preferred `.cmd` wrapper verifies the complete restore/build/test/fixture/checklist/diff path after that help-cue packet.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the gate help-cue packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist in checklist-only mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight output included `== Fixture checklist ==` and printed checklist step 6 with `the Quarantine Execution Gate ? help cue`.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/features/2026-05-30-quarantine-execution-gate-help-cue.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains a visible fixture review pass using the updated checklist and Quarantine Execution Gate help cue.

Rejected ideas buffer:

- Do not treat preflight as a replacement for visible UI review; it proves automated gates and checklist printability only.

### 2026-05-30: Quarantine Execution Gate Help Cue

Status: completed

Evidence:

- User feedback favored a small circular `?` help cue as a clearer hover target for important gate tooltip text.
- The Quarantine Execution Gate already had visible state text plus disabled-control tooltips, but the gate itself did not have its own always-visible help affordance.
- Existing WPF patterns use non-clickable circular `?` cues with Help cursor, prompt tooltip delay, mirrored tooltip/help text, and smoke coverage.

Implementation:

- Added a visible circular `?` cue beside the Quarantine Execution Gate readout.
- Added concise dynamic gate help text for startup, closed, open, executed, and undone states.
- Mirrored gate help text onto the readout tooltip, readout automation help text, help cue tooltip, and help cue automation help text.
- Expanded WPF smoke affordance coverage from eleven to twelve tracked circular help cues.
- Updated the fixture checklist to call out the new Quarantine Execution Gate help cue.
- No scan behavior, Quarantine Preview eligibility, fixture execution, undo, selected restore, real-profile availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed without preflight, fixture creation, WPF launch, scan, or file modification.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-30-quarantine-execution-gate-help-cue.md`
- `docs/codex/thread-handoff.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`

ADRs:

- No ADR added. This is WPF affordance/help-text polish only and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- A visible fixture review should confirm the new cue is clear without crowding the Quarantine Shortlist panel.

Rejected ideas buffer:

- Do not turn the gate help cue into a modal or action button unless visual review shows hover help is still insufficient.

### 2026-05-30: Quarantine Execution Gate Preview Button Label

Status: completed

Evidence:

- The visible preview button is `Preview shortlist quarantine`.
- The WPF Quarantine Execution Gate displayed the core missing-preview blocker `Create a Quarantine Preview before entering confirmation text.`
- Recent placeholder polish already aligned nearby startup/reset placeholders to `Preview shortlist quarantine`.

Implementation:

- Added a WPF display formatter for Quarantine Execution Gate blockers.
- Translated only the missing-preview blocker to `Use Preview shortlist quarantine before entering confirmation text.`
- Updated WPF smoke assertions for startup and stale-preview reset gate wording.
- Left core Quarantine Execution Gate builder behavior unchanged.
- No scan behavior, Quarantine Preview eligibility, fixture execution, undo, selected restore, real-profile availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `rg -n "Create a Quarantine Preview before entering confirmation text|Preview quarantine\." src\WindowsFileCleaner.App\MainWindow.xaml README.md docs\codex\thread-handoff.md tools` returned no current-facing matches.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `docs/features/2026-05-30-quarantine-execution-gate-preview-button-label.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is WPF display wording polish only and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Rejected ideas buffer:

- Do not change the core Quarantine Execution Gate blocker just to match WPF button wording; non-UI domain wording can remain generic.

### 2026-05-30: Run Full Local MVP Preflight After Placeholder Label

Status: completed

Evidence:

- Quarantine Preview Placeholder Label changed WPF XAML, WPF reset text, and WPF smoke assertions.
- A full local preflight through the preferred `.cmd` wrapper verifies the complete restore/build/test/fixture/checklist/diff path after that placeholder wording packet.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the placeholder-label packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist in checklist-only mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight output included `== Fixture checklist ==` and printed `Checklist-only mode. No preflight, fixture creation, or WPF launch will run.`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/features/2026-05-30-quarantine-preview-placeholder-label.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains a visible fixture review pass using the updated checklist and placeholder wording.

Rejected ideas buffer:

- Do not treat preflight as a replacement for visible UI review; it proves automated gates and checklist printability only.

### 2026-05-30: Quarantine Preview Placeholder Label

Status: completed

Evidence:

- The visible preview button is `Preview shortlist quarantine`.
- The WPF Quarantine Preview startup/reset placeholder and hardcoded Quarantine Execution Gate placeholder still said `Preview quarantine`.
- `ClearQuarantinePreview` reset the preview pane to the same older placeholder after stale preview invalidation.
- The current next-review prompt also still said `Preview quarantine/export tooltips`.

Implementation:

- Updated WPF startup/reset placeholder text for the Quarantine Preview pane and the hardcoded Quarantine Execution Gate placeholder to use `Preview shortlist quarantine`.
- Updated the stale-preview reset path to use `Preview shortlist quarantine`.
- Added WPF smoke assertions for startup and stale-reset placeholder text, including a guard against the old preview button phrase in execution-gate startup/reset text.
- Updated the next recommended manual review prompt to call out `Preview shortlist quarantine` and `Export preview` tooltips.
- No scan behavior, Quarantine Preview eligibility, fixture execution, undo, selected restore, real-profile availability, permanent deletion, or cleanup history changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `rg -n "after using Preview quarantine|Preview quarantine/export" src tests README.md docs\codex\thread-handoff.md tools` returned no matches.
- `rg -n "Preview quarantine\." src\WindowsFileCleaner.App README.md docs\codex\thread-handoff.md tools` returned no matches.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `docs/features/2026-05-30-quarantine-preview-placeholder-label.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is WPF wording polish only and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Rejected ideas buffer:

- Do not rename historical feature notes that describe the older pre-rename labels; only current-facing UI/checklist prompts need the visible label.

### 2026-05-30: Run Full Local MVP Preflight After Checklist Label

Status: completed

Evidence:

- Fixture Checklist Quarantine Button Label changed `Start-MvpFixtureReview.ps1`, which the full MVP preflight invokes in checklist-only mode.
- A full local preflight through the preferred `.cmd` wrapper verifies the complete restore/build/test/fixture/checklist/diff path after that checklist wording packet.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the checklist-label packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist in checklist-only mode, and ran whitespace diff checking.
- The preflight checklist output included step 7 with `click Quarantine included shortlist`.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight output included `== Fixture checklist ==` and printed `Checklist-only mode. No preflight, fixture creation, or WPF launch will run.`
- Preflight output included `click Quarantine included shortlist` in fixture checklist step 7.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/features/2026-05-30-quarantine-execution-label-doc-alignment.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains a visible fixture review pass using the updated checklist.

Rejected ideas buffer:

- Do not treat preflight as a replacement for visible UI review; it proves automated gates and checklist printability only.

### 2026-05-30: Fixture Checklist Quarantine Button Label

Status: completed

Evidence:

- Full preflight checklist output still said `execute quarantine` generically in step 7.
- The visible WPF button, README, progress prompt, handoff, domain, glossary, and smoke-tested user-facing label are `Quarantine included shortlist`.

Implementation:

- Updated `Start-MvpFixtureReview.ps1 -ChecklistOnly` step 7 to say `click Quarantine included shortlist`.
- Updated the Quarantine execution label docs-alignment feature note with the later checklist evidence.
- No app behavior, scan behavior, Quarantine Preview, fixture execution, undo, selected restore, real-profile availability, permanent deletion, or cleanup history changed.

Verification:

- `.\tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed `Quarantine included shortlist` in step 7 without preflight, fixture creation, WPF launch, scan, or file modification.
- `rg -n "execute quarantine" tools README.md docs\codex\thread-handoff.md docs\domain\context.md docs\features\2026-05-30-quarantine-execution-label-doc-alignment.md` returned no current-facing matches.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `docs/features/2026-05-30-quarantine-execution-label-doc-alignment.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is manual checklist wording alignment only and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Rejected ideas buffer:

- Do not rename code symbols or historical feature notes solely for this checklist alignment.

### 2026-05-30: Run Full Local MVP Preflight After Quarantine Gate Wording

Status: completed

Evidence:

- Quarantine Gate Technical Wording changed WPF Quarantine Preview and Quarantine Execution Gate output plus WPF smoke assertions.
- A full local preflight through the preferred `.cmd` wrapper verifies the complete restore/build/test/fixture/checklist/diff path after that wording packet.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the Quarantine gate wording packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist in checklist-only mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight output included `== Fixture checklist ==` and printed `Checklist-only mode. No preflight, fixture creation, or WPF launch will run.`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/features/2026-05-30-quarantine-gate-technical-wording.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains a visible fixture review pass against the current help-cue, collapsible-panel, Quarantine Preview, execution-gate, and current-session quarantined wording.

Rejected ideas buffer:

- Do not treat preflight as a replacement for visible UI review; it proves automated gates and checklist printability only.

### 2026-05-30: Quarantine Gate Technical Wording

Status: completed

Evidence:

- Quarantine Execution Scope Status kept the technical `Execution implemented` line only for continuity.
- The Quarantine Preview and Quarantine Execution Gate panes already have plain-language `Execution scope status`, `Approval boundary`, and `Can execute` lines.
- Selected Restore Gate Technical Wording made the same simplification successfully.

Implementation:

- Removed `Execution implemented` from WPF Quarantine Preview and Quarantine Execution Gate display text.
- Kept the internal `IsExecutionImplemented` flag as the source for Quarantine Execution Scope Status and execution availability.
- Updated WPF smoke tests so fixture Quarantine execution and custom blockers still pass while Quarantine panes no longer expose the technical line.
- Updated domain and feature notes.
- Did not change Storage Scan, Quarantine Preview eligibility, fixture Quarantine execution semantics, current-fixture undo, selected restore, manifest discovery, restore readiness, real-profile availability, permanent deletion, or cleanup history.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `docs/domain/context.md`
- `docs/features/2026-05-29-quarantine-execution-scope-status.md`
- `docs/features/2026-05-29-quarantine-approval-boundary-wording.md`
- `docs/features/2026-05-30-quarantine-gate-technical-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is WPF wording polish only and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Rejected ideas buffer:

- Do not remove internal `IsExecutionImplemented` flags; only remove the user-facing technical wording where plain scope-status lines carry the meaning.

### 2026-05-30: Selected Restore Gate Technical Wording

Status: completed

Evidence:

- Selected Restore Scope Status had intentionally left open whether the technical `Execution implemented` line should remain.
- The selected restore pane already has plain-language `Execution scope status`, `Approval boundary`, and `Can execute` lines.
- README manual review checks focus on fixture-only scope status, approval-boundary wording, disabled-control wording, and `Can execute`, not implementation internals.

Implementation:

- Removed `Execution implemented` from WPF Selected Restore Confirmation Draft/Gate display text.
- Kept the internal `IsExecutionImplemented` flag as the source for selected restore scope-status and execution availability.
- Updated WPF smoke tests so fixture selected restore and custom blockers still pass while the selected restore pane no longer exposes the technical line.
- Updated domain and feature notes.
- Did not change Storage Scan, Quarantine Preview, fixture Quarantine execution, current-fixture undo, selected restore semantics, manifest discovery, restore readiness, real-profile availability, permanent deletion, or cleanup history.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `docs/domain/context.md`
- `docs/features/2026-05-29-selected-restore-scope-status.md`
- `docs/features/2026-05-30-selected-restore-gate-technical-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is WPF wording polish only and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Rejected ideas buffer:

- Do not remove the technical `Execution implemented` wording from Quarantine execution panes in this packet; those panes have a separate manual-review history and should be changed only after a targeted Quarantine gate wording pass.

### 2026-05-30: Quarantine Execution Label Docs Alignment

Status: completed

Evidence:

- The visible fixture action and WPF smoke coverage use `Quarantine included shortlist`.
- A few current-facing manual-review docs still used the older generic execution label, which could send the next fixture pass hunting for the wrong button.

Implementation:

- Aligned current-facing README, progress, handoff, domain, and feature-note wording with the visible `Quarantine included shortlist` label.
- Kept historical feature context where it describes the older label before the wording packet changed it.
- No app behavior, scan behavior, Quarantine execution, restore behavior, or real-profile availability changed.

Verification:

- `rg -n --fixed-strings "Execute quarantine" README.md docs\codex\thread-handoff.md docs\domain\context.md docs\features\2026-05-29-execution-control-tooltip-clarity.md docs\features\2026-05-29-execution-readiness-automation-help-text.md` returned no current-facing label matches.
- `git diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/features/2026-05-29-execution-control-tooltip-clarity.md`
- `docs/features/2026-05-29-execution-readiness-automation-help-text.md`
- `docs/features/2026-05-30-quarantine-execution-label-doc-alignment.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is wording alignment only and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Rejected ideas buffer:

- Do not rename code symbols solely for this wording packet; the visible label is aligned and a code rename would add churn without behavior value.

### 2026-05-30: CI Preflight CMD Wrapper Alignment

Status: completed

Evidence:

- User-facing instructions prefer `.\tools\Invoke-MvpPreflight.cmd` because direct `.ps1` execution can be blocked by local PowerShell execution policy.
- GitHub Actions still called `.\tools\Invoke-MvpPreflight.ps1` directly, so CI did not prove the preferred wrapper entry point.

Implementation:

- Updated `.github/workflows/mvp-preflight.yml` so the MVP Preflight job runs `tools\Invoke-MvpPreflight.cmd` under `cmd`.
- Kept the existing preflight script and all preflight steps unchanged.
- Updated README, feature notes, progress, and handoff docs.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore` passed.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-ci-mvp-preflight.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/features/2026-05-30-execution-policy-friendly-tool-wrappers.md`
- `docs/features/2026-05-30-preflight-fixture-checklist-step.md`
- `docs/features/2026-05-30-ci-preflight-cmd-wrapper.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible CI verification tooling and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Rejected ideas buffer:

- Do not duplicate the preflight command list in YAML; keep CI pointed at the shared wrapper/script path.

### 2026-05-30: Run Full Local MVP Preflight After Checklist Step

Status: completed

Evidence:

- Preflight Fixture Checklist Step changed `tools\Invoke-MvpPreflight.ps1`, README, MVP readiness/preflight docs, progress, and handoff.
- A full local preflight through the preferred `.cmd` wrapper verifies the new checklist-only step runs in the complete restore/build/test/fixture/diff path before the next visible fixture review.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the checklist-step packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, printed the fixture checklist in checklist-only mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight output included `== Fixture checklist ==` and printed `Checklist-only mode. No preflight, fixture creation, or WPF launch will run.`
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/features/2026-05-30-preflight-fixture-checklist-step.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains a visible fixture review pass against the checklist prompts now covered by preflight.

Rejected ideas buffer:

- Do not treat preflight as a replacement for visible UI review; it proves automated gates and checklist printability only.

### 2026-05-30: Preflight Fixture Checklist Step

Status: completed

Evidence:

- The manual fixture checklist now carries many safety-review prompts and had only focused checklist-only verification in individual packets.
- Full MVP preflight is the shared local/CI gate before real-profile scans, so it should prove checklist-only output can still run before visible review.

Implementation:

- Added `Start-MvpFixtureReview.ps1 -ChecklistOnly` as a `Fixture checklist` step in `tools\Invoke-MvpPreflight.ps1`.
- Added `-SkipFixtureChecklist` for focused local loops.
- Updated README, MVP preflight feature notes, and readiness audit wording to include checklist-only coverage.
- Kept checklist-only behavior terminal-only: no recursive preflight, no fixture creation, and no WPF launch.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore` passed.
- The preflight output included `== Fixture checklist ==` and printed the checklist-only mode text.
- The preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-30-preflight-fixture-checklist-step.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local verification tooling only and does not change architecture, persistence, cleanup execution, restore rule, data-model, or security.

Open questions:

- None.

Rejected ideas buffer:

- Do not make preflight create the fixture or launch WPF to validate the checklist; visible review should stay explicit.

### 2026-05-30: Run Full Local MVP Preflight After Cleanup Scope Cue

Status: completed

Evidence:

- Cleanup Scope Safety Note Help Cue changed WPF header layout, dynamic help text, WPF smoke assertions, fixture checklist wording, README, domain docs, feature notes, progress, and handoff.
- A full local preflight through the preferred `.cmd` wrapper gives stronger evidence that restore, build, test, fixture dry-run, and whitespace gates still pass after the Cleanup Scope Safety Note cue packet.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the Cleanup Scope Safety Note help-cue packet was pushed.
- The first sandboxed attempt failed during restore because the sandbox could not read `C:\Users\moxhe\AppData\Roaming\NuGet\NuGet.Config`.
- Reran the same `.cmd` preflight with approved access so `dotnet restore` could read the normal user NuGet config.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed after approved access.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/features/2026-05-30-cleanup-scope-safety-note-help-cue.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains a visible fixture or real-profile review pass against the Cleanup Scope Safety Note `?` cue, scan-gate summary `?` cue, and the rest of the hoverable help cues.

Rejected ideas buffer:

- Do not treat preflight as a replacement for visible UI review; it proves automated gates only.

### 2026-05-30: Cleanup Scope Safety Note Help Cue

Status: completed

Evidence:

- User feedback favored visible circular `?` hover targets for important tooltip text.
- The Cleanup Scope Safety Note is the first header-level safety context that tells the user whether the entered scope is fixture, real-profile, custom, blank, or invalid.

Implementation:

- Added a visible hoverable `?` cue beside the Cleanup Scope Safety Note.
- Mirrored the dynamic safety note into the note tooltip, note automation help text, cue tooltip, and cue automation help text.
- Expanded WPF smoke coverage from ten to eleven tracked circular help-cue affordances.
- Added WPF smoke assertions for real-profile, fixture, and custom Cleanup Scope Safety Note cue wording.
- Updated the manual fixture checklist to call out the Cleanup Scope Safety Note cue.
- No real user files were scanned or modified.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-safety-note.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance-coverage.md`
- `docs/features/2026-05-30-cleanup-scope-safety-note-help-cue.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF UI affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm the header remains readable with both Cleanup Scope Safety Note and scan-gate summary `?` cues.

Rejected ideas buffer:

- Do not turn the Cleanup Scope Safety Note into another confirmation gate unless future real-profile review shows the current scan gate and acknowledgement are not enough.

### 2026-05-30: Quarantine Root Safety Note Help Cue

Status: completed

Evidence:

- User feedback favored visible circular `?` hover targets for important tooltip text.
- The Quarantine Root Safety Note is safety-sensitive preview-root wording and was easy to miss beside the larger Quarantine Shortlist controls.

Implementation:

- Added a visible hoverable `?` cue beside the Quarantine Root Safety Note.
- Mirrored the dynamic safety note into the note tooltip, note automation help text, cue tooltip, and cue automation help text.
- Expanded WPF smoke coverage from nine to ten tracked circular help-cue affordances.
- Updated the manual fixture checklist to call out the Quarantine Root browse tooltip plus safety-note help cue.
- No real user files were scanned or modified.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-root-safety-note.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance.md`
- `docs/features/2026-05-30-quarantine-root-safety-note-help-cue.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF UI affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm the extra `?` cue improves discovery without crowding the Quarantine Shortlist area.

Rejected ideas buffer:

- Do not turn the safety note into a popup or approval step unless user testing shows the inline cue is still too easy to miss.

### 2026-05-30: Run Full Local MVP Preflight After Scan Gate Cue

Status: completed

Evidence:

- Scan Gate Summary Help Cue changed WPF layout, dynamic help text, WPF smoke assertions, fixture checklist wording, README, domain docs, feature notes, progress, and handoff.
- A full local preflight through the preferred `.cmd` wrapper gives stronger evidence that restore, build, test, fixture dry-run, and whitespace gates still pass after the scan-gate help-cue packet.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the scan-gate summary help-cue packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/features/2026-05-30-scan-gate-summary-help-cue.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains a visible fixture or real-profile review pass against the scan-gate summary `?` cue and the rest of the hoverable help cues.

Rejected ideas buffer:

- Do not treat preflight as a replacement for visible UI review; it proves automated gates only.

### 2026-05-30: Scan Gate Summary Help Cue

Status: completed

Evidence:

- User feedback favored visible circular `?` help cues as clearer hover targets for important tooltip text.
- The Cleanup Scope Scan Gate summary is safety-critical and already visible, but its dynamic boundary text was split between the visible summary and Scan button tooltip/help text.

Implementation:

- Added a visible circular `?` help cue beside the scan-gate summary.
- Added dynamic tooltip and automation help text to the scan-gate summary.
- Mirrored the scan-gate summary help text onto the cue.
- Covered locked real-profile, acknowledged real-profile, fixture, and custom scan-gate summary help text in WPF smoke tests.
- Expanded the tracked hoverable help-cue affordance list from eight to nine cues.
- Updated the fixture review checklist to prompt checking the scan-gate `?` cue.
- Kept Cleanup Scope Scan Gate behavior unchanged: no preflight run, no fixture creation, no automatic scan, no persistence, no cleanup approval, and no real-profile cleanup execution.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-scan-gate-discoverability-polish.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance-coverage.md`
- `docs/features/2026-05-30-scan-gate-summary-help-cue.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-affordance polish with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture or real-profile pass, confirm the extra cue does not crowd the header.

Rejected ideas buffer:

- Do not replace the scan-gate summary with a popup; the scan gate remains visible inline text plus tooltip/help cue.

### 2026-05-30: Real Profile Acknowledgement Help Cue

Status: completed

Evidence:

- User feedback favored a small circular `?` help cue as a clearer hover target for important tooltip text.
- The real-profile preflight and fixture-review acknowledgement already had tooltip and automation help text but no visible cue.

Implementation:

- Added a visible circular `?` help cue beside the real-profile acknowledgement checkbox.
- Mirrored the checkbox tooltip and automation help text on the cue.
- Kept the cue hidden for fixture and custom Cleanup Scopes.
- Expanded the tracked hoverable help-cue affordance list from seven to eight cues.
- Kept Cleanup Scope Scan Gate behavior unchanged: checking the acknowledgement does not run preflight, create fixtures, start scanning, persist approval, or approve cleanup.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-real-profile-acknowledgement-help-text.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance-coverage.md`
- `docs/features/2026-05-30-real-profile-acknowledgement-help-cue.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-affordance polish with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next real-profile visual pass, confirm the cue is noticeable without crowding the header.

Rejected ideas buffer:

- Do not add a popup or modal help flow for the real-profile acknowledgement; the scan gate remains visible text plus tooltip/help cue.

### 2026-05-30: Run Full Local MVP Preflight After Current Quarantined Count

Status: completed

Evidence:

- Current Quarantined Count Label touched WPF app code, WPF smoke assertions, fixture checklist wording, README, domain docs, feature notes, progress, and handoff.
- A full local preflight through the preferred `.cmd` wrapper gives stronger evidence that restore, build, test, fixture dry-run, and whitespace gates still pass before the next visible fixture review.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the current quarantined count-label packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains a visible fixture review pass using the `.cmd` launcher, including `Current quarantined (N)` and the hoverable `?` help cues.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visible fixture review; it proves automated gates only.

### 2026-05-30: Current Quarantined Count Label

Status: completed

Evidence:

- Current-Session Quarantined Review already showed moved-entry counts in tooltip/help text and Review Grid Mode Status.
- The visible grid-switch button still stayed at `Current quarantined` after fixture execution, so the count required hovering or reading adjacent status text.

Implementation:

- Updated the visible grid-switch label to remain `Current quarantined` when no current-session moved entries exist.
- Updated the label to show `Current quarantined (N)` when current-session moved Restore Manifest entries are available.
- Added WPF smoke coverage for the post-execution count label.
- Updated the manual fixture checklist to prompt checking that the count appears after fixture Quarantine execution.
- Kept Current-Session Quarantined Review read-only and current-session-only; older/discovered Restore Manifests still route through `Discover manifests` and readiness panes.
- Did not change Storage Scan, Quarantine Preview, fixture execution, fixture undo, selected restore, manifest discovery, real-profile execution availability, permanent deletion, or cleanup history.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the moved-entry count prompt without preflight, fixture creation, or WPF launch.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-30-current-quarantined-count-label.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI wording and test coverage for existing current-session-only behavior with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm `Current quarantined (N)` is useful without crowding the Quarantine Shortlist toolbar.
- Later ADR 0016 decided that older/discovered Restore Manifest review stays in manifest discovery/readiness panes for now, and `Current quarantined` remains current-session-only rather than all quarantined history.

Rejected ideas buffer:

- Do not put discovered manifest counts on the current-session button; that would blur current-session review with older-manifest discovery/history.

### 2026-05-30: Run Full Local MVP Preflight After Help Cue Coverage

Status: completed

Evidence:

- Hoverable Help Cue Affordance Coverage touched WPF app code, WPF smoke assertions, and durable docs after the previous full `.cmd` preflight.
- A full local preflight through the preferred `.cmd` wrapper gives stronger evidence that restore, build, test, fixture dry-run, and whitespace gates still pass before the next visible fixture review.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the hoverable help-cue affordance coverage packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains a visible fixture review pass using the `.cmd` launcher and updated hoverable help-cue checklist wording.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visible fixture review; it proves automated gates only.

### 2026-05-30: Hoverable Help Cue Affordance Coverage

Status: completed

Evidence:

- Hoverable Help Cue Affordance added the Windows Help cursor and prompt tooltip delay to the seven existing circular `?` cues.
- Fixture Checklist Hoverable Help Cues made the next manual pass explicitly check hoverable `?` cues.
- Existing WPF smoke assertions covered tooltip/help-text content, but did not directly verify the cursor or prompt tooltip-delay affordance.

Implementation:

- Exposed a small test-facing snapshot of the seven circular help-cue affordance settings from `MainWindow`.
- Added a WPF smoke assertion that the Safety Summary header, Review Mix, Matched Review Mix, Review Shortlist Safety Mix, Quarantine Shortlist header, inline Quarantine Preview status, and Review Grid Mode Status cues all use the Help cursor and `250` ms tooltip initial delay.
- Did not change visible WPF layout, tooltip wording, automation help text, scan behavior, Quarantine Preview, fixture execution, undo, restore, manifests, real-profile cleanup availability, or cleanup history.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

Docs updated:

- `docs/features/2026-05-30-hoverable-help-cue-affordance.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance-coverage.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is test coverage for existing WPF UI affordance behavior with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the hoverable `?` cues and prompt tooltips are noticeable without making the dense review surface noisy.

Rejected ideas buffer:

- Do not rely on manual checklist wording alone for stable affordance behavior when a focused WPF smoke assertion can cover it cheaply.

### 2026-05-30: Run Full Local MVP Preflight After Hoverable Checklist

Status: completed

Evidence:

- Fixture Checklist Hoverable Help Cues changed the manual fixture launcher checklist and durable docs after the previous full `.cmd` preflight.
- A full local preflight through the preferred `.cmd` wrapper gives stronger evidence that the current command path still restores, builds, tests, dry-runs the fixture generator, and runs whitespace checks cleanly before the next visible fixture review.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the hoverable checklist packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.
- `git status --short` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains a visible fixture review pass using the `.cmd` launcher and updated hoverable help-cue checklist wording.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visible fixture review; it proves automated gates only.

### 2026-05-30: Fixture Checklist Hoverable Help Cues

Status: completed

Evidence:

- Hoverable Help Cue Affordance added the Windows help cursor and prompt tooltip delay to the seven existing circular `?` help cues.
- The manual fixture checklist still described generic `? help cues`, which made the next visible review pass less precise than the current UI expectation.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist prompts to call the Review Mix, Matched Review Mix, Review Shortlist Safety Mix, Safety Summary header, Quarantine Shortlist header, inline Quarantine Preview readiness, and Review Grid Mode Status cues hoverable.
- Updated README launcher wording so the fixture smoke description matches the checklist.
- Recorded the wording alignment in the fixture checklist and hoverable help-cue feature briefs.
- Did not change WPF layout, controls, scan behavior, Quarantine Preview, fixture execution, undo, restore, manifests, real-profile cleanup availability, or cleanup history.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the hoverable-cue checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the same checklist through the original script path.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance.md`
- `docs/features/2026-05-30-fixture-checklist-hoverable-help-cues.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is checklist/documentation wording only with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the hoverable `?` cues and prompt tooltips are noticeable without making the dense review surface noisy.

Rejected ideas buffer:

- Do not add more visible help controls from checklist wording alone; wait for visual review evidence.

### 2026-05-30: Run Full Local MVP Preflight Through CMD Wrapper

Status: completed

Evidence:

- Execution-Policy Friendly Tool Wrappers made `.\tools\Invoke-MvpPreflight.cmd` the preferred manual preflight command.
- The wrapper packet had already passed `Invoke-MvpPreflight.cmd -SkipRestore`; a full run verifies the complete preferred command path, including restore.

Implementation:

- Ran `Invoke-MvpPreflight.cmd` from the repository root after the tool-wrapper packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal remains visible fixture review using the `.cmd` launcher.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visible fixture review; it proves automated gates only.

### 2026-05-30: Execution-Policy Friendly Tool Wrappers

Status: completed

Evidence:

- The user hit PowerShell execution-policy blocking while running manual fixture review commands.
- The fixture launcher had a `.cmd` wrapper, but standalone preflight and synthetic fixture generation were still documented as direct `.ps1` commands.
- Preflight and synthetic fixture generation are human-facing safety/review workflows that benefit from the same process-scoped bypass path.

Implementation:

- Added `tools\Invoke-MvpPreflight.cmd` and `tools\New-StorageScanSmokeFixture.cmd`.
- Updated README and AGENTS to prefer `.cmd` for manual preflight, fixture generation, and fixture review commands.
- Updated the fixture-launcher feature note, progress, and handoff to record the broader wrapper pattern.
- Did not change Storage Scan, fixture creation semantics, Quarantine Preview, fixture execution, undo, selected restore, manifests, real-profile scan gates, or real-profile cleanup availability.

Verification:

- `cmd.exe /c tools\New-StorageScanSmokeFixture.cmd -WhatIf` passed and showed intended fixture writes only.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the manual fixture checklist without preflight, fixture creation, or WPF launch.
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore` passed build, core tests, WPF app tests, fixture dry run, and whitespace diff check.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-30-execution-policy-friendly-fixture-launcher.md`
- `docs/features/2026-05-30-execution-policy-friendly-tool-wrappers.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible local tooling with no app architecture, persistence, cleanup execution, restore rule, data-model, or security model change.

Open questions:

- None.

Rejected ideas buffer:

- Do not ask the user to change machine/user PowerShell execution policy for this repo; prefer the `.cmd` wrappers or explicit `powershell.exe -ExecutionPolicy Bypass -File ...`.

### 2026-05-30: Run Full Local MVP Preflight After CMD Launcher

Status: completed

Evidence:

- Execution-Policy Friendly Fixture Launcher added a `.cmd` first-hop wrapper and changed `Invoke-MvpPreflight.ps1` next-step output.
- A full local preflight gives stronger evidence that the new launcher packet did not disturb restore, build, tests, fixture dry-run, or whitespace gates.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the `.cmd` launcher packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- Preflight printed the updated next manual fixture step: `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight`.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass using `.\tools\Start-MvpFixtureReview.cmd`, including hoverable `?` help cues, collapsed panel header summaries/state styling, styled inline Quarantine Preview readiness, styled Review Grid Mode Status, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visible fixture review; it proves automated gates only.

### 2026-05-30: Execution-Policy Friendly Fixture Launcher

Status: completed

Evidence:

- User hit the Windows PowerShell `running scripts is disabled on this system` error more than once when trying to run the fixture review checklist.
- Direct `.ps1` invocation can fail before the existing fixture launcher has a chance to run its nested scripts with process-scoped `-ExecutionPolicy Bypass`.
- The next best work remains manual fixture visual review, so reducing this local tooling friction helps unblock better product feedback.

Implementation:

- Added `tools\Start-MvpFixtureReview.cmd`, which calls the existing PowerShell launcher with `powershell.exe -NoProfile -ExecutionPolicy Bypass` and forwards all arguments.
- Updated `Invoke-MvpPreflight.ps1` to suggest `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.
- Updated README, AGENTS, handoff, and fixture-checklist feature notes to offer the `.cmd` launcher while keeping the `.ps1` launcher available.
- Did not change Storage Scan, Quarantine Preview, fixture execution, undo, selected restore, manifests, real-profile scan gates, or real-profile cleanup availability.

Verification:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the manual fixture checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and preserved the original explicit-bypass path.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch` passed and forwarded arguments through the wrapper without creating fixture files or launching WPF.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1 -SkipRestore -SkipFixtureWhatIf` passed build, core tests, WPF app tests, whitespace diff check, and printed the updated `.cmd -SkipPreflight` next manual fixture step.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-execution-policy-friendly-fixture-launcher.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible local fixture-review tooling with no app architecture, persistence, cleanup execution, restore rule, data-model, or security model change.

Open questions:

- None.

Rejected ideas buffer:

- Do not ask the user to change machine/user PowerShell execution policy for this repo; prefer process-scoped bypass through the wrapper or explicit `powershell.exe -ExecutionPolicy Bypass -File ...`.

### 2026-05-30: Hoverable Help Cue Affordance

Status: completed

Evidence:

- User agreed that a little question mark in a circle that can be hovered for a tooltip would be better.
- The seven existing circular `?` help cues already had tooltip and automation help text, but the cursor did not explicitly signal hover-help behavior.

Implementation:

- Added the Windows help cursor and a short tooltip initial delay to the existing circular help cues for Safety Summary header, Review Mix, Matched Review Mix, Review Shortlist Safety Mix, Quarantine Shortlist header, inline Quarantine Preview status, and Review Grid Mode Status.
- Did not change tooltip wording, automation help text, scan behavior, Quarantine Preview, fixture execution, undo, restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-30-hoverable-help-cue-affordance.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF UI affordance polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the help cursor and prompt tooltip are noticeable without making the dense review surface feel noisy.

Rejected ideas buffer:

- Do not turn help cues into buttons or modal popups unless visual review shows the hover cue remains insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Header Cues

Status: completed

Evidence:

- Collapsed Header Help Cues touched WPF Expander header layout, dynamic help-text wiring, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the focused app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the collapsed header help-cue packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against all compact `?` help cues, collapsed panel header summaries/state styling, styled inline Quarantine Preview readiness, styled Review Grid Mode Status, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Collapsed Header Help Cues

Status: completed

Evidence:

- User said a little question mark in a circle that can be hovered over for the tooltip would be better than relying on hidden text hover alone.
- Safety Summary and Quarantine Shortlist headers already mirrored dynamic header summary/state into tooltip/help text, but the hover target was visually implicit.
- The existing Collapsed Panel Header Help Text feature brief left open whether safety-critical panel headers should get a small always-visible help affordance.

Implementation:

- Added visible circular `?` help cues beside Safety Summary and Quarantine Shortlist collapsed headers.
- Mirrored each dynamic header tooltip and automation help text onto its help cue from the existing header update path.
- Added WPF smoke assertions for the cue automation names and for cue tooltip/help text mirroring across startup, scan warning, needs-preview, invalid-root, ready preview, stale preview, blocked preview, current-session quarantined, and undo-completed header states.
- Did not change scan behavior, Quarantine Preview eligibility, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated collapsed header `?` help-cue prompts without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-30-collapsed-panel-header-help-text.md`
- `docs/features/2026-05-30-collapsed-header-state-help-text.md`
- `docs/features/2026-05-30-collapsed-header-summary-labels.md`
- `docs/features/2026-05-30-quarantine-shortlist-header-styling.md`
- `docs/features/2026-05-30-safety-summary-header-styling.md`
- `docs/features/2026-05-30-collapsed-header-help-cues.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the header `?` cues are noticeable without making the Expander headers feel crowded.

Rejected ideas buffer:

- Do not add popup help or larger badges for collapsed headers unless manual visual review shows compact cues are still insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Help Cues

Status: completed

Evidence:

- Review Grid Mode Status Help Cue and Quarantine Preview Status Help Cue touched WPF layout, dynamic help-text wiring, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the focused app-test runs before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the recent help-cue packets were pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against the compact `?` help cues, collapsed panel header summaries/state styling, styled inline Quarantine Preview readiness, styled Review Grid Mode Status, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Quarantine Preview Status Help Cue

Status: completed

Evidence:

- User said a little question mark in a circle that can be hovered over for the tooltip would be better than relying on hidden text hover alone.
- Inline Quarantine Preview readiness/status already mirrored dynamic preview state into tooltip/help text and named neutral/success/warning/error state.
- The remaining gap was discoverability for a safety-critical dry-run/approval-boundary tooltip.

Implementation:

- Added a visible circular `?` help cue beside inline Quarantine Preview readiness/status.
- Mirrored the dynamic tooltip and automation help text onto the help cue from the existing status update path.
- Added WPF smoke assertions for the cue automation name, cue tooltip, and cue automation help text across the existing empty, needs-preview, invalid-root, ready, stale, blocked, fixture-executed, and undo status states.
- Did not change scan behavior, Quarantine Preview eligibility, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated inline Quarantine Preview `?` help-cue prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-preview-inline-status.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/features/2026-05-30-quarantine-preview-status-help-text.md`
- `docs/features/2026-05-30-status-state-help-text.md`
- `docs/features/2026-05-30-quarantine-preview-status-help-cue.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the inline preview status `?` cue is noticeable without crowding the Quarantine Shortlist area.

Rejected ideas buffer:

- Do not add a popup, modal, or larger preview badge unless the compact cue and styled inline status are still insufficient after manual fixture review.

### 2026-05-30: Add Review Grid Mode Status Help Cue

Status: completed

Evidence:

- User said a little question mark in a circle that can be hovered over for the tooltip would be better than relying on hidden text hover alone.
- Review Grid Mode Status already mirrored dynamic grid-mode and status-state wording into tooltip/help text.
- The gap was discoverability, not the underlying safety wording.

Implementation:

- Added a visible circular `?` help cue beside Review Grid Mode Status.
- Mirrored the dynamic tooltip and automation help text onto the help cue.
- Added WPF smoke assertions for the cue automation name, cue tooltip, and cue automation help text across the existing startup, scan-row, stale scan-row, current-session quarantined, returned scan-row, and empty moved-entry states.
- Did not change scan behavior, grid switching, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Review Grid Mode Status `?` help-cue prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `docs/features/2026-05-30-review-grid-mode-status-help-text.md`
- `docs/features/2026-05-30-status-state-help-text.md`
- `docs/features/2026-05-30-review-grid-mode-status-help-cue.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the `?` cue is noticeable without crowding the main-grid area.

Rejected ideas buffer:

- This supersedes the earlier deferred idea to wait on a Review Grid Mode Status help icon because the user later preferred visible `?` cues for hidden safety tooltips.
- Do not add a popup, modal, or larger grid-mode badge unless the compact cue is still insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Safety Cue

Status: completed

Evidence:

- Review Shortlist Safety Mix Help Cue touched WPF layout, dynamic help text wiring, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the help-cue packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against the three `?` help cues, collapsed header summaries/state styling, styled inline Quarantine Preview readiness, styled Review Grid Mode Status, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Review Shortlist Safety Mix Help Cue

Status: completed

Evidence:

- User said a little question mark in a circle that can be hovered over for the tooltip would be better than relying on hidden text hover alone.
- Review Shortlist Safety Mix already mirrored dynamic summary wording into tooltip/help text.
- The gap was discoverability, not the underlying safety wording.

Implementation:

- Added a visible circular `?` help cue beside Review Shortlist Safety Mix.
- Mirrored the dynamic tooltip and automation help text onto the help cue.
- Added WPF smoke assertions for the cue automation name, cue tooltip, and cue automation help text across the existing empty, populated, and empty-after-removal shortlist states.
- Did not change scan behavior, Review Shortlist membership, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` initially failed when launched without quoting the path with spaces; reran as `& 'D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe'` and it passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Review Mix / Matched Review Mix / Review Shortlist Safety Mix `?` help-cue prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-shortlist-safety-mix.md`
- `docs/features/2026-05-30-review-shortlist-safety-mix-help-text.md`
- `docs/features/2026-05-30-review-shortlist-safety-mix-help-cue.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `tools/Start-MvpFixtureReview.ps1`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the `?` cue is noticeable without crowding the Review Shortlist toolbar area.

Rejected ideas buffer:

- Do not add a popup, modal, or larger Safety Mix table unless the compact cue is still insufficient.

### 2026-05-30: Add Review Mix Help Cues

Status: completed

Evidence:

- User said visible circular `?` help cues would be better than relying on hidden hover over Review Mix and Matched Review Mix text.
- Review Mix Help Text already mirrored dynamic summary wording into tooltip/help text.
- The gap was discoverability, not the underlying safety wording.

Implementation:

- Added visible circular `?` help cues beside Review Mix and Matched Review Mix.
- Mirrored the dynamic tooltip and automation help text onto both help cues.
- Added WPF smoke assertions for help-cue automation names, cue tooltips, and cue automation help text across the existing startup, completed-scan, descendant-focus, and prefixed-search states.
- Did not change scan behavior, filters, search, Review Shortlist, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Review Mix / Matched Review Mix `?` help-cue prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `docs/features/2026-05-29-matched-review-mix.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-review-mix-help-text.md`
- `docs/features/2026-05-30-review-mix-help-cues.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-cue polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the `?` cues are noticeable without crowding the dense review surface.

Rejected ideas buffer:

- Do not add a popup, modal, or extra help row for these summary lines unless the compact cues are still insufficient.

### 2026-05-30: Add Review Mix Help Text

Status: completed

Evidence:

- Review Mix and Matched Review Mix are read-only context readouts near the Safety Summary and review toolbar.
- Review Shortlist Safety Mix already mirrored dynamic wording into tooltip/help text with explicit non-approval boundaries.
- The top-level Review Mix readouts had visible non-approval wording but did not expose the same boundary through tooltip/help text.

Implementation:

- Added static startup tooltip and automation help text to Review Mix and Matched Review Mix.
- Added dynamic setters so completed-scan, descendant-focus, and prefixed-search summaries are mirrored into tooltip/help text.
- Help text says the readouts are read-only whole-scan or active-review-lens context and do not rescan, modify files, prove storage savings, or approve cleanup.
- Added WPF smoke assertions for startup, completed scan, descendant focus, and prefixed-search states.
- Did not change scan behavior, filters, search, Review Shortlist, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Review Mix / Matched Review Mix tooltip/help-text prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `docs/features/2026-05-29-matched-review-mix.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-review-mix-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture or real-profile review, confirm whether Review Mix / Matched Review Mix tooltip/help text is enough or whether either dense line needs a visible help affordance.

Rejected ideas buffer:

- Do not redesign Review Mix layout until manual review shows the compact text line is too dense.

### 2026-05-30: Add Review Shortlist Safety Mix Help Text

Status: completed

Evidence:

- Review Shortlist Safety Mix is a safety-context readout before Quarantine Preview.
- Neighboring Review Shortlist, preview, and status controls already expose tooltip and automation help text.
- Safety Mix visible text said it was review context and not cleanup approval, but that boundary was not available through tooltip/help text.

Implementation:

- Added static startup tooltip and automation help text to `ShortlistSafetyMixText`.
- Updated Review Shortlist Safety Mix updates so empty, populated, and empty-after-removal text is mirrored into tooltip/help text.
- Help text says the readout is read-only review context and does not rescan, modify files, prove Quarantine readiness, prove storage savings, or approve cleanup.
- Added WPF smoke assertions for empty, populated, and empty-after-removal states.
- Did not change scan behavior, Review Shortlist membership, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Review Shortlist Safety Mix tooltip/help-text prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-review-shortlist-safety-mix.md`
- `docs/features/2026-05-30-review-shortlist-safety-mix-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture or real-profile review, confirm whether Safety Mix tooltip/help text is enough or whether the dense line should become compact chips/table.

Rejected ideas buffer:

- Do not redesign the Safety Mix layout until manual review shows the compact text line is too dense.

### 2026-05-30: Run Full Local MVP Preflight After Header Summary Labels

Status: completed

Evidence:

- Collapsed Header Summary Labels touched WPF header text, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the header-summary-label packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against panel-name collapsed headers, collapsed-header state styling/help text, styled inline Quarantine Preview readiness help text, styled Review Grid Mode Status help text, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Collapsed Header Summary Labels

Status: completed

Evidence:

- User agreed that a header panel summary while closed would be useful.
- The existing collapsed headers already exposed compact summaries, state styling, and state-naming tooltip/help text.
- The visible prefixes were sentence-style and less directly tied to the panel names than the surrounding UI.

Implementation:

- Changed the Safety Summary header prefix to `Safety Summary:`.
- Changed the Quarantine Shortlist header prefix to `Quarantine Shortlist:`.
- Aligned the initial Quarantine Shortlist XAML header with the dynamic runtime summary by including `undo unavailable`.
- Added WPF smoke assertions that both collapsed headers start with their visible panel names.
- Did not change scan behavior, Review Shortlist behavior, Quarantine Preview, fixture execution, undo, selected restore, manifests, or real-profile cleanup availability.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed panel-name prefix checks for Safety Summary and Quarantine Shortlist collapsed headers without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-collapsed-header-state-help-text.md`
- `docs/features/2026-05-30-collapsed-header-summary-labels.md`
- `docs/features/2026-05-30-collapsed-panel-header-help-text.md`
- `docs/features/2026-05-30-quarantine-shortlist-header-styling.md`
- `docs/features/2026-05-30-safety-summary-header-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF header readability polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm whether the panel-name prefixes make the collapsed summaries easier to scan when both panels are collapsed.

Rejected ideas buffer:

- Do not add badges, help icons, or another visible row unless manual fixture review shows the panel-name prefixes and existing tooltip/help text are still insufficient.

### 2026-05-30: Add Status State Help Text

Status: completed

Evidence:

- Review Grid Mode Status and inline Quarantine Preview readiness/status already use visual state styling.
- Their tooltip/help text mirrored the visible status and safety boundary, but did not name the current semantic state.
- The previous collapsed-header state packet showed the value of avoiding color-only state cues.

Implementation:

- Added textual `Status state:` wording to Review Grid Mode Status tooltip/help text.
- Added textual `Status state:` wording to inline Quarantine Preview readiness/status tooltip/help text.
- Used safety-preserving labels: neutral, information, warning, success, and error.
- Kept the status lines compact; no new row, badge, modal, cleanup execution, restore behavior, persisted history, scan gate change, or real-profile file movement was added.
- Strengthened shared WPF smoke assertions so every exercised grid-mode and preview-status state must expose matching state text.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated state-naming status tooltip/help-text prompts without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/features/2026-05-29-review-grid-mode-status-styling.md`
- `docs/features/2026-05-30-quarantine-preview-status-help-text.md`
- `docs/features/2026-05-30-review-grid-mode-status-help-text.md`
- `docs/features/2026-05-30-status-state-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm whether state-naming status tooltip/help text is enough or whether safety-critical status lines need a small always-visible help affordance.

Rejected ideas buffer:

- Do not add another visible help icon or popup until manual fixture review shows tooltip/help text is insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Header State Help

Status: completed

Evidence:

- Collapsed Header State Help Text touched WPF header help text, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the collapsed-header state help-text packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against collapsed header state styling and state-naming tooltip/help text, styled inline Quarantine Preview readiness tooltip/help text, styled Review Grid Mode Status tooltip/help text, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Collapsed Header State Help Text

Status: completed

Evidence:

- Safety Summary and Quarantine shortlist headers now use visual state styling, but tooltip/help text only mirrored the compact summaries and safety boundary.
- Color-only state cues are less useful for tooltip inspection and automation/screen-reader review.

Implementation:

- Added textual `Header state:` wording to Safety Summary header tooltip/help text.
- Added textual `Header state:` wording to Quarantine shortlist header tooltip/help text.
- Used safety-preserving labels: neutral, needs review, ready or completed, and current-session quarantined review.
- Kept the headers compact; no new row, badge, modal, cleanup execution, restore behavior, persisted history, scan gate change, or real-profile file movement was added.
- Added WPF smoke assertions for neutral startup state, Safety Summary needs-review state, Quarantine shortlist needs-review state, preview-ready/completed state, and current-session quarantined review state.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated state-naming header tooltip/help-text prompts without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-collapsed-header-state-help-text.md`
- `docs/features/2026-05-30-collapsed-panel-header-help-text.md`
- `docs/features/2026-05-30-quarantine-shortlist-header-styling.md`
- `docs/features/2026-05-30-safety-summary-header-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm whether state-naming header tooltip/help text is enough or whether safety-critical panel headers still need a small always-visible help affordance.

Rejected ideas buffer:

- Do not add another visible help icon until manual fixture review shows the header tooltip/help text is insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Safety Header Styling

Status: completed

Evidence:

- Safety Summary Header Styling touched WPF header controls, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the Safety Summary header styling packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against collapsed header state styling, styled inline Quarantine Preview readiness tooltip/help text, styled Review Grid Mode Status tooltip/help text, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Safety Summary Header Styling

Status: completed

Evidence:

- User ran manual fixture checklist steps 1-11 successfully and agreed that a useful header panel summary while collapsed is desirable.
- Quarantine Shortlist Header Styling added state cues for one collapsed panel; Safety Summary still had compact text/help text but no neutral/warning state cue.
- `StorageScanSafetySummary.HasReviewWarnings` already defines whether scan safety signals need review.

Implementation:

- Added lightweight neutral/warning styling to `SafetySummaryHeaderText`.
- Waiting-for-scan uses neutral styling.
- A scanned summary with review warnings uses warning styling.
- Kept the header compact; no new row, badge, modal, cleanup execution, restore behavior, persisted history, scan gate change, or real-profile file movement was added.
- Added WPF smoke assertions for startup neutral styling and fixture post-scan warning styling.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated Safety Summary header state styling prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-collapsed-panel-header-help-text.md`
- `docs/features/2026-05-30-safety-summary-header-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm whether the styled Safety Summary header helps distinguish waiting versus needs-review without looking like cleanup approval.

Rejected ideas buffer:

- Do not add a success state to Safety Summary; the summary is not a safety guarantee or cleanup approval.

### 2026-05-28: Create Grill with Docs scaffold

Status: completed

What changed:

- Added the Grill with Docs documentation scaffold.
- Added a SkillOpt-inspired workflow note for evidence-driven, bounded documentation improvement.
- Added this progress log to preserve task evidence and rejected ideas across sessions.

Verification:

- Verified scaffold files and folders with `rg --files` plus a forced recursive listing for hidden `.codex/`.
- `git status --short` could not run because this folder is not currently a Git repository.

Docs updated:

- `AGENTS.md`
- `README-codex-grill-with-docs.md`
- `MANIFEST.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/domain/context-map.md`
- `docs/decisions/`
- `docs/features/`
- `docs/codex/`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0001-use-grill-with-docs-workflow.md`.

Open questions:

- What is this product/app?
- Who is the target user?
- What problem does it solve?
- What stack or framework will it use?
- What is the first feature or workflow?

Rejected ideas buffer:

- Do not write production code before the project summary and initial domain language are captured.

## Verification history

| Date | Check | Result | Notes |
|---|---|---|---|
| 2026-05-28 | Scaffold creation | passed | Verified visible scaffold files with `rg --files`; verified hidden `.codex/progress.md` with `Get-ChildItem -Force -Recurse`. |
| 2026-05-28 | Git status | not available | Folder is not currently a Git repository. |

### 2026-05-28: Capture initial project summary

Status: completed

Evidence:

- Product/app: local Windows cleanup app to trim unwanted bloat from `C:\Users`.
- Target user: project owner only.
- Problem: `C:` is a 250 GB Windows 11 operating-system partition, and `C:\Users` is using about 75 GB.
- Stack/framework: unknown.
- First feature/workflow: unknown.

Docs updated:

- `AGENTS.md`: added project-specific safety rules for read-only scanning, explicit confirmation, reversible cleanup, and conservative path handling.
- `docs/domain/context.md`: replaced template product summary and added initial domain concepts, business rules, lifecycle states, permissions assumptions, and deletion policy.
- `docs/domain/glossary.md`: replaced example terms with initial cleanup domain terms and forbidden synonyms.

ADRs:

- No new ADR added. The current decision is domain framing, not a durable architecture or persistence choice.

Open questions:

- What should count as unwanted bloat?
- Which paths are in scope and out of scope?
- What cleanup action should be available first?
- What stack should the app use?
- What is the first feature brief?

Rejected ideas buffer:

- Do not equate "large" with removable.
- Do not start with permanent deletion as the default first cleanup action.
- Do not treat all of `AppData` as safe bloat.

## Known risks

- Project commands are placeholders until the stack is known.
- First workflow is still unknown.
- Cleanup behavior has destructive potential and needs explicit safety decisions before implementation.

### 2026-05-28: Capture first Grill with Docs answers

Status: completed

Evidence:

- First workflow: read-only scan/report.
- Protected and sensitive folders should still be shown for inspection, with the app helping rate importance and whether deletion is advisable.
- Initial bloat categories: old downloads, temp folders, installer caches, app caches, duplicate files, old game files, Node/Python package caches, and Windows app leftovers.
- Safety constraint: cleanup should not break current apps, including Codex.
- Preferred eventual cleanup path: Quarantine on `D:` with an easy undo workflow.
- Preferred product shape: desktop app.

Docs updated:

- `AGENTS.md`: added first workflow, desktop preference, quarantine preference, and current-app safety rule.
- `docs/domain/context.md`: added Bloat Category, Importance Rating, Deletion Recommendation, Quarantine, Undo Quarantine, and rules for inspection and preserving current apps.
- `docs/domain/glossary.md`: added the new terms and clarified forbidden generic labels.
- `docs/features/2026-05-28-read-only-user-profile-scan.md`: created first draft feature brief.

ADRs:

- No new ADR yet. Stack and Quarantine architecture decisions are still open.

Open questions:

- What desktop stack should be used?
- Should the first scan target all of `C:\Users` or only the current user's profile folder?
- How deep should the first scan inspect folders by default?
- What Importance Rating labels should the UI use?
- What should the first workflow be called?

Rejected ideas buffer:

- Do not hide Protected Locations entirely; show them with conservative warnings.
- Do not build cleanup execution before the read-only scan/review workflow.
- Do not use permanent deletion as the first cleanup mechanism.

### 2026-05-28: Capture Storage Scan implementation choices

Status: completed

Evidence:

- Initial Cleanup Scope: `C:\Users\moxhe`.
- Scan mode: recursive scan of everything accessible within the Cleanup Scope.
- Importance Rating labels: `Likely safe`, `Caution`, `High risk`.
- First workflow name: Storage Scan.
- Desktop stack choice delegated to Codex.
- Local environment has .NET SDK 8.0.421 and 9.0.314, plus Windows Desktop runtimes for .NET 8 and .NET 9.
- Official .NET support policy shows .NET 10 is active LTS through November 14, 2028; .NET 8 and .NET 9 are supported through November 10, 2026.
- Microsoft Learn documents WPF as a .NET Windows desktop UI framework.

Decision:

- Use C# WPF for the desktop app.
- Initial target framework recommendation: .NET 8 for immediate local buildability.
- Revisit .NET 10 after installing its SDK.

Docs updated:

- `AGENTS.md`: replaced placeholder project commands with .NET/WPF commands.
- `docs/domain/context.md`: updated initial Cleanup Scope, recursive Storage Scan workflow, WPF product shape, and Importance Rating labels.
- `docs/domain/glossary.md`: added Storage Scan and WPF terms; updated Cleanup Scope and Importance Rating.
- `docs/features/2026-05-28-read-only-user-profile-scan.md`: updated validation gate and implementation plan.
- `docs/decisions/0002-use-dotnet-wpf-desktop-stack.md`: added stack ADR.

ADRs:

- Added `docs/decisions/0002-use-dotnet-wpf-desktop-stack.md`.

Rejected ideas buffer:

- Do not choose Electron/Tauri unless WPF proves inadequate.
- Do not implement a command-line-only product as the primary UX because the user prefers a desktop app.
- Do not scan all of `C:\Users` in the first version; start with `C:\Users\moxhe`.
- Do not install .NET 10 before the first implementation; use installed .NET 8 for now.

### 2026-05-28: Implement Storage Scan MVP packet

Status: completed

Evidence:

- Created `WindowsFileCleaner.sln`.
- Added `src/WindowsFileCleaner.Core` for read-only scanning and recommendation rules.
- Added `src/WindowsFileCleaner.App` as a WPF desktop app.
- Added `tests/WindowsFileCleaner.Tests` as a dependency-free console test harness.
- Initialized local Git repository and added origin `https://github.com/Smellybum1/Windows-File-Cleaner.git`.
- Remote currently has no refs from `git ls-remote`.

Implementation:

- Storage Scan recursively scans a Cleanup Scope without modifying files.
- Initial UI defaults to `C:\Users\moxhe`.
- Scanner refuses paths outside Cleanup Scope.
- Reparse points are not followed.
- Inaccessible paths are recorded instead of crashing the scan.
- Classifier assigns Bloat Categories, Importance Ratings, Deletion Recommendations, and evidence.
- WPF UI displays totals, top 2,000 largest paths, ratings, recommendations, categories, and evidence details.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- Committed checkpoint: `de7b854 Build initial Storage Scan MVP`.
- Pushed `main` to `https://github.com/Smellybum1/Windows-File-Cleaner.git`.

Docs updated:

- `AGENTS.md`
- `docs/features/2026-05-28-read-only-user-profile-scan.md`
- `.codex/progress.md`

ADRs:

- No new ADR. The implementation follows ADR 0002.

Open questions:

- User needs to run the desktop app and confirm the first real Storage Scan output.
- Quarantine path and undo workflow remain deferred.

Rejected ideas buffer:

- Do not run the real `C:\Users\moxhe` scan automatically from the background.
- Do not add deletion or quarantine buttons before the user reviews real scan output.

### 2026-05-28: Add Storage Scan review filters from real scan feedback

Status: completed

Evidence:

- User ran the WPF app against `C:\Users\moxhe`.
- Real scan completed successfully.
- Reported totals from screenshot:
  - Total size: 58.02 GB.
  - Folders: 37,740.
  - Files: 188,580.
  - Access issues: 3.
- The scan surfaced UX/classification issues:
  - Results table was too broad without filters.
  - Large container folders such as `AppData`, `Local`, `Roaming`, `Google`, `Chrome`, and `pip` showed too many `None` categories.
  - Cache subfolders such as `NVIDIA\DXCache` and Python cache paths need clearer but still conservative labeling.

Implementation:

- Added `StorageReviewFilter`, `StorageReviewEntry`, `StorageReviewSummary`, `StorageScanReview`, and `StorageScanReviewBuilder`.
- Added WPF filter buttons for All, Likely safe, Caution, High risk, and Quarantine candidates.
- Added filter counts and displayed-size summary.
- Lightened DataGrid row presentation.
- Added conservative categories for Profile container, AppData area, Browser data, and GPU shader cache.
- Improved Python `pip` cache recognition.
- Preserved read-only behavior; no cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-review-filters.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental review/filter improvement.

Open questions:

- User should rerun the app and confirm whether filters help.
- Need user feedback on which real rows should be explicit Protected Locations.

Rejected ideas buffer:

- Do not make AppData-derived cache rows "Likely safe" just because they are caches.
- Do not add cleanup buttons based only on the first successful scan.

### 2026-05-28: Add selected-folder child breakdown

Status: completed

Evidence:

- Real scan screenshot showed large container rows such as `moxhe`, `AppData`, `Local`, `pip`, and browser folders.
- The user originally asked for the app to show what is inside folders before cleanup decisions.

Implementation:

- Added `StorageChildSummaryEntry`.
- Added `StorageChildSummaryBuilder`.
- Updated the WPF detail pane with Evidence and Largest immediate children sections.
- Child breakdown shows immediate children with name, size, importance, recommendation, and categories.
- Files explicitly show they have no immediate children.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-folder-child-breakdown.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible review UI improvement.

Open questions:

- User should rerun the app and confirm whether the child breakdown makes large folders understandable.

Rejected ideas buffer:

- Do not replace the main flat table with a tree view until the detail-pane approach is tested.

### 2026-05-28: Add selected-path inspection actions

Status: completed

Evidence:

- Storage Scan now shows enough data that the user needs to inspect selected paths manually.
- Copying and opening selected paths supports review without cleanup execution.

Implementation:

- Added `PathInspectionPlan`.
- Added `PathInspectionPlanBuilder`.
- Added Copy path and Open in Explorer buttons to the selected-row detail pane.
- Folder paths open directly in Explorer.
- File paths ask Explorer to select the file.
- Status messages state that no files were modified.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-inspection-actions.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a read-only inspection UI improvement.

Open questions:

- User should verify Copy path and Open in Explorer on real scan results.

Rejected ideas buffer:

- Do not add destructive selected-row actions next to read-only inspection actions yet.

### 2026-05-28: Add Scan Report CSV export

Status: completed

Evidence:

- Storage Scan now produces enough review data that exporting filtered results will help manual analysis.
- Exporting a report keeps the app read-only with respect to scanned files.

Implementation:

- Added `StorageScanCsvExporter`.
- Added Export CSV button to the Storage Scan toolbar.
- Export uses the current Storage Review Filter.
- Export includes path, name, type, size, importance, recommendation, categories, modified time, evidence, and access issue.
- Export writes a user-selected CSV report file.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only report feature.

Open questions:

- User should verify CSV export after a real scan.

Rejected ideas buffer:

- Do not export file contents.
- Do not treat CSV export as scan history or restore metadata.

### 2026-05-28: Add Review Mix summary and fix flattened-size semantics

Status: completed

Evidence:

- Filtered recursive scan rows overlap because parent folders include child sizes.
- Adding flattened row sizes would overstate storage and imply false savings.

Implementation:

- Added Review Mix display to WPF.
- Changed `StorageReviewSummary` byte fields from summed totals to largest-row sizes.
- Updated filter summary wording to show largest displayed row.
- Added test coverage for largest quarantine candidate row.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental reporting semantics fix.

Open questions:

- User should confirm whether largest-row triage is useful in the real scan.

Rejected ideas buffer:

- Do not report summed bytes for flattened recursive rows.
- Do not call filtered row totals Storage Savings until selections are non-overlapping.

### 2026-05-28: Add Access issues review filter

Status: completed

Evidence:

- The first real scan reported 3 access issues.
- The app showed the count but did not provide a direct way to filter to those paths.

Implementation:

- Added `AccessIssues` to `StorageReviewFilter`.
- Added access issue count and largest-row summary fields to `StorageReviewSummary`.
- Added Access issues filter behavior in `StorageScanReview`.
- Added Access issues button and Review Mix count in WPF.
- Added fixture-style coverage with a synthetic inaccessible row.
- No elevated scan, permission change, cleanup execution, or retry workflow was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-access-issues-review-filter.md`
- `docs/features/2026-05-28-storage-scan-review-filters.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review filter.

Open questions:

- Should access issues remain informational only, or should a future separate workflow retry scanning as administrator?

Rejected ideas buffer:

- Do not request elevation automatically.
- Do not change permissions to resolve access issues.

### 2026-05-28: Add Bloat Category Filter

Status: completed

Evidence:

- The real scan surfaced many category-relevant rows such as app caches, Python package caches, GPU shader caches, browser data, protected locations, and access issues.
- Risk filters alone do not let the user inspect one category of evidence at a time.

Implementation:

- Added `StorageCategorySummaryEntry`.
- Added category summaries to `StorageScanReview`.
- Added combined filtering for `StorageReviewFilter` plus optional `BloatCategory`.
- Added a WPF Category dropdown below the filter buttons.
- CSV export now uses the current review filter and selected category filter.
- Added fixture coverage for category summaries and combined filtering.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-bloat-category-filter.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review feature.

Open questions:

- Should uncategorized rows get a `None` category filter option?
- Which category labels need stronger Protected Location behavior before any cleanup preview?

Rejected ideas buffer:

- Do not treat category matches as cleanup approval.
- Do not sum category rows as Storage Savings while recursive parent/child rows overlap.

### 2026-05-28: Add No category filter

Status: completed

Evidence:

- The first real scan showed many rows with `None` in the Categories column.
- The Bloat Category Filter packet left uncategorized rows as an open question.

Implementation:

- Added `StorageCategoryFilter` and `StorageCategoryFilterKind`.
- Added core filtering for No category rows.
- Added WPF No category dropdown option when uncategorized rows exist.
- Added fixture coverage for No category filtering and combined review/category filtering.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-no-category-filter.md`
- `docs/features/2026-05-28-bloat-category-filter.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review feature.

Open questions:

- Which uncategorized real-scan folders should become explicit Bloat Categories or Protected Locations?

Rejected ideas buffer:

- Do not turn uncategorized rows into `Unknown` categories just to make them filterable.
- Do not treat No category rows as safe or unsafe by default.

### 2026-05-28: Add Review Shortlist

Status: completed

Evidence:

- The user ran the WPF app and confirmed that Storage Scan completed against `C:\Users\moxhe`.
- Real scan output is large enough that the user needs a smaller follow-up set before any cleanup preview.

Implementation:

- Added `StorageReviewShortlist` as an in-memory, per-scan selection model.
- Added Add to shortlist, Remove, Clear shortlist, and Export shortlist controls to the WPF UI.
- Added a Shortlist column to visible Storage Scan rows.
- Review Shortlist export uses the existing CSV exporter and writes only a report.
- Starting a new Storage Scan clears the Review Shortlist.
- No cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-shortlist.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review feature.

Open questions:

- Should Review Shortlist remain per-scan only, or should a later persistence model be added after cleanup approval and manifest rules are designed?

Rejected ideas buffer:

- Do not treat Review Shortlist as Quarantine approval.
- Do not persist shortlisted paths until restore-manifest and cleanup-preview semantics are defined.

### 2026-05-28: Add Quarantine Preview

Status: completed

Evidence:

- The user requested a quarantine folder preferably on `D:` with easy undo.
- Review Shortlist now provides a smaller user-selected set to preview before any cleanup action.
- Safety docs require dry-run or preview behavior before file-moving code.

Implementation:

- Added `QuarantinePreview`, `QuarantinePreviewEntry`, `QuarantinePreviewDisposition`, and `QuarantinePreviewBuilder`.
- Added default preview root `D:\WindowsFileCleanerQuarantine`.
- Added WPF Preview quarantine control and preview summary display.
- Preview output shows included, blocked, and redundant rows, non-overlapping previewed bytes, and destination paths for included rows.
- Preview blocks high-risk, protected, inaccessible, reparse-point, outside-scope, and non-quarantine-candidate rows.
- Preview marks child rows redundant when a selected parent is already included.
- No folder creation, file move, deletion, manifest write, or cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only preview feature. Actual Quarantine execution and Restore Manifest design may need ADR coverage later.

Open questions:

- Should actual Quarantine execution use `D:\WindowsFileCleanerQuarantine` by default, or ask the user to choose a path?
- Should a later preview export a restore-manifest-shaped draft?

Rejected ideas buffer:

- Do not add an Execute, Move, Delete, or Quarantine button in this packet.
- Do not write a Restore Manifest during preview.
- Do not count overlapping parent/child rows as separate Storage Savings.

### 2026-05-28: Add Quarantine Preview CSV export

Status: completed

Evidence:

- Quarantine Preview now produces more detail than the bounded UI preview pane can comfortably show.
- The product still needs review/report workflows before any cleanup execution.

Implementation:

- Added `QuarantinePreviewCsvExporter`.
- Added WPF Export preview control enabled only after a current Quarantine Preview exists.
- Exported cleanup scope, quarantine root, disposition, source path, destination path, size, importance, recommendation, categories, reasons, evidence, access issue, and no-files-modified note.
- Changing the Review Shortlist clears the current preview and disables preview export.
- No folder creation, file move, deletion, manifest write, or cleanup execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only report feature. Restore Manifest and actual Quarantine execution remain future design work.

Open questions:

- Should a later preview export support JSON for machine-readable restore-manifest drafting?
- Should preview exports include a summary row or remain row-only CSV?

Rejected ideas buffer:

- Do not call the preview export a manifest.
- Do not export an executable cleanup script.
- Do not auto-save the preview without a user-selected report path.

### 2026-05-28: Add Storage Scan Safety Summary

Status: completed

Evidence:

- The first real scan surfaced access issues, high-risk/protected rows, and many rows requiring review.
- Future cleanup work needs the read-only safety boundary to stay visible.

Implementation:

- Added `StorageScanSafetySummary`.
- Added `StorageScanSafetySummaryBuilder`.
- Added WPF Safety Summary text under Review Mix.
- Summary displays Cleanup Scope/read-only notes, high-risk count, Protected Location count, access issue count, reparse point count, Quarantine candidate count, and Uncategorized Result count.
- Starting a new scan clears the previous safety summary.
- No permission change, cleanup execution, quarantine execution, manifest write, or rescan behavior was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only reporting feature.

Open questions:

- Should the summary be exportable as part of a future scan report bundle?
- Should summary notes become clickable filters later?

Rejected ideas buffer:

- Do not use the summary as a Cleanup Action gate yet.
- Do not make the summary a safety score.
- Do not hide access issues or protected rows from review.

### 2026-05-28: Add Safety Summary review shortcuts

Status: completed

Evidence:

- Storage Scan Safety Summary exposes warning counts that should be easy to inspect.
- Existing review and category filters already provide safe read-only lenses.

Implementation:

- Added `StorageScanSafetyShortcut`.
- Added `StorageScanSafetyShortcutFilter`.
- Added `StorageScanSafetyShortcutFilterBuilder`.
- Added WPF shortcut buttons for High risk, Protected, Access issues, Reparse points, Quarantine candidates, and No category.
- Shortcuts apply existing Storage Review Filter and Bloat Category Filter combinations.
- Shortcut buttons are disabled before scans, during scans, and for zero-count buckets.
- No cleanup execution, quarantine execution, manifest write, permission change, or rescan behavior was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-safety-summary-review-shortcuts.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental read-only review UI feature.

Open questions:

- Should shortcut clicks scroll to the first matching row or preserve selection if possible?
- Should shortcuts be visually grouped with warning severity later?

Rejected ideas buffer:

- Do not create cleanup-specific shortcut actions.
- Do not treat shortcuts as approvals.
- Do not add new category synonyms for Uncategorized Results.

### 2026-05-28: Add Restore Manifest Draft

Status: completed

Evidence:

- The user wants quarantine on `D:` with easy undo.
- Quarantine Preview now proves eligible destination paths, but future Undo Quarantine needs a versioned metadata contract before file-moving code exists.

Decision:

- Added ADR 0003: use JSON Restore Manifest with schema version `restore-manifest.v1`.

Implementation:

- Added `RestoreManifestDraft`.
- Added `RestoreManifestEntryDraft`.
- Added `RestoreManifestDraftBuilder`.
- Added `RestoreManifestDraftJsonSerializer`.
- Drafts include only included Quarantine Preview rows.
- Drafts exclude blocked and redundant preview rows.
- Draft JSON clearly identifies `isExecutedManifest` as false.
- No folder creation, file move, deletion, manifest file write, quarantine execution, or undo execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/decisions/0003-use-json-restore-manifest.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-restore-manifest-draft.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0003-use-json-restore-manifest.md`.

Open questions:

- What exact manifest file path should future Quarantine execution use?
- Should future executed manifests include hashes for files, and if so which hash algorithm?
- Should future Undo Quarantine restore timestamps and attributes?

Rejected ideas buffer:

- Do not write a draft file automatically.
- Do not use CSV as the executed restore manifest format.
- Do not include blocked or redundant preview rows as draft entries.

### 2026-05-28: Add Quarantine Confirmation Draft

Status: completed

Evidence:

- Quarantine Preview and Restore Manifest Draft now exist as read-only core artifacts.
- Actual Quarantine execution still needs an explicit confirmation gate before any file-moving code.

Implementation:

- Added `QuarantineConfirmationDraft`.
- Added `QuarantineConfirmationDraftBuilder`.
- Confirmation Draft records included counts and bytes, blocked/redundant counts, Restore Manifest Draft id, required future confirmation text, data blockers, and review notes.
- Builder checks preview and manifest agreement for Cleanup Scope, Quarantine root, schema version, entry count, bytes, destination paths, missing rows, duplicate rows, and stray manifest rows.
- `IsExecutionImplemented` remains false.
- No folder creation, file move, deletion, manifest file write, quarantine execution, or undo execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a read-only draft gate; actual execution flow should get ADR review when designed.

Open questions:

- What exact UI should ask for the confirmation phrase?
- Should future execution require hashes before moving files?

Rejected ideas buffer:

- Do not call this a cleanup plan.
- Do not make execution availability depend only on a boolean.
- Do not treat the confirmation phrase as sufficient without matching preview and manifest data.

### 2026-05-28: Add Quarantine Readiness UI

Status: completed

Evidence:

- Restore Manifest Draft and Quarantine Confirmation Draft were implemented in core but not visible in the WPF app.
- The progress log identified WPF display as the next safety-review step before any execution flow.

Implementation:

- WPF `Preview quarantine` now builds a Restore Manifest Draft and Quarantine Confirmation Draft in memory after building the Quarantine Preview.
- The detail pane now shows preview counts, Restore Manifest Draft id and entry summary, Quarantine Confirmation Draft id, required future confirmation text, execution status, readiness blocker count, and blocker details.
- Clearing scan, shortlist, or preview state clears both drafts.
- Quarantine Preview CSV export remains unchanged as a report, not a manifest.
- No folder creation, file move, deletion, manifest file write, quarantine execution, or undo execution was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/features/2026-05-28-quarantine-readiness-ui.md`
- `docs/features/2026-05-28-restore-manifest-draft.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is display-only UI for existing read-only draft artifacts.

Open questions:

- Should future execution use a modal confirmation dialog or dedicated review screen?
- Should Restore Manifest Draft JSON be exportable before execution?

Rejected ideas buffer:

- Do not add an execute button beside preview controls.
- Do not auto-save Restore Manifest Draft JSON from the UI.
- Do not hide readiness blockers behind a single pass/fail label.

### 2026-05-28: Add conservative app data classification

Status: completed

Evidence:

- Real scan evidence included large app/game rows that needed clearer but conservative labels.
- The app must avoid breaking current apps, game saves, app settings, and installed tools.

Implementation:

- Added `WindowsAppData`, `InstalledApplication`, and `GameData` Bloat Category values.
- Added classifier hints for `AppData\Local\Packages`, `AppData\Local\Programs`, Larian/Baldur's Gate, Stellaris, Paradox, and IronyMod-style paths.
- These categories are treated as Protected Location / High risk / Keep by default.
- Added display labels in WPF and CSV exporters.
- Added fixture coverage for Windows app package data, per-user installed app folders, and known game data.
- No cleanup execution, manifest writing, file move, deletion, or Quarantine behavior was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-conservative-app-data-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an incremental conservative classifier refinement.

Open questions:

- Which specific app or game folders should get cleanup exceptions later?
- Should some Windows app package subfolders be downgraded after manual review?

Rejected ideas buffer:

- Do not mark Windows app package data as likely safe by default.
- Do not classify game folders as removable just because they look old.
- Do not make `AppData\Local\Programs` a cleanup candidate.

### 2026-05-28: Add read-only safety regression

Status: completed

Evidence:

- The MVP boundary is read-only Storage Scan plus user-selected CSV reports.
- Future cleanup execution has not been approved or designed.

Implementation:

- Added `ProductionCodeDoesNotContainCleanupExecutionCalls` to the fixture test harness.
- The guard scans production C# files under `src/`.
- The guard fails on obvious file/directory move, delete, replace, write-bytes, set-attributes, and production directory-creation APIs.
- The guard allows exactly three `File.WriteAllText(dialog.FileName, ...)` calls for user-selected CSV report exports.
- Tests still create/delete fixture directories only inside the test harness.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `docs/features/2026-05-28-read-only-safety-regression.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a verification guard for the existing read-only boundary.

Open questions:

- Should future approved Quarantine execution add a more precise allowlist for its own file-moving implementation?

Rejected ideas buffer:

- Do not block user-selected CSV report writes.
- Do not scan docs or tests for banned APIs; fixtures are allowed to create/delete test files.
- Do not treat this guard as a substitute for execution design if cleanup actions are added later.

### 2026-05-28: Add MVP runbook

Status: completed

Evidence:

- The repo had detailed Grill with Docs files but no app-focused root `README.md`.
- Next work requires a safe manual WPF rerun against `C:\Users\moxhe`.

Implementation:

- Added `README.md` for the current Windows File Cleaner MVP.
- Documented safety status, requirements, verification commands, WPF run command, default Cleanup Scope, manual MVP checklist, current workflow, and not-yet-implemented cleanup workflows.
- Kept the existing `README-codex-grill-with-docs.md` scaffold README.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-runbook.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a documentation runbook for the current MVP.

Open questions:

- Should packaging instructions be added after a release build exists?

Rejected ideas buffer:

- Do not document cleanup execution commands before they exist.
- Do not replace the Grill with Docs scaffold docs.
- Do not claim the app is safe to delete files.

### 2026-05-28: Add MVP readiness audit

Status: completed

Evidence:

- The user provided a screenshot showing Storage Scan completed against `C:\Users\moxhe` with 58.02 GB, 37,740 folders, 188,580 files, 3 access issues, and no file modifications.
- The repo already contained WPF, .NET 8, read-only Storage Scan, fixture tests, conservative classification, preview-only Quarantine artifacts, and safety regression evidence.

Implementation:

- Added `docs/features/2026-05-28-mvp-readiness-audit.md`.
- Mapped the original MVP requirements to repo evidence and status.
- Marked latest WPF UI retest as pending manual verification.
- Marked actual Quarantine execution and Undo Quarantine execution as out of MVP.
- Linked the audit from `README.md`.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is an evidence audit, not a new durable architecture or persistence decision.

Open questions:

- Should the next packet focus on manual WPF retest feedback, or start designing actual Quarantine execution only after that retest?

Rejected ideas buffer:

- Do not mark the MVP fully complete until the latest WPF review flow is manually retested.
- Do not treat Quarantine Preview, Restore Manifest Draft, or Quarantine Confirmation Draft as cleanup approval.

### 2026-05-28: Add WPF fixture smoke launch

Status: completed

Evidence:

- The MVP readiness audit identified the latest WPF UI manual retest as the main remaining verification gap.
- Core fixture tests existed, but the WPF app always opened with the default real Cleanup Scope.

Implementation:

- Added `StorageScanLaunchOptions` for parsing `--scope`.
- Updated WPF startup to construct `MainWindow` explicitly with the parsed initial Cleanup Scope.
- Added a `MainWindow` constructor overload that fills the Cleanup Scope box without starting a scan.
- Added parser coverage to the console test harness.
- Added `tools/New-StorageScanSmokeFixture.ps1` to create a synthetic fixture under `.local\storage-scan-smoke-fixture`.
- Added `.local/` to `.gitignore`.
- Updated README, AGENTS, the domain context rule, and the MVP readiness audit.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1` passed and created the ignored fixture Cleanup Scope.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed and showed intended fixture writes.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/domain/context.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-fixture-smoke-launch.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible launch/testability improvement and does not change architecture, persistence, cleanup behavior, or security posture.

Open questions:

- Should the fixture include more real-scan-inspired app/game examples after the next manual pass?

Rejected ideas buffer:

- Do not auto-scan on startup.
- Do not create fixture files from production app code.
- Do not treat fixture WPF smoke testing as a substitute for one final real-profile retest.

### 2026-05-28: Add WPF shell smoke test

Status: completed

Evidence:

- Core fixture tests already covered scan/review/preview logic.
- WPF fixture launch support already covered argument parsing.
- The remaining automated gap was proving the WPF shell consumes the launch Cleanup Scope without starting a scan.

Implementation:

- Added read-only `MainWindow` startup-state properties.
- Added `tests/WindowsFileCleaner.App.Tests` targeting `net8.0-windows` with `UseWPF`.
- Added smoke coverage for default Cleanup Scope, launch Cleanup Scope, idle startup state, enabled scan action, and disabled CSV export before scan.
- Added the app test project to `WindowsFileCleaner.sln`.
- Updated README, AGENTS, the MVP readiness audit, and this progress log.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed after restore.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-shell-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a testability improvement and does not change architecture, persistence, security posture, or cleanup behavior.

Open questions:

- Should future UI automation cover actual button/filter interactions after the manual retest?

Rejected ideas buffer:

- Do not treat WPF shell construction as proof of the full interaction flow.
- Do not add visible GUI automation dependencies before the manual fixture smoke pass identifies a real need.

### 2026-05-28: Add WPF fixture scan smoke test

Status: completed

Evidence:

- Core tests already covered scanner and review logic.
- WPF shell smoke tests covered construction and launch-scope wiring.
- The next evidence gap was proving a fixture scan updates WPF state without touching real profile data.

Implementation:

- Refactored the Scan button path to call `RunStorageScanForCurrentScopeAsync`.
- Added read-only WPF state properties for smoke-test assertions.
- Added `WindowsFileCleaner.App.Tests` coverage that creates a synthetic fixture, runs Storage Scan through `MainWindow`, and asserts visible status, summary, filter, row, rating, recommendation, and category state.
- Verified fixture marker files remain after the scan.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-fixture-scan-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a testability improvement and does not change architecture, persistence, security posture, or cleanup behavior.

Open questions:

- Should future WPF automation cover Review Shortlist and Quarantine Preview interactions?

Rejected ideas buffer:

- Do not treat WPF fixture scan state as proof of visible layout quality.
- Do not automate real-profile scans in the test harness.

### 2026-05-28: Add WPF review interaction smoke test

Status: completed

Evidence:

- README manual MVP checklist asks for filters, Review Shortlist, and Quarantine Preview to be checked before relying on the app against real profile data.
- Core tests already covered review and preview logic.
- WPF fixture scan tests covered scan state but not review-control interactions.

Implementation:

- Added WPF command methods for Storage Review Filters, Bloat Category Filters, Safety Summary review shortcuts, displayed-row selection, Review Shortlist changes, and Quarantine Preview generation.
- Added `WindowsFileCleaner.App.Tests` coverage that runs a synthetic fixture scan through `MainWindow`, applies review filters and safety shortcuts, shortlists a likely-safe candidate, creates a Quarantine Preview, checks Restore Manifest Draft and Quarantine Confirmation Draft text, and verifies fixture files remain.
- Kept export dialogs manual and no cleanup execution was added.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-review-interaction-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is testability and smoke coverage for existing read-only UI behavior, not a durable architecture, persistence, security, deployment, or cleanup-execution decision.

Open questions:

- Should visible GUI automation or screenshot verification be added after the manual fixture pass?

Rejected ideas buffer:

- Do not automate file export dialogs in this packet.
- Do not treat WPF review interaction state as proof of visible layout quality.
- Do not automate real-profile scans in the test harness.

### 2026-05-28: Polish WPF review toolbar layout

Status: completed

Evidence:

- README and MVP audit still identify the visible fixture UI pass as the next manual verification step.
- The review toolbar previously used fixed grid columns and a horizontal action stack that could crowd as labels and counts grow.
- WPF app smoke tests can verify the intended wrapping toolbar structure without launching a visible desktop window.

Implementation:

- Replaced the fixed review toolbar grid with two named `WrapPanel` toolbars.
- Kept Filter Summary as its own wrapping line between filters and action controls.
- Added a small read-only WPF layout property for smoke-test assertions.
- Added `WindowsFileCleaner.App.Tests` coverage that verifies the review controls use wrapping toolbars.
- No scanner, classifier, cleanup, quarantine execution, or manifest-writing behavior was changed.

Verification:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-wpf-review-toolbar-layout-polish.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible UI layout polish and does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- Should the entire app later get a broader visual design pass after the real-profile retest?

Rejected ideas buffer:

- Do not introduce a new UI framework or dependency for this polish.
- Do not treat wrapping layout structure as proof of visual quality.

### 2026-05-28: Add MVP preflight script

Status: completed

Evidence:

- README verification commands existed individually, but the full pre-real-scan sequence was easy to run incompletely.
- The fixture generator already supported `-WhatIf`.
- Progress history repeatedly used the same restore, build, core test, app test, and fixture dry-run sequence.

Implementation:

- Added `tools/Invoke-MvpPreflight.ps1`.
- Preflight runs restore, build, core tests, WPF app tests, the fixture generator in `-WhatIf` mode, and `git diff --check`.
- Preflight prints the next fixture review launcher command.
- Added `-SkipRestore`, `-SkipFixtureWhatIf`, and `-SkipDiffCheck` switches for focused local loops.
- No real profile scan, WPF launch, fixture creation, cleanup execution, quarantine execution, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a local verification wrapper around existing commands and does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- Should the app later expose a built-in diagnostics/preflight screen?

Rejected ideas buffer:

- Do not make preflight launch the WPF app or scan `C:\Users\moxhe`.
- Do not create fixture files by default from preflight.
- Do not hide individual commands from README; keep them visible for troubleshooting.

### 2026-05-28: Add MVP fixture review launcher

Status: completed

Evidence:

- Progress log identified the manual fixture UI pass as the next recommended work.
- Preflight and fixture scripts existed, but the user still had to run multiple commands and copy the fixture launch command manually.
- WPF launch-scope support only pre-fills the Cleanup Scope and does not auto-scan.

Implementation:

- Added `tools/Start-MvpFixtureReview.ps1`.
- The launcher runs MVP preflight by default, creates the synthetic fixture Cleanup Scope inside the repo, and launches the WPF app with that fixture scope.
- The launcher prints that the app will not auto-scan and that the user must click `Scan`.
- Added `-SkipPreflight`, `-SkipLaunch`, and `-WhatIf` support for safe verification and focused local loops.
- No real profile scan, auto-scan, cleanup execution, quarantine execution, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf` passed and showed intended preflight, fixture creation, and WPF launch actions without executing them.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `AGENTS.md`
- `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a local workflow launcher around existing preflight, fixture, and WPF launch paths; it does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- Should the app later include an in-app fixture/demo mode for visual QA?

Rejected ideas buffer:

- Do not make the launcher scan automatically.
- Do not point the launcher at `C:\Users\moxhe`.
- Do not make fixture creation happen from production app code.

### 2026-05-28: Add Selected Path Review Guidance

Status: completed

Evidence:

- The user-tested real scan showed very large rows where `Caution` plus passive evidence was not enough to guide review.
- Existing selected-row detail already showed evidence and Child Breakdown, so the smallest improvement was next-step wording for the selected path.

Implementation:

- Added `SelectedPathReviewGuidance` and `SelectedPathReviewGuidanceBuilder`.
- Added a Review guidance section to the WPF selected-row detail pane.
- Guidance covers access issues, reparse points, profile containers, protected/high-risk rows, quarantine candidates, cache/package rows, Uncategorized Results, and generic evidence review.
- Added core guidance coverage and WPF smoke coverage that selected Quarantine candidates show guidance before shortlisting.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-review-guidance.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible selected-row review improvement and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should guidance become user-customizable after actual cleanup execution exists?

Rejected ideas buffer:

- Do not turn selected-row guidance into cleanup approval language.
- Do not hide High risk rows; explain the safest next review step.
- Do not use Selected Path Review Guidance as a cleanup executor.

### 2026-05-28: Add Cleanup Scope Safety Note

Status: completed

Evidence:

- The project requires fixture-based verification before real-profile scans.
- The app previously showed only the Cleanup Scope path field, so fixture-vs-real scope context was documented but not visible in the WPF shell.

Implementation:

- Added `CleanupScopeSafetyNote` and `CleanupScopeSafetyNoteBuilder`.
- Added a WPF note below the Cleanup Scope controls.
- The note distinguishes Fixture Cleanup Scope, Real Profile Cleanup Scope, Custom Cleanup Scope, Choose Cleanup Scope, and Check Cleanup Scope.
- Added core tests for real-profile, fixture, custom, and blank scope notes.
- Added WPF smoke assertions for default real-profile startup and fixture launch startup notes.
- No scan blocking, preflight execution, fixture creation, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-safety-note.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only UI reminder and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future release record the last successful preflight timestamp for local-only display?

Rejected ideas buffer:

- Do not treat the note as proof that preflight ran.
- Do not make the note run shell commands, create fixtures, or block scanning.
- Do not add a modal pre-scan gate before the visible fixture workflow is manually tested.

### 2026-05-28: Add Storage Review Search

Status: completed

Evidence:

- The real scan scale was 188,580 files and 37,740 folders.
- Existing filters helped broad triage, but finding a specific app, tool, cache, or game path still required scrolling or CSV export.

Implementation:

- Added `StorageReviewSearch`.
- Extended `StorageScanReview` filtering to combine Storage Review Filter, Bloat Category Filter, and Storage Review Search.
- Added a WPF Search field and Clear search action.
- Search matches path, name, category, Importance Rating, Deletion Recommendation, evidence, and access issue text.
- Search normalizes whitespace, hyphens, and underscores so spaced search terms can match enum-style labels such as `HighRisk` and `PythonPackageCache`.
- Search resets after each new Storage Scan.
- No filesystem rescan, persistence, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible in-memory review feature and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should search later support explicit field prefixes such as `path:`, `category:`, or `rating:`?

Rejected ideas buffer:

- Do not make search rescan, watch the filesystem, or search outside completed scan results.
- Do not persist search history.
- Do not use search results as cleanup approval.

### 2026-05-28: Add Storage Review Display Limit wording

Status: completed

Evidence:

- The user reran the WPF app against `C:\Users\moxhe` and provided a screenshot showing the scan completed.
- The real scan contained 188,580 files, 37,740 folders, and 3 access issues.
- The WPF grid caps visible rows at 2,000, but prior wording did not clearly distinguish displayed rows from matched review rows.

Implementation:

- Split active matched review rows from WPF displayed rows.
- Updated completed-scan status to say `Showing 2,000 of ... paths` when the display cap is reached.
- Updated Filter Summary to say `2,000 shown of ... matched`, label largest-row triage as the largest matched row, and suggest narrowing with filters/search.
- Added a WPF smoke test with a large synthetic fixture that exceeds the 2,000-row display limit.
- No scanner traversal, classification, export, cleanup execution, Quarantine execution, Undo Quarantine, or manifest-writing behavior was changed.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-display-limit.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF review wording and smoke coverage; it does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future UI add paging or a virtualized tree/grid for all matched rows?

Rejected ideas buffer:

- Do not lower scan depth or skip files to fit the grid.
- Do not call displayed rows the complete scan result.
- Do not add cleanup execution while improving review wording.

### 2026-05-28: Add Shortlist shown action

Status: completed

Evidence:

- The real scan result set is large enough that adding one row at a time to Review Shortlist is cumbersome.
- Storage Review Display Limit wording now makes the visible grid boundary explicit, so bulk shortlisting can safely target only visible rows.

Implementation:

- Added `StorageReviewShortlist.AddMany` for unique bulk additions.
- Added a WPF `Shortlist shown` action that adds only currently displayed rows to Review Shortlist.
- Status text states that Review Shortlist is not cleanup approval and that no files were modified.
- `Shortlist shown` disables when all currently displayed rows are already shortlisted.
- No hidden matched rows, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-shortlist-shown-review-rows.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a later review UI support selecting a specific subset of displayed rows before bulk shortlisting?

Rejected ideas buffer:

- Do not add all matched rows when the grid is capped.
- Do not treat Review Shortlist as cleanup approval.
- Do not add cleanup execution from the shortlist toolbar.

### 2026-05-28: Add Remove shown shortlist action

Status: completed

Evidence:

- `Shortlist shown` makes visible-window bulk review easier, but correcting a broad visible-window shortlist previously required clearing the whole Review Shortlist or removing one selected row at a time.
- Review Shortlist remains an in-memory review aid, so visible-window removal is reversible and does not touch scanned files.

Implementation:

- Added `StorageReviewShortlist.RemoveMany` for unique bulk removals.
- Added a WPF `Remove shown` action that removes only currently displayed rows from Review Shortlist.
- `Remove shown` disables when no currently displayed rows are shortlisted.
- Updated WPF smoke coverage for add shown, remove shown, selected-row add, preview generation, and read-only status text.
- No hidden matched rows, cleanup execution, Quarantine execution, Undo Quarantine, or manifest writing was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-shortlist-shown-review-rows.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a later review UI support selecting a specific subset of displayed rows before bulk shortlisting/removal?

Rejected ideas buffer:

- Do not add or remove all matched rows when the grid is capped.
- Do not treat Review Shortlist as cleanup approval.
- Do not add cleanup execution from the shortlist toolbar.

### 2026-05-28: Align Scan Report Export with Storage Review Search

Status: completed

Evidence:

- Storage Review Search narrows the WPF grid and filter summary, but the main Scan Report Export path still used only Storage Review Filter and Bloat Category Filter.
- A searched review should export the same active review lens, while still exporting all matched rows rather than only the 2,000 displayed rows.

Implementation:

- Added a WPF export-row helper that applies Storage Review Filter, Bloat Category Filter, and Storage Review Search.
- Updated Export CSV to use that helper.
- Added WPF smoke coverage that searched export rows honor the active `pip` search.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only report alignment fix and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should export filenames include a sanitized search segment later?

Rejected ideas buffer:

- Do not export only displayed rows; export all rows matched by the active review lens.
- Do not treat exported reports as cleanup history or restore manifests.

### 2026-05-28: Add searched Scan Report Export filenames

Status: completed

Evidence:

- Scan Report Export now honors Storage Review Search, but the suggested filename still did not show when a search was active.
- Report filenames should help the user distinguish searched CSV exports without becoming persisted scan history.

Implementation:

- Added a sanitized `search-...` segment to the main Scan Report Export filename when Storage Review Search is active.
- Search filename segments use lowercase letters/digits separated by hyphens and are capped in length.
- Added WPF smoke coverage for searched and cleared-search export filename behavior.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors during final preflight.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only report naming behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should exports include all rows, active filter rows, or both as separate options?

Rejected ideas buffer:

- Do not treat report filenames as scan history.
- Do not include raw user paths in generated filenames.

### 2026-05-28: Add hierarchy context to Scan Report Export

Status: completed

Evidence:

- Recursive Storage Scan rows are flattened in the grid and CSV report.
- Full paths are present, but spreadsheet review is easier when each row also carries its immediate parent and depth.

Implementation:

- Added `Parent path` and `Depth` columns to `StorageScanCsvExporter`.
- Kept root-level parent path blank so the export does not invent a parent outside the reviewed hierarchy.
- Extended CSV fixture coverage for the new hierarchy columns.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only report schema improvement and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future export include a separate cleanup-scope-relative path column?

Rejected ideas buffer:

- Do not make Scan Report Export a persisted scan history feature.
- Do not include cleanup approval or restore-manifest data in this CSV.

### 2026-05-28: Add Storage Review Size Note

Status: completed

Evidence:

- The real scan screenshot showed large parent and child folders together in the flattened review grid.
- Review Mix and filter summaries avoid summing rows internally, but the WPF review surface did not state the recursive size rule near the grid.

Implementation:

- Added a visible Storage Review Size Note below the filter summary.
- The note says folder sizes include children, parent/child rows can overlap, and row sizes are triage clues rather than Storage Savings.
- Added WPF smoke coverage for the note text.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-size-note.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible UI wording and smoke coverage.

Open questions:

- Is the note visually readable in the next manual fixture UI pass?

Rejected ideas buffer:

- Do not hide recursive parent or child rows to avoid overlap confusion.
- Do not label row sizes as cleanup savings before Quarantine Preview or a future explicit cleanup plan computes non-overlapping bytes.

### 2026-05-28: Add Storage Review field-prefix search

Status: completed

Evidence:

- The real scan scale makes broad text search useful but sometimes noisy.
- The Storage Review Search feature brief left `path:`, `category:`, and `rating:` prefixes as a deferred question.

Implementation:

- Added `StorageReviewSearchField` and prefix parsing in `StorageReviewSearch`.
- Supported `path:`, `name:`, `category:`/`cat:`, `rating:`/`importance:`, `recommendation:`/`rec:`, `evidence:`, and `issue:`/`access:`.
- Updated `StorageScanReview` to restrict matching to the parsed field when a recognized prefix is used.
- Preserved unprefixed broad search and treated unrecognized prefixes as literal broad search text.
- Added WPF search tooltip examples and smoke coverage for prefixed search summary/export filename behavior.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible in-memory search behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Are the tooltip examples discoverable enough during the next manual fixture UI pass?

Rejected ideas buffer:

- Do not make prefixed search rescan the filesystem or persist search history.
- Do not use prefixed search matches as cleanup approval.

### 2026-05-28: Add Large old file classification

Status: completed

Evidence:

- The real scan screenshot included large files with little category context.
- `No category` remains useful, but old multi-gigabyte files deserve a conservative triage label when last-modified evidence is stale.

Implementation:

- Added `BloatCategory.LargeOldFile`.
- Passed file size into `CleanupCandidateClassifier`.
- Labeled files at least 1 GB and older than 90 days as `Large old file`.
- Kept unknown large old files as `Caution` / `Inspect`; size and age alone do not approve cleanup.
- Kept large old files with stronger cleanup evidence, such as old Downloads or installer evidence, on the existing likely-safe/quarantine-candidate path.
- Added display/export labels and classifier coverage.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-read-only-user-profile-scan.md`
- `docs/features/2026-05-28-large-old-file-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible classifier triage behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does the real scan now surface helpful `Large old file` rows?

Rejected ideas buffer:

- Do not classify directories as Large old file from recursive size because parent and child rows overlap.
- Do not treat large old files as cleanup approval without stronger category evidence.

### 2026-05-28: Add Storage Entry Type Filter

Status: completed

Evidence:

- Recursive scan review mixes folders and files in one flattened grid.
- The user needs to inspect both container folders and individual files, especially after adding Large old file classification.

Implementation:

- Added `StorageEntryTypeFilter` with `All`, `Files`, and `Folders`.
- Added core filtering that combines entry type with review filter, Bloat Category Filter, and Storage Review Search.
- Added a WPF Type filter combo box.
- Included active type in Filter Summary and Scan Report Export filenames.
- Added core and WPF smoke coverage.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `docs/features/2026-05-28-storage-entry-type-filter.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review filtering and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does Files plus Large old file help the next real scan review?

Rejected ideas buffer:

- Do not treat file-only rows as automatically safer than folders.
- Do not hide the underlying completed scan rows; type is only an active review lens.

### 2026-05-28: Add Review View Reset

Status: completed

Evidence:

- Review filters now include rating, category, type, search, and Safety Summary shortcuts.
- Stacked review lenses can make it cumbersome to return to full review without losing Review Shortlist state.

Implementation:

- Added `Reset view` to the WPF review toolbar.
- Added `ResetReviewView` to restore All, All categories, All types, and empty search.
- Kept Review Shortlist intact during reset.
- Added reset-enabled state handling and WPF smoke coverage.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-view-reset.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Is Reset view discoverable enough during manual fixture review?

Rejected ideas buffer:

- Do not combine Reset view with Clear shortlist.
- Do not treat reset as a rescan or cleanup action.

### 2026-05-28: Add Selected Path Hierarchy Context

Status: completed

Evidence:

- Real scan output included deeply nested cache rows with short names such as one-letter folders and hash fragments.
- Those rows are hard to interpret from `Name` alone, even though the selected-row full path exists in the detail pane.

Implementation:

- Added `StorageEntryRow.ParentLocation`.
- Added a `Parent` column to the WPF Storage Scan grid.
- Added selected-row parent path, hierarchy depth, and modified-time context to the detail pane.
- Added WPF smoke coverage for parent/depth context.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-hierarchy-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does the `Parent` column make the next real scan easier to review without overcrowding the table?

Rejected ideas buffer:

- Do not classify short or deeply nested names as safe solely from their hierarchy context.

### 2026-05-28: Add Selected File Content Preview

Status: completed

Evidence:

- The original product request asked the app to show what is in files before rating importance or recommending cleanup.
- The current selected-row detail pane showed metadata, evidence, guidance, and child breakdowns, but could not preview selected file content.

Implementation:

- Added `SelectedFileContentPreview` and `SelectedFileContentPreviewBuilder`.
- Added an explicit WPF `Preview file` action for selected files.
- Added a `File preview` section to the selected-row detail pane.
- Kept preview bounded to a small text sample and avoided rendering binary-looking content as text.
- Added core coverage for text, binary, and folder preview outcomes.
- Added WPF smoke coverage for previewing a selected fixture text file.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-file-content-preview.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only review action and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does selected file preview help with real scan rows without exposing too much sensitive text in the review pane?

Rejected ideas buffer:

- Do not automatically preview file content when row selection changes.
- Do not treat previewed text as cleanup approval.

### 2026-05-28: Tune Cache-Specific Review Guidance

Status: completed

Evidence:

- The real scan showed large cache-heavy rows such as `NVIDIA\DXCache` and Python package cache paths.
- The classifier intentionally keeps these rows conservative, but generic guidance was less useful than category-specific review wording.

Implementation:

- Added GPU shader cache guidance that mentions rebuildability and temporary shader recompile delays.
- Added Python package cache guidance that protects active development tooling and Codex-related paths.
- Added Node package cache guidance that warns about active project dependencies.
- Added app cache guidance that prefers specific child rows over broad app folders.
- Added generic AppData guidance for rows with AppData evidence but no narrower cache category.
- No Bloat Category, Importance Rating, Deletion Recommendation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/features/2026-05-28-cache-specific-review-guidance.md`
- `docs/features/2026-05-28-selected-path-review-guidance.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible wording and triage guidance, not architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does cache-specific guidance make the next real scan easier to act on without making cache rows feel automatically safe?

Rejected ideas buffer:

- Do not promote GPU shader caches or package caches to likely-safe cleanup candidates solely because they are cache-like.

### 2026-05-28: Add Real Profile Scan Gate

Status: completed

Evidence:

- The project requires fixture-based verification before scanning real user files.
- The previous Cleanup Scope Safety Note reminded the user to run preflight and fixture review, but docs explicitly noted that the reminder was not proof and did not block scanning.

Implementation:

- Added `CleanupScopeScanGate` and `CleanupScopeScanGateBuilder`.
- Added a WPF acknowledgement checkbox for real-profile Cleanup Scopes.
- Disabled `Scan` for `C:\Users\moxhe` and child scopes until the acknowledgement is checked.
- Kept fixture Cleanup Scopes scan-ready without the real-profile acknowledgement.
- Enforced the gate in `RunStorageScanForCurrentScopeAsync`, not only through button state.
- Reset acknowledgement when the Cleanup Scope changes.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, preflight execution from WPF, fixture creation from WPF, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-safety-note.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-real-profile-scan-gate.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible local scan-start gate and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Should a future app version display the last successful preflight time if a trustworthy local marker exists?

Rejected ideas buffer:

- Do not run preflight or create fixture files from WPF.
- Do not persist real-profile scan acknowledgement yet.

### 2026-05-28: Add Selected Row Contents Context

Status: completed

Evidence:

- Large recursive scan rows need more context than size alone because parent and child row sizes overlap.
- The app already shows largest immediate children, but selected rows did not show contained file/folder counts.

Implementation:

- Added contained file and descendant folder counts to `StorageEntryRow`.
- Added contents context to the WPF selected-row detail pane.
- Added `Contained files` and `Contained folders` columns to Scan Report CSV export.
- Added WPF fixture coverage for selected-folder contents context.
- Added CSV coverage for exported contents counts.
- No scanner traversal, Bloat Category, Importance Rating, Deletion Recommendation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `docs/features/2026-05-28-selected-row-contents-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review/export context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Do contents counts make the next real scan easier to review alongside largest-child breakdowns?

Rejected ideas buffer:

- Do not treat contained file/folder counts as recoverable storage savings.

### 2026-05-28: Add Cleanup Scope Root Classification

Status: completed

Evidence:

- The first real scan screenshot showed the top `C:\Users\moxhe` row as an ordinary caution/inspect row with no category.
- The scan root should be reviewed through child rows and should never look like a cleanup target.
- Path-shape inference is not enough because fixture and custom Cleanup Scope roots also need root-specific treatment.

Implementation:

- Added `BloatCategory.CleanupScopeRoot`.
- Passed explicit scan-root context from `StorageScanner` into `CleanupCandidateClassifier`.
- Classified the Cleanup Scope Root as `High risk` / `Keep` with `Cleanup scope root` and `Protected location` categories.
- Added Selected Path Review Guidance for scope-root rows.
- Added WPF, Scan Report CSV, and Quarantine Preview CSV labels for `Cleanup scope root`.
- Added core scanner coverage and WPF fixture coverage.
- No child classification, scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real-profile automation was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-root-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a conservative scan classification and review-guidance change, not a persistence, cleanup execution, security, deployment, or public API decision.

Open questions:

- Does the next real scan make the first row clearly read as the reviewed scope rather than bloat?

Rejected ideas buffer:

- Do not infer Cleanup Scope Root solely from `C:\Users` path shape.
- Do not allow the scope root into cleanup execution or quarantine.

### 2026-05-28: Add Quarantine Preview Protected Descendant Blocker

Status: completed

Evidence:

- Broad cache-like rows can contain protected descendants, including Codex runtime data.
- A broad parent row may otherwise appear to be a Quarantine candidate even though moving it would also move protected child data.
- Quarantine Preview already has the scanned child tree in memory, so it can block broad parent preview without touching the filesystem again.

Implementation:

- Added descendant blocker checks to `QuarantinePreviewBuilder`.
- Blocked parent preview when descendants are protected, high-risk, inaccessible, reparse points, or Cleanup Scope Roots.
- Added blocked reason text with example descendant paths and guidance to select narrower reviewed child rows.
- Added fixture coverage for a synthetic `.cache` parent containing protected `codex-runtimes` data.
- No cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or additional filesystem reads were added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a conservative read-only preview rule within existing Quarantine Preview behavior.

Open questions:

- In the next real scan, are broad cache-parent blocker reasons readable enough when paths are long?

Rejected ideas buffer:

- Do not rescan the filesystem from Quarantine Preview to discover blockers.
- Do not allow a broad parent to be included just because the parent row itself is a Quarantine candidate.

### 2026-05-28: Add WPF Proof for Quarantine Preview Protected Descendant Blocker

Status: completed

Evidence:

- Core tests proved the protected-descendant blocker, but the WPF workflow also needs to show the blocked reason and readiness blockers clearly.
- The app should prove the same boundary through selection, Review Shortlist, Preview quarantine, and the preview pane.

Implementation:

- Added a WPF smoke fixture with `.cache` containing protected `codex-runtimes` data.
- Added WPF smoke coverage that selects the broad `.cache` parent, adds it to Review Shortlist, runs Quarantine Preview, and asserts:
  - `0 included`
  - `1 blocked`
  - blocked preview readiness wording
  - `codex-runtimes` descendant evidence
  - narrower-row guidance
  - no-files-modified wording
- No production code, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `docs/features/2026-05-28-wpf-review-interaction-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is WPF smoke coverage for an existing read-only preview rule.

Open questions:

- Should the visible preview pane separate row-specific blockers from confirmation-readiness blockers more clearly?

Rejected ideas buffer:

- Do not weaken confirmation readiness just to make the blocked-row count read as a single blocker.

### 2026-05-28: Improve Quarantine Preview Pane Readability

Status: completed

Evidence:

- The WPF preview pane showed confirmation-readiness blockers and row-level blocked reasons close together.
- A blocked broad parent can have both confirmation blockers and row-specific reasons, so the visible wording should keep those concepts separate.

Implementation:

- Renamed the readiness count line to `Confirmation readiness blockers`.
- Labeled readiness entries as `Confirmation blocker`.
- Added a `Preview rows:` section before row-level included/blocked/redundant entries.
- Labeled each row entry as `Preview row | Included`, `Preview row | Blocked`, or `Preview row | Redundant`.
- Updated WPF smoke assertions for both included-row preview and protected-descendant blocked preview.
- No Quarantine Preview eligibility rules, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was changed.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-quarantine-preview-pane-readability.md`
- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `docs/features/2026-05-28-wpf-review-interaction-smoke-test.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF wording and smoke coverage.

Open questions:

- Does the real scan need a more structured preview table instead of plain text for long blocked paths?

Rejected ideas buffer:

- Do not change preview eligibility or confirmation readiness semantics as part of wording polish.

### 2026-05-28: Add Access Status Review Field

Status: completed

Evidence:

- The real scan showed access issues, and incomplete scan coverage should stay visible in the main review and exported reports.
- Access issue rows were countable/filterable, but normal review rows did not expose a simple readable/access-issue label.

Implementation:

- Added `StorageEntryRow.AccessStatus` with user-facing values `Readable` and `Access issue`.
- Added an `Access` column to the WPF Storage Scan grid.
- Added `Access: ...` to selected-row metadata.
- Added `Access status` columns to Scan Report CSV and Quarantine Preview CSV exports.
- Added core CSV coverage for readable and access issue statuses.
- Added WPF fixture coverage for readable row access status and selected-row detail metadata.
- No access retry, permission change, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuilding.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-access-status-review-field.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `docs/features/2026-05-28-quarantine-preview-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI/report context.

Open questions:

- In the next real scan, does the Access column make the three access issue rows easier to find?

Rejected ideas buffer:

- Do not add retry-as-admin or permission-changing behavior as part of access status display.

### 2026-05-28: Add Access Status Search

Status: completed

Evidence:

- The real scan screenshot showed readable rows and 3 access issue rows.
- Access Status is now visible in the grid, selected-row metadata, and CSV exports, so it should also participate in the existing search workflow.

Implementation:

- Added Access Status matching to broad Storage Review Search.
- Added Access Status matching to `access:` and `issue:` field-prefix search.
- Preserved access issue message search for `access:<error text>` and `issue:<error text>`.
- Updated the WPF search tooltip to include `access:readable`, `access:access issue`, and `issue:denied` examples.
- Added core coverage for `access:readable`, `access:access issue`, and access issue message searches.
- Added WPF fixture coverage for `access:readable` search, searched export filename hints, and access-prefix tooltip guidance.
- No access retry, permission change, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, scanner traversal, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-access-status-search.md`
- `docs/features/2026-05-28-access-status-review-field.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible in-memory review behavior and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do `access:readable`, `access:access issue`, and `issue:<error text>` make access review easier alongside the Access issues filter?

Rejected ideas buffer:

- Do not add retry-as-admin or permission-changing behavior as part of access status search.

### 2026-05-28: Add Storage Review Display Window

Status: completed

Evidence:

- The real scan contained 188,580 files and 37,740 folders, while the WPF grid intentionally displays at most 2,000 rows at once.
- The app explained the cap but did not let the user move beyond the first matched row window inside the app.

Implementation:

- Added read-only Previous rows and Next rows controls to the WPF review toolbar.
- Added a Storage Review Display Window label showing active row ranges such as `rows 1-2,000 of N matched`.
- Added `_currentDisplayStartIndex` to page through matched in-memory review rows without rescanning.
- Reset the display window to the first matched rows when filters, type filters, category filters, search, safety shortcuts, or Review View Reset change the active review lens.
- Updated completed-scan status and filter summary wording to use active row-window ranges.
- Corrected Scan Report Export row selection to include the active Storage Entry Type Filter.
- Added WPF fixture coverage for next/previous row windows, display-window reset, read-only status wording, and type-filtered exports.
- No scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-review-display-window.md`
- `docs/features/2026-05-28-storage-review-display-limit.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF review behavior and does not change architecture, persistence, security, deployment, cleanup execution, or public APIs.

Open questions:

- In the next real scan, are row-window controls enough, or would a virtualized tree/grid still be worth designing later?

Rejected ideas buffer:

- Do not rescan or discard matched rows when changing display windows.
- Do not make `Shortlist shown` apply to rows outside the current display window.

### 2026-05-28: Tighten Display Window Reset for Combo Filters

Status: completed

Evidence:

- Storage Review Display Window should reset when the active review lens changes.
- Programmatic filter helpers already reset the display window, but the actual WPF category/type combo-box event handlers needed the same reset path for manual UI selection.

Implementation:

- Reset `_currentDisplayStartIndex` in `CategoryFilterBox_SelectionChanged`.
- Reset `_currentDisplayStartIndex` in `EntryTypeFilterBox_SelectionChanged`.
- Added WPF test hooks that select type/category options through the real combo boxes.
- Added WPF fixture coverage proving combo-driven type and no-category filter changes reset the display window to the first matched rows.
- No scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `docs/features/2026-05-28-storage-review-display-window.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a small WPF state-consistency fix inside an existing read-only review behavior.

Open questions:

- None.

Rejected ideas buffer:

- Do not rely only on programmatic helper methods when the visible WPF controls use separate event handlers.

### 2026-05-28: Add Contents Column to Storage Review Grid

Status: completed

Evidence:

- The app already computed contained file/folder counts and exported them to CSV, but the main review grid only showed those counts after selecting a row.
- Large real-profile container rows are easier to compare when contents counts are visible before selection.

Implementation:

- Added a WPF grid `Contents` column bound to `StorageEntryRow.Contents`.
- Kept contents counts read-only and derived from the completed Storage Scan result.
- Added WPF fixture coverage proving a folder row exposes file/folder counts in the visible row data.
- No scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-row-contents-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF display of existing read-only scan context.

Open questions:

- In the next real scan, is the grid `Contents` column useful without making the table feel too crowded?

Rejected ideas buffer:

- Do not treat contents counts as storage savings or cleanup approval.

### 2026-05-28: Sort Contents Column by Contained Count

Status: completed

Evidence:

- The WPF grid `Contents` column displays formatted text, which is useful to read but weak as a sort value.
- The user-tested real scan surfaced many large containers, so comparing rows by total contained items is useful review context before selecting a row.

Implementation:

- Added `StorageEntryRow.ContainedTotalCount` as the numeric sort value for contained files plus descendant folders.
- Set the WPF grid `Contents` column `SortMemberPath` to `ContainedTotalCount`.
- Added WPF fixture coverage for the column sort contract and a folder row's numeric contained-item total.
- Kept the change read-only; no scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-row-contents-context.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible WPF review behavior for an existing read-only context signal.

Open questions:

- In the next real scan, does sorting by `Contents` help separate broad buckets from focused cleanup candidates?

Rejected ideas buffer:

- Do not rank cleanup safety by contained-item count alone.

### 2026-05-28: Add Relative Paths to Scan Report Export

Status: completed

Evidence:

- Real-profile rows all share the long `C:\Users\moxhe` prefix, which can make spreadsheet review harder than the in-app tree context.
- Earlier progress left a relative-path export column as an open follow-up for Scan Report Export.

Implementation:

- Added a `Relative path` CSV column to `StorageScanCsvExporter`.
- Derived relative paths from the completed Cleanup Scope when available; unsupported or outside-scope rows leave the relative path blank.
- Updated WPF export and Review Shortlist export to pass the completed scan scope to CSV generation.
- Added a WPF test hook for the current Scan Report Export CSV and fixture coverage for relative paths.
- Kept this report-only; no scanner traversal, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-scan-report-csv-export.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible CSV report shape improvement.

Open questions:

- None.

Rejected ideas buffer:

- Do not replace full paths in exports; relative paths are additional review context.

### 2026-05-28: Show Access Issue Examples in Safety Summary

Status: completed

Evidence:

- The first real scan reported 3 access issues.
- The app already counted and filtered access issues, but the top safety summary did not show any concrete example paths before the user clicked into the filtered review.

Implementation:

- Added bounded access issue examples to `StorageScanSafetySummary`.
- Derived up to three cleanup-scope-relative examples from completed scan rows, including scanner error text when available.
- Updated WPF Safety Summary text to show access examples when incomplete scan coverage exists.
- Added core coverage for relative access issue examples and scanner error text.
- Kept this read-only; no scanner retry, permission change, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, real-profile automation, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only summary display improvement.

Open questions:

- In the next real scan, are the three access issue examples enough context before using the Access issues filter?

Rejected ideas buffer:

- Do not add automatic elevated retries or permission changes for access issues.

### 2026-05-28: Add CI MVP Preflight Workflow

Status: completed

Evidence:

- Local MVP preflight already restores, builds, runs core tests, runs WPF app tests, runs the synthetic fixture generator in `-WhatIf` mode, and runs `git diff --check`.
- Remote pushes should use the same read-only gate so the command list does not drift from local verification.

Implementation:

- Added `.github/workflows/mvp-preflight.yml`.
- Configured the workflow for pushes and pull requests targeting `main`.
- Used a Windows runner, read-only repository permissions, .NET SDK `8.0.421`, and the existing `.\tools\Invoke-MvpPreflight.ps1` script.
- Documented CI preflight behavior in `README.md`.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- GitHub Actions `MVP Preflight` run `26575441204` for commit `711cfb6` completed with conclusion `success`.

Docs updated:

- `README.md`
- `docs/features/2026-05-28-ci-mvp-preflight.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible repository verification workflow using the existing MVP preflight.

Open questions:

- None.

Rejected ideas buffer:

- Do not duplicate the local preflight command list in CI YAML.

### 2026-05-28: Add Cleanup Scope Browse Action

Status: completed

Evidence:

- The WPF app allowed typing a Cleanup Scope path, but manual fixture/custom scope review is less error-prone with native folder selection.
- Cleanup Scope Selection should remain separate from Storage Scan so fixture review and real-profile preflight gates stay explicit.

Implementation:

- Added a `Browse...` button beside the Cleanup Scope path box.
- Used WPF's native `OpenFolderDialog` to choose a folder.
- Folder selection updates `ScopePathBox`, which keeps existing Cleanup Scope Safety Note and Scan Gate behavior authoritative.
- Disabled browsing while scanning and re-enabled it after fixture scans.
- Added WPF smoke assertions for browse visibility and availability.
- No scanner traversal changes, auto-scan behavior, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-browse-action.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible WPF usability improvement that does not change scan, persistence, security, deployment, or cleanup behavior.

Open questions:

- Does the extra header button fit comfortably during the next manual fixture UI pass?

Rejected ideas buffer:

- Do not auto-start a scan after folder selection.

### 2026-05-28: Promote Specific Rebuildable Cache Candidates

Status: completed

Evidence:

- The real scan screenshot showed large cache rows such as `NVIDIA\DXCache` and `pip\Cache`.
- Broad AppData-adjacent folders should stay conservative, but specific rebuildable cache rows should be easier to find through Quarantine candidates.

Implementation:

- Added a narrow `HasSpecificRebuildableCacheEvidence` classifier rule.
- Promoted specific GPU shader cache rows and package-cache rows with app-cache evidence to `Likely safe` / `Quarantine candidate`.
- Kept broad parent folders such as `pip`, `NVIDIA`, generic `AppData`, browser data, installed apps, Windows app data, game data, source-code paths, and Codex-related paths conservative.
- Reordered Selected Path Review Guidance so likely-safe cache rows still show cache-specific warning text and Review Shortlist / Quarantine Preview wording.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cache-specific-review-guidance.md`
- `docs/features/2026-05-28-specific-rebuildable-cache-candidates.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible classification and review-guidance refinement that does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do `DXCache` and `pip\Cache` appearing under Quarantine candidates make review easier without making broad parent folders look safe?

Rejected ideas buffer:

- Do not promote generic `AppData`, app, profile, package, source-code, Codex, or browser parent folders just because they contain a cache-like descendant.

### 2026-05-28: Add Storage Review Relative Path Column

Status: completed

Evidence:

- The real scan includes many rows under the same `C:\Users\moxhe` prefix and many short or repeated names, such as cache folders and hashed files.
- Scan Report Export already includes cleanup-scope-relative paths for spreadsheet review; the WPF grid did not show that same compact context directly.

Implementation:

- Added `StorageEntryRow.RelativePath`, derived from the completed Cleanup Scope.
- Added a WPF `Relative path` grid column after `Name`.
- Added `Relative:` to the selected-row detail context above `Parent:`.
- Kept full parent path context and all cleanup recommendations unchanged.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-hierarchy-context.md`
- `docs/features/2026-05-28-storage-review-relative-path-column.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only UI review context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Does the extra grid column fit comfortably during the next manual fixture and real-profile review passes?

Rejected ideas buffer:

- Do not remove the full `Parent` column yet; relative path and parent path answer different review questions.

### 2026-05-28: Show Safety Summary Candidate Examples

Status: completed

Evidence:

- Storage Scan Safety Summary already counted Quarantine candidates and showed access issue examples.
- Real-profile review benefits from concrete candidate examples before using filters, search, Review Shortlist, or Quarantine Preview.

Implementation:

- Added `StorageScanSafetySummary.QuarantineCandidateExamples`.
- Built up to three largest Quarantine candidate examples using cleanup-scope-relative paths and row sizes.
- Added `Candidate examples:` to WPF Safety Summary text.
- Kept examples read-only and separate from Review Shortlist, Quarantine Preview, and cleanup approval.
- No scanner traversal changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `docs/features/2026-05-28-safety-summary-candidate-examples.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only summary context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Do the candidate examples help real-profile review, or does the Safety Summary become too dense?

Rejected ideas buffer:

- Do not use Safety Summary candidate examples as cleanup approval or as a replacement for Review Shortlist and Quarantine Preview.

### 2026-05-28: Use Relative Paths for Quarantine Preview Blocker Examples

Status: completed

Evidence:

- Quarantine Preview protected-descendant blockers were useful, but absolute descendant paths are noisy in real-profile review where every row shares the same Cleanup Scope prefix.
- The app now uses relative paths elsewhere in the grid, detail pane, safety summary, and CSV reports.

Implementation:

- Updated protected-descendant blocker reasons to format descendant examples relative to the Cleanup Scope.
- Kept absolute source/destination paths in preview row details for precise identity.
- Added core and WPF smoke coverage for `.cache\codex-runtimes` relative blocker evidence.
- No scanner traversal changes, preview eligibility changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview-protected-descendant-blocker.md`
- `docs/features/2026-05-28-quarantine-preview-relative-blocker-examples.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only preview wording and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, are relative blocker examples enough, or does the preview pane need a structured blocked-descendant table?

Rejected ideas buffer:

- Do not remove absolute source paths from preview row details; they remain useful for precise identity.

### 2026-05-28: Show Safety Summary No Category Examples

Status: completed

Evidence:

- Storage Scan Safety Summary counted Uncategorized Results and exposed a No category review shortcut, but it did not show concrete examples.
- The real scan surfaced many unfamiliar rows where the first step is classification, not cleanup.

Implementation:

- Added `StorageScanSafetySummary.UncategorizedExamples`.
- Built up to three largest no-category examples using cleanup-scope-relative paths and row sizes.
- Added `No category examples:` to WPF Safety Summary text.
- Kept examples read-only and separate from cleanup recommendations, Review Shortlist, and Quarantine Preview.
- No scanner traversal changes, classification changes, recommendation changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-scan-safety-summary.md`
- `docs/features/2026-05-28-safety-summary-no-category-examples.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only summary context and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do no-category examples help target classification work, or does the Safety Summary become too dense?

Rejected ideas buffer:

- Do not treat no-category examples as cleanup candidates; they are classification prompts.

### 2026-05-28: Add Storage Size Threshold Filter

Status: completed

Evidence:

- User's real scan screenshot showed a 58.02 GB profile with many large rows, including multi-GB app/cache containers and many smaller recursive rows.
- The app already had review/category/type/search lenses, but no direct way to focus on rows above a chosen size.

Implementation:

- Added `StorageSizeThresholdFilter` with All sizes, 1 MB+, 100 MB+, 1 GB+, 5 GB+, and 10 GB+.
- Applied size-threshold filtering in `StorageScanReview` so WPF display, review-window paging, and Scan Report Export use the same active review lens.
- Added a WPF Size combo box, filter-summary wording, reset behavior, and export filename segment.
- Kept the change read-only; no scanner traversal changes, classification changes, recommendation changes, real-profile automation, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-storage-size-threshold-filter.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible read-only review filtering and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- In the next real scan, do `1 GB+`, `5 GB+`, and `10 GB+` make large-row triage easier without overemphasizing size as safety evidence?

Rejected ideas buffer:

- Do not make large rows more likely to be cleanup candidates solely because they match a size threshold.

### 2026-05-28: Extend Game Mod Data Protection Hints

Status: completed

Evidence:

- The real scan surfaced game and mod-related folders that should be clearer than generic uncategorized rows before any cleanup execution exists.
- Conservative app-data classification already treats game data as Protected Location / High risk / Keep.

Implementation:

- Extended game data hints to include Minecraft, OptiFine, CurseForge, Modrinth, Vortex, and Nexus Mods paths.
- Added fixture coverage proving OptiFine, CurseForge, and Vortex folders are `Game data`, `High risk`, and `Keep`.
- Kept the change conservative and read-only; no scanner traversal changes, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-conservative-app-data-classification.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This extends an existing conservative classifier rule and does not change architecture, persistence, security, deployment, or cleanup execution.

Open questions:

- Which specific game or mod-manager cache subfolders, if any, should become cleanup exceptions after manual review?

Rejected ideas buffer:

- Do not treat mod-manager downloads or profiles as safe bloat just because they can be large.

### 2026-05-29: Add Cloud Sync and Credential Protection

Status: completed

Evidence:

- The app is intended to review `C:\Users\moxhe`, where cloud sync roots and credential/key/password-manager paths can be large or hidden but should not be cleanup targets by default.
- Selected File Content Preview should help with unfamiliar text files without exposing secrets from credential rows.

Implementation:

- Added `CloudSyncData` and `CredentialData` Bloat Category values.
- Added conservative classifier hints for common cloud sync providers and credential/password-manager/key paths.
- Added user-facing labels in WPF rows, category filters, Scan Report Export, and Quarantine Preview CSV.
- Blocked Selected File Content Preview for Credential Data.
- Added fixture coverage for OneDrive, Dropbox, SSH keys, Bitwarden, KeePass vault files, and credential preview blocking.
- Kept the change conservative and read-only; no scanner traversal changes, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, or real user file access was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-file-content-preview.md`
- `docs/features/2026-05-29-cloud-sync-credential-protection.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible conservative classification and preview-safety refinement that does not change architecture, persistence, deployment, or cleanup execution.

Open questions:

- Which cloud-provider cache folders, if any, should become cleanup exceptions after manual review?

Rejected ideas buffer:

- Do not preview credential file contents merely because the file is small or text-like.
- Do not classify cloud sync roots as cleanup candidates without provider-specific review.

### 2026-05-29: Add Quarantine Root Preview Selection

Status: completed

Evidence:

- The user requested a quarantine folder preferably on `D:` and easy undo later.
- Quarantine Preview already showed destination paths, but the preview root was an invisible fixed default.
- Keeping the root visible and editable improves review without adding cleanup execution.

Implementation:

- Added a WPF Quarantine root text box defaulting to `D:\WindowsFileCleanerQuarantine`.
- Routed Quarantine Preview destination paths through the typed Quarantine Root Selection.
- Cleared stale Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft output when the root changes.
- Added WPF smoke coverage proving custom preview roots affect destination paths and do not create folders.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-preview.md`
- `docs/features/2026-05-29-quarantine-root-preview-selection.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only preview UI refinement and does not change cleanup execution, persistence, deployment, or manifest writes.

Open questions:

- In the next manual fixture or real-profile pass, is the Quarantine root field readable in the wrapped review toolbar?

Rejected ideas buffer:

- Do not persist the Quarantine Root Selection until actual Quarantine execution needs local settings.
- Do not create or validate the quarantine folder by touching the filesystem during preview.

### 2026-05-29: Add Quarantine Root Browse Action

Status: completed

Evidence:

- The user prefers a quarantine location on `D:`.
- Typing a root path works, but browsing is less error-prone during manual review.
- Browsing can update preview destinations without creating folders or moving files.

Implementation:

- Added `BrowseQuarantineRootButton` next to the Quarantine root text box.
- Added a native folder picker for Quarantine Root Selection.
- Added WPF smoke coverage that the browse action is present and enabled before scanning.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-root-browse-action.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only UI refinement and does not change cleanup execution, persistence, deployment, or manifest writes.

Open questions:

- In the next manual fixture pass, does browsing to a `D:` root make preview destination review easier?

Rejected ideas buffer:

- Do not treat browsing to a root as approval to execute Quarantine.
- Do not create the selected folder during browse or preview.

### 2026-05-29: Split Review Shortlist and Quarantine Toolbar

Status: completed

Evidence:

- Quarantine Root Selection and browse controls made the review action toolbar wider.
- The next recommended work is a visible fixture pass, so keeping review controls easy to scan is useful before more manual testing.

Implementation:

- Kept search, row-window, type, size, and category controls in the existing wrapping Review Action toolbar.
- Moved Review Shortlist and Quarantine Preview controls into a separate wrapping `ReviewShortlistToolbar`.
- Extended the WPF smoke layout assertion to require the new wrapping toolbar.
- Kept the change UI-only and read-only; no scanner behavior, cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `docs/features/2026-05-28-wpf-review-toolbar-layout-polish.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is reversible UI layout polish and does not change architecture, persistence, security, deployment, or cleanup behavior.

Open questions:

- In the next manual fixture pass, does the separated shortlist/quarantine toolbar make the Quarantine root controls easier to read?

Rejected ideas buffer:

- Do not treat structural layout assertions as proof of visible polish; keep the manual fixture pass as the visual check.

### 2026-05-29: Add Quarantine Root Safety Note

Status: completed

Evidence:

- Quarantine Root Selection is now typed or browsable, so obvious relative-path mistakes should be caught before Quarantine Preview builds destination paths.
- The user prefers quarantine on `D:`, but fixture review can still benefit from fully qualified non-`D:` preview roots.

Implementation:

- Added `QuarantineRootSafetyNote` and `QuarantineRootSafetyNoteBuilder`.
- Added WPF safety-note text under the Quarantine root controls.
- Disabled `Preview quarantine` when the current root is relative or invalid.
- Kept blank roots falling back to `D:\WindowsFileCleanerQuarantine`.
- Kept fully qualified non-`D:` roots usable for preview with non-preferred wording.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-root-safety-note.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only preview guard and does not change cleanup execution, persistence, deployment, or manifest writes.

Open questions:

- Should actual Quarantine execution require an existing `D:` folder, offer to create it, or support a stricter destination policy?

Rejected ideas buffer:

- Do not create or probe the quarantine folder just to decide whether Quarantine Preview can run.

### 2026-05-29: Add Quarantine Execution Gate

Status: completed

Evidence:

- Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft already prove preview and undo metadata shape.
- Actual Quarantine execution still needs explicit confirmation semantics before file-moving code exists.

Implementation:

- Added `QuarantineExecutionGate` and `QuarantineExecutionGateBuilder`.
- Added WPF confirmation text, disabled `Execute quarantine` button, and execution gate readout.
- Required exact `QUARANTINE` text while preserving confirmation-readiness blockers.
- Kept execution unavailable because Quarantine execution is not implemented.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-execution-gate.md`
- `.codex/progress.md`

ADRs:

- No new ADR. This is a reversible read-only gate and does not decide file-moving layout, manifest write order, or Undo Quarantine behavior.

Open questions:

- What exact manifest write order should actual Quarantine execution use?
- Should future execution require a selected manifest path, a generated action id, or both?

Rejected ideas buffer:

- Do not add real file-moving code in the same packet as the first visible execution gate.
- Do not let matching confirmation text override data blockers or unimplemented execution support.

### 2026-05-29: Add Quarantine Action Draft

Status: completed

Evidence:

- Quarantine execution needs a concrete action-scoped destination and manifest layout before file-moving code exists.
- Preview paths should remain preview-only and separate from future executed quarantine paths.

Implementation:

- Added ADR 0004 for action-scoped quarantine layout.
- Added `QuarantineActionDraft`, `QuarantineActionEntryDraft`, and `QuarantineActionDraftBuilder`.
- Mapped future item paths under `<quarantine-root>\actions\<action-id>\items\...`.
- Mapped the future Restore Manifest path to `<quarantine-root>\actions\<action-id>\restore-manifest.json`.
- Added consistency checks across Quarantine Preview, Restore Manifest Draft, and Quarantine Confirmation Draft metadata.
- Added WPF Quarantine Execution Gate readout for action items root and restore manifest path.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0004-use-action-scoped-quarantine-layout.md`
- `docs/features/2026-05-29-quarantine-action-draft.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0004-use-action-scoped-quarantine-layout.md`.

Open questions:

- What exact manifest write order should actual Quarantine execution use?
- How should partial move failures update the executed Restore Manifest?

Rejected ideas buffer:

- Do not reuse preview paths as executed quarantine paths.
- Do not use a flat quarantine root for all moved items.

### 2026-05-29: Add Write-Ahead Restore Manifest Model

Status: completed

Evidence:

- Sidecar safety review found a manifest timing conflict: domain context said to persist a Restore Manifest before moving files, while ADR 0003 still said after file moves are attempted.
- Actual Quarantine execution needs a recoverable write order and partial-failure states before any file-moving code exists.

Implementation:

- Added ADR 0005 for write-ahead Restore Manifest ordering.
- Added `RestoreManifest`, `RestoreManifestEntry`, `RestoreManifestBuilder`, and `RestoreManifestJsonSerializer`.
- Added `RestoreManifestActionStatus` and `RestoreManifestEntryStatus`.
- Built a planned Restore Manifest from the Quarantine Action Draft using action-scoped quarantine paths, not preview paths.
- Added in-memory status transitions for Moving, Moved, Failed, Completed, Partial failure, and Failed action outcomes.
- Added WPF Quarantine Execution Gate readout for write-ahead manifest status and write order.
- Kept the change read-only; no cleanup execution, Quarantine execution, Undo Quarantine, manifest file writing, folder creation, or real-profile automation was added.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0003-use-json-restore-manifest.md`
- `docs/decisions/0004-use-action-scoped-quarantine-layout.md`
- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/features/2026-05-29-quarantine-action-draft.md`
- `docs/features/2026-05-29-quarantine-execution-gate.md`
- `docs/features/2026-05-29-write-ahead-restore-manifest.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0005-use-write-ahead-restore-manifest.md`.

Open questions:

- What exact recovery UI should handle Moving entries after interruption?
- How should the future app surface leftover temp files after a hard crash?

Rejected ideas buffer:

- Do not write a final-only manifest after all moves succeed.
- Do not keep the old write-after-attempt wording; it creates a recovery gap.
- Do not add file-moving API allowlists until a narrow execution component exists.

### 2026-05-29: Add Restore Manifest File Store

Status: completed

Evidence:

- ADR 0005 requires a planned Restore Manifest to be written before any future move.
- Sidecar safety review recommended introducing write APIs only in a narrow execution component with a strict source allowlist.
- Manifest writing can be proven against fixtures before adding file-moving code.

Implementation:

- Added ADR 0006 for temp-file replacement Restore Manifest writes.
- Added `RestoreManifestFileStore` and `RestoreManifestFileWriteResult`.
- The file store validates that `ManifestPath` stays inside `ActionRootPath` and that the filename is `restore-manifest.json`.
- The file store writes JSON to a temporary file in the same action folder, then replaces or moves it into place.
- Added fixture-backed tests for first write, replacement write, invalid outside paths, invalid filenames, temp cleanup, source preservation, and not creating the action items folder.
- Updated the source-level filesystem-call regression to allow write APIs only for user-selected CSV exports and `RestoreManifestFileStore`.
- Kept WPF execution unavailable; no scanned files are moved, deleted, quarantined, or restored by the app.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `docs/features/2026-05-29-write-ahead-restore-manifest.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`.

Open questions:

- How should the future app surface leftover temp files after a hard crash?
- Should future Undo Quarantine expose a manifest integrity check before restore?

Rejected ideas buffer:

- Do not write manifests directly to the final path.
- Do not loosen the filesystem-call guard globally.
- Do not wire WPF execution in the same packet as the first manifest writer.

### 2026-05-29: Add Fixture-First Quarantine Executor

Status: completed

Evidence:

- Restore Manifest File Store is fixture-tested and can write action-scoped manifests.
- Sidecar safety review recommended a separate executor component, a narrow allowlist, and keeping WPF execution closed.
- MVP needs actual Quarantine movement eventually, but synthetic fixture execution should prove move semantics first.

Implementation:

- Added ADR 0007 for the fixture-first Quarantine Executor boundary.
- Added `QuarantineExecutor`, `QuarantineExecutionResult`, and `QuarantineExecutionEntryResult`.
- The executor writes the planned Restore Manifest before any move, writes Moving before each move attempt, revalidates source/destination/reparse status, moves the file or folder, then writes Moved or Failed.
- The executor continues after per-entry move failures so partial-failure manifests can be produced.
- The executor stops before later moves when a manifest write fails.
- Extended the source-level filesystem-call regression to allow `Directory.CreateDirectory`, `Directory.Move`, and `File.Move` only in `QuarantineExecutor`.
- Kept WPF execution unavailable; `Execute quarantine` remains disabled/status-only.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `docs/decisions/0007-use-fixture-first-quarantine-executor.md`
- `docs/features/2026-05-29-quarantine-executor-fixture-first.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0007-use-fixture-first-quarantine-executor.md`.

Open questions:

- What exact WPF stale-state checks are required before calling the executor?
- What recovery UI should handle Moving entries after interruption?
- How should the app surface leftover temp manifest files after a hard crash?

Rejected ideas buffer:

- Do not wire WPF execution in the same packet as the first executor.
- Do not implement rollback inside Quarantine Executor; Undo Quarantine needs a separate design.
- Do not overwrite existing quarantine destinations.

### 2026-05-29: Add Fixture-First Undo Quarantine

Status: completed

Evidence:

- User requested quarantine on `D:` with an easy undo path.
- Core fixture-first Quarantine Executor can already produce Moved Restore Manifest entries.
- ADR 0008 selects a separate fixture-first Undo Quarantine Executor before WPF execution or WPF undo is wired.

Implementation:

- Added `UndoQuarantineExecutor`, `UndoQuarantineResult`, and `UndoQuarantineEntryResult`.
- Extended Restore Manifest action statuses with Restoring, Restored, RestorePartialFailure, and RestoreFailed.
- Extended Restore Manifest entry statuses with Restoring, Restored, and RestoreFailed.
- Added restore start/completion timestamps to Restore Manifest entries.
- Undo restores only Moved entries, writes Restoring before each restore attempt, refuses original-path collisions, keeps move failures for recovery review, checks missing quarantine paths and reparse points, then writes Restored or RestoreFailed.
- Undo continues after per-entry restore failures and stops before later restore attempts when manifest writing fails.
- Extended the source-level filesystem-call regression to allow `Directory.CreateDirectory`, `Directory.Move`, and `File.Move` only in `UndoQuarantineExecutor` for restore movement.
- Kept WPF Quarantine execution and WPF Undo Quarantine unavailable.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0005-use-write-ahead-restore-manifest.md`
- `docs/decisions/0006-use-temp-replace-restore-manifest-writes.md`
- `docs/decisions/0007-use-fixture-first-quarantine-executor.md`
- `docs/decisions/0008-use-fixture-first-undo-quarantine.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-quarantine-executor-fixture-first.md`
- `docs/features/2026-05-29-restore-manifest-file-store.md`
- `docs/features/2026-05-29-undo-quarantine-fixture-first.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0008-use-fixture-first-undo-quarantine.md`.

Open questions:

- What UI should discover and select existing Restore Manifests?
- Should successful WPF Undo Quarantine offer to clean up empty action folders?
- How should the app surface leftover temp manifest files after a hard crash?

Rejected ideas buffer:

- Do not overwrite original paths during undo.
- Do not automatically delete quarantine action folders after restore.
- Do not implement same-execution rollback inside Quarantine Executor.
- Do not wire WPF Undo Quarantine in the same packet as core fixture undo.

### 2026-05-29: Add Fixture-Only WPF Quarantine Execution

Status: completed

Evidence:

- Core Quarantine Executor and Undo Quarantine Executor are fixture-tested.
- ADR 0009 selects visible WPF execution for fixture Cleanup Scopes only.
- Real-profile and custom non-fixture execution must remain unavailable.

Implementation:

- Added ADR 0009 for fixture-only WPF Quarantine execution.
- Extended `QuarantineConfirmationDraft` so execution availability can be true for fixture scopes and false elsewhere.
- Updated `QuarantineExecutionGateBuilder` to open only when readiness blockers are clear, exact `QUARANTINE` is entered, and execution is available.
- Wired WPF `Execute quarantine` to `QuarantineExecutor.Execute` for recognized fixture Cleanup Scopes.
- After execution, WPF shows execution result evidence, clears stale Review Shortlist state, disables re-execution for the current preview, and tells the user to rescan.
- Added WPF smoke coverage proving a fixture execution moves a synthetic file and writes `restore-manifest.json`.
- Added WPF smoke coverage proving a custom non-fixture scope remains blocked even after exact confirmation.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0009-use-fixture-only-wpf-quarantine-execution.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-wpf-fixture-only-quarantine-execution.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0009-use-fixture-only-wpf-quarantine-execution.md`.

Open questions:

- What visible WPF Undo Quarantine flow should discover and restore manifests?
- What additional confirmation or backup step should exist before real-profile execution?
- Should a fixture execution clear the grid entirely or is stale-state wording enough?

Rejected ideas buffer:

- Do not enable real-profile WPF execution in this packet.
- Do not implement file movement directly in WPF.
- Do not leave the gate enabled after execution.
- Do not imply WPF Undo Quarantine exists after fixture execution.

### 2026-05-29: Add WPF Current Fixture Undo Quarantine

Status: completed

Evidence:

- Core Undo Quarantine is fixture-tested.
- WPF fixture-only Quarantine execution produces a current in-memory Restore Manifest.
- ADR 0010 selects current-fixture-execution WPF undo before broad manifest discovery or real-profile undo.

Implementation:

- Added ADR 0010 for WPF current fixture undo.
- Added `Undo fixture quarantine` to the WPF execution gate area.
- Wired WPF undo to `UndoQuarantineExecutor.Undo` for the current fixture Restore Manifest.
- WPF undo restores the synthetic source file, updates Restore Manifest status, shows undo result evidence, disables repeat undo, and preserves stale-state wording.
- Kept real-profile WPF undo and manifest discovery unavailable.
- Extended WPF smoke coverage to execute and undo a fixture Quarantine action.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-wpf-current-fixture-undo-quarantine.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`.

Open questions:

- What UI should discover and select old Restore Manifests?
- Should successful undo offer to clean up empty action folders?
- What additional confirmation should real-profile undo require?

Rejected ideas buffer:

- Do not add manifest discovery in this packet.
- Do not enable real-profile undo.
- Do not delete quarantine folders after undo.
- Do not implement restore movement in WPF.

### 2026-05-29: Add Quarantine Manifest Discovery

Status: completed

Evidence:

- WPF current-fixture undo proves only the current in-memory Restore Manifest.
- ADR 0011 selects read-only discovery before any broad WPF Undo Quarantine.
- Existing Restore Manifests are action-scoped under `<quarantine-root>\actions\<action-id>\restore-manifest.json`.

Implementation:

- Added ADR 0011 for read-only Quarantine Manifest Discovery.
- Added Restore Manifest JSON deserialization.
- Added `QuarantineManifestDiscovery`, `QuarantineManifestDiscoveryBuilder`, `QuarantineManifestDiscoveryIssue`, and `RestoreManifestSummary`.
- Discovery reads direct action-scoped `restore-manifest.json` files, summarizes valid manifests, and reports missing/invalid/path-mismatch issues.
- Added WPF `Discover manifests` status-only action and pane.
- Kept discovered-manifest restore, real-profile execution, permanent deletion, cleanup history, and quarantine-folder cleanup unavailable.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0011-use-read-only-quarantine-manifest-discovery.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-quarantine-manifest-discovery.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0011-use-read-only-quarantine-manifest-discovery.md`.

Open questions:

- What UI should select one old Restore Manifest for future Undo Quarantine?
- Should discovery support browsing directly to a single manifest file?
- Should successful undo offer to clean up empty action folders?

Rejected ideas buffer:

- Do not restore discovered old manifests in this packet.
- Do not call discovery cleanup history.
- Do not scan the entire quarantine root recursively.
- Do not clean up empty action folders.

### 2026-05-29: Fix MVP Preflight Failure Propagation

Status: completed

Evidence:

- During Quarantine Manifest Discovery work, a WPF app test failure still let `Invoke-MvpPreflight.ps1` print `MVP preflight passed`.
- PowerShell native command failures did not throw from `$ErrorActionPreference = "Stop"` without checking `$LASTEXITCODE`.

Implementation:

- Updated `Invoke-PreflightStep` to reset and check `$global:LASTEXITCODE` for every step.
- Added a source-level regression check that the preflight script captures and throws on native non-zero exit codes.
- Updated README preflight wording to state that non-zero child commands fail preflight/CI.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-mvp-preflight-failure-propagation.md`
- `.codex/progress.md`

ADRs:

- No ADR. This is a reversible verification script fix.

Open questions:

- Should the preflight script also emit structured machine-readable output later?

Rejected ideas buffer:

- Do not add a fake failure mode to the production preflight script.
- Do not duplicate the local preflight command list in CI.

### 2026-05-29: Add Restore Readiness Preview

Status: completed

Evidence:

- Quarantine Manifest Discovery can show old action-scoped Restore Manifests but did not show entry-level restore blockers.
- ADR 0012 selects read-only Restore Readiness Preview before broad WPF Undo Quarantine.
- Undo Quarantine Executor already defines restore blockers such as missing quarantine paths and original-path collisions.

Implementation:

- Added ADR 0012 for read-only Restore Readiness Preview.
- Added `RestoreReadinessPreview`, `RestoreReadinessManifestPreview`, `RestoreReadinessEntryPreview`, `RestoreReadinessDisposition`, and `RestoreReadinessPreviewBuilder`.
- Extended discovery to retain valid Restore Manifest objects for readiness evaluation.
- WPF now exposes `Preview all-manifest readiness` as a status-only action under the selected Quarantine Root.
- Readiness preview reports restorable, blocked, already-restored, recovery-review, and not-moved entries without restoring files.
- Kept discovered-manifest restore, real-profile execution, permanent deletion, cleanup history, manifest writes, and quarantine-folder cleanup unavailable.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0012-use-read-only-restore-readiness-preview.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0012-use-read-only-restore-readiness-preview.md`.

Open questions:

- What UI should select one discovered Restore Manifest for execution?
- What exact confirmation should broad WPF Undo Quarantine require?
- Should successful restore offer empty action-folder cleanup?

Rejected ideas buffer:

- Do not restore discovered old manifests in this packet.
- Do not treat readiness as approval to restore.
- Do not add manifest selection yet.
- Do not clean up empty action folders.

### 2026-05-29: Add Selected Restore Manifest Review

Status: completed

Evidence:

- Quarantine Manifest Discovery can find older action-scoped Restore Manifests.
- Restore Readiness Preview can evaluate all discovered manifests, but future broad Undo Quarantine needs explicit one-manifest selection.
- ADR 0013 selects read-only Selected Restore Manifest Review before any selected old-manifest restore execution.

Implementation:

- Added ADR 0013 for read-only Selected Restore Manifest Review.
- Added `SelectedRestoreManifestReview` and `SelectedRestoreManifestReviewBuilder`.
- WPF now populates `RestoreManifestSelectionBox` after `Discover manifests`, auto-selects the newest discovered Restore Manifest, and exposes `Preview selected readiness`.
- Selected readiness evaluates only the selected Restore Manifest and reports selection issues for missing discovery, blank selection, or stale paths.
- Kept discovered-manifest restore, real-profile execution, permanent deletion, cleanup history, manifest writes, and quarantine-folder cleanup unavailable.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0013-use-read-only-selected-restore-manifest-review.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-selected-restore-manifest-review.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0013-use-read-only-selected-restore-manifest-review.md`.

Open questions:

- What exact confirmation phrase should selected broad Undo Quarantine require?
- Should future restore execution allow only manifests with zero readiness blockers?
- Should successful restore offer empty action-folder cleanup?

Rejected ideas buffer:

- Do not restore selected old manifests in this packet.
- Do not treat selected readiness as approval to restore.
- Do not add cleanup history in this packet.
- Do not remove all-manifest Restore Readiness Preview.

### 2026-05-29: Add Selected Restore Confirmation Gate

Status: completed

Evidence:

- Selected Restore Manifest Review can focus one discovered Restore Manifest and preview selected readiness.
- Future selected restore execution needs exact confirmation semantics before any file-moving workflow is exposed.
- ADR 0014 selects read-only Selected Restore Confirmation Draft and Selected Restore Execution Gate before fixture-first selected restore execution.

Implementation:

- Added ADR 0014 for read-only Selected Restore Confirmation Gate.
- Added `SelectedRestoreConfirmationDraft`, `SelectedRestoreConfirmationDraftBuilder`, `SelectedRestoreExecutionGate`, and `SelectedRestoreExecutionGateBuilder`.
- Core selected restore gate requires exact `RESTORE`, clear blockers, and explicit execution availability before `CanExecute` can open.
- WPF now exposes `Preview selected restore gate` and a selected restore confirmation text box after selected readiness.
- WPF passes selected restore execution as unavailable, so typing `RESTORE` can match but still leaves `Can execute: no`.
- Kept selected old-manifest restore, real-profile execution, permanent deletion, cleanup history, manifest writes, and quarantine-folder cleanup unavailable.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0014-use-read-only-selected-restore-confirmation-gate.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-selected-restore-confirmation-gate.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0014-use-read-only-selected-restore-confirmation-gate.md`.

Open questions:

- Should future selected restore execution require zero blocked, recovery-review, and not-moved readiness rows?
- Should successful selected restore offer empty action-folder cleanup?
- What stale-state wording should appear when readiness changes between preview and execution?

Rejected ideas buffer:

- Do not restore selected old manifests in this packet.
- Do not treat typed `RESTORE` as approval while execution is unavailable.
- Do not expose a selected restore execution button yet.
- Do not reuse the Quarantine confirmation phrase for restore.

### 2026-05-29: Add Fixture-only Selected Restore Execution

Status: completed

Evidence:

- Selected Restore Manifest Review and Selected Restore Confirmation Gate prove selected manifest, selected readiness, and exact `RESTORE` confirmation semantics.
- Current-fixture WPF undo proves the visible app can call `UndoQuarantineExecutor` safely for synthetic fixture files.
- ADR 0015 selects fixture-only selected restore execution before any real-profile selected restore workflow.

Implementation:

- Added ADR 0015 for fixture-only selected restore execution.
- Added WPF `Restore selected fixture manifest` action.
- WPF now marks selected restore execution available only when the selected Restore Manifest Cleanup Scope is recognized as a fixture.
- WPF calls `UndoQuarantineExecutor.Undo` for selected discovered fixture Restore Manifests after clean selected readiness and exact `RESTORE`.
- WPF shows selected restore result evidence and stale-state wording, then disables repeat selected restore for the current selected review.
- Custom non-fixture selected restore remains blocked even when `RESTORE` is typed.
- Kept real-profile selected restore, real-profile Quarantine execution, permanent deletion, cleanup history, and quarantine-folder cleanup unavailable.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors after rerunning by itself.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors after rerunning by itself.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0015-use-fixture-only-selected-restore-execution.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-fixture-only-selected-restore-execution.md`
- `.codex/progress.md`

ADRs:

- Added `docs/decisions/0015-use-fixture-only-selected-restore-execution.md`.

Open questions:

- Should successful selected restore offer empty action-folder cleanup?
- What extra backup/manual review should real-profile selected restore require?
- Should selected restore refresh discovery automatically after execution or require the user to rediscover?

Rejected ideas buffer:

- Do not enable real-profile selected restore in this packet.
- Do not implement restore movement in WPF.
- Do not clean up action folders after selected restore.
- Do not let selected restore run more than once for the same current selected review.

### 2026-05-29: Add Selected Folder Child Focus

Status: completed

Evidence:

- The user's real scan screenshot showed large container rows and short/hash-like child names that are hard to reason about in a flat list.
- Existing Child Breakdown shows only a bounded detail-pane summary; the main grid still needed a direct way to focus on a selected folder's immediate children.
- Storage Review Search already provided an in-memory read-only lens that could be safely extended with parent matching.

Implementation:

- Added `parent:` Storage Review Search support through `StorageReviewSearchField.Parent`.
- Added WPF `Show children` selected-row action for folders.
- `Show children` resets review/category/type/size filters to All, applies `parent:<selected folder full path>`, focuses the grid on immediate child rows, and reports that no files were modified.
- Files keep `Show children` disabled.
- Kept real-profile cleanup execution, Quarantine execution changes, Undo changes, permanent deletion, cleanup history, and classification changes out of this packet.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed before rebuild.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed before rebuild.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed after rebuild.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuild.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `docs/features/2026-05-29-selected-folder-child-focus.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible in-memory review and WPF inspection improvement that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Open questions:

- Should a later tree/grid view replace Selected Folder Child Focus if real-profile review still feels too flat?
- Should repeated child focus show breadcrumbs?

Rejected ideas buffer:

- Do not build a full tree view before proving this smaller selected-folder focus action.
- Do not preserve active filters during `Show children`; hidden immediate children would make the action feel broken.
- Do not make child focus imply cleanup safety or shortlist approval.

### 2026-05-29: Add Storage Hotspot Trail

Status: completed

Evidence:

- The user's real scan screenshot showed large nested storage buckets where the next review step is understanding where size concentrates.
- Child Breakdown and Selected Folder Child Focus make immediate children inspectable, but the selected-row detail pane still lacked a fast largest-branch cue.
- Existing `StorageEntry.Children` can provide this in-memory without rescanning or changing cleanup eligibility.

Implementation:

- Added `StorageHotspotTrailEntry` and `StorageHotspotTrailBuilder`.
- Added WPF Largest hotspot trail detail text for selected rows.
- The trail follows the largest child at each level, stops at a terminal file/folder or bounded depth, and uses deterministic name tie-breaking.
- The WPF detail pane says trail sizes overlap and are not storage savings.
- Files explicitly report that they do not have descendant hotspot trails.
- Kept real-profile cleanup execution, Quarantine execution changes, Undo changes, permanent deletion, cleanup history, and classification changes out of this packet.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-storage-hotspot-trail.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible read-only review aid that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Open questions:

- Should a later version show multiple hotspot trails instead of only the single largest-child trail?
- Should the trail eventually support clickable rows that focus the grid on that child?

Rejected ideas buffer:

- Do not call the trail a savings estimate.
- Do not use the hotspot trail to auto-shortlist or recommend cleanup.
- Do not build a full tree view before manual review proves the smaller trail/focus tools are insufficient.

### 2026-05-29: Add Selected Folder Subtree Summary

Status: completed

Evidence:

- The user's real scan screenshot showed large container rows where the next decision depends on the descendant mix, not just parent size.
- Child Breakdown, Storage Hotspot Trail, and Selected Folder Child Focus help inspect shape, but the detail pane still lacked a compact descendant risk/count summary.
- Quarantine Preview already blocks broad parents with protected descendants, so a pre-preview read-only summary helps explain why broad parents should stay inspection-first.

Implementation:

- Added `StorageSubtreeReviewSummary` and `StorageSubtreeReviewSummaryBuilder`.
- Added WPF Descendant review summary detail text for selected rows.
- The summary excludes the selected folder itself, counts descendant files/folders, Importance Ratings, Quarantine candidates, Protected Location rows, Access issues, Reparse points, and Uncategorized Results.
- The summary shows bounded examples for Quarantine candidates, Protected Location rows, Access issues, and Uncategorized Results.
- The WPF detail pane says recursive row sizes overlap and are not storage savings or cleanup approval.
- Files explicitly report that they do not have descendant subtree summaries.
- Kept real-profile cleanup execution, Quarantine execution changes, Undo changes, permanent deletion, cleanup history, and classification changes out of this packet.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-selected-folder-subtree-summary.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible read-only review aid that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Open questions:

- Should the summary eventually include clickable review shortcuts for each descendant bucket?
- Should the summary appear as a compact table instead of text after manual layout review?

Rejected ideas buffer:

- Do not turn descendant summary counts into cleanup approval.
- Do not sum flattened recursive row sizes as savings.
- Do not add clickable bulk shortcuts before manual review proves the wording/layout works.

### 2026-05-29: Add Selected Folder Descendant Focus

Status: completed

Evidence:

- The user's real scan screenshot showed large flat results and nested buckets where immediate-child focus is useful but not always enough.
- Selected Folder Subtree Summary shows descendant risk/count context, but the grid still needed a recursive selected-folder lens for sorting, paging, filtering, shortlisting, and export.
- Existing Storage Review Search could support this safely as an in-memory read-only field prefix.

Implementation:

- Added `under:` Storage Review Search support through `StorageReviewSearchField.Under`.
- Added WPF `Show descendants` selected-row action for folders.
- `Show descendants` resets review/category/type/size filters to All, applies `under:<selected folder full path>`, focuses the grid on descendant rows, excludes the selected folder itself, and reports that no files were modified.
- Files keep `Show descendants` disabled.
- Kept real-profile cleanup execution, Quarantine execution changes, Undo changes, permanent deletion, cleanup history, and classification changes out of this packet.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-28-storage-review-field-prefix-search.md`
- `docs/features/2026-05-28-storage-review-search.md`
- `docs/features/2026-05-29-selected-folder-descendant-focus.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible in-memory review and WPF inspection improvement that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Open questions:

- Should descendant focus eventually preserve the active rating/category filters when invoked from a future summary bucket shortcut?
- Should descendant bucket shortcuts be added after a visible layout pass?

Rejected ideas buffer:

- Do not build a full tree view before proving this smaller recursive focus action.
- Do not preserve active filters during `Show descendants`; hidden descendants would make the action feel broken.
- Do not make descendant focus imply cleanup safety, storage savings, or shortlist approval.

### 2026-05-29: Add Matched Review Mix

Status: completed

Evidence:

- Selected Folder Descendant Focus and field-prefixed search make current review lenses more important.
- The whole-scan Review Mix does not explain the currently matched rows after `under:`, `parent:`, category, size, or access lenses are active.
- A read-only summary line can improve triage without adding cleanup execution or more buttons.

Implementation:

- Added WPF Matched Review Mix under Filter Summary.
- Matched Review Mix counts the full active matched set, not only the visible display window.
- The readout includes rows, Likely safe, Caution, High risk, Quarantine candidates, Protected, Access issues, and No category.
- The readout recomputes through the existing filter-summary refresh path and states that it is review context, not cleanup approval.
- Kept real-profile cleanup execution, Quarantine execution changes, Undo changes, permanent deletion, cleanup history, classification changes, and bucket shortcut buttons out of this packet.

Verification:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, ran `git diff --check`, and reported that no real user files were scanned or modified.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-matched-review-mix.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible read-only UI summary that does not change persistence, cleanup execution, recovery rules, or durable architecture.

Open questions:

- Should future bucket shortcuts be added from Matched Review Mix after visible layout review?
- Should Matched Review Mix eventually include largest-row sizes per bucket?

Rejected ideas buffer:

- Do not add clickable bucket shortcuts in this packet.
- Do not show matched byte sums as savings.
- Do not use Matched Review Mix to auto-shortlist or recommend cleanup.

### 2026-05-29: Debounce Storage Review Search typing

Status: completed

Evidence:

- User tested the real `C:\Users\moxhe` scan and reported sluggish typing in the Search box.
- Real-profile searches such as `cache`, `chrome`, and `user data` matched tens or hundreds of thousands of rows.
- The WPF `SearchBox_TextChanged` path refreshed the large result grid synchronously on every keystroke.
- User retested after the debounce change and reported that Search typing feels much better.
- User confirmed the status-bar message is enough; no separate visible Search pending indicator is needed right now.

Implementation:

- Added a 350 ms debounce for user-typed Storage Review Search text.
- Kept direct/programmatic search application immediate for tests, review shortcuts, and selected-folder focus flows.
- Reused the already computed matched result set when updating the post-scan filter summary, avoiding one duplicate large-filter pass.
- Preserved read-only behavior; no scan, cleanup, quarantine, restore, or classification behavior changed.

Verification:

- `dotnet build WindowsFileCleaner.sln` passed with 0 warnings and 0 errors after the running app was closed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.

Docs updated:

- `docs/domain/context.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible UI responsiveness change with no persistence, cleanup execution, security, or data-model impact.

Open questions:

- None for this packet.

Rejected ideas buffer:

- Do not reduce search coverage just to improve speed.
- Do not make search require pressing Enter unless real-profile typing remains sluggish after debounce.
- Do not add a separate visible Search pending indicator unless future testing shows the status bar is insufficient.

### 2026-05-29: Add fresh-thread handoff note

Status: completed

Evidence:

- User said the current thread is getting slow and asked for a handoff polish pass plus a prompt for a new thread.
- Repo was clean on `main` tracking `origin/main` before the handoff packet.
- Latest pushed functional packet was `ce7c1db Debounce storage review search`, with GitHub Actions MVP Preflight passing.

Implementation:

- Added `docs/codex/thread-handoff.md` with current state, safety boundary, recent verification, best next work, commands, and a startup prompt.
- Linked the handoff note from `README.md`.
- Updated the progress log current status and next recommended work to point new threads at the handoff note.
- Added README wording that user-typed Storage Review Search is debounced and uses the status bar as the pending-search indicator.

Verification:

- Docs-only packet; no production code changed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is handoff documentation only.

Open questions:

- None.

Rejected ideas buffer:

- Do not hide the real-profile cleanup execution gaps in the handoff prompt.
- Do not ask the next thread to continue from memory; require it to inspect current files and git state.

### 2026-05-29: Add Review Shortlist Safety Mix

Status: completed

Evidence:

- User asked this thread to prefer Review Shortlist safety context before any real cleanup execution.
- Review Shortlist is already an in-memory review aid, and Quarantine Preview remains the separate dry-run readiness step.
- WPF app smoke tests already cover shortlist actions, making them the narrowest relevant verification surface.

Implementation:

- Added WPF `ShortlistSafetyMixText` beneath the Review Shortlist/Quarantine toolbar.
- The readout summarizes shortlisted rows by Likely safe, Caution, High risk, Quarantine candidates, Protected, Access issues, No category, and largest shortlisted row.
- Empty, removed, and cleared shortlist states return to empty wording.
- Kept all behavior read-only and derived only from completed Storage Scan review rows.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, persisted cleanup history, and cleanup eligibility unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-shortlist-safety-mix.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible read-only UI context improvement with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later layout pass convert Review Shortlist Safety Mix into compact chips or a table if the visible line feels too dense?

Rejected ideas buffer:

- Do not treat Shortlist Safety Mix as Quarantine Preview readiness.
- Do not show shortlisted row sizes as confirmed storage savings.

### 2026-05-29: Add Scan Gate Discoverability Polish

Status: completed

Evidence:

- The real-profile scan gate was functionally correct, but its visible locked/ready state relied on quiet checkbox and gray text.
- Handoff guidance lists scan-gate discoverability as preferred safety work before real cleanup execution.
- WPF startup tests already cover real-profile and fixture gate states.

Implementation:

- Added a visible `ScanGateSummaryText` line in the WPF header.
- Added `Scan` button tooltip wording for locked real-profile, acknowledged real-profile, fixture, and custom/invalid states.
- Kept the existing real-profile acknowledgement requirement, scan-start enforcement, and fixture/custom behavior unchanged.
- Kept all behavior read-only; no scan was run against the real profile and no cleanup execution was added.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/features/2026-05-29-scan-gate-discoverability-polish.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible WPF wording/discoverability improvement with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the extra header line fit comfortably during the next visible fixture and real-profile review pass?

Rejected ideas buffer:

- Do not replace the acknowledgement checkbox with a modal until manual review proves the inline gate is still easy to miss.
- Do not run preflight from the WPF app as part of scan-gate polish.

### 2026-05-29: Add Quarantine Execution Scope Status

Status: completed

Evidence:

- Handoff guidance recommends Quarantine Preview/readiness clarity before any real cleanup execution.
- Existing WPF tests proved custom non-fixture execution stayed unavailable, but visible wording leaned on technical `Execution implemented: no` text.
- Fixture and custom Quarantine Preview/Gate flows already have narrow WPF smoke coverage.

Implementation:

- Added plain-language `Execution scope status` lines to Quarantine Preview output.
- Added the same scope-status wording to Quarantine Execution Gate output.
- Fixture scopes now state fixture-only execution is available only after preview readiness and exact `QUARANTINE` confirmation.
- Custom and real-profile-style non-fixture scopes state the workflow is preview-only and real-profile/custom execution remains unavailable.
- Kept existing `Execution implemented` lines for continuity and did not change eligibility, confirmation, execution, restore, or persistence behavior.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-execution-scope-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the extra line make the Quarantine Preview/Gate panes easier to scan during the next visible fixture review?

Rejected ideas buffer:

- Do not replace the technical `Execution implemented` line before manual review confirms the new wording is sufficient.
- Do not use scope-status wording as permission to enable real-profile execution.

### 2026-05-29: Add Fixture Review Checklist Output

Status: completed

Evidence:

- The next recommended work remains a visible fixture review pass.
- `Start-MvpFixtureReview.ps1` already launched the safe fixture workflow, but the detailed review checklist lived in README rather than the terminal where the user starts the pass.
- Recent packets added scan-gate, shortlist, and execution-scope wording that should be checked together during fixture review.

Implementation:

- Added a compact manual fixture review checklist to `tools/Start-MvpFixtureReview.ps1`.
- The checklist covers fixture scope wording, manual Scan, no-files-modified status, review summaries, search/focus actions, Review Shortlist Safety Mix, Quarantine Preview, Quarantine Execution Scope Status, fixture-only execution/undo, manifest review, and real/custom blockers.
- Added `-SkipChecklist` for focused loops.
- Kept the launcher from scanning automatically, scanning real-profile files, or modifying anything during `-WhatIf`.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and printed the checklist without creating fixture files or launching WPF.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch -SkipChecklist` passed and omitted the checklist for focused loops.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a local workflow-output improvement with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- After the next visible fixture review, should the checklist be shortened or split into grouped prompts?

Rejected ideas buffer:

- Do not make the launcher click Scan automatically.
- Do not replace the full README checklist with only the compact launcher checklist.

### 2026-05-29: Add Fixture Review Checklist-Only Mode

Status: completed

Evidence:

- The next recommended work remains a visible fixture review pass.
- The compact launcher checklist is useful as a terminal prompt, but the user may want to print it without running preflight, creating fixture files, or launching WPF.
- The fixture launcher already resolves and validates the fixture root before doing work.

Implementation:

- Added `-ChecklistOnly` to `tools/Start-MvpFixtureReview.ps1`.
- Checklist-only mode prints the resolved Fixture Cleanup Scope and manual review checklist, then exits before preflight, fixture creation, or WPF launch.
- Kept the existing repository containment guard for the fixture root.
- Kept real-profile scanning and cleanup execution unchanged.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly -FixtureRoot ".local\storage-scan-smoke-fixture"` passed and resolved the fixture path inside the repo.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the checklist in dry-run launcher output.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch -SkipChecklist` passed and omitted the checklist for focused loops.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-review-checklist-only-mode.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a local workflow-output option with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- After the next visible fixture review, should the checklist be shortened or split into grouped prompts?

Rejected ideas buffer:

- Do not make checklist-only mode create fixture files.
- Do not make the launcher click Scan automatically.

### 2026-05-29: Add Quarantine Approval Boundary Wording

Status: completed

Evidence:

- Handoff guidance recommends Quarantine Preview/readiness clarity before real cleanup execution.
- Existing preview/gate panes showed execution scope status, but the approval boundary was spread across surrounding wording.
- WPF smoke tests already exercise fixture and custom non-fixture Quarantine Preview/Gate panes.

Implementation:

- Added an `Approval boundary:` line to Quarantine Preview output.
- Added the same approval-boundary line to Quarantine Execution Gate output.
- Fixture wording says Review Shortlist and Quarantine Preview are not cleanup approval and exact `QUARANTINE` can open only fixture execution in this build.
- Preview-only wording says Review Shortlist and Quarantine Preview are not cleanup approval and real-profile/custom execution remains unavailable.
- Kept real-profile Quarantine execution, custom execution, restore behavior, permanent deletion, cleanup history, and eligibility rules unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-quarantine-approval-boundary-wording.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the extra line make the preview/gate panes easier to scan during the next visible fixture review?

Rejected ideas buffer:

- Do not use approval-boundary wording as permission to enable real-profile execution.
- Do not remove the existing execution scope status line before manual review.

### 2026-05-29: Add Fixture Checklist Approval Boundary

Status: completed

Evidence:

- The latest WPF Quarantine Preview/Gate panes now show `Approval boundary:` wording.
- The fixture launcher checklist still told the user to check Execution scope status, but not the new approval-boundary line.
- The next recommended work remains manual fixture review.

Implementation:

- Updated `tools/Start-MvpFixtureReview.ps1` checklist line 6 to mention Approval boundary plus Execution scope status.
- Updated README and feature notes so fixture-review prompts match the latest WPF panes.
- Kept real-profile scanning, fixture creation, WPF launch, Quarantine Preview, execution, restore, and cleanup eligibility behavior unchanged.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-checklist-approval-boundary.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local checklist wording with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- After visible fixture review, should the checklist be shortened or split into grouped prompts?

Rejected ideas buffer:

- Do not add more checklist items for every WPF line; keep the terminal prompt compact.

### 2026-05-29: Add Confirmation Label Wording

Status: completed

Evidence:

- Fixture-only Quarantine execution and fixture-only selected restore now exist.
- WPF Quarantine Preview and Selected Restore panes still said `Required future text`, which was stale and could make current fixture-only gates harder to understand.
- Existing WPF smoke tests already cover Quarantine Preview/Gate and Selected Restore Gate wording.

Implementation:

- Changed WPF Quarantine Confirmation Draft output to `Required confirmation text: QUARANTINE`.
- Changed WPF Selected Restore Confirmation Draft output to `Required confirmation text: RESTORE`.
- Added WPF smoke assertions that the stale `Required future text` label is absent from those panes.
- Updated current docs and test wording to use required confirmation text.
- Kept confirmation phrases, execution gates, fixture-only execution, real-profile blockers, restore behavior, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.
- Initial parallel core test attempts hit transient Windows build-output file locks while another .NET command was building; sequential verification passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0014-use-read-only-selected-restore-confirmation-gate.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`
- `docs/features/2026-05-28-quarantine-readiness-ui.md`
- `docs/features/2026-05-29-confirmation-label-wording.md`
- `docs/features/2026-05-29-quarantine-execution-gate.md`
- `docs/features/2026-05-29-selected-restore-confirmation-gate.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible wording alignment with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- None for this packet.

Rejected ideas buffer:

- Do not rename model properties or change gate semantics for a visible-label polish packet.

### 2026-05-29: Add Selected Restore Scope Status

Status: completed

Evidence:

- Fixture-only selected restore execution exists, but the Selected Restore Execution Gate pane relied on technical `Execution implemented` wording to explain fixture-only versus preview-only behavior.
- Quarantine Preview/Gate panes already show `Execution scope status` and `Approval boundary` lines, which made selected restore the remaining similar safety-gate wording gap.
- Existing WPF smoke tests cover fixture selected restore and custom non-fixture selected restore blocking.

Implementation:

- Added `Execution scope status` and `Approval boundary` lines to Selected Restore Execution Gate output.
- Fixture wording says fixture-only selected restore is available only after selected readiness and exact `RESTORE` confirmation.
- Preview-only wording says real-profile and custom selected restore remain unavailable.
- Kept selected restore gate behavior, fixture-only execution, real-profile blockers, custom blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-restore-scope-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the extra selected restore gate wording make the pane easier to scan during the next visible fixture review?

Rejected ideas buffer:

- Do not use selected restore scope-status wording as permission to enable real-profile selected restore.
- Do not remove the technical `Execution implemented` line before manual review confirms the clearer wording is sufficient.

### 2026-05-29: Add Fixture Checklist Selected Restore Scope

Status: completed

Evidence:

- The latest WPF Selected Restore Execution Gate panes now show `Execution scope status` and `Approval boundary` wording.
- The fixture launcher checklist still referred to selected readiness/gate generically and did not tell the user to check the new selected restore scope-status line.
- The next recommended work remains manual fixture review.

Implementation:

- Updated `tools/Start-MvpFixtureReview.ps1` checklist line 8 to call out selected restore `Approval boundary` plus `Execution scope status`.
- Updated checklist line 9 to explicitly name both Quarantine and selected restore execution blockers for real-profile/custom scopes.
- Updated README and feature notes so fixture-review prompts match the latest WPF selected restore pane.
- Kept real-profile scanning, fixture creation, WPF launch, Quarantine execution, selected restore execution, permanent deletion, and cleanup history behavior unchanged.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-selected-restore-scope-status.md`
- `docs/features/2026-05-29-fixture-checklist-selected-restore-scope.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local checklist wording with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- After visible fixture review, should selected restore scope-status wording stay in the compact checklist or move only to README?

Rejected ideas buffer:

- Do not add a separate checklist item for every selected restore line; keep the terminal prompt compact.

### 2026-05-29: Add Broad Restore Action Wording

Status: completed

Evidence:

- Fixture-only selected restore execution now exists for selected discovered fixture manifests.
- Quarantine Manifest Discovery, Selected Restore Manifest Review, and Restore Readiness Preview still used generic `No restore action is available` wording, which could be read as contradicting fixture-only selected restore.
- Existing WPF smoke tests cover discovery, selected review, readiness preview, selected restore gate, and fixture-only selected restore execution.

Implementation:

- Updated Quarantine Manifest Discovery output to say no broad restore action is available from discovery and fixture selected restore must go through selected readiness and the selected restore gate.
- Updated Restore Readiness Preview output with the same broad-restore boundary.
- Updated Selected Restore Manifest Review output to say selected review is readiness evidence only and fixture selected restore must go through the selected restore gate.
- Kept discovery, readiness preview, selected review, selected restore gate behavior, fixture-only execution, real-profile blockers, custom blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-broad-restore-action-wording.md`
- `docs/features/2026-05-29-quarantine-manifest-discovery.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `docs/features/2026-05-29-selected-restore-manifest-review.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does `broad restore action` read clearly during visible fixture review?

Rejected ideas buffer:

- Do not use broad-restore wording as permission to add broad WPF Undo Quarantine.

### 2026-05-29: Add Fixture Checklist Broad Restore Wording

Status: completed

Evidence:

- The latest WPF discovery/readiness/selected-review panes now distinguish no broad restore action from fixture-only selected restore.
- The fixture launcher checklist still told the user to use discovery/readiness/gate but did not explicitly tell them to check the no-broad-restore wording.
- The next recommended work remains manual fixture review.

Implementation:

- Updated `tools/Start-MvpFixtureReview.ps1` checklist line 8 to call out no broad restore action plus selected restore Approval boundary and Execution scope status.
- Updated README and feature notes so fixture-review prompts match the latest WPF restore panes.
- Kept real-profile scanning, fixture creation, WPF launch, Quarantine execution, selected restore execution, permanent deletion, and cleanup history behavior unchanged.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-broad-restore-action-wording.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-fixture-checklist-broad-restore-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local checklist wording with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Is `no broad restore action` clear enough in the compact checklist?

Rejected ideas buffer:

- Do not add another checklist line for every restore pane; keep the terminal prompt compact until visible review proves it needs splitting.

### 2026-05-29: Add All-Manifest Restore Wording

Status: completed

Evidence:

- The latest WPF discovery/readiness panes used `broad restore action` to distinguish all-manifest restore from fixture selected restore.
- That wording was accurate but internal; `all-manifest restore action` names the unavailable action more concretely.
- Existing WPF smoke tests cover discovery and readiness wording.

Implementation:

- Updated Quarantine Manifest Discovery and Restore Readiness Preview output to say no all-manifest restore action is available.
- Updated WPF smoke assertions for the new wording.
- Updated the fixture launcher checklist to use no all-manifest restore action wording.
- Updated README, domain docs, related feature notes, and handoff.
- Kept discovery/readiness preview behavior, selected restore gate behavior, fixture-only execution, real-profile blockers, custom blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the all-manifest restore checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-all-manifest-restore-wording.md`
- `docs/features/2026-05-29-broad-restore-action-wording.md`
- `docs/features/2026-05-29-fixture-checklist-broad-restore-wording.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-quarantine-manifest-discovery.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- If all-manifest restore is ever designed, should the button use `Restore all discovered manifests` or another explicit label?

Rejected ideas buffer:

- Do not add an all-manifest restore domain term before execution design exists.

### 2026-05-29: Add Visible Row Shortlist Labels

Status: completed

Evidence:

- Review Shortlist bulk actions already operate only on the current Storage Review Display Window.
- Existing button/status wording used `shown`, while README/domain language needed to reinforce visible-row scope before any cleanup execution.
- WPF smoke tests already exercise bulk add/remove behavior.

Implementation:

- Renamed WPF bulk shortlist buttons to `Shortlist visible rows` and `Remove visible rows`.
- Updated bulk shortlist status text to say visible rows.
- Updated fixture launcher checklist wording to prompt visible-row shortlist label review.
- Added WPF smoke assertions for the visible-row button labels.
- Updated README, domain docs, related feature notes, and handoff.
- Kept Review Shortlist behavior read-only, in-memory, and limited to displayed rows; no Quarantine Preview, execution, restore, deletion, or cleanup history behavior changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the visible-row shortlist label review prompt without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-visible-row-shortlist-labels.md`
- `docs/features/2026-05-28-shortlist-shown-review-rows.md`
- `docs/features/2026-05-28-storage-review-display-window.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI wording clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later layout pass add icons or tooltips for Review Shortlist bulk actions?

Rejected ideas buffer:

- Do not rename internal method names in this packet.
- Do not make the action apply to all matched rows.

### 2026-05-29: Add Scope-Specific Scan Gate Ready Wording

Status: completed

Evidence:

- Cleanup Scope Scan Gate behavior was correct, but non-real ready wording was generic even though fixture-only cleanup actions exist later behind preview and exact confirmation gates.
- Handoff guidance prefers scan-gate discoverability before any real cleanup execution.
- Core and WPF tests already cover scan-gate states.

Implementation:

- Updated core scan-gate ready messages for acknowledged real-profile, fixture, and custom scopes.
- Updated WPF scan-gate summaries and Scan tooltips with scope-specific execution-boundary wording.
- Added core and WPF smoke assertions for real-profile, fixture, and custom ready states.
- Updated fixture launcher checklist wording to mention gated fixture cleanup actions.
- Kept scan enablement, acknowledgement behavior, Storage Scan, Quarantine Preview, execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the fixture cleanup actions stay gated checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-scope-specific-scan-gate-ready-wording.md`
- `docs/features/2026-05-29-scan-gate-discoverability-polish.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible wording clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the longer scope-specific header wording fit comfortably during visible fixture and real-profile review?

Rejected ideas buffer:

- Do not make fixture scan readiness imply cleanup approval.
- Do not hide fixture-only cleanup execution by saying no cleanup execution exists for fixture scopes.

### 2026-05-29: Add Execution Control Tooltip Clarity

Status: completed

Evidence:

- Selected restore confirmation tooltip still said discovered-manifest restore execution was unavailable even though fixture selected restore now exists after selected readiness and exact `RESTORE`.
- Quarantine execution and undo buttons did not carry their own tooltip boundary text.
- WPF smoke tests already cover startup disabled controls and selected fixture restore flow.

Implementation:

- Updated Quarantine confirmation, Execute quarantine, Undo fixture quarantine, selected restore confirmation, and Restore selected fixture manifest tooltips.
- Enabled disabled-control tooltip display for those execution gate controls.
- Added WPF smoke assertions for startup tooltip wording and enabled fixture selected restore tooltip wording.
- Updated fixture launcher checklist wording to include execution and restore tooltip review.
- Kept Quarantine Preview, Quarantine Execution Gate, Selected Restore Execution Gate, fixture-only execution, restore, real-profile blockers, custom blockers, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed execution/restore tooltip review prompts without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-execution-control-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should future layout polish add inline help icons for execution gates if tooltips are not discoverable enough?

Rejected ideas buffer:

- Do not add new execution paths.
- Do not rely only on pane text when disabled controls can carry their own safety context.

### 2026-05-29: Add Undo Quarantine Domain Consistency

Status: completed

Evidence:

- Current WPF app can undo the current fixture execution and run fixture-only selected restore through `UndoQuarantineExecutor`.
- `docs/domain/context.md` still said Undo Quarantine Executor was fixture-tested but not wired to WPF yet.
- ADR 0010 context also read like current state rather than decision-time state.

Implementation:

- Updated Undo Quarantine Executor current-state wording in domain context.
- Clarified ADR 0010 context as decision-time wording.
- Preserved real-profile WPF Undo Quarantine and all-manifest discovered-manifest restore blockers.
- Left older feature briefs historical when their stale-sounding wording describes packet-start behavior.

Verification:

- `rg -n "not wired to the WPF|not wired to WPF|does not expose undo yet|Keep WPF Undo Quarantine unavailable" docs/domain docs/decisions/0010-use-current-fixture-execution-wpf-undo.md` found no current-state stale wording.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `docs/domain/context.md`
- `docs/decisions/0010-use-current-fixture-execution-wpf-undo.md`
- `docs/features/2026-05-29-undo-quarantine-domain-consistency.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a docs consistency correction, not a new durable decision.

Open questions:

- Should future docs distinguish current-state domain context from historical ADR/feature context more explicitly?

Rejected ideas buffer:

- Do not rewrite old feature briefs that accurately describe packet-start behavior.
- Do not turn this docs correction into a real-profile undo design packet.

### 2026-05-29: Add Restore Manifest Wording Polish

Status: completed

Evidence:

- Current-facing README, WPF tooltip, and WPF smoke assertion messages still used `old manifest` wording.
- The glossary already uses Restore Manifest as the preferred term.
- Handoff guidance prefers readiness clarity before any real cleanup execution.

Implementation:

- Updated the disabled current-fixture undo tooltip to refer to discovered Restore Manifests.
- Updated README manual check wording to say discovery should state that no all-manifest restore action is available.
- Updated WPF smoke assertion messages to use Restore Manifest/all-manifest restore wording.
- Updated domain context and glossary wording for current undo and selected restore boundaries.
- Kept Quarantine Manifest Discovery, Restore Readiness Preview, current-fixture undo, fixture-only selected restore, real-profile blockers, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the all-manifest restore checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-restore-manifest-wording-polish.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible wording clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- If all-manifest restore is designed later, what should the final button label be?

Rejected ideas buffer:

- Do not rename historical ADR rationale wholesale in this packet.
- Do not introduce a new `discovered-manifest restore` term when `Restore Manifest` already exists.

### 2026-05-29: Add Selected Manifest Readiness Label Polish

Status: completed

Evidence:

- The visible WPF action label `Preview selected readiness` was compact but did not name the selected Restore Manifest.
- README, checklist output, domain docs, and tests used the same shorter selected-readiness wording in current-facing guidance.
- Handoff guidance prefers manual fixture/review clarity before any real cleanup execution.

Implementation:

- Renamed the WPF action to `Preview selected manifest readiness` and widened the button.
- Updated placeholder text, discovery/readiness pane wording, selected restore tooltip, and selected restore scope-status text to use selected manifest readiness wording.
- Added WPF smoke assertions for the button label and related pane/tooltip wording.
- Updated core assertion messages and a selection blocker string to use selected manifest readiness wording.
- Updated README, domain docs, fixture checklist, relevant feature notes, and handoff.
- Kept manifest discovery, selected review, restore readiness preview, fixture-only selected restore, real-profile blockers, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the selected manifest readiness/gate checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-manifest-readiness-label-polish.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-29-restore-manifest-wording-polish.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI wording clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the longer button label fit comfortably during visible fixture review?

Rejected ideas buffer:

- Do not rename core types or workflows in this packet.
- Do not use the shorter `selected readiness` label again unless visible review shows the longer label does not fit.

### 2026-05-29: Add All-Manifest Readiness Label Polish

Status: completed

Evidence:

- `Preview restore readiness` was accurate but less explicit after the selected-manifest readiness label became `Preview selected manifest readiness`.
- ADR 0012 and the Restore Readiness Preview feature brief describe this workflow as evaluating discovered Restore Manifests under the selected Quarantine Root.
- Handoff guidance prefers readiness clarity before any real cleanup execution.

Implementation:

- Renamed the WPF Restore Readiness Preview action to `Preview all-manifest readiness`.
- Updated the readiness placeholder text and added WPF smoke assertions for the button label and placeholder.
- Updated README, domain docs, fixture checklist, relevant feature notes, progress, and handoff.
- Kept Restore Readiness Preview read-only and kept all restore execution availability boundaries unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the all-manifest readiness preview checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-all-manifest-readiness-label-polish.md`
- `docs/features/2026-05-29-restore-readiness-preview.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI wording clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the longer all-manifest readiness label fit comfortably during visible fixture review?

Rejected ideas buffer:

- Do not rename the domain concept away from Restore Readiness Preview.
- Do not add all-manifest restore while polishing a read-only readiness label.

### 2026-05-29: Add Readiness Scope Tooltip Clarity

Status: completed

Evidence:

- The readiness button labels now distinguish selected manifest readiness from all-manifest readiness, but the controls did not have dedicated tooltips explaining scope and approval boundaries.
- The selected manifest readiness button starts disabled before discovery, so its boundary wording should remain available via disabled-state tooltip behavior.
- Handoff guidance prefers manual fixture/review clarity before any real cleanup execution.

Implementation:

- Added a tooltip to `Preview all-manifest readiness` that says it is read-only for discovered Restore Manifests under the selected Quarantine Root and restores no files.
- Added a disabled-state tooltip to `Preview selected manifest readiness` that says it reviews the selected Restore Manifest only and is not restore approval.
- Added WPF smoke assertions for both readiness tooltips, including the enabled selected-manifest state after discovery.
- Updated README, domain docs, fixture checklist, progress, and handoff.
- Kept Restore Readiness Preview, Selected Restore Manifest Review, fixture-only selected restore, real-profile blockers, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the readiness scope tooltips checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-readiness-scope-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are the tooltips discoverable enough or should future polish add inline help icons?

Rejected ideas buffer:

- Do not add restore execution or approval semantics while polishing read-only readiness tooltips.

### 2026-05-29: Add Visible-Row Shortlist Tooltip Clarity

Status: completed

Evidence:

- Visible-row shortlist labels made scope clearer, but the controls did not expose their no-file-modified and not-cleanup-approval boundary before use.
- The visible-row label feature left tooltips as an open follow-up.
- Handoff guidance prefers Review Shortlist safety context before any real cleanup execution.

Implementation:

- Added disabled-state WPF tooltips to `Shortlist visible rows` and `Remove visible rows`.
- Added test-facing tooltip accessors and WPF smoke assertions for visible-row scope, not-cleanup-approval wording, and no-file-modified wording.
- Updated README, domain docs, fixture checklist, related feature notes, progress, and handoff.
- Kept Review Shortlist read-only and in-memory; no Quarantine Preview, execution, restore, deletion, or cleanup history behavior changed.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the visible-row shortlist labels/tooltips checklist wording without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-visible-row-shortlist-tooltip-clarity.md`
- `docs/features/2026-05-29-visible-row-shortlist-labels.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are hover tooltips discoverable enough or should a later keyboard-accessibility pass add explicit help/focus text?

Rejected ideas buffer:

- Do not add icons, persistent Review Shortlist behavior, or cleanup approval semantics while polishing visible-row tooltip clarity.

### 2026-05-29: Add Review Toolbar Report and Preview Tooltip Clarity

Status: completed

Evidence:

- The visible-row shortlist controls had safety tooltips, but adjacent report/clear/preview controls in the same toolbar did not expose equivalent pre-use boundaries.
- The toolbar includes actions that write reports, clear in-memory review state, or build dry runs, so the distinction from cleanup approval should be visible before use.
- Handoff guidance prefers manual review polish and Review Shortlist safety context before any real cleanup execution.

Implementation:

- Added disabled-state WPF tooltips to `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview`.
- Added test-facing tooltip accessors and WPF smoke assertions for report-only, in-memory-only, dry-run, and no-file-modified wording.
- Updated README, domain docs, fixture checklist, progress, and handoff.
- Kept Review Shortlist, CSV export, Quarantine Preview, cleanup execution, restore, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed Review Shortlist labels/tooltips plus preview/export tooltip checks without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-toolbar-report-preview-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are toolbar tooltips discoverable enough or should a later keyboard-accessibility pass add focus/help text?

Rejected ideas buffer:

- Do not add always-visible help text, export behavior changes, or cleanup approval semantics while polishing report/preview tooltip clarity.

### 2026-05-29: Add Selection Browse Tooltip Clarity

Status: completed

Evidence:

- Cleanup Scope Selection and Quarantine Root Selection already had safety notes, but the `Browse...` buttons did not expose their path-only or preview-only boundaries before use.
- Cleanup Scope Selection must stay separate from `Scan` and the real-profile scan gate.
- Quarantine Root Selection must stay separate from folder creation, file movement, and cleanup approval.

Implementation:

- Added disabled-state WPF tooltips to Cleanup Scope and Quarantine Root `Browse...` buttons.
- Added test-facing tooltip accessors and WPF smoke assertions for path-only selection, real-profile scan-gate, preview-only, no-folder-creation, and no-approval wording.
- Updated README, domain docs, fixture checklist, progress, and handoff.
- Kept browsing, Storage Scan, Quarantine Preview, cleanup execution, restore, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed Cleanup Scope and Quarantine Root browse tooltip checks without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selection-browse-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are browse tooltips discoverable enough or should a later keyboard-accessibility pass add focus/help text?

Rejected ideas buffer:

- Do not add modal warnings, Quarantine Root folder creation/probing, or scan-gate changes while polishing browse tooltip clarity.

### 2026-05-29: Add Selected-Row Action Tooltip Clarity

Status: completed

Evidence:

- Recent packets clarified the main Review Shortlist toolbar, report/preview controls, and browse controls.
- The selected-row action strip still lacked comparable pre-use boundary wording for shortlist, focus, preview, copy, and Explorer inspection actions.
- Domain docs already define these actions as read-only, selected-only, or inspection-only.

Implementation:

- Added disabled-state WPF tooltips to selected-row shortlist, copy, focus, preview, and Explorer actions.
- Added test-facing tooltip accessors and WPF smoke assertions for selected-only, read-only focus, bounded preview, manual inspection, no-file-modified, and not-cleanup-approval wording.
- Updated README, domain docs, fixture checklist, progress, and handoff.
- Kept selected-row action behavior, Storage Scan, Review Shortlist, Quarantine Preview, cleanup execution, restore, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the selected-row tooltip checklist item without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-row-action-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are selected-row action tooltips discoverable enough or should a later keyboard-accessibility pass add focus/help text?

Rejected ideas buffer:

- Do not add always-visible helper text, focus behavior changes, or cleanup approval semantics while polishing selected-row action tooltips.

### 2026-05-29: Add Review Navigation and Export Tooltip Clarity

Status: completed

Evidence:

- Recent packets clarified selected-row actions, browse controls, and Review Shortlist/report/preview toolbar controls.
- The main review toolbar still had read-only navigation/export controls without pre-use tooltip boundaries.
- Domain docs already define these controls as read-only, in-memory, or report-only.

Implementation:

- Added disabled-state WPF tooltips to `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows`.
- Added test-facing tooltip accessors and WPF smoke assertions for report-only, no-rescan, in-memory display-window, Review Shortlist-preserving, no-file-modified, and not-cleanup-approval wording.
- Updated README, domain docs, fixture checklist, progress, and handoff.
- Kept Storage Review Search, Review View Reset, Storage Review Display Window, Scan Report Export, cleanup execution, restore, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the review navigation/export tooltip checklist item without preflight, fixture creation, or WPF launch.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-navigation-export-tooltip-clarity.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI tooltip clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- During visible fixture review, are review navigation/export tooltips discoverable enough or should a later keyboard-accessibility pass add focus/help text?

Rejected ideas buffer:

- Do not change export row selection, debounce indicators, display-window size, or cleanup approval semantics while polishing review navigation/export tooltips.

### 2026-05-29: Add Review Toolbar Automation Help Text

Status: completed

Evidence:

- Review Navigation and Export Tooltip Clarity made the main review toolbar boundaries visible by hover tooltip.
- The next small polish gap was non-hover discoverability for the same safety boundaries.
- Domain docs already define `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows` as read-only, in-memory, or report-only controls.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows`.
- Added test-facing help-text accessors and WPF smoke assertions for report-only, no-rescan, in-memory display-window, Review Shortlist-preserving, no-file-modified, and not-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Storage Review Search, Review View Reset, Storage Review Display Window, Scan Report Export, cleanup execution, restore, deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-toolbar-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later accessibility pass add automation help text to the remaining safety-critical tooltiped controls?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, change enablement, or broaden this packet to every tooltiped control while proving the review-toolbar help-text pattern.

### 2026-05-29: Add Review Report and Preview Automation Help Text

Status: completed

Evidence:

- Review Toolbar Report and Preview Tooltip Clarity made `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview` boundaries visible by hover tooltip.
- The previous automation-help-text packet proved the same non-hover pattern on the main review navigation/export controls.
- Domain docs already define Review Shortlist as in-memory review context and Quarantine Preview as a dry run.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview`.
- Added test-facing help-text accessors and WPF smoke assertions for report-only, in-memory-only, dry-run, no-file-modified, and not-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Review Shortlist behavior, Quarantine Preview eligibility/output, CSV export behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-report-preview-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later accessibility pass add automation help text to selected-row, browse, execution-gate, and restore-readiness controls?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, change enablement, or broaden this packet beyond report/preview controls while proving the help-text pattern.

### 2026-05-29: Add Selected Row Action Automation Help Text

Status: completed

Evidence:

- Selected Row Action Tooltip Clarity made selected-row action boundaries visible by hover tooltip.
- Recent automation-help-text packets proved the same non-hover pattern on main review toolbar and report/preview controls.
- Domain docs already define selected-row actions as review-only, focus-only, inspection-only, or bounded read-only preview.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to `Add to shortlist`, `Remove`, `Copy path`, `Show children`, `Show descendants`, `Preview file`, and `Open in Explorer`.
- Added test-facing help-text accessors and WPF smoke assertions for selected-only, read-only focus, bounded preview, manual inspection, no-file-modified, and not-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept selected-row actions, Review Shortlist behavior, focus/search behavior, file preview behavior, clipboard/Explorer behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selected-row-action-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later accessibility pass add automation help text to browse, execution-gate, and restore-readiness controls?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, change enablement, or broaden this packet beyond selected-row controls while proving the help-text pattern.

### 2026-05-29: Add Selection Browse Automation Help Text

Status: completed

Evidence:

- Selection Browse Tooltip Clarity made Cleanup Scope and Quarantine Root browse boundaries visible by hover tooltip.
- Recent automation-help-text packets proved the same non-hover pattern on review toolbar, report/preview, and selected-row controls.
- Domain docs already define Cleanup Scope browsing as path selection only and Quarantine Root browsing as preview-root selection only.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to Cleanup Scope and Quarantine Root `Browse...` controls.
- Added test-facing help-text accessors and WPF smoke assertions for path-only, preview-only, no-scan, no-folder-creation, no-file-modified, scan-gate, and no-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Cleanup Scope Selection behavior, Quarantine Root Selection behavior, scan gating, preview-root behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed; Git printed line-ending normalization warnings for touched files but no whitespace errors.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-selection-browse-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later accessibility pass add automation help text to execution-gate and restore-readiness controls?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, change enablement, weaken scan gates, probe folders, or broaden this packet beyond browse controls while proving the help-text pattern.

### 2026-05-29: Add Execution and Readiness Automation Help Text

Status: completed

Evidence:

- Execution Control Tooltip Clarity and Readiness Scope Tooltip Clarity made gate/readiness boundaries visible by hover tooltip.
- Recent automation-help-text packets proved the same non-hover pattern on review toolbar, report/preview, selected-row, and browse controls.
- Domain docs already define Quarantine execution, current-fixture undo, Restore Readiness Preview, Selected Restore Manifest Review, and Selected Restore Execution Gate boundaries.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to Quarantine confirmation, Execute quarantine, Undo fixture quarantine, Preview all-manifest readiness, Preview selected manifest readiness, selected restore confirmation, and Restore selected fixture manifest controls.
- Added a tooltip and matching automation help text to `Preview selected restore gate`.
- Added test-facing help-text accessors and WPF smoke assertions for fixture-only, read-only, selected-only, exact-confirmation, no-restore, and real-profile/custom blocker wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Quarantine Preview, Quarantine execution availability, current-fixture undo, manifest discovery, restore readiness, selected restore gate behavior, fixture-only selected restore, real-profile/custom blockers, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-execution-readiness-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/tooltip polish with no persistence, cleanup execution, restore semantics, security, deployment, or data-model change.

Open questions:

- Should visible fixture review lead to always-visible help icons for safety-critical gates, or are tooltip plus automation help text enough?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, change enablement, enable real-profile movement, or broaden this packet beyond execution/readiness controls while proving the help-text pattern.

### 2026-05-29: Add Visible Row Shortlist Automation Help Text

Status: completed

Evidence:

- Visible Row Shortlist Tooltip Clarity made the bulk Review Shortlist boundaries visible by hover tooltip.
- Domain docs already require visible-row control tooltips and automation help text to keep row/display-window scope, no-file-modified behavior, and not-cleanup-approval boundaries available.
- Recent automation-help-text packets proved the same non-hover pattern on nearby Review Shortlist report/preview controls.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to `Shortlist visible rows` and `Remove visible rows`.
- Added test-facing help-text accessors and WPF smoke assertions for visible-row scope, review-only/not-cleanup-approval wording, and no-file-modified behavior.
- Updated README, progress, handoff, and the feature brief.
- Kept Review Shortlist behavior, display-window scope, Quarantine Preview eligibility, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-visible-row-shortlist-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should visible fixture review lead to always-visible help for visible-row bulk actions, or are tooltip plus automation help text enough?

Rejected ideas buffer:

- Do not add visible helper text, alter focus order, persist Review Shortlist, change display-window scope, enable real-profile movement, or broaden this packet beyond visible-row bulk controls.

### 2026-05-29: Add Scan Gate Automation Help Text

Status: completed

Evidence:

- Scan Gate Discoverability Polish and Scope-Specific Scan Gate Ready Wording made the `Scan` tooltip dynamic and scope-specific.
- The `Scan` button did not mirror that dynamic wording into WPF automation help text.
- The disabled real-profile `Scan` tooltip should remain discoverable before acknowledgement.

Implementation:

- Added disabled-state tooltip support to `Scan`.
- Reused the dynamic `FormatScanButtonToolTip` output as `Scan` automation help text.
- Added WPF smoke assertions for locked real-profile, acknowledged real-profile, fixture, and custom Scan automation help text.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept scan enablement, acknowledgement behavior, Storage Scan behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-scan-gate-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/tooltip polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the scan-gate header and tooltip wording need tightening after visible fixture/real-profile review?

Rejected ideas buffer:

- Do not change scan enablement, preflight acknowledgement semantics, or cleanup execution availability while polishing `Scan` help text.

### 2026-05-29: Add Quarantine Root Input Automation Help Text

Status: completed

Evidence:

- Quarantine Root Selection can be typed or browsed, and the browse action already had tooltip plus automation help text.
- The typed Quarantine Root field had a preview-only tooltip but no automation help text.
- Domain docs require Quarantine Root Selection to stay preview-only and separate from folder creation, file movement, and cleanup approval.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to the Quarantine Root text box.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for preview-only, no-folder-created, and no-file-moved wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Quarantine Root Selection behavior, Quarantine Root Safety Note, preview gating, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-root-input-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does visible fixture review show that the Quarantine Root toolbar and safety note need tightening?

Rejected ideas buffer:

- Do not create, probe, persist, or approve Quarantine Root paths while polishing typed-root help text.

### 2026-05-29: Add Cleanup Scope Input Automation Help Text

Status: completed

Evidence:

- Cleanup Scope Selection can be typed or browsed, and the browse action already had tooltip plus automation help text.
- The typed Cleanup Scope field had no tooltip or automation help text.
- Domain docs require Cleanup Scope Selection to stay path-only, separate from scanning, and unable to bypass the real-profile scan gate or approve cleanup.

Implementation:

- Added WPF tooltip and `AutomationProperties.HelpText` to `ScopePathBox`.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for path-only selection, no auto-scan, no real-profile gate bypass, and no cleanup approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Cleanup Scope Selection behavior, Cleanup Scope Safety Note, Cleanup Scope Scan Gate, Storage Scan behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-cleanup-scope-input-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does visible fixture/real-profile review show that the header needs tightening?

Rejected ideas buffer:

- Do not start scans, bypass acknowledgement, or approve cleanup while polishing typed Cleanup Scope help text.

### 2026-05-29: Add Storage Review Search Input Automation Help Text

Status: completed

Evidence:

- User confirmed debounced Storage Review Search is better on large real-profile results and the status-bar pending-search message is enough.
- The Storage Review Search input already had a tooltip with prefix examples, but no matching WPF automation help text.
- Domain docs require Storage Review Search to remain in-memory, read-only, and separate from cleanup approval.

Implementation:

- Added matching WPF `AutomationProperties.HelpText` to the Storage Review Search text box.
- Added test-facing automation help text accessor and WPF smoke assertions for prefix examples, no-rescan, no-file-modified, and no-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept search matching, debounce behavior, status-bar pending-search wording, filters, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-storage-review-search-input-automation-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does visible fixture/real-profile review show that the search input needs always-visible help, or are tooltip plus automation help text enough?

Rejected ideas buffer:

- Do not add another pending-search indicator, alter debounce timing, change search matching, or treat search text as cleanup approval while polishing search input help text.

### 2026-05-29: Add Review Lens Filter Help Text

Status: completed

Evidence:

- Review filters are central to manual fixture and real-profile review.
- Neighboring review controls already had tooltip and automation help text, but Storage Review Filter buttons plus Type, Size, and Category filters did not.
- Domain docs require filters to stay read-only, in-memory, and separate from cleanup approval.

Implementation:

- Added WPF tooltips and `AutomationProperties.HelpText` to Storage Review Filter buttons.
- Added WPF tooltips and `AutomationProperties.HelpText` to Type, Size, and Category filter combo boxes.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for review-only, no-rescan, no-file-modified, no-permission-change, Storage Savings, and not-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept filter behavior, search debounce, scan behavior, exports, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-lens-filter-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Do visible fixture/real-profile reviews show that filter controls need always-visible help icons, or are tooltip plus automation help text enough?

Rejected ideas buffer:

- Do not change filter behavior, add new filters, alter export selection, retry access issue paths, or treat filter state as cleanup approval while polishing review lens help text.

### 2026-05-29: Add Safety Summary Shortcut Help Text

Status: completed

Evidence:

- Safety Summary shortcuts are central to manual fixture and real-profile safety review.
- Neighboring review controls already had tooltip and automation help text, but the Safety Summary shortcut buttons did not.
- Domain docs require Safety Summary shortcuts to stay read-only and separate from rescans, permission changes, link following, file changes, and cleanup approval.

Implementation:

- Added disabled-state WPF tooltips and `AutomationProperties.HelpText` to Safety Summary shortcut buttons.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for read-only shortcut scope, no-rescan, no-file-modified, no-permission-change, no-link-following, and not-cleanup/Quarantine-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept shortcut mapping, filter behavior, scan behavior, exports, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-safety-summary-shortcut-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Do visible fixture/real-profile reviews show that Safety Summary shortcuts need always-visible help icons or stronger grouping?

Rejected ideas buffer:

- Do not change shortcut mapping, add shortcuts, retry access issue paths, follow reparse points, or treat shortcut state as cleanup approval while polishing Safety Summary shortcut help text.

### 2026-05-29: Add Real Profile Acknowledgement Help Text

Status: completed

Evidence:

- User confirmed the real-profile scan gate checkbox enabled `Scan` after acknowledgement.
- The `Scan` button mirrored scan-gate wording through tooltip and automation help text, but the acknowledgement checkbox itself did not.
- Domain docs require the real-profile acknowledgement to stay local, in-memory, and separate from running preflight, creating fixtures, starting scans by itself, persistence, or cleanup approval.

Implementation:

- Added WPF tooltip and `AutomationProperties.HelpText` to the real-profile preflight/fixture-review acknowledgement checkbox.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for MVP preflight, fixture review, no-preflight-run, no-auto-scan, and no-cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept Cleanup Scope Scan Gate behavior, acknowledgement reset behavior, Storage Scan behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-real-profile-acknowledgement-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does visible real-profile review show that the scan-gate header needs shorter text or always-visible help?

Rejected ideas buffer:

- Do not run preflight, create fixtures, auto-start scans, persist acknowledgement, or treat acknowledgement as cleanup approval while polishing the real-profile acknowledgement help text.

### 2026-05-29: Add Manifest Discovery and Selection Help Text

Status: completed

Evidence:

- Quarantine Manifest Discovery and Restore Manifest selection are safety-sensitive controls that can lead toward restore review.
- Neighboring readiness and execution controls already had tooltip and automation help text, but `Discover manifests` and the selection combo box did not.
- Domain docs require discovery to remain read-only and selection to remain separate from restore approval.

Implementation:

- Added disabled-state WPF tooltips and `AutomationProperties.HelpText` to `Discover manifests` and the Restore Manifest selection combo box.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for read-only discovery, no restore/move/delete/folder-cleanup/history, selection not approval, and no manifest writes.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept discovery behavior, selection behavior, readiness previews, restore gates, fixture-only restore, real-profile blockers, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-manifest-discovery-selection-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Do visible fixture/real-profile reviews show that manifest discovery and selection need always-visible help icons?

Rejected ideas buffer:

- Do not add restore behavior, all-manifest restore, cleanup history, manifest writes, or quarantine-folder cleanup while polishing discovery/selection help text.

### 2026-05-29: Add Scan Cancel Help Text

Status: completed

Evidence:

- `Cancel` is the only header scan-control action that had no tooltip or automation help text.
- Long real-profile scans make cancellation discoverability part of the manual review safety story.
- Existing code already reports canceled scans as no-file-modified and cancellation does not invoke cleanup, restore, or deletion.

Implementation:

- Added disabled-state WPF tooltip and `AutomationProperties.HelpText` to `Cancel`.
- Added test-facing tooltip/help-text accessors and WPF smoke assertions for in-progress read-only Storage Scan cancellation and no move/delete/quarantine/restore/cleanup-approval wording.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept scan cancellation behavior, scan gate behavior, Storage Scan results, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-scan-cancel-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does visible fixture/real-profile review show that `Cancel` needs a visible scanning-state note beyond tooltip/help text?

Rejected ideas buffer:

- Do not change cancellation semantics, partial-result handling, scan gates, cleanup execution, undo, restore, or deletion while polishing scan cancel help text.

### 2026-05-29: Add Manual Fixture Show Children and Clipboard Fix

Status: completed

Evidence:

- During manual fixture review, the user confirmed `Show children` worked but did not make the way back to all rows obvious.
- The app then crashed during the same review.
- Windows Application logs showed the crash was an unhandled `System.Runtime.InteropServices.COMException (0x800401D0): OpenClipboard Failed` in `CopyPathButton_Click`.

Implementation:

- Updated `Show children` status text to say `Reset view` returns to all rows.
- Wrapped `Clipboard.SetText` for `Copy path` in a `COMException` handler that shows a warning/status instead of crashing.
- Added WPF smoke coverage for the `Reset view` way-back wording after selected-folder child focus.
- Updated README, domain docs, progress, handoff, and the feature brief.
- Kept selected-folder focus semantics, Review Shortlist, scan behavior, cleanup execution, restore, deletion, and cleanup history unchanged.

Verification:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-manual-fixture-show-children-clipboard-fix.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a bounded manual fixture bugfix with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Is naming `Reset view` in status text enough, or should the UI add a dedicated way-back button near selected-folder focus?

Rejected ideas buffer:

- Do not add a navigation stack, change `Show children` search semantics, remove Copy path, or add cleanup behavior while fixing this manual fixture finding.

### 2026-05-29: Preserve Current Fixture Undo After Rescan

Status: completed

Evidence:

- During manual fixture review, the user executed fixture Quarantine, rescanned, and found the moved synthetic file no longer appeared in the main Storage Scan rows.
- The user correctly pointed out that undo was no longer reachable from the disappeared row context.
- Code inspection showed `ApplyStorageScanResult` called `ClearQuarantinePreview`, which cleared `_currentRestoreManifest` and `_currentQuarantineExecutionResult`, losing current-fixture undo state after rescan.
- The user suggested a separate quarantined-files area; that is recorded as follow-up because it is a larger UI packet than the immediate safety bugfix.
- The user also expected Quarantine Preview success to be more visible than the status-bar line; that remains a follow-up polish candidate.

Implementation:

- Preserved the current in-memory Restore Manifest and execution result when clearing stale Quarantine Preview state if current-fixture undo is still available.
- Updated fixture execution status text to say `Undo fixture quarantine` can restore and that rescan refreshes review rows.
- Updated post-execution rescan status text to point to the preserved undo action in the Quarantine execution area.
- Extended WPF smoke coverage so fixture execution is followed by a post-execution rescan, then confirms `Undo fixture quarantine` remains available before undo.
- Updated README, domain context, glossary, handoff, and the feature brief.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, cleanup history, and broad manifest restore unavailable.

Verification:

- Initial `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` was blocked because the manually launched WPF app was still running and locking `WindowsFileCleaner.App.exe`.
- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated header styling checklist prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-preserve-current-fixture-undo-after-rescan.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a bounded lifecycle bugfix for current-fixture undo under ADR 0010, with no new persistence, real-profile cleanup execution, restore architecture, or deletion behavior.

Open questions:

- Should the next UI packet add a dedicated quarantined-files area independent of the currently visible Storage Scan rows?
- Should Quarantine Preview success appear in a more prominent pane or callout instead of relying on the status bar?

Rejected ideas buffer:

- Do not keep moved source rows artificially visible after a rescan; the refreshed Storage Scan should reflect the filesystem.
- Do not add real-profile cleanup execution, real-profile Undo Quarantine, all-manifest restore, persisted cleanup history, or permanent deletion while fixing current-fixture undo reachability.

### 2026-05-29: Add Shortlist-Level Quarantine Wording

Status: completed

Evidence:

- During manual fixture review, the user expected a single button to quarantine files/folders on the Review Shortlist instead of needing to visit each file and type `QUARANTINE`.
- Existing WPF execution already acts on the current Quarantine Preview built from Review Shortlist rows, but the visible `Execute quarantine` label did not make that scope clear.

Implementation:

- Renamed `Preview quarantine` to `Preview shortlist quarantine`.
- Renamed `Execute quarantine` to `Quarantine included shortlist`.
- Updated confirmation and execution tooltips/automation help text to say `QUARANTINE` is typed once and applies to all included Review Shortlist rows.
- Added preview/gate/status wording that makes Review Shortlist the source and included rows the execution target.
- Extended WPF smoke coverage from a one-row execution to a two-row Review Shortlist execution and undo.
- Kept exact confirmation, blockers, real-profile/custom execution blockers, permanent deletion, and cleanup history unchanged.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-shortlist-level-quarantine-wording.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI wording and WPF smoke coverage for existing fixture-only execution under ADR 0009.

Open questions:

- Should future real-profile execution keep `Quarantine included shortlist` as the exact label after the separate real-profile Grill with Docs pass?
- Should a dedicated Quarantined Files area show included shortlist rows immediately after execution?

Rejected ideas buffer:

- Do not remove exact confirmation while clarifying shortlist-level execution.
- Do not call the action `Quarantine all` without qualifying that only included rows move; blocked and redundant rows must stay out of execution.

### 2026-05-29: Add Current-Session Quarantined Review

Status: completed

Evidence:

- During manual fixture review, the user wanted Quarantine shortlist execution controls out of the selected-row detail scroll area and grouped with Quarantine Root.
- The user also wanted a `Quarantined`-style button that switches the main grid to quarantined items, plus a way back to normal scan rows.
- Existing docs already captured that current-fixture undo remains available after a post-execution rescan, but the visible controls were still too easy to miss.

Implementation:

- Added a dedicated Quarantine shortlist area above the main grid for Quarantine Root Selection, preview/export, `QUARANTINE` confirmation, `Quarantine included shortlist`, `Undo fixture quarantine`, and Quarantine Execution Gate text.
- Removed the shortlist execution gate from the right selected-row scroll panel.
- Added `Quarantined` and `Back to scan rows` controls.
- Added a read-only current-session quarantined grid backed by current Restore Manifest entries still in `Moved` state.
- Kept older/discovered manifest review in the existing Quarantine Manifest Discovery and readiness panes.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, and cleanup history unavailable.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantined-review-mode.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is a reversible UI placement and read-only current-session review packet under existing fixture-only execution and undo ADRs 0009 and 0010.

Open questions:

- Should a future `Quarantined` view include discovered Restore Manifest entries, or should discovered manifests remain in the existing manifest discovery/readiness panes?

Rejected ideas buffer:

- Do not make moved source rows persist in refreshed Storage Scan results.
- Do not use the `Quarantined` button as cleanup history or broad manifest restore.

### 2026-05-29: Make Review Panels Collapsible

Status: completed

Evidence:

- User review showed the new Quarantine shortlist area made the main grid tiny because the verbose Quarantine Execution Gate text was always expanded above the grid.
- User suggested making the Quarantine list/panel collapsible and possibly doing the same for other vertical sections.

Implementation:

- Made the Quarantine shortlist area collapsible while preserving the grouped Quarantine root, preview, execution, undo, and current-session quarantined controls.
- Kept verbose Quarantine Execution Gate details in a constrained scroll viewport when the panel is expanded.
- Made the Safety Summary section collapsible so the grid can recover additional vertical space during focused review.
- Kept all cleanup execution boundaries unchanged.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/features/2026-05-29-quarantined-review-mode.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF layout behavior with no persistence, cleanup execution, restore, data-model, or security change.

Open questions:

- Should additional review sections, such as filters or selected-row detail panes, become collapsible after the next manual pass?

Rejected ideas buffer:

- Do not hide the Quarantine Execution Gate details entirely; keep them available in a constrained view.

### 2026-05-29: Add Collapsed Panel Summaries

Status: completed

Evidence:

- User verified the collapsible panel pass worked.
- User asked for useful panel header summaries while panels are closed.

Implementation:

- Replaced static Expander header text with dynamic Safety Summary and Quarantine shortlist header text.
- Safety Summary header now keeps compact risk counts visible while collapsed.
- Quarantine shortlist header now summarizes Review Shortlist count, preview included/blocked state, current quarantined count, and undo availability/completion.
- Added WPF smoke assertions for these collapsed-header summaries across scan, preview, fixture execution, and undo.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/features/2026-05-29-quarantined-review-mode.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF header text and test coverage with no cleanup execution, persistence, restore, or data-model change.

Open questions:

- Should the closed Quarantine header also include the selected Quarantine Root, or is that too visually noisy?

### 2026-05-29: Add Quarantine Preview Inline Status

Status: completed

Evidence:

- User noticed that clicking `Preview shortlist quarantine` appeared to do nothing because the success text was only visible at the bottom/status area.
- The Quarantine shortlist area is now the control cluster for root, preview, execution, undo, and current-session quarantined review, so preview readiness should be visible there.

Implementation:

- Added inline `QuarantinePreviewStatusText` inside the Quarantine shortlist panel.
- Before preview, the line explains whether Review Shortlist rows need to be added or previewed.
- After preview, the line summarizes included, blocked, redundant, previewed bytes, readiness blockers, not-cleanup-approval wording, and no-file-modified wording.
- After Quarantine Root changes, the line says preview destinations must be regenerated.
- After fixture execution or undo, the line switches to fixture execution/undo evidence instead of dry-run wording.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, and cleanup history unavailable.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-preview-inline-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI visibility polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should the inline readiness line get visual success/warning styling after a manual fixture pass, or is plain text enough?

Rejected ideas buffer:

- Do not show a modal popup for dry-run preview success unless manual review shows inline text is still too easy to miss.

### 2026-05-29: Add Review Grid Mode Status

Status: completed

Evidence:

- The main grid can now show either Storage Scan rows or Current-Session Quarantined Review rows.
- The visible `Quarantined` / `Back to scan rows` buttons help, but a persistent grid-mode label reduces ambiguity during manual fixture review and after post-execution stale scan rows.

Implementation:

- Added `ReviewGridModeText` above the main grid.
- Storage Scan mode names the scan row display window and says whether current-session quarantined items are available.
- Current-session quarantined mode identifies the read-only current in-memory Restore Manifest view and points to `Back to scan rows`.
- After fixture Quarantine execution, Storage Scan mode warns that scan rows may be stale and points to available current-session quarantined entries.
- After fixture undo clears moved entries, the mode text stops advertising current quarantined rows.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, and cleanup history unavailable.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF status text with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should the Review Grid Mode Status later use a badge or visual severity style, or is plain text sufficient?

Rejected ideas buffer:

- Do not use the grid mode status as cleanup history or as proof that Storage Scan rows are refreshed after fixture execution.

### 2026-05-29: Align Fixture Checklist With Review Polish

Status: completed

Evidence:

- The WPF app now has collapsible Safety Summary and Quarantine shortlist panels, inline Quarantine Preview readiness, Review Grid Mode Status, and `Quarantined` / `Back to scan rows` grid switching.
- The terminal fixture checklist still used older generic wording for Safety Summary, Quarantine Preview, and fixture execution/undo flow.

Implementation:

- Updated `Start-MvpFixtureReview.ps1` checklist item 3 to prompt collapsible Safety Summary header/details review.
- Updated checklist item 6 to prompt collapsible Quarantine shortlist header/details, inline preview readiness, preview/export tooltip, approval-boundary, execution-scope, and execution tooltip review.
- Updated checklist item 7 to prompt `Quarantined` / `Back to scan rows` plus Review Grid Mode Status during fixture execution/undo review.
- Kept checklist-only mode read-only: no preflight, fixture creation, WPF launch, scan, move, restore, delete, or cleanup history.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local manual-review checklist wording with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should the checklist be split into shorter grouped prompts if it becomes too dense during the next manual pass?

Rejected ideas buffer:

- Do not make checklist-only mode run preflight, create fixtures, launch WPF, or scan.

### 2026-05-29: Add Quarantined View Control Help Text

Status: completed

Evidence:

- `Quarantined` / `Back to scan rows` now switch the main grid between Storage Scan rows and current-session quarantined rows.
- Review Grid Mode Status labels the active grid, but the buttons still used mostly static help text and did not explain disabled states.
- User manual fixture review reported steps 1 through 11 working, and confirmed closed panel header summaries are desirable/useful.

Implementation:

- Added dynamic tooltip and automation help text for `Quarantined`.
- Added dynamic tooltip and automation help text for `Back to scan rows`.
- Disabled `Quarantined` help text now explains when current-session rows appear, when the view is already active, and when no moved entries remain after undo.
- Enabled `Quarantined` help text summarizes the current-session moved entry count and read-only/no-restore/no-delete/no-history boundary.
- `Back to scan rows` help text says returning does not rescan, modify files, or perform undo.
- Kept real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, and cleanup history unavailable.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantined-view-control-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help text with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should discovered Restore Manifests eventually get a separate grid switch, or remain in discovery/readiness panes?

Rejected ideas buffer:

- Do not use `Quarantined` as discovered-manifest history or as a broad restore entry point.

### 2026-05-29: Add Quarantine Preview Status Styling

Status: completed

Evidence:

- User originally expected a popup after `Preview shortlist quarantine` and then noticed the inline/status text after looking near the bottom of the Quarantine shortlist area.
- Quarantine Preview Inline Status made the feedback local to the controls, but every state still used the same muted styling.

Implementation:

- Added lightweight semantic styling for `QuarantinePreviewStatusText`.
- Neutral waiting text remains normal weight.
- Ready preview and successful fixture execution/undo evidence use success styling.
- Shortlisted-but-not-previewed, stale preview, blocked preview, and recovery-review states use warning styling.
- Preview creation failure uses error styling.
- Kept Quarantine Preview as a dry run and did not add popup behavior, real-profile execution, permanent deletion, or cleanup history.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should Review Grid Mode Status get similar styling later, or is plain text enough there?

Rejected ideas buffer:

- Do not add a modal popup for Quarantine Preview success unless a future manual visual pass proves styled inline status is still too easy to miss.

### 2026-05-29: Align Fixture Checklist With Preview Status Styling

Status: completed

Evidence:

- Quarantine Preview Status Styling added neutral/success/warning/error inline status styling.
- `Start-MvpFixtureReview.ps1` checklist item 6 still mentioned generic inline preview readiness and did not prompt the next manual pass to inspect the new semantic states.

Implementation:

- Updated fixture checklist item 6 to prompt styled inline preview readiness review with neutral/success/warning/error states.
- Updated README launcher wording from inline preview readiness to styled inline Quarantine Preview readiness.
- Updated the fixture checklist and status-styling feature briefs to record this follow-up alignment.
- Kept checklist-only and dry-run launcher paths read-only: no preflight, fixture creation, WPF launch, scan, move, restore, delete, or cleanup history.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local manual-review checklist wording with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Does the next visible fixture pass show that the styled inline preview status is noticeable enough without a popup?

Rejected ideas buffer:

- Do not make checklist-only mode perform visual verification or launch WPF; it stays terminal-output-only.

### 2026-05-30: Add Quarantine Preview Status Help Text

Status: completed

Evidence:

- User ran the latest manual fixture review checklist through steps 1-11 and reported that all checked behavior worked.
- Collapsed panel header summaries already exist and remain useful while closed.
- The inline Quarantine Preview status is safety-relevant and dynamic, but its exact state/boundary was not mirrored into tooltip or automation help text.

Implementation:

- Added tooltip and automation help text to `QuarantinePreviewStatusText`.
- Updated dynamic status updates so the tooltip/help text mirrors the current inline status and repeats read-only, no-create, no-move, no-restore, no-delete, and not-cleanup-approval boundaries.
- Added WPF smoke assertions across waiting, shortlisted, invalid-root, ready, stale, fixture execution, fixture undo, and blocked-preview states.
- Updated the fixture checklist to prompt manual review of the inline status tooltip/help text.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-quarantine-preview-inline-status.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/features/2026-05-30-quarantine-preview-status-help-text.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next manual fixture visual pass, confirm whether the collapsed panel header summaries are visible enough when panels are closed.

### 2026-05-30: Add Quarantine Shortlist Header Styling

Status: completed

Evidence:

- User confirmed the collapsible layout worked and said a header panel summary while closed would be nice.
- The Quarantine shortlist header already summarized shortlist, preview, current quarantined, and undo state, but it used the same visual style for waiting, ready, blocked/stale, current-quarantined, and undo-completed states.

Implementation:

- Added lightweight semantic styling to `QuarantineShortlistHeaderText`.
- Header state is neutral before shortlist/preview/current quarantine, warning while shortlist needs preview or preview is blocked/stale, success for clean preview or completed undo, and information while current-session quarantined rows exist.
- Kept the header compact; no new row, badge, modal, cleanup execution, restore behavior, persisted history, or real-profile file movement was added.
- Added WPF smoke assertions for startup, shortlisted-before-preview, invalid-root, clean preview, stale preview, blocked preview, current quarantined, and undo-completed header states.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-30-collapsed-panel-header-help-text.md`
- `docs/features/2026-05-30-quarantine-shortlist-header-styling.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- In the next visible fixture pass, confirm whether the styled closed header makes the Quarantine shortlist state easier to scan without looking like cleanup approval.

### 2026-05-30: Record Manual Fixture Checklist Progress

Status: completed

Evidence:

- User ran the latest manual fixture review checklist through steps 1-11 and reported that all checked behavior worked.
- User agreed that a useful panel header summary while a panel is collapsed is desirable.
- The current build already keeps compact dynamic Safety Summary and Quarantine shortlist summaries in the Expander headers and mirrors them through tooltip/help text.

Implementation:

- Recorded the manual verification evidence and closed-header summary preference in durable docs.
- No app behavior changed.
- No real user files were scanned or modified by this docs-only packet.

Verification:

- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed the branch clean before the docs-only update.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm whether the collapsed header summaries are noticeable enough in the actual WPF window, or whether they need stronger visual treatment.

Rejected ideas buffer:

- Do not add new cleanup execution, restore execution, permanent deletion, or cleanup history in response to review-panel layout feedback.

### 2026-05-30: Add Review Grid Mode Status Help Text

Status: completed

Evidence:

- Review Grid Mode Status was visible and semantically styled, but unlike adjacent review controls it did not expose matching tooltip or automation help text.
- The status line is safety-relevant because it distinguishes Storage Scan rows from Current-Session Quarantined Review rows and warns when scan rows may be stale after fixture Quarantine execution.

Implementation:

- Added startup tooltip and automation help text to `ReviewGridModeText`.
- Mirrored each dynamic Review Grid Mode Status message into tooltip and automation help text.
- Added explicit read-only/no-rescan/no-file-modified/no-restore/not-cleanup-approval boundary wording.
- Kept Storage Scan, Quarantine Preview, fixture execution, undo, selected restore, real-profile execution availability, permanent deletion, and cleanup history behavior unchanged.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated grid-mode tooltip/help-text prompt without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `docs/features/2026-05-29-review-grid-mode-status-styling.md`
- `docs/features/2026-05-30-review-grid-mode-status-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help text with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm whether Review Grid Mode Status help text is discoverable enough without a visible help icon.

Rejected ideas buffer:

- Do not add another visible help icon for Review Grid Mode Status unless manual review shows tooltip/help text is insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Grid Mode Help

Status: completed

Evidence:

- Review Grid Mode Status Help Text touched WPF status text, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the grid-mode help-text packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal is still a visible fixture review pass against collapsed header summaries, styled inline Quarantine Preview readiness, styled Review Grid Mode Status tooltip/help text, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Retire Stale Grid-Mode Styling Follow-ups

Status: completed

Evidence:

- `docs/features/2026-05-29-quarantine-preview-status-styling.md` still listed Review Grid Mode Status styling as a future follow-up even though it landed in `2026-05-29-review-grid-mode-status-styling.md`.
- `docs/features/2026-05-29-review-grid-mode-status.md` still framed the question as badge versus plain text, but the current implementation is styled text.

Implementation:

- Updated the Quarantine Preview status-styling brief to mark Review Grid Mode Status styling as completed and leave only manual visual review open.
- Updated the Review Grid Mode Status brief so the remaining question is badge versus styled text after manual fixture review, not badge versus plain text.
- No code or app behavior changed.

Verification:

- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.
- `rg -n "Review Grid Mode Status get similar styling|plain text sufficient|badge or stay as plain text" docs README.md .codex/progress.md` found only historical progress entries and this verification note, not current feature guidance.

Docs updated:

- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is documentation cleanup only.

Open questions:

- Manual fixture review still needs to decide whether styled text is enough or a badge is worth adding.

Rejected ideas buffer:

- Do not let stale feature-brief follow-ups override newer completed packets.

### 2026-05-30: Add Collapsed Panel Header Help Text

Status: completed

Evidence:

- User verified the collapsible panels worked and confirmed that header panel summaries while closed are useful.
- Safety Summary and Quarantine shortlist headers now carry compact dynamic summaries, but long summaries can be harder to inspect when horizontal space is tight.

Implementation:

- Added tooltip and `AutomationProperties.HelpText` to the Safety Summary header.
- Added tooltip and `AutomationProperties.HelpText` to the Quarantine shortlist header.
- Mirrored each dynamic header summary into its tooltip/help text whenever scan, preview, fixture execution, or undo state changes.
- Added header text trimming so tight layouts keep panel headers compact instead of crowding the grid.
- Updated the fixture checklist to prompt header tooltip/help-text review.
- Kept Storage Scan, Quarantine Preview, fixture execution, undo, restore, real-profile execution availability, permanent deletion, and cleanup history behavior unchanged.

Verification:

- Initial `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` failed on a local variable name shadowing error in `UpdateSafetySummaryHeader`; renamed the waiting-state locals and reran the same build.
- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated header tooltip/help-text prompts without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantined-review-mode.md`
- `docs/features/2026-05-30-collapsed-panel-header-help-text.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF help text and layout polish with no cleanup execution, persistence, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, is header tooltip/help text enough, or should safety-critical panel headers get a small always-visible help affordance?

Rejected ideas buffer:

- Do not add another visible help icon until manual fixture review shows header tooltip/help text is insufficient.

### 2026-05-30: Run Full Local MVP Preflight After Header Help

Status: completed

Evidence:

- Collapsed Panel Header Help Text touched WPF header controls, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than the narrow app-test run before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the header-help packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal still needs a visible fixture review pass against collapsed header tooltip/help text, styled inline Quarantine Preview readiness, styled Review Grid Mode Status, and current-session quarantined controls.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-30: Add Current Quarantined Label

Status: completed

Evidence:

- Current-Session Quarantined Review is intentionally current-session and fixture-execution state only.
- The visible `Quarantined` label could be mistaken for all quarantined history or discovered Restore Manifests even though tooltip/help text already explains the narrower scope.

Implementation:

- Renamed the visible `Quarantined` button to `Current quarantined`.
- Updated Review Grid Mode Status wording so stale Storage Scan rows point to `Current quarantined` when current-session moved entries are available.
- Added WPF smoke coverage that asserts the visible button label exposes current-session scope.
- Updated README, domain docs, feature briefs, fixture checklist, handoff, and progress notes.
- Kept current-session grid behavior unchanged: it remains read-only, fixture/current-manifest-only, and does not discover older manifests, restore, move, delete, or create cleanup history.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated `Current quarantined` checklist wording without preflight, fixture creation, or WPF launch.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantined-review-mode.md`
- `docs/features/2026-05-29-quarantined-view-control-help-text.md`
- `docs/features/2026-05-30-current-quarantined-label.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible UI wording for existing current-session-only behavior under ADR 0010, with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Later ADR 0016 decided that older/discovered Restore Manifest review stays in manifest discovery/readiness panes for now, and `Current quarantined` remains current-session-only rather than all quarantined history.

Rejected ideas buffer:

- Do not use the current-session grid switch as discovered-manifest history or broad restore entry point.

### 2026-05-30: Run Full Local MVP Preflight After Current Label

Status: completed

Evidence:

- Current Quarantined Label touched WPF UI text, WPF smoke assertions, fixture checklist wording, and durable docs.
- A full local preflight gives stronger evidence than narrow app-test runs before the next manual fixture visual pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root after the current-label packet was pushed.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- Preflight suggested the next manual fixture step: `.\tools\Start-MvpFixtureReview.ps1 -SkipPreflight`.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal still needs a visible fixture review pass against `Current quarantined`, collapsed header tooltip/help text, styled inline Quarantine Preview readiness, and styled Review Grid Mode Status.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-29: Harden Quarantine Preview Error Style Coverage

Status: completed

Evidence:

- Quarantine Preview Status Styling documented an error style for preview creation failures.
- Existing WPF smoke assertions covered neutral, warning, success, execution, undo, and blocked-preview styling, but did not directly exercise the invalid Quarantine Root error path.

Implementation:

- Added WPF smoke coverage that attempts Quarantine Preview with a relative Quarantine Root after shortlisting a fixture row.
- Asserted the inline Quarantine Preview status says the preview could not be created, keeps no-file-modified wording visible, uses `Error` styling, and remains visually emphasized.
- Kept Quarantine Preview dry-run behavior unchanged; no files or folders are created, moved, restored, deleted, or added to cleanup history.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is test coverage for existing WPF styling behavior with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Does the next visible fixture pass show that the styled inline preview status is noticeable enough without a popup?

Rejected ideas buffer:

- Do not rely on documentation alone for semantic status states; keep each state covered by focused WPF smoke assertions when practical.

### 2026-05-29: Run Full Local MVP Preflight After Review Polish

Status: completed

Evidence:

- Recent packets changed WPF safety/status styling, checklist wording, and WPF smoke coverage around Quarantine Preview and current-session quarantined review.
- A full local preflight gives stronger evidence than narrow app-test runs before the next manual fixture pass.

Implementation:

- Ran `Invoke-MvpPreflight.ps1` from the repository root.
- Preflight restored packages, built the solution, ran core tests, ran WPF app tests, ran the synthetic fixture generator in `-WhatIf` mode, and ran whitespace diff checking.
- No real user files were scanned or modified.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.
- Preflight output ended with `MVP preflight passed. No real user files were scanned or modified.`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch` showed a clean tree before recording this docs-only verification note.

Docs updated:

- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is verification evidence only, with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- The next useful product signal still needs a visible fixture review pass against the styled inline preview status.

Rejected ideas buffer:

- Do not treat preflight as a replacement for manual visual fixture review; it proves automated gates only.

### 2026-05-29: Add Review Grid Mode Status Styling

Status: completed

Evidence:

- During manual fixture review, after fixture Quarantine execution and rescan, moved files disappeared from Storage Scan rows as expected.
- Review Grid Mode Status already names Storage Scan rows versus current-session quarantined rows, but it used the same muted styling for ordinary scan rows, stale scan warnings, and current-session quarantined review.

Implementation:

- Added lightweight semantic styling for `ReviewGridModeText`.
- Ordinary Storage Scan rows use neutral styling.
- Current-session quarantined rows use informational styling.
- Stale Storage Scan rows after fixture Quarantine execution use warning styling.
- Empty current-session quarantined rows after moved entries are gone use warning styling.
- Kept the status read-only and did not add cleanup execution, restore behavior, persistent history, or new UI layout.

Verification:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"` passed.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-grid-mode-status.md`
- `docs/features/2026-05-29-review-grid-mode-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Should the status eventually use a badge instead of styled text?

Rejected ideas buffer:

- Do not add tabs, modal explanations, persisted cleanup history, or discovered-manifest merging just to distinguish the current grid mode.

### 2026-05-29: Align Fixture Checklist With Grid Mode Styling

Status: completed

Evidence:

- Review Grid Mode Status Styling added neutral/informational/warning styling to the WPF grid-mode line.
- `Start-MvpFixtureReview.ps1` checklist item 7 still mentioned generic Review Grid Mode Status and did not prompt the next manual pass to inspect the new semantic states.

Implementation:

- Updated fixture checklist item 7 to prompt styled Review Grid Mode Status review with neutral/informational/warning states.
- Updated README launcher wording from Review Grid Mode Status to styled Review Grid Mode Status.
- Updated the fixture checklist and grid-mode styling feature briefs to record this follow-up alignment.
- Kept checklist-only and dry-run launcher paths read-only: no preflight, fixture creation, WPF launch, scan, move, restore, delete, or cleanup history.

Verification:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly` passed and printed the updated checklist without preflight, fixture creation, or WPF launch.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch` passed and preserved the updated checklist in dry-run launcher output.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

Docs updated:

- `README.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `docs/features/2026-05-29-review-grid-mode-status-styling.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs:

- No ADR added. This is local manual-review checklist wording with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- Does the next visible fixture pass show that styled Review Grid Mode Status is noticeable without crowding the grid?

Rejected ideas buffer:

- Do not make checklist-only mode perform visual verification or launch WPF; it stays terminal-output-only.
