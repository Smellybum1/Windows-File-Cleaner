# Feature: Fixture Checklist Fixture-First Boundary

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Keep the manual fixture review checklist explicit that its final real-profile/custom blocker item does not require scanning `C:\Users\moxhe` during the fixture pass.

## Non-goals

- Do not change WPF behavior.
- Do not scan `C:\Users\moxhe`.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not create folders, write Restore Manifests, move files, restore files, delete files, or clean up folders.

## Implementation

- Updated `Start-MvpFixtureReview.ps1` checklist step 10 to say the final real-profile/custom blocker check should be done without scanning `C:\Users\moxhe` as part of the fixture pass.
- Updated README fixture-smoke and manual-review wording with the same fixture-first boundary.
- Kept the existing ADR 0017, ADR 0018, ADR 0019, and read-only approval-evidence expectations in place.

## Test plan

Automated checks:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- The fixture checklist now tells reviewers to use custom preview-only paths or existing synthetic real-profile readiness evidence for the final real-profile/custom blocker check unless they intentionally start a separate real-profile retest after MVP preflight.

ADRs:

- No ADR added. This is checklist and README clarification only; it does not change architecture, persistence, cleanup execution, restore behavior, or safety policy.

Follow-up work:

- Run the next visible fixture review with `.\tools\Start-MvpFixtureReview.cmd` when the user is ready.
- After visible review, decide whether the compact Quarantine Readiness Summary is clear enough or needs a dedicated readiness pane.

Open questions:

- None for this checklist clarification.

Risky assumptions:

- Clarifying the fixture-first boundary reduces the chance of accidental real-profile scanning during a fixture visual pass without weakening the real-profile blocker expectations.
