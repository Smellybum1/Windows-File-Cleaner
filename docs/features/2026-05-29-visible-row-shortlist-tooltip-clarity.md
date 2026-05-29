# Feature: Visible Row Shortlist Tooltip Clarity

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the Review Shortlist visible-row bulk actions explain their scope and safety boundary on hover.

## Non-goals

- Do not change which rows are added or removed.
- Do not persist the Review Shortlist.
- Do not change Quarantine Preview eligibility.
- Do not move, restore, delete, create, or modify scanned files.
- Do not enable real-profile Quarantine execution.

## User story / job story

As the project owner, I want the visible-row shortlist bulk actions to explain that they affect only the current displayed review rows and are not cleanup approval, so that I can use them during large real-profile review without confusing review context for execution consent.

## Current behavior

The buttons are labeled `Shortlist visible rows` and `Remove visible rows`, and they already affect only the current Storage Review Display Window. Their scope and no-file-modified boundary are visible in status text after use, but not before use.

## Desired behavior

- `Shortlist visible rows` has a tooltip explaining that it applies only to the currently visible review rows.
- `Remove visible rows` has a tooltip explaining that it removes only currently visible review rows from Review Shortlist.
- Both tooltips say Review Shortlist is review context, not cleanup approval, and that no files are modified.
- Disabled-state tooltip behavior keeps the boundary available before scan state enables the buttons.

## Domain language changes

No new durable term.

| Term | Change | Docs updated? |
|---|---|---|
| Review Shortlist | Clarify that visible-row bulk controls include scope tooltips and remain review context only. | yes |

## Evidence and validation gate

Evidence gathered:

- The visible-row labels clarified button names, but their follow-up question asked whether icons or tooltips should be added.
- Review Shortlist already has a strong non-approval boundary in domain docs and status text.
- Handoff guidance prefers Review Shortlist safety context before any real cleanup execution.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: UI tooltip wording and tests only, no filesystem mutation.
- [x] The narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.

Rejected ideas buffer:

- Do not add icons in this packet; tooltip clarity is smaller and directly addresses the ambiguity.
- Do not make Review Shortlist persistent or executable.
- Do not introduce cleanup approval semantics.

## Decisions made

Small feature-level decisions:

- Use hover tooltips instead of adding more always-visible toolbar text.
- Include disabled-state tooltips so the boundary is inspectable before buttons become enabled.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add WPF tooltips to the visible-row add/remove buttons.
2. Add test-facing tooltip accessors and WPF smoke assertions.
3. Update README, domain docs, fixture checklist, progress, handoff, and the related visible-row label feature note.
4. Run WPF smoke tests, build, checklist-only output, and diff check.

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Manual checks:

- During fixture review, confirm the tooltip text is discoverable and the toolbar still fits comfortably.

## Risks and assumptions

Risks:

- Tooltip-only guidance may still be less discoverable than inline text for keyboard-only review.

Assumptions:

- The existing labels plus status text are enough visible guidance, and tooltips are the right next small polish.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added WPF disabled-state tooltips to `Shortlist visible rows` and `Remove visible rows`.
- Added WPF smoke assertions for visible-row scope, not-cleanup-approval wording, and no-file-modified wording.
- Updated README, domain docs, fixture checklist, progress, handoff, and the related visible-row label feature note.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Start-MvpFixtureReview.ps1 -ChecklistOnly`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, related feature brief, progress log, handoff, and this feature brief.

ADRs added or skipped:

- Skipped. This is reversible UI tooltip clarity with no architecture, persistence, cleanup execution, restore, security, deployment, or data-model change.

Follow-up work:

- Check during visible fixture/real-profile review whether the toolbar remains comfortable with disabled-state tooltip discovery.

Open questions:

- Would a later keyboard-accessibility pass need explicit help text or focus descriptions beyond hover tooltips?

Risky assumptions:

- Hover tooltips are enough extra clarity for this small review packet.
