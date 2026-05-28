namespace WindowsFileCleaner.Core;

public sealed record QuarantinePreviewEntry(
    StorageReviewEntry ReviewEntry,
    QuarantinePreviewDisposition Disposition,
    string? DestinationPath,
    IReadOnlyList<string> Reasons)
{
    public StorageEntry Entry => ReviewEntry.Entry;
    public string SourcePath => Entry.FullPath;
    public long SizeBytes => Entry.SizeBytes;
    public string SizeDisplay => Entry.SizeDisplay;
    public bool IsIncluded => Disposition == QuarantinePreviewDisposition.Included;
}
