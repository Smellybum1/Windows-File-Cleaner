# Feature: Quarantine Tab Restore Manifest Review Panel

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Move Restore Manifest discovery, selected-manifest readiness, all-manifest readiness, and selected restore gate controls into the Quarantine tab so recovery actions are findable beside the Quarantine Root and Quarantine readiness workflow.

## Context

After the successful first live selected restore recovery, the user tried to follow the rediscover/rescan instructions and reported that `Discover manifests` was not visible in the Quarantine tab. It was still buried in the Main Grid selected-row detail pane, which made the recovery workflow hard to find after the tabbed workbench split.

## Implementation

- Added a dedicated `Restore Manifest Review` panel under the Quarantine tab's `Quarantine Shortlist` panel.
- Moved `Discover manifests`, `Preview all-manifest readiness`, selected manifest selection, selected manifest readiness, selected restore confirmation/gate, selected restore result highlight, and all-manifest readiness output into that panel.
- Moved detailed Quarantine Preview output from the Main Grid detail pane into the Quarantine tab near the Quarantine readiness summary.
- Left Main Grid focused on scan rows, current-session quarantined rows, and selected-row evidence/guidance.
- Added WPF smoke coverage proving the Restore Manifest Review panel is expanded inside the Quarantine tab and not in the Main Grid tab.

## Verification

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `tools/Start-MvpFixtureReview.ps1`

## ADRs

No ADR added. ADR 0016 already keeps older/discovered Restore Manifest review in manifest discovery/readiness panes rather than the current-session quarantined grid; this packet only places that surface in the correct Quarantine tab.

## Follow-up Work

- During the next visual pass, confirm the Restore Manifest Review panel is easy to find below Quarantine Shortlist and does not make the Quarantine tab feel crowded.

## Risks And Assumptions

- The Quarantine tab can scroll enough to hold both Quarantine execution and Restore Manifest recovery review without hiding important controls.
