namespace WindowsFileCleaner.Core;

public static class StorageScanReviewBuilder
{
    public static StorageScanReview Build(StorageScanResult result)
    {
        var entries = Flatten(result.Root)
            .OrderByDescending(row => row.Entry.SizeBytes)
            .ThenBy(row => row.Entry.FullPath, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return new StorageScanReview(entries, BuildSummary(entries));
    }

    private static StorageReviewSummary BuildSummary(IReadOnlyList<StorageReviewEntry> entries)
    {
        return new StorageReviewSummary(
            entries.Count,
            entries.Count(row => row.Entry.ImportanceRating == ImportanceRating.LikelySafe),
            entries.Count(row => row.Entry.ImportanceRating == ImportanceRating.Caution),
            entries.Count(row => row.Entry.ImportanceRating == ImportanceRating.HighRisk),
            entries.Count(row => row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate),
            entries.Where(row => row.Entry.ImportanceRating == ImportanceRating.LikelySafe).Sum(row => row.Entry.SizeBytes),
            entries.Where(row => row.Entry.ImportanceRating == ImportanceRating.Caution).Sum(row => row.Entry.SizeBytes),
            entries.Where(row => row.Entry.ImportanceRating == ImportanceRating.HighRisk).Sum(row => row.Entry.SizeBytes),
            entries.Where(row => row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate).Sum(row => row.Entry.SizeBytes));
    }

    private static IEnumerable<StorageReviewEntry> Flatten(StorageEntry root)
    {
        return Flatten(root, depth: 0);
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

