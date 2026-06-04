# Progress Log

Last updated: 2026-06-04

This is the compact current progress log. Historical packet evidence lives in:

- `.codex/archive/progress-2026-05-2026-06.md`
- `.codex/archive/progress-2026-06-04-pre-context-compaction.md`

Read archived evidence only when the current task needs old packet detail.

## Current Status

Read first: `docs/codex/current-state.md`.

Latest docs/workflow packet: `2026-06-04-startup-context-compaction`. It moved oversized read-first docs into reference/archive files and replaced them with compact active docs to reduce Codex thread lag. No app behavior changed.

Latest tooling/evidence packet: `2026-06-04-ci-actions-runtime-maintenance`. GitHub Actions MVP Preflight now uses Node 24-capable official actions and pins the hosted runner to `windows-2022` while remaining read-only.

Latest package candidate: `.local\releases\windows-file-cleaner-v20260604-121922` at app commit `e6ac3eb`, verified but not human-accepted. Current pending acceptance notes: `.local\release-acceptance\release-acceptance-20260604-164509.md`.

Accepted package baseline: `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`, with completed ignored notes `.local\release-acceptance\release-acceptance-20260602-011743.md`.

Latest live-product evidence: the 2026-06-04 second tiny exact real-profile WPF Quarantine batch moved one `pip\cache\http-v2` `.body` file, `28.93 MB`, with `moved 1`, `failed 0`, `Recovery review: no`.

Post-action evidence:

- Manifest: `D:\WindowsFileCleanerQuarantine\actions\quarantine-action-draft-20260604014901-b7b402a2\restore-manifest.json`
- Exact-profile displayed undo work: `1`
- Exact-profile displayed recovery review: `2`
- User WPF selected-manifest readiness screenshot showed the selected second-batch manifest has `1` restorable entry and `0` blocked selected entries.

## Next Recommended Work

1. Stop after the second tiny exact real-profile batch; do not chain another real-profile Quarantine batch.
2. Do not click real-profile Quarantine from Codex.
3. Do not run or treat another next-batch review as movement evidence while exact-profile displayed undo work is present unless a new Grill with Docs pass decides outstanding selected-manifest undo work is acceptable.
4. If recovery is needed, use selected-manifest restore only for the exact selected `C:\Users\moxhe` Restore Manifest after readiness, exact `RESTORE`, and immediate selected-restore revalidation.
5. If packaging is the next focus, complete human acceptance for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it over the accepted `bc9b869` baseline.
6. Use `docs/operations/*.md` for command detail.

## Recent Packet Summaries

### 2026-06-04: CI Actions Runtime Maintenance

Status: completed

Goal:

- Remove upcoming GitHub Actions Node 20 runtime warnings and avoid silently accepting the June 2026 `windows-latest` Windows 2025 / Visual Studio 2026 image migration.

Safety profile:

- `terminal-readonly`. The change updates CI YAML and docs only. It does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, write acceptance notes, write Restore Manifests, or create cleanup history.

Changes:

- `.github\workflows\mvp-preflight.yml` now uses `actions/checkout@v6`.
- `.github\workflows\mvp-preflight.yml` now uses `actions/setup-dotnet@v5`.
- `.github\workflows\mvp-preflight.yml` now runs on `windows-2022` instead of floating `windows-latest`.
- Added `docs\features\2026-06-04-ci-actions-runtime-maintenance.md`.

Verification:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

ADRs:

- Skipped; this is reversible CI runtime maintenance and does not change product behavior, cleanup execution, restore execution, persistence, security, data model, deployment packaging, or core UX flow.

### 2026-06-04: Package Acceptance Current-HEAD Summary

Status: completed

Goal:

- Make package acceptance summaries show whether the notes creation commit and package commit match the current repository `HEAD`, so pending human acceptance can see package/current-HEAD mismatch context without inferring it from separate commands.

Safety profile:

- `terminal-readonly`. The change adds read-only Git metadata to package acceptance summaries and extends the existing temporary-note regression. It does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, write acceptance notes, or create cleanup history.

Changes:

- `tools\Summarize-LocalReleaseAcceptanceNotes.ps1` now prints `Current repository HEAD`.
- It also prints `Notes/current HEAD status` as `Matches current HEAD`, `Differs from current HEAD`, `Notes commit unavailable`, or `Current HEAD unavailable`.
- It prints `Package/current HEAD status` as `Matches current HEAD`, `Differs from current HEAD`, `Package commit unavailable`, or `Current HEAD unavailable`.
- Short and full Git commit text are treated as matching when one is a prefix of the other.
- `tools\Test-LocalReleaseAcceptanceSummary.cmd` / `.ps1` now covers matching, differing, and missing notes/package commit cases.

Verification:

- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-164509.md"`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`

ADRs:

- Skipped; this adds terminal-only evidence wording for existing package acceptance tooling under ADR 0020 and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Package Summary Ignored-Path Guard Regression

Status: completed

Goal:

- Make `Summarize-LocalReleaseAcceptanceNotes.cmd -Path` enforce the same ignored `.local` notes boundary promised by the summary output and recorder guidance.

Safety profile:

- `terminal-readonly`. The change affects terminal summary path validation only. The regression uses committed `README.md` as an existing in-repo but non-`.local` explicit path, writes temporary test notes only under ignored `.local`, and removes those temporary notes. It does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- `tools\Summarize-LocalReleaseAcceptanceNotes.ps1` now rejects explicit notes paths outside ignored `.local` before reading or summarizing the file.
- `tools\Test-LocalReleaseAcceptanceSummary.cmd` / `.ps1` now verifies an explicit non-`.local` path exits with the ignored-path guard and does not print the normal summary boundary.
- The summary regression now compact-normalizes captured child PowerShell output for contains/not-contains assertions, so runner-style path wrapping does not turn the ignored-path guard into a false failure.
- Existing default selection remains unchanged: complete accepted notes are still selected from `.local\release-acceptance`.

