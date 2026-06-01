# Feature: Daily Readiness Fixture Acceptance Status

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Let the read-only daily readiness command optionally print formal Fixture Acceptance Notes status, and optionally fail when those notes are incomplete.

## Non-goals

- Do not launch WPF.
- Do not create fixture files.
- Do not click `Scan`, scan, move, restore, delete, approve cleanup, write Restore Manifests, or create cleanup history.
- Do not automatically mark fixture notes complete.
- Do not make fixture notes a default daily gate.
- Do not enable broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, persisted cleanup history, shortcut creation, or installer behavior.

## User story / job story

As the local app owner, I want daily readiness to optionally surface formal fixture acceptance status, so that incomplete local fixture notes are visible when they matter without making ordinary daily accepted-package checks noisy or strict.

## Current behavior

`tools\Invoke-DailyLocalReadiness.cmd` verifies accepted package notes, runs one accepted package verifier pass, prints accepted normal/fixture launch commands, and prints Restore Manifest Summary output.

Formal fixture notes evidence is available through `tools\Summarize-FixtureAcceptanceNotes.cmd`, but it is a separate command. The latest real notes remain unfilled unless the user intentionally fills them manually or runs `Record-FixtureAcceptanceNotes.cmd -RecordManualAcceptance` after an all-pass visible fixture review.

## Desired behavior

`tools\Invoke-DailyLocalReadiness.cmd` should stay unchanged by default, but support:

- `-IncludeFixtureAcceptanceNotes` to print a read-only fixture notes summary.
- `-FixtureAcceptanceNotesPath` to summarize a specific ignored notes file.
- `-RequireFixtureAcceptanceComplete` to forward `-RequireComplete` and fail when formal fixture notes are incomplete.

`tools\Invoke-RealProfileQuarantineReadiness.cmd` should forward the same fixture-notes options into its daily readiness step when the stricter real-profile readiness review should include that evidence.

## Domain language changes

No new durable product term.

| Term | Change | Docs updated? |
|---|---|---|
| Fixture Acceptance Notes | Clarified that daily readiness may optionally include or require read-only fixture notes evidence. | yes |
| Portable Release Package | Clarified that daily local readiness can include optional Fixture Acceptance Notes evidence while remaining print-only/read-only. | yes |
| Real-Profile Quarantine Readiness Review | Clarified that fixture notes options may be forwarded through daily readiness. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether the user wants to record the existing user-reported fixture visual pass into the ignored notes, or only use the recorder for a future pass.

## Grill notes

### Scenarios discussed

- The user asked whether they needed to do anything; the answer for this packet is no because Codex can add read-only optional evidence plumbing.
- Formal fixture notes remain unfilled, so the strict flag should fail until a human intentionally records or fills notes.
- Ordinary daily readiness should not suddenly fail on fixture notes unless strictness is requested.

### Edge cases

- Passing `-FixtureAcceptanceNotesPath` implies the fixture notes summary should be printed even if `-IncludeFixtureAcceptanceNotes` is omitted.
- `-RequireFixtureAcceptanceComplete` should stop the daily wrapper before launch commands when notes are incomplete.
- The fixture notes summary remains read-only and constrained by the summary helper; it does not write ignored notes.

### Dependencies between decisions

- Fixture Acceptance Notes Recorder remains the only terminal writer for all-pass ignored fixture notes, and it requires `-RecordManualAcceptance`.
- Daily readiness remains accepted-package and recovery evidence, not WPF launch or cleanup approval.
- ADR 0017, ADR 0018, and ADR 0019 still govern real-profile movement and selected restore.

## Evidence and validation gate

Evidence gathered:

