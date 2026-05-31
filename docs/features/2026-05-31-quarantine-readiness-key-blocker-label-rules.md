# Feature: Quarantine Readiness Key-Blocker Label Rules

Date started: 2026-05-31
Status: completed
Owner: project-owner

## Goal

Keep the compact Quarantine Readiness Summary key-blocker labels maintainable by making one rule table drive both priority selection and fallback label formatting.

## Non-goals

- Do not change visible WPF wording or label priority.
- Do not enable real-profile Quarantine execution.
- Do not enable real-profile Undo Quarantine, selected real-profile restore, permanent deletion, or cleanup history.
- Do not create folders, write Restore Manifests, move files, restore files, delete files, or scan `C:\Users\moxhe`.

## Implementation

- Replaced duplicated key-blocker pattern checks in `MainWindow.xaml.cs` with a shared `ReadinessKeyBlockerLabelRules` table.
- Kept the compact summary output, first-phase blocker priority, and fallback dimension behavior unchanged.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`

## ADRs

No ADR added. This is a behavior-preserving maintainability cleanup under accepted ADR 0018.

## Open questions

- None.

## Follow-up work

- Continue visible manual fixture review of compact readiness wording before considering any real-profile execution wiring.

## Risky assumptions

- Existing WPF coverage around compact summary labels is sufficient for this small refactor.
