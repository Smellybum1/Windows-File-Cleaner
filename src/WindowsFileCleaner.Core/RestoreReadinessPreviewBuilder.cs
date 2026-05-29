namespace WindowsFileCleaner.Core;

public static class RestoreReadinessPreviewBuilder
{
    public static RestoreReadinessPreview BuildForQuarantineRoot(string quarantineRootPath)
    {
        var discovery = QuarantineManifestDiscoveryBuilder.Discover(quarantineRootPath);
        var manifestPreviews = discovery.RestoreManifests
            .Select(BuildManifestPreview)
            .OrderByDescending(preview => preview.UpdatedAtUtc)
            .ThenBy(preview => preview.ActionId, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return new RestoreReadinessPreview(
            discovery.QuarantineRootPath,
            discovery.ActionsRootPath,
            manifestPreviews,
            discovery.Issues);
    }

    public static RestoreReadinessManifestPreview BuildManifestPreview(RestoreManifest manifest)
    {
        var manifestBlockers = BuildManifestBlockers(manifest);
        var entries = manifest.Entries
            .Select(entry => BuildEntryPreview(entry, manifestBlockers.Count > 0))
            .ToArray();

        return new RestoreReadinessManifestPreview(
            manifest.ManifestPath,
            manifest.ActionId,
            manifest.ActionStatus,
            manifest.UpdatedAtUtc,
            manifest.EntryCount,
            manifest.TotalBytes,
            manifest.RequiresRecoveryReview,
            manifestBlockers,
            entries);
    }

    private static RestoreReadinessEntryPreview BuildEntryPreview(
        RestoreManifestEntry entry,
        bool manifestHasBlockers)
    {
        if (manifestHasBlockers)
        {
            return CreateEntryPreview(
                entry,
                RestoreReadinessDisposition.Blocked,
                ["Restore Manifest has path or scope blockers."]);
        }

        if (entry.Status == RestoreManifestEntryStatus.Restored)
        {
            return CreateEntryPreview(entry, RestoreReadinessDisposition.AlreadyRestored, []);
        }

        if (entry.Status is RestoreManifestEntryStatus.Failed
            or RestoreManifestEntryStatus.Moving
            or RestoreManifestEntryStatus.Restoring
            or RestoreManifestEntryStatus.RestoreFailed)
        {
            return CreateEntryPreview(
                entry,
                RestoreReadinessDisposition.NeedsRecoveryReview,
                [$"Entry status requires recovery review before restore: {FormatEntryStatus(entry.Status)}."]);
        }

        if (entry.Status != RestoreManifestEntryStatus.Moved)
        {
            return CreateEntryPreview(
                entry,
                RestoreReadinessDisposition.NotMoved,
                [$"Entry is not moved and cannot be restored from quarantine: {FormatEntryStatus(entry.Status)}."]);
        }

        var blockers = BuildMovedEntryBlockers(entry);
        return CreateEntryPreview(
            entry,
            blockers.Count == 0 ? RestoreReadinessDisposition.Restorable : RestoreReadinessDisposition.Blocked,
            blockers);
    }

    private static RestoreReadinessEntryPreview CreateEntryPreview(
        RestoreManifestEntry entry,
        RestoreReadinessDisposition disposition,
        IReadOnlyList<string> blockers)
    {
        return new RestoreReadinessEntryPreview(
            entry.OriginalPath,
            entry.QuarantinePath,
            entry.RelativePath,
            entry.IsDirectory,
            entry.SizeBytes,
            entry.Status,
            disposition,
            blockers);
    }

    private static IReadOnlyList<string> BuildManifestBlockers(RestoreManifest manifest)
    {
        var blockers = new List<string>();

        if (!PathSafety.IsWithinScope(manifest.QuarantineRootPath, manifest.ActionRootPath))
        {
            blockers.Add("Quarantine Action root must stay inside the Quarantine root.");
        }

        if (!PathSafety.IsWithinScope(manifest.ActionRootPath, manifest.ManifestPath))
        {
            blockers.Add("Restore Manifest path must stay inside the Quarantine Action root.");
        }

        if (!PathSafety.IsWithinScope(manifest.ActionRootPath, manifest.ItemsRootPath))
        {
            blockers.Add("Restore Manifest items root must stay inside the Quarantine Action root.");
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

    private static IReadOnlyList<string> BuildMovedEntryBlockers(RestoreManifestEntry entry)
    {
        var blockers = new List<string>();

        if (!QuarantinePathExists(entry))
        {
            blockers.Add($"Quarantine path no longer exists: {entry.QuarantinePath}");
            return blockers;
        }

        if (OriginalPathExists(entry))
        {
            blockers.Add($"Original path already exists: {entry.OriginalPath}");
        }

        var reparsePointBlocker = BuildReparsePointBlocker(entry.QuarantinePath);
        if (reparsePointBlocker is not null)
        {
            blockers.Add(reparsePointBlocker);
        }

        var originalParent = Path.GetDirectoryName(entry.OriginalPath);
        if (string.IsNullOrWhiteSpace(originalParent))
        {
            blockers.Add($"Original path has no parent folder: {entry.OriginalPath}");
        }

        return blockers;
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

    private static string? BuildReparsePointBlocker(string path)
    {
        try
        {
            return File.GetAttributes(path).HasFlag(FileAttributes.ReparsePoint)
                ? $"Quarantine path is a reparse point: {path}"
                : null;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return $"Could not inspect quarantine path attributes: {path} | {ex.Message}";
        }
    }

    private static string FormatEntryStatus(RestoreManifestEntryStatus status)
    {
        return status switch
        {
            RestoreManifestEntryStatus.Planned => "Planned",
            RestoreManifestEntryStatus.Moving => "Moving",
            RestoreManifestEntryStatus.Moved => "Moved",
            RestoreManifestEntryStatus.Failed => "Failed",
            RestoreManifestEntryStatus.Restoring => "Restoring",
            RestoreManifestEntryStatus.Restored => "Restored",
            RestoreManifestEntryStatus.RestoreFailed => "Restore failed",
            _ => status.ToString()
        };
    }
}
