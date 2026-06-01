# Feature: Daily Readiness Single Package Verification

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Keep `tools\Invoke-DailyLocalReadiness.cmd` safe for daily use while avoiding duplicate accepted package verifier output when it prints both normal and fixture launch commands.

## Non-goals

- Do not publish or accept a new package.
- Do not launch WPF.
- Do not create an installer, shortcut, service, scheduled task, or background automation.
- Do not scan, move, restore, delete, write Restore Manifests, approve cleanup, or create cleanup history.
- Do not weaken the default verification behavior of direct accepted-package launch commands.

## Current behavior

`Invoke-DailyLocalReadiness.cmd` verifies completed acceptance notes, then calls `Start-AcceptedLocalRelease.cmd -PrintOnly` and `Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly`. Each accepted launcher call delegates to `Start-LocalRelease.cmd`, so the same package verifier output appears twice in one daily readiness run.

## Desired behavior

The daily readiness command should still verify the accepted package before printing launch commands, but it should run the package verifier once and skip the duplicate verifier pass only for the second print-only fixture command in the same wrapper flow.

Direct `Start-AcceptedLocalRelease.cmd` use should remain verified by default.

## Domain language changes

No new durable product term.

| Term | Change | Docs updated? |
|---|---|---|
| Portable Release Package | Clarified that a wrapper may skip duplicate package verification after the same run already verified the accepted package. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Whether an installed shortcut or installer is worth a later explicit user-approved packaging packet.

## Grill notes

### Scenarios discussed

- Daily readiness should remain read-only, print-only, and safe even when the accepted package is behind newer docs/tooling commits.
- Reducing duplicate verifier noise makes the command more usable without turning it into a launcher or shortcut.

### Edge cases

- If the first accepted package verifier fails, the wrapper must stop before printing the fixture command.
- `-SkipVerify` on the accepted launcher should be explicit and should say it is intended only after package verification already passed in the same readiness or acceptance flow.
- Restore Manifest filters and strictness flags are unrelated and should keep their current behavior.

## Decisions made

Small feature-level decisions:

- Add `-SkipVerify` forwarding to `Start-AcceptedLocalRelease.cmd` because `Start-LocalRelease.cmd` already owns that behavior.
- Use `-SkipVerify` only for the second daily readiness accepted-package print step after the normal print step has already verified the package.
- Print a clear skip-verification note when the accepted launcher is called that way.

ADR-worthy decisions:

- [x] None. This is read-only local tooling polish over existing package verification and print-only launcher behavior.

## Implementation plan

1. Add `-SkipVerify` forwarding to `Start-AcceptedLocalRelease.ps1`.
2. Update `Invoke-DailyLocalReadiness.ps1` so the first accepted launch command verifies the package and the second fixture command skips duplicate verification.
3. Update docs and progress evidence.
4. Run narrow read-only checks.

## Files expected to change

- `tools/Start-AcceptedLocalRelease.ps1`
- `tools/Invoke-DailyLocalReadiness.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-daily-readiness-single-package-verification.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## Test plan

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd`
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RecoveryReviewOnly -ShowRestoreEntries`
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly -SkipVerify`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- `-SkipVerify` could be misused directly. The accepted launcher prints a boundary note when it is used, and direct accepted-package launches remain verified by default.

Assumptions:

- One package verifier run is enough for a single wrapper invocation that immediately prints both accepted launch commands.

## Completion notes

Completed on: 2026-06-02

What changed:

- Added `-SkipVerify` forwarding to `tools\Start-AcceptedLocalRelease.ps1`.
- Updated `tools\Invoke-DailyLocalReadiness.ps1` so the normal accepted print command verifies the package and the fixture print command skips duplicate verification in the same run.
- Added a daily-readiness package-verification line to the wrapper output and a skip-verification note to the accepted launcher output.

Files changed:

- `tools/Start-AcceptedLocalRelease.ps1`
- `tools/Invoke-DailyLocalReadiness.ps1`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-daily-readiness-single-package-verification.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd` passed. The output printed exactly one `Portable v1 release verifier` heading, then printed the fixture command with `Delegated package verification: skipped by request`; no WPF app was launched and no scan, movement, restore, delete, approval, or cleanup history occurred.
- `cmd.exe /c tools\Invoke-DailyLocalReadiness.cmd -RecoveryReviewOnly -ShowRestoreEntries` passed and preserved recovery-review filtering, showing the 2 older failed real-profile `DXCache` recovery-review manifests.
- `cmd.exe /c tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly -SkipVerify` passed and printed the skip-verification boundary plus the fixture launch command without launching WPF.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected line-ending normalization warnings only.

Docs updated:

- README, domain docs, glossary, live-product readiness roadmap, this feature brief, handoff, and progress log.

ADRs added or skipped:

- No ADR added. This does not change architecture, persistence, cleanup execution, restore scope, deployment model, data model, or security policy.

Follow-up work:

- Keep direct accepted-package launcher use verified by default.
- Consider installed shortcut or installer automation only as a later explicit user-approved packaging packet.

Open questions:

- Whether shortcut or installer work is worth doing later.

Risky assumptions:

- The daily wrapper's single verifier run is clearer than duplicate verifier output while still preserving enough package evidence for one local readiness invocation.
