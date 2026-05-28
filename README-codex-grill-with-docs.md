# Codex Grill with Docs Template

Use this package as a starter scaffold for Codex-heavy projects.

The goal is to give Codex a persistent project memory made of five layers:

1. `AGENTS.md`: how Codex should behave in this repo.
2. `docs/domain/`: shared domain language, glossary, and context boundaries.
3. `docs/decisions/`: ADRs for non-obvious, durable decisions.
4. `docs/features/`: feature-specific planning, grilling notes, implementation plans, and completion notes.
5. `.codex/progress.md`: running evidence log, rejected ideas, checks, and next recommended work.

## How to install

Copy these files into the root of your project.

If your project already has an `AGENTS.md`, do not overwrite it blindly. Merge the instructions from this template into your existing file.

Recommended folder structure after installation:

```txt
.
|-- AGENTS.md
|-- MANIFEST.md
|-- README-codex-grill-with-docs.md
|-- .codex/
|   `-- progress.md
|-- docs/
|   |-- domain/
|   |   |-- context.md
|   |   |-- glossary.md
|   |   `-- context-map.md
|   |-- decisions/
|   |   |-- README.md
|   |   `-- 0001-use-grill-with-docs-workflow.md
|   |-- features/
|   |   `-- README.md
|   `-- codex/
|       |-- grill-with-docs.md
|       |-- skillopt-inspired-workflow.md
|       |-- feature-brief-template.md
|       |-- adr-template.md
|       |-- end-of-task-checklist.md
|       |-- new-feature-prompt.md
|       `-- finish-task-prompt.md
|-- scripts/
|   `-- check-domain-language.md
`-- optional/
    `-- global-codex-AGENTS.md
```

## First thing to customize

Edit these files first:

1. `docs/domain/context.md`
   - Replace the product summary.
   - Add your first real domain concepts.

2. `docs/domain/glossary.md`
   - Replace the example terms.
   - Add your preferred terms and forbidden synonyms.

3. `AGENTS.md`
   - Add your actual build, test, lint, and run commands.
   - Add your project-specific conventions.

## Basic usage

When starting a non-trivial feature, paste the prompt from:

```txt
docs/codex/new-feature-prompt.md
```

When Codex says the work is complete, paste the prompt from:

```txt
docs/codex/finish-task-prompt.md
```

## Rule of thumb

```txt
If it changes the product language:
  update docs/domain/context.md or docs/domain/glossary.md

If it changes how a feature should be built:
  update docs/features/YYYY-MM-DD-feature-name.md

If it explains why the project chose one durable approach over another:
  add docs/decisions/NNNN-decision-name.md

If it tells Codex how to behave every session:
  update AGENTS.md or docs/codex/

If it captures evidence from completed work:
  update .codex/progress.md
```

