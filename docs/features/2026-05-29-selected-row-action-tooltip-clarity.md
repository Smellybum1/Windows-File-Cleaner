# Feature: Selected Row Action Tooltip Clarity

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make selected-row review actions explain their read-only, selected-only, or inspection-only boundaries before use.

## Non-goals

- Do not change selected-row action behavior.
- Do not change Review Shortlist behavior.
- Do not change Selected Folder Child Focus or Selected Folder Descendant Focus behavior.
- Do not change Selected File Content Preview behavior.
- Do not move, restore, delete, create, or modify scanned files.
- Do not enable real-profile Quarantine execution or real-profile restore.

## User story / job story

As the project owner, I want selected-row action buttons to explain what they do before I click them, so that shortlisting, focus, preview, copy, and Explorer inspection do not blur into cleanup approval.

## Current behavior

The selected-row detail pane has action buttons for adding/removing the selected row from Review Shortlist, copying the path, showing children, showing descendants, previewing a file, and opening File Explorer. Status text and docs describe their read-only boundaries after use, but the controls did not have dedicated pre-use tooltips.

## Desired behavior

- `Add to shortlist` and `Remove` explain selected-row scope, review context, not-cleanup-approval wording, and no-file-modified behavior.
- `Copy path` explains manual inspection only and no-file-modified/no-approval behavior.
- `Show children` explains read-only `parent:` focus, no rescan, no file modification, and no cleanup approval.
- `Show descendants` explains read-only `under:` focus, no rescan, no file modification, and no cleanup approval.
- `Preview file` explains bounded text preview, Credential Data guardrails, and no file modification.
- `Open in Explorer` explains manual inspection only, not cleanup approval, and no app-side file modification.
- Disabled-state tooltip behavior keeps those boundaries inspectable before a row is selected.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Review Shortlist | Clarify selected-row add/remove tooltips. | yes |
| Selected Folder Child Focus | Clarify read-only `parent:` focus tooltip expectations. | yes |
| Selected Folder Descendant Focus | Clarify read-only `under:` focus tooltip expectations. | yes |
| Selected Path Inspection | Clarify copy/Open in Explorer tooltip expectations. | yes |
| Selected File Content Preview | Clarify bounded read-only preview tooltip expectations. | yes |

## Evidence and validation gate

Evidence gathered:

- Recent packets clarified the main Review Shortlist toolbar, report/preview controls, and browse controls.
- The selected-row action strip is the remaining pre-use review surface users touch while triaging unfamiliar rows.
- Domain docs already define these actions as read-only or inspection-only.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: UI tooltip wording and tests only, no filesystem mutation.
- [x] The narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.

Rejected ideas buffer:

- Do not add more always-visible helper text to the selected-row pane in this packet.
- Do not change focus/search behavior.
- Do not turn Open in Explorer into cleanup approval.

## Decisions made

Small feature-level decisions:

- Use disabled-state tooltips for every selected-row action so boundaries are visible before row selection.
- Keep tooltip wording tied to existing project terms: Review Shortlist, `parent:`, `under:`, Credential Data, cleanup approval.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF disabled-state tooltips to selected-row action buttons.
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

- During fixture review, confirm selected-row action tooltips are readable and do not imply cleanup approval or file modification.

## Risks and assumptions

Risks:

- Tooltip-only guidance may still be missed by keyboard-only review.

Assumptions:

- The existing selected-row guidance text plus status messages remain the primary visible guidance; tooltips add pre-use clarity without crowding the pane.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added disabled-state WPF tooltips to selected-row shortlist, copy, focus, preview, and Explorer actions.
- Added WPF smoke assertions for selected-only, read-only focus, bounded preview, manual inspection, no-file-modified, and not-cleanup-approval wording.
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

- Check during visible fixture review whether selected-row action tooltips remain readable and the detail pane action row still wraps comfortably.

Open questions:

- Should a later accessibility pass expose selected-row action boundaries through focus text instead of hover-only tooltips?

Risky assumptions:

- Tooltip clarity is enough for this selected-row review-polish packet.
