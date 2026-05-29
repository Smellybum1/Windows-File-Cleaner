# Feature: Scan Gate Summary Help Cue

Date started: 2026-05-30
Status: completed
Owner: project-owner

## Goal

Make the visible Cleanup Scope Scan Gate status easier to inspect by adding a hoverable circular `?` help cue beside the locked/ready summary.

## Non-goals

- Do not change when `Scan` is enabled.
- Do not run MVP preflight from the WPF app.
- Do not create fixtures from the WPF app.
- Do not start a Storage Scan automatically.
- Do not persist acknowledgement state.
- Do not enable real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, or persisted cleanup history.

## Desired behavior

- The scan-gate summary has tooltip and automation help text that mirror the current locked/ready summary.
- A nearby circular `?` help cue mirrors that same tooltip/help text.
- Locked real-profile, acknowledged real-profile, fixture, and custom scopes keep scope-specific boundary wording.
- The cue uses the same Help cursor and prompt tooltip delay as the other circular help cues.

## Completion notes

Completed on: 2026-05-30

What changed:

- Wrapped the scan-gate summary in a small header row with a visible circular `?` help cue.
- Added dynamic summary tooltip and automation help text for real-profile locked, real-profile ready, fixture ready, custom ready, and invalid/blank states.
- Mirrored the summary help text onto the cue.
- Expanded WPF smoke assertions to cover scan-gate summary/cue help text for locked real-profile, acknowledged real-profile, fixture, and custom scopes.
- Expanded the tracked hoverable help-cue affordance list to nine cues.
- Kept scan-gate behavior, scan execution, Quarantine, restore, deletion, and cleanup history unchanged.

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"` passed.
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe` passed.
- Later verification packet `2026-05-30: Run Full Local MVP Preflight After Scan Gate Cue` passed `cmd.exe /c tools\Invoke-MvpPreflight.cmd` without scanning or modifying real user files.

ADRs added or skipped:

- Skipped. This is reversible WPF help-affordance polish with no architecture, persistence, cleanup execution, restore rule, data-model, or security change.

Open questions:

- During the next visible fixture or real-profile pass, confirm the extra cue still leaves the header comfortable.

Risky assumptions:

- A scan-gate summary cue improves discoverability without making the header feel too busy.