Verification:

- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- Fresh CI-shaped checkout at `D:\a\Windows-File-Cleaner\Windows-File-Cleaner` reproduced the pre-patch long-path assertion failure, then passed `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd` and `cmd.exe /c tools\Invoke-MvpPreflight.cmd` after copying in the patched regression script.
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-164509.md"`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- Confirmed `.local\release-acceptance-summary-test` was absent after cleanup.
- Confirmed no `release-acceptance-summary-default-test-*.md` files remained under `.local\release-acceptance`.

ADRs:

- Skipped; this tightens terminal-only validation for existing package acceptance summary tooling under ADR 0020 and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Pending Package Acceptance Notes Refresh After Tooling Hardening

Status: completed

Goal:

- Refresh the pending human package acceptance notes for `.local\releases\windows-file-cleaner-v20260604-121922` after package/readiness tooling commits advanced current `HEAD` beyond the packaged app commit.

Safety profile:

- `terminal-readonly`. `Start-LocalRelease.cmd -ChecklistOnly -WriteAcceptanceNotes` verified the existing ignored package candidate and wrote a new ignored notes file under `.local\release-acceptance`. It did not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- Generated refreshed pending notes at `.local\release-acceptance\release-acceptance-20260604-164509.md`.
- The refreshed notes record verifier evidence at current `HEAD` `1aa5cf1`.
- They intentionally leave commit evidence unrecorded because package commit `e6ac3eb` differs from current `HEAD`; human package acceptance must explicitly record `-RecordCommitMismatch` after accepting that mismatch.
- Updated current package handoff docs and runbooks to point at the refreshed notes path while keeping the accepted package baseline on `bc9b869`.

Verification:

- `cmd.exe /c tools\Start-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-164509.md"`
- Expected incomplete candidate check: `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-164509.md" -RequireComplete` exited `1` with the expected missing commit, normal launch, fixture launch, overall result, and checklist evidence.
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`

ADRs:

- Skipped; this refreshes ignored acceptance evidence and committed handoff docs under ADR 0020 portable-package boundaries. It does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Local Release Acceptance Command Stamping Regression

Status: completed

Goal:

- Convert prior manual command-stamping evidence for generated portable release acceptance notes into a clean-runner regression that runs in MVP preflight.

Safety profile:

- `terminal-readonly`. The regression writes temporary synthetic release folders under `.local\release-acceptance-command-stamping-test`, generates ignored package acceptance notes under `.local\release-acceptance`, invokes `Start-LocalRelease.cmd` in checklist-only skipped-verifier mode, inspects the generated notes, and removes the temporary folders and notes. It does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- Added `tools\Test-LocalReleaseAcceptanceCommandStamping.cmd` / `.ps1`.
- The regression verifies generated notes stamp actual `-ReleasePath` verifier, checklist, and fixture checklist commands.
- It verifies behind-current-`HEAD` notes omit `-RequireCurrentCommit` from stamped verifier/checklist command lines and include the guarded `-RecordCommitMismatch` recorder guidance.
- It verifies current-commit notes include `-RequireCurrentCommit` in stamped verifier/checklist command lines and do not include mismatch guidance.
- `tools\Invoke-MvpPreflight.cmd` now runs this regression by default before the accepted local release selection regression, with `-SkipLocalReleaseAcceptanceCommandStampingCheck` for focused loops.

Verification:

- `cmd.exe /c tools\Test-LocalReleaseAcceptanceCommandStamping.cmd`
- Temporary no-space-path simulation via `subst W: "D:\Codex\Windows File Cleaner"` and `cmd.exe /c W:\tools\Test-LocalReleaseAcceptanceCommandStamping.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `git diff --check`
- Confirmed `.local\release-acceptance-command-stamping-test` was absent after cleanup.
- Confirmed no generated package acceptance notes from the regression remained under `.local\release-acceptance`.

ADRs:

- Skipped; this adds terminal-only regression coverage for existing package acceptance notes generation under ADR 0020 and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Package Summary Default Selection Regression

Status: completed

Goal:

- Cover default `Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` selection when newer incomplete or malformed-looking acceptance notes exist in the normal ignored notes folder.

Safety profile:

- `terminal-readonly`. The regression writes temporary ignored complete, incomplete, and malformed-looking package acceptance notes under `.local\release-acceptance-summary-test` and `.local\release-acceptance`, invokes the read-only summary helper, and removes the temporary files. It does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- Extended `tools\Test-LocalReleaseAcceptanceSummary.cmd` / `.ps1`.
- The regression now creates default-selection test notes in `.local\release-acceptance`.
- It makes the malformed-looking notes newest and the incomplete notes newer than the complete notes, then verifies default `-RequireComplete` still selects the latest complete notes.
- It verifies the default-selected completed notes do not print pending acceptance next steps or select the incomplete/malformed-looking notes.

Verification:

- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Test-DailyReadinessLatestPackageNotes.cmd`
- `cmd.exe /c tools\Test-AcceptedLocalReleaseSelection.cmd`
- `cmd.exe /c tools\Test-LocalReleaseAcceptanceRecorder.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

ADRs:

- Skipped; this adds terminal-only regression coverage for existing package acceptance summary selection under ADR 0020 and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Package Summary Malformed Notes Regression

