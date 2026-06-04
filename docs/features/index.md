# Feature Brief Index

Last updated: 2026-06-04

Use this index before opening individual feature briefs. The folder contains many historical packet notes; do not bulk-read all feature briefs in a fresh thread.

## Active Or Current

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
