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

- Scope eligibility: first real-profile phase targets exact `C:\Users\moxhe` only. Child scopes, `ProgramData`, `Program Files`, and custom non-fixture scopes remain preview-only unless a later design includes them.
- Review readiness: Review Shortlist, Quarantine Preview, Restore Manifest Draft, Quarantine Confirmation Draft, and Quarantine Action Draft must agree with no blockers or redundant parent/child overlap.
- First-phase eligibility: real-profile execution is capped at 10 included rows and 1 GB, limited to `Likely safe` + `Quarantine candidate` rows, and may include files plus narrow folders only when strict descendant checks pass.
- Quarantine Root Execution Safety: the root must be safe for execution, not merely usable for preview. `D:` is default/preferred; non-`D:` roots are allowed only with an extra acknowledgement after all other root safety checks pass.
- Pre-Execution Revalidation: immediately before execution, every included source, planned destination, action root, and manifest path must be checked against the live filesystem.
- Recovery readiness: real-profile Restore Manifest write-ahead behavior, partial-failure guidance, recovery-review states, and selected-manifest real-profile Undo Quarantine readiness must exist before forward movement.
- Explicit confirmation and approval: exact `QUARANTINE` remains the typed confirmation phrase. It is necessary but not sufficient; all readiness dimensions must still pass.
- Post-execution behavior: after future real-profile movement, show stale-scan guidance and ask the user to rescan manually.
- Durable record: Restore Manifest remains the only durable cleanup record for now.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Quarantine Execution Readiness | Added as the composite readiness result future real-profile execution must use. | yes |
| Quarantine Root Execution Safety | Added as execution-specific root validation, separate from preview-only Quarantine Root Safety Note. | yes |
| Pre-Execution Revalidation | Added as the immediate live-filesystem check run after approval and before movement. | yes |
| Real-Profile Restore Readiness | Added as the recovery prerequisite for real-profile forward movement. | yes |

## Open questions

Answered before implementation:

- First real-profile execution phase supports only exact `C:\Users\moxhe`; custom non-fixture scopes remain preview-only.
- Other folders can be considered later: `ProgramData` should be a separate cautious mode, while `Program Files` should prefer report/uninstall guidance before file movement.
- `D:` is the default/preferred Quarantine Root, but the user can choose another safe fully qualified root.
- Non-`D:` roots are allowed only with an extra acknowledgement; unsafe roots stay blocked.
- First real-profile execution is capped at 10 included rows and 1 GB total previewed size.
- Selected-manifest real-profile Undo Quarantine must be implemented and tested before forward real-profile Quarantine execution.
- The typed approval phrase remains `QUARANTINE`.
- First real-profile execution is limited to `Likely safe` + `Quarantine candidate` rows.
- Files and narrow folders are allowed; folders must pass strict descendant checks.
- After real-profile Quarantine, the app should ask the user to rescan manually rather than auto-rescanning.
- Restore Manifest remains the only durable cleanup record for now.

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
- The real-profile confirmation phrase stays aligned with fixture confirmation, so the extra safety must come from readiness blockers and clear scope wording rather than a different typed phrase.

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
- [x] Open questions are answered or explicitly deferred before implementation.

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
- User decisions above are accepted input for the first non-moving readiness model packet.

ADR-worthy decisions:

- [ ] None
- [x] ADR needed: ADR 0018 accepts a composite real-profile Quarantine execution readiness model.

## Implementation plan

1. Add `QuarantineExecutionReadiness` core model and tests that report fixture-executable, real-profile-candidate, and custom-preview-only states without changing WPF execution availability. Completed in the first code packet.
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

- Review the answered first-phase decisions before any implementation packet.
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
- Allowing non-`D:` roots with acknowledgement adds a branch that must be tested carefully so unsafe roots still have no override.

Assumptions:

- The first real-profile target remains `C:\Users\moxhe`.
- Permanent deletion remains out of scope.
- The user prefers slower safety proof over fast execution enablement.

## Completion notes

Completed on: 2026-05-31

What changed:

- Added ADR 0018 for the composite Real-Profile Quarantine Execution Readiness model; it was later accepted after user decisions.
- Added this feature brief with readiness dimensions, implementation staging, open questions, and non-goals.
- Added draft domain/glossary terms for execution readiness, root execution safety, pre-execution revalidation, and restore readiness.
- Updated handoff/progress docs so the next packet can start from the design contract.
- Later recorded user decisions for exact scope, root policy, batch cap, row/folder eligibility, Undo prerequisite, `QUARANTINE` phrase, manual rescan, and Restore Manifest-only durable record.

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

- Added ADR 0018; it was later accepted after user decisions.

Follow-up work:

- Implement Quarantine Root Execution Safety while keeping real-profile movement blocked.
- Implement Pre-Execution Revalidation while keeping real-profile movement blocked.

Open questions:

- Deferred questions remain: whether custom non-fixture scopes ever become executable, whether all-manifest real-profile restore should exist, and whether permanent deletion should ever exist.

Risky assumptions:

- This design assumes no real-profile movement happens until the user explicitly approves a later implementation packet after reviewing these open questions.
