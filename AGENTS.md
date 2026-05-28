# AGENTS.md

## Purpose

This repository uses a "Grill with Docs" workflow for Codex work.

Before implementing non-trivial features, Codex should clarify the domain, update durable project knowledge, then implement the code. The goal is to keep the codebase, documentation, product language, and user-facing language aligned.

## Required reading before work

Before starting implementation, read:

1. `docs/domain/context.md`
2. `docs/domain/glossary.md`
3. `docs/domain/context-map.md` if it exists and is relevant
4. Existing ADRs in `docs/decisions/`
5. Any relevant feature brief in `docs/features/`
6. `docs/codex/grill-with-docs.md`
7. `docs/codex/skillopt-inspired-workflow.md`
8. This `AGENTS.md`

## Project commands

The project stack is C# with WPF for a Windows-only desktop app. The initial target is `.NET 8` because the SDK and Windows Desktop runtime are already installed locally. Revisit `.NET 10` after installing that SDK if longer support horizon becomes more important than zero-setup local buildability.

```bash
# Install dependencies
# No package restore beyond the .NET SDK is expected for the initial app.
dotnet restore WindowsFileCleaner.sln --configfile NuGet.Config

# Run full MVP preflight before any real-profile scan
.\tools\Invoke-MvpPreflight.ps1

# Start local development
dotnet run --project src/WindowsFileCleaner.App

# Create a synthetic WPF smoke-test Cleanup Scope
.\tools\New-StorageScanSmokeFixture.ps1

# Start local development against the synthetic Cleanup Scope
dotnet run --project src/WindowsFileCleaner.App -- --scope "D:\Codex\Windows File Cleaner\.local\storage-scan-smoke-fixture"

# Run tests
dotnet run --project tests/WindowsFileCleaner.Tests/WindowsFileCleaner.Tests.csproj
dotnet run --project tests/WindowsFileCleaner.App.Tests/WindowsFileCleaner.App.Tests.csproj

# Run linting
# No separate lint command selected yet.

# Run type checks
dotnet build

# Build
dotnet build
```

## Project-specific safety rules

This project is a local Windows cleanup app for trimming unwanted bloat from `C:\Users` on a Windows 11 system drive.

Because the product may recommend or perform file removal:

- Default to read-only scanning, reporting, and recommendation workflows before any cleanup execution.
- Do not implement irreversible deletion as the first cleanup behavior unless the user explicitly asks for it after reviewing safer options.
- Prefer explicit user confirmation, clear path display, byte counts, and a dry run before any cleanup action.
- Prefer reversible cleanup actions such as moving to the Recycle Bin or a quarantine location when technically feasible.
- Treat Windows profile, application settings, browser profiles, credentials, documents, photos, source code, and game saves as high-risk until the user defines safer category rules.
- Never treat a file or folder as bloat purely because it is large, hidden, old, or located under `AppData`.
- Keep path handling Windows-aware. Avoid string-only path logic when a platform path API is available.
- Use "cleanup candidate" for something proposed for review, not "junk" unless the user explicitly chooses that user-facing language.
- The first workflow should be a read-only desktop-app scan/report before any cleanup executor.
- The app should help the user inspect folder contents and rate importance before recommending cleanup.
- The first preferred cleanup mechanism is quarantine on `D:` with an easy undo path, not permanent deletion.
- Do not recommend cleanup that is likely to break current apps, including Codex-related files, active development tooling, package caches that current tools depend on, or active app settings.

## Core workflow

Use this sequence for non-trivial work:

### 1. Grill

- Ask focused questions until the feature is clear enough to implement.
- Resolve ambiguity before coding.
- Walk through dependencies between decisions.
- Prefer concrete scenarios over abstract agreement.
- Do not ask low-impact questions before high-impact domain, data, permission, and lifecycle questions.

### 2. Define

