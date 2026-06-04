# Thread Handoff

Last updated: 2026-06-04

Use this when starting a fresh Codex thread for this repository.

## Read First

1. `AGENTS.md`
2. `docs/codex/current-state.md`
3. `.codex/progress.md`
4. `docs/codex/safety-profiles.md`
5. Relevant ADRs in `docs/decisions/`
6. Relevant active feature briefs or indexes in `docs/features/`

Load detailed reference docs or archived evidence only when the task needs historical packet detail.

## Current State

- Repo: `D:\Codex\Windows File Cleaner`
- Branch: `main`
- Latest package/evidence packet: `2026-06-04-pending-package-acceptance-notes-refresh-after-tooling-hardening`
- Latest tooling/evidence packet: `2026-06-04-real-profile-selected-restore-trust-helper-preflight-regression`
- Latest docs/workflow packet: `2026-06-04-pending-notes-head-wording-stabilization`
- Latest live-product evidence: `2026-06-04-second-real-profile-quarantine-batch`
- Latest working app packet: `2026-06-04-real-profile-quarantine-inline-status-wording`
- Previous docs/workflow baseline: `1ea1b76 Reduce workflow markdown bloat`
- App: C# / WPF / .NET 8 local Windows cleanup reviewer for `C:\Users\moxhe`
- Storage Scan: read-only
- Accepted package: `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`
- Accepted notes: `.local\release-acceptance\release-acceptance-20260602-011743.md`
- Verified package candidate pending acceptance: `.local\releases\windows-file-cleaner-v20260604-121922` at commit `e6ac3eb`
- Current pending candidate notes: `.local\release-acceptance\release-acceptance-20260604-164509.md`
- Pending candidate notes record verifier evidence from their generation time. Later docs/tooling commits can make notes/current-HEAD status differ again, so use package acceptance summary status lines for live mismatch context instead of treating the recorded notes commit as permanently current.
- Daily readiness shows the latest package acceptance notes as informational context after verifying the completed accepted notes. `-LatestPackageAcceptanceNotesPath` can point that informational block at an explicit ignored notes file for focused checks or pending-note inspection.
- Daily readiness ends with an exact-profile undo-work stop-state spotlight after the broad Restore Manifest summary.
- `tools\Test-RealProfileSelectedRestoreTrustManifestPathGuard.cmd` covers the sacrificial real-profile selected restore trust helper's root and action-layout path guards. It now derives the escaped path's profile segment from the helper's safe preview output, and MVP preflight runs it by default. It runs only with `-WhatIf`, an ignored `.local` Quarantine Root, and committed `README.md` as a non-`.local` rejection target, proving out-of-bound roots and escaped relative paths fail before preview output and without writing Restore Manifests or modifying real-profile files.
- `tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd` covers that spotlight with ignored synthetic Restore Manifests, and MVP preflight now runs it by default before the whitespace diff check. The regression uses the focused `-SyntheticRestoreManifestOnly` daily readiness mode so CI and clean runners do not need ignored accepted-package evidence for this check. It also asserts the synthetic mode rejects missing/outside-`.local` roots and acceptance-note parameters, including explicit latest package notes.
- Incomplete package acceptance summaries print guarded recorder and recheck commands; the current candidate recorder command includes `-RecordCommitMismatch` and remains human-pass-only.
- Completed package acceptance summaries say the evidence is complete and no recorder action is pending.
- Completed fixture acceptance summaries say the evidence is complete and no recorder action is pending.
- Package acceptance summaries print current repository `HEAD`, notes/current-HEAD status, and package/current-HEAD status, so pending candidate notes visibly show whether package/current-HEAD mismatch context is present.
- GitHub Actions MVP Preflight now uses `actions/checkout@v6`, `actions/setup-dotnet@v5`, `windows-2022` for push/pull-request runs, and a manual `workflow_dispatch` runner-image choice for intentional `windows-2025-vs2026` canary runs.
- Push CI run #365 passed on `98d3412`, validating representative current-path evidence after active feature-index entries were added to the documentation consistency regression. Earlier #360 validated the no-input `inputs.runner_image || 'windows-2022'` fallback. Do not update the representative CI evidence for every green docs-only push.
- `docs/operations/ci.md` captures normal push/PR CI behavior, the manual Windows image canary procedure, baseline-change rule, and safety boundary.
- `tools\Test-DocumentationConsistency.cmd` verifies active documentation links, bare active feature-index entries, and latest packet breadcrumbs; MVP preflight now runs it by default before the whitespace diff check.
- `tools\Test-FixtureRootPathGuard.cmd` covers synthetic fixture creation and fixture review root path guards. MVP preflight now runs it by default before fixture dry-run output. It asserts explicit non-`.local` fixture roots fail before fixture writes, checklist output, or WPF launch.
- `tools\Test-FixtureAcceptanceNotes.cmd` covers fixture acceptance summary and recorder behavior with temporary ignored notes, and MVP preflight now runs it by default after the fixture checklist. It asserts incomplete summaries show recording guidance, explicit summary paths outside ignored `.local` are rejected before output or recorder guidance, `-RequireComplete` reports blockers, recorder calls require `-RecordManualAcceptance`, `-WhatIf` does not write, and synthetic explicit recording can complete notes.
- `tools\Test-DailyReadinessFixtureAcceptanceNotes.cmd` covers optional and strict daily readiness fixture-note forwarding with temporary ignored package files, acceptance notes, fixture notes, and an empty Restore Manifest root. MVP preflight now runs it by default after the standalone fixture notes regression. It asserts required incomplete fixture notes fail before launch-command printing, explicit fixture notes paths outside ignored `.local` fail before launch-command printing, optional incomplete notes show recording guidance and continue, and complete required notes pass the normal print-only daily flow.
- `tools\Test-DailyReadinessLatestPackageNotes.cmd` covers daily readiness latest package notes visibility with temporary ignored package notes and fixture notes under its private `.local\daily-readiness-latest-package-notes-test` folder. MVP preflight now runs it by default after the daily readiness fixture acceptance notes regression. It asserts explicit package acceptance notes paths outside ignored `.local` fail during accepted-package evidence before latest-notes or launch-command printing, accepted evidence stays on completed notes while an explicit incomplete candidate is shown as informational context with guarded `-RecordCommitMismatch` next steps, and an explicit malformed-looking notes file reports missing evidence without blocking accepted-package readiness.
- `tools\Test-LocalReleasePathGuards.cmd` covers portable release publisher, verifier, and launcher path guards. MVP preflight now runs it by default before local release acceptance command stamping. It asserts explicit non-`.local` release roots and release paths fail before publisher, verifier, launcher, or launch-command output.
- `tools\Test-LocalReleaseAcceptanceCommandStamping.cmd` covers generated package acceptance notes command stamping with temporary synthetic release folders and generated notes under ignored `.local`. MVP preflight now runs it by default before the accepted local release selection regression. It asserts `-ReleasePath` is stamped into verifier/checklist commands, `-RequireCurrentCommit` is stamped only for current-commit notes, and `-RecordCommitMismatch` guidance appears only for behind-current-`HEAD` notes.
- `tools\Test-AcceptedLocalReleaseSelection.cmd` covers accepted-package launcher selection with temporary ignored notes and synthetic print-only package files, and MVP preflight now runs it by default before the package summary regression. It asserts newer incomplete or malformed-looking notes files do not shadow a completed accepted note, while explicit incomplete or malformed-looking notes paths still stop before launch-command printing. It also asserts explicit accepted launcher notes paths outside ignored `.local` fail before accepted launcher output or launch-command printing.
- `tools\Test-LocalReleaseAcceptanceSummary.cmd` provides targeted temporary-note regression coverage for package acceptance summaries, and MVP preflight now runs it by default before the recorder regression. It covers complete, incomplete, and malformed-looking notes, including missing-checklist wording, `-RequireComplete` blockers, default selection of the latest complete notes when newer incomplete or malformed-looking notes exist, and explicit summary path rejection outside ignored `.local`. Its captured child-output assertions are whitespace-normalized so long runner-style paths can wrap without false failures.
- `tools\Test-LocalReleaseAcceptanceRecorder.cmd` covers the package acceptance recorder guardrails with temporary ignored notes, and MVP preflight now runs it by default after the package summary regression. It asserts missing manual intent, missing verifier evidence, missing commit evidence without explicit mismatch acceptance, `-WhatIf` no-write behavior, and explicit `-RecordCommitMismatch` completion for synthetic notes.
- `Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` checks displayed undo work before MVP preflight, and `tools\Test-RealProfileNextBatchStopGuard.cmd` covers that early stop with ignored synthetic manifests. MVP preflight now runs that regression by default before the whitespace diff check. The regression uses focused `-SyntheticRestoreManifestOnly` readiness so CI and clean runners do not need ignored accepted-package evidence for this check. It also asserts the synthetic mode requires `-SkipMvpPreflight`, rejects missing/outside-`.local` roots, and rejects acceptance-note parameters.
- Accepted-package helpers select the latest complete acceptance notes by default; use explicit notes paths for pending candidate review.
- `Record-LocalReleaseAcceptanceNotes.cmd` requires verifier evidence and explicit `-RecordCommitMismatch` when package/current-HEAD mismatch evidence is not already recorded.
- Generated package acceptance notes stamp the actual `-ReleasePath` verifier/checklist commands and include `-RequireCurrentCommit` only when that switch created the notes.

