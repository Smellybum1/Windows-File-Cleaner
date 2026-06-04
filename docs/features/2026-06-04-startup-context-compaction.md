# Startup Context Compaction

Date: 2026-06-04

Status: completed

## Goal

Reduce Codex thread lag by shrinking the read-first documentation surfaces while preserving detailed historical/reference material.

## Safety Profile

`docs-only`. This packet does not launch WPF, click `Scan`, scan real-profile files, move, restore, delete, approve cleanup, write Restore Manifests, create shortcuts, install anything, or create cleanup history.

## Problem

The previous read-first set pulled in oversized docs before ordinary work:

- `docs/domain/context.md` had grown into a full product-history reference.
- `docs/domain/glossary.md` mixed preferred names with long UI behavior notes.
- `README.md` carried daily use, package details, restore workflows, and full manual review checklists.
- The active roadmap carried many completed packet notes.
- `.codex/progress.md` had begun accumulating completed packet evidence again.

ADRs were not the main issue. The lag came from always-read context shape and duplicated active evidence.

## Changes

- Preserved the old full domain context as `docs/domain/context-reference.md`.
- Preserved the old full glossary as `docs/domain/glossary-reference.md`.
- Preserved the old full README as `docs/operations/readme-full-reference.md`.
- Preserved the old full roadmap as `docs/features/archive/2026-06-01-live-product-readiness-roadmap-history.md`.
- Preserved the old progress log as `.codex/archive/progress-2026-06-04-pre-context-compaction.md`.
- Replaced `docs/domain/context.md`, `docs/domain/glossary.md`, `README.md`, the active roadmap, and `.codex/progress.md` with compact read-first versions.
- Tightened `AGENTS.md`, `docs/codex/grill-with-docs.md`, `docs/features/index.md`, and handoff/current-state docs so reference/archive docs are opened only when relevant.

## Verification

- Read-first size scan before compaction: 11 files, 337,831 chars, about 84,458 approximate tokens.
- Read-first size scan after compaction: 11 files, 55,911 chars, about 13,978 approximate tokens.
- Active feature brief scan before compaction: 17 listed files, 88,034 chars, about 22,008 approximate tokens.
- Active feature brief scan after compaction: 5 active feature files, 15,275 chars, about 3,819 approximate tokens.
- Stale read-first instruction search found no active instruction still requiring bulk reads of both domain reference docs; the only stale bulk-read prompt match was in archived handoff evidence.
- Active long-line scan found no lines over 900 characters in the reviewed startup docs.
- `git diff --check` passed with expected CRLF warnings only.

## ADRs

No ADR added. This is a documentation architecture packet; it does not change product behavior, persistence, security, deployment, data model, cleanup execution, restore execution, or core UX.

## Follow-Up

- Keep future packet details in feature briefs and archives rather than expanding read-first docs.
- Prefer `docs/codex/safety-profiles.md` profile names over repeated safety boilerplate in active docs.
