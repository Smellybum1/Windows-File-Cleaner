# Feature: Cleanup Scope Browse Action

Date started: 2026-05-28
Status: completed
Owner: project-owner

## Goal

Make Cleanup Scope Selection easier in the WPF app by adding a native folder browse action beside the path box.

## Non-goals

- Do not auto-scan after selecting a folder.
- Do not bypass real-profile preflight acknowledgement.
- Do not scan `C:\Users\moxhe` from tests or automation.
- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, manifest writing, or scan-history persistence.

## User story / job story

As the project owner, I want to browse for a fixture or custom Cleanup Scope instead of typing every path by hand, so that manual fixture review and later real-profile retests are less error-prone.

## Desired behavior

- The WPF header shows `Browse...` next to the Cleanup Scope path.
- `Browse...` opens a native folder picker.
- Choosing a folder updates the Cleanup Scope path box and safety-note/gate text.
- The app still waits for a manual `Scan` click.
- The browse action is disabled while a scan is running.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Cleanup Scope Selection | Added as the pre-scan path-selection action. | yes |

## Decisions made

- Use the built-in WPF `OpenFolderDialog`; no new dependency is needed.
- Keep the selected folder in `ScopePathBox` so existing safety note and scan gate logic remains authoritative.
- Keep browsing separate from scanning.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed with 0 warnings and 0 errors.
- WPF smoke tests assert the browse action is visible and available before and after fixture scans.
- Full MVP preflight was run after implementation.

## Completion notes

Completed on: 2026-05-28

What changed:

- Added `BrowseScopeButton` to the WPF header.
- Added native folder browser handling in `MainWindow`.
- Added WPF smoke assertions for browse availability.
- Updated README, domain docs, and progress log.

ADRs added or skipped:

- No ADR. This is a reversible WPF usability improvement that does not change scan, persistence, security, deployment, or cleanup behavior.

Rejected ideas buffer:

- Do not auto-start a scan after folder selection.
