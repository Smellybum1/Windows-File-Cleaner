# Feature: Cloud Sync and Credential Protection

Date started: 2026-05-29  
Status: completed  
Owner: project-owner

## Goal

Make cloud sync folders and credential/key/password-manager paths explicit high-risk review rows so large or unfamiliar protected data does not look like generic bloat.

## Non-goals

- Do not add cleanup execution, Quarantine execution, Undo Quarantine, permanent deletion, or manifest writing.
- Do not treat cloud sync folders as removable just because they are large or duplicated.
- Do not render credential file contents in Selected File Content Preview.
- Do not scan `C:\Users\moxhe` from tests or automation.

## User story / job story

As the project owner, I want synced user files and credential data to be clearly marked as protected, so that I do not accidentally shortlist files that could remove personal data or break access to tools and services.

## Desired behavior

- OneDrive, Dropbox, Google Drive, iCloud Drive/Photos, Nextcloud, Syncthing, and MEGA paths are labeled `Cloud sync data`.
- SSH keys, `.gnupg`, `.aws`, `.azure`, `.kube`, 1Password, Bitwarden, KeePass/KeePassXC, and `.kdbx` vault files are labeled `Credential data`.
- Both categories are `High risk` / `Keep` and gain `Protected location`.
- Credential Data is not rendered by Selected File Content Preview.
- CSV/export/category filters use user-facing labels for the new categories.

## Domain language changes

| Term | Change | Docs updated? |
|---|---|---|
| Cloud Sync Data | Added as protected user-owned sync data. | yes |
| Credential Data | Added as protected authentication/key/password-manager data. | yes |

## Decisions made

- Add explicit Bloat Category values rather than hiding these paths under generic Protected Location.
- Keep the rule conservative until the user defines precise provider-specific or credential-safe exceptions.
- Block credential content preview at the preview builder layer so UI callers cannot accidentally render secrets.

## Verification

- `dotnet build WindowsFileCleaner.sln --no-restore` passed.
- `dotnet run --project tests\WindowsFileCleaner.Tests\WindowsFileCleaner.Tests.csproj --no-build` passed.
- `powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\tools\Invoke-MvpPreflight.ps1` passed.

## Completion notes

Completed on: 2026-05-29

What changed:

- Added `CloudSyncData` and `CredentialData` Bloat Category values.
- Added classifier hints for common sync providers and credential/password-manager/key paths.
- Added display/export labels for WPF rows, category filters, Scan Report Export, and Quarantine Preview CSV.
- Blocked Selected File Content Preview for Credential Data.
- Added fixture coverage for OneDrive, Dropbox, SSH keys, Bitwarden, and KeePass vaults.

ADRs added or skipped:

- No ADR. This is a reversible conservative classification and preview-safety refinement that does not change architecture, persistence, deployment, or cleanup execution.

Follow-up work:

- In the next real scan, check whether cloud sync and credential rows are clearer and whether any provider-specific cache exceptions should be designed later.
