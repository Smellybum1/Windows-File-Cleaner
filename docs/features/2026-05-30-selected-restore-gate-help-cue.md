# Feature: Selected Restore Gate Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Add a visible hoverable `?` help cue beside the WPF Selected Restore Execution Gate so its fixture-only and preview-only restore boundaries are easier to discover.

## Non-goals

- Do not change selected restore confirmation semantics.
- Do not change fixture selected restore execution behavior.
- Do not enable real-profile or custom non-fixture selected restore.
- Do not change Quarantine execution, permanent deletion, or cleanup history.

## User story / job story

As the project owner, I want the selected restore gate to have the same small visible help cue pattern as the Quarantine gate, so I can find the exact-confirmation and not-approval wording without guessing that the gate text is hoverable.

## Current behavior

The Selected Restore Execution Gate already shows scope-status and approval-boundary text, but its tooltip/help text is not exposed through an obvious visible cue.

## Desired behavior

- A compact circular `?` cue appears beside the Selected Restore Execution Gate readout.
- The cue uses the shared Help cursor and prompt tooltip delay.
- The gate readout and cue mirror the same concise tooltip and automation help text.
- The help text names the current gate state, exact `RESTORE`, fixture-only selected restore, real-profile/custom blockers, and not-restore-approval boundary.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Restore Execution Gate | Clarified visible `?` help cue behavior. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This follows the established help-cue affordance pattern.

Questions that can be deferred:

- During the next visible fixture pass, confirm the cue is noticeable without crowding the selected restore gate area.

## Grill notes

### Scenarios discussed

- User prefers a small question-mark help cue over relying on hidden tooltip discovery.
- Selected restore remains a safety-critical exact-confirmation path, so the same affordance as Quarantine Execution Gate is appropriate.

### Edge cases

- Startup/waiting state before selected restore gate preview.
- Fixture gate closed before exact `RESTORE`.
- Fixture gate open after exact `RESTORE`.
- Fixture selected restore already executed and stale review state needs rediscovery/rescan.
- Custom/real-profile selected restore stays blocked even when exact `RESTORE` matches.

### Dependencies between decisions

- Depends on ADR 0015 keeping selected restore execution fixture-only in WPF.
- Mirrors the established Quarantine Execution Gate help-cue pattern.

## Evidence and validation gate

Evidence gathered:

- User feedback favored small circular hoverable `?` help cues for discoverability.
- Existing code/docs inspected: WPF Selected Restore Execution Gate formatting, WPF smoke tests, README, domain context, glossary, thread handoff, progress log, ADR 0015, and the Quarantine Execution Gate help-cue packet.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not make the help cue a button, modal, popup, or approval control.
- Do not use this cue packet to enable real-profile selected restore.

## Decisions made

Small feature-level decisions:

- Use the existing non-clickable circular `?` cue style.
- Keep the cue help text concise and dynamic by gate state.
- Mirror the help text onto both the gate readout and cue for pointer and assistive-tech discoverability.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add a visible Selected Restore Execution Gate help cue in WPF.
2. Add dynamic selected-restore gate help text and mirror it to tooltip/automation help text.
3. Extend WPF smoke coverage for startup, fixture closed/open/restored, and custom blocked states.
4. Update README, domain docs, feature brief, handoff, progress log, and fixture checklist wording.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-30-selected-restore-gate-help-cue.md`
- `docs/codex/thread-handoff.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- During the next fixture review, hover the Selected Restore Execution Gate `?` cue after previewing selected restore gate, after exact `RESTORE`, and after fixture selected restore execution.
- Confirm real-profile/custom selected restore wording remains unavailable and preview-only.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git diff --check`

## Risks and assumptions

Risks:

- The extra cue may crowd the selected restore gate area during visible review.

Assumptions:

- A compact help cue is enough discoverability and a popup would be heavier than needed.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a visible circular `?` cue beside the Selected Restore Execution Gate.
- Mirrored dynamic gate help text onto the gate readout tooltip, gate automation help text, cue tooltip, and cue automation help text.
- Expanded hoverable-help-cue smoke coverage from twelve to thirteen tracked cues.
- Updated the fixture checklist to call out the new Selected Restore Execution Gate help cue.
- No scan behavior, Quarantine behavior, selected restore semantics, real-profile/custom restore availability, permanent deletion, or cleanup history changed.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-30-selected-restore-gate-help-cue.md`
- `docs/codex/thread-handoff.md`
- `tools/Start-MvpFixtureReview.ps1`
- `.codex/progress.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git diff --check`
- Later verification packet: `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed; restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output with the Selected Restore Execution Gate `?` help cue, and whitespace diff check all passed without scanning or modifying real user files.

Docs updated:

- README, domain context, glossary, thread handoff, progress log, fixture checklist, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible WPF affordance/help-text polish with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Follow-up work:

- Confirm the selected restore cue does not crowd the visible gate area during the next manual fixture review.
- Later packet `2026-05-30-fixture-checklist-selected-restore-gate-cue.md` added that visual check to the fixture launcher checklist and README manual review wording.

Open questions:

- Does the selected restore gate cue feel discoverable enough without a popup?

Risky assumptions:

- The existing selected restore gate state is the right source for concise cue help text.
