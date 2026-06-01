# Feature: Restore Manifest Displayed Strictness

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Add optional read-only strict checks for the displayed Restore Manifest set, so exact real-profile readiness can prove focused states such as "no displayed exact-profile undo work" without treating older fixture manifests as blockers.

## Non-goals

- Do not launch WPF.
- Do not scan fixture or real-profile files.
- Do not move, restore, delete, write Restore Manifests, approve cleanup, or create cleanup history.
- Do not change the existing full-root `-RequireNoRecoveryReview` or `-RequireNoUndoWork` behavior.
- Do not add broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, persisted cleanup history, shortcut creation, or installer behavior.

## User story / job story

As the local app owner, I want optional strict checks over the currently displayed Restore Manifest evidence, so that exact `C:\Users\moxhe` readiness can fail or pass on exact-profile manifests while full-root fixture history remains visible as aggregate context.

## Current behavior

`tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly` can display zero exact real-profile undo-work manifests. However, `-RequireNoUndoWork` still checks the full selected Quarantine Root and fails when fixture manifests still have undo work. That full-root behavior is useful, but it is not the same question as focused exact-profile readiness.

When proving "no exact-profile undo work," the check should assert that the exact-profile display exists and has zero displayed undo-work manifests; it should not require at least one undo-work-only displayed manifest.

## Desired behavior

The summary helper should support display-filtered strict flags:

- `-RequireAnyDisplayed` fails when no manifest matches the current display filters.
- `-RequireNoDisplayedRecoveryReview` fails when any displayed manifest needs recovery review.
- `-RequireNoDisplayedUndoWork` fails when any displayed manifest has undo work.

The existing full-root strict flags remain unchanged:

- `-RequireAny`
- `-RequireNoRecoveryReview`
- `-RequireNoUndoWork`

Daily and real-profile readiness wrappers should forward the displayed strict flags.

## Domain language changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Restore Manifest Summary | Clarified that local summary tooling can fail checks against displayed manifests as well as full-root aggregate state. | yes |
| Real-Profile Quarantine Readiness Review | Clarified that displayed strict checks can apply to the exact real-profile display focus. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether a later WPF restore/history surface should group manifests by Cleanup Scope.

## Grill notes

### Scenarios discussed

- The default Quarantine Root has exact real-profile manifests and fixture manifests.
- Exact real-profile undo-work display currently shows zero matching manifests.
- Full-root undo-work still reports fixture manifests, which should remain visible but should not be the only strictness option for exact-profile evidence.

### Edge cases

- Displayed strictness can pass with zero displayed manifests unless `-RequireAnyDisplayed` is also set.
- Combining `-RecoveryReviewOnly` with `-RequireNoDisplayedRecoveryReview` intentionally fails when matching recovery-review manifests exist.
- Full-root strict flags should continue to fail when any valid manifest under the selected Quarantine Root has the state, regardless of display filters.

### Dependencies between decisions

- Restore Manifest Cleanup Scope Filter made `-CleanupScope` a display filter only.
- Real-Profile Readiness Default Scope Focus defaults real-profile readiness display to exact `C:\Users\moxhe`.
- ADR 0017, ADR 0018, and ADR 0019 still govern movement and selected restore; this packet is terminal evidence only.

## Evidence and validation gate

Evidence gathered:

- User answers:
  - Continue toward a safe live product while preserving safety gates.
  - Do not move, delete, quarantine, or restore real-profile files unless explicitly asked for the specific action.
- Existing code/docs inspected:
  - `AGENTS.md`
  - `.codex/progress.md`
  - `README.md`
  - `docs/codex/thread-handoff.md`
  - `docs/codex/grill-with-docs.md`
  - `docs/codex/skillopt-inspired-workflow.md`
  - `docs/domain/context.md`
  - `docs/domain/glossary.md`
  - `docs/features/2026-06-02-restore-manifest-cleanup-scope-filter.md`
  - `docs/features/2026-06-02-real-profile-readiness-default-scope-focus.md`
  - `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
  - ADR 0017, ADR 0018, and ADR 0019
  - `tools/Summarize-RestoreManifests.ps1`
  - `tools/Invoke-DailyLocalReadiness.ps1`
  - `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- Tests/checks planned:
  - `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayed -RequireNoDisplayedUndoWork`
  - `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -RequireNoDisplayedRecoveryReview`
  - `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork`
  - `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireNoDisplayedRecoveryReview`
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not change full-root strict flags to silently follow display filters.
- Do not hide full-root aggregate counts when displayed strictness is used.
- Do not make displayed strictness imply cleanup approval or selected restore approval.

