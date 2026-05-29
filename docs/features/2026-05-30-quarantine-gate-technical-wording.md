# Feature: Quarantine Gate Technical Wording

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Remove technical `Execution implemented` wording from visible Quarantine Preview and Quarantine Execution Gate panes now that fixture-only versus preview-only behavior is explained by plain scope-status and approval-boundary lines.

## Non-goals

- Do not change Quarantine Preview eligibility.
- Do not change Quarantine Confirmation Draft or Quarantine Execution Gate semantics.
- Do not change fixture-only Quarantine execution.
- Do not enable real-profile or custom Quarantine execution.
- Do not change current-fixture undo, selected restore, manifest discovery, restore readiness, permanent deletion, or cleanup history.

## Desired behavior

- Quarantine Preview output shows included/blocked/redundant counts, required `QUARANTINE`, `Execution scope status`, `Approval boundary`, and the action draft target.
- Quarantine Execution Gate output shows `Execution scope status`, `Approval boundary`, confirmation-match state, and `Can execute`.
- Visible Quarantine Preview/Gate output does not show the technical `Execution implemented` field.
- Fixture Quarantine execution still opens only after clean preview readiness and exact `QUARANTINE`.
- Real-profile/custom Quarantine execution stays preview-only and unavailable.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- `docs/features/2026-05-29-quarantine-execution-scope-status.md` kept the technical line only for continuity.
- Current WPF output already includes plain `Execution scope status`, `Approval boundary`, and `Can execute` lines.
- The selected restore gate just made the same wording simplification successfully.

Validation gate before implementation:

- [x] Quarantine Execution Scope Status and Quarantine Execution Gate terms are already defined.
- [x] The change is user-facing wording only.
- [x] The narrowest relevant verification path is WPF app smoke tests plus diff check.

## Completion notes

Completed on: 2026-05-30

What changed:

- Removed `Execution implemented` from WPF Quarantine Preview and Quarantine Execution Gate display text.
- Updated WPF smoke assertions to prove fixture and custom Quarantine panes use plain scope-status wording while behavior stays the same.
- Updated domain and feature notes.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `git diff --check`
- Later verification packet: `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed; restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and whitespace diff check all passed without scanning or modifying real user files.

ADRs added or skipped:

- Skipped. This is reversible WPF wording polish and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Risky assumptions:

- `Execution scope status`, `Approval boundary`, and `Can execute` are clearer than exposing the internal implementation flag in Quarantine panes.
