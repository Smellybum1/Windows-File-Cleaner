# Architectural Decision Records

This folder contains ADRs: Architectural Decision Records.

Use ADRs for decisions that are:

- Hard to reverse
- Surprising without context
- The result of a real trade-off
- Likely to affect future implementation

Do not use ADRs for small choices that are easy to change later.

## File naming

Use this format:

```txt
0001-short-decision-title.md
0002-another-decision.md
0003-payment-provider-choice.md
```

## ADR status values

Use one of:

- `proposed`
- `accepted`
- `superseded`
- `rejected`

## ADR threshold

Create an ADR only when at least two of these are true:

- The decision affects architecture, persistence, security, auth, pricing, data model, public API, deployment, or core UX flow.
- A reasonable engineer would ask, "Why did we do it this way?"
- Reversing it later would require a migration, large refactor, user-facing change, or data cleanup.
- There were at least two plausible options.
- The chosen approach creates a known downside.

## Template

Use:

```txt
docs/codex/adr-template.md
```

