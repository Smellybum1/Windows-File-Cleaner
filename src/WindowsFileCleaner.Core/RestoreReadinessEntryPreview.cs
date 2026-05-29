namespace WindowsFileCleaner.Core;

public sealed record RestoreReadinessEntryPreview(
    string OriginalPath,
    string QuarantinePath,
    string RelativePath,
    bool IsDirectory,
    long SizeBytes,
    RestoreManifestEntryStatus CurrentStatus,
    RestoreReadinessDisposition Disposition,
    IReadOnlyList<string> Blockers)
{
    public string SizeDisplay => ByteSizeFormatter.Format(SizeBytes);

    public bool CanRestore => Disposition == RestoreReadinessDisposition.Restorable;
}
