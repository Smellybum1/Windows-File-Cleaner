# Feature: Real-Profile Quarantine Design Pass

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Design the next safety layer for real-profile Quarantine without enabling real-profile file movement.

The output of this pass is a proposed contract future implementation can follow: what readiness must mean, what must be revalidated, what recovery must exist, and what user decisions remain before `C:\Users\moxhe` files can ever be moved by the app.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not enable custom non-fixture Quarantine execution.
- Do not enable permanent deletion or persisted cleanup history.
- Do not move, delete, restore, create, or rewrite real-profile files.
- Do not change fixture-only Quarantine execution behavior.

## User story / job story

As the local app owner, I want the app to define the exact real-profile Quarantine safety contract before any real files move, so that future cleanup execution can be implemented in small, reviewable packets without weakening the current read-only boundary.

## Current behavior

- Storage Scan is read-only.
- Fixture-only WPF Quarantine execution can move files only from recognized synthetic fixture Cleanup Scopes.
- Fixture-only current Undo Quarantine and fixture-only selected restore are available.
- Real-profile and custom non-fixture Quarantine execution stay preview-only even with a clean Quarantine Preview and exact `QUARANTINE`.
- Quarantine Root Safety Note is preview-only and does not check free space, action collisions, or whether the root is execution-safe.
- The current execution gate exposes a coarse execution-availability flag, which is sufficient for the fixture boundary but not for real-profile movement.

## Desired behavior

Future real-profile Quarantine implementation should use a composite readiness contract before enabling movement:

- Scope eligibility: first real-profile phase targets the recognized real-profile Cleanup Scope only. Custom non-fixture scopes remain preview-only unless a later design includes them.
- Review readiness: Review Shortlist, Quarantine Preview, Restore Manifest Draft, Quarantine Confirmation Draft, and Quarantine Action Draft must agree with no blockers or redundant parent/child overlap.
- Quarantine Root Execution Safety: the root must be safe for execution, not merely usable for preview.
- Pre-Execution Revalidation: immediately before execution, every included source, planned destination, action root, and manifest path must be checked against the live filesystem.
- Recovery readiness: real-profile Restore Manifest write-ahead behavior, partial-failure guidance, recovery-review states, and selected-manifest real-profile Undo Quarantine readiness must exist before forward movement.
- Explicit real-profile approval: exact `QUARANTINE` remains necessary but is not sufficient. The real-profile phase needs a stronger, real-profile-specific approval step or phrase.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Quarantine Execution Readiness | Added as the composite readiness result future real-profile execution must use. | yes |
| Quarantine Root Execution Safety | Added as execution-specific root validation, separate from preview-only Quarantine Root Safety Note. | yes |
| Pre-Execution Revalidation | Added as the immediate live-filesystem check run after approval and before movement. | yes |
| Real-Profile Restore Readiness | Added as the recovery prerequisite for real-profile forward movement. | yes |

## Open questions

Questions that must be answered before implementation:

- Should the first real-profile execution phase support only `C:\Users\moxhe`, leaving all custom non-fixture Cleanup Scopes preview-only? Recommendation: yes.
- Should the first real-profile execution phase require a `D:` Quarantine Root instead of allowing a non-`D:` override? Recommendation: require `D:` first.
- What first-batch cap should apply to real-profile movement, if any? Recommendation: start with a small explicit row and byte cap.
- Should selected-manifest real-profile Undo Quarantine execution be implemented before the first forward real-profile Quarantine action? Recommendation: yes.
- What real-profile approval phrase should be required beyond exact `QUARANTINE`? Recommendation: use a phrase that names real-profile movement, such as `QUARANTINE REAL PROFILE`.

Questions that can be deferred:

- Should custom non-fixture Cleanup Scopes ever become executable?
- Should all-manifest real-profile restore exist, or should real-profile restore stay selected-manifest-only?
- Should permanent deletion exist after quarantine aging, or stay outside the app?

## Grill notes

### Scenarios discussed

- The user successfully completed fixture-review steps 1 through 10.
- The user noticed that redundant parent/child overlap made the `Quarantine included shortlist` button stay disabled after typing `QUARANTINE`, which led to the `Remove overlapping parents` workflow.
- The user asked whether overlap should be explained instead of feeling like a mysterious error.
- The project is now moving toward full-app readiness, but still with a local-first safety boundary.

### Edge cases

- A source path existed at preview time but is deleted, replaced, changed, or becomes a reparse point before execution.
- A planned destination appears after preview and before execution.
- A Quarantine Root is fully qualified but inside the Cleanup Scope, or is a parent of the Cleanup Scope.
- A manifest can be written initially but later manifest updates fail after some files move.
- A restore target original path reappears after quarantine, so Undo Quarantine must refuse to overwrite it.
- A broad parent row would move descendants that are protected, high-risk, inaccessible, or current-tooling-related.

### Dependencies between decisions

- Real-profile forward execution depends on restore/recovery design, not just preview readiness.
- Quarantine Root execution safety depends on deciding whether `D:` is mandatory for the first real-profile phase.
- Real-profile approval wording depends on how much risk the UI should make the user restate before movement.
- Batch caps depend on the first execution scope and how much manual review evidence the user wants per action.

