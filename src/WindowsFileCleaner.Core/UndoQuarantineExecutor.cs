namespace WindowsFileCleaner.Core;

public static class UndoQuarantineExecutor
{
    public static UndoQuarantineResult Undo(RestoreManifest manifest)
    {
        return Undo(manifest, RestoreManifestFileStore.Write);
    }

    public static UndoQuarantineResult Undo(
        RestoreManifest manifest,
        Func<RestoreManifest, RestoreManifestFileWriteResult> writeManifest)
    {
        var blockers = ValidateManifestForUndo(manifest);
        if (blockers.Count > 0)
        {
            return new UndoQuarantineResult(
                manifest with
                {
                    ActionStatus = RestoreManifestActionStatus.RestoreFailed,
                    UpdatedAtUtc = DateTimeOffset.UtcNow
                },
                [],
                blockers);
        }

        var currentManifest = manifest;
        var results = new List<UndoQuarantineEntryResult>();
        var undoBlockers = new List<string>();

        foreach (var entry in currentManifest.Entries.Where(entry => entry.Status == RestoreManifestEntryStatus.Moved))
        {
            currentManifest = RestoreManifestBuilder.WithEntryStatus(
                currentManifest,
                entry.OriginalPath,
                RestoreManifestEntryStatus.Restoring,
                DateTimeOffset.UtcNow);

            if (!TryWriteManifest(writeManifest, currentManifest, undoBlockers, $"Restore Manifest write failed before restoring: {entry.OriginalPath}"))
            {
                results.Add(new UndoQuarantineEntryResult(
                    entry.OriginalPath,
                    entry.QuarantinePath,
                    RestoreManifestEntryStatus.RestoreFailed,
                    WasRestored: false,
                    "Restore Manifest write failed before restore."));
                break;
            }

            var restoreError = TryRestoreEntry(entry);
            if (restoreError is null)
            {
                currentManifest = RestoreManifestBuilder.WithEntryStatus(
                    currentManifest,
                    entry.OriginalPath,
                    RestoreManifestEntryStatus.Restored,
                    DateTimeOffset.UtcNow);

                results.Add(new UndoQuarantineEntryResult(
                    entry.OriginalPath,
                    entry.QuarantinePath,
                    RestoreManifestEntryStatus.Restored,
                    WasRestored: true,
                    ErrorMessage: null));

                if (!TryWriteManifest(writeManifest, currentManifest, undoBlockers, $"Restore Manifest write failed after restoring: {entry.OriginalPath}"))
                {
                    break;
                }

                continue;
            }

            currentManifest = RestoreManifestBuilder.WithEntryStatus(
                currentManifest,
                entry.OriginalPath,
                RestoreManifestEntryStatus.RestoreFailed,
                DateTimeOffset.UtcNow,
                restoreError);

            results.Add(new UndoQuarantineEntryResult(
                entry.OriginalPath,
                entry.QuarantinePath,
                RestoreManifestEntryStatus.RestoreFailed,
                WasRestored: false,
                restoreError));

            if (!TryWriteManifest(writeManifest, currentManifest, undoBlockers, $"Restore Manifest write failed after failed restore attempt: {entry.OriginalPath}"))
            {
                break;
            }
        }

        return new UndoQuarantineResult(currentManifest, results, undoBlockers);
    }

    private static IReadOnlyList<string> ValidateManifestForUndo(RestoreManifest manifest)
    {
        var blockers = new List<string>();

        if (manifest.Entries.All(entry => entry.Status != RestoreManifestEntryStatus.Moved))
        {
            blockers.Add("Undo Quarantine requires at least one moved Restore Manifest entry.");
        }

        if (!PathSafety.IsWithinScope(manifest.QuarantineRootPath, manifest.ActionRootPath))
        {
            blockers.Add("Quarantine Action root must stay inside the Quarantine root.");
        }

        if (!PathSafety.IsWithinScope(manifest.ActionRootPath, manifest.ManifestPath))
        {
            blockers.Add("Restore Manifest path must stay inside the Quarantine Action root.");
        }

        foreach (var entry in manifest.Entries)
        {
            if (!PathSafety.IsWithinScope(manifest.CleanupScopePath, entry.OriginalPath))
            {
                blockers.Add($"Restore Manifest entry is outside the Cleanup Scope: {entry.OriginalPath}");
            }

            if (!PathSafety.IsWithinScope(manifest.ItemsRootPath, entry.QuarantinePath))
            {
                blockers.Add($"Restore Manifest entry quarantine path is outside the action items root: {entry.QuarantinePath}");
            }
        }

        return blockers;
    }

    private static bool TryWriteManifest(
        Func<RestoreManifest, RestoreManifestFileWriteResult> writeManifest,
        RestoreManifest manifest,
        List<string> blockers,
        string failureMessage)
    {
        try
        {
            writeManifest(manifest);
            return true;
        }
        catch (Exception ex)
        {
            blockers.Add($"{failureMessage} {ex.Message}");
            return false;
        }
    }

    private static string? TryRestoreEntry(RestoreManifestEntry entry)
    {
        try
        {
            if (!QuarantinePathExists(entry))
            {
                return $"Quarantine path no longer exists: {entry.QuarantinePath}";
            }

            if (OriginalPathExists(entry))
            {
                return $"Original path already exists: {entry.OriginalPath}";
            }

            if (IsReparsePoint(entry.QuarantinePath))
            {
                return $"Quarantine path became a reparse point before restore: {entry.QuarantinePath}";
            }

            var originalParent = Path.GetDirectoryName(entry.OriginalPath);
            if (string.IsNullOrWhiteSpace(originalParent))
            {
                return $"Original path has no parent folder: {entry.OriginalPath}";
            }

            Directory.CreateDirectory(originalParent);

            if (entry.IsDirectory)
            {
                Directory.Move(entry.QuarantinePath, entry.OriginalPath);
            }
            else
            {
                File.Move(entry.QuarantinePath, entry.OriginalPath);
            }

            return null;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private static bool QuarantinePathExists(RestoreManifestEntry entry)
    {
        return entry.IsDirectory
            ? Directory.Exists(entry.QuarantinePath)
            : File.Exists(entry.QuarantinePath);
    }

    private static bool OriginalPathExists(RestoreManifestEntry entry)
    {
        return entry.IsDirectory
            ? Directory.Exists(entry.OriginalPath)
            : File.Exists(entry.OriginalPath);
    }

    private static bool IsReparsePoint(string path)
    {
        return File.GetAttributes(path).HasFlag(FileAttributes.ReparsePoint);
    }
}
