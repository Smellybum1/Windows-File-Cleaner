# Feature: Quarantine Execution Scope Status

Date started: 2026-05-29  
Status: completed  
Owner: project-owner

## Goal

Make Quarantine Preview and Quarantine Execution Gate wording clearer about fixture-only execution versus preview-only real-profile/custom scopes.

## Non-goals

- Do not enable real-profile Quarantine execution.
- Do not enable custom non-fixture Quarantine execution.
- Do not move, delete, restore, rename, create, or modify files.
- Do not change Quarantine Preview eligibility rules.
- Do not change the exact `QUARANTINE` confirmation requirement.

## Desired behavior

The WPF preview and gate output should include plain-language execution scope status:

- Fixture scopes say fixture-only execution is available only after preview readiness and exact `QUARANTINE` confirmation.
- Real-profile and custom scopes say the workflow is preview-only and execution remains unavailable.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Execution Scope Status | Added as a WPF read-only wording concept. | yes |

## Evidence and validation gate

Evidence gathered:

- Existing custom-scope WPF tests prove execution stays unavailable, but visible wording leaned on technical `Execution implemented: no` text.
- Handoff guidance recommends Quarantine Preview/readiness clarity before any real cleanup execution.
- WPF tests already cover fixture and custom non-fixture Quarantine Preview/Gate flows.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: WPF wording only, no filesystem mutation.
- [x] Narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.

## Decisions made

Small feature-level decisions:

- Add `Execution scope status:` to both Quarantine Preview and Quarantine Execution Gate text.
- Keep the technical `Execution implemented:` line for continuity with existing tests and docs.
- Reuse the same formatter for preview and gate output.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF formatter text for fixture-only and preview-only execution scope status.
2. Show the status in Quarantine Preview and Quarantine Execution Gate output.
3. Add WPF tests for fixture and custom non-fixture wording.
4. Update README, domain docs, feature brief, handoff, and progress log.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added Quarantine Execution Scope Status wording to Quarantine Preview output.
- Added Quarantine Execution Scope Status wording to Quarantine Execution Gate output.
- Added WPF smoke assertions for fixture-only and custom preview-only wording.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-execution-scope-status.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. This is reversible WPF wording/readiness clarity with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the extra line make the Quarantine Preview/Gate panes easier to scan during the next visible fixture review?

Risky assumptions:

- Keeping both `Execution implemented:` and the new plain-language scope line is clearer than replacing the technical line before manual review.
