# Feature: Preflight Fixture Notes Next Step

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Align the MVP preflight success output with the current notes-enabled manual fixture review workflow.

## Non-goals

- Do not launch WPF from preflight.
- Do not create fixture files from preflight.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, create, or rewrite real-profile files.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.

## Current Behavior

`Invoke-MvpPreflight.cmd` prints the sectioned fixture checklist in checklist-only mode and then suggests `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight` as the next manual fixture step.

The fixture launcher now supports `-WriteAcceptanceNotes`, and the live-product roadmap names visible fixture acceptance with local notes as the next gate.

## Desired Behavior

After preflight passes, the success output should suggest:

```powershell
.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes
```

That keeps the post-preflight manual fixture pass from rerunning preflight while still creating an ignored `.local` acceptance notes template.

## Domain Language Changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing MVP preflight, fixture review, and Cleanup Scope language used. | n/a |

## Evidence And Validation Gate

Evidence gathered:

- User visually approved the compact header layout.
- The live-product roadmap identifies visible fixture acceptance as the next gate.
- The fixture acceptance notes template and sectioned checklist are already implemented.
- The preflight success output still pointed at the older no-notes launcher command.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or deferred.

## Decisions Made

Small feature-level decisions:

- Keep preflight read-only and no-launch.
- Change only the success hint, not the preflight steps.
- Prefer the `.cmd` launcher in output because it works with the local execution-policy wrapper.

ADR-worthy decisions:

- [x] None.

## Implementation Plan

1. Update the preflight success message.
2. Update README and the existing preflight feature brief.
3. Record the packet in progress and handoff docs.
4. Run the focused preflight output check and whitespace checks.

## Test Plan

Automated checks:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `git diff --check`

## Completion Notes

Completed on: 2026-06-01

What changed:

- Updated `tools/Invoke-MvpPreflight.ps1` so successful preflight output suggests `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes`.
- Later packet `Fixture Acceptance Post-Pass Guidance` updated the success output to tell the user that the notes-enabled launcher will print exact summary and completion-check commands after it writes a notes file.
- Later packet `Fixture Acceptance Notes Embedded Commands` made the generated notes file include those commands too.
- Updated README and preflight docs to match the notes-enabled fixture-review workflow.
- Kept all preflight steps, fixture launcher behavior, Storage Scan behavior, fixture execution behavior, real-profile blockers, permanent deletion, and cleanup history unchanged.

Files changed:

- `tools/Invoke-MvpPreflight.ps1`
- `README.md`
- `docs/features/2026-05-28-mvp-preflight-script.md`
- `docs/features/2026-06-01-preflight-fixture-notes-next-step.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Invoke-MvpPreflight.cmd -SkipRestore`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `git diff --check`

Docs updated:

- README, preflight feature brief, this feature brief, progress log, and thread handoff.

ADRs added or skipped:

- No ADR added. This is local workflow-output polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Follow-up work:

- Run the visible fixture pass with `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` after a successful preflight when the user is ready, then run the embedded or printed summary and completion-check commands after filling notes.

Open questions:

- None for this packet.

Risky assumptions:

- The notes-enabled post-preflight command is the clearest next manual fixture step now that acceptance notes exist.
