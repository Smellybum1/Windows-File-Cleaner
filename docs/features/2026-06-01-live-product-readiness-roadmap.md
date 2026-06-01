# Feature: Live Product Readiness Roadmap

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make the remaining path to a safe live product explicit without enabling any new cleanup, restore, delete, or history behavior.

The roadmap should help future packets choose the next safest work while preserving the current fixture-first and read-only real-profile boundary.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile selected restore execution in this roadmap packet.
- Do not enable broad real-profile Undo Quarantine.
- Do not enable permanent deletion.
- Do not add persisted cleanup history.
- Do not scan, move, restore, delete, create, or rewrite real-profile files.
- Do not replace ADR 0017, ADR 0018, ADR 0019, README, or the fixture checklist.

## User Story / Job Story

As the local app owner, I want one compact readiness roadmap, so that the project can keep moving toward a safe live cleanup product without confusing fixture proof, read-only readiness evidence, and real-profile movement approval.

## Current Behavior

The app is a local WPF desktop reviewer with:

- read-only Storage Scan,
- fixture-only Quarantine execution,
- current-fixture Undo Quarantine,
- fixture selected restore and exact real-profile selected restore under ADR 0019,
- exact real-profile Quarantine execution behind ADR 0017/0018 readiness and approval evidence,
- read-only selected restore revalidation evidence,
- a full `.cmd` MVP preflight,
- a manual fixture review checklist.

Real-profile Quarantine movement, permanent deletion, and persisted cleanup history remain intentionally unavailable.

## Desired Behavior

Future work should follow this readiness sequence.

