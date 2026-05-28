namespace WindowsFileCleaner.Core;

public sealed record RestoreManifestSummary(
    string ManifestPath,
    string ActionId,
    string ActionRootPath,
    string CleanupScopePath,
    RestoreManifestActionStatus ActionStatus,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc,
    int EntryCount,
    long TotalBytes,
    int MovedCount,
    int RestoredCount,
    int FailedCount,
    int RestoreFailedCount,
    bool RequiresRecoveryReview)
{
    public string TotalSizeDisplay => ByteSizeFormatter.Format(TotalBytes);

    public bool HasUndoWork => MovedCount > 0;
}
