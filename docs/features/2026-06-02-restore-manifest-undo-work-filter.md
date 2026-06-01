# Feature: Restore Manifest Undo Work Filter

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make outstanding undo work easy to inspect from the terminal without adding broad restore, cleanup history, or any file movement.

## Non-goals

- Do not launch WPF.
- Do not scan fixture or real-profile files.
- Do not move, restore, delete, write Restore Manifests, approve cleanup, or create cleanup history.
- Do not add broad/all-manifest restore.
- Do not change selected restore, Quarantine, permanent deletion, or history availability.

## User story / job story

As the local app owner, I want to focus Restore Manifest Summary output on manifests that still have moved entries, so that pending undo/restore work is visible before future live cleanup work.

## Current behavior

`tools\Summarize-RestoreManifests.cmd` reports the count of manifests with undo work, but the displayed manifest list still includes every valid Restore Manifest unless the recovery-review filter is used.

## Desired behavior

The summary helper supports:

- `-UndoWorkOnly` to display only valid manifests with moved entries while keeping aggregate counts for the full selected Quarantine Root.
- `-RequireNoUndoWork` to exit non-zero when any valid manifest still has undo work.

Both options are read-only terminal evidence only.

## Domain language changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Restore Manifest Summary | Clarified that terminal summary tooling can focus undo-work manifests or fail a read-only check when undo work remains. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether a later dedicated restore/history surface should group outstanding undo work in WPF.

## Grill notes

### Scenarios discussed

- The default `D:\WindowsFileCleanerQuarantine` root currently has a small amount of fixture undo work, and older failed real-profile attempts remain recovery-review evidence.
- The first-live real-profile manifest is restored by user report and should not appear as undo work.

### Edge cases

- `-UndoWorkOnly` should not hide full-root aggregate counts.
- `-RequireNoUndoWork` should fail the terminal check without attempting repair, restore, delete, or manifest writes.
- `-UndoWorkOnly` and `-RecoveryReviewOnly` may be combined; display output should then show the intersection of both read-only filters.

### Dependencies between decisions

- ADR 0016 keeps discovered manifests in manifest panes rather than all quarantined history.
- ADR 0019 keeps real-profile restore selected-manifest-only.

## Evidence and validation gate

Evidence gathered:

- User answers:
  - Continue advancing with small safe packets.
  - No real-profile move, delete, quarantine, or restore without explicit approval for the specific action.
- Existing code/docs inspected:
  - `AGENTS.md`
  - `.codex/progress.md`
  - `README.md`
  - `docs/codex/thread-handoff.md`
  - `docs/codex/grill-with-docs.md`
  - `docs/codex/skillopt-inspired-workflow.md`
  - `docs/domain/context.md`
  - `docs/domain/glossary.md`
  - `docs/features/2026-06-01-restore-manifest-summary-tool.md`
  - `docs/features/2026-06-02-restore-manifest-recovery-review-filter.md`
  - `docs/features/2026-06-01-live-product-readiness-roadmap.md`
  - ADR 0011, ADR 0012, and ADR 0019
  - `tools/Summarize-RestoreManifests.ps1`
- Tests/checks planned:
  - `cmd.exe /c tools\Summarize-RestoreManifests.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAny -RequireNoRecoveryReview -RequireNoUndoWork`
  - `cmd.exe /c tools\Summarize-RestoreManifests.cmd -UndoWorkOnly -ShowEntries`
  - Expected-failure check for `-RequireNoUndoWork` against the default root while undo-work manifests remain.
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add an automatic restore action.
- Do not add all-manifest restore.
- Do not treat a terminal failure as cleanup history.

## Decisions made

Small feature-level decisions:

- Add focused undo-work flags to the existing summary helper instead of creating a separate script.
- Keep full-root aggregate counts visible even when the displayed manifest list is filtered.
- Allow recovery-review and undo-work display filters to combine as an intersection.

ADR-worthy decisions:

- [x] None. This is read-only terminal evidence for existing Restore Manifest Summary behavior and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Implementation plan

1. Add `-UndoWorkOnly` and `-RequireNoUndoWork` to `tools/Summarize-RestoreManifests.ps1`.
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
- `docs/features/2026-06-02-restore-manifest-undo-work-filter.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Review output wording to confirm it remains read-only and does not imply restore or cleanup.

Automated tests:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAny -RequireNoRecoveryReview -RequireNoUndoWork`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -UndoWorkOnly -ShowEntries`
- Expected-failure check for `-RequireNoUndoWork` against the default root while undo-work manifests remain.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- The check can fail on the user's real Quarantine Root because fixture or live manifests may intentionally remain moved until the user chooses selected restore/undo.

Assumptions:

- Making undo-work manifests more visible is safer than hiding them in full manifest output.
- Outstanding moved entries should stay visible as recovery evidence rather than being hidden or auto-restored.

## Completion notes

Completed on: 2026-06-02

What changed:

- Added `-UndoWorkOnly` to display only manifests with undo work.
- Added `-RequireNoUndoWork` to fail a read-only terminal check if any valid Restore Manifest still has moved entries.
- Kept aggregate counts for all valid manifests visible even when the display list is filtered.

Files changed:

- `tools/Summarize-RestoreManifests.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-restore-manifest-summary-tool.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-restore-manifest-undo-work-filter.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAny -RequireNoRecoveryReview -RequireNoUndoWork`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -UndoWorkOnly -ShowEntries` reported 10 valid manifests, 2 matching the undo-work display filter, and showed two fixture manifests with 3 moved entries.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -RecoveryReviewOnly -UndoWorkOnly` reported zero manifests matching both filters while preserving full-root aggregate counts.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -RecoveryReviewOnly -ShowEntries` still reported 2 recovery-review manifests with the older failed real-profile `DXCache` error evidence.
- Expected-failure check for `-RequireNoUndoWork` against the default root returned exit code 1 while 2 undo-work manifests remain.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- README, domain docs, glossary, Restore Manifest Summary feature brief, live-product readiness roadmap, this feature brief, handoff, and progress log.

ADRs added or skipped:

- No ADR added. This is read-only terminal evidence for existing Restore Manifest Summary behavior.

Follow-up work:

- Use `-UndoWorkOnly -ShowEntries` when inspecting manifests that still have moved entries before future live cleanup batches.
- Keep any actual recovery action selected-manifest-only through WPF gates unless a later ADR expands restore/history behavior.

Open questions:

- Whether a later dedicated restore/history surface should group outstanding undo work in WPF.

Risky assumptions:

- Outstanding moved entries should remain visible as undo-work evidence rather than being hidden or auto-restored.
