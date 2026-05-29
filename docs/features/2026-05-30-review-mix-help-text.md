# Feature: Review Mix Help Text

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Mirror Review Mix and Matched Review Mix wording into tooltip and WPF automation help text so their safety boundary is available outside the visible summary lines.

## Non-goals

- Do not change Storage Scan results, filters, search, focus actions, Review Shortlist membership, Quarantine Preview, fixture execution, undo, restore, or manifest behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.
- Do not add a new row, badge, popup, modal, or help icon.

## Desired behavior

- Review Mix keeps summarizing the whole completed Storage Scan.
- Matched Review Mix keeps summarizing the current active review lens across the full matched set, not only the visible display window.
- Each readout mirrors its current visible text into tooltip and automation help text.
- Help text keeps clear that these readouts are read-only review context, not rescans, file modification, storage-savings proof, or cleanup approval.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Review Mix | Clarified tooltip/help-text boundary. | yes |
| Matched Review Mix | Clarified tooltip/help-text boundary. | yes |

## Evidence and validation gate

Evidence gathered:

- Review Shortlist Safety Mix already had mirrored tooltip/help text.
- Review Mix and Matched Review Mix are neighboring read-only context lines with similar non-approval risk.
- Existing WPF smoke tests already cover scan, descendant focus, prefixed search, and Review Shortlist Safety Mix help text.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added static startup tooltip and automation help text to Review Mix and Matched Review Mix.
- Added dynamic setters so post-scan, descendant-focus, and prefixed-search summary text is mirrored into tooltip/help text.
- Added WPF smoke assertions for startup, completed scan, descendant focus, and prefixed search states.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-28-review-mix-summary.md`
- `docs/features/2026-05-29-matched-review-mix.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This is reversible WPF help-text polish with no persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm whether Review Mix and Matched Review Mix tooltip/help text is enough or whether the dense summary lines need a visible help affordance.

Risky assumptions:

- Mirrored tooltip/help text is the right next safety-context polish before any broader layout change.
