# Thread Handoff

Last updated: 2026-05-29

Use this when starting a fresh Codex thread for this repository.

## Current state

- Repo: `D:\Codex\Windows File Cleaner`
- GitHub: `Smellybum1/Windows-File-Cleaner`
- Branch: `main`
- Latest pushed packet: `ce7c1db Debounce storage review search`
- Current app stack: C# / WPF / .NET 8
- Desktop shortcut target: `D:\Codex\Windows File Cleaner\src\WindowsFileCleaner.App\bin\Debug\net8.0-windows\WindowsFileCleaner.App.exe`

## Product status

Windows File Cleaner is a local Windows-only desktop app for reviewing storage under `C:\Users\moxhe`.

The current MVP has a read-only Storage Scan that can inspect large real-profile scans with filters, debounced search, Review Mix, Matched Review Mix, selected-folder child/descendant focus, selected-folder summaries, hotspot trail, file preview, CSV export, Review Shortlist, Quarantine Preview, Restore Manifest Draft, Quarantine Confirmation Draft, Quarantine Action Draft, manifest discovery, selected manifest review, selected restore gate, and restore-readiness preview.

Fixture-only cleanup execution exists for synthetic Cleanup Scopes. Real-profile cleanup execution remains intentionally unavailable.

## Safety boundary

- Do not enable real-profile file movement without a new Grill with Docs pass, feature brief, tests, and ADR review if needed.
- Do not implement permanent deletion as the next step.
- Keep Storage Scan read-only.
- Keep Review Shortlist as review context, not cleanup approval.
- Keep Quarantine Preview as a dry run until explicit execution gates are designed and tested.
- Treat browser profiles, credentials, game saves, cloud sync data, source code, active app settings, Codex/tooling caches, and broad `AppData` parents conservatively.

## Verified recently

- User confirmed real-profile scan works.
- User confirmed the real-profile scan gate checkbox enabled `Scan` after acknowledgement.
- User reported Search typing was sluggish on large real-profile results.
- `ce7c1db` added debounced user-typed Storage Review Search.
- User retested and confirmed Search typing feels much better.
- User confirmed the status-bar pending-search message is enough; do not add another indicator unless future testing contradicts this.
- GitHub Actions MVP Preflight passed for `ce7c1db`: `https://github.com/Smellybum1/Windows-File-Cleaner/actions/runs/26616156540`

## Best next work

Prefer a small packet. Good candidates:

1. Manual fixture visual polish pass using `.\tools\Start-MvpFixtureReview.ps1`, then update docs with any findings.
2. Real-profile Quarantine Preview/readiness wording pass, still without enabling real-profile execution.
3. Add Review Shortlist Safety Mix if the user is about to shortlist real scan rows.
4. Improve the scan-gate discoverability if the checkbox remains easy to miss.
5. Start real-profile Quarantine execution design only after the user explicitly wants to move beyond preview and the safety/undo story is reviewed again.

## Commands

```powershell
git -c safe.directory='D:/Codex/Windows File Cleaner' status --short --branch
.\tools\Invoke-MvpPreflight.ps1
dotnet build WindowsFileCleaner.sln
dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj
dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj
dotnet run --project src\WindowsFileCleaner.App
.\tools\Start-MvpFixtureReview.ps1
```

## Startup prompt

```text
We are continuing Windows File Cleaner in D:\Codex\Windows File Cleaner.

Read AGENTS.md, .codex/progress.md, README.md, docs/codex/thread-handoff.md, docs/domain/context.md, docs/domain/glossary.md, and relevant docs/features/ and docs/decisions/ before implementing.

Current state: main is pushed through ce7c1db Debounce storage review search. The app is a C#/.NET 8 WPF local Windows cleanup reviewer for C:\Users\moxhe. Storage Scan is read-only. Fixture-only Quarantine execution and fixture-only selected restore exist, but real-profile Quarantine execution, real-profile Undo Quarantine, permanent deletion, and persisted cleanup history remain intentionally unavailable.

User verification: real-profile scan works; search typing was sluggish, then debounced search fixed it; the status-bar message is enough. Keep this as a local-first, safety-gated app. Do not move or delete real-profile files unless I explicitly ask after a Grill with Docs pass.

Please inspect current git status first, then choose the best next small packet. Prefer manual fixture/real-profile review polish, Quarantine Preview/readiness clarity, scan-gate discoverability, or Review Shortlist safety context before any real cleanup execution. Run the narrowest relevant checks, update docs, and commit/push at clean packet boundaries.
```
