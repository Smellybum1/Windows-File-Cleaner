# Restore Manifest Review Runbook

Last updated: 2026-06-03

Restore Manifest discovery/review lives in the WPF Quarantine tab's `Restore Manifest Review` panel. Terminal summaries are read-only evidence and do not add broad/all-manifest restore.

## Terminal Summary

```powershell
.\tools\Summarize-RestoreManifests.cmd
```

## Exact-Profile Display

```powershell
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RequireAnyDisplayed -RequireNoDisplayedUndoWork
```

## Recovery Review Focus

```powershell
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -RecoveryReviewOnly -ShowEntries
```

## Undo Work Focus

```powershell
.\tools\Summarize-RestoreManifests.cmd -CleanupScope "C:\Users\moxhe" -UndoWorkOnly
```

## Boundaries

- WPF restore remains one selected fixture or exact `C:\Users\moxhe` Restore Manifest after readiness and exact `RESTORE`.
- Broad/all-manifest restore remains unavailable.
- Terminal summaries do not launch WPF, scan, move, restore, delete, write manifests, approve cleanup, or create cleanup history.
