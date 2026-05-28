namespace WindowsFileCleaner.Core;

public static class RestoreManifestBuilder
{
    public static RestoreManifest BuildPlanned(
        QuarantineActionDraft actionDraft,
        RestoreManifestDraft restoreManifestDraft,
        DateTimeOffset createdAtUtc,
        string manifestId)
    {
        var normalizedManifestId = NormalizeManifestId(manifestId);
        var consistencyBlockers = BuildConsistencyBlockers(actionDraft, restoreManifestDraft);
        if (consistencyBlockers.Count > 0)
        {
            throw new ArgumentException("Restore Manifest inputs do not agree: " + string.Join(" ", consistencyBlockers));
        }

        var actionEntriesByOriginalPath = actionDraft.Entries
            .GroupBy(entry => PathSafety.GetFullPath(entry.OriginalPath), StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.ToArray(), StringComparer.OrdinalIgnoreCase);

        var entries = restoreManifestDraft.Entries
            .Select(entry => BuildEntry(actionEntriesByOriginalPath, entry))
            .ToArray();

        var created = createdAtUtc.ToUniversalTime();
        return new RestoreManifest(
            RestoreManifest.CurrentSchemaVersion,
            normalizedManifestId,
            restoreManifestDraft.DraftId,
            actionDraft.ActionId,
            created,
            created,
            PathSafety.GetFullPath(actionDraft.CleanupScopePath),
            PathSafety.GetFullPath(actionDraft.QuarantineRootPath),
            PathSafety.GetFullPath(actionDraft.ActionRootPath),
            PathSafety.GetFullPath(actionDraft.ItemsRootPath),
            PathSafety.GetFullPath(actionDraft.RestoreManifestPath),
            RestoreManifestActionStatus.Planned,
            entries,
            BuildWriteOrderNotes());
    }

    public static RestoreManifest WithEntryStatus(
        RestoreManifest manifest,
        string originalPath,
        RestoreManifestEntryStatus status,
        DateTimeOffset updatedAtUtc,
        string? errorMessage = null)
    {
        var normalizedOriginalPath = PathSafety.GetFullPath(originalPath);
        var matched = false;
        var updatedEntries = manifest.Entries
            .Select(entry =>
            {
                if (!SamePath(entry.OriginalPath, normalizedOriginalPath))
                {
                    return entry;
                }

                matched = true;
                var updatedAt = updatedAtUtc.ToUniversalTime();
                return entry with
                {
                    Status = status,
                    MoveStartedAtUtc = status == RestoreManifestEntryStatus.Moving && entry.MoveStartedAtUtc is null
                        ? updatedAt
                        : entry.MoveStartedAtUtc,
                    MoveCompletedAtUtc = status is RestoreManifestEntryStatus.Moved or RestoreManifestEntryStatus.Failed
                        ? updatedAt
                        : entry.MoveCompletedAtUtc,
                    ErrorMessage = status == RestoreManifestEntryStatus.Failed
                        ? string.IsNullOrWhiteSpace(errorMessage) ? "Move failed." : errorMessage.Trim()
                        : null
                };
            })
            .ToArray();

        if (!matched)
        {
            throw new ArgumentException($"Restore Manifest entry was not found: {originalPath}", nameof(originalPath));
        }

        return manifest with
        {
            UpdatedAtUtc = updatedAtUtc.ToUniversalTime(),
            ActionStatus = DetermineActionStatus(updatedEntries),
            Entries = updatedEntries
        };
    }

    public static RestoreManifestActionStatus DetermineActionStatus(IReadOnlyList<RestoreManifestEntry> entries)
    {
        if (entries.Count == 0)
        {
            return RestoreManifestActionStatus.Failed;
        }

        var movedCount = entries.Count(entry => entry.Status == RestoreManifestEntryStatus.Moved);
        var movingCount = entries.Count(entry => entry.Status == RestoreManifestEntryStatus.Moving);
        var failedCount = entries.Count(entry => entry.Status == RestoreManifestEntryStatus.Failed);

        if (movedCount == entries.Count)
        {
            return RestoreManifestActionStatus.Completed;
        }

        if (failedCount == entries.Count)
        {
            return RestoreManifestActionStatus.Failed;
        }

        if (failedCount > 0 && movedCount == 0 && movingCount == 0)
        {
            return RestoreManifestActionStatus.Failed;
        }

        if (failedCount > 0)
        {
            return RestoreManifestActionStatus.PartialFailure;
        }

        if (movingCount > 0 || movedCount > 0)
        {
            return RestoreManifestActionStatus.Moving;
        }

        return RestoreManifestActionStatus.Planned;
    }

