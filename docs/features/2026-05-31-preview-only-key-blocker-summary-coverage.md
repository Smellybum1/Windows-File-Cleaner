# Feature: Preview-Only Key-Blocker Summary Coverage

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Keep compact Quarantine Readiness Summary key-blocker wording covered for preview-only scopes beyond the exact real-profile first-phase batch-limit case.

## Non-goals

- Do not change visible WPF wording.
- Do not enable real-profile Quarantine execution.
- Do not enable real-profile selected restore, real-profile Undo Quarantine, permanent deletion, or cleanup history.
- Do not create folders, write Restore Manifests, move files, restore files, delete files, or scan `C:\Users\moxhe`.

## Implementation

- Extended WPF smoke assertions for custom non-fixture preview-only summaries to require `Key blockers:` with `custom scope preview-only`.
- Extended exact real-profile synthetic preview assertions to require `pre-execution revalidation`, `restore readiness`, and `current build unavailable` key labels.
- Extended real-profile-child synthetic preview assertions to require `exact profile scope`.
- Kept the visible app behavior unchanged.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

## ADRs

No ADR added. This is regression coverage for accepted ADR 0018 preview-only readiness wording.

## Open questions

- None.

## Follow-up work

- Continue manual fixture visual review of compact readiness wording and fixture-only boundaries.

## Risky assumptions

- The current labels are the intended concise vocabulary for these preview-only scope blockers.
