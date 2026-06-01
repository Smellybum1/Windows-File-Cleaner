# Feature: Next-Batch Checklist Fixture Notes Guidance

Date started: 2026-06-02
Status: completed
Owner: project-owner

## Goal

Make the Real-Profile Next-Batch Checklist explicitly remind the reviewer how to handle Fixture Acceptance Notes status before any future tiny exact `C:\Users\moxhe` Quarantine batch.

## Non-goals

- Do not launch WPF.
- Do not click `Scan`, scan `C:\Users\moxhe`, move, restore, delete, quarantine, approve cleanup, write Restore Manifests, or create cleanup history.
- Do not make completed Fixture Acceptance Notes a default strict gate.
- Do not record fixture notes automatically from existing chat or user-reported visual acceptance.
- Do not weaken the next-batch evidence preset, WPF readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, immediate Pre-Execution Revalidation, or explicit user approval.

## Context

The next-batch evidence preset already includes optional Fixture Acceptance Notes status. Incomplete summaries now print the exact `Record-FixtureAcceptanceNotes.cmd -Path ... -RecordManualAcceptance` command, but that recorder remains only for after an actual all-pass visible fixture review.

The standalone checklist is the terminal prompt a reviewer sees immediately before manual WPF work. It should carry the same incomplete-notes boundary so the reviewer does not have to remember it from earlier terminal output.

## Implementation

- Added one `Before launch` checklist item to `tools\Show-RealProfileNextBatchChecklist.ps1`.
- The new item tells the reviewer to confirm Fixture Acceptance Notes status from the evidence preset.
- If formal notes are incomplete, it points back to the printed summary guidance.
- It preserves the all-pass-human-review boundary for `Record-FixtureAcceptanceNotes.cmd -RecordManualAcceptance` and says to fill notes manually when there were issues or not-checked items.

## Verification

- `cmd.exe /c tools\Show-RealProfileNextBatchChecklist.cmd` passed and printed the new Fixture Acceptance Notes reminder without launching WPF, scanning, movement, restore, deletion, approval, manifest writes, or cleanup history.
- `cmd.exe /c tools\Invoke-RealProfileNextBatchReview.cmd -SkipMvpPreflight` passed and printed the reminder through the combined wrapper after read-only evidence. Skipped preflight remains smoke evidence only, not fresh movement evidence.
- Follow-up full evidence: `cmd.exe /c tools\Invoke-RealProfileQuarantineReadiness.cmd -RequireNextBatchEvidence` passed on `d257090`, including full MVP preflight and current exact-profile manifest evidence, without launching WPF, scanning the real profile, movement, restore, deletion, approval, manifest writes, or cleanup history.
- `rg -n "Fixture Acceptance Notes status|actual all-pass visible fixture review|Next-Batch Checklist Fixture Notes Guidance|Record-FixtureAcceptanceNotes" tools docs README.md .codex` passed.
- `git diff --check` passed with expected line-ending normalization warnings only.

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/features/2026-06-02-full-next-batch-evidence-after-checklist-guidance.md`
- `docs/features/2026-06-02-next-batch-checklist-fixture-notes-guidance.md`
- `docs/features/2026-06-02-real-profile-next-batch-wpf-checklist.md`
- `docs/features/2026-06-02-real-profile-next-batch-review-wrapper.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. This is terminal checklist wording over existing ignored local evidence tooling and ADR 0017/0018/0019 movement gates.

## Follow-up Work

- Keep formal Fixture Acceptance Notes completion optional unless a future review explicitly supplies `-RequireFixtureAcceptanceComplete`.
- Use `Record-FixtureAcceptanceNotes.cmd -RecordManualAcceptance` only after an actual all-pass visible fixture review.

## Risks And Assumptions

- Risk: the checklist grows verbose, but the added line sits before launch where the manual-review blocker is easiest to catch.
- Assumption: repeating the incomplete-notes boundary in the checklist reduces friction without making fixture acceptance automatic.
