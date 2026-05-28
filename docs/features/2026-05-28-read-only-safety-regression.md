# Feature: Read-only Safety Regression

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Add a fixture-style regression check that protects the Storage Scan MVP's read-only boundary.

## Non-goals

- Do not add cleanup execution.
- Do not remove CSV report export.
- Do not scan real user files from the test harness.
- Do not replace manual review or future execution design.

## User story / job story

As the project owner, I want tests to fail if production code quietly gains destructive filesystem calls, so that read-only Storage Scan work cannot accidentally become cleanup execution.

## Current behavior

The app has many explicit read-only workflows and tests, but there is no source-level regression guard for production file move/delete APIs.

## Desired behavior

The test harness scans production C# source under `src/` and fails if it finds obvious cleanup execution calls such as file deletion, file moves, directory moves, directory deletion, or production directory creation.

The guard allows the existing three `File.WriteAllText(dialog.FileName, ...)` calls because those are user-selected CSV report exports.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing read-only Storage Scan language used. | n/a |

## Open questions

Questions that must be answered before implementation:

- None. This is a verification-only packet.

Questions that can be deferred:

- Should future approved Quarantine execution add a more precise allowlist for its own file-moving implementation?

## Grill notes

### Scenarios discussed

- The MVP must remain read-only until explicit cleanup execution is designed and approved.
- Existing report exports write CSV files selected by the user and should stay allowed.
- Test fixtures create and delete temporary fixture files, but those are not production code.

### Edge cases

- A future cleanup executor should fail this guard until the project intentionally updates the safety model and docs.
- CSV exports should continue to be allowed only through user-selected dialog paths.

### Dependencies between decisions

- Depends on the current preview-only cleanup boundary.
- Future cleanup execution depends on separate ADR and test updates.

## Evidence and validation gate

Evidence gathered:

- Existing source inspected for filesystem write/delete/move calls.
- Test harness already uses fixture directories for scan verification.
- Tests/checks planned: source guard test, build, test harness, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not block user-selected CSV report writes.
- Do not scan docs or tests for banned APIs; fixtures are allowed to create/delete test files.
- Do not treat this guard as a substitute for execution design if cleanup actions are added later.

## Decisions made

Small feature-level decisions:

- Add a test harness check named `ProductionCodeDoesNotContainCleanupExecutionCalls`.
- Scan only production C# files under `src/`.
- Block obvious file and directory move/delete/write-bytes/create-directory APIs.
- Allow exactly three `File.WriteAllText(dialog.FileName, ...)` report exports.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add the source-level safety regression check to the test harness.
2. Verify it passes with the current production code.
3. Update docs and progress log.
4. Run build, tests, and diff checks.

## Files expected to change

Expected:

- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/features/2026-05-28-read-only-safety-regression.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- None needed for this test-only packet.

Automated tests:

- Verify production source contains no blocked cleanup execution API calls.
- Verify only the three user-selected CSV report writes use `File.WriteAllText`.
- Run build and full fixture test harness.

## Risks and assumptions

Risks:

- Source scanning is intentionally simple and may need refinement when future approved cleanup execution exists.

Assumptions:

- Blocking obvious destructive APIs in production source is useful evidence for the current read-only MVP boundary.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `ProductionCodeDoesNotContainCleanupExecutionCalls` to the test harness.
- The guard fails on obvious production file/directory move/delete/create-directory/write-bytes APIs.
- The guard allows exactly the three user-selected CSV report writes in WPF.

Files changed:

- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/features/2026-05-28-read-only-safety-regression.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- Added this feature brief.
- Updated progress log.

ADRs added or skipped:

- Skipped. This is a verification guard for the existing read-only boundary.

Follow-up work:

- Keep the guard in place until actual cleanup execution has explicit ADR and tests.
- If Quarantine execution is later approved, update the guard with a precise allowlist.

Open questions:

- Should future approved Quarantine execution add a more precise allowlist for its own file-moving implementation?

Risky assumptions:

- Simple source scanning is enough to catch accidental destructive API additions during MVP work.
