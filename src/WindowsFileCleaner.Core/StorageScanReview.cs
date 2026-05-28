namespace WindowsFileCleaner.Core;

public sealed record StorageScanReview(
    IReadOnlyList<StorageReviewEntry> Entries,
    StorageReviewSummary Summary)
{
    public IReadOnlyList<StorageReviewEntry> ApplyFilter(StorageReviewFilter filter)
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
