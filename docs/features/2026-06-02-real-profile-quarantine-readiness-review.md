# Feature: Real-Profile Quarantine Readiness Review

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Add one terminal-only command that gathers the evidence needed before considering another tiny exact `C:\Users\moxhe` Quarantine batch, without launching WPF, scanning the real profile, moving, restoring, deleting, approving cleanup, or creating cleanup history.

## Non-goals

- Do not launch WPF.
- Do not click `Scan` or scan `C:\Users\moxhe`.
- Do not move, restore, delete, quarantine, approve cleanup, write Restore Manifests, or create cleanup history.
- Do not broaden real-profile movement beyond the existing exact first-phase gate.
- Do not add broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, persisted cleanup history, shortcut creation, or installer behavior.

## User story / job story

As the local app owner, I want a repeatable read-only readiness review before any future real-profile Quarantine batch, so that preflight, accepted-package, and Restore Manifest evidence are visible before the WPF app is launched manually.

## Current behavior

The necessary evidence exists, but it is split across separate commands:

- `tools\Invoke-MvpPreflight.cmd`
- `tools\Invoke-DailyLocalReadiness.cmd`
- `tools\Summarize-RestoreManifests.cmd -RecoveryReviewOnly`
- `tools\Summarize-RestoreManifests.cmd -UndoWorkOnly`

That keeps the pieces safe, but makes the next tiny live-batch checklist easier to miss.

## Desired behavior

`tools\Invoke-RealProfileQuarantineReadiness.cmd` should run a read-only review sequence:

1. Run full MVP preflight by default.
2. Run daily local readiness for accepted package and Restore Manifest evidence.
3. Print recovery-review Restore Manifest focus.
4. Print undo-work Restore Manifest focus.
5. Repeat the exact real-profile stop boundary.

The command may skip MVP preflight only with `-SkipMvpPreflight`, and skipped output must say it is not fresh movement evidence. Restore Manifest root, entry display, and strict evidence flags should forward to the summary steps.

A later scope-focus packet made the default Restore Manifest display focus exact `C:\Users\moxhe`; use `-AllCleanupScopes` when fixture manifests should also be displayed. A later displayed-strictness packet added `-RequireAnyDisplayedRestoreManifest`, `-RequireNoDisplayedRecoveryReview`, and `-RequireNoDisplayedUndoWork` forwarding; `-RequireAnyDisplayedRestoreManifest` applies to the default exact-profile daily display, while the no-displayed flags can fail the focused recovery-review or undo-work evidence without changing full-root strictness. A later fixture-status packet added forwarding for `-IncludeFixtureAcceptanceNotes`, `-FixtureAcceptanceNotesPath`, and `-RequireFixtureAcceptanceComplete` when formal fixture notes evidence should appear in the daily readiness step.

## Domain language changes

New durable local tooling term.

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Quarantine Readiness Review | Added as terminal-only evidence composition before any future exact real-profile Quarantine batch. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether a later installed shortcut or installer is useful after accepted-package daily use is stable.
- Whether future broad/all-manifest restore or cleanup history should ever exist.

## Grill notes

### Scenarios discussed

- The user asked whether they needed to do anything; the right answer for this packet is no, because Codex can gather read-only terminal evidence without touching the profile.
- Any future real-profile movement must remain user-clicked in WPF after explicit approval for the specific tiny batch.
- Existing recovery-review and undo-work manifests should be visible before more live cleanup is considered.

### Edge cases

- `-SkipMvpPreflight` is useful for fast script smoke checks but must not be used as fresh movement evidence.
- `-RequireNoRecoveryReview` and `-RequireNoUndoWork` should fail safely while the default Quarantine Root still contains those states.
- Package/current-HEAD commit warnings remain expected when the accepted package is behind later docs/tooling commits.

### Dependencies between decisions

- ADR 0017 and ADR 0018 still control exact real-profile Quarantine readiness.
- ADR 0019 still controls selected real-profile restore recovery.
- The daily readiness wrapper remains the accepted-package and default Restore Manifest evidence source.
- Restore Manifest Summary remains the focused recovery-review and undo-work evidence source.

## Evidence and validation gate

Evidence gathered:

- User answers:
  - Keep the app local-first and safety-gated.
  - Do not move, delete, quarantine, or restore real-profile files unless explicitly asked for the specific action after readiness review.
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
  - `docs/features/2026-06-02-daily-local-readiness-check.md`
  - ADR 0017, ADR 0018, and ADR 0019
  - `tools/Invoke-MvpPreflight.cmd`
  - `tools/Invoke-DailyLocalReadiness.cmd`
  - `tools/Summarize-RestoreManifests.cmd`
