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
        return ApplyFilter(filter, categoryFilter, StorageEntryTypeFilter.All, search);
    }

    public IReadOnlyList<StorageReviewEntry> ApplyFilter(
        StorageReviewFilter filter,
        StorageCategoryFilter categoryFilter,
        StorageEntryTypeFilter typeFilter,
        StorageReviewSearch search)
    {
        return ApplyFilter(filter, categoryFilter, typeFilter, StorageSizeThresholdFilter.All, search);
    }

    public IReadOnlyList<StorageReviewEntry> ApplyFilter(
        StorageReviewFilter filter,
        StorageCategoryFilter categoryFilter,
        StorageEntryTypeFilter typeFilter,
        StorageSizeThresholdFilter sizeThresholdFilter,
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

        filtered = typeFilter switch
        {
            StorageEntryTypeFilter.Files => filtered.Where(row => !row.Entry.IsDirectory).ToArray(),
            StorageEntryTypeFilter.Folders => filtered.Where(row => row.Entry.IsDirectory).ToArray(),
            _ => filtered
        };

        var minimumSizeBytes = sizeThresholdFilter.GetMinimumSizeBytes();
        if (minimumSizeBytes > 0)
        {
            filtered = filtered.Where(row => row.Entry.SizeBytes >= minimumSizeBytes).ToArray();
        }

        return search.IsActive
            ? filtered.Where(row => MatchesSearch(row.Entry, search)).ToArray()
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

    private static bool MatchesSearch(StorageEntry entry, StorageReviewSearch search)
    {
        var query = search.Term;
        var normalizedQuery = NormalizeForSearch(query);
        return search.Field switch
        {
            StorageReviewSearchField.Path => ContainsSearchText(entry.FullPath, query, normalizedQuery),
            StorageReviewSearchField.Parent => ContainsSearchText(GetParentPath(entry.FullPath), query, normalizedQuery),
            StorageReviewSearchField.Under => IsUnderPath(query, entry.FullPath),
            StorageReviewSearchField.Name => ContainsSearchText(entry.Name, query, normalizedQuery),
            StorageReviewSearchField.Category => MatchesCategorySearch(entry, query, normalizedQuery),
            StorageReviewSearchField.Rating => ContainsSearchText(entry.ImportanceRating.ToString(), query, normalizedQuery),
            StorageReviewSearchField.Recommendation => ContainsSearchText(entry.DeletionRecommendation.ToString(), query, normalizedQuery),
            StorageReviewSearchField.Evidence => ContainsSearchText(entry.Evidence, query, normalizedQuery),
            StorageReviewSearchField.AccessIssue => MatchesAccessIssueSearch(entry, query, normalizedQuery),
            _ =>
                ContainsSearchText(entry.Name, query, normalizedQuery)
                || ContainsSearchText(entry.FullPath, query, normalizedQuery)
                || ContainsSearchText(GetParentPath(entry.FullPath), query, normalizedQuery)
                || ContainsSearchText(entry.Evidence, query, normalizedQuery)
                || MatchesAccessIssueSearch(entry, query, normalizedQuery)
                || ContainsSearchText(entry.ImportanceRating.ToString(), query, normalizedQuery)
                || ContainsSearchText(entry.DeletionRecommendation.ToString(), query, normalizedQuery)
                || MatchesCategorySearch(entry, query, normalizedQuery)
        };
    }

    private static bool IsUnderPath(string ancestorPath, string candidatePath)
    {
        try
        {
            var ancestor = PathSafety.GetFullPath(ancestorPath);
            var candidate = PathSafety.GetFullPath(candidatePath);
            return !candidate.Equals(ancestor, StringComparison.OrdinalIgnoreCase)
                && PathSafety.IsWithinScope(ancestor, candidate);
        }
        catch (ArgumentException)
        {
            return false;
        }
        catch (NotSupportedException)
        {
            return false;
        }
    }

    private static string? GetParentPath(string fullPath)
    {
        try
        {
            return Path.GetDirectoryName(fullPath);
        }
        catch (ArgumentException)
        {
            return null;
        }
        catch (NotSupportedException)
        {
            return null;
        }
    }

    private static bool MatchesCategorySearch(StorageEntry entry, string query, string normalizedQuery)
    {
        return entry.BloatCategories.Any(category => ContainsSearchText(category.ToString(), query, normalizedQuery));
    }

    private static bool MatchesAccessIssueSearch(StorageEntry entry, string query, string normalizedQuery)
    {
        return ContainsSearchText(FormatAccessStatus(entry), query, normalizedQuery)
            || ContainsSearchText(entry.ErrorMessage, query, normalizedQuery)
            || (IsAccessIssue(entry) && ContainsSearchText("Access issue", query, normalizedQuery));
    }

    private static string FormatAccessStatus(StorageEntry entry)
    {
        return entry.IsAccessible ? "Readable" : "Access issue";
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
