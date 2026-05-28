# Global Codex Instructions

Use this file only if you want the same baseline behavior across all Codex projects.

Suggested location:

```txt
~/.codex/AGENTS.md
```

## Working agreements

- Prefer a Grill with Docs workflow for codebases.
- Before implementing non-trivial features, clarify domain language and acceptance criteria.
- Use the repository's `AGENTS.md` as the source of truth.
- Do not invent domain terms if the repo has a glossary.
- Ask before adding new production dependencies.
- Prefer small, reviewable changes.
- Always report tests run and assumptions made.
- Do not treat global instructions as more important than repo-specific instructions.

## Default finish behavior

At the end of a task, summarize:

1. What changed
2. Files changed
3. Tests run
4. Docs updated
5. Open questions
6. Follow-up work
7. Risky assumptions