See `docs/codex/current-state.md` for the complete compact snapshot.

## Safety Boundary

Use `docs/codex/safety-profiles.md`.

Default for Codex in fresh threads:

- Use `docs-only` for documentation packets.
- Use `terminal-readonly` for readiness and summary commands.
- Use `real-profile-user-click-only` for any next-batch real-profile review.

Codex must not click real-profile Quarantine, restore, delete, or cleanup execution.

## Best Next Work

Stop after the 2026-06-04 second tiny exact real-profile batch. The new exact-profile Restore Manifest still has selected-manifest undo work available, so do not chain another real-profile Quarantine batch or treat another next-batch review as movement evidence unless a new Grill with Docs pass decides that outstanding selected-manifest undo work is acceptable.

If recovery is needed, use selected-manifest restore only for the exact selected `C:\Users\moxhe` Restore Manifest after selected manifest readiness, exact `RESTORE`, and immediate selected-restore revalidation pass. If packaging is the next focus, complete the human package acceptance pass for `.local\releases\windows-file-cleaner-v20260604-121922` before promoting it over the accepted `bc9b869` package baseline.

## Operational Runbooks

- `docs/operations/ci.md`
- `docs/operations/daily-use.md`
- `docs/operations/portable-release.md`
- `docs/operations/manual-fixture-review.md`
- `docs/operations/restore-manifest-review.md`