| Readiness track | Current evidence | Done when | Next likely packet |
|---|---|---|---|
| Manual fixture acceptance | Full `.cmd` MVP preflight passed after the Checklist-Only Visible Fixture Next Step packet at `71cf15a`. Header/tab UI has user visual approval. The user ran the visible fixture review flow on 2026-06-01 and reported that it looks good. The fixture launcher can write an ignored `.local` acceptance notes template from the checklist, grouped by fixture review area, with repo path, Git branch/commit, worktree status at notes creation, .NET SDK, WPF app project/target framework/WPF flag, required preflight, post-preflight visible fixture command, preflight/worktree checkboxes, local-not-cleanup-history wording, and exact post-pass recorder/summary/completion commands embedded in the notes, then print the same commands for that notes file. Checklist-only output also repeats the exact notes-enabled visible fixture command and the no-preflight/no-fixture/no-WPF/no-scan/no-movement boundary. The latest ignored notes file was generated from clean `main` at commit `433064e` with `Worktree status at notes creation: clean`, but the formal checklist remains unfilled. A read-only summary helper can report latest ignored notes metadata, worktree status at notes creation, acceptance-evidence checkbox states, overall result, checklist totals, and issue/not-checked/not-recorded items with compact notes or prompt previews, and can fail fast with `-RequireComplete` while notes are incomplete. `Record-FixtureAcceptanceNotes.cmd -RecordManualAcceptance` can mark ignored notes complete after a human all-pass fixture review without launching WPF, scanning, movement, restore, deletion, approval, or cleanup history. | The visible fixture pass is user-accepted for current UI/readiness confidence; formal checklist acceptance is done when the ignored notes record preflight/worktree evidence, an overall result, and no not-recorded checklist items. | Optionally record the latest all-pass notes with `Record-FixtureAcceptanceNotes.cmd -RecordManualAcceptance` after the human pass, then run the summary helper with `-RequireComplete`; otherwise move to fresh real-profile read-only retest after a new full preflight. |
| Fresh real-profile read-only retest | User previously confirmed real-profile scan works and debounced search fixed sluggish typing. After the user-reported fixture visual pass, the user completed the requested full preflight plus WPF run against `C:\Users\moxhe` and reported that everything worked well, covering scan gate, read-only scan completion, search responsiveness, tab/header usability, Review Shortlist context, and preview-only Quarantine boundary. | Completed as user-reported read-only evidence; repeat after future code/workflow changes before crossing any new movement boundary. | Already followed by the approved ADR 0019 selected real-profile restore implementation packet. |
| Real-profile selected restore implementation | ADR 0019 is implemented in WPF for one selected exact `C:\Users\moxhe` Restore Manifest after selected readiness, exact `RESTORE`, and immediate revalidation. Automated WPF coverage opens a synthetic exact real-profile gate without executing movement and proves stale missing-quarantine paths keep the gate blocked. The user completed the sacrificial selected real-profile restore trust test on 2026-06-01 and reported steps 1-9 all succeeded. | Completed for implementation and initial manual trust. | Reuse selected real-profile restore only as recovery for a specific selected Restore Manifest; do not expand to all-manifest restore without a new decision. |
| First real-profile Quarantine execution | Implemented in WPF for exact `C:\Users\moxhe` only after ADR 0017/0018 readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, selected real-profile restore trust, and immediate pre-execution revalidation pass. Automated tests keep synthetic real-profile missing-source attempts blocked without moving real files. The user reported a successful first live batch on 2026-06-01: one `pip\cache\http\b\c` row moved, one Restore Manifest completed, `moved 1, failed 0`, and zero readiness blockers. `tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` can now gather full MVP preflight, accepted-package, formal fixture-notes status, exact-profile displayed Restore Manifest evidence, and focused recovery-review/undo-work evidence before a future manual batch without launching WPF, scanning the real profile, or moving/restoring/deleting files; its Restore Manifest display focus defaults to exact `C:\Users\moxhe`, with `-AllCleanupScopes` available for fixture-inclusive review outside the preset. | Completed for implementation and first tiny-batch manual live trust; terminal readiness review is available as evidence only. | Any next real-profile Quarantine batch should remain tiny, exact-profile, readiness-gated, and user-clicked after the next-batch evidence preset plus WPF readiness review. |
| Real-profile recovery confidence | Fixture current undo, fixture selected restore, and exact real-profile selected restore implementation are available; the sacrificial selected real-profile restore trust test succeeded by user report. The successful first live Quarantine created Restore Manifest `restore-manifest-20260601112432-a715565e` under `D:\WindowsFileCleanerQuarantine`. The first selected restore attempt for that manifest failed safely because cross-volume directory restore needed the same copy-then-delete fallback as Quarantine; after the fallback and narrow retryability fix, the user retried and reported the highlighted result `selected restore succeeded. Restored 1, failed 0`. The read-only `tools\Summarize-RestoreManifests.cmd` helper can summarize action-scoped Restore Manifest state, focus displayed manifests by exact Cleanup Scope with `-CleanupScope`, focus recovery-review manifests with `-RecoveryReviewOnly`, focus undo-work manifests with `-UndoWorkOnly`, and fail terminal evidence checks with `-RequireNoRecoveryReview` or `-RequireNoUndoWork`, without launching WPF, scanning, moving, restoring, deleting, writing manifests, approving cleanup, or creating cleanup history. | Completed for selected-manifest recovery proof without broad all-manifest restore or cleanup history. | Rediscover manifests and rescan before further cleanup review; use the terminal summary helper when compact Restore Manifest evidence is useful; use `-CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries` when exact real-profile recovery-review debt needs focus; use `-CleanupScope "C:\Users\moxhe" -UndoWorkOnly` when exact real-profile moved-entry evidence needs focus; keep recovery selected-manifest-only unless a new docs/ADR packet deliberately expands it. |
| Release/packaging readiness | Portable v1 packaging exists through `tools\Publish-LocalRelease.cmd`. It runs MVP preflight by default, publishes a self-contained `Release` / `win-x64` WPF app under ignored `.local\releases\windows-file-cleaner-vYYYYMMDD-HHMMSS\app`, writes local release metadata including the executable SHA-256, creates a zip plus zip `.sha256` sidecar, prints launch commands, and writes release-local normal/fixture launch `.cmd` scripts plus `README-FIRST.txt`. `tools\Test-LocalRelease.cmd` can verify the latest or explicit ignored package folder, metadata, README, launch scripts, zip, checksum evidence, and safety-boundary lines without launching WPF or scanning anything. `tools\Start-LocalRelease.cmd` can verify then print package-local README/script paths plus the launch command, print a package-level acceptance checklist, write ignored acceptance notes, or start the latest package from the repo root; `-Fixture` only prefills the repo-local smoke fixture Cleanup Scope. `tools\Start-LocalRelease.cmd -ChecklistOnly -WriteAcceptanceNotes` stamps exact normal and fixture launch commands into ignored notes, `tools\Summarize-LocalReleaseAcceptanceNotes.cmd` can print them while summarizing ignored package acceptance notes without launching WPF or scanning, and `tools\Record-LocalReleaseAcceptanceNotes.cmd` can mark ignored notes complete after a human has already accepted the package. Current accepted local package baseline is `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`, with completed ignored notes `.local\release-acceptance\release-acceptance-20260602-011743.md`; `tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` reports verifier/current-commit/normal-launch/fixture-launch evidence recorded, overall result `Pass`, and `6 pass, 0 issue, 0 not checked, 0 not recorded`. `tools\Start-AcceptedLocalRelease.cmd` can select that latest completed acceptance-notes package and delegate to `Start-LocalRelease.cmd` without treating later docs-only commits as a package mismatch. `tools\Invoke-DailyLocalReadiness.cmd` now composes the accepted evidence check, one accepted package verification pass, accepted normal/fixture print-only launch commands, and Restore Manifest Summary output into one read-only daily check. The README Daily Local Use section surfaces the daily readiness command, accepted package print commands, accepted evidence check, read-only Restore Manifest summary, and real-profile movement stop boundary near the top of the project docs. | Completed for a local portable v1 package plus read-only package verification, repo-level launch ergonomics, ignored local acceptance notes, terminal recording of completed package acceptance, one accepted local package baseline, an accepted-package launcher for daily use, top-level daily-use guidance, and a daily local readiness wrapper that verifies the accepted package once per run. | Use the README Daily Local Use section or run `Invoke-DailyLocalReadiness.cmd` for the read-only daily check. Use `Start-AcceptedLocalRelease.cmd -PrintOnly` / `-Fixture -PrintOnly` to print only the accepted package command; remove `-PrintOnly` only when intentionally launching the accepted package. Use `Test-LocalRelease.cmd -RequireCurrentCommit` after cutting a new package when the package must exactly match current `HEAD`; use `Start-LocalRelease.cmd -PrintOnly -RequireCurrentCommit` to print a current-HEAD package command; use `Start-LocalRelease.cmd -ChecklistOnly -RequireCurrentCommit -WriteAcceptanceNotes` when formal local package acceptance notes are useful; use `Record-LocalReleaseAcceptanceNotes.cmd -RecordManualAcceptance` only after human package acceptance is complete; consider installed shortcut/installer automation only in a later explicit user-approved packet. |
| Later full cleanup expansion | Permanent deletion and persisted cleanup history are intentionally unavailable. | Only if the user chooses them after reversible Quarantine and recovery are trusted, with new ADRs, tests, recovery wording, and explicit approval. | Separate Grill with Docs packets; do not bundle with first real-profile Quarantine. |

