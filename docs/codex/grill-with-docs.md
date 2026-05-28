# Grill with Docs Workflow

Use this workflow before implementing non-trivial features.

## Objective

Interview the user until the feature, terminology, domain rules, implementation constraints, and acceptance criteria are clear enough to build.

Do not rush to implementation. First create shared understanding.

## Step 1: Load existing project knowledge

Read:

- `AGENTS.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/domain/context-map.md`
- Relevant ADRs in `docs/decisions/`
- Relevant feature briefs in `docs/features/`
- `.codex/progress.md`

## Step 2: Restate the request using project language

Summarize the request using glossary terms.

Flag any terms that are:

- New
- Fuzzy
- Overloaded
- Inconsistent with existing docs
- Likely to affect code naming
- Likely to affect UI wording
- Likely to affect data modeling

## Step 3: Ask clarifying questions

Ask questions in batches.

Prioritize questions that affect:

- Domain model
- Entity relationships
- Cardinality
- Status/lifecycle rules
- Permissions
- Data persistence
- External integrations
- User-facing language
- Edge cases
- Deletion/archive behavior
- Error handling
- Testing
- Migration/backfill needs

Avoid asking low-impact questions too early.

## Step 4: Challenge language

For every important term, ask:

- Is this already named in the glossary?
- Is there a better domain term?
- Is this term user-facing or internal only?
- Does this term conflict with another concept?
- Would this term make sense as a filename, variable, database table, API route, and UI label?
- Are there forbidden synonyms we should avoid?

## Step 5: Use concrete scenarios

Clarify behavior through examples.

Ask questions like:

- What happens when this exists with zero related records?
- What happens when it has many related records?
- Can it be archived?
- Can it be deleted?
- Who can see it?
- Who can modify it?
- What is the default state?
- What state transitions are allowed?
- What should happen if the user cancels halfway through?
- What should happen when related data is deleted?
- What should happen when permissions change?

## Step 6: Update docs before code

Before implementation, update docs if needed:

- New stable domain term: `docs/domain/context.md`
- New preferred or forbidden term: `docs/domain/glossary.md`
- Multiple contexts or language boundaries: `docs/domain/context-map.md`
- Durable decision: new ADR in `docs/decisions/`
- Feature-specific plan: new or updated brief in `docs/features/`

## Step 7: Plan implementation

Create or update a feature brief using:

```txt
docs/codex/feature-brief-template.md
```

The brief must include:

- Goal
- Non-goals
- Domain language changes
- Open questions
- Implementation plan
- Files expected to change
- Test plan
- Risks and assumptions

## Step 8: Build

Implement only after the docs and plan are clear enough.

Use glossary terms in:

- Types
- Interfaces
- Components
- Functions
- Routes
- Database fields
- Tests
- UI copy

## Step 9: Reconcile

At the end:

- Update the feature brief completion notes.
- Update domain docs if implementation revealed better language.
- Add ADRs for decisions that became durable.
- Update `.codex/progress.md` for meaningful packets.
- List tests run.
- List unresolved questions.
- List follow-up work.
- List risky assumptions.

## Step 10: Improve the workflow from evidence

Use `docs/codex/skillopt-inspired-workflow.md` when work reveals a reusable improvement to the way Codex should operate in this repo.

Keep the change bounded:

- Prefer adding one specific rule over rewriting a whole file.
- Preserve instructions that worked.
- Record rejected ideas if they may come up again.
- Accept workflow changes only when they help with a real observed failure, repeated friction, or verification gap.

## Anti-patterns

Avoid:

- Coding before the core domain terms are clear.
- Inventing synonyms for existing glossary terms.
- Treating `context.md` as a dumping ground for temporary implementation details.
- Creating ADRs for trivial choices.
- Leaving feature briefs unfinished after implementation.
- Saying a task is complete without listing tests and assumptions.
- Rewriting workflow docs broadly after a single anecdote.

