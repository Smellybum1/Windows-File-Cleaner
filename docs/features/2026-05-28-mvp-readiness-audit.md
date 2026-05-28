# Feature: MVP Readiness Audit

Date started: 2026-05-28  
Status: completed  
Owner: project-owner

## Goal

Audit the current Windows File Cleaner MVP against the original agreed requirements and make the remaining verification gap explicit.

This audit is evidence, not a declaration that cleanup execution is ready.

## Non-goals

- Do not add production code.
- Do not add Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not claim the app is safe to remove files.
- Do not scan or modify real user files from this documentation packet.

## User story / job story

As the project owner, I want a clear readiness audit for the current MVP, so that I can decide what needs manual retesting before moving toward any cleanup execution workflow.

## Current behavior

The repository contains a Windows-only WPF desktop app with a read-only Storage Scan workflow. The app scans the selected Cleanup Scope, classifies rows, supports review filters, shows safety/readiness summaries, supports Review Shortlist, and creates Quarantine Preview / Restore Manifest Draft / Quarantine Confirmation Draft artifacts in memory.

The user previously tested the app against `C:\Users\moxhe` and provided a screenshot showing:

- Total size: 58.02 GB.
- Folders: 37,740.
- Files: 188,580.
- Access issues: 3.
- Status: Storage Scan completed and no files were modified.

Several review and readiness features were added after that first real scan, so the latest WPF UI still needs a manual retest.

## Desired behavior

The repo should have a compact evidence table that separates:

- Requirements verified in the current repo.
- Requirements verified by fixture tests.
- Requirements supported by earlier user/manual evidence.
- Requirements still pending manual WPF retest.
- Work explicitly outside the MVP safety boundary.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Storage Scan | No change. | not needed |
| Cleanup Scope | No change. | not needed |
| Quarantine Preview | No change. | not needed |
| Restore Manifest Draft | No change. | not needed |
| Quarantine Confirmation Draft | No change. | not needed |

## Evidence and validation gate

Evidence gathered:

- User answers established the app goal, Cleanup Scope `C:\Users\moxhe`, recursive scan requirement, rating labels, workflow name, and desktop app preference.
- User screenshot verified an earlier real Storage Scan completed against `C:\Users\moxhe`.
- Existing code/docs inspected: `AGENTS.md`, `README.md`, `docs/domain/context.md`, `docs/domain/glossary.md`, ADRs, feature briefs, `.codex/progress.md`, WPF project, core project, and test harness.
- SkillOpt-inspired workflow evidence already exists in `docs/codex/skillopt-inspired-workflow.md` and ADR 0001.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Lifecycle, permission, and persistence boundaries are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not treat this audit as approval to add cleanup execution.
- Do not mark the goal fully complete while the latest WPF UI remains manually untested.
- Do not blur Quarantine Preview with a real Quarantine Cleanup Action.

## Requirements audit

| Requirement | Evidence inspected | Status | Notes |
|---|---|---|---|
| Windows-only desktop app | `src/WindowsFileCleaner.App/WindowsFileCleaner.App.csproj`, `src/WindowsFileCleaner.App/MainWindow.xaml`, `tests/WindowsFileCleaner.App.Tests`, ADR 0002 | Verified in current repo | WPF is the selected UI stack. Windows-only smoke tests construct the shell on an STA thread, check wrapping review toolbar structure, run a fixture scan through the shell, and exercise read-only review interactions. |
| .NET 8 target | `src/WindowsFileCleaner.App/WindowsFileCleaner.App.csproj`, `src/WindowsFileCleaner.Core/WindowsFileCleaner.Core.csproj`, `tests/WindowsFileCleaner.Tests/WindowsFileCleaner.Tests.csproj` | Verified in current repo | App targets `net8.0-windows`; core and tests target `net8.0`. |
| Read-only Storage Scan | `src/WindowsFileCleaner.Core/StorageScanner.cs`, `src/WindowsFileCleaner.App/MainWindow.xaml.cs`, read-only safety regression test | Verified in current repo | Status text and safety summaries repeatedly state no files were modified. |
| Default Cleanup Scope `C:\Users\moxhe` | `src/WindowsFileCleaner.Core/StorageScanOptions.cs`, `src/WindowsFileCleaner.App/MainWindow.xaml`, README | Verified in current repo | The UI defaults to the agreed Cleanup Scope. |
| Recursive scanning | `src/WindowsFileCleaner.Core/StorageScanner.cs`, fixture tests | Verified in current repo | Scanner walks accessible descendants and records access issues instead of crashing. |
| Cleanup candidate categories | `BloatCategory` usage in core, WPF labels, CSV exporters, docs | Verified in current repo | Categories include caches, package caches, app data, protected locations, access issues, and conservative app/game labels. |
| Importance ratings | `ImportanceRating`, WPF grid labels, fixture tests, glossary | Verified in current repo | User-facing labels are `Likely safe`, `Caution`, and `High risk`. |
| Deletion recommendations | `DeletionRecommendation`, classifier rules, WPF grid labels, glossary | Verified in current repo | Recommendations remain conservative: `Keep`, `Inspect`, or `Quarantine candidate`. |
| Fixture-based verification before real scan | `tests/WindowsFileCleaner.Tests/Program.cs`, `tests/WindowsFileCleaner.App.Tests/Program.cs`, WPF fixture smoke launch docs, progress log | Verified in current repo | Test harnesses cover scanner, classifier, summaries, preview, drafts, CSV, safety guard behavior, WPF shell startup state, WPF fixture scan state, and WPF review interaction state. The WPF app can also launch with a synthetic Cleanup Scope via `--scope`. |
| No cleanup execution in MVP | `ProductionCodeDoesNotContainCleanupExecutionCalls`, README Safety Status | Verified in current repo | Source-level guard blocks obvious production move/delete/write execution APIs except user-selected CSV report writes. |
| Quarantine on `D:` with undo path explored safely | Quarantine Preview, Restore Manifest Draft, Quarantine Confirmation Draft, ADR 0003 | Preview-only verified | The app proves destination/readiness shape without writing manifests or moving files. |
| Docs kept current | README, domain docs, ADRs, feature briefs, `.codex/progress.md` | Verified in current repo | Docs use the Grill with Docs control layer requested for the project. |
| Commits and remote push | Git history and remote `origin/main` | Verified at packet start | Branch was clean and tracking `origin/main` before this audit packet. |
| Latest WPF UI retest | User screenshot, WPF shell, fixture scan, review interaction, and toolbar layout smoke tests, README manual checklist | Partially automated, pending manual verification | Shell startup, launch-scope wiring, wrapping review toolbar structure, fixture scan state, filters, safety shortcuts, shortlist, and Quarantine Preview state are automated. The first scan was manually verified, but visible layout quality, wording, export dialogs, and real-profile behavior still need a fresh visible app run. Use the fixture smoke path first, then retest `C:\Users\moxhe`. |
| Actual Quarantine execution | README Not Implemented Yet, domain rules, ADR 0003 | Out of MVP | Requires explicit future design, confirmation semantics, restore rules, tests, and ADR review. |
| Undo Quarantine execution | README Not Implemented Yet, Restore Manifest docs | Out of MVP | Requires executed manifests and file-moving workflow first. |

