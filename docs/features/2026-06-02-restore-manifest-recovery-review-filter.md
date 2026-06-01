# Feature: Restore Manifest Recovery Review Filter

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make recovery-review debt easy to inspect from the terminal without adding broad restore, cleanup history, or any file movement.

## Non-goals

- Do not launch WPF.
- Do not scan fixture or real-profile files.
- Do not move, restore, delete, write Restore Manifests, approve cleanup, or create cleanup history.
- Do not add broad/all-manifest restore.
- Do not change selected restore, Quarantine, permanent deletion, or history availability.

## User story / job story

As the local app owner, I want to focus Restore Manifest Summary output on manifests that need recovery review, so that failed or partial actions are visible before any future live cleanup work.

## Current behavior

`tools\Summarize-RestoreManifests.cmd` reports all valid action-scoped Restore Manifests under the selected Quarantine Root, including counts for manifests needing recovery review. To inspect only those manifests, the user has to read the full manifest list.

## Desired behavior

The summary helper supports:

- `-RecoveryReviewOnly` to display only valid manifests that need recovery review while keeping aggregate counts for the full selected Quarantine Root.
- `-RequireNoRecoveryReview` to exit non-zero when any valid manifest still needs recovery review.

Both options are read-only terminal evidence only.

## Domain language changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Restore Manifest Summary | Clarified that terminal summary tooling can focus recovery-review manifests or fail a read-only check when recovery-review manifests remain. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether a later dedicated restore/history surface should group recovery-review manifests in WPF.

## Grill notes

### Scenarios discussed

- The first-live real-profile manifest is restored by user report and terminal summary evidence.
- The default `D:\WindowsFileCleanerQuarantine` root also has older failed real-profile attempts that need recovery review.

### Edge cases

- `-RecoveryReviewOnly` should not hide full-root aggregate counts.
- `-RequireNoRecoveryReview` should fail the terminal check without attempting repair, restore, delete, or manifest writes.

### Dependencies between decisions

- ADR 0016 keeps discovered manifests in manifest panes rather than all quarantined history.
- ADR 0019 keeps real-profile restore selected-manifest-only.

## Evidence and validation gate

Evidence gathered:

- User answers:
  - Continue advancing toward a safe live product while preserving safety gates.
  - Do not move, delete, quarantine, or restore real-profile files without explicit approval for the specific action.
- Existing code/docs inspected:
  - `AGENTS.md`
  - `.codex/progress.md`
  - `README.md`
  - `docs/codex/thread-handoff.md`
  - `docs/domain/context.md`
  - `docs/domain/glossary.md`
  - `docs/features/2026-06-01-restore-manifest-summary-tool.md`
  - `docs/features/2026-06-01-live-product-readiness-roadmap.md`
  - ADR 0016 and ADR 0019
  - `tools/Summarize-RestoreManifests.ps1`
- Tests/checks planned:
  - `cmd.exe /c tools\Summarize-RestoreManifests.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAny -RequireNoRecoveryReview`
  - `cmd.exe /c tools\Summarize-RestoreManifests.cmd -RecoveryReviewOnly -ShowEntries`
  - Expected-failure check for `-RequireNoRecoveryReview` against the default root while recovery-review manifests remain.
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add an automatic repair action.
- Do not add all-manifest restore.
- Do not treat a terminal failure as cleanup history.

## Decisions made

Small feature-level decisions:

- Add focused flags to the existing summary helper instead of creating a separate script.
- Keep full-root aggregate counts visible even when the displayed manifest list is filtered.

ADR-worthy decisions:

- [x] None. This is read-only terminal evidence for existing Restore Manifest Summary behavior and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Implementation plan

1. Add `-RecoveryReviewOnly` and `-RequireNoRecoveryReview` to `tools/Summarize-RestoreManifests.ps1`.
2. Update README, domain docs, glossary, and relevant feature briefs.
3. Record the packet in `.codex/progress.md` and handoff.
4. Run narrow read-only checks.

## Files expected to change

Expected:

- `tools/Summarize-RestoreManifests.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-restore-manifest-summary-tool.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-restore-manifest-recovery-review-filter.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Review output wording to confirm it remains read-only and does not imply repair or restore.

Automated tests:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAny -RequireNoRecoveryReview`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -RecoveryReviewOnly -ShowEntries` reported 10 valid manifests, 2 matching the recovery-review display filter, and showed both older failed real-profile `DXCache` attempts with entry-level error evidence.
- Expected-failure check for `-RequireNoRecoveryReview` against the default root returned exit code 1 while 2 recovery-review manifests remain.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- The check can fail on the user's real Quarantine Root because older failed manifests are intentionally preserved as recovery evidence.

Assumptions:

- Making recovery-review manifests more visible is safer than hiding them in full manifest output.

## Completion notes

Completed on: 2026-06-02

What changed:

- Added `-RecoveryReviewOnly` to display only manifests that need recovery review.
- Added `-RequireNoRecoveryReview` to fail a read-only terminal check if any valid manifest still needs recovery review.
- Kept aggregate counts for all valid manifests visible even when the display list is filtered.

Files changed:

- `tools/Summarize-RestoreManifests.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-restore-manifest-summary-tool.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-restore-manifest-recovery-review-filter.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAny -RequireNoRecoveryReview`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -RecoveryReviewOnly -ShowEntries`
- Expected-failure check for `-RequireNoRecoveryReview` against the default root while recovery-review manifests remain.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- README, domain docs, glossary, Restore Manifest Summary feature brief, live-product readiness roadmap, this feature brief, handoff, and progress log.

ADRs added or skipped:

- No ADR added. This is read-only terminal evidence for existing Restore Manifest Summary behavior.

Follow-up work:

- Use `-RecoveryReviewOnly -ShowEntries` when inspecting older failed manifests before future live cleanup batches.
- Keep any actual recovery action selected-manifest-only through WPF gates unless a later ADR expands restore/history behavior.

Open questions:

- Whether a later dedicated restore/history surface should group recovery-review manifests in WPF.

Risky assumptions:

- The failed older real-profile attempts should remain visible as recovery-review evidence rather than being hidden or auto-cleaned.

## Follow-up evidence: Exact-profile entry review

Packet `Exact-Profile Recovery Review Entry Evidence` reran `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries` and confirmed the two displayed recovery-review manifests are failed-only NVIDIA `DXCache` directory attempts with moved count `0` and undo work `no`.

- `quarantine-action-draft-20260601110130-9010b9d9`: failed because a descendant `.nvph` file was in use by another process.
- `quarantine-action-draft-20260601103558-b3e00fc2`: failed because source and destination roots differed before the guarded cross-volume fallback existed.

The follow-up also reran exact-profile undo-work evidence; `-UndoWorkOnly` showed zero matches, and `-RequireAnyDisplayed -RequireNoDisplayedUndoWork` passed with 4 exact-profile displayed manifests, displayed undo work `0`, and displayed recovery review `2`.
