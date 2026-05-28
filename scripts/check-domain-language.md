# Domain Language Check

Use this checklist before merging or finishing a Codex task.

## Manual check

1. Open `docs/domain/glossary.md`.
2. Search the codebase for forbidden synonyms.
3. Search new files for vague names:
   - `thing`
   - `item`
   - `record`
   - `data`
   - `object`
   - `manager`
   - `helper`
   - `util`
4. Check whether new entities are documented in `docs/domain/context.md`.
5. Check whether new user-facing labels match glossary terms.
6. Check whether new database fields match glossary terms.
7. Check whether new API routes match glossary terms.
8. Check whether new test names match glossary terms.

## Suggested shell searches

From the repo root, adapt these commands as needed:

```bash
rg "\b(thing|item|record|data|object|manager|helper|util)\b" src app components lib server test tests
```

Search for each forbidden synonym listed in `docs/domain/glossary.md`.

## Optional future automation

A real script could:

- Read forbidden synonyms from `docs/domain/glossary.md`.
- Scan `src/`, `app/`, `components/`, `lib/`, `server/`, and tests.
- Fail if forbidden terms appear in code.
- Allow exceptions through an allowlist.
- Print the preferred replacement term.

## Allowlist idea

Some words may be acceptable in specific contexts.

Example:

```txt
# scripts/domain-language-allowlist.txt
src/lib/object-storage.ts: object is part of external service terminology
src/utils/date.ts: utils is existing folder convention
```

