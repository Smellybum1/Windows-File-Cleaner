# Feature: Selection Browse Tooltip Clarity

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make Cleanup Scope and Quarantine Root browse actions explain that choosing a folder is only path selection, not scanning, folder creation, cleanup approval, or execution.

## Non-goals

- Do not change Cleanup Scope Selection behavior.
- Do not change Quarantine Root Selection behavior.
- Do not run a Storage Scan from browse actions.
- Do not create fixture or quarantine folders.
- Do not move, restore, delete, create, or modify scanned files.
- Do not enable real-profile Quarantine execution or real-profile restore.

## User story / job story

As the project owner, I want browse controls to explain what they do before I use them, so that choosing a folder is not mistaken for scan approval, Quarantine approval, or filesystem modification.

## Current behavior

The app already keeps Browse separate from `Scan`, and Quarantine Root browsing only changes preview path input. The buttons were visible but did not have their own boundary tooltips.

## Desired behavior

- Cleanup Scope `Browse...` says it chooses only a Cleanup Scope path.
- Cleanup Scope `Browse...` says browsing does not start a scan, bypass the real-profile gate, or approve cleanup.
- Quarantine Root `Browse...` says it chooses only a Quarantine Root for preview paths.
- Quarantine Root `Browse...` says browsing does not create folders, move files, or approve cleanup.
- Disabled-state tooltip behavior keeps those boundaries inspectable while scanning.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope Selection | Clarify that browse tooltips should keep path-only selection and scan-gate boundaries visible. | yes |
| Quarantine Root Selection | Clarify that browse tooltips should keep preview-only and no-folder-creation boundaries visible. | yes |

## Evidence and validation gate

Evidence gathered:

- The current handoff recommends scan-gate discoverability, Quarantine Preview/readiness clarity, and manual fixture review polish before real cleanup execution.
- Cleanup Scope Selection docs already state browsing updates the path and safety note but does not start scanning.
- Quarantine Root Selection docs already state browsing is preview-root selection and does not create folders.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: UI tooltip wording and tests only, no filesystem mutation.
- [x] The narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.

Rejected ideas buffer:

- Do not add modal warnings to browse buttons in this packet.
- Do not add folder creation or probing for Quarantine Root browsing.
- Do not weaken the real-profile scan gate.

## Decisions made

Small feature-level decisions:

- Use disabled-state tooltips so browse boundaries are still available while scanning.
- Keep the tooltips concise and tied to existing domain terms.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF disabled-state tooltips to Cleanup Scope and Quarantine Root `Browse...` buttons.
2. Add test-facing tooltip accessors and WPF smoke assertions.
3. Update README, domain docs, fixture checklist, progress, and handoff.
4. Run WPF smoke tests, build, checklist-only output, and diff check.

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During fixture review, confirm the Cleanup Scope and Quarantine Root browse tooltips are readable and do not imply scan, folder creation, or cleanup approval.

## Risks and assumptions

Risks:

- More tooltip guidance may be less discoverable than inline text for keyboard-only review.

Assumptions:

- Browse actions are simple enough that tooltip clarity is the right small safety packet.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added disabled-state WPF tooltips to Cleanup Scope and Quarantine Root `Browse...` buttons.
- Added WPF smoke assertions for path-only selection, real-profile scan-gate, preview-only, no-folder-creation, and no-approval wording.
- Updated README, domain docs, fixture checklist, progress, and handoff.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible UI tooltip clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Check during visible fixture review whether browse tooltip wording is discoverable and fits with the existing safety notes.

Open questions:

- Should a later accessibility pass expose path-selection boundaries through focus text instead of hover-only tooltips?

Risky assumptions:

- Tooltip clarity is enough for browse controls because nearby safety-note and scan-gate text remain the primary visible guidance.
