# Feature Brief Index

Last updated: 2026-06-04

Use this index before opening individual feature briefs. The folder contains many historical packet notes; do not bulk-read all feature briefs in a fresh thread.

## Active Or Current

- `2026-06-04-real-profile-selected-restore-trust-helper-preflight-regression.md`: MVP preflight now runs the sacrificial real-profile selected restore trust helper path guard regression by default, with an escaped-path case derived from the helper's safe preview for clean-runner portability.
- `2026-06-04-real-profile-selected-restore-trust-helper-path-guard.md`: the sacrificial real-profile selected restore trust helper now rejects explicit roots outside the default `D:\WindowsFileCleanerQuarantine` root or ignored `.local`, and generated quarantine source paths outside the action `items` root, with a local `-WhatIf` regression.
- `2026-06-04-fixture-root-path-guard-regression.md`: synthetic fixture creation and fixture review launch roots now have MVP preflight coverage that explicit roots outside ignored `.local` fail before fixture writes, checklist output, or WPF launch.
- `2026-06-04-local-release-path-guard-regression.md`: portable release publisher, verifier, and launcher now have MVP preflight coverage that explicit release roots and release paths outside ignored `.local` fail before publisher, verifier, or launch-command output.
- `2026-06-04-accepted-launcher-notes-path-guard.md`: accepted-package launcher regression now covers explicit acceptance notes paths outside ignored `.local` failing before launch-command output.
- `2026-06-04-pending-notes-head-wording-stabilization.md`: pending package acceptance notes docs now describe refresh commits as generation-time provenance and rely on summary status lines for live current-HEAD context.
- `2026-06-04-daily-readiness-latest-notes-isolation.md`: daily readiness latest package notes regression now uses explicit synthetic latest-notes paths under its private ignored test folder.
- `2026-06-04-daily-readiness-package-notes-path-guard.md`: daily readiness now has composed regression coverage that explicit package acceptance notes paths outside ignored `.local` fail before latest-notes or launch-command printing.
- `2026-06-04-daily-readiness-fixture-notes-path-guard.md`: daily readiness now has composed regression coverage that explicit fixture notes paths outside ignored `.local` fail before accepted launch-command printing.
- `2026-06-04-fixture-summary-ignored-path-guard.md`: fixture acceptance summaries now reject explicit notes paths outside ignored `.local`.
- `2026-06-04-fixture-completion-summary-wording.md`: completed fixture acceptance summaries now say evidence is complete and no recorder action is pending.
- `2026-06-04-package-completion-summary-wording.md`: completed package acceptance summaries now say evidence is complete and no recorder action is pending.
- `2026-06-04-ci-evidence-wording-stabilization.md`: CI evidence docs now use representative current-path wording instead of self-staling latest-run wording.
- `2026-06-04-ci-evidence-refresh.md`: CI runbook and compact handoff docs now record #365 proof for the current normal push preflight path.
- `2026-06-04-feature-index-entry-regression.md`: documentation consistency now verifies bare active feature-index entries resolve to existing feature briefs.
- `2026-06-04-documentation-consistency-regression.md`: MVP preflight now verifies active docs links and latest packet breadcrumb alignment.
- `2026-06-04-ci-windows-image-canary.md`: MVP Preflight now has a manual runner-image canary for intentionally testing `windows-2025-vs2026` while push/PR runs remain on `windows-2022`.
- `2026-06-04-ci-actions-runtime-maintenance.md`: GitHub Actions MVP Preflight now uses Node 24-capable official actions and pins CI to `windows-2022` before the `windows-latest` VS 2026 image migration.
- `2026-06-04-package-acceptance-current-head-summary.md`: package acceptance summaries now show current repository `HEAD`, notes/current-HEAD status, and package/current-HEAD status.
- `2026-06-04-startup-context-compaction.md`: current docs/context reduction packet for faster Codex threads.
- `2026-06-04-daily-readiness-latest-package-notes-regression.md`: daily readiness latest package notes informational behavior, now included in MVP preflight.
- `2026-06-04-daily-readiness-fixture-acceptance-regression.md`: daily readiness fixture acceptance notes forwarding, now included in MVP preflight.
- `2026-06-04-fixture-acceptance-notes-regression.md`: fixture acceptance notes summary/recorder guardrails, now included in MVP preflight.
- `2026-06-04-daily-readiness-exact-profile-undo-spotlight.md`: default daily readiness now spotlights exact-profile undo-work stop state, covered by guard-tested MVP preflight regression.
- `2026-06-04-real-profile-next-batch-early-undo-guard.md`: next-batch evidence preset stops before MVP preflight when displayed undo work exists, covered by guard-tested clean-runner MVP preflight regression.
- `2026-06-04-accepted-package-complete-notes-selection.md`: accepted-package helpers select latest complete notes by default, now covered by MVP preflight regression for incomplete and malformed-looking candidate notes.
- `2026-06-04-local-release-acceptance-summary-regression-check.md`: targeted regression command for package acceptance summary next-step output, malformed-looking notes, default completed-notes selection, and ignored-path guard coverage, now included in MVP preflight.
- `2026-06-04-local-release-recorder-commit-evidence-guard.md`: package acceptance recorder guardrails for verifier/commit evidence, now included in MVP preflight.
- `2026-06-04-package-acceptance-summary-next-steps.md`: incomplete package acceptance summaries print guarded recorder and recheck commands.
- `2026-06-04-daily-readiness-latest-package-notes.md`: daily readiness now surfaces latest package acceptance notes without promoting incomplete candidates.
- `2026-06-04-local-release-acceptance-command-stamping.md`: generated package acceptance notes stamp actual release-path/current-commit commands, now covered by MVP preflight regression.
- `2026-06-04-pending-package-acceptance-notes-refresh.md`: refreshed pending candidate acceptance notes after docs/tooling `HEAD` advanced beyond the app package commit.
- `2026-06-04-verified-portable-package-candidate.md`: verified current app-code portable package candidate, pending human package acceptance.
- `2026-06-04-second-real-profile-quarantine-batch.md`: latest user-clicked exact-profile Quarantine evidence and current stop state.
- `2026-06-01-live-product-readiness-roadmap.md`: current readiness tracks and next live-product gates.

## Operational References

Command detail now belongs in:

- `docs/operations/ci.md`
- `docs/operations/daily-use.md`
- `docs/operations/portable-release.md`
- `docs/operations/manual-fixture-review.md`
- `docs/operations/restore-manifest-review.md`

## Historical Briefs

Historical briefs from 2026-05-28 through 2026-06-02 remain in `docs/features/` for link stability. Treat them as an archive unless a task specifically touches that feature.

Completed 2026-06-04 packet briefs that are not listed above are also historical unless the task specifically touches their surface. Search by filename or term instead of opening every brief.

Use filename/date searches instead of broad reads, for example:

```powershell
rg -n "Real-Profile Quarantine|Restore Manifest|Portable" docs\features
```