Status: completed

Goal:

- Cover malformed-looking portable release acceptance notes in the summary helper so missing checklist structure is reported accurately and `-RequireComplete` fails with explicit blockers.

Safety profile:

- `terminal-readonly`. The regression writes temporary ignored complete, incomplete, and malformed-looking package acceptance notes under `.local\release-acceptance-summary-test`, invokes the read-only summary helper, and removes the temporary files. It does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- `Summarize-LocalReleaseAcceptanceNotes.cmd` now prints `No portable release checklist items were found.` when a notes file has zero checklist sections instead of saying all checklist items passed.
- Extended `tools\Test-LocalReleaseAcceptanceSummary.cmd` / `.ps1`.
- The regression now verifies malformed-looking notes report unknown/missing metadata and evidence, zero checklist totals, guarded verifier guidance, and no `-RecordCommitMismatch` command while verifier evidence is missing.
- It verifies `-RequireComplete` fails malformed-looking notes with missing verifier, commit, normal launch, fixture launch, overall result, and checklist blockers.

Verification:

- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Test-DailyReadinessLatestPackageNotes.cmd`
- `cmd.exe /c tools\Test-AcceptedLocalReleaseSelection.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

ADRs:

- Skipped; this changes terminal summary wording and regression coverage for existing package acceptance notes under ADR 0020 and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Accepted Launcher Malformed Notes Regression

Status: completed

Goal:

- Cover accepted-package launcher selection when a newer malformed-looking acceptance notes file exists, so default daily launch commands stay on completed accepted notes and explicit malformed notes cannot print launch commands.

Safety profile:

- `terminal-readonly`. The regression writes temporary ignored complete, incomplete, and malformed-looking package acceptance notes under `.local\release-acceptance`, writes synthetic print-only package placeholder files under `.local\accepted-release-selection-test`, invokes the accepted launcher with `-PrintOnly -SkipVerify`, and removes the temporary files. It does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- Extended `tools\Test-AcceptedLocalReleaseSelection.cmd` / `.ps1`.
- The regression now creates a newer malformed-looking notes file with a synthetic release folder.
- It verifies explicit malformed-looking notes report missing verifier, commit, launch, overall-result, and checklist evidence before launch-command printing.
- It verifies default accepted-package selection still uses the completed notes and does not select newer incomplete or malformed-looking notes.

Verification:

- `cmd.exe /c tools\Test-AcceptedLocalReleaseSelection.cmd`
- `cmd.exe /c tools\Test-DailyReadinessLatestPackageNotes.cmd`
- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

ADRs:

- Skipped; this adds terminal-only regression coverage for existing accepted-package launcher guardrails under ADR 0020 and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Daily Readiness Latest Package Notes Regression

Status: completed

Goal:

- Cover daily readiness latest package notes visibility so incomplete and malformed-looking candidate notes stay informational and do not replace completed accepted package evidence.

Safety profile:

- `terminal-readonly`. The regression writes temporary ignored complete, incomplete, and malformed-looking package acceptance notes under `.local\release-acceptance`, fixture acceptance notes and an empty Restore Manifest root under `.local\daily-readiness-latest-package-notes-test`, invokes daily readiness with explicit accepted notes, and removes the test files. It does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- Added `tools\Test-DailyReadinessLatestPackageNotes.cmd` and `.ps1`.
- The regression creates older complete accepted notes and newer incomplete candidate notes under ignored `.local\release-acceptance`.
- It verifies accepted package evidence uses the complete notes passed explicitly.
- It verifies the informational latest-notes block selects the newer incomplete notes and prints guarded pending next steps including `-RecordCommitMismatch`.
- It verifies the informational latest-notes block is not treated as a failure, then intentionally stops before accepted launch-command printing by requiring incomplete fixture acceptance notes.
- It also verifies a newer malformed-looking latest notes file is selected by the informational block, reports missing verifier/checklist evidence, and still reaches the fixture-note stop before accepted launch-command printing.
- `Invoke-MvpPreflight.cmd` now runs the daily readiness latest package notes regression by default after the daily readiness fixture acceptance notes regression.
- Added `-SkipDailyReadinessLatestPackageNotesCheck` for focused local loops.

Verification:

- `cmd.exe /c tools\Test-DailyReadinessLatestPackageNotes.cmd`
- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Test-DailyReadinessFixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

ADRs:

- Skipped; this adds terminal-only regression coverage for existing daily readiness package acceptance evidence and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Daily Readiness Fixture Acceptance Regression

Status: completed

Goal:

- Cover optional and strict daily readiness fixture acceptance notes forwarding with synthetic evidence that does not depend on ignored real package artifacts.

Safety profile:

- `terminal-readonly`. The regression writes temporary ignored synthetic package files, package acceptance notes, fixture acceptance notes, and an empty Restore Manifest root under `.local\daily-readiness-fixture-acceptance-test`, invokes daily readiness with explicit paths, and removes the test folder. It does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- Added `tools\Test-DailyReadinessFixtureAcceptanceNotes.cmd` and `.ps1`.
- The regression creates a verifier-valid synthetic local release package and complete package acceptance notes under ignored `.local`.
- It verifies strict daily readiness fails incomplete fixture notes during the fixture-notes step and before accepted launch-command printing.
- It verifies optional incomplete fixture notes print recorder guidance and allow the print-only daily readiness flow to continue.
- It verifies complete required fixture notes pass daily readiness with accepted launch commands still print-only and Restore Manifest summary pointed at the synthetic empty root.
- `Invoke-MvpPreflight.cmd` now runs the daily readiness fixture acceptance notes regression by default after the standalone fixture acceptance notes regression.
- Added `-SkipDailyReadinessFixtureAcceptanceCheck` for focused local loops.

