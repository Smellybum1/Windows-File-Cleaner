# Feature: Review Navigation and Export Tooltip Clarity

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make review navigation, search reset, view reset, and scan report export controls explain their read-only/report-only boundaries before use.

## Non-goals

- Do not change Storage Review Search behavior.
- Do not change Review View Reset behavior.
- Do not change Storage Review Display Window behavior.
- Do not change Scan Report Export behavior.
- Do not move, restore, delete, create, or modify scanned files.
- Do not enable real-profile Quarantine execution or real-profile restore.

## User story / job story

As the project owner, I want the main review navigation and export controls to explain that they only change the review lens, move through in-memory rows, or write a report, so that I do not mistake them for rescanning, cleanup approval, or file modification.

## Current behavior

`Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows` are read-only or report-only actions. Status text and docs describe their behavior after use, but the controls did not have dedicated pre-use boundary tooltips.

## Desired behavior

- `Export CSV` says it exports the active review lens as a CSV report, is not cleanup approval, and does not modify scanned files.
- `Clear search` says it clears only Storage Review Search, does not rescan or modify files, and keeps Review Shortlist.
- `Reset view` says it clears active filters/search, keeps Review Shortlist, and does not rescan or modify files.
- `Previous rows` and `Next rows` say they move through the in-memory Storage Review Display Window and do not rescan or modify files.
- Disabled-state tooltip behavior keeps those boundaries inspectable before the controls are enabled.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Scan Report Export | Clarify report-only tooltip expectations. | yes |
| Storage Review Search | Clarify clear-search tooltip expectations. | yes |
| Review View Reset | Clarify reset tooltip expectations. | yes |
| Storage Review Display Window | Clarify Previous/Next rows tooltip expectations. | yes |

## Evidence and validation gate

Evidence gathered:

- Recent packets clarified selected-row actions, browse controls, and Review Shortlist/report/preview toolbar controls.
- The main review toolbar still had read-only navigation/export controls without pre-use tooltip boundaries.
- Domain docs already define these controls as read-only, in-memory, or report-only.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: UI tooltip wording and tests only, no filesystem mutation except user-initiated report export behavior that already existed and is unchanged.
- [x] The narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.

Rejected ideas buffer:

- Do not change export row selection or suggested filenames in this packet.
- Do not add a new manual visual indicator for debounced search.
- Do not change display-window size or add virtualization/tree view behavior.

## Decisions made

Small feature-level decisions:

- Use disabled-state tooltips for review navigation and export controls so boundaries are visible before scan state enables them.
- Keep `Export CSV` wording tied to Scan Report Export, not Quarantine manifests or cleanup history.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF disabled-state tooltips to `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows`.
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

- During fixture review, confirm review navigation/export tooltips are readable and do not imply rescanning, cleanup approval, or file modification.

## Risks and assumptions

Risks:

- Tooltip guidance may become repetitive across the app until a future accessibility pass consolidates focus help.

Assumptions:

- This is still useful because these controls sit in the primary real-profile review workflow.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added disabled-state WPF tooltips to `Export CSV`, `Clear search`, `Reset view`, `Previous rows`, and `Next rows`.
- Added WPF smoke assertions for report-only, no-rescan, in-memory display-window, Review Shortlist-preserving, no-file-modified, and not-cleanup-approval wording.
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

- Check during visible fixture review whether the main review toolbar still wraps comfortably with the tooltip-heavy controls.

Open questions:

- Should a later accessibility pass expose review toolbar boundaries through focus text instead of hover-only tooltips?

Risky assumptions:

- Tooltip clarity is enough for review navigation/export controls because status text and review summaries remain the visible guidance.
