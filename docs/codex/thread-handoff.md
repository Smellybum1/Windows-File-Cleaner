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
- Latest package/evidence packet: `2026-06-04-verified-portable-package-candidate`
- Latest tooling/evidence packet: `2026-06-04-daily-readiness-exact-profile-undo-spotlight-regression`
- Latest docs/workflow packet: `2026-06-04-startup-context-compaction`
- Latest live-product evidence: `2026-06-04-second-real-profile-quarantine-batch`
- Latest working app packet: `2026-06-04-real-profile-quarantine-inline-status-wording`
- Previous docs/workflow baseline: `1ea1b76 Reduce workflow markdown bloat`
- App: C# / WPF / .NET 8 local Windows cleanup reviewer for `C:\Users\moxhe`
- Storage Scan: read-only
- Accepted package: `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`
- Accepted notes: `.local\release-acceptance\release-acceptance-20260602-011743.md`
- Verified package candidate pending acceptance: `.local\releases\windows-file-cleaner-v20260604-121922` at commit `e6ac3eb`
- Current pending candidate notes: `.local\release-acceptance\release-acceptance-20260604-134337.md`
- Daily readiness shows the latest package acceptance notes as informational context after verifying the completed accepted notes.
- Daily readiness ends with an exact-profile undo-work stop-state spotlight after the broad Restore Manifest summary.
- `tools\Test-DailyReadinessExactProfileUndoSpotlight.cmd` covers that spotlight with ignored synthetic Restore Manifests, and MVP preflight now runs it by default before the whitespace diff check.
- Incomplete package acceptance summaries print guarded recorder and recheck commands; the current candidate recorder command includes `-RecordCommitMismatch` and remains human-pass-only.
- `tools\Test-LocalReleaseAcceptanceSummary.cmd` provides targeted temporary-note regression coverage for package acceptance summaries, and MVP preflight now runs it by default before the whitespace diff check.
- `Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` checks displayed undo work before MVP preflight, and `tools\Test-RealProfileNextBatchStopGuard.cmd` covers that early stop with ignored synthetic manifests. MVP preflight now runs that regression by default before the whitespace diff check.
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

Current state: main includes the second tiny exact real-profile Quarantine evidence, real-profile inline status wording fix, and a verified portable package candidate. The app is a C#/.NET 8 WPF local Windows cleanup reviewer for C:\Users\moxhe. Storage Scan is read-only. Fixture Quarantine, current-fixture undo, fixture selected restore, exact real-profile selected restore, and first-phase exact real-profile Quarantine exist behind their gates. The first tiny exact real-profile Quarantine and selected restore recovery loop both succeeded by user report; the second tiny exact real-profile Quarantine batch also succeeded and currently leaves exact-profile displayed undo work 1.

The second user-clicked WPF batch moved one exact C:\Users\moxhe pip\cache\http-v2 .body file, 28.93 MB, with moved 1, failed 0, Recovery review no. Post-action read-only summary showed 5 of 11 exact-profile manifests, displayed exact-profile undo work 1, displayed exact-profile recovery review 2, and new manifest D:\WindowsFileCleanerQuarantine\actions\quarantine-action-draft-20260604014901-b7b402a2\restore-manifest.json.

Next best step: stop after the second tiny exact real-profile batch. Do not chain another real-profile Quarantine batch while exact-profile displayed undo work is present unless a new Grill with Docs pass decides that outstanding selected-manifest undo work is acceptable. Recovery remains selected-manifest-only with exact RESTORE and immediate selected-restore revalidation if needed. Daily readiness spotlights this exact-profile undo-work stop state, and MVP preflight covers the spotlight with a synthetic regression. The next-batch evidence preset now stops before MVP preflight while displayed undo work exists; do not use that early-stop output as movement evidence.

Accepted package baseline remains .local\releases\windows-file-cleaner-v20260602-011556 at commit bc9b869 with completed ignored acceptance notes .local\release-acceptance\release-acceptance-20260602-011743.md. Accepted-package helpers select latest complete notes by default. Daily readiness shows latest package acceptance notes as informational context but still keeps incomplete candidate notes from replacing the accepted baseline. MVP preflight includes the local release acceptance summary, real-profile next-batch stop guard, and daily readiness exact-profile undo spotlight regression checks; use -SkipLocalReleaseAcceptanceSummaryCheck, -SkipRealProfileNextBatchStopGuardCheck, or -SkipDailyReadinessUndoSpotlightCheck only for focused local loops.

Verified package candidate .local\releases\windows-file-cleaner-v20260604-121922 at commit e6ac3eb is pending human package acceptance; refreshed pending notes .local\release-acceptance\release-acceptance-20260604-134337.md are incomplete and should be inspected with an explicit path. They stamp the actual -ReleasePath verifier/checklist commands and intentionally leave commit evidence unrecorded until the human accepts the expected package/current-HEAD mismatch. The summary helper prints the guarded -RecordCommitMismatch recorder command, but it should be used only after the human package acceptance pass.

If new candidate notes are created after docs-only commits, use -RecordCommitMismatch only after intentionally accepting the package/current-HEAD mismatch. Use docs/operations/*.md for command detail.

Startup docs were compacted on 2026-06-04; detailed prior context lives in docs/domain/*-reference.md, docs/operations/readme-full-reference.md, docs/features/archive/2026-06-01-live-product-readiness-roadmap-history.md, .codex/archive/progress-2026-06-04-pre-context-compaction.md, and older archives. Open those only when current compact docs are insufficient.
```
