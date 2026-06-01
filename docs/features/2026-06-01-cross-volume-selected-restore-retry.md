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

After the cross-volume selected restore retry and gate-highlight fix was pushed, the user retried the same selected manifest and reported success from the highlighted strip:

- `Key status: selected restore succeeded. Restored 1, failed 0. Rediscover manifests and rescan.`

After the Quarantine-tab Restore Manifest Review panel made rediscovery easier to find, the user completed the follow-up rediscovery/rescan check and reported all three manual checks succeeded:

- Rediscovery showed the selected manifest as restored/already restored.
- The rescan completed normally.
- The restored `pip\cache\http\b\c` path appeared again.

## Implementation

- `UndoQuarantineExecutor` now restores directories through the guarded directory move fallback instead of direct `Directory.Move`.
- The fallback can copy a directory to the original path and then delete the quarantined directory when source and destination are on different volumes.
- A selected restore entry in `RestoreFailed` state can be retried when the quarantine path still exists, the original path is absent, and the selected manifest remains otherwise safe; all-manifest readiness still treats the same entry as recovery-review context.
- Quarantine and Selected Restore panes now show a highlighted key-status strip above the detailed gate text so the main blocker, ready state, or result is visible without reading the full audit output.
- Follow-up polish after user review: the selected-restore highlighted ready strip now explicitly includes `Can execute: yes`, exact `RESTORE` matched, and real-profile `Can proceed: yes` when revalidation evidence exists.

## Verification

- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj`
- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check` passed with only existing LF-to-CRLF working-copy warnings.
- User-reported manual rediscovery/rescan confirmation after selected restore recovery: manifest restored/already restored, rescan completed normally, restored `pip\cache\http\b\c` path visible again.

## Follow-up Work

- Rediscovery and rescan for the first-live selected restore recovery are complete by user report.
- Any further real-profile cleanup review should still start with fresh preflight and a new tiny exact selected batch.
- Keep recovery selected-manifest-only. Do not add broad Undo Quarantine or deletion in this packet.

## ADRs

No ADR added. ADR 0019 still governs selected-manifest exact real-profile restore; this packet fixes cross-volume directory restore and allows retrying the same selected manifest after a failed restore attempt when the quarantine path still exists.

## Risks And Assumptions

- Copy-then-delete restore is not a single-volume rename. If the final delete of the quarantined directory fails after copy placement, recovery review is still required.
- Retryability is intentionally narrow: it applies to selected restore rows already marked `RestoreFailed` only when the quarantined path is still present and the original path is still clear.
