# Feature: Quarantine Preview Relative Blocker Examples

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make Quarantine Preview protected-descendant blockers easier to read by showing descendant examples relative to the Cleanup Scope instead of repeating the full absolute profile path.

## Non-goals

- Do not change Quarantine Preview eligibility rules.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, manifest writing, or scan history.
- Do not change scanner traversal, classifications, recommendations, Review Shortlist behavior, or preview counts.
- Do not scan `C:\Users\moxhe` from tests or automation.

## User story / job story

As the project owner, I want a blocked parent row to explain protected descendants with compact paths like `.cache\codex-runtimes`, so that I can narrow review without reading long repeated Cleanup Scope prefixes.

## Desired behavior

- Broad parent blockers still trigger when a shortlisted row contains protected, high-risk, inaccessible, reparse-point, or Cleanup Scope Root descendants.
- The blocker reason uses cleanup-scope-relative descendant examples when possible.
- The WPF preview pane may still show absolute source/destination paths for row identity, but blocker evidence should not repeat the absolute Cleanup Scope.
- Preview remains read-only and does not create folders, write manifests, move files, delete files, or approve cleanup.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Preview | Clarified that protected-descendant blocker examples use cleanup-scope-relative paths. | yes |

## Decisions made

- Keep absolute source paths in row details for precise identity.
- Use relative paths only inside the protected-descendant blocker reason.
- Reuse existing `PathSafety` scope checks rather than adding another path abstraction.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Updated `QuarantinePreviewBuilder` to format protected-descendant examples relative to the Cleanup Scope.
- Added core coverage that blocked parent reasons include `.cache\codex-runtimes` and omit the absolute fixture root from blocker evidence.
- Added WPF coverage for relative blocker evidence in the preview pane.

ADRs added or skipped:

- No ADR. This is a reversible read-only wording/reporting refinement and does not change architecture, persistence, security, deployment, or cleanup behavior.

Follow-up work:

- In the next real scan, confirm broad-parent blockers are understandable with relative descendant examples.
