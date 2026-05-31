# Feature: Fixture Checklist First-Phase Blocker Alignment

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Keep manual fixture review wording aligned with the WPF real-profile first-phase blocker output.

## Non-goals

- Do not change WPF behavior.
- Do not enable real-profile Quarantine execution.
- Do not enable real-profile Undo Quarantine, selected real-profile restore, permanent deletion, or cleanup history.
- Do not scan, move, restore, delete, create, write, or clean up real-profile files.

## Implementation

- Updated `Start-MvpFixtureReview.ps1` checklist step 10 so the final real-profile/custom blocker prompt includes ADR 0018 first-phase limits: 10 rows, 1 GB, no-category rows, and strict descendant checks.
- Updated README fixture review and manual check wording with the same expectation.
- Left `AGENTS.md` unchanged.

## Test plan

Automated checks:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Manual fixture review prompts now call out ADR 0018 first-phase blocker visibility alongside ADR 0017 Quarantine blockers, ADR 0019 selected-restore blockers, and read-only Real-Profile Quarantine Approval Evidence.

ADRs:

- No ADR added. This is checklist wording alignment for accepted ADR 0018 and existing WPF output.

Follow-up work:

- Run the next visible fixture review and decide whether the gate text is clear enough without a dedicated readiness pane.

Open questions:

- None for this checklist packet.

Risky assumptions:

- Naming the ADR 0018 limits in the checklist helps reviewers notice missing blocker visibility without making the checklist feel like cleanup approval.
