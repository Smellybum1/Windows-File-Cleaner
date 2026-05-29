# Feature: Hoverable Help Cue Affordance Coverage

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Add focused automated coverage for the existing hoverable circular `?` help-cue affordance so future WPF changes do not silently remove the Help cursor or prompt tooltip delay.

## Non-goals

- Do not change visible WPF layout, tooltip wording, automation help text, scan behavior, Quarantine Preview, fixture execution, undo, restore, or manifest behavior.
- Do not launch WPF for manual visual review or scan the real profile as part of this packet.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

- The seven existing circular `?` help cues remain tracked by the WPF smoke harness.
- Each tracked cue uses the Windows Help cursor.
- Each tracked cue keeps the prompt tooltip initial delay used by the current UI.

## Completion notes

Completed on: 2026-05-30

What changed:

- Exposed a small test-facing snapshot of the seven hoverable help-cue affordance settings from `MainWindow`.
- Added a WPF smoke assertion that all seven circular `?` cues use the Help cursor and `250` ms tooltip initial delay.
- Kept existing tooltip/help-text assertions and all runtime behavior unchanged.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`

ADRs added or skipped:

- Skipped. This is test coverage for existing WPF UI affordance behavior with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture pass, confirm the hoverable `?` cues and prompt tooltips are noticeable without making the dense review surface noisy.

Risky assumptions:

- A test-facing affordance snapshot is an acceptable low-risk way to prevent regression in WPF-only UI affordance settings.