## Automated evidence commands

Completed in this packet:

```powershell
dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config
dotnet build WindowsFileCleaner.sln --no-restore
dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build
dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build
git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check
```

Results:

- `dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config` passed with escalation because sandboxed restore could not read the user's NuGet config.
- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed with `All WindowsFileCleaner.Tests checks passed.`
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed with `All WindowsFileCleaner.App.Tests checks passed.` The app smoke tests now cover WPF shell construction, wrapping review toolbar structure, fixture scan state, review filters, safety shortcuts, Review Shortlist, and Quarantine Preview state.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\New-StorageScanSmokeFixture.ps1 -WhatIf` passed and listed the synthetic fixture writes without creating files.
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check` passed. Git reported line-ending normalization warnings for existing tracked Markdown files, but no whitespace errors.

## Manual retest checklist

Use `README.md` as the current manual MVP checklist. The highest-value retest is:

1. Run `.\tools\New-StorageScanSmokeFixture.ps1`.
2. Run the printed `dotnet run --project src\WindowsFileCleaner.App -- --scope "<fixture path>"` command.
3. Smoke test the WPF controls against the fixture.
4. Run the app with `dotnet run --project src\WindowsFileCleaner.App`.
5. Scan `C:\Users\moxhe`.
6. Confirm the status still says no files were modified.
7. Test Safety Summary shortcuts.
8. Test Access issues, Bloat Category, and No category filters.
9. Add one likely-safe row to Review Shortlist.
10. Create a Quarantine Preview.
11. Confirm Restore Manifest Draft and Quarantine Confirmation Draft wording is understandable.
12. Export the Quarantine Preview CSV only to a user-selected report path.

## Decisions made

Small feature-level decisions:

- Keep this as a feature-level audit because it evaluates the MVP requirement set.
- Do not update domain language because no new terms were introduced.
- Keep manual WPF retest pending instead of pretending code-level checks cover UI usability.

ADR-worthy decisions:

- [x] None

## Files expected to change

Expected:

- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `.codex/progress.md`
- `README.md`

Possible:

- None

## Test plan

Manual checks:

- Real WPF retest remains pending for the latest UI.

Automated tests:

- Build the solution.
- Run the console fixture test harness.
- Run Markdown/whitespace diff check with `git diff --check`.

## Risks and assumptions

Risks:

- The latest WPF UI may have usability or layout issues that code-level checks cannot detect.
- The real user profile can change between scans, so totals may differ from the original screenshot.

Assumptions:

- The user-provided screenshot is valid evidence for the earlier real Storage Scan.
- `C:\Users\moxhe` remains the intended Cleanup Scope for this MVP.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added an MVP readiness audit that maps the original requirements to repo evidence, fixture coverage, user/manual evidence, pending WPF retest work, and explicit out-of-MVP cleanup execution work.
- Linked the audit from the root README.
- Updated the progress log current status and next recommended work.

Files changed:

- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `README.md`
- `.codex/progress.md`

Tests run:

- `dotnet build WindowsFileCleaner.sln --no-restore`
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- `docs/features/2026-05-28-mvp-readiness-audit.md`
- `README.md`
- `.codex/progress.md`

ADRs added or skipped:

- No ADR added. This audit records evidence and does not introduce a durable architecture, persistence, security, deployment, or core UX decision.

Follow-up work:

- Use `README.md` and this audit to run a fresh WPF manual retest against `C:\Users\moxhe`.
- Only after that retest, decide whether to design actual Quarantine execution and Undo Quarantine.

Open questions:

- Should the next packet focus on WPF manual retest feedback, or start designing actual Quarantine execution after retest?

Risky assumptions:

- The user-provided screenshot remains valid evidence for the earlier real Storage Scan, even though the latest WPF UI has changed since then.
- `C:\Users\moxhe` remains the intended Cleanup Scope for the MVP.