## Still Unavailable

- Broad/all-manifest real-profile Undo Quarantine.
- Custom/non-exact real-profile Quarantine.
- Custom selected restore.
- Permanent deletion.
- Persisted cleanup history.
- Installed shortcut or installer automation unless the user explicitly asks for an ADR 0020 follow-up packet.

## Startup Prompt

```text
We are continuing Windows File Cleaner in D:\Codex\Windows File Cleaner.

Read AGENTS.md, docs/codex/current-state.md, .codex/progress.md, docs/codex/safety-profiles.md, and relevant active docs/features/ before implementing.

Read docs/domain/context.md and docs/domain/glossary.md for domain, naming, UI wording, cleanup behavior, restore behavior, or product-rule changes. Read relevant docs/decisions/ when touching an ADR-backed decision. Open detailed reference/archive docs only when current context is unclear.

Current state: main includes the second tiny exact real-profile Quarantine evidence, real-profile inline status wording fix, verified portable package candidate, CI Windows image canary, compact CI operations runbook, fixture/package path guard hardening, a real-profile selected restore trust-helper path guard, and CI #365 evidence for the active feature-index documentation consistency regression. The app is a C#/.NET 8 WPF local Windows cleanup reviewer for C:\Users\moxhe. Storage Scan is read-only. Fixture Quarantine, current-fixture undo, fixture selected restore, exact real-profile selected restore, and first-phase exact real-profile Quarantine exist behind their gates. The first tiny exact real-profile Quarantine and selected restore recovery loop both succeeded by user report; the second tiny exact real-profile Quarantine batch also succeeded and currently leaves exact-profile displayed undo work 1.

The second user-clicked WPF batch moved one exact C:\Users\moxhe pip\cache\http-v2 .body file, 28.93 MB, with moved 1, failed 0, Recovery review no. Post-action read-only summary showed 5 of 11 exact-profile manifests, displayed exact-profile undo work 1, displayed exact-profile recovery review 2, and new manifest D:\WindowsFileCleanerQuarantine\actions\quarantine-action-draft-20260604014901-b7b402a2\restore-manifest.json.

Next best step: stop after the second tiny exact real-profile batch. Do not chain another real-profile Quarantine batch while exact-profile displayed undo work is present unless a new Grill with Docs pass decides that outstanding selected-manifest undo work is acceptable. Recovery remains selected-manifest-only with exact RESTORE and immediate selected-restore revalidation if needed. Daily readiness spotlights this exact-profile undo-work stop state, and MVP preflight covers the spotlight with a synthetic regression. The next-batch evidence preset now stops before MVP preflight while displayed undo work exists; do not use that early-stop output as movement evidence.

Accepted package baseline remains .local\releases\windows-file-cleaner-v20260602-011556 at commit bc9b869 with completed ignored acceptance notes .local\release-acceptance\release-acceptance-20260602-011743.md. Accepted-package helpers select latest complete notes by default. Daily readiness and accepted-package launchers keep incomplete or malformed-looking candidate notes from replacing the accepted baseline; explicit incomplete or malformed-looking accepted-launcher notes paths stop before launch-command printing, explicit non-.local accepted-launcher notes paths fail before launch-command printing, explicit fixture creation/review roots must stay under ignored .local, explicit package publish/verify/launch roots and paths must stay under ignored .local, explicit package-summary notes paths must stay under ignored .local, and the selected-restore trust helper path guard runs in MVP preflight with an escaped-path case derived from the helper's safe preview. MVP preflight includes the fixture root path guard, fixture acceptance notes, daily readiness fixture acceptance notes, daily readiness latest package notes, local release path guard, local release acceptance command stamping, accepted local release selection, local release acceptance summary, local release acceptance recorder, real-profile selected restore trust helper path guard, real-profile next-batch stop guard, daily readiness exact-profile undo spotlight, and documentation consistency regression checks; use -SkipFixtureRootPathGuardCheck, -SkipFixtureAcceptanceNotesCheck, -SkipDailyReadinessFixtureAcceptanceCheck, -SkipDailyReadinessLatestPackageNotesCheck, -SkipLocalReleasePathGuardCheck, -SkipLocalReleaseAcceptanceCommandStampingCheck, -SkipAcceptedLocalReleaseSelectionCheck, -SkipLocalReleaseAcceptanceSummaryCheck, -SkipLocalReleaseAcceptanceRecorderCheck, -SkipRealProfileSelectedRestoreTrustHelperPathGuardCheck, -SkipRealProfileNextBatchStopGuardCheck, -SkipDailyReadinessUndoSpotlightCheck, or -SkipDocumentationConsistencyCheck only for focused local loops. GitHub Actions MVP Preflight uses actions/checkout@v6, actions/setup-dotnet@v5, windows-2022 for push/pull-request runs, and a manual workflow_dispatch runner-image canary documented in docs/operations/ci.md. The synthetic Restore Manifest-only modes used inside those focused regressions are guard-tested and are not daily or next-batch evidence substitutes.

Verified package candidate .local\releases\windows-file-cleaner-v20260604-121922 at commit e6ac3eb is pending human package acceptance; refreshed pending notes .local\release-acceptance\release-acceptance-20260604-164509.md are incomplete and should be inspected with an explicit path. They stamp the actual -ReleasePath verifier/checklist commands, record verifier evidence from their generation time, and intentionally leave commit evidence unrecorded until the human accepts the expected package/current-HEAD mismatch. Later docs/tooling commits can make notes/current-HEAD status differ again; use the summary helper status lines for live mismatch context. The summary helper prints the guarded -RecordCommitMismatch recorder command, but it should be used only after the human package acceptance pass.

If new candidate notes are created after docs-only commits, use -RecordCommitMismatch only after intentionally accepting the package/current-HEAD mismatch. Use docs/operations/*.md for command detail, including docs/operations/ci.md for the manual Windows image canary.

Startup docs were compacted on 2026-06-04; detailed prior context lives in docs/domain/*-reference.md, docs/operations/readme-full-reference.md, docs/features/archive/2026-06-01-live-product-readiness-roadmap-history.md, .codex/archive/progress-2026-06-04-pre-context-compaction.md, and older archives. Open those only when current compact docs are insufficient.
```
