# Feature: Confirmation Label Wording

Date started: 2026-05-29  
Status: completed  
Owner: project-owner

## Goal

Normalize WPF confirmation labels so Quarantine and Selected Restore panes say `Required confirmation text` instead of stale `Required future text`.

## Non-goals

- Do not change the required `QUARANTINE` or `RESTORE` confirmation phrases.
- Do not change Quarantine Preview eligibility rules.
- Do not change Quarantine or selected restore execution gates.
- Do not enable real-profile Quarantine execution or real-profile restore.
- Do not move, restore, delete, create, or modify files.

## Desired behavior

The WPF Quarantine Preview pane and Selected Restore Execution Gate pane should use current confirmation wording:

- `Required confirmation text: QUARANTINE`
- `Required confirmation text: RESTORE`

The wording should not imply a new approval state or broader execution availability.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Confirmation Draft | Use current `required confirmation text` wording in WPF/docs. | yes |
| Selected Restore Confirmation Draft | Use current `required confirmation text` wording in WPF/docs. | yes |

## Evidence and validation gate

Evidence gathered:

- Fixture-only Quarantine execution and fixture-only selected restore now exist, so `Required future text` is stale in WPF panes.
- Existing execution gates still enforce exact confirmation text and fixture-only availability.
- WPF smoke tests already cover both panes.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: wording/tests only, no filesystem mutation.
- [x] The narrowest relevant verification path is app/core test harnesses plus build and diff check.

## Decisions made

Small feature-level decisions:

- Change visible WPF labels only; keep model property names as `RequiredConfirmationText`.
- Add negative WPF assertions so `Required future text` does not drift back into these panes.
- Update historical docs only where wording describes current behavior or current labels.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Update WPF formatter labels for Quarantine and Selected Restore confirmation drafts.
2. Update WPF assertions and core test wording.
3. Update domain docs, relevant feature notes, progress, and handoff.
4. Run focused tests and diff check.

## Completion notes

Completed on: 2026-05-29

What changed:

- WPF Quarantine Preview now says `Required confirmation text: QUARANTINE`.
- WPF Selected Restore Execution Gate now says `Required confirmation text: RESTORE`.
- WPF smoke tests assert the stale `Required future text` label is absent from those panes.
- Docs now use `required confirmation text` for current confirmation draft wording.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Notes:

- An initial parallel core test run hit a transient Windows build-output file lock while another .NET command was building; the sequential no-build rerun passed.

Docs updated:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/decisions/0014-use-read-only-selected-restore-confirmation-gate.md`
- `docs/features/2026-05-28-quarantine-confirmation-draft.md`
- `docs/features/2026-05-28-quarantine-readiness-ui.md`
- `docs/features/2026-05-29-confirmation-label-wording.md`
- `docs/features/2026-05-29-quarantine-execution-gate.md`
- `docs/features/2026-05-29-selected-restore-confirmation-gate.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This is reversible wording alignment with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- None for this packet.

Risky assumptions:

- `Required confirmation text` is clearer now that fixture-only execution paths exist.
