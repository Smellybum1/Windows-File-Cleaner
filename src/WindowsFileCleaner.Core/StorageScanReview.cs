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
        return ApplyFilter(filter, categoryFilter, StorageReviewSearch.Empty);
    }

    public IReadOnlyList<StorageReviewEntry> ApplyFilter(
        StorageReviewFilter filter,
        StorageCategoryFilter categoryFilter,
        StorageReviewSearch search)
    {
        var filtered = ApplyReviewFilter(filter);
        filtered = categoryFilter.Kind switch
        {
            StorageCategoryFilterKind.Category when categoryFilter.Category is not null =>
                filtered.Where(row => row.Entry.BloatCategories.Contains(categoryFilter.Category.Value)).ToArray(),
            StorageCategoryFilterKind.NoCategory =>
                filtered.Where(row => row.Entry.BloatCategories.Count == 0).ToArray(),
            _ => filtered
        };

        return search.IsActive
            ? filtered.Where(row => MatchesSearch(row.Entry, search.Query)).ToArray()
            : filtered;
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

    private static bool MatchesSearch(StorageEntry entry, string query)
    {
        var normalizedQuery = NormalizeForSearch(query);
        return ContainsSearchText(entry.Name, query, normalizedQuery)
            || ContainsSearchText(entry.FullPath, query, normalizedQuery)
            || ContainsSearchText(entry.Evidence, query, normalizedQuery)
            || ContainsSearchText(entry.ErrorMessage, query, normalizedQuery)
            || ContainsSearchText(entry.ImportanceRating.ToString(), query, normalizedQuery)
            || ContainsSearchText(entry.DeletionRecommendation.ToString(), query, normalizedQuery)
            || entry.BloatCategories.Any(category => ContainsSearchText(category.ToString(), query, normalizedQuery));
    }

    private static bool ContainsSearchText(string? value, string query, string normalizedQuery)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        return value.Contains(query, StringComparison.OrdinalIgnoreCase)
            || NormalizeForSearch(value).Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeForSearch(string value)
    {
        return new string(value.Where(character =>
            !char.IsWhiteSpace(character)
            && character != '-'
            && character != '_').ToArray());
    }
}