Verification:

- `cmd.exe /c tools\Test-DailyReadinessFixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Test-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

ADRs:

- Skipped; this adds terminal-only regression coverage for existing daily readiness and fixture acceptance notes tooling and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Fixture Acceptance Notes Regression

Status: completed

Goal:

- Fold fixture acceptance notes summary and recorder behavior into default MVP preflight so formal fixture notes stay human-owned and completion-checked.

Safety profile:

- `terminal-readonly`. The regression writes temporary ignored fixture acceptance notes under `.local\fixture-acceptance-notes-test`, invokes summary and recorder tooling against those notes, and removes the test folder. It does not launch WPF, create fixtures, scan real-profile files, move, restore, delete, approve cleanup, create shortcuts, install anything, or create cleanup history.

Changes:

- Added `tools\Test-FixtureAcceptanceNotes.cmd` and `.ps1`.
- The regression verifies incomplete fixture notes summaries print recording guidance and that `-RequireComplete` fails with preflight, worktree, overall result, and checklist blockers.
- It verifies `Record-FixtureAcceptanceNotes.cmd` requires `-RecordManualAcceptance`.
- It verifies `-WhatIf` leaves notes unchanged.
- It verifies explicit manual acceptance recording completes synthetic ignored notes and passes `Summarize-FixtureAcceptanceNotes.cmd -RequireComplete`.
- `Invoke-MvpPreflight.cmd` now runs the fixture acceptance notes regression by default after the fixture checklist.
- Added `-SkipFixtureAcceptanceNotesCheck` for focused local loops.

Verification:

- `cmd.exe /c tools\Test-FixtureAcceptanceNotes.cmd`
- `cmd.exe /c tools\Test-AcceptedLocalReleaseSelection.cmd`
- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Test-LocalReleaseAcceptanceRecorder.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

ADRs:

- Skipped; this adds terminal-only regression coverage for existing fixture acceptance notes tooling and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Accepted Local Release Selection Regression

Status: completed

Goal:

- Fold accepted-package launcher note selection into default MVP preflight so newer incomplete candidate notes cannot shadow the completed accepted baseline.

Safety profile:

- `terminal-readonly`. The regression writes temporary ignored acceptance notes under `.local\release-acceptance`, writes synthetic package placeholder files under `.local\accepted-release-selection-test`, invokes the accepted launcher with `-PrintOnly -SkipVerify`, and removes the temporary files. It does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- Added `tools\Test-AcceptedLocalReleaseSelection.cmd` and `.ps1`.
- The regression creates a newer incomplete notes file and an older complete notes file, then verifies default accepted launcher selection uses the complete notes.
- It verifies an explicit incomplete notes path exits before launch-command printing and reports the missing evidence blockers.
- It uses a synthetic package placeholder only with `-PrintOnly -SkipVerify`, so clean runners do not need ignored package artifacts.
- `Invoke-MvpPreflight.cmd` now runs the accepted local release selection regression by default before the package acceptance summary regression.
- Added `-SkipAcceptedLocalReleaseSelectionCheck` for focused local loops.

Verification:

- `cmd.exe /c tools\Test-AcceptedLocalReleaseSelection.cmd`
- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Test-LocalReleaseAcceptanceRecorder.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

ADRs:

- Skipped; this adds terminal-only regression coverage for accepted package launcher selection under ADR 0020 and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Local Release Acceptance Recorder Regression

Status: completed

Goal:

- Fold the portable package acceptance recorder guardrails into default MVP preflight so a future candidate cannot be recorded or promoted through a weakened notes path.

Safety profile:

- `terminal-readonly`. The regression writes temporary ignored acceptance notes under `.local\release-acceptance-recording-test`, invokes recorder and summary tooling against those notes, and removes the test folder. It does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- Added `tools\Test-LocalReleaseAcceptanceRecorder.cmd` and `.ps1`.
- The regression asserts `Record-LocalReleaseAcceptanceNotes.cmd` requires `-RecordManualAcceptance`.
- It asserts missing verifier evidence fails without writing.
- It asserts missing commit evidence fails without writing unless `-RecordCommitMismatch` is explicit.
- It asserts `-WhatIf` leaves notes unchanged.
- It asserts an explicit mismatch recording can complete synthetic ignored notes and pass `Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`.
- `Invoke-MvpPreflight.cmd` now runs the recorder regression by default after the local release acceptance summary regression.
- Added `-SkipLocalReleaseAcceptanceRecorderCheck` for focused local loops.

Verification:

- `cmd.exe /c tools\Test-LocalReleaseAcceptanceRecorder.cmd`
- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

ADRs:

- Skipped; this adds terminal-only regression coverage for existing package acceptance recorder guardrails under ADR 0020 and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Synthetic Restore Manifest-Only Mode Guard Regression

Status: completed

Goal:

- Ensure the focused synthetic Restore Manifest-only modes remain regression-only and cannot be used as casual bypasses for accepted-package or next-batch evidence.

Safety profile:

- `terminal-readonly`. The checks call readiness scripts with invalid synthetic arguments and write temporary ignored synthetic Restore Manifests under `.local` for the existing positive paths. They do not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, write real Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- `tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd` now asserts `Invoke-DailyLocalReadiness.cmd -SyntheticRestoreManifestOnly` rejects a missing Quarantine Root, a Quarantine Root outside ignored `.local`, and package/fixture acceptance notes parameters.
- `tools\Test-RealProfileNextBatchStopGuard.cmd` now asserts `Invoke-RealProfileQuarantineReadiness.cmd -SyntheticRestoreManifestOnly` requires `-SkipMvpPreflight`, rejects a missing Quarantine Root, rejects a Quarantine Root outside ignored `.local`, and rejects package/fixture acceptance notes parameters.
- Normal daily readiness and normal next-batch readiness remain unchanged.

Verification:

- `cmd.exe /c tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd`
- `cmd.exe /c tools\Test-RealProfileNextBatchStopGuard.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`

ADRs:

- Skipped; this adds terminal-only negative regression coverage for existing focused test modes and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real readiness behavior.

### 2026-06-04: Daily Readiness Exact-Profile Undo Spotlight

Status: completed

Goal:

- Make the current exact-profile undo-work stop state visible in the default daily readiness command without requiring a separate Restore Manifest command.

Safety profile:

- `terminal-readonly`. The spotlight reads Restore Manifest summaries only. No WPF launch, scan, real-profile movement, restore, delete, approval, Restore Manifest writes, package promotion, shortcut, install, or cleanup history.

Changes:

- `Invoke-DailyLocalReadiness.cmd` now prints an `Exact-profile undo-work stop state` section after the broad Restore Manifest summary.
- The spotlight uses `-CleanupScope "C:\Users\moxhe" -UndoWorkOnly`.
- `-ShowRestoreEntries` forwards to the spotlight when requested.
- Updated compact runbooks, current state, and feature notes.

Verification:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this adds terminal-only visibility for existing Restore Manifest evidence and does not change product behavior, cleanup execution, restore execution, persistence, deployment, or package acceptance policy.

### 2026-06-04: Daily Readiness Exact-Profile Undo Spotlight Regression

Status: completed

Goal:

- Cover the daily readiness exact-profile undo-work stop-state spotlight in normal MVP preflight so future changes do not accidentally mix fixture-scope undo work into the final exact-profile readout.

Safety profile:

- `terminal-readonly`. The regression writes temporary ignored synthetic Restore Manifests under `.local\daily-readiness-undo-spotlight-test`, removes them, and does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, write real Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- Added `tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd` and `.ps1`.
- The regression creates one synthetic exact `C:\Users\moxhe` undo-work manifest and one synthetic fixture-scope undo-work manifest under the same ignored Quarantine Root.
- The test asserts broad daily readiness sees both manifests and the final exact-profile spotlight displays only the exact-profile manifest.
- `Invoke-MvpPreflight.cmd` now runs the regression by default before `git diff --check`.
- Added `-SkipDailyReadinessUndoSpotlightCheck` for focused preflight loops.

Verification:

- `cmd.exe /c tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check` passed with expected CRLF warnings only.
- Confirmed `.local\daily-readiness-undo-spotlight-test` was absent after cleanup.

ADRs:

- Skipped; this extends terminal-only verification coverage for existing daily readiness and Restore Manifest summary tooling without changing product behavior, cleanup execution, restore execution, persistence, deployment, or package acceptance policy.

### 2026-06-04: Daily Readiness Exact-Profile Undo Spotlight Clean-Runner Regression

Status: completed

Goal:

- Keep the MVP preflight spotlight regression portable for CI and clean clones that do not have ignored accepted-package notes or local package folders.

Safety profile:

- `terminal-readonly`. This changes a focused synthetic Restore Manifest-only daily readiness mode and the regression that uses it. The regression writes temporary ignored synthetic Restore Manifests under `.local\daily-readiness-undo-spotlight-test`, removes them, and does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, write real Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- Added `Invoke-DailyLocalReadiness.cmd -SyntheticRestoreManifestOnly` for focused regression use.
- The mode requires an explicit ignored `.local` Quarantine Root and refuses package or fixture acceptance notes parameters.
- The spotlight regression now uses that mode and asserts accepted-package evidence sections are skipped.
- Normal daily readiness remains unchanged: it still verifies accepted package notes and prints accepted launch commands before Restore Manifest evidence.

Verification:

- `cmd.exe /c tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check` passed with expected CRLF warnings only.
- Confirmed `.local\daily-readiness-undo-spotlight-test` was absent after cleanup.

ADRs:

- Skipped; this narrows a terminal-only regression dependency and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real daily readiness behavior.

### 2026-06-04: Real-Profile Next-Batch Stop Guard Clean-Runner Regression

Status: completed

Goal:

- Keep the MVP preflight next-batch stop-guard regression portable for CI and clean clones that do not have ignored accepted-package notes or local package folders.

Safety profile:

- `terminal-readonly`. This changes a focused synthetic Restore Manifest-only readiness mode and the regression that uses it. The regression writes temporary ignored synthetic Restore Manifests under `.local\real-profile-next-batch-stop-guard-test`, removes them, and does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, write real Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- Added `Invoke-RealProfileQuarantineReadiness.cmd -SyntheticRestoreManifestOnly` for focused regression use.
- The mode requires `-SkipMvpPreflight` and an explicit ignored `.local` Quarantine Root, and refuses package or fixture acceptance notes parameters.
- The next-batch stop-guard regression now uses that mode for its clear synthetic path and asserts accepted-package evidence sections are skipped.
- Normal next-batch readiness remains unchanged: it still uses full MVP preflight, daily accepted-package evidence, and Fixture Acceptance Notes status before any user-facing review evidence.

Verification:

- `cmd.exe /c tools\Test-RealProfileNextBatchStopGuard.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check` passed with expected CRLF warnings only.
- Confirmed `.local\real-profile-next-batch-stop-guard-test` was absent after cleanup.

ADRs:

- Skipped; this narrows a terminal-only regression dependency and does not change product behavior, cleanup execution, restore execution, persistence, deployment, package acceptance policy, or real next-batch readiness behavior.

### 2026-06-04: MVP Preflight Next-Batch Stop Guard Regression

Status: completed

Goal:

- Fold the real-profile next-batch stop guard regression into normal MVP preflight so the synthetic early-stop coverage is not a standalone command people can forget before future real-profile scan review evidence.

Safety profile:

- `terminal-readonly`. The preflight step writes temporary ignored synthetic Restore Manifests under `.local\real-profile-next-batch-stop-guard-test`, removes them, and does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, write real Restore Manifests, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- `Invoke-MvpPreflight.cmd` now runs `Test-RealProfileNextBatchStopGuard.ps1` by default after the local release acceptance summary regression and before `git diff --check`.
- Added `-SkipRealProfileNextBatchStopGuardCheck` for focused preflight loops.
- Updated compact runbooks, handoff docs, and active feature notes to show the new default preflight coverage.

Verification:

- `cmd.exe /c tools\Test-RealProfileNextBatchStopGuard.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this extends terminal-only verification coverage for existing ADR 0017, ADR 0018, and ADR 0019 stop-state tooling and does not change product behavior, cleanup execution, restore execution, persistence, deployment, or package acceptance policy.

