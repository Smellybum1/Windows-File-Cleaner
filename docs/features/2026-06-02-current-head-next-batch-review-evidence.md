# Feature: Current-Head Next-Batch Review Evidence

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Refresh the full terminal-only next-batch review evidence on current `main` before asking the user to do another manual tiny exact `C:\Users\moxhe` WPF batch review.

## Non-goals

- Do not publish or accept a new package.
- Do not launch WPF.
- Do not click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, quarantine, write Restore Manifests, approve cleanup, or create cleanup history.
- Do not record Fixture Acceptance Notes automatically.
- Do not create shortcuts or install anything.
- Do not enable broad/all-manifest restore, custom/non-exact real-profile Quarantine, custom selected restore, permanent deletion, or persisted cleanup history.

## Evidence

`cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd` passed on current `main` at `c7cb545`.

The command:

- ran full MVP preflight: restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and whitespace diff checking;
- verified completed accepted package notes for `.local\releases\windows-file-cleaner-v20260602-011556` at accepted package commit `bc9b869`;
- printed optional Fixture Acceptance Notes status and confirmed `.local\fixture-review-acceptance\fixture-acceptance-20260601-132126.md` remains formally unfilled;
- verified the accepted package once with the expected package/current-HEAD warning because the accepted package commit `bc9b869` differs from current `HEAD` `c7cb545`;
- printed accepted normal and fixture launch commands in print-only mode without launching WPF;
- showed exact-profile Restore Manifest display `(4 of 10)`, displayed undo work `0`, and displayed recovery review `2`;
- showed focused recovery-review evidence for the two older failed NVIDIA `DXCache` attempts;
- showed focused undo-work evidence with zero exact-profile matches;
- printed the manual WPF next-batch checklist.

## Decisions

- Treat the fresh wrapper pass as enough terminal evidence to ask the user for the next manual WPF batch review.
- Keep the accepted package baseline unchanged; this packet is current-head evidence only.
- Keep incomplete Fixture Acceptance Notes visible but optional unless a future command explicitly uses `-RequireFixtureAcceptanceComplete`.
- Keep exact-profile recovery-review debt visible while gating the next-batch evidence on displayed undo work staying zero.

## Verification

- `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd`

## Docs

- `docs/features/2026-06-02-current-head-next-batch-review-evidence.md`
- `docs/features/2026-06-02-real-profile-next-batch-review-wrapper.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is read-only terminal evidence over existing ADR 0017/0018/0019 movement gates and ADR 0020 deployment boundaries.

## Follow-up Work

- Ask the user to do the manual WPF next-batch review if they are ready.
- Keep the batch exact `C:\Users\moxhe`, at most 10 rows and 1 GB, Likely safe plus Quarantine candidate only, and stop on any hard blocker.
- Do not click Quarantine unless the user explicitly chooses a specific tiny batch after WPF readiness, exact `QUARANTINE`, approval evidence, and immediate Pre-Execution Revalidation are visible.

## Risks And Assumptions

- The accepted-package/current-HEAD warning remains expected while the accepted package is behind newer docs-only commits.
- The current exact-profile recovery-review manifests are failed-only historical evidence with no displayed undo work, so they remain review context rather than a default blocker.
