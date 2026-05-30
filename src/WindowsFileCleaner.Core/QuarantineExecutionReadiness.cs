namespace WindowsFileCleaner.Core;

public sealed record QuarantineExecutionReadiness(
    string CleanupScopePath,
    string QuarantineRootPath,
    QuarantineExecutionReadinessScopeKind ScopeKind,
    QuarantineExecutionReadinessDisposition Disposition,
    string RequiredConfirmationText,
    int IncludedCount,
    long IncludedBytes,
    int RealProfileIncludedRowLimit,
    long RealProfileIncludedByteLimit,
    bool IsPreferredQuarantineRoot,
    bool IsNonPreferredQuarantineRootAcknowledged,
    bool AllowsNarrowFolders,
    bool RequiresSelectedManifestRealProfileUndoBeforeForwardQuarantine,
    bool RequiresManualRescanAfterExecution,
    bool UsesRestoreManifestOnly,
    IReadOnlyList<string> Blockers,
    IReadOnlyList<string> ReviewNotes)
{
    public const int DefaultRealProfileIncludedRowLimit = 10;
    public const long DefaultRealProfileIncludedByteLimit = 1024L * 1024L * 1024L;

    public bool HasBlockers => Blockers.Count > 0;
    public bool CanExecuteInCurrentBuild => Disposition == QuarantineExecutionReadinessDisposition.FixtureExecutable && !HasBlockers;
    public string IncludedSizeDisplay => ByteSizeFormatter.Format(IncludedBytes);
    public string RealProfileIncludedByteLimitDisplay => ByteSizeFormatter.Format(RealProfileIncludedByteLimit);
}