Later packet note: `Fixture Acceptance Notes Recorder` added `Record-FixtureAcceptanceNotes.cmd -RecordManualAcceptance` so a completed all-pass manual fixture review can be recorded in ignored notes without hand-editing markdown. This remains local evidence only and does not replace the human visible pass.

Later packet note: `Restore Manifest Displayed Strictness` added displayed-only terminal checks to the real-profile recovery confidence path, so exact `C:\Users\moxhe` displayed evidence can prove "display exists and has no undo work" without changing full-root fixture-history strictness. This remains read-only terminal evidence and not cleanup approval.

Later packet note: `Daily Readiness Fixture Acceptance Status` made `Invoke-DailyLocalReadiness.cmd` and `Invoke-RealProfileQuarantineReadiness.cmd` optionally include or require Fixture Acceptance Notes evidence. This is read-only local ignored-note evidence only and does not record notes, launch WPF, scan, move, restore, delete, approve cleanup, or create cleanup history.

Later packet note: `Real-Profile Next-Batch Evidence Preset` added `Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` as the recommended terminal-only review before another tiny exact real-profile batch. The preset stays exact `C:\Users\moxhe` only, includes Fixture Acceptance Notes status, requires displayed Restore Manifest evidence, and requires zero displayed undo-work manifests without turning recovery-review debt into a default blocker.

## Domain Language Changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing terms are reused. | n/a |

## Open Questions

Questions that must be answered before implementation:

- None for this roadmap packet.

Questions that can be deferred:

- Should permanent deletion ever exist in this app, or remain outside the product?
- Should persisted cleanup history exist after Restore Manifest-only recovery is trusted?
- Should all-manifest real-profile restore ever exist, or should real-profile recovery remain selected-manifest-only?

## Grill Notes

### Scenarios Discussed

- The user wants the project to continue toward a safe full live product.
- The UI was messy, then header/tabs/panels were cleaned up and visually approved.
- The user explicitly said not to move, delete, quarantine, or restore real-profile files unless asked after a Grill with Docs pass.

