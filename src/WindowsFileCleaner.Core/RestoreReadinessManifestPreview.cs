namespace WindowsFileCleaner.Core;

public sealed record RestoreReadinessManifestPreview(
    string ManifestPath,
    string ActionId,
    RestoreManifestActionStatus ActionStatus,
    DateTimeOffset UpdatedAtUtc,
    int EntryCount,
    long TotalBytes,
    bool RequiresRecoveryReview,
    IReadOnlyList<string> Blockers,
    IReadOnlyList<RestoreReadinessEntryPreview> Entries)
{
    public string TotalSizeDisplay => ByteSizeFormatter.Format(TotalBytes);

    public int RestorableCount => Entries.Count(entry => entry.Disposition == RestoreReadinessDisposition.Restorable);

    public int BlockedCount => Entries.Count(entry => entry.Disposition == RestoreReadinessDisposition.Blocked);

    public int RecoveryReviewCount => Entries.Count(entry => entry.Disposition == RestoreReadinessDisposition.NeedsRecoveryReview);

    public int AlreadyRestoredCount => Entries.Count(entry => entry.Disposition == RestoreReadinessDisposition.AlreadyRestored);

    public int NotMovedCount => Entries.Count(entry => entry.Disposition == RestoreReadinessDisposition.NotMoved);

    public bool HasRestorableEntries => RestorableCount > 0;
}
