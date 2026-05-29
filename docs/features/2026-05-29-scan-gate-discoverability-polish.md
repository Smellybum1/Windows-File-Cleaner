# Feature: Scan Gate Discoverability Polish

Date started: 2026-05-29  
Status: completed  
Owner: project-owner

## Goal

Make the Cleanup Scope Scan Gate easier to notice before a real-profile Storage Scan, without weakening the safety gate or running any file-modifying workflow.

## Non-goals

- Do not run MVP preflight from the WPF app.
- Do not create fixtures from the WPF app.
- Do not persist acknowledgement state.
- Do not scan or modify real-profile files from this packet.
- Do not add Quarantine execution, Undo Quarantine, permanent deletion, or cleanup history.

## Desired behavior

The WPF header should make scan readiness visible:

- Real-profile scopes without acknowledgement show a short locked-state summary.
- The disabled `Scan` button has a tooltip explaining the lock.
- Acknowledged real-profile scopes show a read-only ready summary.
- Fixture scopes show fixture readiness and keep the read-only Scan tooltip.

## Domain language changes

No new terms.

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope Scan Gate | Clarified visible summary/tooltip expectations. | yes |

## Evidence and validation gate

Evidence gathered:

- The current real-profile gate was functionally tested, but the checkbox and gray gate text were visually quiet.
- Project guidance asks for scan-gate discoverability before any real cleanup execution.
- WPF app startup tests already cover real-profile and fixture launch states.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Permission boundary is clear: UI wording only, no scan or cleanup execution.
- [x] Narrowest relevant verification path is `WindowsFileCleaner.App.Tests`.

## Decisions made

Small feature-level decisions:

- Add a concise `ScanGateSummaryText` line rather than a modal dialog.
- Add `Scan` button tooltips for locked and ready states.
- Keep the existing acknowledgement checkbox and scan-start enforcement unchanged.

ADR-worthy decisions:

- [x] None

## Implementation plan

1. Add a visible scan-gate summary line in the WPF header.
2. Update the summary and `Scan` button tooltip from existing Cleanup Scope Safety Note and Cleanup Scope Scan Gate state.
3. Add WPF smoke assertions for locked real-profile, acknowledged real-profile, and fixture states.
4. Update README, domain docs, feature brief, and progress log.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added `ScanGateSummaryText` to the WPF header.
- Added `Scan` button tooltip wording for locked and ready states.
- Added WPF test coverage for real-profile locked, real-profile acknowledged, and fixture-ready scan-gate discoverability.

Tests run:

- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `dotnet build WindowsFileCleaner.sln --no-restore`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `README.md`
- `docs/domain/context.md`
- `docs/features/2026-05-29-scan-gate-discoverability-polish.md`
- `.codex/progress.md`

ADRs added or skipped:

- Skipped. This is a reversible WPF wording/discoverability improvement with no persistence, cleanup execution, restore, security, deployment, or data-model change.

Open questions:

- Does the extra header line fit comfortably during the next visible fixture and real-profile review pass?

Risky assumptions:

- A summary line plus tooltip is enough discoverability until the next manual visible UI pass proves otherwise.
