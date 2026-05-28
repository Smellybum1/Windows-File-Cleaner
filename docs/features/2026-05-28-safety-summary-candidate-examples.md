# Feature: Safety Summary Candidate Examples

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Show a bounded set of the largest Quarantine candidate examples in the Storage Scan Safety Summary so the user can see likely cleanup opportunities without leaving the summary area.

## Non-goals

- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, manifest writing, or scan history.
- Do not treat Quarantine candidate examples as cleanup approval.
- Do not change scanner traversal, classification, recommendation rules, Review Shortlist, or Quarantine Preview eligibility.
- Do not scan `C:\Users\moxhe` from tests or automation.

## User story / job story

As the project owner, I want the Safety Summary to show a few concrete Quarantine candidate examples, so that I can quickly see where the largest likely cleanup opportunities are before using filters, search, Review Shortlist, or Quarantine Preview.

## Desired behavior

- Safety Summary shows `Candidate examples:` when Quarantine candidates exist.
- Candidate examples are limited to three rows.
- Candidate examples are sorted largest-first.
- Candidate examples use cleanup-scope-relative paths and include the row size.
- Candidate examples remain read-only and do not change Review Shortlist or preview state.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Storage Scan Safety Summary | Expanded to include bounded Quarantine candidate examples. | yes |

## Decisions made

- Use the largest Quarantine candidate rows as examples because they help triage high-storage opportunities first.
- Keep examples as plain summary text instead of a new control; Safety Summary shortcuts already provide the action to review candidates.
- Do not deduplicate parent/child examples in this packet because flattened recursive row overlap is already explained by Storage Review Size Note and Quarantine Preview blockers.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `StorageScanSafetySummary.QuarantineCandidateExamples`.
- Added largest-first bounded example generation in `StorageScanSafetySummaryBuilder`.
- Added WPF summary text for `Candidate examples:`.
- Added core coverage for largest-first relative path/size examples.
- Added WPF fixture coverage for the summary text.

ADRs added or skipped:

- No ADR. This is reversible read-only summary context and does not change architecture, persistence, security, deployment, or cleanup behavior.

Follow-up work:

- In the next real scan, confirm the candidate examples point to useful rows without making the summary feel too dense.
