# New Feature Prompt for Codex

Paste this when starting a non-trivial feature.

```md
Use the Grill with Docs workflow before implementing this.

Read:

- `AGENTS.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/codex/grill-with-docs.md`
- Relevant ADRs in `docs/decisions/`
- Relevant feature briefs in `docs/features/`
- `.codex/progress.md`

Feature request:

[Describe the feature here.]

Before coding:

1. Restate the feature using project domain language.
2. Identify new, fuzzy, overloaded, or conflicting terms.
3. Ask me the highest-impact clarifying questions.
4. Update domain docs if needed.
5. Create a feature brief in `docs/features/`.
6. Tell me the implementation plan.

Only start implementation after the domain language and plan are clear enough.
```

## Shorter version for small changes

```md
Use the repo's `AGENTS.md`.

For this change, check whether any domain language, glossary terms, feature briefs, ADRs, or progress-log entries need to be updated. If not, say so briefly and implement the change.

Change request:

[Describe the change here.]
```

