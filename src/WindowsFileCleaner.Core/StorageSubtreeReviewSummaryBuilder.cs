namespace WindowsFileCleaner.Core;

public static class StorageSubtreeReviewSummaryBuilder
{
    private const int MaxExamples = 3;

    public static StorageSubtreeReviewSummary Build(StorageEntry entry, string? cleanupScopePath = null)
    {
        var descendants = FlattenDescendants(entry).ToArray();
        return new StorageSubtreeReviewSummary(
            descendants.Length,
            descendants.Count(descendant => !descendant.IsDirectory),
            descendants.Count(descendant => descendant.IsDirectory),
            descendants.Count(descendant => descendant.ImportanceRating == ImportanceRating.LikelySafe),
            descendants.Count(descendant => descendant.ImportanceRating == ImportanceRating.Caution),
            descendants.Count(descendant => descendant.ImportanceRating == ImportanceRating.HighRisk),
            descendants.Count(descendant => descendant.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate),
            descendants.Count(descendant => descendant.BloatCategories.Contains(BloatCategory.ProtectedLocation)),
            descendants.Count(IsAccessIssue),
            descendants.Count(IsReparsePoint),
            descendants.Count(descendant => descendant.BloatCategories.Count == 0),
            descendants.Select(descendant => descendant.SizeBytes).DefaultIfEmpty(0).Max(),
            BuildExamples(descendants, cleanupScopePath, descendant => descendant.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate),
            BuildExamples(descendants, cleanupScopePath, descendant => descendant.BloatCategories.Contains(BloatCategory.ProtectedLocation)),
            BuildExamples(descendants, cleanupScopePath, IsAccessIssue),
            BuildExamples(descendants, cleanupScopePath, descendant => descendant.BloatCategories.Count == 0));
    }

    private static IEnumerable<StorageEntry> FlattenDescendants(StorageEntry entry)
    {
        foreach (var child in entry.Children)
        {
            yield return child;

            foreach (var descendant in FlattenDescendants(child))
            {
                yield return descendant;
            }
        }
    }

    private static IReadOnlyList<string> BuildExamples(
        IReadOnlyList<StorageEntry> descendants,
        string? cleanupScopePath,
        Func<StorageEntry, bool> predicate)
    {
        return descendants
            .Where(predicate)
            .OrderByDescending(descendant => descendant.SizeBytes)
            .ThenBy(descendant => descendant.FullPath, StringComparer.OrdinalIgnoreCase)
            .Take(MaxExamples)
            .Select(descendant => $"{FormatPath(cleanupScopePath, descendant.FullPath)} ({descendant.SizeDisplay})")
            .ToArray();
    }

    private static string FormatPath(string? cleanupScopePath, string fullPath)
    {
        if (string.IsNullOrWhiteSpace(cleanupScopePath))
        {
            return fullPath;
        }

        try
        {
            return PathSafety.IsWithinScope(cleanupScopePath, fullPath)
                ? Path.GetRelativePath(PathSafety.GetFullPath(cleanupScopePath), fullPath)
                : fullPath;
        }
        catch (ArgumentException)
        {
            return fullPath;
        }
        catch (NotSupportedException)
        {
            return fullPath;
        }
    }

    private static bool IsAccessIssue(StorageEntry entry)
    {
        return !entry.IsAccessible || entry.BloatCategories.Contains(BloatCategory.AccessIssue);
    }

    private static bool IsReparsePoint(StorageEntry entry)
    {
        return entry.IsReparsePoint || entry.BloatCategories.Contains(BloatCategory.ReparsePoint);
    }
}
