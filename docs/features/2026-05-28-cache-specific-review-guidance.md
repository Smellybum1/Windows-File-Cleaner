# Feature: Cache-Specific Review Guidance

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make selected-row guidance more useful for cache-heavy real scan rows without making those rows look safer than they are.

## Non-goals

- Do not change Bloat Categories.
- Do not change Importance Ratings or Deletion Recommendations.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not treat cache guidance as cleanup approval.

## User story / job story

As the project owner, I want rows like `DXCache`, `pip`, and broad `AppData` folders to explain their specific review risk, so that I can inspect likely bloat without breaking current apps or Codex.

## Desired behavior

- GPU shader cache rows should mention rebuildability and possible temporary shader recompile delays.
- Python package cache rows should mention nearby development tooling and Codex-related paths.
- Node package cache rows should mention active project dependency risk.
- App cache rows should steer review toward specific child rows instead of broad app folders.
- Generic AppData rows should remain careful-inspection guidance.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected Path Review Guidance | Tuned with cache-specific variants. | yes |

## Decisions made

- Initial packet kept GPU shader caches as `Caution` / `Inspect`; `docs/features/2026-05-28-specific-rebuildable-cache-candidates.md` later refined specific rebuildable cache rows such as `DXCache` to `Likely safe` / `Quarantine candidate`.
- This packet tuned guidance only; later candidate classification is tracked separately in the specific rebuildable cache feature brief.
- Keep guidance short enough for the existing detail pane.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added cache-specific branches to `SelectedPathReviewGuidanceBuilder`.
- Added core coverage for GPU shader cache, Python package cache, and generic AppData guidance.

ADRs added or skipped:

- No ADR. This is a reversible wording/triage improvement and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Rerun the real scan and select large cache rows such as `DXCache`, `pip`, and broad AppData containers to see whether the guidance is specific enough.
