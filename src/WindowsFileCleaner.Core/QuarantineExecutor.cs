namespace WindowsFileCleaner.Core;

public static class QuarantineExecutor
{
    public static QuarantineExecutionResult Execute(RestoreManifest plannedManifest)
    {
        return Execute(plannedManifest, RestoreManifestFileStore.Write);
    }

    public static QuarantineExecutionResult Execute(
        RestoreManifest plannedManifest,
        Func<RestoreManifest, RestoreManifestFileWriteResult> writeManifest)
    {
        var blockers = ValidatePlannedManifest(plannedManifest);
        if (blockers.Count > 0)
        {
            var failedManifest = plannedManifest with
            {
                ActionStatus = RestoreManifestActionStatus.Failed,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            };
            return new QuarantineExecutionResult(failedManifest, [], blockers);
        }

        var currentManifest = plannedManifest;
        var results = new List<QuarantineExecutionEntryResult>();
        var executionBlockers = new List<string>();

        if (!TryWriteManifest(writeManifest, currentManifest, executionBlockers, "Planned Restore Manifest write failed before any files were moved."))
        {
            return new QuarantineExecutionResult(
                currentManifest with
                {
                    ActionStatus = RestoreManifestActionStatus.Failed,
                    UpdatedAtUtc = DateTimeOffset.UtcNow
                },
                results,
                executionBlockers);
        }

        foreach (var entry in currentManifest.Entries)
        {
            currentManifest = RestoreManifestBuilder.WithEntryStatus(
                currentManifest,
                entry.OriginalPath,
                RestoreManifestEntryStatus.Moving,
                DateTimeOffset.UtcNow);

            if (!TryWriteManifest(writeManifest, currentManifest, executionBlockers, $"Restore Manifest write failed before moving: {entry.OriginalPath}"))
            {
                results.Add(new QuarantineExecutionEntryResult(
                    entry.OriginalPath,
                    entry.QuarantinePath,
                    RestoreManifestEntryStatus.Failed,
                    WasMoved: false,
                    "Restore Manifest write failed before move."));
                break;
            }

            var moveError = TryMoveEntry(entry);
            if (moveError is null)
            {
                currentManifest = RestoreManifestBuilder.WithEntryStatus(
                    currentManifest,
                    entry.OriginalPath,
                    RestoreManifestEntryStatus.Moved,
                    DateTimeOffset.UtcNow);

                results.Add(new QuarantineExecutionEntryResult(
                    entry.OriginalPath,
                    entry.QuarantinePath,
                    RestoreManifestEntryStatus.Moved,
                    WasMoved: true,
                    ErrorMessage: null));

                if (!TryWriteManifest(writeManifest, currentManifest, executionBlockers, $"Restore Manifest write failed after moving: {entry.OriginalPath}"))
                {
                    break;
                }

                continue;
            }

            currentManifest = RestoreManifestBuilder.WithEntryStatus(
                currentManifest,
                entry.OriginalPath,
                RestoreManifestEntryStatus.Failed,
                DateTimeOffset.UtcNow,
                moveError);

            results.Add(new QuarantineExecutionEntryResult(
                entry.OriginalPath,
                entry.QuarantinePath,
                RestoreManifestEntryStatus.Failed,
                WasMoved: false,
                moveError));

            if (!TryWriteManifest(writeManifest, currentManifest, executionBlockers, $"Restore Manifest write failed after failed move attempt: {entry.OriginalPath}"))
            {
                break;
            }
        }

        return new QuarantineExecutionResult(currentManifest, results, executionBlockers);
    }

    private static IReadOnlyList<string> ValidatePlannedManifest(RestoreManifest manifest)
    {
        var blockers = new List<string>();

        if (manifest.ActionStatus != RestoreManifestActionStatus.Planned)
        {
            blockers.Add("Quarantine Executor requires a planned Restore Manifest.");
        }

        if (manifest.EntryCount == 0)
        {
            blockers.Add("Quarantine Executor requires at least one Restore Manifest entry.");
        }

        if (manifest.Entries.Any(entry => entry.Status != RestoreManifestEntryStatus.Planned))
        {
            blockers.Add("Quarantine Executor requires all entries to start in Planned status.");
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
                blockers.Add($"Restore Manifest entry destination is outside the action items root: {entry.QuarantinePath}");
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

    private static string? TryMoveEntry(RestoreManifestEntry entry)
    {
        try
        {
            if (!SourceExists(entry))
            {
                return $"Source no longer exists: {entry.OriginalPath}";
            }

            if (DestinationExists(entry.QuarantinePath))
            {
                return $"Destination already exists: {entry.QuarantinePath}";
            }

            if (IsReparsePoint(entry.OriginalPath))
            {
                return $"Source became a reparse point after preview: {entry.OriginalPath}";
            }

            var destinationParent = Path.GetDirectoryName(entry.QuarantinePath);
            if (string.IsNullOrWhiteSpace(destinationParent))
            {
                return $"Destination path has no parent folder: {entry.QuarantinePath}";
            }

            Directory.CreateDirectory(destinationParent);

            if (entry.IsDirectory)
            {
                Directory.Move(entry.OriginalPath, entry.QuarantinePath);
            }
            else
            {
                File.Move(entry.OriginalPath, entry.QuarantinePath);
            }

            return null;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private static bool SourceExists(RestoreManifestEntry entry)
    {
        return entry.IsDirectory
            ? Directory.Exists(entry.OriginalPath)
            : File.Exists(entry.OriginalPath);
    }

    private static bool DestinationExists(string quarantinePath)
    {
        return File.Exists(quarantinePath) || Directory.Exists(quarantinePath);
    }

    private static bool IsReparsePoint(string path)
    {
        return File.GetAttributes(path).HasFlag(FileAttributes.ReparsePoint);
    }
}
