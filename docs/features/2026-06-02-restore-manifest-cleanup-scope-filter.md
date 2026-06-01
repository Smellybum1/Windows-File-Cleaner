# Feature: Restore Manifest Cleanup Scope Filter

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make Restore Manifest Summary output focusable by exact Cleanup Scope so real-profile recovery evidence can be inspected separately from fixture undo-work, without adding restore, history, scan, or movement behavior.

## Non-goals

- Do not launch WPF.
- Do not scan fixture or real-profile files.
- Do not move, restore, delete, write Restore Manifests, approve cleanup, or create cleanup history.
- Do not add broad/all-manifest restore.
- Do not change selected restore, Quarantine, permanent deletion, or history availability.
- Do not hide full-root aggregate counts.

## User story / job story

As the local app owner, I want terminal Restore Manifest evidence filtered to `C:\Users\moxhe`, so that future real-profile readiness review can distinguish real-profile recovery-review or undo-work debt from older fixture manifests.

## Current behavior

`tools\Summarize-RestoreManifests.cmd` can focus recovery-review manifests and undo-work manifests, but it cannot focus the displayed list to one Cleanup Scope. On the default Quarantine Root this mixes exact real-profile manifests with fixture manifests in some focused views.

## Desired behavior

The summary helper supports:

- `-CleanupScope "<fully-qualified path>"` to display only valid manifests whose Cleanup Scope exactly matches the provided path.
- Composition with `-RecoveryReviewOnly`, `-UndoWorkOnly`, and `-ShowEntries`.
- The existing full-root aggregate counts and strict `-RequireNoRecoveryReview` / `-RequireNoUndoWork` checks remain full selected-root evidence.

This is read-only terminal evidence only.

## Domain language changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Restore Manifest Summary | Clarified that terminal summary tooling can focus displayed manifests by exact Cleanup Scope. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether a later dedicated restore/history surface should group manifests by Cleanup Scope in WPF.

## Grill notes

### Scenarios discussed

- The first-live real-profile manifest is restored by user report and terminal summary evidence.
- The default `D:\WindowsFileCleanerQuarantine` root includes both exact real-profile manifests and older fixture manifests.
- Before any future exact real-profile batch, it is useful to see whether the exact real-profile Cleanup Scope still has recovery-review or undo-work evidence.

### Edge cases

- `-CleanupScope` should require a fully qualified path.
- `-CleanupScope` should be a display filter only; it should not change the selected Quarantine Root, scan, restore, repair, delete, or write manifests.
- Filter intersections may produce no displayed manifests and still pass because "no matching display rows" is evidence, not cleanup approval.

### Dependencies between decisions

- ADR 0016 keeps discovered manifests in manifest panes rather than all quarantined history.
- ADR 0019 keeps real-profile restore selected-manifest-only.
- The Real-Profile Quarantine Readiness Review packet composes summary output but does not replace WPF readiness gates.

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
  - `docs/codex/grill-with-docs.md`
  - `docs/codex/skillopt-inspired-workflow.md`
  - `docs/domain/context.md`
  - `docs/domain/glossary.md`
  - `docs/features/2026-06-01-restore-manifest-summary-tool.md`
  - `docs/features/2026-06-02-restore-manifest-recovery-review-filter.md`
  - `docs/features/2026-06-02-restore-manifest-undo-work-filter.md`
  - `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
  - ADR 0016 and ADR 0019
  - `tools/Summarize-RestoreManifests.ps1`
- Tests/checks planned:
  - `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries`
  - `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly`
  - `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly`
  - `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -CleanupScope "C:\Users\moxhe"`
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add a cleanup-scope repair action.
- Do not change strict no-recovery/no-undo checks to silently ignore other manifests under the selected root.
- Do not add broad/all-manifest restore or cleanup history to make filtering feel more useful.

## Decisions made

Small feature-level decisions:

- Add `-CleanupScope` as a display filter to the existing summary helper.
- Forward the filter through daily and real-profile readiness wrappers for command consistency.
- Keep full-root aggregate counts visible even when the displayed manifest list is filtered.

ADR-worthy decisions:

- [x] None. This is read-only terminal evidence for existing Restore Manifest Summary behavior and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Implementation plan

1. Add `-CleanupScope` to `tools/Summarize-RestoreManifests.ps1`.
2. Forward `-CleanupScope` through `Invoke-DailyLocalReadiness.ps1` and `Invoke-RealProfileQuarantineReadiness.ps1`.
3. Update README, domain docs, glossary, Restore Manifest Summary feature brief, live-product readiness roadmap, handoff, and progress log.
4. Run narrow read-only checks.

## Files expected to change

Expected:

- `tools/Summarize-RestoreManifests.ps1`
- `tools/Invoke-DailyLocalReadiness.ps1`
- `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-restore-manifest-summary-tool.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-restore-manifest-cleanup-scope-filter.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Review output wording to confirm `-CleanupScope` is a display filter and not cleanup approval.

Automated tests:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly`
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly`
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -CleanupScope "C:\Users\moxhe"`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- A display filter can be misread as clearance if the user ignores the full-root aggregate counts.
- Strict `-RequireNoRecoveryReview` and `-RequireNoUndoWork` still apply to the full selected root, so combining them with `-CleanupScope` may fail because of other manifests.

Assumptions:

- Distinguishing exact real-profile manifests from fixture manifests is useful before future real-profile cleanup review.

## Completion notes

Completed on: 2026-06-02

What changed:

- Added a read-only `-CleanupScope` display filter to Restore Manifest Summary tooling.
- Forwarded the filter through Daily Local Readiness and Real-Profile Quarantine Readiness.
- Kept full-root aggregate counts and strict no-recovery/no-undo checks scoped to the selected Quarantine Root.

Files changed:

- `tools/Summarize-RestoreManifests.ps1`
- `tools/Invoke-DailyLocalReadiness.ps1`
- `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-restore-manifest-summary-tool.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-restore-manifest-cleanup-scope-filter.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries` passed; displayed the two exact real-profile recovery-review manifests and their failed DXCache entries.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly` passed; displayed zero exact real-profile undo-work manifests while preserving full-root aggregate counts.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly` passed; verified accepted package evidence in print-only mode and printed the scoped recovery-review summary.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -CleanupScope "C:\Users\moxhe"` passed; printed scoped real-profile totals, recovery-review focus, and undo-work focus without WPF launch, scan, movement, restore, deletion, or history.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed; printed the checklist without preflight, fixture creation, WPF launch, scan, movement, restore, deletion, or history.
- `git diff --check` passed with expected CRLF conversion warnings.

Docs updated:

- README Daily Local Use / Restore Manifest Summary command examples.
- Domain context and glossary Restore Manifest Summary wording.
- Restore Manifest Summary and live-product readiness feature briefs.
- Fresh-thread handoff and progress log.

ADRs added or skipped:

- No ADR added. This is read-only terminal evidence for existing Restore Manifest Summary behavior.

Follow-up work:

- Use `-CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries` when exact real-profile recovery-review evidence is needed.
- Use `-CleanupScope "C:\Users\moxhe" -UndoWorkOnly` when exact real-profile moved-entry evidence is needed.
- Later packet `Restore Manifest Displayed Strictness` builds on this display filter with displayed-only strict checks such as `-RequireAnyDisplayed` and `-RequireNoDisplayedUndoWork`.

Open questions:

- Whether a later dedicated restore/history surface should group manifests by Cleanup Scope in WPF.

Risky assumptions:

- Full-root aggregate counts plus explicit display-filter wording are enough to avoid treating filtered output as cleanup clearance.
