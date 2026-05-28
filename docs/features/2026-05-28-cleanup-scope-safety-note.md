# Feature: Cleanup Scope Safety Note

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make the app visibly distinguish fixture, real-profile, and custom Cleanup Scopes before the user clicks `Scan`.

## Non-goals

- Do not block scanning.
- Do not run preflight from the WPF app.
- Do not auto-create fixtures.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.

## User story / job story

As the project owner, I want the app to remind me when I am about to scan the real profile, so that fixture review stays part of the workflow before real user files are scanned.

## Current behavior

The WPF app shows a Cleanup Scope path field and waits for the user to click `Scan`. The README and launcher document fixture-first review, but the app itself does not explain whether the current scope is a synthetic fixture or a real profile path.

## Desired behavior

The WPF app should show a read-only Cleanup Scope Safety Note below the path field:

- Fixture Cleanup Scope for synthetic smoke-test paths.
- Real Profile Cleanup Scope for `C:\Users\moxhe` and child paths.
- Custom Cleanup Scope for other paths.
- Choose Cleanup Scope for blank input.
- Check Cleanup Scope for paths that cannot be normalized.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope Safety Note | Added as read-only UI text explaining the current scope safety context. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should a future release record the last successful preflight timestamp for local-only display?

## Grill notes

### Scenarios discussed

- The project requires fixture-based verification before real-profile scans.
- The user has already performed a real scan, so the app should make future fixture-vs-real scope context more visible.

### Edge cases

- The note should not claim preflight was run.
- The note should not block a scan or imply scan approval.
- Blank or invalid paths should not throw while the user is editing the text box.

### Dependencies between decisions

- This depends on the existing `--scope` launch option and fixture launcher.
- This does not change scanner behavior or cleanup readiness.

## Evidence and validation gate

Evidence gathered:

- Existing WPF startup and scope-entry code.
- Existing launch-scope parser and fixture launcher behavior.
- README preflight and fixture review workflow.

Tests/checks planned:

- Core builder coverage for real profile, fixture, custom, and blank scope notes.
- WPF startup smoke coverage for default real-profile and fixture launch notes.
- Build and both test harnesses.
- `git diff --check`.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not use the note as proof that preflight ran.
- Do not make the note run shell commands or create fixture files.
- Do not add a blocking modal before the user has tested the visible fixture workflow.

## Decisions made

Small feature-level decisions:

- Implement note classification in core so tests do not depend on WPF visuals.
- Keep the WPF note informational and always visible under the Cleanup Scope controls.
- Treat child paths under `C:\Users\moxhe` as real-profile scope.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add `CleanupScopeSafetyNote` and `CleanupScopeSafetyNoteBuilder`.
2. Add a WPF note under the Cleanup Scope controls and update it when the path changes.
3. Add core and WPF smoke coverage.
4. Update README, domain docs, and progress log.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/CleanupScopeSafetyNote.cs`
- `src/WindowsFileCleaner.Core/CleanupScopeSafetyNoteBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Run fixture launcher and confirm the app shows Fixture Cleanup Scope.
- Run the app normally and confirm the app shows Real Profile Cleanup Scope.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- The note improves visibility but cannot prove the user actually ran preflight.

Assumptions:

- A visible reminder is useful without blocking scans.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added Cleanup Scope Safety Note classification in core.
- Added a WPF scope note below the Cleanup Scope controls.
- Added tests for real-profile, fixture, custom, blank, and WPF startup notes.
- Later added Cleanup Scope Scan Gate so real-profile scans require explicit preflight and fixture-review acknowledgement before `Scan` is enabled.

Files changed:

- `src/WindowsFileCleaner.Core/CleanupScopeSafetyNote.cs`
- `src/WindowsFileCleaner.Core/CleanupScopeSafetyNoteBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-safety-note.md`
- `.codex/progress.md`

Tests run:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-cleanup-scope-safety-note.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is a reversible read-only UI reminder and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Include Cleanup Scope Safety Note in the next visible fixture UI pass.

Open questions:

- Should a future release record the last successful preflight timestamp for local-only display?

Risky assumptions:

- The note alone is not sufficient as proof that preflight happened; the later scan gate adds explicit acknowledgement but still does not run preflight from WPF.