### Edge Cases

- A roadmap must not imply that real-profile movement is available.
- "Live product" can mean multiple milestones; this roadmap keeps reversible real-profile cleanup separate from later deletion/history expansion.
- The next visible fixture pass can find layout or wording issues that should be fixed before real-profile retest.

### Dependencies Between Decisions

- Real-profile Quarantine depends on selected real-profile restore readiness.
- Selected real-profile restore manual trust depends on the implemented ADR 0019 path and explicit user approval for the specific restore click.
- Permanent deletion and cleanup history depend on trusted reversible Quarantine/recovery first.
- Packaging is lower risk after behavior is stable, but should not distract from safety gates.

## Evidence and Validation Gate

Evidence gathered:

- User answers:
  - Real-profile scan works.
  - Debounced search fixed large-scan sluggish typing.
  - The non-`D:` acknowledgement row feels clear.
  - The tabbed/header UI looks much better.
  - The manual fixture visual pass looks good.
  - The fresh real-profile read-only retest worked well end to end.
  - The first approved exact real-profile Quarantine batch succeeded with `moved 1, failed 0`.
  - Do not move, delete, quarantine, or restore real-profile files without explicit approval after Grill with Docs.
- Existing code/docs inspected:
  - `README.md`
  - `.codex/progress.md`
  - `docs/codex/thread-handoff.md`
  - `docs/domain/context.md`
  - `docs/domain/glossary.md`
  - `docs/features/2026-05-28-mvp-readiness-audit.md`
  - `docs/features/2026-05-31-real-profile-quarantine-design-pass.md`
  - `docs/features/2026-05-31-real-profile-selected-restore-execution-contract.md`
  - ADR 0017, ADR 0018, and ADR 0019
- Tests/checks planned:
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not treat the roadmap as approval to implement real-profile movement.
- Do not collapse selected real-profile restore and forward real-profile Quarantine into one packet.
- Do not bundle permanent deletion or cleanup history into the first live cleanup action.

## Decisions Made

Small feature-level decisions:

- Use a feature brief rather than an ADR because this packet organizes existing accepted decisions and evidence.
- Treat visible fixture acceptance and fresh real-profile read-only retest as gates before any real-profile movement implementation.
- Treat selected real-profile restore as the recovery prerequisite before first real-profile Quarantine execution.

ADR-worthy decisions:

- [x] None. ADR 0017, ADR 0018, and ADR 0019 already hold the durable movement/restore decisions.

## Implementation Plan

1. Add this roadmap feature brief.
2. Link it from README and handoff docs.
3. Record the packet in `.codex/progress.md`.
4. Run checklist-only and whitespace checks.

## Files Expected To Change

Expected:

- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test Plan

Manual checks:

- Review the roadmap wording and confirm it does not imply real-profile movement is currently available.

Automated checks:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks And Assumptions

Risks:

- A roadmap can become stale if real-profile restore or Quarantine execution lands without updating it.
- The phrase "full live product" may later need a sharper definition from the user once reversible cleanup is trusted.

Assumptions:

- Reversible Quarantine plus selected restore is the right first live cleanup milestone before deletion or cleanup history.
- The next best product evidence is still a visible fixture pass, not real-profile movement.

## Completion Notes

Completed on: 2026-06-01

What changed:

