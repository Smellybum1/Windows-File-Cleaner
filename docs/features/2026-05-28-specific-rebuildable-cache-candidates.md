# Feature: Specific Rebuildable Cache Candidates

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make large, specific cache rows from the real scan easier to act on by promoting strong rebuildable cache evidence to `Likely safe` / `Quarantine candidate`, while keeping broad parent folders inspection-first.

## Non-goals

- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not mark broad `AppData`, app, package, or profile containers as safe.
- Do not treat Review Shortlist, Quarantine Preview, or a recommendation as cleanup approval.
- Do not scan `C:\Users\moxhe` from tests or automation.

## User story / job story

As the project owner, I want rows like `DXCache` and `pip\Cache` to stand out as likely rebuildable cache candidates, so that I can focus review on storage-heavy bloat without shortlisting broad folders that could break apps or Codex.

## Desired behavior

- `DXCache` and other GPU shader cache rows are classified as `Likely safe` / `Quarantine candidate` when they are not protected by another rule.
- Package-cache rows with both package-cache and app-cache evidence, such as `pip\Cache`, are classified as `Likely safe` / `Quarantine candidate` when they are not protected by another rule.
- Broad parent rows such as `pip`, `NVIDIA`, generic `AppData`, browser profiles, installed apps, Windows app data, game data, source-code paths, and Codex-related paths remain conservative.
- Selected Path Review Guidance still uses cache-specific wording and routes candidates through Review Shortlist and Quarantine Preview.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Specific Rebuildable Cache Evidence | Added as the narrow evidence rule for cache rows that can become Quarantine candidates. | yes |
| Bloat Category | Clarified that cache categories can support different recommendations for specific child rows versus broad parents. | yes |
| Deletion Recommendation | Clarified that specific rebuildable cache rows may be Quarantine candidates without approving cleanup. | yes |

## Decisions made

- Promote specific rebuildable cache rows only when the classifier has strong cache evidence.
- Keep broad cache-adjacent parent folders as `Caution` / `Inspect`.
- Keep cache-specific guidance ahead of generic Quarantine candidate guidance so selected rows still show risk context.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `HasSpecificRebuildableCacheEvidence` to keep cache candidate promotion narrow.
- Updated classifier coverage for broad `pip`, specific `pip\Cache`, and `DXCache`.
- Updated selected-row guidance coverage so likely-safe cache rows still show cache-specific warnings and Review Shortlist wording.

ADRs added or skipped:

- No ADR. This is a reversible classification and review-guidance refinement; it does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Rerun the real scan and confirm `DXCache` and `pip\Cache` are easier to find under Quarantine candidates while their parent folders remain conservative.
