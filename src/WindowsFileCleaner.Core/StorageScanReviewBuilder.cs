namespace WindowsFileCleaner.Core;

public static class StorageScanReviewBuilder
{
    public static StorageScanReview Build(StorageScanResult result)
    {
        var entries = Flatten(result.Root)
            .OrderByDescending(row => row.Entry.SizeBytes)
            .ThenBy(row => row.Entry.FullPath, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return new StorageScanReview(entries, BuildSummary(entries), BuildCategorySummaries(entries));
    }

    private static StorageReviewSummary BuildSummary(IReadOnlyList<StorageReviewEntry> entries)
    {
        return new StorageReviewSummary(
            entries.Count,
            entries.Count(row => row.Entry.ImportanceRating == ImportanceRating.LikelySafe),
            entries.Count(row => row.Entry.ImportanceRating == ImportanceRating.Caution),
            entries.Count(row => row.Entry.ImportanceRating == ImportanceRating.HighRisk),
            entries.Count(row => row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate),
            entries.Count(row => IsAccessIssue(row.Entry)),
            Largest(entries),
            Largest(entries.Where(row => row.Entry.ImportanceRating == ImportanceRating.LikelySafe)),
            Largest(entries.Where(row => row.Entry.ImportanceRating == ImportanceRating.Caution)),
            Largest(entries.Where(row => row.Entry.ImportanceRating == ImportanceRating.HighRisk)),
            Largest(entries.Where(row => row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate)),
            Largest(entries.Where(row => IsAccessIssue(row.Entry))));
    }

    private static long Largest(IEnumerable<StorageReviewEntry> entries)
    {
        return entries
            .Select(row => row.Entry.SizeBytes)
            .DefaultIfEmpty(0)
            .Max();
    }

    private static IReadOnlyList<StorageCategorySummaryEntry> BuildCategorySummaries(IReadOnlyList<StorageReviewEntry> entries)
    {
        return entries
            .SelectMany(row => row.Entry.BloatCategories.Select(category => new { category, row }))
            .GroupBy(pair => pair.category)
            .Select(group => new StorageCategorySummaryEntry(
                group.Key,
                group.Count(),
                Largest(group.Select(pair => pair.row))))
            .OrderByDescending(summary => summary.Count)
            .ThenByDescending(summary => summary.LargestEntryBytes)
            .ThenBy(summary => summary.Category.ToString(), StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static IEnumerable<StorageReviewEntry> Flatten(StorageEntry root)
    {
        return Flatten(root, depth: 0);
    }

    private static bool IsAccessIssue(StorageEntry entry)
    {
        return !entry.IsAccessible || entry.BloatCategories.Contains(BloatCategory.AccessIssue);
    }

    private static IEnumerable<StorageReviewEntry> Flatten(StorageEntry entry, int depth)
    {
        yield return new StorageReviewEntry(entry, depth);

        foreach (var child in entry.Children.SelectMany(child => Flatten(child, depth + 1)))
        {
            yield return child;
        }
    }
}
