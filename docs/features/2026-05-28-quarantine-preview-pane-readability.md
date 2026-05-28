# Feature: Quarantine Preview Pane Readability

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make the WPF Quarantine Preview pane distinguish confirmation-readiness blockers from row-level preview details.

## Non-goals

- Do not change Quarantine Preview eligibility rules.
- Do not add Quarantine execution, Undo Quarantine execution, permanent deletion, or manifest writing.
- Do not scan real user files.

## User story / job story

As the project owner, I want the preview pane to separate readiness blockers from blocked row reasons, so that I can understand why a preview is not ready without confusing that with row-level evidence.

## Desired behavior

- The pane labels confirmation-level blockers as `Confirmation readiness blockers`.
- Individual confirmation blockers use `Confirmation blocker`.
- Row details appear under `Preview rows:`.
- Row details use `Preview row | Included`, `Preview row | Blocked`, or `Preview row | Redundant`.
- The pane still states that no files were modified.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed after rebuilding.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Updated WPF Quarantine Preview pane text formatting.
- Added WPF smoke assertions for included-row and blocked-row preview details.
- Kept the preview read-only; no cleanup execution, manifest writing, file moves, or real-profile automation were added.

ADRs added or skipped:

- No ADR. This is reversible UI wording and smoke coverage.
