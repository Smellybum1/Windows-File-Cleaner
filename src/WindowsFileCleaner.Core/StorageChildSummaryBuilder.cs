namespace WindowsFileCleaner.Core;

public static class StorageChildSummaryBuilder
{
    public static IReadOnlyList<StorageChildSummaryEntry> Build(StorageEntry entry, int maxChildren = 12)
    {
        if (maxChildren <= 0 || entry.Children.Count == 0)
        {
            return [];
        }

        return entry.Children
            .OrderByDescending(child => child.SizeBytes)
            .ThenBy(child => child.Name, StringComparer.OrdinalIgnoreCase)
            .Take(maxChildren)
            .Select(child => new StorageChildSummaryEntry(
                child.Name,
                child.FullPath,
                child.IsDirectory,
                child.SizeBytes,
                child.ImportanceRating,
                child.DeletionRecommendation,
                child.BloatCategories))
            .ToArray();
    }
}

