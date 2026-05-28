namespace WindowsFileCleaner.Core;

public sealed record UndoQuarantineResult(
    RestoreManifest RestoreManifest,
    IReadOnlyList<UndoQuarantineEntryResult> Entries,
    IReadOnlyList<string> Blockers)
{
    public bool Succeeded => RestoreManifest.ActionStatus == RestoreManifestActionStatus.Restored && Blockers.Count == 0;
    public int RestoredCount => Entries.Count(entry => entry.WasRestored);
    public int FailedCount => Entries.Count(entry => entry.Status == RestoreManifestEntryStatus.RestoreFailed);
    public bool HasBlockers => Blockers.Count > 0;
    public bool RequiresRecoveryReview => RestoreManifest.RequiresRecoveryReview || HasBlockers;
}
