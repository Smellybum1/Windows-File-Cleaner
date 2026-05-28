# Feature Briefs

This folder contains feature-specific planning docs.

Use a feature brief for any non-trivial feature, refactor, migration, or product change.

Feature briefs are temporary-to-semi-durable working documents. They capture the plan, open questions, decisions made during grilling, implementation notes, and completion notes.

## File naming

Use this format:

```txt
YYYY-MM-DD-feature-name.md
```

Examples:

```txt
2026-05-28-add-user-invites.md
2026-05-28-refactor-billing-flow.md
2026-05-28-add-project-dashboard.md
```

## Template

Use:

```txt
docs/codex/feature-brief-template.md
```

## When to create a feature brief

Create a feature brief when:

- The work changes product behavior.
- The work introduces or changes domain language.
- The work affects persistence, permissions, billing, integrations, auth, or deployment.
- The work spans multiple files or modules.
- The work needs clarification before implementation.

Skip a feature brief when:

- The change is tiny and mechanical.
- The change does not affect domain language.
- The change is a one-line bug fix with obvious expected behavior.

## Completion notes

Every feature brief should be updated when the task is complete.

Completion notes should include:

- What changed
- Tests run
- Docs updated
- ADRs added or skipped
- Follow-up work
- Open questions
- Risky assumptions

