# ADR 0001: Use Grill with Docs workflow

Date: 2026-05-28  
Status: accepted  
Owner: project-owner

## Context

This project will use Codex heavily for planning, implementation, refactoring, and review.

AI coding agents perform better when project language, domain rules, and non-obvious decisions are written down rather than rediscovered in every conversation.

The project needs a lightweight way to preserve shared language, feature decisions, and implementation context without creating heavyweight documentation overhead.

## Decision

We will use a Grill with Docs workflow.

Before non-trivial implementation, Codex should:

1. Ask clarifying questions.
2. Check existing domain docs.
3. Update shared language docs when needed.
4. Record durable decisions as ADRs.
5. Create or update a feature brief.
6. Implement the feature.
7. Reconcile docs and completion notes at the end.

We will also adopt a lightweight SkillOpt-inspired discipline for improving the workflow itself: collect evidence from real tasks, make bounded edits to the docs, keep rejected ideas visible, and gate workflow changes on demonstrated usefulness.

## Options considered

### Option A: Use ad hoc prompting only

Pros:

- Fast to start.
- No documentation overhead.

Cons:

- Context gets lost between sessions.
- Naming drifts.
- Codex may re-ask the same questions.
- Decisions are harder to audit later.
- Future work depends too much on conversation memory.

### Option B: Use Grill with Docs plus lightweight repo docs

Pros:

- Creates persistent project memory.
- Keeps naming consistent.
- Makes Codex sessions easier to start.
- Makes code easier to navigate.
- Makes durable decisions easier to understand later.

Cons:

- Requires small documentation updates during development.
- Poorly maintained docs could mislead Codex.

### Option C: Use a heavier process with formal specs for every change

Pros:

- Very explicit.
- High traceability.

Cons:

- Too much overhead for small packets.
- Slows early discovery.
- Encourages ceremony before the product language is known.

## Why this decision

The documentation overhead is small compared with the cost of repeated explanations, inconsistent naming, and undocumented architectural decisions.

This workflow should make Codex more useful over time because every non-trivial task improves the repo's shared memory.

## Consequences

Positive consequences:

- Codex has clearer project context.
- Domain terms are more consistent.
- Important decisions are easier to understand later.
- Feature work can start from existing context instead of rediscovering it.
- The workflow can improve from evidence without broad rewrites.

Negative consequences:

- Every non-trivial feature has a small docs-maintenance cost.
- Stale docs may create confusion if not reconciled after implementation.
- The team must keep evidence logs short enough to remain useful.

## Reversal cost

Low.

We can stop using the workflow and keep, archive, or delete the docs later. Existing docs may still be useful as historical project context.

## Follow-up work

- Keep `AGENTS.md` current.
- Keep `docs/domain/context.md` and `docs/domain/glossary.md` updated.
- Add ADRs only when they meet the ADR threshold.
- Use feature briefs for non-trivial work.
- Update `.codex/progress.md` after meaningful work packets.