- If new domain language is introduced, update `docs/domain/context.md` and/or `docs/domain/glossary.md` before coding.
- Challenge fuzzy or overloaded terms.
- Prefer the project glossary over inventing new names.
- Avoid synonyms that conflict with the glossary.
- Use the same language in code, file names, database fields, routes, tests, UI labels, and docs.

### 3. Decide

- Create an ADR in `docs/decisions/` when a decision is durable, surprising, costly to reverse, or involves a real trade-off.
- Do not create ADRs for trivial implementation details.
- Use `docs/codex/adr-template.md`.

### 4. Plan

- For each non-trivial feature, create or update a feature brief in `docs/features/`.
- Use `docs/codex/feature-brief-template.md`.
- The feature brief should include goal, non-goals, open questions, decisions, expected files, test plan, risks, and completion notes.

### 5. Build

- Implement using the agreed domain language.
- Match existing project structure and conventions.
- Keep changes focused and reviewable.
- Avoid broad refactors unless explicitly requested or required.
- Ask before adding new production dependencies.

### 6. Reconcile

At the end of the task:

- Update docs with what actually changed.
- Record unresolved questions.
- Record tests run.
- Record risky assumptions.
- Update feature brief completion notes.
- Add ADRs for durable decisions discovered during implementation.

## SkillOpt-inspired workflow discipline

This repo treats its durable docs as external agent state. Improve them through evidence from actual work instead of broad rewrites.

- Rollout evidence: record what happened, what failed, and what checks passed in `.codex/progress.md` and feature briefs.
- Bounded edits: update the smallest relevant doc sections that improve future work.
- Validation gates: only accept workflow or naming changes when they resolve real ambiguity, prevent repeated mistakes, or improve verification.
- Rejected ideas buffer: record discarded terms, designs, and assumptions when they are likely to come up again.
- Exportable rules: keep stable agent behavior in `AGENTS.md` or `docs/codex/`; keep feature-specific details in feature briefs.

See `docs/codex/skillopt-inspired-workflow.md` for the lightweight version adapted for this repo.

## Documentation rules

- `docs/domain/context.md` contains stable product/domain concepts.
- `docs/domain/glossary.md` contains preferred terms, forbidden synonyms, and naming rules.
- `docs/domain/context-map.md` explains multiple bounded contexts if this project grows beyond one shared language.
- `docs/decisions/` contains ADRs.
- `docs/features/` contains feature-specific briefs, plans, and completion notes.
- `docs/codex/` contains reusable Codex workflow templates.
- `.codex/progress.md` contains the running work log, evidence, rejected ideas, checks, and next recommended work.

## Naming rules

- Use glossary terms in code, filenames, database fields, API routes, UI labels, tests, and docs.
- Do not introduce near-synonyms unless the glossary explicitly allows them.
- When uncertain, ask before naming core entities.
- If a better term emerges, update the glossary first, then rename code consistently.
- Avoid vague names such as `thing`, `item`, `record`, `data`, `object`, `manager`, `helper`, and `util` unless the name is clearly conventional in the existing codebase.

## ADR threshold

Create an ADR only when at least two of these are true:

- The decision affects architecture, persistence, security, auth, pricing, data model, public API, deployment, or core UX flow.
- A reasonable engineer would ask, "Why did we do it this way?"
- Reversing it later would require a migration, large refactor, user-facing change, or data cleanup.
- There were at least two plausible options.
- The chosen approach creates a known downside.

## Done means

A task is not done until:

- Relevant code is implemented.
- Relevant tests are added or updated.
- Required checks have been run where possible.
- Domain docs are updated if language changed.
- ADRs are added if durable decisions were made.
- Feature brief completion notes are updated.
- Remaining assumptions or open questions are listed.

## Final response format

When finishing a task, summarize:

1. What changed
2. Files changed
3. Tests run
4. Docs updated
5. ADRs added or skipped
6. Open questions
7. Follow-up work
8. Risky assumptions
