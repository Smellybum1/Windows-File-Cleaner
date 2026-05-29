# Feature: Collapsed Panel Header Help Text

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the collapsed Safety Summary and Quarantine shortlist headers easier to inspect when their compact summaries are long.

## Non-goals

- Do not change Storage Scan results, Quarantine Preview, fixture execution, undo, restore, or manifest behavior.
- Do not enable real-profile Quarantine execution or real-profile Undo Quarantine.
- Do not add permanent deletion or persisted cleanup history.

## User story / job story

As the project owner, I want closed panel headers to keep useful summary context visible and inspectable, so I can collapse review panels without losing safety state.

## Desired behavior

- Safety Summary and Quarantine shortlist headers keep their compact dynamic summaries.
- Header text uses character trimming if space is tight.
- Header tooltips and automation help text mirror the current dynamic summary.
- Header help text says the summary is read-only review context, not cleanup approval.

## Domain language changes

No new terms. This extends existing Safety Summary and Quarantine shortlist header behavior.

## Decisions made

- Keep the visual header compact and use tooltip/help text for full summary inspection.
- Later packet `2026-05-30-collapsed-header-help-cues.md` superseded the earlier wait-and-see idea after the user preferred visible `?` cues for hidden safety tooltips.

## Test plan

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Completion notes

Completed on: 2026-05-30

What changed:

- Added tooltip and automation help text to the Safety Summary header.
- Added tooltip and automation help text to the Quarantine shortlist header.
- Mirrored each header's dynamic summary into its tooltip/help text whenever scan, preview, execution, or undo state changes.
- Added header text trimming so tight layouts keep the header compact instead of crowding the review grid.
- Later packet `2026-05-30-quarantine-shortlist-header-styling.md` added lightweight semantic styling to the Quarantine shortlist header while preserving the same compact summary and help text.
- Later packet `2026-05-30-safety-summary-header-styling.md` added lightweight neutral/warning styling to the Safety Summary header while preserving the same compact summary and help text.
- Later packet `2026-05-30-collapsed-header-state-help-text.md` added textual header-state wording to both collapsed header tooltips/help text so state is not color-only.
- Later packet `2026-05-30-collapsed-header-summary-labels.md` made the compact headers start with the visible panel names: `Safety Summary:` and `Quarantine Shortlist:`.
- Later packet `2026-05-30-collapsed-header-help-cues.md` added visible circular `?` help cues beside both collapsed headers that mirror the same dynamic tooltip and automation help text.

Docs updated:

- README, domain context, glossary, fixture checklist, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible WPF help text and layout polish with no cleanup execution, persistence, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the header `?` help cues are noticeable without crowding the Expander headers.

Risky assumptions:

- Mirrored tooltip/help text is enough to make long closed-header summaries inspectable without adding more visible chrome.