- Added this roadmap with explicit readiness tracks from fixture acceptance through later deletion/history decisions.
- Linked the roadmap from README and handoff docs.
- Recorded the packet in the progress log.
- Later packet `2026-06-01-fixture-acceptance-notes-template.md` updated the manual fixture acceptance track to note the launcher can write an ignored `.local` acceptance notes template from the checklist.
- Later packet `2026-06-01-fixture-checklist-section-grouping.md` updated the manual fixture acceptance track to note grouped checklist and notes output.
- Later packet `2026-06-01-fixture-notes-launcher-wording-alignment.md` updated the manual fixture acceptance track to use the post-preflight `-SkipPreflight -WriteAcceptanceNotes` launcher command.
- Later packet `2026-06-01-fixture-acceptance-evidence-header.md` updated the manual fixture acceptance track to note repo path, Git branch/commit, required preflight, post-preflight visible fixture command, preflight/worktree checkboxes, and local-not-cleanup-history wording in ignored `.local` notes.
- Later packet `Full Local MVP Preflight After Evidence Header` confirmed the current full `.cmd` MVP preflight after the evidence-header packet before the next visible fixture acceptance pass.
- Later packet `2026-06-01-fixture-acceptance-build-context-header.md` updated the manual fixture acceptance track to note .NET SDK and WPF app project/target framework/WPF flag evidence in ignored `.local` notes.
- Later packet `Full Local MVP Preflight After Build Context Header` confirmed the current full `.cmd` MVP preflight after the build-context-header packet before the next visible fixture acceptance pass.
- Later packet `2026-06-01-fixture-acceptance-current-commit-notes-preview.md` generated checklist-only acceptance notes from commit `fd8e1d4` after that full preflight evidence and kept the next gate as the visible fixture pass.
- Later packet `2026-06-01-fixture-acceptance-notes-summary-helper.md` added a read-only helper for summarizing latest or explicit ignored acceptance notes before copying relevant results into the progress log.
- Later packet `Fixture Acceptance Summary Prompt Preview` added compact prompt previews for open checklist items in the summary helper output.
- Later packet `Fixture Acceptance Evidence Checkbox Summary` added preflight-passed and worktree-clean/intentional evidence checkbox states to the summary helper output.
- Later packet `Fixture Acceptance Completion Check` added `-RequireComplete` to the summary helper so incomplete local acceptance notes can fail fast after the visible fixture pass.
- Later packet `Fixture Acceptance Post-Pass Guidance` made preflight output point to the printed follow-up commands and made the fixture launcher print exact summary and completion-check commands for newly written notes.
- Later packet `Fixture Acceptance Notes Embedded Commands` made the generated notes file include the exact summary and completion-check commands as well.
- Later packet `Full Local MVP Preflight After Embedded Notes` confirmed the current full `.cmd` MVP preflight after the embedded-command notes workflow before the next visible fixture acceptance pass.
- Later packet `Fixture Acceptance Notes Worktree Stamp` added worktree status at notes creation to ignored notes and the summary helper so the visible pass has clearer clean/intentional-worktree evidence.
- Later packet `Fixture Acceptance Clean Worktree Notes Preview` generated `.local\fixture-review-acceptance\fixture-acceptance-20260601-122940.md` from clean `main` at `f181627`, confirmed the clean worktree stamp through explicit summary output, and kept the visible fixture pass as the next gate.
- Later packet `Full Local MVP Preflight After Clean Notes Preview` confirmed the current full `.cmd` MVP preflight after `8529a91`, including restore, build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output, and whitespace diff before the next visible fixture acceptance pass.
- Later packet `Full Local MVP Preflight After Current Evidence Wording Alignment` confirmed the current full `.cmd` MVP preflight after `466ad79`, including restore, build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output, and whitespace diff before the next visible fixture acceptance pass.
- Later packet `Checklist-Only Visible Fixture Next Step` made plain checklist-only output repeat the exact notes-enabled visible fixture command and the no-preflight/no-fixture/no-WPF/no-scan/no-movement boundary so the next manual fixture pass is discoverable without launching anything.
- Later packet `Full Local MVP Preflight After Checklist-Only Next Step` confirmed the current full `.cmd` MVP preflight after `71cf15a`, including restore, build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output with the exact visible fixture next-step block, and whitespace diff before the next visible fixture acceptance pass.
- Later packet `Fixture Acceptance Current Baseline Notes Preview` generated `.local\fixture-review-acceptance\fixture-acceptance-20260601-131233.md` from clean `main` at `dd86566`, confirmed the clean worktree stamp through explicit summary output, and kept the visible fixture pass as the next gate.
- Later packet `User-Reported Manual Fixture Visual Pass` recorded that the user ran the visible fixture review flow and reported it looks good; the latest notes file `.local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md` is stamped clean from `433064e` but remains formally unfilled.
- Later packet `User-Reported Fresh Real-Profile Read-Only Retest` recorded that the user completed the requested full preflight plus WPF run against `C:\Users\moxhe` and reported everything worked well, including scan-gate, scan, search, UI, Review Shortlist, and preview-only Quarantine checks.
- Later packet `User-Reported First Real-Profile Quarantine Success` recorded that the user approved and clicked one exact real-profile Quarantine batch and reported `moved 1, failed 0`, one completed Restore Manifest, and zero readiness blockers.
- Later packet `Portable v1 Release Packaging` added `tools\Publish-LocalRelease.cmd`, which runs MVP preflight by default, publishes a self-contained local `win-x64` portable release under ignored `.local\releases`, writes release metadata, creates a zip, and prints launch commands while keeping v1 reversible-only.
- Later packet `Portable Release Verifier` added `tools\Test-LocalRelease.cmd`, which reads ignored local package artifacts only and verifies the package folder, executable, metadata, zip, safety-boundary lines, and commit evidence without launching WPF or scanning.
- Later packet `Portable Release Launch Scripts` made each new package include release-local normal and fixture launch `.cmd` scripts and extended the verifier to check those ignored artifacts.
- Later packet `Restore Manifest Summary Tool` added `tools\Summarize-RestoreManifests.cmd`, which reads action-scoped Restore Manifest JSON and prints compact recovery evidence without launching WPF, scanning, moving, restoring, deleting, writing manifests, approving cleanup, creating cleanup history, or adding broad/all-manifest restore.
- Later packet `Restore Manifest Recovery Review Filter` added `-RecoveryReviewOnly` and `-RequireNoRecoveryReview` to the read-only summary helper so recovery-review debt can be focused or gated without adding restore/history behavior.
- Later packet `Restore Manifest Undo Work Filter` added `-UndoWorkOnly` and `-RequireNoUndoWork` to the read-only summary helper so outstanding moved entries can be focused or gated without adding restore/history behavior.
- Later packet `Restore Manifest Cleanup Scope Filter` added `-CleanupScope` to focus displayed manifests by exact Cleanup Scope while preserving full-root aggregate counts and read-only/no-restore behavior.
- Later packet `Local Release Launcher` added `tools\Start-LocalRelease.cmd`, which verifies the latest or explicit ignored package by default, can print the normal or fixture launch command without launching WPF, and only launches the packaged app when `-PrintOnly` is omitted.
- Later packet `Portable Release Start Here Readme` made each new package include `README-FIRST.txt` with launch choices and reversible-only v1 boundaries, and extended package verification to require that README in the folder and zip.
- Later packet `Local Release Launcher Start-Here Output` made `tools\Start-LocalRelease.cmd` print the package-local README and matching release-local launch script path before the launch command.
- Later packet `Portable Release Checksum Evidence` made each new package include executable SHA-256 metadata and a sibling zip `.sha256` sidecar, and extended verification to recompute both without launching WPF or scanning.
- Later packet `Local Release Acceptance Checklist` added `tools\Start-LocalRelease.cmd -ChecklistOnly`, which verifies by default and prints package-level acceptance steps without launching WPF.
- Later packet `Local Release Acceptance Notes` added `tools\Start-LocalRelease.cmd -ChecklistOnly -WriteAcceptanceNotes` and `tools\Summarize-LocalReleaseAcceptanceNotes.cmd` for ignored local package acceptance evidence without launching WPF.
- Later packet `Local Release Acceptance Notes Evidence Prefill` made generated notes pre-record verifier/current-commit evidence when the launcher has actually proven those facts, while keeping launch and fixture scan evidence manual.
- Later packet `Local Release Acceptance Notes Launch Commands` made generated notes, checklist-only output, and the read-only summary print the exact normal and fixture launch commands while keeping launch evidence manual.
- Later packet `Local Release Acceptance Notes Recorder` added `tools\Record-LocalReleaseAcceptanceNotes.cmd`, which updates ignored package acceptance notes after human confirmation without launching WPF, scanning, moving, restoring, deleting, approving cleanup, or creating cleanup history.
- Later packet `Portable v1 Acceptance Baseline` recorded that package `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869` has completed local ignored acceptance notes at `.local\release-acceptance\release-acceptance-20260602-011743.md`, with `-RequireComplete` passing after user-confirmed package acceptance.
- Later packet `Accepted Local Release Launcher` added `tools\Start-AcceptedLocalRelease.cmd`, which requires completed ignored acceptance notes, selects the accepted release folder, and delegates to the normal local release launcher without requiring the package to match later docs-only commits.
- Later packet `Accepted Package Daily Use Guide` added a top-level README Daily Local Use section for accepted package print commands, accepted evidence checking, read-only Restore Manifest summary, and the stop boundary before real-profile movement.
- Later packet `Daily Local Readiness Check` added `tools\Invoke-DailyLocalReadiness.cmd`, which verifies accepted package notes, prints accepted normal/fixture launch commands, and prints Restore Manifest Summary output without creating shortcuts, installing anything, launching WPF, scanning, moving, restoring, deleting, approving cleanup, or creating cleanup history.
- Later packet `Daily Readiness Single Package Verification` made the daily readiness wrapper run the accepted package verifier once, then skip duplicate verification only for the second print-only fixture launch command in the same run.
- Later packet `Real-Profile Quarantine Readiness Review` added `tools\Invoke-RealProfileQuarantineReadiness.cmd`, which runs full MVP preflight by default, then daily local readiness plus focused Restore Manifest recovery-review and undo-work summaries before any future tiny exact real-profile batch review, without launching WPF, scanning the real profile, moving, restoring, deleting, approving cleanup, or creating cleanup history.
- Later packet `Real-Profile Readiness Default Scope Focus` made `tools\Invoke-RealProfileQuarantineReadiness.cmd` focus Restore Manifest display output to exact `C:\Users\moxhe` by default, while `-AllCleanupScopes` preserves the fixture-inclusive display path.
- Later packet `Daily Readiness Fixture Acceptance Status` added optional Fixture Acceptance Notes summary and strict completion forwarding to daily readiness and real-profile readiness without changing default daily output.
- Later packet `Real-Profile Next-Batch Evidence Preset` added `tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` as the exact-profile-only terminal preset before another tiny exact batch review.