    private static RestoreManifestEntry BuildEntry(
        IReadOnlyDictionary<string, QuarantineActionEntryDraft[]> actionEntriesByOriginalPath,
        RestoreManifestEntryDraft entry)
    {
        var originalPath = PathSafety.GetFullPath(entry.OriginalPath);
        if (!actionEntriesByOriginalPath.TryGetValue(originalPath, out var actionEntries))
        {
            throw new ArgumentException($"Restore Manifest Draft entry is missing from Quarantine Action Draft: {entry.OriginalPath}");
        }

        if (actionEntries.Length > 1)
        {
            throw new ArgumentException($"Quarantine Action Draft has duplicate entries for: {entry.OriginalPath}");
        }

        var actionEntry = actionEntries[0];
        if (entry.SizeBytes != actionEntry.SizeBytes)
        {
            throw new ArgumentException($"Restore Manifest Draft size does not match Quarantine Action Draft entry: {entry.OriginalPath}");
        }

        return new RestoreManifestEntry(
            originalPath,
            actionEntry.RelativePath,
            PathSafety.GetFullPath(actionEntry.ActionQuarantinePath),
            entry.IsDirectory,
            entry.SizeBytes,
            entry.LastModifiedUtc,
            entry.ImportanceRating,
            entry.DeletionRecommendation,
            entry.BloatCategories,
            entry.Evidence,
            RestoreManifestEntryStatus.Planned,
            MoveStartedAtUtc: null,
            MoveCompletedAtUtc: null,
            ErrorMessage: null);
    }

    private static IReadOnlyList<string> BuildConsistencyBlockers(
        QuarantineActionDraft actionDraft,
        RestoreManifestDraft restoreManifestDraft)
    {
        var blockers = new List<string>();

        if (restoreManifestDraft.SchemaVersion != RestoreManifestDraft.CurrentSchemaVersion)
        {
            blockers.Add("Restore Manifest Draft schema version is not supported.");
        }

        if (!string.Equals(actionDraft.RestoreManifestDraftId, restoreManifestDraft.DraftId, StringComparison.Ordinal))
        {
            blockers.Add("Restore Manifest Draft id does not match the Quarantine Action Draft.");
        }

        if (!SamePath(actionDraft.CleanupScopePath, restoreManifestDraft.CleanupScopePath))
        {
            blockers.Add("Cleanup Scope paths do not match.");
        }

        if (!SamePath(actionDraft.QuarantineRootPath, restoreManifestDraft.QuarantineRootPath))
        {
            blockers.Add("Quarantine root paths do not match.");
        }

        if (actionDraft.EntryCount != restoreManifestDraft.EntryCount)
        {
            blockers.Add("Entry counts do not match.");
        }

        if (actionDraft.TotalBytes != restoreManifestDraft.TotalBytes)
        {
            blockers.Add("Entry byte totals do not match.");
        }

        return blockers;
    }

    private static IReadOnlyList<string> BuildWriteOrderNotes()
    {
        return
        [
            "Write this planned Restore Manifest before the first file or folder move.",
            "Before each move, update that entry to Moving and write the Restore Manifest again.",
            "After each move attempt, update that entry to Moved or Failed and write the Restore Manifest again.",
            "Undo Quarantine should restore entries recorded as Moved; Moving and Failed entries require recovery review."
        ];
    }

    private static string NormalizeManifestId(string manifestId)
    {
        var normalized = (manifestId ?? "").Trim();
        if (normalized.Length == 0)
        {
            throw new ArgumentException("Restore Manifest id is required.", nameof(manifestId));
        }

        return normalized;
    }

    private static bool SamePath(string left, string right)
    {
        return PathSafety.GetFullPath(left).Equals(PathSafety.GetFullPath(right), StringComparison.OrdinalIgnoreCase);
    }
}
