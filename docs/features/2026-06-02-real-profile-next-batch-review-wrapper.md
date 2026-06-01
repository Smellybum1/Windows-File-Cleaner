# Feature: Real-Profile Next-Batch Review Wrapper

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Add one terminal-only command that runs the exact-profile next-batch evidence preset and then prints the manual WPF checklist before any future tiny exact `C:\Users\moxhe` Quarantine batch.

## Non-goals

- Do not launch WPF.
- Do not click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, quarantine, approve cleanup, write Restore Manifests, or create cleanup history.
- Do not replace WPF readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, immediate Pre-Execution Revalidation, or explicit user approval.
- Do not add broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, persisted cleanup history, shortcut creation, or installer behavior.
- Do not add all-scope or custom Cleanup Scope knobs to the next-batch review wrapper.

## User story / job story

As the local app owner, I want one command for the full terminal review before a future tiny exact real-profile batch, so that the required evidence and the WPF checklist stay together without relying on memory.

## Desired behavior

`tools\Invoke-RealProfileNextBatchReview.cmd` should:

1. Run `tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence`.
2. Print `tools\Show-RealProfileNextBatchChecklist.cmd` only after the preset passes.
3. Forward local evidence paths, Quarantine Root display, optional entry display, and strict Fixture Acceptance Notes completion when requested.
4. Allow `-SkipMvpPreflight` only as a smoke-check option with explicit warning text.
5. Stay terminal-only evidence/guidance.

## Domain language changes

New durable local tooling term.

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Next-Batch Review | Added as terminal-only wrapper for the exact-profile evidence preset plus checklist. | yes |

## Grill notes

### Scenarios discussed

- The evidence preset and WPF checklist already exist as separate safe commands.
- The next human-clicked tiny batch benefits from one harder-to-miss terminal path.
- Codex must not click real-profile movement.

### Edge cases

- `-SkipMvpPreflight` is useful for wrapper smoke tests but is not fresh movement evidence.
- Formal fixture notes completion remains optional unless `-RequireFixtureAcceptanceComplete` is supplied.
- The wrapper intentionally does not expose `-AllCleanupScopes` or `-CleanupScope`, because the next-batch preset is exact `C:\Users\moxhe` only.

## Decisions made

- Add a small wrapper over the existing preset and checklist rather than duplicating evidence parsing or checklist text.
- Keep the wrapper exact-profile-only by construction.
- Keep shortcut/installer automation out of this packet.

ADR-worthy decisions:

- [x] None. This is read-only local tooling over existing ADR 0017/0018/0019 gates.

## Implementation

- Added `tools\Invoke-RealProfileNextBatchReview.ps1` and `.cmd`.
- The wrapper runs the preset first and stops if it fails, then prints the checklist.
- Follow-up polish made the standalone checklist point back to this wrapper as the recommended combined evidence-plus-checklist command.
- Updated README, domain context, glossary, live-product roadmap, handoff, and progress docs.

## Verification

- `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd` passed. It ran full MVP preflight, including restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and whitespace diff checking, then ran daily readiness, exact-profile Restore Manifest display, recovery-review and undo-work focus, and printed the WPF checklist. No WPF app was launched, no real-profile scan was started, and no files were moved, restored, deleted, approved, or added to cleanup history.
- `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd -SkipMvpPreflight` passed. It ran the next-batch evidence preset with skipped-preflight warning, verified accepted package evidence, printed optional Fixture Acceptance Notes status, showed exact-profile Restore Manifest display with zero displayed undo-work manifests, printed recovery-review and undo-work focus, then printed the WPF checklist. No WPF app was launched, no scan was started, and no files were moved, restored, deleted, approved, or added to cleanup history.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `git diff --check` passed with expected line-ending normalization warnings only.
- Follow-up wrapper-hint smoke: `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd -SkipMvpPreflight` passed after the checklist began printing the recommended combined wrapper command. No WPF app was launched, no scan was started, and no files were moved, restored, deleted, approved, written to Restore Manifests, or added to cleanup history.

## Risks and assumptions

Risks:

- Another wrapper can add command-list weight if it is not surfaced clearly.
- The output is intentionally verbose because it preserves safety evidence.

Assumptions:

- Keeping evidence and checklist together is useful before any future user-clicked tiny exact batch.
- The accepted-package/current-HEAD warning remains expected while the accepted package is behind newer docs/tooling commits.
