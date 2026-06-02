# Workflow And Markdown Bloat Reduction

Date: 2026-06-03

Status: completed

## Goal

Reduce Codex startup/context load by making the active docs smaller and more navigable while preserving the existing local-first safety boundaries.

## Non-Goals

- No app behavior changes.
- No cleanup, Quarantine, restore, deletion, WPF launch, scan, shortcut creation, installer behavior, or cleanup history.
- No change to accepted portable package evidence.
- No ADR change unless the cleanup uncovers a durable product or architecture decision.

## Safety Profile

`docs-only` from `docs/codex/safety-profiles.md`.

## Changes

- Archive the long historical progress log and long thread handoff instead of making every fresh thread read them.
- Add compact current-state and safety-profile docs for startup.
- Move daily command detail into `docs/operations/`.
- Add a feature brief index so fresh threads can open current briefs first and search historical briefs only when needed.
- Trim README and glossary command/checklist duplication by pointing to the operational runbooks.

## Verification

- `git diff --check` passed with expected CRLF warnings.
- Active doc size scan showed `.codex/progress.md` at 56 lines, `docs/codex/thread-handoff.md` at 87 lines, and no active long lines over 900 characters in `README.md`, `docs/domain/glossary.md`, or `docs/domain/context.md`.
- Stale startup/checklist phrase search found only archived handoff matches.
- No app tests, preflight, WPF launch, scan, movement, restore, deletion, or cleanup history commands were run because this packet is docs-only.

## ADRs

Skipped. This packet reorganizes documentation surfaces and does not introduce a new durable product, persistence, security, deployment, data-model, or core UX decision.

## Follow-Up

- Consider a later link-preserving archive/index pass for older feature briefs if the top-level `docs/features/` folder remains hard to navigate.
- Consider consolidating repeated safety boilerplate in older historical feature briefs only when those files are touched for related work.
