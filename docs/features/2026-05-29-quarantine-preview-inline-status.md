# Feature: Quarantine Preview Inline Status

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make Quarantine Preview success and readiness visible inside the Quarantine shortlist panel, not only in the status bar or verbose preview/gate text.

## Non-goals

- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not add permanent deletion.
- Do not add persisted cleanup history.
- Do not change Quarantine Preview eligibility rules.

## User story / job story

As the project owner, I want a visible readiness line near the Quarantine controls, so I can tell whether `Preview shortlist quarantine` worked without hunting for status-bar text.

## Desired behavior

- Show a compact inline readiness/status line in the Quarantine shortlist area.
- Before preview, explain whether Review Shortlist rows need to be added or previewed.
- After preview, summarize included, blocked, redundant, previewed bytes, readiness blockers, no-file-modified behavior, and the not-cleanup-approval boundary.
- After fixture execution or undo, switch the inline line to fixture execution or undo evidence.
- When Quarantine Root changes, show that preview destinations need to be regenerated.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Preview | Added `QuarantinePreviewStatusText` as the WPF inline status surface for preview/readiness state. | yes |

## Open questions

- Should the inline line later become a styled warning/success state, or is plain text enough after manual review?

## Decisions made

Small feature-level decisions:

- Keep this as text inside the existing Quarantine shortlist area so it stays close to Quarantine Root, Preview, confirmation, execution, and undo.
- Reuse current preview, confirmation draft, execution, and undo state instead of adding new model state.

ADR-worthy decisions:

- [x] None expected. This is reversible WPF visibility polish with no persistence, file movement rule, restore rule, data-model, or security change.

## Implementation plan

1. Add `QuarantinePreviewStatusText` to the Quarantine shortlist panel.
2. Add a formatter/update helper driven by current preview, confirmation draft, execution, and undo state.
3. Update WPF smoke tests for preview-ready, root-invalidated, fixture execution, and undo states.
4. Update README, domain docs, progress, and handoff.

## Test plan

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- The Quarantine shortlist panel is already dense, so another text line could add vertical pressure.

Assumptions:

- A compact inline status is less surprising than a popup because preview remains a dry run and should not feel like approval or execution.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added an inline Quarantine Preview readiness/status line in the Quarantine shortlist panel.
- Preview success now shows included, blocked, redundant, bytes previewed, blocker count, not-cleanup-approval wording, and no-file-modified wording near the Preview button.
- Quarantine Root changes now show inline preview invalidation.
- Fixture execution and undo now replace the inline preview status with fixture execution/undo evidence.
- Later packet `2026-05-30-quarantine-preview-status-help-text.md` mirrored the dynamic inline status into tooltip and automation help text with no-create/no-move/no-restore/no-delete/not-cleanup-approval boundaries.
- Later packet `2026-05-30-quarantine-preview-status-help-cue.md` added a visible circular `?` help cue beside the inline status so the mirrored help text is easier to discover.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml`
- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-preview-inline-status.md`
- `docs/codex/thread-handoff.md`
- `.codex/progress.md`

Tests run:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

Docs updated:

- README, domain context, glossary, this feature brief, progress log, and handoff.

ADRs added or skipped:

- Skipped. This is reversible UI visibility polish.

Follow-up work:

- Manual fixture visual pass to confirm the added inline text does not make the grid feel cramped.

Open questions:

- Should the inline readiness line use visual severity styling after another manual pass?

Risky assumptions:

- Plain text is enough to resolve the missed-preview-success issue without adding a modal popup.
- Later visual review and help-text packets kept the inline status non-modal while making the current state easier to find.
