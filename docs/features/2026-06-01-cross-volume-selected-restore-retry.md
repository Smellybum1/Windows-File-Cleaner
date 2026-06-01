# Feature: Cross-Volume Selected Restore Retry and Gate Highlights

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Fix the user-reported selected real-profile restore failure where Windows rejected restoring a directory from the `D:` Quarantine Root back to `C:\Users\moxhe`, and make dense Quarantine / Selected Restore gate text easier to scan.

## Non-goals

- Do not run real-profile restore from Codex or automated tests.
- Do not enable broad or all-manifest real-profile Undo Quarantine.
- Do not enable custom or non-exact real-profile selected restore.
- Do not enable permanent deletion.
- Do not add persisted cleanup history.
- Do not clean up quarantine action folders.

## Evidence

The user attempted selected restore for the first live exact real-profile Quarantine manifest and reported failure:

- Selected restore result: `Restored 0 | Failed 1 | Recovery review: yes`.
- Source: `D:\WindowsFileCleanerQuarantine\actions\quarantine-action-draft-20260601112432-ab98a4a0\items\AppData\Local\pip\cache\http\b\c`.
- Destination: `C:\Users\moxhe\AppData\Local\pip\cache\http\b\c`.
- Error: Windows requires identical roots for directory moves across volumes.

The same output showed selected readiness and pre-execution revalidation were otherwise clean before movement.

## Implementation

- `UndoQuarantineExecutor` now restores directories through the guarded directory move fallback instead of direct `Directory.Move`.
- The fallback can copy a directory to the original path and then delete the quarantined directory when source and destination are on different volumes.
- A selected restore entry in `RestoreFailed` state can be retried when the quarantine path still exists, the original path is absent, and the selected manifest remains otherwise safe; all-manifest readiness still treats the same entry as recovery-review context.
- Quarantine and Selected Restore panes now show a highlighted key-status strip above the detailed gate text so the main blocker, ready state, or result is visible without reading the full audit output.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check` passed with only existing LF-to-CRLF working-copy warnings.

## Follow-up Work

- Close the current WPF app, run full `.cmd` MVP preflight, relaunch, rediscover the manifest, and retry selected restore only if the highlighted key status and detailed gate show the selected manifest is retryable/restorable.
- Keep recovery selected-manifest-only. Do not add broad Undo Quarantine or deletion in this packet.

## ADRs

No ADR added. ADR 0019 still governs selected-manifest exact real-profile restore; this packet fixes cross-volume directory restore and allows retrying the same selected manifest after a failed restore attempt when the quarantine path still exists.

## Risks And Assumptions

- Copy-then-delete restore is not a single-volume rename. If the final delete of the quarantined directory fails after copy placement, recovery review is still required.
- Retryability is intentionally narrow: it applies to selected restore rows already marked `RestoreFailed` only when the quarantined path is still present and the original path is still clear.
