# Live Product Readiness Roadmap

Date started: 2026-06-01
Last compacted: 2026-06-04
Status: active summary

## Goal

Keep the path to a safe live product explicit without making every fresh thread read the full packet history.

Historical roadmap notes are preserved in `docs/features/archive/2026-06-01-live-product-readiness-roadmap-history.md`.

## Current Readiness Tracks

| Track | Current state | Next action |
|---|---|---|
| Manual fixture confidence | Fixture Quarantine, current-fixture undo, fixture selected restore, and fixture review surfaces have passed prior user-visible review. Formal fixture notes remain optional/incomplete. | Use `docs/operations/manual-fixture-review.md` when a fixture pass is needed. |
| Real-profile read-only scan | Real-profile Storage Scan is read-only and previously worked by user report. | Run MVP preflight before any real-profile scan after code/workflow changes. |
| Real-profile selected restore | Exact selected `C:\Users\moxhe` Restore Manifest restore is implemented under ADR 0019 and succeeded in the first live recovery loop. | Use selected-manifest restore only for a specific selected manifest after readiness, exact `RESTORE`, and immediate revalidation. |
| Real-profile Quarantine | First and second tiny exact-profile batches succeeded by user report. The second batch currently leaves exact-profile displayed undo work `1`. | Stop. Do not chain another real-profile batch unless a new Grill with Docs pass decides outstanding selected-manifest undo work is acceptable. |
| Restore Manifest evidence | Terminal summary and WPF manifest panes can review manifests read-only. The latest selected manifest readiness screenshot showed the second batch manifest as restorable with `0` blocked selected entries. | Use terminal summaries or WPF selected readiness before any recovery action. |
| Portable package | Accepted baseline remains `.local\releases\windows-file-cleaner-v20260602-011556` at `bc9b869`. Candidate `.local\releases\windows-file-cleaner-v20260604-121922` at `e6ac3eb` is verified but not accepted. | Complete human package acceptance before promoting the candidate. |
| Installer/shortcut | ADR 0020 defers installed shortcuts and installer behavior. | Start a separate ADR 0020 follow-up only if the user explicitly asks. |
| Later cleanup expansion | Permanent deletion, broad restore, custom real-profile movement, and persisted cleanup history are unavailable. | Separate Grill with Docs and ADR packets only if chosen later. |

## Current Stop Boundaries

- Codex must not click real-profile movement.
- Do not run or treat another next-batch review as movement evidence while exact-profile displayed undo work is present.
- Do not use broad/all-manifest real-profile Undo Quarantine.
- Do not add permanent deletion or cleanup history as a shortcut to progress.
- Keep accepted-package daily use on the completed `bc9b869` baseline until the newer candidate is human-accepted.

## Relevant Current Briefs

- `2026-06-04-second-real-profile-quarantine-batch.md`
- `2026-06-04-real-profile-quarantine-inline-status-wording.md`
- `2026-06-04-verified-portable-package-candidate.md`
- `2026-06-04-package-acceptance-summary-next-steps.md`
- `2026-06-04-daily-readiness-latest-package-notes.md`
- `2026-06-04-local-release-acceptance-command-stamping.md`
- `2026-06-04-startup-context-compaction.md`

## ADRs

- ADR 0017: Real-Profile Quarantine Readiness Contract.
- ADR 0018: Real-Profile Quarantine Execution Readiness Model.
- ADR 0019: Real-Profile Selected Restore Execution Contract.
- ADR 0020: Defer Installed Shortcut and Installer.

No new ADR is required for this compact roadmap. It summarizes existing decisions.
