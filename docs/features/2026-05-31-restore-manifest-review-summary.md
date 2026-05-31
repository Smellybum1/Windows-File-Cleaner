# Feature: Restore Manifest Review Summary

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Add a compact read-only WPF summary for Restore Manifest review state so manual fixture review can see discovery, selected-manifest readiness, all-manifest readiness, and selected restore gate/result state without parsing every detailed pane.

## Non-goals

- Do not enable real-profile selected restore.
- Do not enable real-profile Quarantine execution.
- Do not add all-manifest restore execution.
- Do not create folders, write manifests, move files, restore files, delete files, clean up folders, or add cleanup history.

## Implementation

- Added `RestoreManifestReviewSummaryText` under the manifest review controls.
- The summary starts in a waiting state, then updates after manifest discovery, selected manifest readiness, selected restore gate preview, exact `RESTORE`, all-manifest readiness preview, and fixture selected restore result.
- The summary uses neutral/information/success/warning styling and mirrors `Summary state:` plus read-only/no-restore/no-write/no-approval boundaries into tooltip and automation help text.
- Added WPF smoke coverage for startup, discovery, selected readiness, closed/open selected restore gate, all-manifest readiness, and selected fixture restore result summary states.

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Added visible compact Restore Manifest review state without changing execution availability.
- Updated README, domain glossary/context, fixture checklist, progress, and handoff docs.

ADRs:

- No ADR added. This is reversible WPF readability polish under ADR 0013, ADR 0015, ADR 0016, and ADR 0019.

Follow-up work:

- Manual fixture review should confirm whether the compact summary makes manifest discovery/readiness state easier to scan in the details pane.

Open questions:

- None for this packet.

Risky assumptions:

- A compact summary line is enough for readiness orientation without needing a new manifest-review panel.