- Tests/checks planned:
  - `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight`
  - `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd`
  - `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAnyRestoreManifest -RequireNoRecoveryReview -RequireNoUndoWork`
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not create shortcut/installer artifacts in this packet.
- Do not use a terminal wrapper as cleanup approval.
- Do not make Codex click real-profile movement.
- Do not turn Restore Manifest summary strict flags into automatic restore or cleanup behavior.

## Decisions made

Small feature-level decisions:

- Add a wrapper over existing evidence tools rather than duplicating preflight, package, or manifest parsing.
- Run full MVP preflight by default, with explicit skipped-preflight wording for smoke checks.
- Always show recovery-review and undo-work Restore Manifest focus after daily readiness, because both are important before another live batch.

ADR-worthy decisions:

- [x] None. This is read-only local tooling that composes existing evidence commands. It does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Implementation plan

1. Add `tools/Invoke-RealProfileQuarantineReadiness.ps1` and `.cmd`.
2. Update README Daily Local Use.
3. Update domain and glossary wording.
4. Update roadmap, handoff, and progress log.
5. Run read-only checks.

## Files expected to change

Expected:

- `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- `tools/Invoke-RealProfileQuarantineReadiness.cmd`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Review command output and docs wording to confirm the command is evidence-only and not cleanup approval.

Automated tests:

- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight`
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd`
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAnyRestoreManifest -RequireNoRecoveryReview -RequireNoUndoWork`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- The default command is intentionally slower because it runs full MVP preflight.
- The output is verbose because it preserves package verifier and Restore Manifest evidence.
- Strict no-recovery or no-undo flags are expected to fail on the default Quarantine Root while historical recovery-review or undo-work manifests remain.

Assumptions:

- A terminal evidence wrapper is useful before future WPF use, even though WPF readiness still must be reviewed separately.
- The accepted package commit warning remains acceptable when newer commits are docs/tooling-only.

## Completion notes

Completed on: 2026-06-02

What changed:

- Added `tools\Invoke-RealProfileQuarantineReadiness.cmd` / `.ps1`.
- The command runs full MVP preflight by default, then daily local readiness, recovery-review Restore Manifest focus, and undo-work Restore Manifest focus.
- Added optional forwarding for accepted-notes path, Quarantine Root, Restore Manifest entry display, and strict Restore Manifest evidence flags.

Files changed:

- `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- `tools/Invoke-RealProfileQuarantineReadiness.cmd`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight` passed. It verified accepted package notes, ran one accepted package verifier pass, printed accepted normal and fixture launch commands, summarized the default Restore Manifest root as 10 valid manifests / 13 entries / 8 restored / 3 moved / 2 failed / 2 undo-work / 2 recovery-review, then printed focused recovery-review and undo-work summaries. It did not launch WPF, scan the real profile, move, restore, delete, approve cleanup, or create cleanup history.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAnyRestoreManifest -RequireNoRecoveryReview -RequireNoUndoWork` passed, proving explicit root and strict Restore Manifest forwarding on a repo-local smoke root with 1 restored manifest and no recovery/undo work.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd` passed. Full MVP preflight restored, built, ran core tests, ran WPF app tests, ran fixture `-WhatIf`, printed the fixture checklist, and ran whitespace diff checking before the daily and focused Restore Manifest evidence steps.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected line-ending normalization warnings only.

Docs updated:

- README, domain docs, glossary, live-product readiness roadmap, this feature brief, handoff, and progress log.

ADRs added or skipped:

- No ADR added. This is read-only local tooling over existing ADR 0017/0018/0019 gates.

Follow-up work:

- Before any next real-profile Quarantine batch, run the default readiness review, then have the user review WPF readiness for a tiny exact batch and explicitly approve the specific click.
- Keep shortcut/installer automation as a later explicit user-approved packaging packet.
- Later packet `Real-Profile Readiness Default Scope Focus` made default readiness output focus exact `C:\Users\moxhe` Restore Manifest display rows and added `-AllCleanupScopes` for the old fixture-inclusive view.
- Later packet `Restore Manifest Displayed Strictness` forwarded displayed strictness flags so exact-profile displayed evidence can be enforced separately from full-root fixture history.
- Later packet `Daily Readiness Fixture Acceptance Status` forwarded optional Fixture Acceptance Notes flags so stricter readiness reviews can include local fixture notes status without launching WPF, scanning, movement, restore, deletion, approval, or cleanup history.

Open questions:

- Whether the user wants an installed shortcut later.
- Whether permanent deletion, persisted cleanup history, or all-manifest restore should ever be added.

Risky assumptions:

- Verbose terminal evidence is acceptable for the extra safety before future live cleanup.
