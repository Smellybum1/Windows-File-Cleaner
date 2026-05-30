# AGENTS.md

## Purpose

This repo uses a compact "Grill with Docs" workflow: clarify risky product/domain choices, keep durable docs aligned, then build small verified packets. The app is C# / WPF / .NET 8.

## Read Before Work

Before non-trivial implementation, read:

- `docs/domain/context.md`
- `docs/domain/glossary.md`
- relevant ADRs in `docs/decisions/`
- relevant feature briefs in `docs/features/`
- `.codex/progress.md`
- `docs/codex/grill-with-docs.md`
- `docs/codex/skillopt-inspired-workflow.md`
- this file

Read `docs/domain/context-map.md` only if it exists and is relevant.

## Commands

```powershell
dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config
dotnet build WindowsFileCleaner.sln
dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj
dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj
.\tools\Invoke-MvpPreflight.cmd
.\tools\New-StorageScanSmokeFixture.cmd
.\tools\Start-MvpFixtureReview.cmd
.\tools\Start-MvpFixtureReview.cmd -ChecklistOnly
dotnet run --project src\WindowsFileCleaner.App
dotnet run --project src\WindowsFileCleaner.App -- --scope "D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture"
```

## Safety Rules

This is a local Windows cleanup app for reviewing storage under `C:\Users\moxhe`.

- Keep Storage Scan read-only unless the user explicitly asks for cleanup execution after a Grill with Docs pass.
- Run `.\tools\Invoke-MvpPreflight.cmd` before any real-profile scan after code or workflow changes.
- Do not move, delete, restore, or modify real-profile files unless the relevant gate, docs, tests, and user approval are in place.
- Prefer Quarantine on `D:` with Undo Quarantine before any permanent deletion.
- Do not implement permanent deletion as a first cleanup action.
- Treat Windows profile data, app settings, browser profiles, credentials, documents, photos, source code, game saves, cloud sync data, Codex/tooling files, and broad `AppData` parents as high-risk by default.
- Never classify something as removable purely because it is large, hidden, old, or under `AppData`.
- Use Windows-aware path APIs instead of string-only path logic where possible.
- Use `Cleanup Candidate`, not `junk`, unless the user explicitly chooses different user-facing language.
- Keep Review Shortlist as review context, not cleanup approval.
- Keep Quarantine Preview as a dry run until explicit execution gates are designed and tested.

## Workflow

- For non-trivial product, domain, persistence, security, cleanup, restore, or irreversible behavior: follow `docs/codex/grill-with-docs.md`.
- If new durable language appears, update `docs/domain/context.md` and/or `docs/domain/glossary.md` before spreading it through code or UI.
- Add or update feature briefs for non-trivial features in `docs/features/`.
- Add an ADR only for decisions that are durable, surprising, hard to reverse, or affect architecture, persistence, security, data model, deployment, or core UX flow. Use `docs/codex/adr-template.md`.
- Keep changes focused and prefer existing project patterns.
- Reconcile at the end: update docs/progress, record checks, note open questions and assumptions.

## Documentation Map

- `docs/domain/context.md`: stable product/domain concepts.
- `docs/domain/glossary.md`: preferred terms, forbidden synonyms, and naming rules.
- `docs/decisions/`: ADRs.
- `docs/features/`: feature briefs, plans, and completion notes.
- `docs/codex/`: reusable Codex workflow templates.
- `.codex/progress.md`: running evidence log, checks, known risks, and next recommended work.

## Naming Rules

- Use glossary terms in code, file names, tests, UI labels, and docs.
- Avoid vague names such as `thing`, `item`, `record`, `data`, `object`, `manager`, `helper`, and `util` unless clearly conventional in the surrounding code.
- If a better term emerges, update the glossary first, then rename consistently.

## Done Means

- Code/docs changed as needed.
- Relevant tests or checks run.
- Feature brief/progress updated when the packet is meaningful.
- ADR added or explicitly skipped when a durable decision was considered.
- Remaining assumptions, open questions, and follow-up work are recorded.

## Final Response

When finishing a task, summarize:

1. What changed
2. Files changed
3. Tests run
4. Docs updated
5. ADRs added or skipped
6. Open questions
7. Follow-up work
8. Risky assumptions
