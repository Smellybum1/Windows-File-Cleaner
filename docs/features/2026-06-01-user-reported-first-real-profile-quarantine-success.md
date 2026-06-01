# Feature: User-Reported First Real-Profile Quarantine Success

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Record the user's successful first exact real-profile Quarantine execution as manual live-trust evidence without enabling any broader cleanup, restore, deletion, or history behavior.

## Non-goals

- Do not run real-profile Quarantine from Codex or automated tests.
- Do not run selected restore from Codex or automated tests.
- Do not enable custom or non-exact real-profile Quarantine.
- Do not enable broad or all-manifest real-profile Undo Quarantine.
- Do not enable permanent deletion.
- Do not add persisted cleanup history.

## Evidence

The user reported WPF success on 2026-06-01 for one approved exact real-profile Quarantine batch:

- Cleanup Scope: exact `C:\Users\moxhe`.
- Source: `C:\Users\moxhe\AppData\Local\pip\cache\http\b\c`.
- Quarantine destination: `D:\WindowsFileCleanerQuarantine\actions\quarantine-action-draft-20260601112432-ab98a4a0\items\AppData\Local\pip\cache\http\b\c`.
- Restore Manifest: `restore-manifest-20260601112432-a715565e`.
- Restore Manifest status: `Completed`.
- Entries: `1`.
- Bytes: `13.97 MB`.
- Execution result: `moved 1, failed 0, blockers 0, recovery review: no`.
- Readiness blockers: `0`.

Codex did not click the real-profile movement path and did not move, restore, delete, or modify real-profile files.

## Current Behavior After This Evidence

The first exact real-profile Quarantine path is now implementation-complete and user-trusted for one tiny batch. It remains constrained by ADR 0017 and ADR 0018:

- exact `C:\Users\moxhe` scope only,
- exact `QUARANTINE`,
- first-phase limits of 10 rows and 1 GB,
- `Likely safe` plus `Quarantine candidate` rows only,
- strict descendant checks for folders,
- Quarantine Root Execution Safety,
- Real-Profile Quarantine Approval Evidence,
- immediate Pre-Execution Revalidation,
- Restore Manifest-only durable record,
- manual rescan guidance.

Broad/all-manifest real-profile Undo Quarantine, custom/non-exact real-profile Quarantine, custom selected restore, permanent deletion, and persisted cleanup history remain unavailable.

## Follow-Up Work

- Recovery proof for this created Restore Manifest later succeeded by user report after the cross-volume selected restore retry fix: highlighted result `selected restore succeeded. Restored 1, failed 0`.
- The user then rediscovered manifests and rescanned, reporting that the manifest showed restored/already restored, the rescan completed normally, and the restored `pip\cache\http\b\c` path appeared again.
- Do not proceed to larger real-profile Quarantine batches without fresh preflight, a new tiny selected batch, and explicit user review; broad/all-manifest restore remains unavailable.
- Keep packaging and any deletion/history decisions separate from this first live-trust evidence.

## Verification

- User-reported WPF execution success for the specific approved batch.
- `git diff --check`

## ADRs

No ADR added. ADR 0017 and ADR 0018 already govern this exact first real-profile Quarantine path; this feature brief records manual trust evidence.

## Risks And Assumptions

- The reported WPF output is accepted as manual evidence.
- A single successful tiny batch proves the narrow path works, not that larger or different real-profile cleanup targets are safe.
- Recovery was later proven through selected restore for this first-live manifest by user report, but larger real-profile cleanup still needs fresh readiness review and tiny-batch discipline.
