# Feature: Quarantine Readiness Summary Preview-Only Wording

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Make the compact Quarantine Readiness Summary use clearer user-facing wording for preview-only scopes.

This keeps the most visible summary line focused on the user's safety question: whether file movement is available.

## Non-goals

- Do not change detailed Quarantine Execution Gate evidence.
- Do not enable real-profile or custom Quarantine execution.
- Do not change fixture execution behavior.
- Do not move, restore, delete, create, or rewrite real-profile files.

## Current behavior

For real-profile and custom preview-only scopes, the compact summary now says movement is unavailable instead of showing the more technical `Current build can execute: no` wording.

The detailed preview/gate panes still show the full Execution Readiness contract, including `Current build can execute from this readiness model: no`.

## Decisions made

- Keep the detailed contract wording unchanged for auditability.
- Use `movement unavailable` only in the compact summary line where quick readability matters.

## Files changed

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `docs/features/2026-05-31-quarantine-readiness-summary-preview-only-wording.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

## Test plan

Automated tests:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`

## Completion notes

Completed on: 2026-05-31

What changed:

- Changed the compact preview-only summary suffix to `movement unavailable`.
- Updated WPF smoke assertions for custom, synthetic real-profile, and synthetic real-profile-child preview-only summaries.

ADRs:

- No ADR added. This is reversible WPF wording polish under existing ADR 0017/0018 boundaries.

Follow-up work:

- Manual fixture review should confirm whether the summary feels clearer without needing a dedicated readiness pane.

Open questions:

- None for this wording packet.

Risky assumptions:

- Keeping technical `Current build can execute...` wording in the detailed gate while simplifying only the compact summary is the right balance.
