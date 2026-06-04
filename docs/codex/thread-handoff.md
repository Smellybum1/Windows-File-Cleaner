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
- Latest docs/workflow packet before this closeout: `1ea1b76 Reduce workflow markdown bloat`
- Latest product/evidence packet before this docs cleanup: `3dad056 Record current-head next-batch review evidence`
- App: C# / WPF / .NET 8 local Windows cleanup reviewer for `C:\Users\moxhe`
- Storage Scan: read-only
- Accepted package: `.local\releases\windows-file-cleaner-v20260602-011556` at commit `bc9b869`
- Accepted notes: `.local\release-acceptance\release-acceptance-20260602-011743.md`

See `docs/codex/current-state.md` for the complete compact snapshot.

## Safety Boundary

Use `docs/codex/safety-profiles.md`.

Default for Codex in fresh threads:

- Use `docs-only` for documentation packets.
- Use `terminal-readonly` for readiness and summary commands.
- Use `real-profile-user-click-only` for any next-batch real-profile review.

Codex must not click real-profile Quarantine, restore, delete, or cleanup execution.

## Best Next Work

The next live-product step can be the user's manual WPF next-batch review if they are ready. Fresh terminal evidence has already passed through `.\tools\Invoke-RealProfileNextBatchReview.cmd` on `c7cb545`.

Keep any next batch:

- exact `C:\Users\moxhe`;
- at most 10 rows and 1 GB;
- Likely safe plus Quarantine candidate only;
- selected-batch-only;
- readiness-gated;
- human-clicked only.

Do not proceed to movement unless the user explicitly chooses a specific tiny batch after WPF readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, and immediate Pre-Execution Revalidation are visible.

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

Current state: main includes the compact workflow-docs cleanup packet at 1ea1b76. The latest product/evidence packet before that cleanup was 3dad056, Record current-head next-batch review evidence. The app is a C#/.NET 8 WPF local Windows cleanup reviewer for C:\Users\moxhe. Storage Scan is read-only. Fixture Quarantine, current-fixture undo, fixture selected restore, exact real-profile selected restore, and first-phase exact real-profile Quarantine exist behind their gates. The first tiny exact real-profile Quarantine and selected restore recovery loop both succeeded by user report.

Fresh current-head next-batch evidence passed with .\tools\Invoke-RealProfileNextBatchReview.cmd on c7cb545: full MVP preflight, accepted package verification with expected package/current-HEAD warning, optional Fixture Acceptance Notes status, exact-profile Restore Manifest display, focused recovery-review and undo-work evidence, and manual WPF checklist printing all completed without WPF launch, real-profile scan, movement, restore, deletion, approval, manifest writes, shortcut creation, installer behavior, or cleanup history.

Next best step: ask the user to do the manual WPF next-batch review if they are ready. Do not click real-profile movement from Codex. Do not guide a Quarantine click unless the user explicitly chooses a specific tiny exact C:\Users\moxhe batch after WPF readiness, exact QUARANTINE, Real-Profile Quarantine Approval Evidence, and immediate Pre-Execution Revalidation are visible.

Accepted package baseline remains .local\releases\windows-file-cleaner-v20260602-011556 at commit bc9b869 with completed ignored acceptance notes .local\release-acceptance\release-acceptance-20260602-011743.md. Use docs/operations/*.md for command detail. Historical packet evidence is archived in .codex/archive/progress-2026-05-2026-06.md and docs/codex/archive/thread-handoff-2026-06-02.md.
```
