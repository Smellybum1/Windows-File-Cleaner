# Feature: Real-Profile Quarantine Readiness Contract

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the boundary before real-profile Quarantine execution explicit before any WPF path can move files from `C:\Users\moxhe`.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine or selected real-profile restore.
- Do not add permanent deletion, quarantine-folder cleanup, or persisted cleanup history.
- Do not scan, move, restore, delete, create, write, or clean up real-profile files.
- Do not change fixture-only Quarantine execution or fixture-only selected restore behavior.

## Current behavior

- Storage Scan remains read-only.
- Quarantine Preview and readiness panes are dry-run review surfaces.
- WPF Quarantine execution can open only for recognized fixture Cleanup Scopes after preview readiness and exact `QUARANTINE`.
- Real-profile and custom non-fixture Cleanup Scopes remain preview-only even when preview is clean and exact `QUARANTINE` is typed.
- Core Quarantine and Undo Quarantine executors are fixture-tested, but WPF real-profile execution and WPF real-profile Undo Quarantine remain unavailable.

## Desired behavior

- Future real-profile Quarantine work must start from a named readiness contract rather than toggling the fixture-only implementation flag.
- The current build must continue to block real-profile and custom execution from the WPF app.
- Regression coverage should prove real-profile acknowledgement unlocks only read-only scanning, and custom non-fixture clean preview plus exact `QUARANTINE` still cannot execute.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Quarantine Readiness Contract | Added as the durable prerequisites for enabling real-profile WPF Quarantine execution later. | yes |

## Evidence and validation gate

Evidence gathered:

- ADR 0009 keeps WPF Quarantine execution fixture-only.
- ADRs 0005-0008 define write-ahead Restore Manifest, action-scoped layout, fixture-first Quarantine Executor, and fixture-first Undo Quarantine.
- ADRs 0010, 0015, and 0016 keep WPF undo/selected restore fixture-scoped and avoid implying broad history or real-profile restore.
- WPF code currently derives execution availability from `CleanupScopeSafetyNoteBuilder.Build(...).IsFixtureScope`.
- Existing custom-scope WPF coverage proved the visible gate stayed closed; this packet strengthens the boundary by attempting execution after exact confirmation.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission and cleanup-execution lifecycle boundaries are clear enough.
- [x] The narrowest relevant verification path is WPF app tests plus whitespace diff.
- [x] Open questions are deferred to the future real-profile execution design packet.

Rejected ideas buffer:

- Do not enable real-profile movement by passing `isExecutionImplemented: true` from the current WPF bool.
- Do not treat real-profile scan acknowledgement, Review Shortlist, Quarantine Preview, or exact `QUARANTINE` as enough to move real files.
- Do not implement permanent deletion before trusted real-profile Quarantine and Undo Quarantine exist.

## Decisions made

Small feature-level decisions:

- Add a non-scanning WPF real-profile guard test so the default real-profile path remains protected without scanning `C:\Users\moxhe`.
- Strengthen the custom non-fixture test as the safe surrogate for clean preview plus exact `QUARANTINE`, including an execution-method attempt that must report a closed gate.

ADR-worthy decisions:

- [x] ADR needed: `docs/decisions/0017-real-profile-quarantine-readiness-contract.md`

## Implementation plan

1. Add ADR 0017 for the real-profile Quarantine readiness contract.
2. Add this feature brief with the current boundary, non-goals, readiness criteria, and follow-up ladder.
3. Add WPF regression coverage for real-profile no-scan execution unavailability and custom non-fixture closed-gate execution attempts.
4. Update domain docs, README, progress, and handoff.

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- None required for this docs-and-regression packet.

## Risks and assumptions

Risks:

- The real-profile WPF path still needs a separate design before it can move files.
- Testing a clean real-profile preview would require scanning real-profile data, so this packet avoids that and uses custom non-fixture coverage for the exact-confirmation execution blocker.

Assumptions:

- The next safest full-app-readiness step is to document and test the closed boundary before designing real-profile movement.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added ADR 0017 and this feature brief for the readiness contract.
- Added WPF regression coverage that real-profile acknowledgement unlocks only read-only scanning without scanning the real profile.
- Strengthened custom non-fixture coverage so clean preview plus exact `QUARANTINE` still leaves `Quarantine included shortlist` closed and execution attempts report no file modifications.
- Updated domain docs, README, progress, and handoff.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0017-real-profile-quarantine-readiness-contract.md`
- `docs/features/2026-05-30-real-profile-quarantine-readiness-contract.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs added or skipped:

- Added ADR 0017 because this is a durable core UX and safety-gate decision with multiple plausible future paths.

Follow-up work:

- Design a richer execution availability/readiness model before real-profile movement.
- Add immediate pre-execution revalidation, quarantine-root safety checks, Undo Quarantine readiness, and recovery UX before enabling real-profile WPF Quarantine execution.

Open questions:

- What exact user approval sequence should open real-profile Quarantine after readiness exists?
- Should real-profile execution require a fresh full MVP preflight immediately before each execution session?

Risky assumptions:

- Custom non-fixture WPF coverage is the safest automated surrogate for clean-preview blocked execution without scanning or touching real-profile files.
