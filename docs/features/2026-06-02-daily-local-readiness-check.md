# Feature: Daily Local Readiness Check

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Add one read-only daily command that confirms accepted package evidence, prints accepted package launch commands, and summarizes Restore Manifests without launching WPF or creating shortcut/installer artifacts.

## Non-goals

- Do not publish a new package.
- Do not launch WPF.
- Do not create an installer, shortcut, service, scheduled task, or background automation.
- Do not scan, move, restore, delete, write Restore Manifests, approve cleanup, or create cleanup history.
- Do not enable broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, or persisted cleanup history.

## User story / job story

As the local app owner, I want one safe daily readiness command, so that I can verify the accepted package path and current recovery evidence before deciding whether to launch the app manually.

## Current behavior

Daily local use is possible through several separate read-only commands:

- `tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
- `tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly`
- `tools\Summarize-RestoreManifests.cmd`

That is safe but a little scattered for repeated daily use.

## Desired behavior

`tools\Invoke-DailyLocalReadiness.cmd` should run the same checks in a single read-only sequence:

1. Confirm accepted package notes are complete.
2. Print the accepted normal launch command.
3. Print the accepted fixture launch command.
4. Print Restore Manifest Summary output.

The command should stop on failed required evidence, preserve optional Restore Manifest filters, and repeat the no-shortcut/no-installer/no-WPF/no-scan/no-movement/no-history boundary.

A later fixture-status packet added opt-in Fixture Acceptance Notes evidence flags: `-IncludeFixtureAcceptanceNotes`, `-FixtureAcceptanceNotesPath`, and `-RequireFixtureAcceptanceComplete`. The default daily command stays unchanged.

## Domain language changes

No new durable product term.

| Term | Change | Docs updated? |
|---|---|---|
| Portable Release Package | Added daily local readiness tooling as read-only, print-only package/recovery evidence composition. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether a real installed shortcut or installer is worth a later explicit user-approved packet.

## Grill notes

### Scenarios discussed

- The accepted package is intentionally behind current docs-only `HEAD`, so the verifier warning is expected.
- The daily command should make the accepted path easier to use without creating profile artifacts or changing app behavior.

### Edge cases

- Restore Manifest Summary may show recovery-review or undo-work debt; the daily command should report it without failing unless strict `-RequireNoRecoveryReview` or `-RequireNoUndoWork` is requested.
- Accepted package evidence should fail the daily command when the completed notes are missing or incomplete.
- The command should support explicit acceptance notes path and Quarantine Root when the user wants to inspect a non-default local artifact.

### Dependencies between decisions

- The Accepted Local Release Launcher remains the source for accepted package selection.
- Restore Manifest Summary remains the source for terminal recovery evidence.
- Portable v1 remains non-installed and reversible-only.

## Evidence and validation gate

Evidence gathered:

- User answers:
  - Continue advancing toward a safe live product with small safe packets.
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
  - `docs/features/2026-06-02-accepted-package-daily-use-guide.md`
  - `docs/features/2026-06-02-accepted-local-release-launcher.md`
  - `docs/features/2026-06-02-portable-v1-acceptance-baseline.md`
  - ADR 0017, ADR 0018, and ADR 0019
  - `tools/Start-AcceptedLocalRelease.ps1`
  - `tools/Start-LocalRelease.ps1`
  - `tools/Summarize-LocalReleaseAcceptanceNotes.ps1`
  - `tools/Summarize-RestoreManifests.ps1`
- Tests/checks planned:
  - `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
  - `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RecoveryReviewOnly -ShowRestoreEntries`
  - `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -UndoWorkOnly -ShowRestoreEntries`
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not create a desktop shortcut from Codex.
- Do not turn the daily command into a WPF launcher by default.
- Do not fail daily readiness on known recovery-review or undo-work evidence unless strict flags are requested.

## Decisions made

Small feature-level decisions:

- Add a wrapper command over existing evidence tools instead of duplicating accepted-package or Restore Manifest parsing.
- Keep Restore Manifest strictness optional through forwarded flags.
- Treat shortcut/installer automation as deferred; this packet creates no installed artifacts.

ADR-worthy decisions:

- [x] None. This is local read-only tooling that composes existing accepted package and Restore Manifest evidence. It does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Implementation plan

1. Add `tools/Invoke-DailyLocalReadiness.ps1` and `.cmd`.
2. Update README Daily Local Use.
3. Update domain/glossary wording for Portable Release Package tooling.
4. Update the roadmap, handoff, and progress log.
5. Run narrow read-only checks.

## Files expected to change

Expected:

- `tools/Invoke-DailyLocalReadiness.ps1`
- `tools/Invoke-DailyLocalReadiness.cmd`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-daily-local-readiness-check.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Review the command output and README wording to confirm it does not imply shortcut creation, app launch, scan, movement, or cleanup approval.

