# Feature: Accepted Package Daily Use Guide

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make the safest daily-use path visible near the top of the README: print or launch the latest accepted Portable Release Package, verify its ignored acceptance notes, and use the Restore Manifest Summary helper for read-only recovery evidence.

## Non-goals

- Do not publish a new package.
- Do not launch WPF.
- Do not scan, move, restore, delete, approve cleanup, or create cleanup history.
- Do not create an installer, shortcut, service, scheduled task, or background automation.
- Do not enable broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, or persisted cleanup history.

## User story / job story

As the local app owner, I want the accepted-package daily-use commands in one obvious README section, so that I can start from the accepted reversible-only app package without accidentally treating newer docs-only commits as a fresh package.

## Current behavior

The README already documents Portable Release Package tooling and Restore Manifest Summary tooling, but the accepted-package daily-use path is inside the longer release section. A fresh session can find it, but it requires more scanning than the daily path deserves.

## Desired behavior

The README has a compact Daily Local Use section near the top that shows:

- `Start-AcceptedLocalRelease.cmd -PrintOnly`
- `Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly`
- `Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `Summarize-RestoreManifests.cmd`
- the stop boundary before real-profile movement.

## Domain language changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing terms are reused. | n/a |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether installed shortcut or installer automation is worth a separate packaging packet.

## Grill notes

### Scenarios discussed

- The accepted package baseline is the right daily-use app package until a new package is cut and accepted.
- Restore Manifest Summary is read-only terminal evidence and does not add broad/all-manifest restore.

### Edge cases

- The accepted package can intentionally be behind current docs-only `HEAD`.
- `-Fixture` only prefills the Cleanup Scope and does not create fixture files or click `Scan`.

### Dependencies between decisions

- ADR 0016 keeps discovered manifests in manifest panes rather than all quarantined history.
- ADR 0017 and ADR 0018 keep real-profile Quarantine exact, readiness-gated, and user-clicked.
- ADR 0019 keeps real-profile selected restore selected-manifest-only.

## Evidence and validation gate

Evidence gathered:

- User answers:
  - Prefer accepted-package daily-use guidance or Restore Manifest summary/recovery evidence as the next small packet.
  - Do not move, delete, quarantine, or restore real-profile files unless the user explicitly asks for the specific action after readiness review.
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
  - `docs/features/2026-06-01-restore-manifest-summary-tool.md`
  - `docs/features/2026-06-02-accepted-local-release-launcher.md`
  - `docs/features/2026-06-02-portable-v1-acceptance-baseline.md`
  - ADR 0016, ADR 0017, ADR 0018, and ADR 0019.
- Tests/checks planned:
  - `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
  - `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly`
  - `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
  - `cmd.exe /c tools\Summarize-RestoreManifests.cmd`
  - `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
  - `git diff --check`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add a shortcut or installer in this packet.
- Do not cut or accept a new package for a docs-only daily-use guide.
- Do not turn Restore Manifest Summary into cleanup history or broad restore.

## Decisions made

Small feature-level decisions:

- Add a short top-level README section rather than a new tool.
- Keep existing release and Restore Manifest sections intact for detailed reference.

ADR-worthy decisions:

- [x] None. This is documentation guidance for existing accepted tooling and does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

## Implementation plan

1. Add the Daily Local Use section to README.
2. Record this feature brief and completion evidence.
3. Update the handoff and progress log.
4. Run the narrow print-only/read-only checks.

## Files expected to change

Expected:

- `README.md`
- `docs/features/2026-06-02-accepted-package-daily-use-guide.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- `docs/features/2026-06-01-live-product-readiness-roadmap.md`

## Test plan

Manual checks:

- Review the README section for command clarity and safety-boundary wording.

Automated tests:

- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd` reported 10 valid manifests, 13 entries, 8 restored entries, 3 still moved fixture entries, 2 failed real-profile attempts needing recovery review, and the first-live real-profile manifest restored with no undo work.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- A quick daily-use section can become stale if future packages are accepted without updating the baseline docs.

Assumptions:

- The accepted local package remains the right daily-use baseline until a fresh package is intentionally cut and accepted.
- Read-only terminal summaries are useful daily evidence without changing the app safety boundary.

## Completion notes

Completed on: 2026-06-02

What changed:

- Added a compact README Daily Local Use section with accepted package print commands, accepted evidence summary command, Restore Manifest Summary command, and the real-profile movement stop boundary.
- Kept release details and Restore Manifest Summary details in their existing sections.

Files changed:

- `README.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-accepted-package-daily-use-guide.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -PrintOnly`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly`
- `cmd.exe /c tools\Summarize-LocalReleaseAcceptanceNotes.cmd -RequireComplete`
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Docs updated:

- README, live-product readiness roadmap, this feature brief, handoff, and progress log.

ADRs added or skipped:

- No ADR added. This is daily-use documentation for existing accepted tooling.

Follow-up work:

- Consider installed shortcut or installer automation only as a separate packaging packet.
- Cut and accept a fresh package only after future behavior changes should ship as the next accepted app package.

Open questions:

- Whether installed shortcut or installer automation is worth doing later.

Risky assumptions:

- The accepted local package remains the correct daily-use package until the user intentionally accepts a new one.
