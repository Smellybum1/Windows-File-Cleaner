# Feature: Quarantine Preview Placeholder Label

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Align visible Quarantine placeholder text with the current `Preview shortlist quarantine` button label.

## Non-goals

- Do not change Quarantine Preview, fixture execution, undo, selected restore, manifests, or scan behavior.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

- The initial Quarantine Preview pane placeholder points to `Preview shortlist quarantine`.
- Any hardcoded Quarantine Execution Gate placeholder points to `Preview shortlist quarantine`.
- Reset placeholder text after stale preview invalidation also uses `Preview shortlist quarantine`.

## Domain language changes

No new domain terms.

## Evidence and validation gate

Evidence gathered:

- The visible preview button is `Preview shortlist quarantine`.
- Startup and reset placeholders still said `Preview quarantine`, which could send manual reviewers looking for an old label.
- README manual review already uses the current visible label.

Validation gate before implementation:

- [x] This is visible wording alignment only.
- [x] The narrowest relevant verification path is focused WPF app smoke tests plus stale-label search and diff check.

## Completion notes

Completed on: 2026-05-30

What changed:

- Updated WPF Quarantine Preview and hardcoded Quarantine Execution Gate placeholder text to use `Preview shortlist quarantine`.
- Updated the preview reset path after stale preview invalidation to use the same label.
- Added WPF smoke assertions for startup and stale-reset placeholder wording, including a guard against the old preview button phrase in execution-gate startup/reset text.
- Updated the current progress prompt for the next manual fixture pass to name `Preview shortlist quarantine` and `Export preview`.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `rg -n "after using Preview quarantine|Preview quarantine/export" src tests README.md docs\codex\thread-handoff.md tools`
- `rg -n "Preview quarantine\." src\WindowsFileCleaner.App README.md docs\codex\thread-handoff.md tools`
- `git diff --check`
- Later verification packet: `cmd.exe /c tools\Invoke-MvpPreflight.cmd` passed; restore, build, core tests, WPF app tests, fixture `-WhatIf`, fixture checklist-only output, and whitespace diff check all passed without scanning or modifying real user files.

ADRs added or skipped:

- Skipped. This is reversible WPF wording polish and does not change architecture, persistence, cleanup execution, restore rules, data model, or security.

Open questions:

- None.

Risky assumptions:

- The longer `Preview shortlist quarantine` phrase is worth using in placeholders because it matches the visible button and makes Review Shortlist scope clear.
