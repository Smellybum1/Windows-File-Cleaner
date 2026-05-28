# Feature: Selected File Content Preview

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Help the project owner inspect what is inside a selected file without modifying files or reading large content into the UI.

## Non-goals

- Do not preview folders as file content.
- Do not render binary or unsupported content as text.
- Do not render Credential Data content.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not treat file content preview as cleanup approval.

## User story / job story

As the project owner, I want to preview a small text snippet from a selected file, so that I can understand suspicious or unfamiliar rows before deciding whether to shortlist them for more review.

## Desired behavior

- Add a `Preview file` action for the selected Storage Scan row.
- Enable it only for selected files.
- Read a bounded text sample only after the user requests preview.
- Show text content for plain text files.
- Explain when the selected file is a folder, inaccessible, binary, or unsupported.
- Explain when the selected file is Credential Data and do not show its contents.
- Keep status wording explicit that no files were modified.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Selected File Content Preview | Added as a bounded, read-only text preview for a selected file. | yes |

## Decisions made

- Preview is explicit rather than automatic on row selection.
- The default preview reads at most 8 KB and displays at most 80 lines.
- Binary-looking content is not displayed as text.
- The preview uses file sharing flags that avoid taking an exclusive lock.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `dotnet run --project tests\WindowsFileCleaner.App.Tests\WindowsFileCleaner.App.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `SelectedFileContentPreview` and `SelectedFileContentPreviewBuilder`.
- Added `Preview file` to the WPF selected-row actions.
- Added a `File preview` section to the detail pane.
- Added core tests for text, binary, and folder preview outcomes.
- Added WPF smoke coverage for selected-file preview.
- Later cloud/credential guardrail packet blocks Credential Data content from preview and shows a read-only explanation instead.

Additional verification from later cloud/credential guardrail packet:

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.

ADRs added or skipped:

- No ADR. This is a reversible read-only review action and does not change architecture, persistence, security, deployment, or cleanup execution.

Follow-up work:

- Try `Preview file` against real scan text rows such as logs, config files, and small notes.
- Do not rely on preview for large binary installers, databases, archives, or browser/profile state.
