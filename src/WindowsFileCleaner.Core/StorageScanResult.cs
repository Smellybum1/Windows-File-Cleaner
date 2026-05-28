namespace WindowsFileCleaner.Core;

public sealed record StorageScanResult(
    string CleanupScopePath,
    DateTimeOffset StartedAtUtc,
    DateTimeOffset CompletedAtUtc,
    StorageEntry Root)
{
    public long TotalBytes => Root.SizeBytes;
    public string TotalSizeDisplay => Root.SizeDisplay;
    public int FileCount => Root.FileCount;
    public int FolderCount => Root.FolderCount;
    public int InaccessibleCount => Root.InaccessibleCount;
}

