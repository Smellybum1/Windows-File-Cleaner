# Context Map

---
last_reviewed: 2026-05-28
owner: project-owner
stability: draft
---

## Purpose

Use this file when the project has more than one bounded context.

A bounded context is an area of the application where a specific shared language applies. If the whole app currently uses one shared language, keep this file minimal.

## Current bounded contexts

| Context | Purpose | Primary docs | Notes |
|---|---|---|---|
| Core Product | Main product experience. | `docs/domain/context.md` | Default context. |

## Context boundaries

Describe where terms mean different things in different parts of the app.

For now:

- No separate bounded contexts have been identified.
- Use `docs/domain/context.md` and `docs/domain/glossary.md` as the default shared language.

## Integration points

Describe where contexts interact.

For now:

- No cross-context integrations have been identified.

## When to add a new context

Add a new context when:

- The same term means different things in different areas.
- Different user groups use different language for the same workflow.
- A subsystem has its own domain model.
- The repo grows into multiple apps, packages, or services.
- A single `context.md` becomes too large or confusing.

## Context template

```md
### Context name

Purpose:

Primary users/domain experts:

Key terms:

Terms that conflict with other contexts:

Primary code locations:

Primary docs:
```

