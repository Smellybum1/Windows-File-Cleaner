# Feature: Fixture Checklist Readiness-Contract Boundary

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Keep the manual fixture review checklist aligned with ADR 0017 so fixture review does not make real-profile Quarantine feel closer to execution than it is.

## Non-goals

- Do not change WPF layout or behavior.
- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine or selected real-profile restore.
- Do not add permanent deletion, persisted cleanup history, or quarantine-folder cleanup.
- Do not scan, move, restore, delete, create, write, or clean up real-profile files.

## Current behavior

The final launcher checklist step already tells the reviewer to confirm real-profile/custom Quarantine and selected restore execution remain unavailable. After ADR 0017, that is true but incomplete: the manual prompt does not name the specific readiness-contract blockers that matter before real-profile movement can ever be enabled.

## Desired behavior

The final checklist step should make the ADR 0017 boundary explicit: Review Shortlist, exact `QUARANTINE`, a clean preview, real-profile scan acknowledgement, and exact `RESTORE` are not enough to move or restore real-profile files. Durable docs should repeat the same review expectation while keeping fixture-only execution unchanged.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Real-Profile Quarantine Readiness Contract | Reused in checklist and README wording. | yes |

## Evidence and validation gate

Evidence gathered:

- ADR 0017 requires a richer readiness contract before real-profile WPF Quarantine can move files.
- The fixture launcher checklist is the first human-facing prompt before manual fixture review.
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly` prints the checklist without preflight, fixture creation, WPF launch, scan, or file modification.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission and cleanup-execution lifecycle boundaries are clear enough.
- [x] The narrowest relevant verification path is checklist-only output plus whitespace diff check.
- [x] Open questions are deferred to the future real-profile execution design packet.

Rejected ideas buffer:

- Do not add another WPF surface just to repeat ADR 0017 before visible fixture review shows it is needed.
- Do not make checklist-only mode launch WPF or validate real-profile paths.
- Do not imply real-profile execution is available after exact confirmation or clean preview.

## Decisions made

Small feature-level decisions:

- Update the existing final checklist step instead of adding another long checklist step.
- Mention the concrete false positives: Review Shortlist, exact `QUARANTINE`, clean preview, real-profile scan acknowledgement, and exact `RESTORE`.
- Keep the packet documentation-only aside from the launcher text.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update the fixture launcher checklist final step.
2. Update README fixture/manual-review wording.
3. Add this feature brief.
4. Update progress and handoff.
5. Verify checklist-only output and whitespace.

## Completion notes

Completed on: 2026-05-31

What changed:

- Updated `Start-MvpFixtureReview.ps1` checklist step 10 to name the ADR 0017 blockers.
- Updated README fixture launcher and manual review wording to keep the real-profile readiness-contract boundary visible.
- Recorded this packet in progress and handoff docs.

Files changed:

- `tools/Start-MvpFixtureReview.ps1`
- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-31-fixture-checklist-readiness-contract-boundary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `docs/features/2026-05-31-fixture-checklist-readiness-contract-boundary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. ADR 0017 already records the durable decision; this packet only aligns manual checklist wording.

Follow-up work:

- Run a visible fixture review pass and confirm the final blocker wording matches what the WPF disabled controls show.
- Start real-profile execution design only after a future Grill with Docs pass expands ADR 0017 into implementation details.

Open questions:

- During visible review, is checklist wording enough, or should the WPF disabled gate text name more of the ADR 0017 prerequisites?

Risky assumptions:

- A clearer final checklist step is sufficient until manual review shows a WPF wording gap.