### 2026-06-04: Real-Profile Next-Batch Early Undo Guard

Status: completed

Goal:

- Stop the real-profile next-batch evidence preset before MVP preflight when displayed Restore Manifest undo work is present, so a blocked run cannot produce fresh preflight evidence that looks like movement readiness.

Safety profile:

- `terminal-readonly`. The guard reads Restore Manifest summaries only. The regression writes temporary ignored synthetic Restore Manifests under `.local\real-profile-next-batch-stop-guard-test`, then removes them. No WPF launch, scan, real-profile movement, restore, delete, approval, real Restore Manifest write, package promotion, shortcut, install, or cleanup history.

Changes:

- `Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` now runs an early displayed undo-work stop check before MVP preflight.
- Added `tools\Test-RealProfileNextBatchStopGuard.cmd` and `.ps1`.
- The regression proves a synthetic `Moved` exact-profile manifest exits before full preflight or daily readiness, while a synthetic `Restored` exact-profile manifest can continue through the skipped-preflight preset.
- Updated compact runbooks, handoff docs, and active feature notes.

Verification:

- `cmd.exe /c tools\Test-RealProfileNextBatchStopGuard.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this strengthens terminal-only enforcement around existing ADR 0017, ADR 0018, and ADR 0019 gates without changing product behavior, cleanup execution, restore execution, persistence, deployment, or package acceptance policy.

Notes:

- The real current next-batch review was not rerun as movement evidence because exact-profile displayed undo work is present.

### 2026-06-04: MVP Preflight Release Acceptance Summary Regression

Status: completed

Goal:

- Fold the local release acceptance summary regression check into the normal MVP preflight path so package-acceptance next-step output is covered before future real-profile scan review evidence.

Safety profile:

- `terminal-readonly`. The preflight step writes temporary ignored notes under `.local\release-acceptance-summary-test`, removes them after the regression, and does not launch WPF, scan real-profile files, move, restore, delete, approve cleanup, promote a package, create shortcuts, install anything, or create cleanup history.

Changes:

- `Invoke-MvpPreflight.cmd` now runs `Test-LocalReleaseAcceptanceSummary.ps1` by default after fixture checklist output and before `git diff --check`.
- Added `-SkipLocalReleaseAcceptanceSummaryCheck` for focused preflight loops.
- Updated compact runbooks, handoff docs, and active feature notes to show the new default preflight coverage.

Verification:

- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore`
- `cmd.exe /c tools\Invoke-MvpPreflight.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this extends terminal-only verification coverage for existing portable-package acceptance tooling and does not change product behavior, cleanup execution, restore execution, persistence, deployment, or package acceptance policy.

### 2026-06-04: Local Release Acceptance Summary Regression Check

Status: completed

Goal:

- Add a targeted regression check for package acceptance summary output, especially the guarded next-step block for incomplete notes.

Safety profile:

- `terminal-readonly`. The check writes temporary ignored notes under `.local\release-acceptance-summary-test`, runs read-only summaries, and removes its test files. No WPF launch, scan, movement, restore, deletion of real-profile files, approval, installed shortcut, installer behavior, package promotion, or cleanup history.

Changes:

- Added `tools\Test-LocalReleaseAcceptanceSummary.cmd` and `.ps1`.
- The test synthesizes incomplete and complete portable release acceptance notes under ignored `.local`.
- It asserts incomplete notes print `Pending acceptance next steps`, `-RecordCommitMismatch`, and the no-launch/no-scan/no-cleanup-history boundary.
- It asserts completed notes pass `-RequireComplete` without printing pending next steps.

Verification:

- `cmd.exe /c tools\Test-LocalReleaseAcceptanceSummary.cmd`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- Confirmed `.local\release-acceptance-summary-test` was absent after the test cleaned up.
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this adds terminal-only regression coverage for existing portable-package acceptance tooling and does not change product, cleanup, restore, persistence, or deployment behavior.

### 2026-06-04: Package Acceptance Summary Next Steps

Status: completed

Goal:

- Make incomplete package acceptance summaries print the exact next commands while preserving manual acceptance and package-promotion boundaries.

Safety profile:

- `terminal-readonly`. This packet changes terminal output and docs only. No WPF launch, scan, movement, restore, deletion, approval, installed shortcut, installer behavior, package promotion, or cleanup history.

Changes:

- `Summarize-LocalReleaseAcceptanceNotes.cmd` now prints `Pending acceptance next steps` when the selected notes are incomplete.
- The next-step block prints the guarded recorder command only after verifier evidence is recorded, and uses `-RecordCommitMismatch` when commit evidence is not recorded.
- The block repeats summary and completion-check commands and says the human package acceptance pass must happen before recording manual acceptance.
- Completed accepted notes remain unchanged: no pending next-step block is printed.

Verification:

- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md"`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- Expected incomplete candidate check: `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md" -RequireComplete` exited `1`.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this is terminal guidance only and preserves ADR 0020 portable-package boundaries.

