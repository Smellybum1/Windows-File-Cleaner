# Feature: Portable v1 Release Packaging

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Create a repeatable local portable v1 release package for Windows File Cleaner without adding an installer, deletion, broad restore, or cleanup history.

## Non-goals

- Do not add MSI/MSIX or desktop shortcut automation.
- Do not enable permanent deletion.
- Do not add persisted cleanup history.
- Do not add broad or all-manifest restore.
- Do not widen custom/non-exact real-profile Quarantine or restore.
- Do not scan, move, restore, delete, or modify real-profile files from Codex.

## Current Behavior

The app can run from the development output or `dotnet run`, and the current recovery loop is trusted for one tiny exact real-profile batch. There was no repeatable release command that produced a self-contained portable folder and zip.

## Implementation

- Added `tools\Publish-LocalRelease.ps1` and `.cmd`.
- The publisher prints current git status, runs `tools\Invoke-MvpPreflight.cmd` by default, publishes `src\WindowsFileCleaner.App\WindowsFileCleaner.App.csproj` as `Release` / `win-x64` / self-contained, and writes output under `.local\releases\windows-file-cleaner-vYYYYMMDD-HHMMSS\app`.
- The publisher verifies `WindowsFileCleaner.App.exe`, writes `release-metadata.txt`, creates `.local\releases\windows-file-cleaner-vYYYYMMDD-HHMMSS.zip`, and prints exact launch commands.
- Release metadata records branch, commit, worktree status, SDK, project, configuration, runtime, publish command, executable path, zip path, and the reversible-only safety boundary.
- Before running preflight, the publisher checks for a running Debug `WindowsFileCleaner.App` process that would lock build output and tells the user to close it instead of letting MSBuild fail with repeated file-copy errors.
- Later packet `Portable Release Launch Scripts` made the publisher write release-local `Launch-WindowsFileCleaner.cmd` and `Launch-WindowsFileCleaner-Fixture.cmd` scripts, record them in metadata, and include them in the zip without creating installed shortcuts.
- Later packet `Portable Release Start Here Readme` made the publisher write release-local `README-FIRST.txt`, record it in metadata, and include it in the zip so the portable folder carries launch instructions and the reversible-only v1 boundary.
- Later packet `Portable Release Checksum Evidence` made the publisher record the packaged executable SHA-256 in release metadata and write a sibling zip `.sha256` sidecar without adding installer behavior or cleanup history.

## Verification

- `cmd.exe /c tools\Publish-LocalRelease.cmd`
- Confirmed the published executable exists under `.local\releases\...\app`.
- Confirmed the release zip exists under `.local\releases`.
- Confirmed the packaged executable can start against the fixture Cleanup Scope without scanning or moving files automatically.
- User reran the publisher from clean `main`, launched the packaged app with the printed fixture launch command, clicked fixture Scan, and confirmed the packaged app opened and the fixture scan completed normally.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. ADR 0002 already selects WPF/.NET 8; this packet adds local publish tooling without changing app architecture, persistence, cleanup execution, restore scope, or security boundaries.

## Follow-up Work

- Use the portable package for local v1 daily use.
- Use `tools\Test-LocalRelease.cmd` for read-only local package verification.
- Decide later whether a desktop shortcut or installer is worth adding.
- Keep permanent deletion, persisted cleanup history, and broad/all-manifest restore separate Grill with Docs decisions.

## Risks And Assumptions

- The portable folder/zip is enough for v1 on the owner's machine.
- Self-contained `win-x64` output is acceptable even though it is larger than framework-dependent output.
- Release artifacts stay ignored under `.local` and are not part of durable cleanup history.
