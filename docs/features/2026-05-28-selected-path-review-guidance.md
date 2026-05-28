# Feature: Selected Path Review Guidance

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make the selected-row detail pane more useful after a real Storage Scan by showing the safest next review step for the selected path.

## Non-goals

- Do not add cleanup execution.
- Do not add Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not change scanner traversal behavior.
- Do not change ratings just to make rows look safer.

## User story / job story

As the project owner, I want selected scan rows to explain what I should do next, so that large rows such as profile containers, AppData folders, browser data, and package caches are easier to triage without risking current apps.

## Current behavior

The selected-row pane shows path, metadata, evidence, Largest immediate children, inspection actions, and Quarantine Preview readiness text. For large rows, evidence can still be too passive, especially when the row is a container or has no category.

## Desired behavior

The selected-row pane should include read-only guidance such as:

- Inspect children, not the container.
- Keep by default.
- Shortlist after review.
- Inspect before shortlisting.
- Classify before cleanup.
- Investigate access issue.
- Inspect link target.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Path Review Guidance | Added as read-only selected-row next-step wording. | yes |

## Open questions

Questions that must be answered before implementation:

- None.

Questions that can be deferred:

- Should future guidance become user-customizable after real cleanup execution exists?

## Grill notes

### Scenarios discussed

- The user tested the app against `C:\Users\moxhe` and showed a real scan where giant rows need clearer review direction.
- The app must continue protecting current apps, including Codex, and remain read-only.

### Edge cases

- Access issue rows should not imply cleanup readiness.
- Reparse points should not be followed or cleaned broadly.
- Whole profile containers should direct the user to child rows instead of shortlisting the container.
- Quarantine candidates still need recognition and preview before any future cleanup execution.

### Dependencies between decisions

- This depends on existing Bloat Categories, Importance Ratings, Deletion Recommendations, Child Breakdown, Review Shortlist, and Quarantine Preview.
- This does not change Quarantine execution readiness.

## Evidence and validation gate

Evidence gathered:

- User screenshot of a real scan against `C:\Users\moxhe`.
- Existing code/docs inspected: WPF detail pane, classifier, fixture tests, domain context, glossary, README, and progress log.

Tests/checks planned:

- Core guidance builder coverage.
- WPF smoke assertion that selected quarantine candidates show guidance before shortlisting.
- Build and both test harnesses.
- `git diff --check`.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not turn guidance into approval language.
- Do not hide High risk rows; tell the user why they are keep-by-default.
- Do not use Selected Path Review Guidance as a cleanup executor.

## Decisions made

Small feature-level decisions:

- Implement guidance as a core builder so it can be tested outside WPF.
- Keep the UI wording short enough for the existing detail pane.
- Keep Quarantine candidate wording tied to Review Shortlist and Quarantine Preview, not deletion.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add a core `SelectedPathReviewGuidance` model and builder.
2. Add a Review guidance section to the selected-row detail pane.
3. Add core and WPF smoke coverage.
4. Update README, domain docs, and progress log.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.Core/SelectedPathReviewGuidance.cs`
- `src/WindowsFileCleaner.Core/SelectedPathReviewGuidanceBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- Run the app, select large rows, and verify the detail pane shows Review guidance before Child Breakdown.

Automated tests:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- Guidance wording may still need tuning after another real-profile scan.
- Conservative wording can feel repetitive, but that is preferable to overstating cleanup safety.

Assumptions:

- Selected-row guidance can be derived from existing scan metadata without adding persistence or new user decisions.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added Selected Path Review Guidance for the WPF selected-row detail pane.
- Added core guidance rules for access issues, reparse points, profile containers, protected/high-risk rows, quarantine candidates, cache/package rows, uncategorized rows, and generic evidence review.
- Later tuned cache/package guidance so GPU shader caches, Python package caches, Node package caches, app caches, and generic AppData rows get more specific read-only review wording without changing ratings or recommendations.
- Added automated coverage for core guidance and WPF selected-row guidance display.

Files changed:

- `src/WindowsFileCleaner.Core/SelectedPathReviewGuidance.cs`
- `src/WindowsFileCleaner.Core/SelectedPathReviewGuidanceBuilder.cs`
- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.Tests/Program.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-review-guidance.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-selected-path-review-guidance.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This is a reversible detail-pane review improvement and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Retest visible WPF layout after a real-profile scan and tune wording if the guidance is too noisy or not specific enough.

Open questions:

- Should guidance become user-customizable after actual cleanup execution exists?

Risky assumptions:

- Existing metadata is enough to provide useful next-step guidance without adding a new confidence model.
