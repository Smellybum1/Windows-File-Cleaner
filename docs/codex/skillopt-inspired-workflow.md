# SkillOpt-Inspired Workflow Notes

This repo borrows a lightweight workflow lesson from SkillOpt: treat compact natural-language instructions as durable state that can improve over time, but only through evidence, bounded edits, and validation.

Sources:

- SkillOpt project page: https://microsoft.github.io/SkillOpt/
- SkillOpt arXiv abstract: https://arxiv.org/abs/2605.23904

## What we are adopting

SkillOpt optimizes a skill document by collecting rollout evidence, reflecting on successes and failures, making bounded add/delete/replace edits, and accepting changes only when validation improves.

For this project, that becomes a human-scale repo workflow:

1. Gather evidence from real tasks.
2. Reflect on what helped or failed.
3. Make the smallest useful docs edit.
4. Validate with the next relevant check or task.
5. Keep rejected ideas visible when they are likely to recur.

## What this does not mean

- Do not run an automated training loop for this repo.
- Do not rewrite large docs just because one task felt awkward.
- Do not promote every local observation into a permanent rule.
- Do not add process steps that make small work packets heavy.

## Evidence to capture

Use `.codex/progress.md` and feature briefs to record:

- User answers that clarified domain language.
- Code or docs inspected before implementation.
- Tests, builds, linting, or manual checks run.
- Repeated mistakes or ambiguous terms.
- Rejected terms, designs, and workflow ideas.
- Follow-up work that should influence the next packet.

## Bounded edit rule

When improving docs or workflow instructions:

- Edit the narrowest relevant file.
- Prefer one specific rule over a broad rewrite.
- Preserve instructions that are still working.
- Avoid duplicating the same rule in many places.
- Move reusable rules into `AGENTS.md` or `docs/codex/`.
- Keep feature-specific context in the feature brief.

## Validation gate

Accept a workflow or naming change when at least one is true:

- It resolves a real ambiguity found during Grill with Docs.
- It prevents a repeated mistake.
- It improves verification or handoff quality.
- It records a durable decision that would otherwise be rediscovered.
- It reduces future context needed for a similar task.

Defer or reject the change when:

- It is speculative.
- It adds process without reducing risk.
- It conflicts with existing project language.
- It belongs in a feature brief rather than durable project docs.

## Rejected ideas buffer

Record rejected ideas when they are likely to resurface.

Use this shape:

```md
Rejected idea:

- Idea:
- Rejected because:
- Revisit if:
```

## Slow update habit

If a workflow improvement is plausible but not yet proven, leave it in `.codex/progress.md` as a candidate. Promote it into `AGENTS.md` or `docs/codex/` only after another task confirms it is useful.

## Exportable skill habit

Keep permanent instructions compact enough that a new Codex session can read them quickly.

If a doc grows too large:

- Move stable rules into a shorter summary.
- Move examples and evidence into feature briefs or `.codex/progress.md`.
- Keep `AGENTS.md` focused on behavior that should apply every session.

