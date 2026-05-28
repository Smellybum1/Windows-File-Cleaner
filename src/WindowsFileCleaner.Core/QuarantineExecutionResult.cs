namespace WindowsFileCleaner.Core;

public sealed record QuarantineExecutionResult(
    RestoreManifest RestoreManifest,
    IReadOnlyList<QuarantineExecutionEntryResult> Entries,
    IReadOnlyList<string> Blockers)
{
    public bool Succeeded => RestoreManifest.ActionStatus == RestoreManifestActionStatus.Completed && Blockers.Count == 0;
    public int MovedCount => Entries.Count(entry => entry.WasMoved);
    public int FailedCount => Entries.Count(entry => entry.Status == RestoreManifestEntryStatus.Failed);
    public bool HasBlockers => Blockers.Count > 0;
    public bool RequiresRecoveryReview => RestoreManifest.RequiresRecoveryReview || HasBlockers;
}
