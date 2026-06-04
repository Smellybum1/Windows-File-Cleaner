# Domain Context

---
last_reviewed: 2026-06-04
owner: project-owner
stability: compact
---

## Purpose

This compact context is the read-first domain summary for Windows File Cleaner. It names the stable product rules that should guide implementation without loading the full historical context.

Detailed historical domain notes are preserved in `docs/domain/context-reference.md`. Open that reference only when a task needs old wording, full UI-help details, or a term that is not clear from this compact file and `docs/domain/glossary.md`.

## Product Summary

Windows File Cleaner is a local Windows-only WPF desktop app for reviewing storage under `C:\Users\moxhe`.

The app exists to reduce storage pressure on the Windows system drive without damaging user files, application state, credentials, source code, game saves, current app behavior, or Windows profile behavior.

The first production workflow is Storage Scan: a read-only scan and review experience. Cleanup execution is deliberately narrow, reversible-first, and gated.

## Core Domain Rules

- Cleanup Scope is the path allowed for a scan or cleanup run. The initial real Cleanup Scope is exactly `C:\Users\moxhe`.
- Storage Scan must not modify files. It is analysis and review only.
- Cleanup Candidate means a path is worth review, not that it is safe to remove.
- Large, old, hidden, or `AppData` paths are not removable by default.
- Protected Locations, credential data, cloud sync data, browser profiles, game data, source code, app settings, and broad profile containers are high-risk by default.
- Specific rebuildable cache evidence can support a Likely safe / Quarantine candidate recommendation, but broad parent folders remain inspection-first.
- Review Shortlist is in-memory review context. It is not cleanup approval.
- Quarantine Preview is a dry run. It does not create folders, move files, write manifests, restore files, delete files, or approve cleanup.
- Reversible Quarantine on `D:` is preferred before any permanent deletion. Permanent deletion is not implemented.
- Restore Manifest is the durable recovery record for Quarantine movement.
- Undo Quarantine and selected restore must use Restore Manifest evidence and must not overwrite existing original paths.

## Available Movement Workflows

- Fixture Quarantine execution is available for recognized synthetic fixture Cleanup Scopes after preview readiness and exact `QUARANTINE`.
- Current-fixture undo is available for the current synthetic fixture execution.
- Fixture selected restore is available for one selected discovered fixture Restore Manifest after selected readiness and exact `RESTORE`.
- Exact real-profile selected restore is available for one selected `C:\Users\moxhe` Restore Manifest after selected readiness, exact `RESTORE`, and immediate selected-restore revalidation.
- First-phase exact real-profile Quarantine is available only for exact `C:\Users\moxhe` after ADR 0017/0018 readiness, exact `QUARANTINE`, Real-Profile Quarantine Approval Evidence, selected restore trust, immediate Pre-Execution Revalidation, and explicit human approval for the specific tiny batch.

## Unavailable Workflows

- Broad/all-manifest real-profile Undo Quarantine.
- Custom or non-exact real-profile Quarantine.
- Custom selected restore.
- Permanent deletion.
- Persisted cleanup history.
- Installed shortcut or installer automation, unless a later explicit ADR 0020 follow-up packet defines it.

## Portable Package Rules

- Portable Release Package is a local self-contained WPF publish output under ignored `.local\releases`.
- Accepted-package launch commands are the v1 daily path.
- Accepted-package helpers select completed acceptance notes by default; pending package-candidate notes require an explicit path.
- Acceptance recording requires verifier evidence, and package/current-HEAD mismatch evidence must be explicit.
- Generated package acceptance notes must stamp the actual `-ReleasePath` and current-commit command context.
- Portable v1 is not an installer, does not create shortcuts, and does not add deletion, broad restore, custom real-profile movement, or cleanup history.

## Documentation Boundaries

- Use `docs/codex/current-state.md` for current evidence and next work.
- Use `docs/domain/glossary.md` for preferred names and forbidden synonyms.
- Use `docs/codex/safety-profiles.md` for reusable safety profile wording.
- Use `docs/operations/*.md` for command details.
- Use ADRs for durable, hard-to-reverse decisions.
- Use feature briefs for packet-specific plans and completion evidence.

## Open Domain Questions

- Should the default Quarantine root `D:\WindowsFileCleanerQuarantine` remain the long-term execution default?
- Which additional locations should be protected from cleanup but still shown in reports?
- Should permanent deletion, persisted cleanup history, or all-manifest restore ever be added?
