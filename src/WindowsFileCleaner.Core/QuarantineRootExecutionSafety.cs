namespace WindowsFileCleaner.Core;

public sealed record QuarantineRootExecutionSafety(
    string CleanupScopePath,
    string QuarantineRootPath,
    string ActionRootPath,
    string ItemsRootPath,
    string RestoreManifestPath,
    long PlannedMoveBytes,
    long ManifestOverheadBytes,
    long RequiredBytes,
    long? AvailableFreeBytes,
    bool IsFullyQualifiedQuarantineRoot,
    bool IsPreferredQuarantineRoot,
    bool IsNonPreferredQuarantineRootAcknowledged,
    bool HasCapacityEvidence,
    bool HasEnoughFreeSpace,
    IReadOnlyList<string> Blockers,
    IReadOnlyList<string> ReviewNotes)
{
    public const long DefaultManifestOverheadBytes = 64 * 1024;

    public bool HasBlockers => Blockers.Count > 0;
    public bool CanUseForExecution => !HasBlockers;
    public string RequiredSizeDisplay => ByteSizeFormatter.Format(RequiredBytes);
    public string? AvailableFreeSizeDisplay => AvailableFreeBytes is null
        ? null
        : ByteSizeFormatter.Format(AvailableFreeBytes.Value);
}
