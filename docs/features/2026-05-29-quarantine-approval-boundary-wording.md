# Feature: Quarantine Approval Boundary Wording

Date started: 2026-05-29  
Status: completed  
Owner: project-owner

## Goal

Make Quarantine Preview and Quarantine Execution Gate panes clearer that Review Shortlist and Quarantine Preview are review evidence, not cleanup approval.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable custom non-fixture Quarantine execution.
- Do not change Quarantine Preview eligibility rules.
- Do not change the exact `QUARANTINE` confirmation requirement.
- Do not move, restore, delete, create, or modify files.

## Desired behavior

The WPF Quarantine Preview pane and Quarantine Execution Gate pane should include a compact approval-boundary line:

- Fixture scopes should say Review Shortlist and Quarantine Preview are not cleanup approval, and exact `QUARANTINE` can open only fixture execution in this build.
- Real-profile and custom scopes should say Review Shortlist and Quarantine Preview are not cleanup approval, and real-profile/custom execution remains unavailable.

## Domain language changes

No new durable term. This packet reinforces existing Quarantine Preview, Review Shortlist, and Quarantine Execution Gate boundaries.

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Preview | Clarified that WPF preview/gate panes should keep the non-approval boundary visible. | yes |

## Evidence and validation gate

Evidence gathered:

- Handoff guidance recommends Quarantine Preview/readiness clarity before real cleanup execution.
- Existing preview/gate text already shows scope status, but the non-approval boundary is spread across README, status text, and surrounding controls.
- WPF smoke tests already exercise fixture and custom non-fixture preview/gate panes.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: wording and tests only, no filesystem mutation.
- [x] The narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.

## Decisions made

Small feature-level decisions:

- Add an `Approval boundary:` line to both Quarantine Preview and Quarantine Execution Gate text.
- Use scope-specific wording so fixture-only execution remains distinct from real/custom preview-only scopes.
- Keep existing `Execution scope status:` lines for continuity; later packet `2026-05-30-quarantine-gate-technical-wording.md` removes the technical implementation flag from visible pane text.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF approval-boundary formatting for fixture and preview-only scopes.
2. Show the line in Quarantine Preview and Quarantine Execution Gate output.
3. Add WPF assertions for fixture and custom scope wording.
4. Update README, domain docs, progress, and handoff.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added `Approval boundary:` wording to Quarantine Preview and Quarantine Execution Gate panes.
- Fixture wording says exact `QUARANTINE` can open only fixture execution in this build.
- Preview-only wording says real-profile and custom execution remain unavailable.
- Added WPF smoke assertions for fixture and custom scope panes.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-approval-boundary-wording.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

ADRs added or skipped:

- Skipped. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the extra line make the preview/gate panes easier to scan during the next visible fixture review?

Risky assumptions:

- A concise extra line improves safety clarity more than it increases pane density.