Automated tests:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RecoveryReviewOnly -ShowRestoreEntries`
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -UndoWorkOnly -ShowRestoreEntries`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- The command is intentionally verbose because it preserves accepted package verifier evidence.
- The accepted package verifier warning can look noisy when docs-only commits move current `HEAD` ahead of the accepted package.

Assumptions:

- A single read-only daily command is useful enough to justify a wrapper over existing tools.
- Shortcut or installer creation should wait for explicit user approval because it writes installed/profile artifacts.

## Completion notes

Completed on: 2026-06-02

What changed:

- Added `tools\Invoke-DailyLocalReadiness.cmd` / `.ps1`.
- The command verifies completed accepted package notes, prints accepted normal and fixture launch commands, and prints Restore Manifest Summary output.
- Added optional Restore Manifest forwarding flags for entry display, recovery-review focus, undo-work focus, and strict no-recovery/no-undo evidence checks.

Files changed:

- `tools/Invoke-DailyLocalReadiness.ps1`
- `tools/Invoke-DailyLocalReadiness.cmd`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-daily-local-readiness-check.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd` passed. It verified completed accepted package notes, printed accepted normal and fixture launch commands, and summarized the default Restore Manifest root as 10 valid manifests, 13 entries, 8 restored entries, 3 moved entries, 2 failed entries, 2 undo-work manifests, and 2 recovery-review manifests. The expected package/current-HEAD commit warning appeared because the accepted package is behind newer docs/tooling commits.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RecoveryReviewOnly -ShowRestoreEntries` passed and displayed the 2 recovery-review manifests, both older failed real-profile `DXCache` attempts, with entry-level error evidence.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -UndoWorkOnly -ShowRestoreEntries` passed and displayed the 2 undo-work fixture manifests with 3 moved entries.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAnyRestoreManifest -RequireNoRecoveryReview -RequireNoUndoWork` passed, proving explicit root and strict Restore Manifest forwarding on a repo-local smoke root with 1 restored manifest and no recovery/undo work.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected line-ending normalization warnings only.
- Follow-up current-HEAD refresh: `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed on `0bc2e96`. It verified accepted package notes, printed optional Fixture Acceptance Notes status, verified the accepted package once with the expected package/current-HEAD warning, printed accepted normal/fixture launch commands, and showed exact-profile display `(4 of 10)`, displayed undo work `0`, and displayed recovery review `2` without launching WPF, scanning, movement, restore, deletion, approval, manifest writes, or cleanup history.
- Follow-up after accepted-launcher output boundary: `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed on `9dc5b98`. It verified accepted package notes, printed optional Fixture Acceptance Notes status, verified the accepted package once with the expected package/current-HEAD warning, printed accepted normal/fixture launch commands in print-only mode, repeated the accepted-release daily-path/debug-shortcut boundary, and showed exact-profile display `(4 of 10)`, displayed undo work `0`, and displayed recovery review `2` without launching WPF, scanning, movement, restore, deletion, approval, manifest writes, or cleanup history.
- Follow-up focused evidence: `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries` showed the two older failed NVIDIA `DXCache` attempts with entry-level error evidence, `cmd.exe /c tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly` showed zero exact-profile undo-work manifests, and `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete` passed.
- Follow-up after exact-profile recovery evidence: `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed on `5b2a982`. It verified accepted package notes, printed optional Fixture Acceptance Notes status, verified the accepted package once with the expected accepted-package/current-HEAD warning (`bc9b869` accepted package versus `5b2a982` current `HEAD`), printed accepted normal/fixture launch commands in print-only mode, and showed exact-profile display `(4 of 10)`, displayed undo work `0`, and displayed recovery review `2` without launching WPF, scanning, movement, restore, deletion, approval, manifest writes, or cleanup history.

Docs updated:

- README, domain docs, glossary, live-product readiness roadmap, this feature brief, handoff, and progress log.

ADRs added or skipped:

- No ADR added. This is read-only local tooling that composes existing accepted package and Restore Manifest evidence.

Follow-up work:

- Continue using the daily command before manual accepted-package launches when compact local readiness evidence is useful.
- Consider a real shortcut or installer only as a later explicit user-approved packaging packet.
- Use `-IncludeFixtureAcceptanceNotes` when formal fixture notes status should be visible in the daily output, and `-RequireFixtureAcceptanceComplete` only when incomplete fixture notes should fail the check.

Open questions:

- Whether the user wants an installed shortcut later, and if so whether it should target the accepted package launcher or a future installer.

Risky assumptions:

- Verbose verifier output is acceptable for a safety-first daily readiness check.
