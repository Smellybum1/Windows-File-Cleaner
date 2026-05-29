namespace WindowsFileCleaner.Core;

public sealed record StorageHotspotTrailEntry(
    int Level,
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
