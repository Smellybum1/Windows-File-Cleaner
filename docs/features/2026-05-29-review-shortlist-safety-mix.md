# Feature: Review Shortlist Safety Mix

Date started: 2026-05-29  
Status: completed  
Owner: project-owner

## Goal

Make the Review Shortlist safer to use during real-profile review by showing a compact read-only mix of shortlisted rows before Quarantine Preview.

## Non-goals

- Do not move, delete, quarantine, restore, or modify files.
- Do not enable real-profile Quarantine execution.
- Do not persist the Review Shortlist.
- Do not treat shortlist counts as cleanup approval or storage savings.

## Desired behavior

When rows are shortlisted, the WPF app shows `Shortlist safety mix` with counts for Likely safe, Caution, High risk, Quarantine candidates, Protected, Access issues, No category, and the largest shortlisted row. Empty, removed, and cleared shortlist states return to empty wording.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Review Shortlist Safety Mix | Added as a read-only composition summary for Review Shortlist. | yes |

## Evidence and validation gate

Evidence gathered:

- User asked to prefer Review Shortlist safety context before any real cleanup execution.
- Existing Review Shortlist and Quarantine Preview docs already define the non-approval boundary.
- WPF smoke tests already cover shortlist actions and Quarantine Preview state.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: completed scan data only, no filesystem mutation.
- [x] Narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.

## Decisions made

Small feature-level decisions:

- Keep the mix as a WPF readout computed from the current in-memory Review Shortlist.
- Include largest shortlisted row as a triage cue, not a savings estimate.
- Keep Quarantine Preview as the next readiness check; the mix does not evaluate blockers.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add a WPF `ShortlistSafetyMixText` readout near Review Shortlist controls.
2. Recompute it when scan results or Review Shortlist membership changes.
3. Add WPF app test coverage for empty, shortlisted, removed, cleared, and broad-parent states.
4. Update README, domain docs, and progress log.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added the WPF Review Shortlist Safety Mix readout.
- Added app smoke assertions for the new readout.
- Kept behavior read-only and separate from Quarantine Preview and fixture-only execution.
- Later packet `2026-05-30-review-shortlist-safety-mix-help-text.md` mirrored the dynamic readout into tooltip and automation help text with no-rescan/no-file-modified/no-readiness-proof/no-savings-proof/not-cleanup-approval wording.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-review-shortlist-safety-mix.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. This is a reversible read-only UI context improvement with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Should a later layout pass convert Review Shortlist Safety Mix into compact chips or a table if the visible line feels too dense?

Risky assumptions:

- A compact text readout is enough until a manual fixture/real-profile review pass shows otherwise.
