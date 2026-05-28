namespace WindowsFileCleaner.Core;

public sealed record RestoreManifestDraft(
    string SchemaVersion,
    string DraftId,
    DateTimeOffset DraftedAtUtc,
    string CleanupScopePath,
    string QuarantineRootPath,
    IReadOnlyList<RestoreManifestEntryDraft> Entries)
{
    public const string CurrentSchemaVersion = "restore-manifest.v1";

    public int EntryCount => Entries.Count;
    public long TotalBytes => Entries.Sum(entry => entry.SizeBytes);
    public string TotalSizeDisplay => ByteSizeFormatter.Format(TotalBytes);
    public bool IsExecutedManifest => false;
}
