# Feature: Fixture Acceptance Build Context Header

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Make ignored fixture acceptance notes capture the local .NET/WPF project context for the next visible fixture pass, alongside the existing Git and preflight evidence.

## Non-goals

- Do not launch WPF.
- Do not create synthetic fixture files in checklist-only checks.
- Do not scan `C:\Users\moxhe`.
- Do not move, restore, delete, create, or rewrite real-profile files.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not turn ignored `.local` notes into persisted cleanup history or tracked acceptance records.
- Do not create a release package or installer.

## User Story / Job Story

As the local app owner, I want fixture acceptance notes to include the SDK and WPF project context, so that the manual fixture pass is tied to both the commit and the local app build shape being reviewed.

## Current Behavior

Generated acceptance notes stamped repository path, Git branch/commit, required preflight command, visible fixture command, preflight/worktree checkboxes, and local-not-cleanup-history wording.

The notes did not record the local .NET SDK version or the WPF app project/target framework evidence.

## Desired Behavior

Generated fixture acceptance notes should include:

- .NET SDK version from `dotnet --version`,
- WPF app project path,
- WPF app target framework,
- WPF app `UseWPF` flag.

If any evidence cannot be read, notes should still be written with `unknown` values.

## Domain Language Changes

No new durable domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing fixture acceptance, WPF, and preflight terms used. | n/a |

## Open Questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should a future release-readiness packet add built executable version/file hash evidence once packaging exists?

## Grill Notes

### Scenarios Discussed

- The next acceptance gate is still the visible fixture pass.
- Local ignored notes are acceptance evidence only, not cleanup history or app persistence.
- Build context helps connect the visual pass to the .NET/WPF app shape without launching WPF.

### Edge Cases

- `dotnet` may be unavailable or fail in a future shell; notes should still be generated.
- The WPF project file may move or change; notes should still be generated with `unknown` fields.
- Checklist-only notes generation must still avoid preflight, fixture creation, WPF launch, scans, movement, restore, delete, and cleanup history.

### Dependencies Between Decisions

- This builds on the existing fixture acceptance notes evidence header.
- This supports the live-product readiness roadmap without changing ADR 0017, ADR 0018, or ADR 0019.

## Evidence And Validation Gate

Evidence gathered:

- Existing docs inspected:
  - `README.md`
  - `.codex/progress.md`
  - `docs/codex/thread-handoff.md`
  - `docs/features/2026-06-01-live-product-readiness-roadmap.md`
  - `docs/features/2026-06-01-fixture-acceptance-evidence-header.md`
  - `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
  - ADR 0017, ADR 0018, and ADR 0019
- Existing code inspected:
  - `tools/Start-MvpFixtureReview.ps1`
  - `src/WindowsFileCleaner.App/WindowsFileCleaner.App.csproj`

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or deferred.

Rejected ideas buffer:

- Do not build or hash the WPF executable in this packet; packaging/release evidence belongs to a later release-readiness packet.
- Do not make `.local` notes tracked product history.

## Decisions Made

Small feature-level decisions:

- Read `.NET SDK` from `dotnet --version`.
- Read WPF app project properties from the `.csproj` XML.
- Use `unknown` fallback for command or project evidence failures.

ADR-worthy decisions:

- [x] None.

## Implementation Plan

1. Add narrow command/project evidence helpers to `Start-MvpFixtureReview.ps1`.
2. Add .NET SDK and WPF project evidence to generated notes.
3. Update README, feature notes, roadmap, progress, and handoff docs.
4. Run focused launcher and diff checks.

## Files Expected To Change

Expected:

- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-build-context-header.md`
- `docs/features/2026-06-01-fixture-acceptance-evidence-header.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test Plan

Manual checks:

- Inspect the generated `.local\fixture-review-acceptance\fixture-acceptance-*.md` header.

Automated checks:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `git diff --check`

## Risks And Assumptions

Risks:

- Build-context fields could be mistaken for proof that a release package exists. The wording stays tied to fixture acceptance notes only.
- Tooling evidence can be unavailable in unusual environments; `unknown` fallback keeps notes generation useful.

Assumptions:

- SDK/project evidence is enough for the next manual fixture pass; executable version/hash evidence can wait for packaging.

## Completion Notes

Completed on: 2026-06-01

What changed:

- Added .NET SDK, WPF app project, WPF app target framework, and WPF enabled fields to generated fixture acceptance notes.
- Kept checklist-only behavior, fixture creation, WPF launch, Storage Scan, fixture execution, restore behavior, real-profile/custom blockers, permanent deletion, and cleanup history unchanged.
- Later packet `Full Local MVP Preflight After Build Context Header` confirmed the full `.cmd` MVP preflight still passes after this launcher/docs change.

Files changed:

- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/features/2026-06-01-fixture-acceptance-build-context-header.md`
- `docs/features/2026-06-01-fixture-acceptance-evidence-header.md`
- `docs/features/2026-06-01-fixture-acceptance-notes-template.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly -WriteAcceptanceNotes`
- Inspected `.local\fixture-review-acceptance\fixture-acceptance-20260601-111237.md` and confirmed it includes .NET SDK `8.0.421`, WPF app project, `net8.0-windows`, and `WPF enabled: true`.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -WhatIf -SkipPreflight -SkipLaunch -WriteAcceptanceNotes`
- `git diff --check`
- Later full-preflight packet: `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and whitespace diff without scanning or modifying real user files.

Docs updated:

- README, this feature brief, fixture acceptance evidence/notes briefs, live-product roadmap, progress log, and thread handoff.

ADRs added or skipped:

- No ADR added. This is local fixture-review evidence capture with no architecture, persistence, cleanup execution, restore behavior, data-model, or security policy change.

Follow-up work:

- Run the visible fixture pass with `.\tools\Start-MvpFixtureReview.cmd -SkipPreflight -WriteAcceptanceNotes` after a successful preflight when the user is ready.
- Add executable version/file hash evidence only when release packaging exists.

Open questions:

- Whether the first live release should include packaging before or after first reversible real-profile cleanup.

Risky assumptions:

- Local SDK/project evidence is useful acceptance context without making the notes look like release packaging or cleanup history.
