namespace WindowsFileCleaner.Core;

public sealed record QuarantinePreview(
    string CleanupScopePath,
    string QuarantineRootPath,
    IReadOnlyList<QuarantinePreviewEntry> Entries)
{
    public int IncludedCount => Entries.Count(entry => entry.Disposition == QuarantinePreviewDisposition.Included);
    public int BlockedCount => Entries.Count(entry => entry.Disposition == QuarantinePreviewDisposition.Blocked);
    public int RedundantCount => Entries.Count(entry => entry.Disposition == QuarantinePreviewDisposition.Redundant);
    public long IncludedBytes => Entries
        .Where(entry => entry.Disposition == QuarantinePreviewDisposition.Included)
        .Sum(entry => entry.SizeBytes);
    public string IncludedSizeDisplay => ByteSizeFormatter.Format(IncludedBytes);
}
