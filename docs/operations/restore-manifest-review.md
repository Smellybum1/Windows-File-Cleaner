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

MVP preflight also runs a synthetic daily readiness spotlight regression to prove the broad summary can include fixture-scope undo work while the final exact-profile spotlight stays focused on `C:\Users\moxhe`.

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
