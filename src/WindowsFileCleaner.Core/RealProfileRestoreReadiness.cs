namespace WindowsFileCleaner.Core;

public sealed record RealProfileRestoreReadiness(
    DateTimeOffset CheckedAtUtc,
    string QuarantineRootPath,
    string ActionsRootPath,
    string? SelectedManifestPath,
    string ActionId,
    string CleanupScopePath,
    bool IsSelectedManifestRealProfileScope,
    bool IsSelectedManifestRealProfileUndoImplemented,
    bool UsesRestoreManifestOnly,
    int RestorableEntryCount,
    long RestorableBytes,
    int BlockedEntryCount,
    int RecoveryReviewEntryCount,
    int AlreadyRestoredEntryCount,
    int NotMovedEntryCount,
    IReadOnlyList<string> Blockers,
    IReadOnlyList<string> ReviewNotes)
{
    public bool HasBlockers => Blockers.Count > 0;
    public bool CanUseForForwardQuarantine => IsSelectedManifestRealProfileUndoImplemented && !HasBlockers;
    public string RestorableSizeDisplay => ByteSizeFormatter.Format(RestorableBytes);
}
