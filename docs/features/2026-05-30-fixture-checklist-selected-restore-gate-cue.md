# Feature: Fixture Checklist Selected Restore Gate Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Align the manual fixture checklist with the new Selected Restore Execution Gate `?` help cue so the next visible review pass checks the states and layout risk that automation cannot see.

## Non-goals

- Do not change WPF layout, controls, tooltips, automation help text, scan behavior, Quarantine Preview, fixture execution, undo, restore, or manifest behavior.
- Do not launch WPF or scan the real profile as part of this packet.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

- The fixture checklist should explicitly ask the reviewer to inspect the Selected Restore Execution Gate `?` help cue in waiting, closed, open, and restored states.
- The checklist should capture the remaining visual risk: whether the cue crowds the selected restore gate area.
- README manual review wording should match the same expectation.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Selected Restore Execution Gate | Existing term used for checklist guidance only. | n/a |

## Evidence and validation gate

Evidence gathered:

- The Selected Restore Gate Help Cue packet added the visible cue and automated state coverage.
- The follow-up risk in that packet is visual: confirm the cue is noticeable without crowding the gate area.
- The next recommended work remains manual fixture visual polish.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: script output and docs only, no filesystem mutation in automated checks.
- [x] The narrowest relevant verification path is launcher `-ChecklistOnly` plus `git diff --check`.

## Decisions made

Small feature-level decisions:

- Update the existing selected-restore checklist line instead of adding another numbered step.
- Name the state set compactly as waiting/closed/open/restored.
- Keep the reminder about layout crowding in the checklist because it is the part automation cannot prove.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update `Start-MvpFixtureReview.ps1` checklist wording for the Selected Restore Execution Gate cue.
2. Update README fixture smoke/manual MVP wording.
3. Record progress and handoff updates.
4. Verify checklist output without preflight, fixture creation, WPF launch, scan, or file modification.

## Test plan

Manual checks:

- During visible fixture review, hover the Selected Restore Execution Gate `?` cue before preview, after preview before exact `RESTORE`, after exact `RESTORE`, and after fixture selected restore execution.
- Confirm the cue is discoverable without crowding the selected restore gate area.

Automated tests:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- The checklist line gets longer and may feel denser in the terminal.

Assumptions:

- One compact reminder is enough to guide the visible review pass without adding another checklist step.

## Completion notes

Completed on: 2026-05-30

What changed:

- Updated fixture checklist step 8 to check the Selected Restore Execution Gate `?` help cue in waiting, closed, open, and restored states and to watch for crowding.
- Updated README fixture smoke and Manual MVP wording to match.
- No WPF behavior, scan behavior, Quarantine behavior, selected restore behavior, real-profile/custom restore availability, permanent deletion, or cleanup history changed.

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-30-fixture-checklist-selected-restore-gate-cue.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `tools/Start-MvpFixtureReview.ps1`

ADRs added or skipped:

- Skipped. This is checklist/documentation wording only with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Follow-up work:

- Use the checklist during the next visible fixture review and decide whether the cue placement needs WPF layout polish.

Open questions:

- Does the selected restore cue crowd the selected restore gate area in the visible app?

Risky assumptions:

- The manual checklist is the right place to preserve this visual-only review requirement.
