# Feature: Manifest Discovery and Selection Help Text

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Keep Quarantine Manifest Discovery and Restore Manifest selection boundaries visible through WPF tooltips and automation help text.

## Non-goals

- Do not change discovery behavior.
- Do not change Restore Manifest selection behavior.
- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not enable permanent deletion or cleanup history.
- Do not move, restore, delete, create, write, or clean up files or folders.

## User story / job story

As the project owner, I want the manifest discovery and selection controls to explain their safety boundaries, so that read-only discovery and selected review are not mistaken for restore approval.

## Current behavior

`Discover manifests` and the Restore Manifest selection combo box already preserve the read-only safety boundary, but they did not expose tooltip or automation help text. Neighboring all-manifest and selected-manifest readiness controls already had scope help text.

## Desired behavior

- `Discover manifests` explains that discovery is read-only and does not restore, move, delete, clean up folders, or create cleanup history.
- Restore Manifest selection explains that selecting one discovered Restore Manifest is read-only review, not restore approval, and does not move, restore, delete, write manifests, or clean up folders.
- WPF smoke coverage pins the wording for both tooltip and automation help text.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Manifest Discovery | Clarify discovery control tooltip/help text expectation. | yes |
| Selected Restore Manifest Review | Clarify selection control tooltip/help text expectation. | yes |

## Open questions

Questions that must be answered before implementation:

- None. Existing ADRs already keep discovery and selected review read-only.

Questions that can be deferred:

- Do these safety-critical controls need always-visible help icons after manual fixture/real-profile review?

## Grill notes

### Scenarios discussed

- The user asked for the next small packet to favor manual fixture/real-profile polish, Quarantine Preview/readiness clarity, scan-gate discoverability, or Review Shortlist safety context before real cleanup execution.
- Existing docs define Quarantine Manifest Discovery as read-only and Selected Restore Manifest Review as selection, not approval.

### Edge cases

- The selection combo box starts disabled before discovery.
- Discovery can run before a Storage Scan and must still present read-only boundaries.
- Selection can become stale when discovery is rerun or the Quarantine Root changes.

### Dependencies between decisions

- Depends on ADR 0011 read-only Quarantine Manifest Discovery.
- Depends on ADR 0013 read-only Selected Restore Manifest Review.
- Works alongside ADR 0015 fixture-only selected restore execution without enabling real-profile restore.

## Evidence and validation gate

Evidence gathered:

- User answers: real-profile scan works; debounced search is good enough with status-bar pending-search text; real-profile cleanup must stay unavailable.
- Existing code/docs inspected: WPF manifest discovery/selection controls, WPF app smoke tests, README, domain context, glossary, feature briefs, ADRs 0011, 0013, 0014, and 0015.
- Tests/checks planned: WPF app smoke test, build, diff checks.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add restore behavior from manifest discovery.
- Do not treat Restore Manifest selection as approval.
- Do not add cleanup history or quarantine-folder cleanup while polishing help text.

## Decisions made

Small feature-level decisions:

- Use disabled-state WPF tooltips plus matching `AutomationProperties.HelpText`.
- Keep the wording focused on read-only discovery and selected review, not all future restore design.

ADR-worthy decisions:

- [x] None. This is reversible WPF metadata/test polish.

## Implementation plan

1. Add tooltip and automation help text to `Discover manifests`.
2. Add tooltip and automation help text to the Restore Manifest selection combo box.
3. Add test-facing accessors and WPF smoke assertions.
4. Update README, domain docs, progress, handoff, and this feature brief.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Possible:

- None.

## Test plan

Manual checks:

- During the next fixture or real-profile review, hover/focus `Discover manifests` and Restore Manifest selection to confirm the wording fits comfortably.

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git diff --check`

## Risks and assumptions

Risks:

- More tooltip text can feel dense in the visible app.

Assumptions:

- Tooltip and automation help text are sufficient for this small packet; always-visible help can wait for manual review evidence.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added disabled-state tooltip and automation help text to manifest discovery and selection controls.
- Added test-facing accessors and WPF smoke assertions for read-only discovery and not-approval selection wording.
- Updated README, domain docs, progress, handoff, and this feature brief.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/thread-handoff.md`
- `docs/features/2026-05-29-manifest-discovery-selection-help-text.md`
- `.codex/progress.md`

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, handoff, this feature brief, and progress log.

ADRs added or skipped:

- No ADR added. This is reversible WPF metadata/test polish with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Restore Manifest selection later gained a visible `?` help cue that mirrors the selection tooltip/help text.
- Confirm in visible fixture/real-profile review whether `Discover manifests` also needs an always-visible help icon.
- Continue polishing Quarantine Preview/readiness clarity before any real-profile cleanup execution.

Open questions:

- Does `Discover manifests` also need a visible help affordance, or is its button tooltip enough?

Risky assumptions:

- The wording fits comfortably enough for now and does not need layout changes in this packet.