Files changed:

- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`
- Later verification packet ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd` after the embedded-command notes workflow; restore, build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output, and whitespace diff passed without scanning or modifying real user files.
- Later worktree-stamp packet ran checklist-notes output and an explicit notes summary that printed the stamped worktree status without launching WPF, scanning, moving, restoring, deleting, or creating cleanup history.
- Later clean-worktree notes-preview packet ran checklist-notes output, inspected the generated notes header, ran explicit notes summary, verified explicit `-Path ... -RequireComplete` fails while notes are unfilled, and did not launch WPF, scan, move, restore, delete, or create cleanup history.
- Later full-preflight-after-clean-notes packet ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output, and whitespace diff passed without launching WPF, scanning real-profile files, moving, restoring, deleting, or creating cleanup history.
- Later full-preflight-after-current-evidence packet ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output, and whitespace diff passed without launching WPF, scanning real-profile files, moving, restoring, deleting, or creating cleanup history.
- Later full-preflight-after-checklist-only-next-step packet ran `cmd.exe /c tools\Invoke-MvpPreflight.cmd`; restore, build, core tests, WPF app tests, fixture `-WhatIf`, sectioned checklist-only output with the exact visible fixture next-step block, and whitespace diff passed without launching WPF, scanning real-profile files, moving, restoring, deleting, or creating cleanup history.
- Later current-baseline notes-preview packet ran checklist-notes output, inspected the generated notes header, ran explicit notes summary, verified explicit `-Path ... -RequireComplete` fails while notes are unfilled, and did not launch WPF, scan, move, restore, delete, or create cleanup history.
- Later user-reported manual fixture visual pass packet ran the read-only summary helper on `.local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md` and verified `-RequireComplete` still exits non-zero while notes are unfilled.
- Later user-reported fresh real-profile read-only retest packet recorded user-reported successful completion of the requested full preflight plus WPF real-profile read-only retest steps; Codex did not run a real-profile scan or move/restore/delete files.
- Later first real-profile Quarantine execution packet implemented the exact `C:\Users\moxhe` forward movement path behind ADR 0017/0018 readiness, approval evidence, and immediate revalidation, while keeping Codex and automated tests from clicking real-profile movement.

Docs updated:

- README, feature brief, handoff, and progress log.

ADRs added or skipped:

- No ADR added. This roadmap organizes existing accepted decisions without changing architecture, persistence, cleanup execution, restore behavior, data model, or security policy.

Follow-up work:

- Run `tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` before considering another tiny exact real-profile Quarantine batch, then still require WPF readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, immediate Pre-Execution Revalidation, and explicit user approval for the specific click.
- Keep real-profile restore selected-manifest-only, exact `C:\Users\moxhe`, exact `RESTORE`, immediate selected-restore revalidation, no original-path overwrite, Restore Manifest-only, no all-manifest restore, no cleanup history, no permanent deletion, and no action-folder cleanup.
- Optionally fill `.local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md` and run the summary helper with `-RequireComplete` if formal notes evidence is desired.

Open questions:

- Whether permanent deletion, persisted cleanup history, or all-manifest restore should ever be added.

Risky assumptions:

- Roadmap-level clarity helps future packets avoid unsafe scope jumps without adding too much process.
