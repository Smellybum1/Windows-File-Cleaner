# Feature: Real-Profile Next-Batch WPF Checklist

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Add one terminal-only checklist command for the manual WPF review that follows the next-batch evidence preset before any future tiny exact `C:\Users\moxhe` Quarantine batch.

## Non-goals

- Do not launch WPF.
- Do not click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, quarantine, approve cleanup, write Restore Manifests, or create cleanup history.
- Do not replace `Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence`.
- Do not weaken WPF readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, immediate Pre-Execution Revalidation, or explicit user approval.
- Do not add broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, persisted cleanup history, shortcut creation, or installer behavior.

## User story / job story

As the local app owner, I want the post-preset WPF review steps printed from the terminal, so that the next tiny exact batch is not guided by memory or chat scrollback.

## Desired behavior

`tools\Show-RealProfileNextBatchChecklist.cmd` should print:

1. The terminal-only boundary.
2. The recommended combined next-batch review wrapper command.
3. The required next-batch evidence preset command when evidence is run separately.
4. The accepted package launch-command printer.
5. The exact `C:\Users\moxhe` scope boundary.
6. The scan, Review Shortlist, tiny-batch caps, and hard blockers.
7. The Quarantine tab readiness evidence to inspect.
8. The human-only exact `QUARANTINE` click boundary.
9. The post-attempt rediscover/rescan follow-up.

## Domain language changes

New durable local tooling term.

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Next-Batch Checklist | Added as terminal-only manual WPF checklist after the readiness preset. | yes |

## Grill notes

### Scenarios discussed

- The readiness preset proves terminal evidence, but the human still has to review WPF readiness before any click.
- The checklist should reduce missed WPF review steps without turning into launch automation.
- Codex must not click real-profile movement.

### Edge cases

- The checklist must not be treated as cleanup approval.
- The accepted package command should be printed with `-PrintOnly`; launching remains a deliberate human action.
- The checklist should keep hard blockers visible, including broad parents, high-risk/protected/no-category/access-issue rows, cloud sync data, credential data, source code, game saves, active app state, and Codex/tooling state.

## Decisions made

- Use a `Show-*` command because the helper only prints guidance.
- Keep the helper independent from the readiness preset so it never runs WPF or file movement indirectly.
- Document the checklist in README and durable domain docs because it is now part of the safe next-batch local workflow.

ADR-worthy decisions:

- [x] None. This is read-only local guidance over existing ADR 0017/0018/0019 gates.

## Implementation

- Added `tools\Show-RealProfileNextBatchChecklist.ps1` and `.cmd`.
- The helper prints prerequisite evidence, accepted-package launch guidance, scan/shortlist checks, hard blockers, Quarantine tab evidence checks, execution boundary, and rediscover/rescan follow-up.
- Follow-up polish surfaced `tools\Invoke-RealProfileNextBatchReview.cmd` at the top of the checklist output so standalone checklist runs still advertise the safer combined evidence-plus-checklist path.
- Follow-up fixture-notes guidance added a `Before launch` reminder to confirm Fixture Acceptance Notes status from the evidence preset and to use the printed recorder command only after an actual all-pass visible fixture review, or fill notes manually for issues/not-checked items.
- Updated README, domain context, glossary, roadmap, handoff, and progress docs.

## Verification

- `cmd.exe /c tools\Show-RealProfileNextBatchChecklist.cmd` passed and printed the manual WPF checklist without launching WPF or touching files.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireNextBatchEvidence` passed and preserved the terminal evidence preset path without launching WPF, scanning, movement, restore, deletion, approval, or cleanup history.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected CRLF conversion warnings only.
- Follow-up wrapper-hint check: `cmd.exe /c tools\Show-RealProfileNextBatchChecklist.cmd` passed and printed the recommended combined wrapper command plus the separate evidence preset without launching WPF, scanning, movement, restore, deletion, approval, manifest writes, or cleanup history.
- Follow-up fixture-notes guidance check: `cmd.exe /c tools\Show-RealProfileNextBatchChecklist.cmd` passed and printed the new Fixture Acceptance Notes reminder without launching WPF, scanning, movement, restore, deletion, approval, manifest writes, or cleanup history.

## Risks and assumptions

Risks:

- Another checklist can become stale if WPF gate wording changes.
- The checklist is intentionally verbose because it is guarding real-profile movement.

Assumptions:

- A terminal checklist is useful when the user is close to a human-clicked tiny batch.
- Keeping the checklist print-only is safer than launching WPF from the helper.
