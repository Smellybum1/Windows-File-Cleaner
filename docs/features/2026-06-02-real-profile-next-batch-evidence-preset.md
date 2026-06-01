# Feature: Real-Profile Next-Batch Evidence Preset

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make the safest terminal-only review before another tiny exact `C:\Users\moxhe` Quarantine batch easy to run as one named preset.

## Non-goals

- Do not launch WPF.
- Do not click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, quarantine, approve cleanup, write Restore Manifests, or create cleanup history.
- Do not broaden real-profile movement beyond the existing exact first-phase gate.
- Do not make Fixture Acceptance Notes completion a default strict requirement.
- Do not require historical recovery-review debt to be zero by default.
- Do not add broad/all-manifest restore, custom real-profile Quarantine, permanent deletion, persisted cleanup history, shortcut creation, or installer behavior.

## User story / job story

As the local app owner, I want one hard-to-misuse command before another tiny real-profile batch, so that the exact-profile evidence gates are visible without remembering several independent strict flags.

## Desired behavior

`tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` should:

1. Remain terminal-only evidence.
2. Require the exact real-profile Cleanup Scope `C:\Users\moxhe`.
3. Reject `-AllCleanupScopes` and non-exact Cleanup Scopes.
4. Include Fixture Acceptance Notes status in the daily readiness step.
5. Require displayed Restore Manifest evidence for the exact-profile display.
6. Require zero displayed undo-work manifests.
7. Keep recovery-review manifests visible without making them a default blocker.

## Domain language changes

No new durable product term.

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Quarantine Readiness Review | Added `-RequireNextBatchEvidence` as the exact-profile-only preset before another tiny batch review. | yes |

## Grill notes

### Scenarios discussed

- The user asked whether they needed to do anything; no user action was needed because this packet is read-only tooling/docs.
- The safest next-batch command should include exact-profile displayed strictness and formal fixture notes status without expecting the user to remember multiple flags.
- Existing exact-profile recovery-review manifests from older failed NVIDIA `DXCache` attempts should stay visible, but should not block the current recommended preset because they are not displayed undo-work.

### Edge cases

- `-RequireNextBatchEvidence -AllCleanupScopes` fails fast.
- `-RequireNextBatchEvidence -CleanupScope <fixture path>` fails fast.
- `-RequireNextBatchEvidence` still allows explicit `-RequireFixtureAcceptanceComplete` when formal fixture notes should be a strict gate.

## Decisions made

- Add one preset switch over existing evidence flags rather than changing default readiness behavior.
- Keep the preset exact `C:\Users\moxhe` only.
- Include Fixture Acceptance Notes status, `-RequireAnyDisplayedRestoreManifest`, and `-RequireNoDisplayedUndoWork`.
- Do not include `-RequireNoDisplayedRecoveryReview`, because current exact-profile recovery-review evidence is known historical debt and should remain visible rather than silently hidden or default-blocking.

ADR-worthy decisions:

- [x] None. This is a read-only local tooling preset over existing ADR 0017/0018/0019 gates.

## Implementation

- Added `-RequireNextBatchEvidence` to `tools\Invoke-RealProfileQuarantineReadiness.ps1`.
- Added exact-profile-only validation for the preset.
- Forwarded preset evidence into daily readiness and focused undo-work evidence without changing cleanup execution behavior.
- Updated README, domain context, glossary, roadmap, real-profile readiness feature notes, handoff, and progress docs.

## Verification

- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -RequireNextBatchEvidence` passed. It printed optional Fixture Acceptance Notes status, exact-profile displayed Restore Manifest evidence, zero displayed undo-work manifests, two displayed recovery-review manifests, and no WPF launch, scan, movement, restore, deletion, approval, or cleanup history.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -AllCleanupScopes -RequireNextBatchEvidence` failed as expected with an exact-profile-only guard.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -SkipMvpPreflight -CleanupScope "D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture" -RequireNextBatchEvidence` failed as expected with an exact-profile-only guard.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed.
- `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` passed with full MVP preflight, accepted package verification, fixture notes status, exact-profile Restore Manifest display, recovery-review focus, and undo-work focus.

## Follow-up packets

- `Real-Profile Next-Batch WPF Checklist` added `tools\Show-RealProfileNextBatchChecklist.cmd` so the manual WPF review after this preset can be printed without launching WPF, scanning, movement, restore, deletion, approval, or cleanup history.

## Risks and assumptions

Risks:

- The preset output remains verbose because it deliberately keeps package, fixture-notes, recovery, and undo evidence visible.
- The accepted-package verifier warning remains expected while the accepted package commit is behind newer docs/tooling commits.

Assumptions:

- The next tiny live batch review should optimize for fewer remembered flags, not a quieter terminal output.
- Historical recovery-review manifests are useful review context but should not block this preset unless they also create displayed undo work.
