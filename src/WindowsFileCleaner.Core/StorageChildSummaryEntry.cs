namespace WindowsFileCleaner.Core;

public sealed record StorageChildSummaryEntry(
    string Name,
    string FullPath,
    bool IsDirectory,
    long SizeBytes,
    ImportanceRating ImportanceRating,
    DeletionRecommendation DeletionRecommendation,
    IReadOnlyList<BloatCategory> BloatCategories)
{
    public string SizeDisplay => ByteSizeFormatter.Format(SizeBytes);
}

