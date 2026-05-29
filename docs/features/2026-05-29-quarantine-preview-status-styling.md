# Feature: Quarantine Preview Status Styling

Date started: 2026-05-29
Status: completed
Owner: project-owner

## Goal

Make the inline Quarantine Preview readiness/status line easier to notice by giving it lightweight semantic styling while preserving the existing dry-run and fixture-only safety boundaries.

## Non-goals

- Do not add a modal popup for preview success.
- Do not enable real-profile WPF Quarantine execution.
- Do not enable real-profile WPF Undo Quarantine.
- Do not add permanent deletion.
- Do not add persisted cleanup history.
- Do not change Quarantine Preview eligibility or blocker rules.

## User story / job story

As the project owner, I want preview/readiness feedback near the Quarantine controls to stand out enough that I can tell whether preview is waiting, ready, blocked, invalidated, executed, or undone without hunting through the status bar.

## Current behavior

The inline Quarantine Preview status text appears near the controls, but every state uses the same muted text styling.

## Desired behavior

- Neutral waiting text remains quiet.
- Ready preview and successful fixture execution/undo evidence use a success style.
- Readiness blockers, stale preview, or "needs review" states use a warning style.
- Invalid Quarantine Root / preview creation failure uses an error style.
- Text still states that preview is not cleanup approval and no files were modified where applicable.

## Domain language changes

No new domain terms.

| Term | Change | Docs updated? |
|---|---|---|
| Quarantine Preview | Clarify that inline preview readiness/status may use semantic styling while remaining a dry run. | yes |

## Open questions

Questions that must be answered before implementation:

- None. This is visual emphasis only.

Questions that can be deferred:

- Should Review Grid Mode Status get similar styling later?

## Grill notes

### Scenarios discussed

- User expected a popup after preview and then noticed the status text at the bottom/inline area.
- User confirmed collapsible panels worked and that useful closed-header summaries are desirable.

### Edge cases

- Styling must not imply cleanup approval.
- Error/warning states must not create folders, move files, restore files, delete files, or create cleanup history.

### Dependencies between decisions

- This depends on the existing inline `QuarantinePreviewStatusText`; no new workflow surface is needed.

## Evidence and validation gate

Evidence gathered:

- User answers: inline status was easy to miss; popup was expected, but the inline text did work once noticed.
- Existing code/docs inspected: `UpdateQuarantinePreviewStatus`, Quarantine Preview Inline Status feature brief, Quarantined Review Mode feature brief, ADRs 0009-0011, domain context/glossary.
- Tests/checks planned: WPF app smoke tests, focused app test build, solution build, `git diff --check`.

Validation gate before implementation:

- [x] Domain terms are clear enough.
- [x] Required lifecycle, permission, and persistence rules are clear enough.
- [x] The narrowest relevant verification path is known.
- [x] Open questions are either answered or explicitly deferred.

Rejected ideas buffer:

- Do not add a popup for preview success unless a future manual pass proves inline styling is still too subtle.

## Decisions made

Small feature-level decisions:

- Use text foreground and semibold weight only, avoiding layout expansion.
- Keep the status line in the existing Quarantine shortlist panel.
- Use semantic styling from current preview/execution state, not from additional persisted state.

ADR-worthy decisions:

- [x] None expected. This is reversible WPF styling with no persistence, file movement rule, restore rule, data-model, or security change.

## Implementation plan

1. Add a small status-style helper for `QuarantinePreviewStatusText`.
2. Apply neutral, success, warning, and error styles in existing preview-status update paths.
3. Add focused WPF smoke assertions for ready, stale/waiting, execution, and undo styling.
4. Update README, domain docs, progress, and handoff.

## Files expected to change

Expected:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `.codex/progress.md`
- `docs/codex/thread-handoff.md`

Possible:

- `src/WindowsFileCleaner.App/MainWindow.xaml`

## Test plan

Manual checks:

- During the next fixture visual pass, confirm the inline status is noticeable but does not make preview feel like cleanup approval.

Automated tests:

- `dotnet build tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/app-tests/"`
- `D:\Codex\Windows File Cleaner\.local\test-bin\app-tests\Debug\net8.0-windows\WindowsFileCleaner.App.Tests.exe`
- `dotnet build WindowsFileCleaner.sln --no-restore "-p:BaseOutputPath=D:/Codex/Windows File Cleaner/.local/test-bin/solution/"`
- `git -c safe.directory='D:/Codex/Windows File Cleaner' diff --check`

## Risks and assumptions

Risks:

- Color/weight may still be too subtle during manual review.
- Too much visual emphasis could make preview look like approval if wording is weakened.

Assumptions:

- Lightweight semantic styling is safer than a modal because Quarantine Preview remains a dry run and not cleanup approval.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added semantic styling for the inline `QuarantinePreviewStatusText`.
- Waiting/empty status remains neutral and normal weight.
- Ready preview plus successful fixture execution/undo evidence use success styling.
- Shortlisted-but-not-previewed, stale preview, blocked preview, and recovery-review states use warning styling.
- Preview creation failure uses error styling.
- Added WPF smoke assertions for neutral, warning, success, fixture execution, fixture undo, and blocked-preview styling.

Files changed:

- `src/WindowsFileCleaner.App/MainWindow.xaml.cs`
- `tests/WindowsFileCleaner.App.Tests/Program.cs`
- `README.md`
- `docs/domain/context.md`
- `docs/domain/glossary.md`
- `docs/features/2026-05-29-quarantine-preview-status-styling.md`
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

- Skipped. This is reversible WPF styling with no persistence, cleanup execution, restore rule, data-model, or security change.

Follow-up work:

- Manual fixture visual pass to confirm the styled inline status is noticeable without reading like cleanup approval.
- Consider similar styling for Review Grid Mode Status only if manual review shows it is still too easy to miss.

Open questions:

- Should Review Grid Mode Status get similar styling later?

Risky assumptions:

- Lightweight semantic styling is enough to resolve the missed-preview-status issue without a modal popup.
