# Feature: Review Toolbar Report and Preview Tooltip Clarity

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the Review Shortlist report, clear, Quarantine Preview, and preview export controls explain their read-only or in-memory safety boundaries before use.

## Non-goals

- Do not change Review Shortlist behavior.
- Do not change Quarantine Preview eligibility or preview output.
- Do not change CSV export behavior.
- Do not move, restore, delete, create, or modify scanned files.
- Do not enable real-profile Quarantine execution or real-profile restore.

## User story / job story

As the project owner, I want the adjacent review toolbar actions to explain whether they export a report, clear in-memory review state, or build a dry run, so that I do not confuse review tools with cleanup approval or execution.

## Current behavior

`Shortlist visible rows` and `Remove visible rows` have scope and safety tooltips. The neighboring `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview` controls are still safety-sensitive but did not expose comparable pre-use tooltip wording.

## Desired behavior

- `Export shortlist` says it writes a CSV report only, is not cleanup approval, and does not modify scanned files.
- `Clear shortlist` says it clears only the in-memory Review Shortlist and does not delete, move, quarantine, or restore files.
- `Preview quarantine` says it builds a read-only Quarantine Preview and does not create folders, move files, or approve cleanup.
- `Export preview` says it exports a CSV report only and does not execute Quarantine or modify scanned files.
- Disabled-state tooltip behavior keeps those boundaries inspectable before the controls are enabled.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Review Shortlist | Clarify that export and clear controls should keep report-only and in-memory boundaries visible. | yes |
| Quarantine Preview | Clarify that preview and preview export controls should keep dry-run and report-only boundaries visible. | yes |

## Evidence and validation gate

Evidence gathered:

- The Review Shortlist toolbar is the next manual-review surface after visible-row label/tooltip polish.
- Handoff guidance prefers manual review polish and Review Shortlist safety context before any real cleanup execution.
- Existing tests already construct the WPF shell and assert safety tooltips at startup.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: UI tooltip wording and tests only, no filesystem mutation.
- [x] The narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.

Rejected ideas buffer:

- Do not add new inline text in this packet; the toolbar is already dense.
- Do not change export behavior, preview eligibility, or cleanup gates.
- Do not introduce cleanup approval semantics.

## Decisions made

Small feature-level decisions:

- Use disabled-state tooltips for all four controls so disabled actions still explain why they are safe/review-only.
- Keep report wording tied to CSV exports and dry-run wording tied to Quarantine Preview.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF disabled-state tooltips to `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview`.
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

- During fixture review, confirm the report/clear/preview/export tooltips are readable and the toolbar still wraps comfortably.

## Risks and assumptions

Risks:

- More tooltip copy can become repetitive if the toolbar later gains dedicated inline help.

Assumptions:

- The current compact toolbar benefits from hover/focus safety text instead of more always-visible text.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added WPF disabled-state tooltips to `Export shortlist`, `Clear shortlist`, `Preview quarantine`, and `Export preview`.
- Added WPF smoke assertions for report-only, in-memory-only, dry-run, and no-file-modified wording.
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

- Check during visible fixture review whether the review toolbar tooltips are discoverable and the toolbar still fits comfortably.

Open questions:

- Should a later accessibility pass expose these tooltip boundaries through keyboard focus/help text?

Risky assumptions:

- Tooltip clarity is enough for this review-polish packet without adding more always-visible toolbar text.
