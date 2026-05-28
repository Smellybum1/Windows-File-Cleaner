namespace WindowsFileCleaner.Core;

public sealed record StorageEntry(
    string FullPath,
    string Name,
    bool IsDirectory,
    long SizeBytes,
    DateTimeOffset? LastModifiedUtc,
    bool IsAccessible,
    bool IsReparsePoint,
    string? ErrorMessage,
    IReadOnlyList<BloatCategory> BloatCategories,
    ImportanceRating ImportanceRating,
    DeletionRecommendation DeletionRecommendation,
    string Evidence,
    IReadOnlyList<StorageEntry> Children)
{
    public string SizeDisplay => ByteSizeFormatter.Format(SizeBytes);
    public int FileCount => IsDirectory ? Children.Sum(child => child.FileCount) : 1;
    public int FolderCount => IsDirectory ? 1 + Children.Sum(child => child.FolderCount) : 0;
    public int InaccessibleCount => (IsAccessible ? 0 : 1) + Children.Sum(child => child.InaccessibleCount);
}

