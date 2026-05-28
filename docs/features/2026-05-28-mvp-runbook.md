# Feature: MVP Runbook

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Add a user-facing README that explains how to verify, run, and manually check the read-only Storage Scan MVP.

## Non-goals

- Do not add cleanup execution.
- Do not run a real scan from automation.
- Do not replace feature briefs or ADRs.
- Do not claim the full cleanup product is complete.

## User story / job story

As the project owner, I want a short runbook in the repository root, so that I can safely build, test, run, and retest the app without remembering thread context.

## Current behavior

The repo has detailed Grill with Docs documentation and feature briefs, but the top-level README is still the generic scaffold README. There is no concise app-focused runbook for the current MVP.

## Desired behavior

The repo has `README.md` with:

- Current product purpose.
- Read-only safety status.
- Build/test commands to run before a real scan.
- WPF run command.
- Default Cleanup Scope.
- Manual MVP checklist for the real scan.
- Clear list of not-yet-implemented cleanup execution workflows.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing project vocabulary used. | n/a |

## Open questions

Questions that must be answered before implementation:

- None. This is a docs-only runbook.

Questions that can be deferred:

- Should packaging instructions be added after a release build exists?

## Grill notes

### Scenarios discussed

- The user needs to rerun the WPF app against `C:\Users\moxhe`.
- The app should remain read-only until explicit cleanup execution is designed.
- Future sessions need quick evidence of how to verify before scanning real files.

### Edge cases

- The README must not imply Quarantine execution exists.
- The README must keep CSV exports separate from cleanup actions.
- The README must tell the user to run fixture tests before a real scan.

### Dependencies between decisions

- Depends on the current read-only Storage Scan MVP.
- Future packaging or cleanup execution docs should update this README when those workflows exist.

## Evidence and validation gate

Evidence gathered:

- Existing docs inspected: AGENTS, progress log, scaffold README, manifest, feature briefs.
- Current repo has no `README.md`.
- Tests/checks planned: build, test harness, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not document cleanup execution commands before they exist.
- Do not replace the Grill with Docs scaffold docs.
- Do not claim the app is safe to delete files.

## Decisions made

Small feature-level decisions:

- Add a top-level `README.md`.
- Keep `README-codex-grill-with-docs.md` as the scaffold/template README.
- Include a manual real-scan checklist in the app README.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add `README.md`.
2. Add this feature brief.
3. Update progress log.
4. Run build, tests, and diff checks.

## Files expected to change

Expected:

- `README.md`
- `docs/features/2026-05-28-mvp-runbook.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Read the README and confirm it describes the current read-only MVP accurately.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- The README can become stale as execution features are added later.

Assumptions:

- A concise root README is useful even though detailed feature briefs already exist.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `README.md` with safety status, verification commands, run command, default Cleanup Scope, manual MVP checklist, current workflow, and not-yet-implemented cleanup workflows.
- Added this feature brief.
- Updated progress log.

Files changed:

- `README.md`
- `docs/features/2026-05-28-mvp-runbook.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- Added root MVP runbook.
- Added this feature brief.
- Updated progress log.

ADRs added or skipped:

- Skipped. This is a documentation runbook for the current MVP.

Follow-up work:

- Retest the WPF app with the manual checklist.
- Update README when cleanup execution, Undo Quarantine, or packaging is designed.

Open questions:

- Should packaging instructions be added after a release build exists?

Risky assumptions:

- The current README checklist is enough for the next manual WPF retest.
