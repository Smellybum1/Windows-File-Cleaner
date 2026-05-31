# Feature: Quarantine Readiness Summary Key Blockers

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Make the compact Quarantine Readiness Summary name the most useful blocker examples for preview-only scopes without requiring the user to read the full Quarantine Execution Gate text.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable real-profile Undo Quarantine, selected real-profile restore, permanent deletion, or cleanup history.
- Do not add a new WPF panel or help cue.
- Do not create folders, write Restore Manifests, move files, restore files, delete files, or scan `C:\Users\moxhe`.

## Desired behavior

- Preview-only summaries continue to show disposition, scope, blocker count, missing dimensions, and `movement unavailable`.
- The same summary also includes `Key blockers:` with concise labels for high-signal blockers.
- ADR 0018 first-phase labels such as `10-row cap`, `1 GB cap`, `no-category rows`, and `strict descendant checks` are prioritized over generic current-build-unavailable wording when summary space is limited.
- Tooltip and automation help text continue to mirror the summary and keep the read-only/no-cleanup-approval boundary.

## Implementation

- Added compact readiness key-blocker labels to `FormatQuarantineReadinessSummary`.
- Prioritized first-phase real-profile blockers before broader readiness blockers in the compact summary.
- Extended the synthetic exact real-profile first-phase WPF smoke test to assert the summary includes row-cap, byte-cap, no-category, and strict-descendant labels while execution remains unavailable.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

## ADRs

No ADR added. This is reversible WPF readability polish under accepted ADR 0018.

## Open questions

- Manual visual review can still decide whether the compact summary is sufficient or a dedicated readiness pane would be clearer later.

## Follow-up work

- Run the next visible fixture review with the updated summary and checklist wording.

## Risky assumptions

- A short key-blocker phrase improves scanability without making the summary too crowded.
