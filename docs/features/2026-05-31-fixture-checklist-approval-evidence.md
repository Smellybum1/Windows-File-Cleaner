# Feature: Fixture Checklist Approval Evidence Alignment

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Keep the manual fixture review checklist aligned with the WPF Quarantine Execution Gate now that it shows read-only Real-Profile Quarantine Approval Evidence for non-fixture preview-only scopes.

## Non-goals

- Do not change WPF behavior.
- Do not enable real-profile Quarantine execution.
- Do not change fixture-only execution or selected restore.
- Do not scan, move, restore, delete, create, write, or clean up real-profile files.

## Implementation

- Updated `Start-MvpFixtureReview.ps1` checklist step 10 to ask reviewers to confirm the Quarantine Execution Gate shows read-only Real-Profile Quarantine Approval Evidence with `Can approve real-profile movement: no`.
- Updated README fixture review and manual check wording with the same expectation.
- Left AGENTS.md unchanged.

## Test plan

Automated checks:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Manual fixture review prompts now call out the visible approval-evidence blocker alongside ADR 0017/0019 blockers.

ADRs:

- No ADR added. This is checklist wording alignment for accepted ADR 0018 and existing WPF output.

Follow-up work:

- Run the next visible fixture review and decide whether the gate text is clear enough without a dedicated readiness pane.

Open questions:

- None for this checklist packet.

Risky assumptions:

- Naming `Can approve real-profile movement: no` in the checklist is clearer than leaving the new visible evidence implicit.
