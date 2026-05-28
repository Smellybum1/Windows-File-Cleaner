# Feature: Storage Scan review filters

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Improve the first real Storage Scan review experience after the user confirmed the app scanned `C:\Users\moxhe`.

The scan produced believable totals, but the initial table was too flat and large container folders showed `None` for categories. The goal is to make the scan easier to triage without adding any cleanup action.

## Non-goals

- Do not add deletion.
- Do not add Quarantine.
- Do not change the Cleanup Scope.
- Do not scan automatically.

## User story / job story

As the project owner, I want to filter scan results by importance and cleanup suitability, so that I can quickly inspect likely opportunities and high-risk locations separately.

## Current behavior

The app scans recursively and shows the top 2,000 largest paths, but the initial table has limited triage controls.

The first user scan showed:

- `C:\Users\moxhe` scanned successfully.
- Total size: 58.02 GB.
- Folders: 37,740.
- Files: 188,580.
- Access issues: 3.
- Large container paths such as `AppData`, `Local`, `Roaming`, `Google`, `Chrome`, and `pip` needed clearer category/evidence language.

## Desired behavior

- Add filters for All, Likely safe, Caution, High risk, and Quarantine candidates.
- Show counts on filter buttons after a scan.
- Show a compact filter summary with number of displayed rows and the largest displayed row.
- Keep scan review read-only.
- Label big container/cache patterns more clearly while keeping recommendations conservative.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Review Filter | Added for read-only result filters. | yes |
| Bloat Category | Expanded to include conservative container/cache categories from the real scan. | yes |

## Decisions made

- Add filtering in the review layer rather than changing scan results.
- Treat Profile container and Browser data as High risk / Keep.
- Treat AppData area, Python package cache, GPU shader cache, and similar cache areas as Caution / Inspect by default.
- Do not mark container folders as automatically cleanup-safe.

## Implementation plan

1. Add core review entries, summaries, and filters.
2. Wire filter buttons into the WPF screen.
3. Add count and largest-row summaries for filtered results.
4. Add classifier labels for real-scan patterns.
5. Add fixture tests for review filters and classifier behavior.

## Test plan

Automated checks:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Manual checks:

- User should rerun the desktop app and confirm filters make the real scan easier to inspect.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added core review/filter types.
- Added Storage Scan filter buttons and filter summary in WPF.
- Later packet updated the summary wording from summed displayed bytes to largest displayed row because flattened recursive rows overlap.
- Added conservative categories for profile containers, AppData areas, browser data, and GPU shader caches.
- Added fixture tests for filter behavior and real-scan-inspired classification.

Files changed:

- `src/WindowsFileCleaner.Core/`
- `src/WindowsFileCleaner.App/`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- This feature brief

ADRs added or skipped:

- No ADR added. This is an incremental review/UI improvement, not a durable architectural decision.

Follow-up work:

- Let the user test the filter UI against the real scan.
- Consider showing immediate child breakdowns for selected folders.
- Consider a grouped category view after filter feedback.

Open questions:

- Which real scan rows should become explicit Protected Locations?
- Should Quarantine candidates exclude all AppData-derived rows until Quarantine exists?

Risky assumptions:

- Filter counts are enough for the next review step before adding charts or a tree view.
