# Feature: In-Use Source Pre-Execution Revalidation

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Turn the second exact real-profile folder Quarantine failure into earlier readiness evidence by detecting source files that are currently in use or inaccessible before movement starts, and expand the WPF Quarantine Execution Gate details area again.

## Non-goals

- Do not run or retry real-profile Quarantine from Codex.
- Do not force-close apps or unlock files.
- Do not delete or permanently remove anything.
- Do not enable custom or non-exact real-profile Quarantine.
- Do not add broad/all-manifest Undo Quarantine or persisted cleanup history.

## Current Behavior

- After the cross-volume directory fallback landed, the user retried the exact real-profile `DXCache` folder and reported another safe failure with `moved 0, failed 1`.
- The failure was now an active-file blocker: Windows could not access a descendant `.nvph` file because another process was using it.
- The app recorded failure evidence in the Restore Manifest, but it still reached execution before discovering the lock.

## Desired Behavior

- Pre-Execution Revalidation checks source files with a read-only exclusive-read probe before movement.
- For folder entries, Pre-Execution Revalidation checks descendant files and reports up to a bounded number of in-use or inaccessible descendants.
- In-use files block the exact real-profile gate before manifest write and movement.
- The WPF Quarantine Execution Gate details area is large enough to read the full readiness and failure evidence comfortably.

## Domain Language Changes

No new durable terms.

| Term | Change | Docs updated? |
|---|---|---|
| Pre-Execution Revalidation | Clarified that it can block in-use or inaccessible source files before movement. | yes |

## Grill Notes

### Scenarios Discussed

- The second failure showed a different blocker from the first: the cross-volume folder path was handled, but an active NVIDIA cache file could not be read/copied/deleted safely.
- The user also asked for the details text box to be bigger because the Quarantine output is dense.

### Edge Cases

- The access probe is conservative. It may block files that are open by another process even if a copy might have succeeded, because deletion or rename may still fail later.
- Files can become locked after revalidation and before movement; the executor still records failure evidence if that race happens.
- Some active caches may remain unsuitable for live Quarantine while their owning app or driver is running.

## Decisions Made

Small feature-level decisions:

- Use a read-only exclusive-read probe in Pre-Execution Revalidation rather than adding another execution retry path.
- Bound folder-descendant in-use blocker output to avoid flooding the gate.
- Increase the Quarantine Execution Gate scroll area from `260` to `520`.

ADR-worthy decisions:

- [x] No ADR added. ADR 0018 already requires immediate live-filesystem revalidation before exact real-profile movement; this packet strengthens that implementation.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check`

## Follow-up Work

- Run full `.cmd` MVP preflight before any user retry.
- If `DXCache` remains blocked as in-use, pick a different tiny reviewed cache row or retry after the owning process is closed/restarted by the user.
- Consider future UI guidance that recommends skipping active GPU shader cache folders while NVIDIA/graphics processes are running.

## Risks And Assumptions

Risks:

- Exclusive-read probing may be stricter than copy-only probing, but that matches the safer cleanup boundary because Quarantine must remove the original path too.

Assumptions:

- The reported `.nvph` file is active NVIDIA cache state and should not be forced while in use.
