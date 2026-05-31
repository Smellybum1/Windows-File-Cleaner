# Feature: WPF Header Shortlist Metrics

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Keep Review Shortlist size and row-type totals visible in the top header so the user can see shortlist scope without switching tabs.

## Non-goals

- Do not change Storage Scan behavior.
- Do not change Review Shortlist membership behavior.
- Do not change Cleanup Scope Scan Gate behavior.
- Do not change Quarantine Preview, fixture execution, selected restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile selected restore, broad Undo Quarantine, permanent deletion, or cleanup history.
- Do not scan, create, move, restore, delete, write, or clean up real-profile files.

## Implementation

- Added a second compact wrapping metric strip in the WPF header for Review Shortlist size, shortlisted folder rows, and shortlisted file rows.
- Positioned the Review Shortlist strip before the scan-total strip by default so wide windows show shortlist totals immediately to the left of `Total size`.
- Populated the strip from the existing in-memory `StorageReviewShortlist` applied to the current `StorageScanReview`.
- Added tooltip and automation help text to keep clear that shortlist row size is read-only review context, not storage savings or cleanup approval.
- Preserved the existing Review Shortlist, Quarantine Preview, and Quarantine Shortlist header update paths.

## Test plan

Automated checks:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

Manual checks:

- Run `.\tools\Start-MvpFixtureReview.cmd` and confirm the header Review Shortlist totals stay readable at the normal window size and sit to the left of the scan totals when space allows.
- Add and remove fixture shortlist rows and confirm size, folders, and files update without implying approval or savings.

## Completion notes

Completed on: 2026-05-31

What changed:

- The visible app now shows Review Shortlist size, shortlisted folder rows, and shortlisted file rows in the global header, positioned before the scan-total boxes by default.

ADRs:

- No ADR added. This is reversible WPF layout/readout polish with no architecture, persistence, cleanup execution, restore behavior, data-model, or security change.

Follow-up work:

- User visual review on 2026-06-01 approved the Review Shortlist totals sitting before the scan totals at the normal wide fixture window size.
- Run a narrower-window fixture pass later if the header metric strips need tighter spacing under constrained width.

Open questions:

- Should the header metric strips need tighter spacing on narrower windows?

Risky assumptions:

- Showing shortlist row size globally helps orientation as long as the help text keeps clear that row sizes can overlap and are not storage savings.
