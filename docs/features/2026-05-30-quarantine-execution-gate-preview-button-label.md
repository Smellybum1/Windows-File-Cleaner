# Feature: Quarantine Execution Gate Preview Button Label

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the WPF Quarantine Execution Gate blocker point to the visible `Preview shortlist quarantine` button label.

## Non-goals

- Do not change core Quarantine Execution Gate semantics.
- Do not change Quarantine Preview, fixture execution, undo, selected restore, manifests, or scan behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

- When the WPF execution gate is blocked because no preview exists, the visible blocker says `Use Preview shortlist quarantine before entering confirmation text.`
- The core gate can keep its domain-oriented `Create a Quarantine Preview...` blocker for non-UI use.
- Startup and stale-preview reset smoke coverage guard against the older visible blocker phrase.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- The visible preview button is `Preview shortlist quarantine`.
- The WPF execution gate displayed the core blocker `Create a Quarantine Preview before entering confirmation text.`, which is technically correct but does not point to the visible button label.
- Recent placeholder polish already aligned nearby placeholder text to `Preview shortlist quarantine`.

Validation gate before implementation:

- [x] This is WPF display wording only.
- [x] The core gate behavior should remain unchanged.
- [x] The narrowest relevant verification path is focused WPF app smoke tests plus stale-label search and diff check.

## Completion notes

Completed on: 2026-05-30

What changed:

- Added a WPF display formatter for Quarantine Execution Gate blockers.
- Translated only the missing-preview blocker to `Use Preview shortlist quarantine before entering confirmation text.`
- Updated WPF smoke assertions for startup and stale-preview reset gate wording.
- Left core Quarantine Execution Gate builder behavior unchanged.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `rg -n "Create a Quarantine Preview before entering confirmation text|Preview quarantine\." src\WindowsFileCleaner.App\MainWindow.xaml README.md docs\codex\thread-handoff.md tools`
- `git diff --check`

ADRs added or skipped:

- Skipped. This is reversible WPF display wording polish and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Risky assumptions:

- Translating this blocker in the WPF layer is clearer for users than changing the core domain wording.
