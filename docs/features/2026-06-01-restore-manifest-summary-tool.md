# Feature: Restore Manifest Summary Tool

Date started: 2026-06-01
Status: completed
Owner: project-owner

## Goal

Add a terminal-only, read-only way to summarize action-scoped Restore Manifests under a Quarantine Root so recovery state is easier to inspect without digging through dense WPF gate text.

## Non-goals

- Do not launch WPF.
- Do not scan fixture or real-profile files.
- Do not move, restore, delete, write Restore Manifests, approve cleanup, or create cleanup history.
- Do not add broad or all-manifest restore.
- Do not change selected restore, Quarantine, permanent deletion, or history availability.

## Context

The first live exact real-profile Quarantine and selected restore recovery loop succeeded by user report. The app now has good WPF recovery visibility, but terminal-side recovery confidence still depended on manually inspecting dense app text or raw JSON.

## Implementation

- Added `tools\Summarize-RestoreManifests.ps1` and `.cmd`.
- The tool defaults to `D:\WindowsFileCleanerQuarantine`, or accepts `-QuarantineRoot`.
- It reads `actions\*\restore-manifest.json`, validates the same core action-scoped layout shape, and reports:
  - manifest count and discovery issues,
  - total entries and size,
  - entry status counts,
  - cleanup scopes,
  - manifests with undo work,
  - manifests needing recovery review,
  - per-manifest status, counts, size, manifest path, and updated timestamp.
- `-ShowEntries` includes entry-level paths and errors.
- `-RequireAny` exits non-zero when no valid manifests are found.
- Later packet `2026-06-02-restore-manifest-recovery-review-filter.md` added `-RecoveryReviewOnly` to focus displayed manifests that need manual recovery review and `-RequireNoRecoveryReview` to fail a read-only terminal evidence check when any valid manifest still needs recovery review.
- Later packet `2026-06-02-restore-manifest-undo-work-filter.md` added `-UndoWorkOnly` to focus displayed manifests with moved entries still needing selected restore/undo review and `-RequireNoUndoWork` to fail a read-only terminal evidence check when any valid manifest still has undo work.
- The output repeats the selected-only/no-all-manifest restore boundary.

## Verification

- Created an ignored synthetic Restore Manifest under `.local\restore-manifest-summary-smoke`.
- `cmd.exe /c tools\Summarize-RestoreManifests.cmd -QuarantineRoot "D:\Codex\Windows File Cleaner\.local\restore-manifest-summary-smoke" -RequireAny -ShowEntries`
- `cmd.exe /c tools\Start-MvpFixtureReview.cmd -ChecklistOnly`
- `git diff --check` passed with existing LF-to-CRLF working-copy warnings.

## Docs

- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-06-01-live-product-readiness-roadmap.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

## ADRs

No ADR added. ADR 0003 already defines JSON Restore Manifests, ADR 0011 already accepts read-only Quarantine Manifest Discovery, ADR 0012 already accepts read-only Restore Readiness Preview, and ADR 0019 keeps real-profile restore selected-manifest-only. This packet adds local read-only summarization only.

## Follow-up Work

- Use the summary helper after future user-clicked Quarantine or selected restore work when terminal evidence is useful.
- Keep broad/all-manifest restore, permanent deletion, and persisted cleanup history as separate future decisions.

## Risks And Assumptions

- The PowerShell summary intentionally duplicates a small amount of manifest validation logic rather than adding a new console project.
- The helper is diagnostic evidence only; WPF readiness and selected restore gates remain authoritative before any restore movement.
