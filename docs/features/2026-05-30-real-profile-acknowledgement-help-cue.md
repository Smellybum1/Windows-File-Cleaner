# Feature: Real Profile Acknowledgement Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the real-profile preflight and fixture-review acknowledgement tooltip easier to discover by adding a visible circular `?` help cue beside the checkbox.

## Non-goals

- Do not change Cleanup Scope Scan Gate behavior.
- Do not run MVP preflight from the WPF app.
- Do not create fixture files from the WPF app.
- Do not start a Storage Scan when the acknowledgement is checked.
- Do not persist acknowledgement state.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

- Real-profile Cleanup Scopes show the acknowledgement checkbox with a nearby hoverable `?` help cue.
- The cue mirrors the checkbox tooltip and automation help text.
- Fixture and custom Cleanup Scopes keep the real-profile acknowledgement cue hidden.
- The cue uses the same Help cursor and prompt tooltip delay as the other circular help cues.

## Completion notes

Completed on: 2026-05-30

What changed:

- Grouped the real-profile acknowledgement checkbox with a visible circular `?` help cue in the WPF header.
- Mirrored the checkbox tooltip and automation help text onto the cue.
- Added WPF smoke assertions for real-profile visibility, fixture/custom hidden states, mirrored tooltip/help text, and the eighth tracked hoverable help-cue affordance.
- Kept scan-gate behavior, scan execution, Quarantine, restore, deletion, and cleanup history unchanged.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.

ADRs added or skipped:

- Skipped. This is reversible WPF help-affordance polish with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next real-profile visual pass, confirm the cue is noticeable without crowding the header.

Risky assumptions:

- The existing checkbox wording remains acceptable when paired with a visible help cue.
