namespace WindowsFileCleaner.Core;

public sealed record RestoreManifest(
    string SchemaVersion,
    string ManifestId,
    string RestoreManifestDraftId,
    string ActionId,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc,
    string CleanupScopePath,
    string QuarantineRootPath,
    string ActionRootPath,
    string ItemsRootPath,
    string ManifestPath,
    RestoreManifestActionStatus ActionStatus,
    IReadOnlyList<RestoreManifestEntry> Entries,
    IReadOnlyList<string> WriteOrderNotes)
{
    public const string CurrentSchemaVersion = RestoreManifestDraft.CurrentSchemaVersion;

    public int EntryCount => Entries.Count;
    public long TotalBytes => Entries.Sum(entry => entry.SizeBytes);
    public string TotalSizeDisplay => ByteSizeFormatter.Format(TotalBytes);
    public int PlannedCount => Entries.Count(entry => entry.Status == RestoreManifestEntryStatus.Planned);
    public int MovingCount => Entries.Count(entry => entry.Status == RestoreManifestEntryStatus.Moving);
    public int MovedCount => Entries.Count(entry => entry.Status == RestoreManifestEntryStatus.Moved);
    public int FailedCount => Entries.Count(entry => entry.Status == RestoreManifestEntryStatus.Failed);
    public int RestoringCount => Entries.Count(entry => entry.Status == RestoreManifestEntryStatus.Restoring);
    public int RestoredCount => Entries.Count(entry => entry.Status == RestoreManifestEntryStatus.Restored);
    public int RestoreFailedCount => Entries.Count(entry => entry.Status == RestoreManifestEntryStatus.RestoreFailed);
    public bool IsExecutedManifest => true;
    public bool RequiresRecoveryReview =>
        MovingCount > 0
        || FailedCount > 0
        || RestoringCount > 0
        || RestoreFailedCount > 0
        || ActionStatus is RestoreManifestActionStatus.PartialFailure
            or RestoreManifestActionStatus.Failed
            or RestoreManifestActionStatus.RestorePartialFailure
            or RestoreManifestActionStatus.RestoreFailed;
}