## Evidence and validation gate

Evidence gathered:

- User answers:
  - Real-profile scan works.
  - Fixture review steps 1 through 10 worked.
  - Redundant overlap blockers should be clearer and easier to resolve.
  - The user wants a real-profile Quarantine design pass before implementation.
- Existing code/docs inspected:
  - ADRs 0004, 0005, 0007, 0008, 0009, 0010, 0015, and 0017.
  - Quarantine Preview, Confirmation Draft, Execution Gate, Action Draft, Restore Manifest, Quarantine Executor, Undo Quarantine Executor, Restore Readiness Preview, Selected Restore Confirmation Draft, Quarantine Root Safety Note, and WPF gate wiring.
- Tests/checks planned:
  - Docs-only whitespace diff check for this packet.
  - Future code packets should add targeted core and WPF tests before any behavior change.

Validation gate before implementation:

- [x] Domain terms are clear enough for the design pass.
- [x] Required lifecycle, permission, and persistence rules are clear enough for a proposed ADR.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are explicitly deferred before implementation.

Rejected ideas buffer:

- Do not enable real-profile Quarantine by setting the current fixture-only execution flag to true.
- Do not treat exact `QUARANTINE` as enough real-profile approval.
- Do not treat Quarantine Root Safety Note as execution-root validation.
- Do not rely on a previous scan/preview without immediate pre-execution revalidation.
- Do not expose real-profile forward movement before the app has trusted restore/recovery behavior.

## Decisions made

Small feature-level decisions:

- This packet is documentation and design only.
- New terms are added as draft domain language so future code can use one vocabulary.
- Future implementation should be staged so each readiness dimension can be tested while movement remains blocked.

ADR-worthy decisions:

- [ ] None
- [x] ADR needed: ADR 0018 proposes a composite real-profile Quarantine execution readiness model.

## Implementation plan

1. Add `QuarantineExecutionReadiness` core model and tests that report fixture-executable, real-profile-candidate, and custom-preview-only states without changing WPF execution availability.
2. Add `QuarantineRootExecutionSafety` checks and tests for fully qualified path, preferred `D:` execution root, root/scope containment, free space, action-root collision, and item destination collision.
3. Add Pre-Execution Revalidation tests using synthetic fixtures for missing source, changed source, reparse source, outside-scope source, destination collision, stale preview, redundant overlap, and manifest/action mismatch.
4. Add WPF readiness output that names missing real-profile prerequisites while keeping real-profile execution disabled.
5. Design and test selected-manifest real-profile Restore Readiness and Undo Quarantine before forward real-profile movement.
6. After an explicit user-approved Grill with Docs pass, enable the first real-profile Quarantine action under the decided scope, root, approval, and batch-cap rules.

## Files expected to change

Expected:

- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/features/2026-05-31-real-profile-quarantine-design-pass.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Possible future implementation files:

- `src/WindowsFileCleaner.Core/QuarantineExecutionReadiness.cs`
- `src/WindowsFileCleaner.Core/QuarantineRootExecutionSafety.cs`
- `src/WindowsFileCleaner.Core/PreExecutionRevalidation*.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/*`
- `tests/WindowsFileCleaner.App.Tests/*`

## Test plan

Manual checks:

- Review the proposed open questions before any implementation packet.
- Keep running fixture review before any later real-profile movement work.

Automated tests:

- Current docs-only packet: `git diff --check`.
- Future core packets: `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`.
- Future WPF packets: `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`.
- Checklist wording packets only: `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`.

## Risks and assumptions

Risks:

- The readiness model could become too noisy unless WPF groups blockers by readiness dimension.
- Revalidation could block legitimate cleanup if source metadata changes for harmless reasons.
- Requiring `D:` for the first real-profile phase may be too strict for some future machines, but it matches the current project preference.

Assumptions:

- The first real-profile target remains `C:\Users\moxhe`.
- Permanent deletion remains out of scope.
- The user prefers slower safety proof over fast execution enablement.

## Completion notes

Completed on: 2026-05-31

What changed:

- Added a proposed ADR for the composite Real-Profile Quarantine Execution Readiness model.
- Added this feature brief with readiness dimensions, implementation staging, open questions, and non-goals.
- Added draft domain/glossary terms for execution readiness, root execution safety, pre-execution revalidation, and restore readiness.
- Updated handoff/progress docs so the next packet can start from the design contract.

Files changed:

- `docs/decisions/0018-use-real-profile-quarantine-execution-readiness-model.md`
- `docs/features/2026-05-31-real-profile-quarantine-design-pass.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Tests run:

- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- Domain context, glossary, feature brief, ADR, progress log, and thread handoff.

ADRs added or skipped:

- Added proposed ADR 0018.

Follow-up work:

- Decide first real-profile scope limit, root rule, batch cap, real-profile approval phrase, and restore prerequisite.
- Implement the core readiness model while keeping real-profile movement blocked.

Open questions:

- See the Open questions section above.

Risky assumptions:

- This design assumes no real-profile movement happens until the user explicitly approves a later implementation packet after reviewing these open questions.