### 2026-06-04: Daily Readiness Latest Package Notes

Status: completed

Goal:

- Make pending package acceptance state visible from the daily terminal readiness command without promoting the pending candidate or launching WPF.

Safety profile:

- `terminal-readonly`. This packet changes terminal output and docs only. No WPF launch, scan, movement, restore, deletion, approval, installed shortcut, installer behavior, or cleanup history.

Changes:

- `Invoke-DailyLocalReadiness.cmd` now prints an informational `Latest package acceptance notes` block after the completed accepted package evidence block.
- The accepted package evidence block still uses `-RequireComplete`, so incomplete candidate notes do not replace the accepted `bc9b869` baseline.
- The latest-notes block is non-fatal; if a future ignored notes file is malformed, accepted package readiness still relies on completed accepted notes only.
- Daily-use docs and read-first handoff docs now call out that incomplete candidate notes are informational.

Verification:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `git diff --check`

ADRs:

- Skipped; this preserves ADR 0020 by keeping accepted package commands as the daily path and not creating shortcuts, installers, or package promotion behavior.

### 2026-06-04: Pending Package Acceptance Notes Refresh

Status: completed

Goal:

- Refresh the pending candidate acceptance notes after docs-only `HEAD` advanced beyond the packaged app commit, without launching WPF or changing app behavior.

