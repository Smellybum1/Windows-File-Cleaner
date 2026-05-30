# Feature: Compact Scan Header Status

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Recover vertical review space by flattening the WPF header's Cleanup Scope safety and scan-gate status text.

## Non-goals

- Do not change Cleanup Scope Scan Gate behavior.
- Do not change Cleanup Scope Safety Note wording or help text semantics.
- Do not run MVP preflight from the WPF app.
- Do not start scans automatically.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## User story / job story

As the project owner, I want the header safety/status text to use the empty horizontal space in the window, so that more of the scan summary, review controls, and grid stay visible underneath.

## Current behavior

The path controls, Cleanup Scope Safety Note, scan-gate summary, optional real-profile acknowledgement, and scan-gate detail were stacked in the top-right header. On a wide monitor this left unused space in the middle while consuming vertical room.

## Desired behavior

The path controls stay in the top-right header. The Cleanup Scope Safety Note, scan-gate summary, and scan-gate detail use one wrapping status strip across the header so wide windows can show them more horizontally and compactly. The real-profile acknowledgement remains visible only for real-profile Cleanup Scopes.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- User manually completed fixture steps 1-10 after the redundant overlap cleanup packet.
- User screenshot showed large unused middle header space and stacked right-side status text consuming vertical room.
- Existing scan-gate and Cleanup Scope Safety Note feature briefs require the same visible safety/help boundaries to stay available.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is the WPF app smoke test harness.
- [x] Open questions are either answered or explicitly deferred.

## Decisions made

Small feature-level decisions:

- Use a wrapping header status strip instead of removing any safety text.
- Keep the Cleanup Scope Safety Note and scan-gate `?` help cues paired with their text.
- Keep the real-profile acknowledgement as its own row because it is only needed for real-profile scopes.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Move the WPF header status text into a shared wrapping strip.
2. Add a focused WPF smoke assertion for the wrapping status layout.
3. Update checklist, README, handoff, and progress notes.

## Test plan

Manual checks:

- Run the fixture launcher and confirm the header uses less vertical space at the current monitor width.
- Confirm the Cleanup Scope Safety Note `?` cue and scan-gate `?` cue remain paired with their text.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Completion notes

Completed on: 2026-05-30

What changed:

- Flattened the WPF header so the Cleanup Scope Safety Note, scan-gate summary, and scan-gate detail share one wrapping status strip.
- Kept existing safety wording, tooltips, automation help text, and help cues.
- Added WPF smoke coverage that the header status area uses wrapping layout.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`
- `docs/features/2026-05-30-compact-scan-header-status.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj` passed.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` passed and printed the compact wrapping header status prompt without preflight, fixture creation, WPF launch, scanning, or file modification.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed with line-ending normalization warnings only.

ADRs added or skipped:

- Skipped. This is reversible WPF layout polish with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Follow-up work:

- During the next visible fixture pass, confirm the compact header gives enough room back to the review area without making the safety/status cues feel crowded.

Open questions:

- None.

Risky assumptions:

- Fixed-width status groups inside a wrapping strip are acceptable for the current desktop target and will wrap gracefully on narrower windows.
