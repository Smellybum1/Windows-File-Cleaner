namespace WindowsFileCleaner.Core;

public static class QuarantinePreviewBuilder
{
    public const string DefaultQuarantineRootPath = @"D:\WindowsFileCleanerQuarantine";

    public static QuarantinePreview Build(
        IReadOnlyList<StorageReviewEntry> shortlistedRows,
        string cleanupScopePath,
        string quarantineRootPath = DefaultQuarantineRootPath)
    {
        var scope = PathSafety.GetFullPath(cleanupScopePath);
        var quarantineRoot = PathSafety.GetFullPath(quarantineRootPath);
        var candidates = shortlistedRows
            .Select(row => new PreviewCandidate(row, GetBlockers(row.Entry, scope)))
            .ToArray();

        var dispositions = new Dictionary<string, QuarantinePreviewEntry>(StringComparer.OrdinalIgnoreCase);
        var includedSources = new List<string>();

        foreach (var candidate in candidates.Where(candidate => candidate.Blockers.Count > 0))
        {
            dispositions[candidate.Row.Entry.FullPath] = new QuarantinePreviewEntry(
                candidate.Row,
                QuarantinePreviewDisposition.Blocked,
                DestinationPath: null,
                Reasons: candidate.Blockers);
        }

        foreach (var candidate in candidates
            .Where(candidate => candidate.Blockers.Count == 0)
            .OrderBy(candidate => candidate.Row.Entry.FullPath.Length)
            .ThenBy(candidate => candidate.Row.Entry.FullPath, StringComparer.OrdinalIgnoreCase))
        {
            var includedAncestor = includedSources.FirstOrDefault(source =>
                IsSameOrDescendant(source, candidate.Row.Entry.FullPath));
            if (includedAncestor is not null)
            {
                dispositions[candidate.Row.Entry.FullPath] = new QuarantinePreviewEntry(
                    candidate.Row,
                    QuarantinePreviewDisposition.Redundant,
                    DestinationPath: null,
                    Reasons: [$"Already covered by selected parent: {includedAncestor}"]);
                continue;
            }

            includedSources.Add(candidate.Row.Entry.FullPath);
            dispositions[candidate.Row.Entry.FullPath] = new QuarantinePreviewEntry(
                candidate.Row,
                QuarantinePreviewDisposition.Included,
                BuildDestinationPath(scope, quarantineRoot, candidate.Row.Entry.FullPath),
                Reasons: ["Eligible for quarantine preview."]);
        }

        return new QuarantinePreview(
            scope,
            quarantineRoot,
            shortlistedRows.Select(row => dispositions[row.Entry.FullPath]).ToArray());
    }

    private static IReadOnlyList<string> GetBlockers(StorageEntry entry, string cleanupScopePath)
    {
        var blockers = new List<string>();

        if (!PathSafety.IsWithinScope(cleanupScopePath, entry.FullPath))
        {
            blockers.Add("Path is outside the Cleanup Scope.");
        }

        if (!entry.IsAccessible)
        {
            blockers.Add("Path could not be fully read.");
        }

        if (entry.IsReparsePoint || entry.BloatCategories.Contains(BloatCategory.ReparsePoint))
        {
            blockers.Add("Reparse points are blocked from quarantine preview.");
        }

        if (entry.ImportanceRating == ImportanceRating.HighRisk)
        {
            blockers.Add("High-risk rows require manual review and are blocked from this preview.");
        }

        if (entry.BloatCategories.Contains(BloatCategory.ProtectedLocation))
        {
            blockers.Add("Protected locations are blocked from quarantine preview.");
        }

        if (entry.DeletionRecommendation != DeletionRecommendation.QuarantineCandidate)
        {
            blockers.Add("Only rows recommended as Quarantine candidate are included.");
        }

        if (blockers.Count == 0)
        {
            var blockedDescendants = FindBlockedDescendants(entry).ToArray();
            if (blockedDescendants.Length > 0)
            {
                blockers.Add(FormatBlockedDescendantReason(blockedDescendants));
            }
        }

        return blockers;
    }

    private static IEnumerable<StorageEntry> FindBlockedDescendants(StorageEntry entry)
    {
        foreach (var child in entry.Children)
        {
            if (IsBlockedDescendant(child))
            {
                yield return child;
            }

            foreach (var descendant in FindBlockedDescendants(child))
            {
                yield return descendant;
            }
        }
    }

    private static bool IsBlockedDescendant(StorageEntry entry)
    {
        return !entry.IsAccessible
            || entry.IsReparsePoint
            || entry.ImportanceRating == ImportanceRating.HighRisk
            || entry.BloatCategories.Contains(BloatCategory.ProtectedLocation)
            || entry.BloatCategories.Contains(BloatCategory.ReparsePoint)
            || entry.BloatCategories.Contains(BloatCategory.AccessIssue)
            || entry.BloatCategories.Contains(BloatCategory.CleanupScopeRoot);
    }

    private static string FormatBlockedDescendantReason(IReadOnlyList<StorageEntry> blockedDescendants)
    {
        var examples = blockedDescendants
            .Take(3)
            .Select(entry => entry.FullPath)
            .ToArray();
        var suffix = blockedDescendants.Count > examples.Length
            ? $" and {blockedDescendants.Count - examples.Length:N0} more"
            : "";

        return $"Contains protected, high-risk, inaccessible, or reparse-point descendant rows: {string.Join("; ", examples)}{suffix}. Select narrower reviewed child rows instead.";
    }

    private static string BuildDestinationPath(string cleanupScopePath, string quarantineRootPath, string sourcePath)
    {
        var relativePath = Path.GetRelativePath(cleanupScopePath, sourcePath);
        return Path.GetFullPath(Path.Combine(quarantineRootPath, "preview", relativePath));
    }

    private static bool IsSameOrDescendant(string parentPath, string candidatePath)
    {
        var parent = PathSafety.GetFullPath(parentPath).TrimEnd(Path.DirectorySeparatorChar);
        var candidate = PathSafety.GetFullPath(candidatePath).TrimEnd(Path.DirectorySeparatorChar);

        return candidate.Equals(parent, StringComparison.OrdinalIgnoreCase)
            || candidate.StartsWith(parent + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
    }

    private sealed record PreviewCandidate(StorageReviewEntry Row, IReadOnlyList<string> Blockers);
}