Safety profile:

- `terminal-readonly`. `Start-LocalRelease.cmd -ChecklistOnly -WriteAcceptanceNotes` wrote ignored `.local\release-acceptance` notes only after read-only package verification. No WPF launch, scan, movement, restore, deletion, approval, installed shortcut, installer behavior, or cleanup history.

Changes:

- Generated refreshed pending notes at `.local\release-acceptance\release-acceptance-20260604-134337.md`.
- Refreshed notes stamp exact `-ReleasePath` verifier, checklist, and fixture checklist commands for `.local\releases\windows-file-cleaner-v20260604-121922`.
- Refreshed notes keep verifier evidence recorded and leave commit evidence unrecorded until the human explicitly accepts the expected `e6ac3eb` package versus `bb82b29` docs-only `HEAD` mismatch with `-RecordCommitMismatch`.
- Kept the accepted package baseline on `.local\releases\windows-file-cleaner-v20260602-011556` at `bc9b869`.

Verification:

- `cmd.exe /c tools\Start-LocalRelease.cmd -ReleasePath ".local\releases\windows-file-cleaner-v20260604-121922" -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md"`
- Expected incomplete candidate check: `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -Path ".local\release-acceptance\release-acceptance-20260604-134337.md" -RequireComplete` exited `1` with the expected missing commit, human launch, overall result, and checklist evidence.
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` still selected the completed accepted `bc9b869` notes.
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly` still printed the accepted `bc9b869` launch command without launching WPF.
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this refreshes ignored acceptance evidence and docs only under ADR 0020 portable-package boundaries.

### 2026-06-04: Startup Context Compaction

Status: completed

Goal:

- Reduce thread lag by shrinking read-first documentation while preserving detailed reference material.

Safety profile:

- `docs-only`. No WPF launch, scan, movement, restore, deletion, approval, installed shortcut, installer behavior, or cleanup history.

Changes:

- Preserved the full prior domain context as `docs/domain/context-reference.md`.
- Preserved the full prior glossary as `docs/domain/glossary-reference.md`.
- Preserved the full prior README as `docs/operations/readme-full-reference.md`.
- Preserved the full prior roadmap as `docs/features/archive/2026-06-01-live-product-readiness-roadmap-history.md`.
- Preserved the pre-compaction progress log as `.codex/archive/progress-2026-06-04-pre-context-compaction.md`.
- Replaced read-first docs with compact active summaries.
- Tightened startup instructions so domain/reference docs are opened only when relevant.

Verification:

- Read-first size scan before compaction: 11 files, 337,831 chars, about 84,458 approximate tokens.
- Read-first size scan after compaction: 11 files, 55,911 chars, about 13,978 approximate tokens.
- Active feature brief scan before compaction: 17 listed files, 88,034 chars, about 22,008 approximate tokens.
- Active feature brief scan after compaction: 5 active feature files, 15,275 chars, about 3,819 approximate tokens.
- Stale read-first instruction search found no active instruction still requiring bulk reads of both domain reference docs; the only stale bulk-read prompt match was in archived handoff evidence.
- Active long-line scan found no lines over 900 characters in the reviewed startup docs.
- `git diff --check` passed with expected CRLF warnings only.

ADRs:

- Skipped; this reorganizes documentation surfaces and does not introduce product, persistence, security, deployment, data-model, or core UX behavior.

### Current Live Product And Package Packets

