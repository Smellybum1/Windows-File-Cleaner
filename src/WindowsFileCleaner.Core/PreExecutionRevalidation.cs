namespace WindowsFileCleaner.Core;

public sealed record PreExecutionRevalidation(
    DateTimeOffset RevalidatedAtUtc,
    string CleanupScopePath,
    string QuarantineRootPath,
    string ActionRootPath,
    string ItemsRootPath,
    string RestoreManifestPath,
    int IncludedCount,
    long IncludedBytes,
    bool IsRootExecutionSafetyClean,
    IReadOnlyList<string> Blockers,
    IReadOnlyList<string> ReviewNotes)
{
    public bool HasBlockers => Blockers.Count > 0;
    public bool CanProceed => !HasBlockers;
    public string IncludedSizeDisplay => ByteSizeFormatter.Format(IncludedBytes);
}
