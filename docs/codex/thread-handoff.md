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

Load archived evidence only when the task needs historical packet detail.

## Current State

- Repo: `D:\Codex\Windows File Cleaner`
- Branch: `main`
- Latest package/evidence packet: `2026-06-04-verified-portable-package-candidate`
- Latest live-product evidence: `2026-06-04-second-real-profile-quarantine-batch`
- Latest working app packet: `2026-06-04-real-profile-quarantine-inline-status-wording`
- Latest docs/workflow baseline before these packets: `1ea1b76 Reduce workflow markdown bloat`
- App: C# / WPF / .NET 8 local Windows cleanup reviewer for `C:\Users\moxhe`
- Storage Scan: read-only
- Accepted package: `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`
- Accepted notes: `.local\release-acceptance\release-acceptance-20260602-011743.md`
- Verified package candidate pending acceptance: `.local\releases\windows-file-cleaner-v20260604-121922` at commit `e6ac3eb`

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

Read AGENTS.md, docs/codex/current-state.md, .codex/progress.md, docs/codex/safety-profiles.md, README.md, docs/domain/context.md, docs/domain/glossary.md, and relevant active docs/features/ and docs/decisions/ before implementing.

Current state: main includes the second tiny exact real-profile Quarantine evidence, real-profile inline status wording fix, and a verified portable package candidate. The app is a C#/.NET 8 WPF local Windows cleanup reviewer for C:\Users\moxhe. Storage Scan is read-only. Fixture Quarantine, current-fixture undo, fixture selected restore, exact real-profile selected restore, and first-phase exact real-profile Quarantine exist behind their gates. The first tiny exact real-profile Quarantine and selected restore recovery loop both succeeded by user report; the second tiny exact real-profile Quarantine batch also succeeded and currently leaves exact-profile displayed undo work 1.

The second user-clicked WPF batch moved one exact C:\Users\moxhe pip\cache\http-v2 .body file, 28.93 MB, with moved 1, failed 0, Recovery review no. Post-action read-only summary showed 5 of 11 exact-profile manifests, displayed exact-profile undo work 1, displayed exact-profile recovery review 2, and new manifest D:\WindowsFileCleanerQuarantine\actions\quarantine-action-draft-20260604014901-b7b402a2\restore-manifest.json.

Next best step: stop after the second tiny exact real-profile batch. Do not chain another real-profile Quarantine batch while exact-profile displayed undo work is present unless a new Grill with Docs pass decides that outstanding selected-manifest undo work is acceptable. Recovery remains selected-manifest-only with exact RESTORE and immediate selected-restore revalidation if needed.

Accepted package baseline remains .local\releases\windows-file-cleaner-v20260602-011556 at commit bc9b869 with completed ignored acceptance notes .local\release-acceptance\release-acceptance-20260602-011743.md. Verified package candidate .local\releases\windows-file-cleaner-v20260604-121922 at commit e6ac3eb is pending human package acceptance; ignored notes .local\release-acceptance\release-acceptance-20260604-122009.md are incomplete. Use docs/operations/*.md for command detail. Historical packet evidence is archived in .codex/archive/progress-2026-05-2026-06.md and docs/codex/archive/thread-handoff-2026-06-02.md.
```
