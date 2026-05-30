# Feature: Fixture Review Checklist Output

Date started: 2026-05-29  
Status: completed  
Owner: project-owner

## Goal

Make the manual fixture review pass easier to run consistently by printing the highest-value review checklist directly from `Start-MvpFixtureReview.ps1`.

## Non-goals

- Do not launch the app in automated checks.
- Do not auto-scan when WPF opens.
- Do not scan or modify `C:\Users\moxhe`.
- Do not add cleanup execution or restore behavior.
- Do not replace the full README manual checklist.

## Desired behavior

When the fixture launcher runs, it should print a compact checklist before WPF launch that reminds the user to check fixture scope wording, manual Scan, read-only status, review summaries, search/focus actions, Review Shortlist Safety Mix, Quarantine Preview, approval-boundary wording, Quarantine Execution Scope Status, fixture-only execute/undo, manifest discovery, and real/custom execution blockers.

The launcher should support `-SkipChecklist` for focused loops.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| None | Existing fixture review and Storage Scan terms used. | n/a |

## Evidence and validation gate

Evidence gathered:

- The next recommended work remains a visible fixture review pass.
- The launcher already prints basic no-auto-scan wording, but the detailed manual checklist lived in README.
- Recent packets added scan-gate, shortlist, and execution-scope wording that should be checked together.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: script output only in automated verification.
- [x] Narrowest relevant check is launcher `-WhatIf -SkipPreflight -SkipLaunch`.

## Decisions made

Small feature-level decisions:

- Add a compact checklist to the existing launcher instead of creating a separate script.
- Print checklist before launching WPF so it remains visible in the terminal.
- Add `-SkipChecklist` for focused loops.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add a checklist writer to `tools/Start-MvpFixtureReview.ps1`.
2. Print the checklist before launch unless `-SkipChecklist` is passed.
3. Update README, fixture launcher feature brief, progress, and handoff.
4. Verify with `-WhatIf -SkipPreflight -SkipLaunch`.

## Completion notes

Completed on: 2026-05-29

What changed:

- `Start-MvpFixtureReview.ps1` now prints a compact manual fixture review checklist.
- Added `-SkipChecklist` for focused loops.
- Kept automated verification read-only/no-launch through `-WhatIf -SkipPreflight -SkipLaunch`.
- Later packet `2026-05-29-fixture-review-checklist-only-mode.md` added `-ChecklistOnly` so the same checklist can be printed without preflight, fixture creation, or WPF launch.
- Later packet `2026-05-29-fixture-checklist-approval-boundary.md` updated the checklist to include Quarantine approval-boundary wording.
- Later packet `2026-05-29-fixture-checklist-selected-restore-scope.md` updated the checklist to include Selected Restore gate scope-status and approval-boundary wording.
- Later packet `2026-05-29-fixture-checklist-broad-restore-wording.md` updated the checklist to include broad-restore boundary wording for discovery/readiness panes.
- Later packet `2026-05-29-all-manifest-restore-wording.md` refined that checklist wording to `all-manifest restore action`.
- Later packet `2026-05-29-review-grid-mode-status.md` updated the checklist to include collapsible Safety Summary and Quarantine shortlist review, inline Quarantine Preview readiness, `Quarantined` / `Back to scan rows`, and Review Grid Mode Status. Later current-label polish renamed the visible grid switch to `Current quarantined`.
- Later packet `2026-05-29-quarantine-preview-status-styling.md` updated the checklist to call out styled inline preview readiness states: neutral, success, warning, and error.
- Later packet `2026-05-29-review-grid-mode-status-styling.md` updated the checklist to call out styled Review Grid Mode Status states: neutral, informational, and warning.
- Later packet `2026-05-30-review-grid-mode-status-help-text.md` updated the checklist to call out Review Grid Mode Status tooltip/help text.
- Later packet `2026-05-30-review-grid-mode-status-help-cue.md` updated the checklist to call out the Review Grid Mode Status `?` help cue.
- Later packet `2026-05-30-quarantine-preview-status-help-text.md` updated the checklist to call out inline Quarantine Preview status tooltip/help text.
- Later packet `2026-05-30-quarantine-shortlist-header-styling.md` updated the checklist to call out Quarantine shortlist header state styling.
- Later packet `2026-05-30-safety-summary-header-styling.md` updated the checklist to call out Safety Summary header state styling.
- Later packet `2026-05-30-collapsed-header-state-help-text.md` updated the checklist to call out state-naming header tooltip/help text.
- Later packet `2026-05-30-status-state-help-text.md` updated the checklist to call out state-naming Review Grid Mode Status and inline Quarantine Preview tooltip/help text.
- Later packet `2026-05-30-collapsed-header-summary-labels.md` updated the checklist to call out panel-name prefixes on the collapsed Safety Summary and Quarantine Shortlist headers.
- Later packet `2026-05-30-review-shortlist-safety-mix-help-text.md` updated the checklist to call out Review Shortlist Safety Mix tooltip/help text.
- Later packet `2026-05-30-review-mix-help-text.md` updated the checklist to call out Review Mix and Matched Review Mix tooltip/help text.
- Later packet `2026-05-30-review-mix-help-cues.md` updated the checklist to call out Review Mix and Matched Review Mix `?` help cues.
- Later packet `2026-05-30-review-shortlist-safety-mix-help-cue.md` updated the checklist to call out the Review Shortlist Safety Mix `?` help cue.
- Later packet `2026-05-30-execution-policy-friendly-fixture-launcher.md` added `Start-MvpFixtureReview.cmd`, so the same checklist can be launched when direct `.ps1` execution is blocked by local policy.
- Later packet `2026-05-30-fixture-checklist-hoverable-help-cues.md` aligned the checklist wording with the hoverable `?` cue affordance and prompt tooltip behavior.
- Later packet `2026-05-31-fixture-checklist-selected-restore-boundary.md` aligned the final real-profile/custom blocker prompt with both ADR 0017 Quarantine blockers and ADR 0019 selected-restore blockers.

Tests run:

- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -WhatIf -SkipPreflight -SkipLaunch -SkipChecklist`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-28-mvp-fixture-review-launcher.md`
- `docs/features/2026-05-29-fixture-review-checklist-output.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. This is a local workflow-output improvement with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- After the next visible fixture review, should the checklist be shortened or split into grouped prompts?

Risky assumptions:

- A compact terminal checklist is enough to make the manual pass easier without adding GUI automation.
