# Feature: Fixture Checklist Hoverable Help Cues

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Align the manual fixture checklist wording with the current hoverable circular `?` help-cue behavior so the next visible review pass checks the intended affordance.

## Non-goals

- Do not change WPF layout, controls, tooltips, automation help text, scan behavior, Quarantine Preview, fixture execution, undo, restore, or manifest behavior.
- Do not launch WPF or scan the real profile as part of this packet.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

- Checklist prompts should call the existing circular `?` cues hoverable.
- Checklist prompts should remind the reviewer to check prompt tooltip/help text where the hover affordance matters.
- README launcher wording should match the same expectation.

## Completion notes

Completed on: 2026-05-30

What changed:

- Updated `Start-MvpFixtureReview.ps1` checklist lines for Review Mix, Matched Review Mix, Review Shortlist Safety Mix, Safety Summary header, Quarantine Shortlist header, inline Quarantine Preview readiness, and Review Grid Mode Status to call out hoverable `?` help cues.
- Updated README fixture launcher wording to describe hoverable `?` help cues.
- Recorded this wording alignment in the related fixture checklist and hoverable affordance feature briefs.
- Later packet `2026-05-30-scan-gate-summary-help-cue.md` updated checklist item 1 to include the scan-gate summary `?` help cue.

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

ADRs added or skipped:

- Skipped. This is checklist/documentation wording only with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the hoverable `?` cues and prompt tooltips are noticeable without making the dense review surface noisy.

Risky assumptions:

- Checklist wording is enough for this small alignment; no WPF code change is needed because the hoverable cue behavior already exists.
