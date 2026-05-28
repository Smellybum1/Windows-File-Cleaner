namespace WindowsFileCleaner.Core;

public sealed record StorageScanReview(
    IReadOnlyList<StorageReviewEntry> Entries,
    StorageReviewSummary Summary,
    IReadOnlyList<StorageCategorySummaryEntry> CategorySummaries)
{
    public IReadOnlyList<StorageReviewEntry> ApplyFilter(StorageReviewFilter filter, BloatCategory? category = null)
    {
        return ApplyFilter(filter, category is null ? StorageCategoryFilter.All : StorageCategoryFilter.ForCategory(category.Value));
    }

    public IReadOnlyList<StorageReviewEntry> ApplyFilter(StorageReviewFilter filter, StorageCategoryFilter categoryFilter)
    {
        var filtered = ApplyReviewFilter(filter);
        return categoryFilter.Kind switch
        {
            StorageCategoryFilterKind.Category when categoryFilter.Category is not null =>
                filtered.Where(row => row.Entry.BloatCategories.Contains(categoryFilter.Category.Value)).ToArray(),
            StorageCategoryFilterKind.NoCategory =>
                filtered.Where(row => row.Entry.BloatCategories.Count == 0).ToArray(),
            _ => filtered
        };
    }

    private IReadOnlyList<StorageReviewEntry> ApplyReviewFilter(StorageReviewFilter filter)
    {
        return filter switch
        {
            StorageReviewFilter.LikelySafe => Entries.Where(row => row.Entry.ImportanceRating == ImportanceRating.LikelySafe).ToArray(),
            StorageReviewFilter.Caution => Entries.Where(row => row.Entry.ImportanceRating == ImportanceRating.Caution).ToArray(),
            StorageReviewFilter.HighRisk => Entries.Where(row => row.Entry.ImportanceRating == ImportanceRating.HighRisk).ToArray(),
            StorageReviewFilter.QuarantineCandidates => Entries.Where(row => row.Entry.DeletionRecommendation == DeletionRecommendation.QuarantineCandidate).ToArray(),
            StorageReviewFilter.AccessIssues => Entries.Where(row => IsAccessIssue(row.Entry)).ToArray(),
            _ => Entries
        };
    }

    private static bool IsAccessIssue(StorageEntry entry)
    {
        return !entry.IsAccessible || entry.BloatCategories.Contains(BloatCategory.AccessIssue);
    }
}
