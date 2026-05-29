# Feature: Scan Cancel Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Keep the WPF `Cancel` scan control's scope visible through tooltip and automation help text.

## Non-goals

- Do not change Storage Scan cancellation behavior.
- Do not change scan-gate behavior.
- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not enable permanent deletion or cleanup history.
- Do not move, restore, delete, quarantine, create, write, or clean up files or folders.

## User story / job story

As the project owner, I want the disabled and enabled `Cancel` scan control to explain what it does, so that scan cancellation is not mistaken for cleanup, undo, restore, or approval.

## Current behavior

`Cancel` is disabled before a Storage Scan starts and enabled while scanning. It requests cancellation through the scan cancellation token and reports that no files were modified, but the control did not expose tooltip or automation help text.

## Desired behavior

- `Cancel` explains that it requests cancellation of the in-progress read-only Storage Scan.
- `Cancel` explains that it does not move, delete, quarantine, restore, or approve cleanup.
- WPF smoke coverage pins the tooltip and automation help text wording while the control is disabled at startup.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Scan | Clarify cancel-control tooltip/help text expectation. | yes |

## Open questions

Questions that must be answered before implementation:

- None. Existing Storage Scan cancellation behavior is clear enough from code and tests.

Questions that can be deferred:

- During real-profile review, does the `Cancel` label need more visible explanatory text for long scans, or is tooltip/help text enough?

## Grill notes

### Scenarios discussed

- The user confirmed real-profile scans work and prefers small safety/readiness polish before any real cleanup execution.
- Long real-profile scans make cancellation discoverability useful, but cancellation should remain separate from cleanup execution or undo.

### Edge cases

- `Cancel` starts disabled before a scan.
- `Cancel` is enabled only while scanning.
- Cancellation can race with scan completion; the existing status text and scan state remain authoritative.

### Dependencies between decisions

- Depends on the existing read-only Storage Scan workflow.
- Does not affect Quarantine Preview, Quarantine execution, Undo Quarantine, selected restore, deletion, or cleanup history.

## Evidence and validation gate

Evidence gathered:

- User answers: real-profile scan works; real-profile cleanup must stay unavailable; small polish packets are preferred.
- Existing code/docs inspected: WPF `CancelButton`, scan cancellation token flow, WPF smoke tests, README, domain context, glossary, handoff, and progress log.
- Tests/checks planned: WPF app smoke test, build, diff checks.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not change cancellation semantics while adding help text.
- Do not add cleanup, undo, restore, or partial-results behavior to the cancel packet.

## Decisions made

Small feature-level decisions:

- Use disabled-state WPF tooltip plus matching `AutomationProperties.HelpText`.
- Keep wording focused on request-cancel behavior and no cleanup action.

ADR-worthy decisions:

- [x] None. This is reversible WPF metadata/test polish.

## Implementation plan

1. Add tooltip and automation help text to `Cancel`.
2. Add test-facing accessors and WPF smoke assertions.
3. Update README, domain docs, progress, handoff, and this feature brief.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- During the next fixture or real-profile review, hover/focus `Cancel` before and during a scan to confirm the wording fits comfortably.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git diff --check`

## Risks and assumptions

Risks:

- Tooltip text may be a little dense for the header area.

Assumptions:

- Tooltip and automation help text are enough for this small packet; visible copy can wait for manual review evidence.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added disabled-state tooltip and automation help text to the WPF `Cancel` scan control.
- Added test-facing accessors and WPF smoke assertions for read-only scan cancellation and no cleanup action wording.
- Updated README, domain docs, progress, handoff, and this feature brief.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-scan-cancel-help-text.md`
- `.codex/progress.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, handoff, this feature brief, and progress log.

ADRs added or skipped:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Confirm during visible fixture/real-profile review whether `Cancel` needs visible explanatory copy for long scans.
- Continue Quarantine Preview/readiness clarity or manual fixture review polish before any real-profile cleanup execution.

Open questions:

- Is tooltip/help text sufficient for scan cancellation, or should the header show an explicit cancellation note while scanning?

Risky assumptions:

- The current cancellation semantics are already acceptable and only discoverability needed improvement.
