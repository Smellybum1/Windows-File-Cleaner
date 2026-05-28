namespace WindowsFileCleaner.Core;

public sealed record QuarantineActionDraft(
    string ActionId,
    DateTimeOffset DraftedAtUtc,
    string CleanupScopePath,
    string QuarantineRootPath,
    string ActionRootPath,
    string ItemsRootPath,
    string RestoreManifestPath,
    string RestoreManifestDraftId,
    IReadOnlyList<QuarantineActionEntryDraft> Entries)
{
    public int EntryCount => Entries.Count;
    public long TotalBytes => Entries.Sum(entry => entry.SizeBytes);
    public string TotalSizeDisplay => ByteSizeFormatter.Format(TotalBytes);
}
