# Windows File Cleaner

Windows File Cleaner is a local Windows-only WPF desktop app for reviewing storage under `C:\Users\moxhe`.

The current MVP centers on read-only Storage Scan plus tightly gated reversible Quarantine and selected restore paths. Current state lives in `docs/codex/current-state.md`; command details live in `docs/operations/`.

## Daily Local Use

Start with terminal-only readiness and accepted package print commands:

```powershell
.\tools\Invoke-DailyLocalReadiness.cmd
.\tools\Start-AcceptedLocalRelease.cmd -PrintOnly
.\tools\Start-AcceptedLocalRelease.cmd -Fixture -PrintOnly
```

Remove `-PrintOnly` only when you intentionally want to launch the accepted package. These commands do not install shortcuts or approve cleanup. Daily readiness also shows the latest package acceptance notes with notes/current-HEAD and package/current-HEAD context; incomplete candidate notes do not replace the accepted baseline.

Before any future tiny exact real-profile Quarantine review, use the terminal-only evidence wrapper and then stop for human WPF review:

```powershell
.\tools\Invoke-RealProfileNextBatchReview.cmd
```

Current stop state: after the 2026-06-04 second tiny exact-profile Quarantine batch, exact-profile displayed undo work is expected to be `1`. Do not treat another next-batch review as movement evidence while that undo work is present unless a new Grill with Docs pass decides the outstanding selected-manifest undo work is acceptable.

For read-only Restore Manifest evidence:

```powershell
.\tools\Summarize-RestoreManifests.cmd
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -ShowEntries
```

## Safety Status

- Storage Scan is read-only and does not modify scanned files.
- Review Shortlist is review context, not cleanup approval.
- Quarantine Preview is a dry run until exact confirmation and readiness gates pass.
- Fixture Quarantine, current-fixture undo, and fixture selected restore are available for synthetic fixture scopes.
- Exact real-profile selected restore is available only for one selected `C:\Users\moxhe` Restore Manifest after selected readiness, exact `RESTORE`, and immediate revalidation.
- First-phase exact real-profile Quarantine is available only for exact `C:\Users\moxhe` after ADR 0017/0018 gates, exact `QUARANTINE`, approval evidence, selected restore trust, immediate revalidation, and explicit human approval for the specific tiny batch.
- Codex and automated checks must not click real-profile movement.
- Broad/all-manifest real-profile Undo Quarantine, custom/non-exact real-profile Quarantine, custom selected restore, permanent deletion, persisted cleanup history, installed shortcuts, and installer behavior remain unavailable.

Use `docs/codex/safety-profiles.md` for reusable safety profiles.

## Requirements

- Windows
- .NET SDK 8.0.x

The SDK is pinned by `global.json`.

## Verify

```powershell
dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config
dotnet build WindowsFileCleaner.sln
dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj
dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj
.\tools\Invoke-MvpPreflight.cmd
```

Run `.\tools\Invoke-MvpPreflight.cmd` before any real-profile scan after code or workflow changes. It also runs the fixture root path guard, local release acceptance, Restore Manifest stop-state, daily readiness, and documentation consistency regressions, including active feature-index entry coverage, so those safety and handoff outputs stay covered by the normal preflight path.

## Run The App

```powershell
dotnet run --project src\WindowsFileCleaner.App
dotnet run --project src\WindowsFileCleaner.App -- --scope "D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture"
```

Default real Cleanup Scope:

```txt
C:\Users\moxhe
```

When opened against the default real-profile Cleanup Scope, `Scan` stays disabled until the preflight and fixture-review acknowledgement is checked.

## Portable v1

Accepted portable package baseline:

- Package: `.local\releases\windows-file-cleaner-v20260602-011556`
- Commit: `bc9b869`
- Completed ignored acceptance notes: `.local\release-acceptance\release-acceptance-20260602-011743.md`

Verified package candidate pending human acceptance:

- Package: `.local\releases\windows-file-cleaner-v20260604-121922`
- Commit: `e6ac3eb`
- Current pending acceptance notes: `.local\release-acceptance\release-acceptance-20260604-164509.md`

Keep using the accepted baseline until the candidate has a completed human acceptance pass.

## Operations

- CI runbook: `docs/operations/ci.md`
- Daily use: `docs/operations/daily-use.md`
- Portable release: `docs/operations/portable-release.md`
- Manual fixture review: `docs/operations/manual-fixture-review.md`
- Restore Manifest review: `docs/operations/restore-manifest-review.md`
- Full previous README reference: `docs/operations/readme-full-reference.md`

## Project Context

- Current state: `docs/codex/current-state.md`
- Progress: `.codex/progress.md`
- Domain context: `docs/domain/context.md`
- Glossary: `docs/domain/glossary.md`
- Detailed domain reference: `docs/domain/context-reference.md`
- Detailed glossary reference: `docs/domain/glossary-reference.md`
- Active feature index: `docs/features/index.md`
- ADRs: `docs/decisions/`

## Not Implemented

- Custom/non-exact real-profile WPF Quarantine execution.
- Broad/all-manifest real-profile WPF Undo Quarantine.
- Permanent deletion.
- Persisted cleanup history.
- Installed shortcut or installer automation.
