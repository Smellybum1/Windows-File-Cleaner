# Finish Task Prompt for Codex

Paste this when Codex says a task is complete.

```md
Before we call this complete, run the end-of-task checklist in:

`docs/codex/end-of-task-checklist.md`

Then summarize:

1. What changed
2. Files changed
3. Tests run
4. Docs updated
5. ADRs added or skipped
6. Open questions
7. Follow-up work
8. Risky assumptions
```

## Stricter version

```md
Run the end-of-task checklist in `docs/codex/end-of-task-checklist.md`.

If any item is incomplete, say exactly which item is incomplete and why.

Then provide the final task summary with:

- What changed
- Files changed
- Tests run
- Docs updated
- ADRs added or skipped
- Open questions
- Follow-up work
- Risky assumptions
```

