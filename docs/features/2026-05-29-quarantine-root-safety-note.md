# Feature: Quarantine Root Safety Note

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make Quarantine Root Selection safer and clearer by explaining whether the typed or browsed preview root is usable before Quarantine Preview builds destination paths.

## Non-goals

- Do not create the quarantine folder.
- Do not move, delete, rename, or modify scanned files.
- Do not write a Restore Manifest.
- Do not persist the Quarantine Root Selection as a setting.
- Do not treat a usable preview root as cleanup approval.

## User story / job story

As the project owner, I want the app to tell me whether the Quarantine root is valid for preview, so that I can correct obvious path mistakes before reviewing destination paths for future cleanup.

## Current behavior

Quarantine Root Selection can be typed or browsed, and Quarantine Preview catches malformed roots only when preview is requested.

## Desired behavior

- The WPF toolbar shows a Quarantine Root Safety Note below the Quarantine root field.
- The default `D:\WindowsFileCleanerQuarantine` shows preferred preview-only wording.
- Fully qualified non-`D:` roots remain usable for preview but explain that `D:` is preferred.
- Relative or invalid roots disable `Preview quarantine` and explain that a fully qualified root is required.
- No folders are created and no scanned files are modified.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Root Selection | Clarified that preview requires a fully qualified root. | yes |
| Quarantine Root Safety Note | Added as read-only preview-root messaging and gating. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should actual Quarantine execution require an existing `D:` folder, offer to create it, or support a stricter destination policy?

## Grill notes

### Scenarios discussed

- The user prefers a quarantine folder on `D:` with an easy undo path.
- Quarantine remains preview-only in the current MVP.

### Edge cases

- Blank roots fall back to the default preview root.
- Relative roots are blocked before preview destination paths are built.
- Fully qualified non-`D:` roots are allowed for preview but marked as not preferred.

### Dependencies between decisions

- Actual Quarantine execution will need stronger destination validation later, but this packet only guards read-only preview path generation.

## Evidence and validation gate

Evidence gathered:

- User answers: quarantine should preferably be on `D:` and reversible.
- Existing code/docs inspected: Quarantine Preview, Quarantine Root Selection, Restore Manifest Draft, Quarantine Confirmation Draft, WPF smoke tests, README manual checklist, domain context, glossary, progress log.
- Tests/checks planned: core note-builder coverage, WPF default/relative-root coverage, build, test harnesses, MVP preflight, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not create or probe the quarantine folder just to decide whether preview can run.
- Do not require `D:` for preview, because non-`D:` fully qualified roots can still be useful during fixture review.

## Decisions made

Small feature-level decisions:

- Add a small core builder for Quarantine Root Safety Note so WPF display and preview gating share the same decision.
- Require fully qualified roots before creating Quarantine Preview destination paths.
- Preserve blank-root fallback to the default `D:\WindowsFileCleanerQuarantine` preview root.
- Keep non-`D:` roots previewable but visibly not preferred.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add core Quarantine Root Safety Note model and builder.
2. Show the note below the WPF Quarantine root controls.
3. Disable `Preview quarantine` when the current root cannot be used for preview.
4. Add core and WPF smoke coverage.
5. Update docs and progress.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/QuarantineRootSafetyNote.cs`
- `src/WindowsFileCleaner.Core/QuarantineRootSafetyNoteBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Possible:

- `README.md`

## Test plan

Manual checks:

- Launch the fixture app and confirm the default root note says the `D:` root is preferred.
- Type a relative root and confirm `Preview quarantine` disables without modifying files.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `git diff --check`

## Risks and assumptions

Risks:

- The added note makes the shortlist toolbar taller and still needs a visible fixture review.

Assumptions:

- Fully qualified path-shape validation is enough for read-only preview; execution-time code will validate filesystem state separately later.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added core Quarantine Root Safety Note model and builder.
- Added WPF safety-note text below Quarantine Root Selection.
- Disabled Quarantine Preview for relative or invalid roots.
- Preserved preview-only behavior and non-`D:` preview support.
- Later packet `2026-05-30-quarantine-root-safety-note-help-cue.md` added a visible hoverable `?` cue that mirrors the note tooltip/help text and keeps no-create/no-move/no-manifest-write/not-cleanup-approval wording discoverable.

Files changed:

- `src/WindowsFileCleaner.Core/QuarantineRootSafetyNote.cs`
- `src/WindowsFileCleaner.Core/QuarantineRootSafetyNoteBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-root-safety-note.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`

Docs updated:

- Domain context, glossary, README manual checklist, feature brief, and progress log.

ADRs added or skipped:

- No ADR. This is a reversible read-only preview guard and does not add cleanup execution, persistence, deployment, or manifest writes.

Follow-up work:

- In the next manual fixture pass, confirm the taller quarantine toolbar remains readable.

Open questions:

- Should actual Quarantine execution require an existing `D:` folder, offer to create it, or support a stricter destination policy?

Risky assumptions:

- Path-shape validation is sufficient before preview; execution-time validation will be designed separately before any file-moving code exists.