- `2026-06-04-ci-actions-runtime-maintenance`: GitHub Actions MVP Preflight now uses Node 24-capable official actions and pins CI to `windows-2022` before the `windows-latest` Windows 2025 / Visual Studio 2026 migration.
- `2026-06-04-package-summary-ignored-path-guard-regression`: package acceptance summaries now reject explicit notes paths outside ignored `.local`, covered by MVP preflight summary regression.
- `2026-06-04-pending-package-acceptance-notes-refresh-after-tooling-hardening`: current pending candidate notes refreshed to `.local\release-acceptance\release-acceptance-20260604-164509.md` after package/readiness tooling commits advanced `HEAD` beyond package commit `e6ac3eb`.
- `2026-06-04-local-release-acceptance-command-stamping-regression`: MVP preflight now covers generated package acceptance notes command stamping for actual release paths, current-commit evidence, and guarded commit-mismatch recorder guidance.
- `2026-06-04-package-summary-default-selection-regression`: MVP preflight now covers default package acceptance summary selection with newer incomplete and malformed-looking notes under ignored `.local\release-acceptance`.
- `2026-06-04-package-summary-malformed-notes-regression`: MVP preflight now covers malformed-looking package acceptance summaries with temporary ignored notes and accurate missing-checklist wording.
- `2026-06-04-accepted-launcher-malformed-notes-regression`: MVP preflight now covers accepted-package launcher default selection and explicit malformed-looking-notes rejection with temporary ignored notes and synthetic print-only package files.
- `2026-06-04-daily-readiness-latest-package-notes-regression`: MVP preflight now covers daily readiness latest package notes informational behavior with temporary ignored complete, incomplete, and malformed-looking package acceptance notes.
- `2026-06-04-daily-readiness-fixture-acceptance-regression`: MVP preflight now covers optional and strict daily readiness fixture-note forwarding with synthetic package and Restore Manifest evidence under ignored `.local`.
- `2026-06-04-fixture-acceptance-notes-regression`: MVP preflight now covers fixture notes summary completion blockers, recorder manual-intent guard, `-WhatIf` no-write behavior, and synthetic completion.
- `2026-06-04-accepted-local-release-selection-regression`: MVP preflight now covers accepted-package launcher default selection and explicit incomplete or malformed-looking notes rejection with temporary ignored notes and synthetic print-only package files.
- `2026-06-04-local-release-acceptance-recorder-regression`: MVP preflight now covers package acceptance recorder guardrails for missing manual intent, missing verifier evidence, missing commit evidence without explicit mismatch acceptance, and `-WhatIf` no-write behavior.
- `2026-06-04-synthetic-restore-manifest-mode-guard-regression`: focused synthetic readiness modes now have negative guard coverage for missing/outside `.local` roots, acceptance-note params, and next-batch missing `-SkipMvpPreflight`.
- `2026-06-04-local-release-acceptance-command-stamping`: completed and pushed at `5a4115e`.
- `2026-06-04-real-profile-next-batch-stop-guard-clean-runner-regression`: MVP preflight now covers the next-batch stop guard with ignored synthetic Restore Manifests without depending on local accepted-package evidence.
- `2026-06-04-daily-readiness-exact-profile-undo-spotlight-clean-runner-regression`: MVP preflight covers the exact-profile undo-work spotlight with ignored synthetic Restore Manifests without depending on local accepted-package evidence.
- `2026-06-04-daily-readiness-exact-profile-undo-spotlight-regression`: MVP preflight covers the exact-profile undo-work spotlight with ignored synthetic Restore Manifests.
- `2026-06-04-daily-readiness-exact-profile-undo-spotlight`: default daily readiness spotlights exact-profile undo-work stop state.
- `2026-06-04-mvp-preflight-next-batch-stop-guard-regression`: MVP preflight now runs the real-profile next-batch stop guard regression by default.
- `2026-06-04-real-profile-next-batch-early-undo-guard`: next-batch evidence now stops before MVP preflight when displayed undo work exists.
- `2026-06-04-mvp-preflight-release-summary-regression`: MVP preflight now runs the package acceptance summary regression check by default.
- `2026-06-04-local-release-acceptance-summary-regression-check`: added targeted temporary-note regression coverage for summary next-step behavior, now including malformed-looking notes with missing checklist structure.
- `2026-06-04-package-acceptance-summary-next-steps`: incomplete package acceptance summaries print guarded recorder and recheck commands.
- `2026-06-04-daily-readiness-latest-package-notes`: daily readiness surfaces the latest package acceptance notes as informational context.
- `2026-06-04-pending-package-acceptance-notes-refresh`: earlier refreshed pending candidate notes at `.local\release-acceptance\release-acceptance-20260604-134337.md`; current pending notes are now `.local\release-acceptance\release-acceptance-20260604-164509.md`.
- `2026-06-04-local-release-recorder-commit-evidence-guard`: recorder blocks missing verifier evidence and requires explicit commit-mismatch recording.
- `2026-06-04-accepted-package-complete-notes-selection`: accepted-package helpers select latest complete notes by default.
- `2026-06-04-verified-portable-package-candidate`: candidate package verified but pending human acceptance.
- `2026-06-04-real-profile-quarantine-inline-status-wording`: WPF wording fix completed.
- `2026-06-04-second-real-profile-quarantine-batch`: second tiny exact-profile WPF Quarantine batch completed by user click.

## Archived Evidence

- `.codex/archive/progress-2026-05-2026-06.md`: historical progress log and completed packet evidence through `3dad056`.
- `.codex/archive/progress-2026-06-04-pre-context-compaction.md`: completed packet evidence through release acceptance command stamping.
- `docs/codex/archive/thread-handoff-2026-06-02.md`: previous long-form handoff and startup prompt.
