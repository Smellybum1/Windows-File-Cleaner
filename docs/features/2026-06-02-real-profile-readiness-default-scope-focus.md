# Feature: Real-Profile Readiness Default Scope Focus

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make `tools\Invoke-RealProfileQuarantineReadiness.cmd` focus Restore Manifest display output to the exact real-profile Cleanup Scope `C:\Users\moxhe` by default, so future live-batch readiness review does not require remembering an extra `-CleanupScope` argument.

## Non-goals

- Do not launch WPF.
- Do not click `Scan` or scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, write Restore Manifests, approve cleanup, or create cleanup history.
- Do not change the daily readiness wrapper default.
- Do not hide full-root aggregate counts in Restore Manifest Summary.
- Do not add broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, persisted cleanup history, shortcut creation, or installer behavior.

## User story / job story

As the local app owner, I want the real-profile readiness command to default to exact real-profile manifest display, so that fixture undo-work does not distract from the evidence needed before a future exact `C:\Users\moxhe` batch.

## Current behavior

`tools\Invoke-RealProfileQuarantineReadiness.cmd` accepts `-CleanupScope "C:\Users\moxhe"` and forwards it to daily and focused Restore Manifest summaries, but the default command displays all Cleanup Scopes. That makes the command safe but leaves an avoidable memory tax for the exact real-profile path it is named for.

## Desired behavior

- The real-profile readiness wrapper defaults Restore Manifest display focus to exact `C:\Users\moxhe`.
- `-CleanupScope "<fully-qualified path>"` can still override that display focus.
- `-AllCleanupScopes` disables the default display focus and preserves the older fixture-inclusive view.
- Supplying both `-CleanupScope` and `-AllCleanupScopes` fails before running the readiness sequence.
- Full-root aggregate counts remain visible because `-CleanupScope` is still only a display filter inside Restore Manifest Summary.

## Domain language changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Quarantine Readiness Review | Clarified that Restore Manifest display focus defaults to exact `C:\Users\moxhe`, with `-AllCleanupScopes` for fixture-inclusive display. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether a later WPF restore/history surface should group manifests by Cleanup Scope.

## Grill notes

### Scenarios discussed

- Before another exact real-profile batch, the most relevant Restore Manifest evidence is exact `C:\Users\moxhe` recovery-review and undo-work state.
- Fixture undo-work remains useful evidence, but it should be an explicit all-scope view from the real-profile readiness command.
- The accepted package/current-HEAD warning remains expected when the accepted package is behind docs/tooling commits.

### Edge cases

- `-AllCleanupScopes` and `-CleanupScope` together are ambiguous and should fail before package verification or summary output.
- A custom explicit `-CleanupScope` should still be fully qualified.
- Exact real-profile undo-work may be zero even when full-root fixture undo-work remains; that is evidence, not cleanup approval.

### Dependencies between decisions

- ADR 0017 and ADR 0018 still control exact real-profile Quarantine movement.
- ADR 0019 still controls selected real-profile restore.
- Restore Manifest Cleanup Scope Filter keeps `-CleanupScope` as a display filter, not a strict root-level clearance.

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
  - `docs/features/2026-06-01-live-product-readiness-roadmap.md`
  - `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
  - `docs/features/2026-06-02-restore-manifest-cleanup-scope-filter.md`
  - ADR 0017, ADR 0018, and ADR 0019
  - `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- Tests/checks planned:
  - `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight`
  - `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -AllCleanupScopes`
  - `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -CleanupScope "C:\Users\moxhe" -AllCleanupScopes`
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make `Invoke-DailyLocalReadiness.cmd` default to exact real-profile display, because daily evidence is broader than the real-profile batch review.
- Do not remove `-CleanupScope`; explicit focus remains useful for smoke roots or future exact scopes.
- Do not make default scoped output a strict no-undo/no-recovery clearance.

## Decisions made

Small feature-level decisions:

- Default only the real-profile readiness wrapper to exact `C:\Users\moxhe`.
- Add `-AllCleanupScopes` to preserve the previous all-scope display.
- Print the selected display focus near the top of the wrapper output.

ADR-worthy decisions:

- [x] None. This is read-only terminal evidence polish over existing readiness tooling and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Implementation plan

1. Add default exact real-profile display focus to `tools\Invoke-RealProfileQuarantineReadiness.ps1`.
2. Add `-AllCleanupScopes` and ambiguous-argument validation.
3. Update README, domain docs, glossary, roadmap, handoff, and progress log.
4. Run narrow read-only checks.

## Files expected to change

Expected:

- `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/features/2026-06-02-real-profile-readiness-default-scope-focus.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Review output wording to confirm the display focus is evidence only and not cleanup approval.

Automated checks:

- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight`
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -AllCleanupScopes`
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -CleanupScope "C:\Users\moxhe" -AllCleanupScopes`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- Users may forget that full-root aggregate counts still include fixture manifests even when display rows are focused.
- `-AllCleanupScopes` is one more option to document, but it keeps the older view available.

Assumptions:

- The exact real-profile Cleanup Scope is still `C:\Users\moxhe`.
- Defaulting the real-profile wrapper to exact real-profile display is clearer than requiring a repeated explicit argument.

## Completion notes

Completed on: 2026-06-02

What changed:

- Defaulted `tools\Invoke-RealProfileQuarantineReadiness.cmd` Restore Manifest display focus to exact `C:\Users\moxhe`.
- Added `-AllCleanupScopes` to preserve the previous fixture-inclusive display path.
- Added an early guard that rejects `-CleanupScope` combined with `-AllCleanupScopes`.

Files changed:

- `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/features/2026-06-02-real-profile-readiness-default-scope-focus.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight` passed; output showed `Restore Manifest display focus: C:\Users\moxhe`, exact real-profile daily display `(4 of 10)`, recovery-review focus `(2 of 10)`, undo-work focus `(0 of 10)`, and no WPF launch, scan, movement, restore, deletion, or cleanup history.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -AllCleanupScopes` passed; output showed `Restore Manifest display focus: all Cleanup Scopes` and fixture-inclusive undo-work focus `(2 of 10)`.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -CleanupScope "C:\Users\moxhe" -AllCleanupScopes` failed as expected with exit code 1 and `Choose either -CleanupScope or -AllCleanupScopes, not both.`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected CRLF conversion warnings.

Docs updated:

- README Daily Local Use and safety status.
- Domain context and glossary Real-Profile Quarantine Readiness Review wording.
- Live-product readiness roadmap and original readiness feature brief.
- Fresh-thread handoff and progress log.

ADRs added or skipped:

- No ADR added. This is read-only readiness tooling polish.

Follow-up work:

- Continue using the default real-profile readiness command before any future exact `C:\Users\moxhe` batch review.

Open questions:

- Whether a later WPF restore/history surface should group manifests by Cleanup Scope.

Risky assumptions:

- Exact `C:\Users\moxhe` is still the right default display focus for real-profile readiness.
