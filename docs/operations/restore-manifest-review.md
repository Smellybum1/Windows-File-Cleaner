# Restore Manifest Review Runbook

Last updated: 2026-06-04

Restore Manifest discovery/review lives in the WPF Quarantine tab's `Restore Manifest Review` panel. Terminal summaries are read-only evidence and do not add broad/all-manifest restore.

## Terminal Summary

```powershell
.\tools\Summarize-RestoreManifests.cmd
```

## Exact-Profile Display

```powershell
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -ShowEntries
```

After the 2026-06-04 second exact-profile Quarantine batch, displayed undo work is expected to be `1` until the selected manifest is restored. Use `-RequireNoDisplayedUndoWork` only when zero displayed undo work is the intended assertion.

The real-profile next-batch evidence preset uses that displayed undo-work strictness before MVP preflight, so outstanding selected-manifest undo work stops the preset early.

MVP preflight also runs a synthetic daily readiness spotlight regression to prove the broad summary can include fixture-scope undo work while the final exact-profile spotlight stays focused on `C:\Users\moxhe`. The regression uses a focused `.local` Restore Manifest-only mode, so it stays independent from ignored accepted-package notes and local package folders.

## Recovery Review Focus

```powershell
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries
```

## Undo Work Focus

```powershell
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly -ShowEntries
```

## Boundaries

- WPF restore remains one selected fixture or exact `C:\Users\moxhe` Restore Manifest after readiness and exact `RESTORE`.
- Broad/all-manifest restore remains unavailable.
- Terminal summaries do not launch WPF, scan, move, restore, delete, write manifests, approve cleanup, or create cleanup history.

## Trust Helper Guard

The sacrificial selected-restore trust helper remains a human-intent test tool. It now rejects explicit `QuarantineRoot` values outside the default `D:\WindowsFileCleanerQuarantine` root or ignored repo `.local`, and rejects generated quarantine source paths outside the action `items` root even when the requested restore target normalizes back inside `C:\Users\moxhe`.

Targeted local regression:

```powershell
.\tools\Test-RealProfileSelectedRestoreTrustManifestPathGuard.cmd
```

This runs the helper only with `-WhatIf`, an ignored `.local` Quarantine Root, and committed `README.md` as a non-`.local` rejection target. It does not launch WPF, scan, move, restore, delete, write Restore Manifests, modify real-profile files, approve cleanup, or create cleanup history.