## Decisions made

Small feature-level decisions:

- Add displayed strict flags to the summary helper rather than changing existing strict flags.
- Forward displayed strict flags through daily and real-profile readiness wrappers.
- Print displayed recovery-review and undo-work counts when display filters or displayed strict flags are active.

ADR-worthy decisions:

- [x] None. This is read-only terminal evidence behavior and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Implementation plan

1. Add displayed strict flags to `tools/Summarize-RestoreManifests.ps1`.
2. Forward the flags through `Invoke-DailyLocalReadiness.ps1`.
3. Forward the flags through `Invoke-RealProfileQuarantineReadiness.ps1`.
4. Update README, domain docs, glossary, roadmap, handoff, and progress log.
5. Run narrow read-only checks.

## Files expected to change

Expected:

- `tools/Summarize-RestoreManifests.ps1`
- `tools/Invoke-DailyLocalReadiness.ps1`
- `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/features/2026-06-02-restore-manifest-cleanup-scope-filter.md`
- `docs/features/2026-06-02-restore-manifest-displayed-strictness.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Review output wording to confirm displayed strictness is evidence only and full-root aggregate counts remain visible.

Automated checks:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayed -RequireNoDisplayedUndoWork`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -RequireNoDisplayedRecoveryReview`
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork`
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireNoDisplayedRecoveryReview`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- Displayed strictness can be misread as all-root clearance unless docs keep the distinction explicit.
- `-RequireAnyDisplayed` needs to be paired with no-displayed-state checks when "zero matching manifests" should not be treated as success.

Assumptions:

- Focused exact-profile evidence is useful before future real-profile cleanup review.

## Completion notes

Completed on: 2026-06-02

What changed:

- Added displayed strict flags to `tools\Summarize-RestoreManifests.ps1`: `-RequireAnyDisplayed`, `-RequireNoDisplayedRecoveryReview`, and `-RequireNoDisplayedUndoWork`.
- Printed displayed undo-work and recovery-review counts when display filters or displayed strict flags are active.
- Forwarded displayed strictness through daily readiness and real-profile readiness wrappers, while keeping full-root strict flags unchanged.
- Kept `-RequireAnyDisplayedRestoreManifest` scoped to the daily/default exact-profile display in real-profile readiness so undo-work focus can legitimately pass with zero displayed undo-work manifests.

Files changed:

- `tools/Summarize-RestoreManifests.ps1`
- `tools/Invoke-DailyLocalReadiness.ps1`
- `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/features/2026-06-02-restore-manifest-cleanup-scope-filter.md`
- `docs/features/2026-06-02-restore-manifest-displayed-strictness.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayed -RequireNoDisplayedUndoWork` passed; exact-profile display showed 4 of 10 manifests, displayed undo work 0, displayed recovery review 2, while full-root aggregates still showed 2 undo-work and 2 recovery-review manifests.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -RequireNoDisplayedRecoveryReview` failed as expected with exit code 1 because two displayed exact-profile manifests need recovery review.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed; daily display showed 4 exact-profile manifests, undo-work focus showed 0 exact-profile undo-work manifests, and no WPF launch, scan, movement, restore, deletion, approval, or cleanup history occurred.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireNoDisplayedRecoveryReview` failed as expected with exit code 1 during daily Restore Manifest summary because two displayed exact-profile manifests need recovery review.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed without preflight, fixture creation, WPF launch, scan, movement, restore, deletion, or cleanup history.
- `git diff --check` passed with expected CRLF conversion warnings.

Docs updated:

- README Daily Local Use and Restore Manifest Summary examples.
- Domain context and glossary Restore Manifest Summary / Real-Profile Quarantine Readiness Review wording.
- Live-product roadmap, real-profile readiness feature brief, cleanup-scope filter feature brief, handoff, and progress log.

ADRs added or skipped:

- No ADR added. This is read-only terminal evidence behavior.

Follow-up work:

- Use displayed strict flags when exact real-profile manifest evidence needs to be enforced separately from fixture history.

Open questions:

- Whether a later WPF restore/history surface should group manifests by Cleanup Scope.

Risky assumptions:

- The displayed/full-root distinction is clear enough in terminal output and docs.