- User answers:
  - Continue toward a safe live product while preserving safety gates.
  - No user action is needed for read-only/tooling/docs work.
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
  - `docs/features/2026-06-02-daily-local-readiness-check.md`
  - `docs/features/2026-06-02-daily-readiness-single-package-verification.md`
  - `docs/features/2026-06-02-fixture-acceptance-notes-recorder.md`
  - `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
  - ADR 0017, ADR 0018, and ADR 0019
  - `tools/Invoke-DailyLocalReadiness.ps1`
  - `tools/Invoke-RealProfileQuarantineReadiness.ps1`
  - `tools/Summarize-FixtureAcceptanceNotes.ps1`
- Tests/checks planned:
  - `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork`
  - `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RequireFixtureAcceptanceComplete`
  - `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -IncludeFixtureAcceptanceNotes -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork`
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make fixture notes completion a default daily readiness requirement.
- Do not record fixture notes automatically from chat evidence.
- Do not let fixture notes status replace WPF fixture review or full MVP preflight.

## Decisions made

Small feature-level decisions:

- Add opt-in flags to daily readiness instead of changing default daily output behavior.
- Treat an explicit fixture notes path as an implicit request to print the fixture notes summary.
- Forward the same flags through real-profile readiness for stricter evidence reviews.

ADR-worthy decisions:

- [x] None. This is read-only terminal evidence composition over existing tools and ignored local notes.

## Implementation plan

1. Add optional Fixture Acceptance Notes flags to `Invoke-DailyLocalReadiness.ps1`.
2. Forward those flags from `Invoke-RealProfileQuarantineReadiness.ps1`.
3. Update README, domain docs, glossary, roadmap, handoff, and progress.
4. Run narrow read-only checks.

## Files expected to change

Expected:

- `tools/Invoke-DailyLocalReadiness.ps1`
- `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-daily-local-readiness-check.md`
- `docs/features/2026-06-02-daily-readiness-fixture-acceptance-status.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Review wrapper output to confirm fixture notes evidence says local ignored summary only and does not imply WPF launch, scan, movement, or cleanup approval.

Automated checks:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork`
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RequireFixtureAcceptanceComplete`
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -IncludeFixtureAcceptanceNotes -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- The strict fixture-notes flag is expected to fail while the latest notes are unfilled; docs and output need to make that unsurprising.
- Daily readiness can get verbose if every optional evidence source is included.

Assumptions:

- Optional fixture evidence is useful before stricter real-profile readiness reviews.
- Keeping fixture notes out of default daily readiness is the right balance for ordinary local use.

## Completion notes

Completed on: 2026-06-02

What changed:

- Added `-IncludeFixtureAcceptanceNotes`, `-FixtureAcceptanceNotesPath`, and `-RequireFixtureAcceptanceComplete` to `tools\Invoke-DailyLocalReadiness.ps1`.
- Added the same forwarding flags to `tools\Invoke-RealProfileQuarantineReadiness.ps1`.
- Kept default daily readiness behavior unchanged.

Files changed:

- `tools/Invoke-DailyLocalReadiness.ps1`
- `tools/Invoke-RealProfileQuarantineReadiness.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-daily-local-readiness-check.md`
- `docs/features/2026-06-02-daily-readiness-fixture-acceptance-status.md`
- `docs/features/2026-06-02-real-profile-quarantine-readiness-review.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -IncludeFixtureAcceptanceNotes -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed. It printed accepted package evidence, the optional fixture notes summary, accepted print-only launch commands, and exact-profile Restore Manifest evidence without launching WPF or touching files.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RequireFixtureAcceptanceComplete` failed as expected during `Fixture acceptance notes evidence` because the latest ignored fixture notes have preflight/worktree evidence not recorded, overall result not recorded, and 10 checklist items not recorded.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -IncludeFixtureAcceptanceNotes -RequireAnyDisplayedRestoreManifest -RequireNoDisplayedUndoWork` passed and forwarded the optional fixture notes summary through the daily readiness step.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected CRLF conversion warnings.

Docs updated:

- README, domain context, glossary, live-product roadmap, daily readiness feature brief, real-profile readiness feature brief, handoff, and progress log.

ADRs added or skipped:

- No ADR added. This is opt-in read-only terminal evidence plumbing, not a durable cleanup execution, restore scope, persistence, deployment, data-model, or security decision.

Follow-up work:

- Use `-IncludeFixtureAcceptanceNotes` when daily or real-profile readiness should surface formal fixture notes status.
- Use `-RequireFixtureAcceptanceComplete` only after the user wants formal fixture notes to be a strict gate.

Open questions:

- Whether to record the existing user-reported fixture visual pass into ignored fixture notes, or wait for a future visible pass.

Risky assumptions:

- Keeping the strict check opt-in prevents incomplete formal fixture notes from disrupting ordinary daily package/recovery evidence.
