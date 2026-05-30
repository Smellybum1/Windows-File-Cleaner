# Feature: Manifest Fixture Checklist Split

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the manual fixture checklist easier to run by separating manifest discovery/all-manifest readiness checks from selected restore gate checks.

## Non-goals

- Do not change WPF manifest review layout.
- Do not change Quarantine Manifest Discovery, Restore Manifest selection, Restore Readiness Preview, selected restore, fixture Quarantine execution, undo, or all-manifest readiness behavior.
- Do not enable real-profile WPF Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not move, restore, delete, create, write, or clean up files or folders.

## Desired behavior

- Checklist step 8 focuses on `Discover manifests`, `Preview all-manifest readiness`, the three manifest-review `?` help cues, cue/control wrapping, no all-manifest restore action wording, and all-manifest readiness scope tooltips.
- Checklist step 9 focuses on selected manifest readiness, selected restore confirmation, selected restore scope status, approval boundary, Selected Restore Execution Gate `?` cue states, and restore tooltips.
- The real-profile/custom execution blocker check remains the final checklist step.

## Decisions made

- Split the previous long manifest/selected-restore checklist step instead of changing app layout without new visual evidence.
- Keep this as manual-review tooling polish only; no new domain term or ADR is needed.

## Completion notes

Completed on: 2026-05-30

What changed:

- Split `Start-MvpFixtureReview.ps1` checklist step 8 into separate manifest-review and selected-restore gate steps.
- Updated README fixture launcher wording to describe the separated checklist checks.

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

ADRs added or skipped:

- Skipped. This is reversible checklist/readme polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During visible fixture review, confirm whether the separated checklist makes manifest cue/control wrapping easier to inspect.

Risky assumptions:

- A clearer manual checklist is the right first response until a visible fixture pass shows whether WPF layout itself needs adjustment.
