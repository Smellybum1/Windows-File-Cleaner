# Feature: Quarantine Confirmation Draft

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Define and test the read-only confirmation-readiness shape that future Quarantine execution must pass before any files can move.

## Non-goals

- Do not create the quarantine folder.
- Do not move, rename, delete, or modify scanned files.
- Do not write a Restore Manifest file.
- Do not add a Quarantine execution button.
- Do not treat readiness as user approval.

## User story / job story

As the project owner, I want the app to check preview and undo metadata before asking me to approve quarantine, so that file-moving work is gated by exact paths, counts, bytes, and blockers.

## Current behavior

The app can create a Quarantine Preview and an in-memory Restore Manifest Draft. It does not yet compare those two artifacts as a confirmation gate.

## Desired behavior

The core library can build an in-memory Quarantine Confirmation Draft from a Quarantine Preview and Restore Manifest Draft. The draft records included counts and bytes, blocked and redundant row counts, the Restore Manifest Draft id, the future confirmation phrase, review notes, and data blockers when preview and manifest metadata disagree.

The draft remains read-only and states that Quarantine execution is not implemented.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Confirmation Draft | Added as the in-memory readiness check before future Quarantine execution. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This packet is read-only and does not add execution.

Questions that can be deferred:

- What exact UI should ask for the confirmation phrase?
- Should confirmation require a typed phrase plus a selected manifest path?
- Should future execution require hashes before moving files?

## Grill notes

### Scenarios discussed

- The user wants Quarantine on `D:` with easy undo.
- The app now has Quarantine Preview and Restore Manifest Draft.
- Before any future file move, the app needs a gate that proves the preview and undo metadata agree.

### Edge cases

- Block confirmation readiness when a preview has no included rows.
- Block confirmation readiness when blocked or redundant preview rows remain.
- Block confirmation readiness when the Restore Manifest Draft does not match preview scope, root, entry count, bytes, or destination paths.

### Dependencies between decisions

- Depends on Quarantine Preview.
- Depends on Restore Manifest Draft.
- Future Quarantine execution depends on a separate explicit execution design.

## Evidence and validation gate

Evidence gathered:

- Existing code/docs inspected: Quarantine Preview, Restore Manifest Draft, ADR 0003, project safety rules.
- Tests/checks planned: confirmation draft readiness test, blocker test, build, test harness, diff check.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not call this a cleanup plan.
- Do not make execution availability depend only on a boolean.
- Do not treat the confirmation phrase as sufficient without matching preview and manifest data.

## Decisions made

Small feature-level decisions:

- Add `QuarantineConfirmationDraft` and `QuarantineConfirmationDraftBuilder`.
- Use `QUARANTINE` as the default future confirmation phrase.
- Separate data blockers from review notes.
- Keep `IsExecutionImplemented` false.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add the core confirmation draft record and builder.
2. Add fixture coverage for matching preview/manifest readiness.
3. Add fixture coverage for blocked/redundant preview rows and manifest mismatches.
4. Update domain docs and progress log.
5. Run build, tests, and diff checks.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/QuarantineConfirmationDraft.cs`
- `src/WindowsFileCleaner.Core/QuarantineConfirmationDraftBuilder.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- None needed for this core-only packet.

Automated tests:

- Verify a matching preview and Restore Manifest Draft produce no data blockers.
- Verify the draft exposes counts, bytes, manifest draft id, and confirmation phrase.
- Verify blocked/redundant preview rows produce blockers.
- Verify manifest mismatches produce blockers.
- Verify no quarantine folder is created.

## Risks and assumptions

Risks:

- The word "confirmation" could imply approval unless UI and docs keep calling this a draft/readiness check.

Assumptions:

- `QUARANTINE` is a reasonable first confirmation phrase for future execution design.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added core Quarantine Confirmation Draft records and builder.
- Added data blocker checks for preview rows, scope/root mismatches, schema version, entry counts, bytes, destinations, and stray manifest rows.
- Added tests for clean readiness and blocker reporting.
- Kept execution unavailable and file operations absent.

Files changed:

- `src/WindowsFileCleaner.Core/QuarantineConfirmationDraft.cs`
- `src/WindowsFileCleaner.Core/QuarantineConfirmationDraftBuilder.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- Added Quarantine Confirmation Draft to domain context and glossary.
- Added this feature brief.
- Updated the progress log.

ADRs added or skipped:

- Skipped. This is a read-only draft gate; the durable execution flow should get ADR review when it is designed.

Follow-up work:

- WPF display for Quarantine Confirmation Draft has been added in the Quarantine Readiness UI packet.
- Design actual Quarantine execution and Undo Quarantine only after confirmation UI semantics are reviewed.

Open questions:

- What exact UI should ask for the confirmation phrase?
- Should future execution require hashes before moving files?

Risky assumptions:

- `QUARANTINE` is a reasonable first confirmation phrase for future execution design.
